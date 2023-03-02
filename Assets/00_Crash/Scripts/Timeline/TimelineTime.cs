using UnityEngine;
using UnityEngine.Playables;

public class TimelineTime : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector = null;
     float currentTimelineTime;
     float timelineDeltaTime;
    private float previousTimelineTime;
    bool overwriteTLDeltaTimeWithTimeDeltaTime = false;

    [SerializeField] float jumpThreshold = 0.1f;

    private void Start()
    {
        if (playableDirector == null)
        { 
            playableDirector = GetComponent<PlayableDirector>();
        }
    }

    private void Update()
    {
        if(playableDirector == null) return;
        currentTimelineTime = (float)playableDirector.time;
        timelineDeltaTime = currentTimelineTime - previousTimelineTime;
        
        if(Mathf.Abs(timelineDeltaTime) > jumpThreshold) // Jump Threshold
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

        return overwriteTLDeltaTimeWithTimeDeltaTime ? Time.deltaTime : timelineDeltaTime;
    }

    public void OverwriteTLDeltaTimeWithTimeDeltaTime(bool value)
    {
        overwriteTLDeltaTimeWithTimeDeltaTime = value ? true : false;
    }
}
