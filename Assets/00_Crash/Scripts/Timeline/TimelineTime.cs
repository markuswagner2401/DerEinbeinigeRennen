using UnityEngine;
using UnityEngine.Playables;

namespace ObliqueSenastions.TimelineSpace
{
    [System.Serializable]
    public enum TimelineTimeMode
    {
        useTimelineTime,
        useDeltaTime,
        useCustomTime
    }

    public class TimelineTime : MonoBehaviour
    {
        [SerializeField] TimelineTimeMode currentMode = TimelineTimeMode.useTimelineTime;

        TimelineTimeMode bufferedMode;
        [SerializeField] PlayableDirector playableDirector = null;
        float currentTimelineTime;
        float timelineDeltaTime;
        private float previousTimelineTime;
        //bool overwriteTLDeltaTimeWithTimeDeltaTime = false;

        [SerializeField] float jumpThreshold = 0.1f;

        [SerializeField] float customDeltaTime = 0f;

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
            //print("TimelineTimeDelta: " + timelineDeltaTime);
            previousTimelineTime = currentTimelineTime;
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

                case TimelineTimeMode.useCustomTime:
                returnValue = customDeltaTime;
                break;

                default:
                break;
            }

            return returnValue;
        }

        public void SetMode(TimelineTimeMode newMode)
        {
            if(currentMode == TimelineTimeMode.useCustomTime)
            {
                bufferedMode = newMode;
            }

            currentMode = newMode;
        }

        public void UseCustomTime(bool yes)
        {
            if(yes)
            {
                bufferedMode = currentMode;
                currentMode = TimelineTimeMode.useCustomTime;
            }

            else
            {
                currentMode = bufferedMode;
            }
            

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
