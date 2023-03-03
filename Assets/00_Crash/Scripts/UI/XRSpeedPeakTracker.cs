using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using ObliqueSenastions.VRRigSpace;

namespace ObliqueSenastions.UISpace
{

    public class XRSpeedPeakTracker : MonoBehaviour
    {




        float veloTheshold = 0.1f;

        bool active;
        [SerializeField] float resetTimer = 0f;
        [SerializeField] float resetAfterStillSeconds = 3f;
        bool timerResetted = false;

        [SerializeField] PeakTracker[] peakTrackers;

        [System.Serializable]
        struct PeakTracker
        {
            public XRVelocityTracker velocityTracker;

            [Tooltip("doesnt need XR Device, is based on simple Vector calculations of the transform")]
            public bool useSimpleVelocityTracker;
            public SimpleVelocityTracker simpleVelocityTracker;
            public Tachonadel tachonadel;
            public UnityEvent unityEvent;
            public float currentSpeed;
            public float speedThreshold;
            public float speadPeak;
            public float speadMin;
            public float speedMax;
            public bool thresholdBroken;
            public float time;
            public float stayTimeAtPeak;
            public float resetTime;
            public float resetAfterStillSeconds;


        }

        void Start()
        {

        }


        void Update()
        {

            for (int i = 0; i < peakTrackers.Length; i++)
            {
                if (peakTrackers[i].useSimpleVelocityTracker)
                {
                    peakTrackers[i].currentSpeed = peakTrackers[i].simpleVelocityTracker.GetSpeed();
                }
                else
                {
                    peakTrackers[i].currentSpeed = peakTrackers[i].velocityTracker.GetSpeed();
                }

                peakTrackers[i].time += Time.deltaTime;

                if (peakTrackers[i].currentSpeed > veloTheshold)
                {
                    // print(">velo");
                    if (!(peakTrackers[i].time > peakTrackers[i].stayTimeAtPeak)) return;

                    // print("> stay");

                    if (peakTrackers[i].thresholdBroken) return;

                    //   print("broken threshold");

                    StartCoroutine(CalculateSpeedPeak(peakTrackers[i]));


                }

                else
                {
                    peakTrackers[i].thresholdBroken = false;
                    peakTrackers[i].resetTime += Time.deltaTime;

                }
            }

            for (int i = 0; i < peakTrackers.Length; i++)
            {
                if (peakTrackers[i].resetTime > peakTrackers[i].resetAfterStillSeconds)
                {
                    //   print("reset");
                    peakTrackers[i].tachonadel.SetTargetPositionNorm(0f);
                    peakTrackers[i].resetTime = 0f;
                }
            }






        }

        IEnumerator CalculateSpeedPeak(PeakTracker peakTracker)
        {
            peakTracker.thresholdBroken = true;
            // print("calculate");
            float previousSpeed = 0f;

            float currentSpeed;

            if (peakTracker.useSimpleVelocityTracker)
            {
                currentSpeed = peakTracker.simpleVelocityTracker.GetSpeed();
            }

            else
            {
                currentSpeed = peakTracker.velocityTracker.GetSpeed();
            }


            while (previousSpeed < currentSpeed)
            {
                previousSpeed = currentSpeed;
                yield return new WaitForSeconds(Time.deltaTime);
                // print("while");
                if (peakTracker.useSimpleVelocityTracker)
                {
                    currentSpeed = peakTracker.simpleVelocityTracker.GetSpeed();
                }
                else
                {
                    currentSpeed = peakTracker.velocityTracker.GetSpeed();
                }

            }


            peakTracker.time = 0f;
            peakTracker.speadPeak = previousSpeed;




            peakTracker.unityEvent.Invoke();
            peakTracker.tachonadel.SetTargetPositionNorm(Mathf.InverseLerp(peakTracker.speadMin, peakTracker.speedMax, peakTracker.speadPeak));



            peakTracker.thresholdBroken = false;
            // print("broken threshold= "+ peakTracker.thresholdBroken);


            yield break;
        }




    }



}
