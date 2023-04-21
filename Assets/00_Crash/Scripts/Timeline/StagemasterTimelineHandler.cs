using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TimelineSpace
{
    public class StagemasterTimelineHandler : MonoBehaviour
{
    public void JumpToTime(float time)
    {
        TimeLineHandler.instance.GetComponent<TimelineScroller2>().JumpToTime(time);
    }
    
   
}
    
}


