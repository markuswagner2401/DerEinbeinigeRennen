using System.Collections;
using System.Collections.Generic;

using ObliqueSenastions.VRRigSpace;
using ObliqueSenastions.ClapSpace;
using UnityEngine;
using TMPro;

namespace ObliqueSenastions.UISpace
{



    public class Tachonadel : MonoBehaviour
    {

        enum Tachomode
        {
            readSpeedPeakTracker,
            readAvarageHandSpeed,

            readVelocityChangeDetector,

            syncedByNetwork,
            readNothing
        }


        [SerializeField] Tachomode tachomode = Tachomode.readSpeedPeakTracker;

        [SerializeField] AverageHandSpeedMapper averageHandSpeedTracker = null;

        [SerializeField] SpeedPeakTracker speedPeakTracker = null;

        [SerializeField] LoadingBar velocityChangeDetectorBar = null;


        [SerializeField] float positionsMin = 0f;
        [SerializeField] float poitionsMax = 12f;

        [SerializeField] float targetPosition;


        [SerializeField] float rotMin = -90f;
        [SerializeField] float rotMax = 90f;

        [SerializeField] HingeJoint joint;

        [SerializeField] float smoothing = 0.1f;

        [SerializeField] TextMeshProUGUI tmpValue = null;
        [SerializeField] TextMeshProUGUI tmpTitel = null;







        JointSpring spring;

        float mappedRotation;

        void Start()
        {
            if (joint == null)
            {
                joint = GetComponent<HingeJoint>();
            }

            switch (tachomode)
            {
                case Tachomode.readSpeedPeakTracker:
                    if (speedPeakTracker != null)
                    {
                        speedPeakTracker.enabled = true;
                    }
                    if (averageHandSpeedTracker != null)
                    {
                        averageHandSpeedTracker.enabled = false;
                    }
                    break;

                case Tachomode.readAvarageHandSpeed:
                    if (speedPeakTracker != null)
                    {
                        speedPeakTracker.enabled = false;
                    }
                    if (averageHandSpeedTracker != null)
                    {
                        averageHandSpeedTracker.enabled = true;
                    }
                    break;


                default:
                    break;
            }



            spring = joint.spring;
        }




        void Update()
        {
            spring.targetPosition = Mathf.Lerp(spring.targetPosition, MappedRotation(targetPosition), smoothing);
            //print("joint position" + spring.targetPosition);
            joint.spring = spring;

            if (tachomode == Tachomode.readSpeedPeakTracker)
            {

                if (speedPeakTracker == null) return;
                SetTargetPositionWithNormedValue(speedPeakTracker.GetOutputValueNormalized());

            }

            else if (tachomode == Tachomode.readAvarageHandSpeed)
            {

                if (averageHandSpeedTracker == null) return;
                SetTargetPositionWithNormedValue(averageHandSpeedTracker.GetOutputValueNormaized(true));
            }

            else if (tachomode == Tachomode.readVelocityChangeDetector)
            {
                if (velocityChangeDetectorBar == null) return;
                SetTargetPositionWithNormedValue(velocityChangeDetectorBar.GetLoadingValue());
            }

            else if (tachomode == Tachomode.syncedByNetwork)
            {

            }



            SetValueText(Mathf.RoundToInt(targetPosition).ToString());


        }



        public void SetTargetPosition(float position)
        {
            targetPosition = position;
        }



        public float GetTargetPosition() // Gets Read by Blend Shape Control
        {
            return targetPosition;
        }

        public float GetNormedTargetPosition()
        {
            //            print("Tachonadel: get Target Position Norm. " + this.gameObject + "   " + targetPosition + "   " + Mathf.InverseLerp(positionsMin, poitionsMax, targetPosition));
            return Mathf.InverseLerp(positionsMin, poitionsMax, targetPosition);
        }



        private void SetTargetPositionWithNormedValue(float positionNorm) // TODO Change to private (Gets Set By XR Speed Peak Tracker)
        {
            //print("Tachonadel: Set Target Position " + positionNorm);
            targetPosition = Mathf.Lerp(positionsMin, poitionsMax, positionNorm);

        }


        private float MappedRotation(float position)
        {
            float normalizedPosition = Mathf.InverseLerp(positionsMin, poitionsMax, position);
            return Mathf.Lerp(rotMin, rotMax, normalizedPosition);
        }


        /// Text

        public void SetTitelText(string text)
        {
            tmpTitel.text = text;
        }

        public void SetValueText(string text)
        {
            tmpValue.text = text;
        }




        /// Networking

        public void SetTachomodeToNetworkSync() // if 
        {
            tachomode = Tachomode.syncedByNetwork;
        }

        public void SyncTargetPositionWithNormedValue(float positionNorm)
        {
            if (tachomode != Tachomode.syncedByNetwork) return; // doppelt gesichert
            SetTargetPositionWithNormedValue(positionNorm);
        }




    }

}
