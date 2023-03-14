using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.VRRigSpace;
using UnityEngine;

namespace ObliqueSenastions.UISpace
{

    public class Tachonadel : MonoBehaviour
    {
        enum Tachomode
        {
            readSpeedPeakTracker,
            readAvarageVelocity,

            readNothing,
        }

        [SerializeField] Tachomode tachomode = Tachomode.readSpeedPeakTracker;

        [SerializeField] AverageHandSpeedMapper averageHandSpeedTracker = null;

        [SerializeField] XRSpeedPeakTrackerSolo speedPeakTracker = null;
        [SerializeField] float positionsMin = 0f;
        [SerializeField] float poitionsMax = 12f;

        [SerializeField] float targetPosition;


        [SerializeField] float rotMin = -90f;
        [SerializeField] float rotMax = 90f;

        [SerializeField] HingeJoint joint;

        [SerializeField] float smoothing = 0.1f;





        JointSpring spring;

        float mappedRotation;

        void Start()
        {
            if (joint == null)
            {
                joint = GetComponent<HingeJoint>();
            }

            spring = joint.spring;
        }


        void Update()
        {
            spring.targetPosition = Mathf.Lerp(spring.targetPosition, MappedRotation(targetPosition),  smoothing) ;
            //print("joint position" + spring.targetPosition);
            joint.spring = spring;

            if(tachomode == Tachomode.readSpeedPeakTracker)
            {
                if(speedPeakTracker == null) return;
                SetTargetPositionNorm(speedPeakTracker.GetCurrentValue());

            }

            else if(tachomode == Tachomode.readAvarageVelocity)
            {
                if(averageHandSpeedTracker == null) return;
                SetTargetPositionNorm(averageHandSpeedTracker.GetAverageSpeed(true));
            }


        }

        public void SetTargetPosition(float position)
        {
            targetPosition = position;
        }

        private void SetTargetPositionNorm(float positionNorm) // TODO Change to private (Gets Set By XR Speed Peak Tracker)
        {
            //print("Tachonadel: Set Target Position " + positionNorm);
            targetPosition = Mathf.Lerp(positionsMin, poitionsMax, positionNorm);

        }

        public float GetTargetPosition() // Gets Read by Blend Shape Control
        {
            return targetPosition;
        }

        public float GetTargetPositionNorm()
        {
//            print("Tachonadel: get Target Position Norm. " + this.gameObject + "   " + targetPosition + "   " + Mathf.InverseLerp(positionsMin, poitionsMax, targetPosition));
            return Mathf.InverseLerp(positionsMin, poitionsMax, targetPosition);
        }





        private float MappedRotation(float position)
        {
            float normalizedPosition = Mathf.InverseLerp(positionsMin, poitionsMax, position);
            return Mathf.Lerp(rotMin, rotMax, normalizedPosition);
        }




    }

}
