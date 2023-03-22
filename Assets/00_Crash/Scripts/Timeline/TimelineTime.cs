using UnityEngine;
using UnityEngine.Playables;

namespace ObliqueSenastions.TimelineSpace
{
    [System.Serializable]
    public enum TimelineTimeMode
    {
        useTimelineTime,
        useDeltaTime,
        useBreakTime
    }

    public class TimelineTime : MonoBehaviour
    {
        [SerializeField] TimelineTimeMode currentMode = TimelineTimeMode.useTimelineTime;

        TimelineTimeMode capturedMode;
        [SerializeField] PlayableDirector playableDirector = null;
        float currentTimelineTime;
        float timelineDeltaTime;
        private float previousTimelineTime;
        //bool overwriteTLDeltaTimeWithTimeDeltaTime = false;

        [SerializeField] float jumpThreshold = 0.1f;

        [SerializeField] float breakDeltaTime = 0f;

        [SerializeField] bool goIntoBreakAtBadTracking = false;

        [SerializeField] OVRHand leftHand = null;

        [SerializeField] OVRHand rightHand = null;

        [SerializeField] float acceptedTimeWithBadTracking = 1f;

        float badTrackingTimer;

        private void Start()
        {
            if (playableDirector == null)
            {
                playableDirector = GetComponent<PlayableDirector>();
            }
        }

        private void Update()
        {
            if (playableDirector == null) return;
            currentTimelineTime = (float)playableDirector.time;
            timelineDeltaTime = currentTimelineTime - previousTimelineTime;

            if (Mathf.Abs(timelineDeltaTime) > jumpThreshold) // Jump Threshold
            {
                Debug.Log("TimelineTime jump threshold");
                timelineDeltaTime = Time.deltaTime;
            }

            previousTimelineTime = currentTimelineTime;

            if(goIntoBreakAtBadTracking)
            {
                if(leftHand == null)
                {
                    leftHand = GameObject.FindWithTag("LeftOVRHand").GetComponent<OVRHand>();
                    return;

                }
                if(rightHand == null)
                {
                    rightHand = GameObject.FindWithTag("RightOVRHand").GetComponent<OVRHand>();
                    return;
                }


                
                if(!leftHand.IsDataHighConfidence && !rightHand.IsDataHighConfidence)
                {
                    badTrackingTimer += Time.deltaTime;
                    if(badTrackingTimer > acceptedTimeWithBadTracking)
                    {
                        UseCustomTime(true);
                    }
                }
                else
                {
                    badTrackingTimer = 0f;
                    UseCustomTime(false);
                }
            }
            //print("TimelineTimeDelta: " + timelineDeltaTime);
            
        }

        public float GetTimelineTime()
        {
            return currentTimelineTime;
        }

        public float GetTimelineDeltaTime()
        {

            return timelineDeltaTime;
        }

        public float GetModeDependentTimelineDeltaTime()
        {
            float returnValue = 0;
            switch (currentMode)
            {
                case TimelineTimeMode.useTimelineTime:
                    returnValue = timelineDeltaTime;
                    break;

                case TimelineTimeMode.useDeltaTime:
                    returnValue = Time.deltaTime;
                    break;

                case TimelineTimeMode.useBreakTime:
                    returnValue = breakDeltaTime;
                    break;

                default:
                    break;
            }

            return returnValue;
        }

        public void SetMode(TimelineTimeMode newMode)
        {
            if (currentMode == TimelineTimeMode.useBreakTime)
            {
                CaptureMode(newMode);
                return;
            }

            currentMode = newMode;
        }

        public void UseCustomTime(bool yes)
        { 
            if (yes)
            {
                
                if (currentMode == TimelineTimeMode.useBreakTime) return;

                CaptureMode(currentMode);
                
                currentMode = TimelineTimeMode.useBreakTime;
            }

            else
            {
                currentMode = capturedMode;
            }


        }

        void CaptureMode(TimelineTimeMode mode)
        {
            if(mode == TimelineTimeMode.useBreakTime) return;
            capturedMode = mode;
        }



        // public TimelineTimeMode GetMode()
        // {
        //     return mode;
        // }

        // public void OverwriteTLDeltaTimeWithTimeDeltaTime(bool value)
        // {
        //     overwriteTLDeltaTimeWithTimeDeltaTime = value ? true : false;
        // }
    }

}
