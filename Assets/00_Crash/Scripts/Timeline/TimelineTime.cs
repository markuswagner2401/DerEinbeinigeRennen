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
        [SerializeField] TimelineTimeMode mode = TimelineTimeMode.useTimelineTime;
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
            switch (mode)
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
            mode = newMode;
        }

        public void SetMode(int newMode)
        {
            mode = (TimelineTimeMode)newMode;
        }

        public TimelineTimeMode GetMode()
        {
            return mode;
        }

        // public void OverwriteTLDeltaTimeWithTimeDeltaTime(bool value)
        // {
        //     overwriteTLDeltaTimeWithTimeDeltaTime = value ? true : false;
        // }
    }

}
