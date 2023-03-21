using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ObliqueSenastions.UISpace;
using UnityEngine.Events;

namespace ObliqueSenastions.VRRigSpace
{
    public class SpeedPeakTracker : MonoBehaviourPun, IVelocityListener
    {
        [SerializeField] List<SimpleVelocityTracker> simpleVelocityTrackersSinglePlayer;

        [SerializeField] List<SimpleVelocityTracker> simpleVelocityTrackersMultiplayer;

        [SerializeField] float speedThreshold = 0.1f;

        [SerializeField] float forcePauseAfterXSeceondes = 6f;

        float resetTimer;

        [SerializeField] float forcedPauseDuration = 5f;

        [SerializeField] UnityEvent doOnForcedPause;

        bool forcePauseState;

        bool forcedPauseTriggered;

        [SerializeField] UnityEvent doOnForcedPauseEnded;

        bool forcedPauseEndTriggered;

        float forcedPauseTimer;



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



        // Update is called once per frame
        void Update()
        {

            currentSpeed = AverageSpeed();
            smoothedSpeed = Mathf.Lerp(smoothedSpeed, currentSpeed, peakDetectorSmoothing);





            //highestSpeed = (currentSpeed > highestSpeed) ? currentSpeed : highestSpeed;

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



            // forced pause



            //





            if (outputValue > 0.001f)
            {
                resetTimer += Time.deltaTime;

                if (resetTimer > forcePauseAfterXSeceondes)
                {
                    if (forcedPauseTimer < forcedPauseDuration) // during pause
                    {
                        forcedPauseTimer += Time.deltaTime;

                        if (!forcedPauseTriggered)
                        {
                            doOnForcedPause.Invoke();
                            forcedPauseTriggered = true;
                        }

                        forcePauseState = true;
                        forcedPauseEndTriggered = false;
                    }

                    else // end of pause
                    {
                        if (!forcedPauseEndTriggered)
                        {
                            doOnForcedPauseEnded.Invoke();
                            forcedPauseEndTriggered = true;
                        }

                        resetTimer = 0;
                        forcedPauseTimer = 0;
                        forcedPauseTriggered = false;
                        forcePauseState = false;

                    }
                }
            }

            




        }

        float AverageSpeed()
        {
            if (PhotonNetwork.IsConnected)
            {
                float speedSum = 0;
                for (int i = 0; i < simpleVelocityTrackersMultiplayer.Count; i++)
                {
                    speedSum += simpleVelocityTrackersMultiplayer[i].GetLocalSpeed();
                }

                return speedSum / simpleVelocityTrackersMultiplayer.Count;

            }

            else
            {
                float speedSum = 0;
                for (int i = 0; i < simpleVelocityTrackersSinglePlayer.Count; i++)
                {
                    speedSum += simpleVelocityTrackersSinglePlayer[i].GetLocalSpeed();
                }

                return speedSum / simpleVelocityTrackersSinglePlayer.Count;

            }

        }

        public void AddMultiplayerTracker(SimpleVelocityTracker[] trackers)
        {
            foreach (var item in trackers)
            {
                simpleVelocityTrackersMultiplayer.Add(item);
            }
        }

        public float GetOutputValueNormalized()
        {
            if (forcePauseState)
            {
                return 0;
            }

            if (dynamicNormaization)
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


        //// IVelocityListener

        public void AddVelocityContributor(SimpleVelocityTracker[] velocityContributors)
        {
            AddMultiplayerTracker(velocityContributors);
        }
    }



}

