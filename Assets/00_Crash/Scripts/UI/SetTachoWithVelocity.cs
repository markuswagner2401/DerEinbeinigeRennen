using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ObliqueSenastions.VRRigSpace;

namespace ObliqueSenastions.UISpace
{

    public class VelocityPeakHandler : MonoBehaviour
    {



        float veloTheshold = 0.1f;

        bool active;

        [SerializeField] PeakTracker[] peakTrackers;


        struct PeakTracker
        {
            public XRVelocityTracker velocityTracker;
            public UnityEvent unityEvent;
            public float currentSpeed;
            public float speedThreshold;
            public float speadPeak;
            public bool thresholdBroken;
            public float time;
            public float stayTimeAtPeak;

        }




        void Update()
        {
            for (int i = 0; i < peakTrackers.Length; i++)
            {
                peakTrackers[i].currentSpeed = peakTrackers[i].velocityTracker.GetSpeed();
                peakTrackers[i].time += Time.deltaTime;
                print(peakTrackers[i].currentSpeed);

                if (peakTrackers[i].time > peakTrackers[i].stayTimeAtPeak && peakTrackers[i].currentSpeed > veloTheshold)
                {
                    if (peakTrackers[i].thresholdBroken) return;
                    peakTrackers[i].thresholdBroken = true;
                    StartCoroutine(CalculateSpeedPeak(peakTrackers[i]));
                }

                else
                {
                    peakTrackers[i].thresholdBroken = false;
                    peakTrackers[i].time = 0f;
                }
            }


        }

        IEnumerator CalculateSpeedPeak(PeakTracker peakTracker)
        {
            float previousSpeed = 0f;
            float currentSpeed = peakTracker.velocityTracker.GetSpeed();

            while (previousSpeed < currentSpeed)
            {
                previousSpeed = currentSpeed;
                yield return new WaitForEndOfFrame();
                currentSpeed = peakTracker.velocityTracker.GetSpeed();
            }

            peakTracker.speadPeak = previousSpeed;

            peakTracker.unityEvent.Invoke();




            yield break;
        }




    }

}
