using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.VRRigSpace
{
    public class SpeedPeakTracker : MonoBehaviour
    {
        [SerializeField] SimpleVelocityTracker simpleVelocityTracker;

        [SerializeField] float speedThreshold = 0.1f;

        float currentSpeed;

        float smoothedSpeed;
        float lastSpeed;

        float lastSmoothedSpeed;

        float capturedSpeedPeak;

        [SerializeField] bool rising;

        [SerializeField] float speedPeak;

        [SerializeField] float smoothing = 0.1f;
        void Start()
        {
            currentSpeed = simpleVelocityTracker.GetLocalSpeed();
            smoothedSpeed = Mathf.Lerp(currentSpeed, simpleVelocityTracker.GetLocalSpeed(), smoothing);

            lastSpeed = currentSpeed;
            lastSmoothedSpeed = smoothedSpeed;
        }

        // Update is called once per frame
        void Update()
        {

            currentSpeed = simpleVelocityTracker.GetLocalSpeed();
            smoothedSpeed = Mathf.Lerp(smoothedSpeed, currentSpeed, smoothing);

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

            // if (currentSpeed > speedThreshold)
            // {
                

            // }


            lastSpeed = currentSpeed;
            lastSmoothedSpeed = smoothedSpeed;

            
        }
    }

}

