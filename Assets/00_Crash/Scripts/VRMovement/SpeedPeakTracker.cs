using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.VRRigSpace
{
    public class SpeedPeakTracker : MonoBehaviour
    {
        [SerializeField] SimpleVelocityTracker[] simpleVelocityTrackers;

        [SerializeField] float speedThreshold = 0.1f;

        float currentSpeed;

        float smoothedSpeed;
        float lastSpeed;

        float lastSmoothedSpeed;

        float capturedSpeedPeak;

        [SerializeField] bool rising;

        [SerializeField] float speedPeak;

        [SerializeField] float peakDetectorSmoothing = 0.1f;

        [SerializeField] float outputRisingSmoothing = 0.1f;

        [SerializeField] float outputFallingSmoothing = 0.1f;

        [SerializeField] float fallingDuration = 3f;

        [SerializeField] AnimationCurve fallingCurve;

        [SerializeField] bool dynamicNormaization = true;

        [SerializeField] float minSpeed = 0f;
        [SerializeField] float maxSpeed = 10f;

        float highestSpeed = 0.1f;

        float risingTimer = 0;

        float fallingTimer = 0;

        [SerializeField] float outputValue;





        

        void Start()
        {
            currentSpeed = AverageSpeed();
            smoothedSpeed = Mathf.Lerp(currentSpeed, currentSpeed, peakDetectorSmoothing);

            lastSpeed = currentSpeed;
            lastSmoothedSpeed = smoothedSpeed;
        }

        float AverageSpeed()
        {
            float speedSum = 0;
            for (int i = 0; i < simpleVelocityTrackers.Length; i++)
            {
                speedSum += simpleVelocityTrackers[i].GetLocalSpeed();
            }

            return speedSum / simpleVelocityTrackers.Length;
        }

        // Update is called once per frame
        void Update()
        {

            currentSpeed = AverageSpeed();
            smoothedSpeed = Mathf.Lerp(smoothedSpeed, currentSpeed, peakDetectorSmoothing);

            highestSpeed = (currentSpeed > highestSpeed) ? currentSpeed : highestSpeed;

            if (lastSmoothedSpeed < smoothedSpeed)
            {
                rising = true;
                speedPeak = (capturedSpeedPeak > currentSpeed) ? capturedSpeedPeak : currentSpeed;
                capturedSpeedPeak = speedPeak;


            }

            else
            {
                rising = false;
                capturedSpeedPeak = 0f;


            }


            lastSpeed = currentSpeed;
            lastSmoothedSpeed = smoothedSpeed;

            ///

            if (rising)
            {
                fallingTimer = 0;
                risingTimer += Time.deltaTime;
                outputValue = Mathf.Lerp(outputValue, speedPeak, outputRisingSmoothing);
            }

            else
            {
                risingTimer = 0;
                fallingTimer += Time.deltaTime;

                float valueOnCurve = Mathf.Lerp(speedPeak, 0f, fallingCurve.Evaluate(fallingTimer / fallingDuration));
                outputValue = Mathf.Lerp(outputValue, valueOnCurve, outputFallingSmoothing);

            }


        }

        public float GetOutputValueNormalized()
        {
            if(dynamicNormaization)
            {
                return Mathf.InverseLerp(0, highestSpeed, outputValue);
            }

            else
            {
                return Mathf.InverseLerp(minSpeed, maxSpeed, outputValue);
            }
            
        }

        private void OnDrawGizmos() 
        {
            
            Gizmos.DrawLine(transform.position, transform.position + transform.up * GetOutputValueNormalized() * 10f);
        }
    }



}

