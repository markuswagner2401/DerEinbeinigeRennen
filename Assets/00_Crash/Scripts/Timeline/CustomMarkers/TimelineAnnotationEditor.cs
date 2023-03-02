#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(TimelineAnnotation))]
public class TimelineAnnotationEditor : MarkerEditor
{
    public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
    {
        var timelineAnnotation = marker as TimelineAnnotation;

        if(timelineAnnotation != null)
        {
            return new MarkerDrawOptions { tooltip =  timelineAnnotation.Title
                                                                            };
        }

        return base.GetMarkerOptions(marker);
    }
}
#endif
