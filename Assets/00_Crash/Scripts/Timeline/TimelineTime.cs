using UnityEngine;
using UnityEngine.Playables;

namespace ObliqueSenastions.TimelineSpace
{


    public class TimelineTime : MonoBehaviour
    {


        TimelineTimeMode capturedMode;

        [SerializeField] PlayableDirector playableDirector = null;

        [SerializeField] TimeModeMachine timeModeMachine = null;


        float currentTimelineTime;
        float timelineDeltaTime;
        private float previousTimelineTime;
        //bool overwriteTLDeltaTimeWithTimeDeltaTime = false;

        [SerializeField] float jumpThreshold = 0.1f;

        [SerializeField] float badTrackingDeltaTime = 0f;

        [SerializeField] float uiDeltaTime = 0f;



        private void Start()
        {
            if (playableDirector == null)
            {
                playableDirector = GetComponent<PlayableDirector>();
            }

            if (timeModeMachine == null)
            {
                timeModeMachine = GetComponent<TimeModeMachine>();
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
            
            switch (timeModeMachine.GetTimelinePlayMode())
            {
                case TimelinePlayMode.Hold:
                    if (timeModeMachine.GetInUiTime())
                    {
                        return uiDeltaTime;
                    }
                    else if (timeModeMachine.GetBadTracking())
                    {
                        return badTrackingDeltaTime;
                    }
                    else
                    {
                        return Time.deltaTime * timeModeMachine.GetCurrentAccidentTimeFactor();
                    }

                
                default:
                    if (timeModeMachine.GetInUiTime())
                    {
                        return uiDeltaTime;
                    }
                    else if (timeModeMachine.GetBadTracking())
                    {
                        return badTrackingDeltaTime;
                    }
                    else
                    {
                        return timelineDeltaTime * timeModeMachine.GetCurrentAccidentTimeFactor();
                    }

            }

            
        }

        
    }

}
