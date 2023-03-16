using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.VRRigSpace;
using UnityEngine;

namespace ObliqueSenastions.ClapSpace
{
    public class ClapHandler : MonoBehaviour
    {
        [SerializeField] ClapDetector clapDetectorLeft;
        [SerializeField] SimpleVelocityTracker velocityTrackerLeft;

        [SerializeField] OVRHand ovrHandLeft = null;

        [SerializeField] OVRHand ovrHandright = null;

        [SerializeField] ClapDetector clapDetectorRight;
        [SerializeField] SimpleVelocityTracker velocityTrackerRight;

        //[SerializeField] ClapDetectorMic clapDetectorMic = null;

        [SerializeField] float timeBetweenClaps = 0.2f;

        [Tooltip("map collider clap strength")]
        [SerializeField] float mapStrengthMin = 0f;
        [SerializeField] float mapStrengthMax = 0.7f;




        public delegate void DoOnColliderClap(float clapStrength);
        public DoOnColliderClap doOnColliderClap;

        // public delegate void DoOnClapHeared(float strength);
        // public DoOnClapHeared doOnClapHeared;

        bool isClapping = false;



        void Start()
        {
            if (clapDetectorLeft != null)
            {
                clapDetectorLeft.clap += OnClapDetected;
                doOnColliderClap += PlaceholderClapCollision;
            }


            // if (clapDetectorMic != null)
            // {
            //     clapDetectorMic.clapHeared += OnClapHeared;
            //     doOnClapHeared += PlaceholderClapHeared;
            // }


        }


        //Collision Clap Handling

        void OnClapDetected()
        {

            if (isClapping) return;

            if (!TrackingIsGood()) return;


            StartCoroutine(OneClapRoutine());
        }

        private bool TrackingIsGood()
        {


            if (ovrHandLeft.IsDataHighConfidence && ovrHandright.IsDataHighConfidence)
            {
                return true;
            }

            else
            {
                return false;
            }





        }

        IEnumerator OneClapRoutine()
        {

            isClapping = true;
            float strength = velocityTrackerLeft.GetLocalSpeed() + velocityTrackerRight.GetLocalSpeed();
            doOnColliderClap(NormalizeStrength(strength));
            yield return new WaitForSecondsRealtime(timeBetweenClaps);
            isClapping = false;
            yield break;
        }

        float NormalizeStrength(float startLoudness)
        {
            float loudnessNorm = Mathf.InverseLerp(mapStrengthMin, mapStrengthMax, startLoudness);
            return loudnessNorm;
        }

        void PlaceholderClapCollision(float clapStrength)
        {
            // print("clap with: " + clapStrength);
            return;
        }



        // Acoustic Clap Handling

        // void OnClapHeared(float strength)
        // {

        //     doOnClapHeared(strength);
        // }


        // void PlaceholderClapHeared(float strength)
        // {
        //     // print("clap heared with: " + strength);
        //     return;
        // }






    }


}
