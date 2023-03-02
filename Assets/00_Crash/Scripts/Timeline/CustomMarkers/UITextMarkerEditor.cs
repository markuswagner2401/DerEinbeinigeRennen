#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(UITextMarker))]
public class UITextMarkerEditor : MarkerEditor
{
    public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
    {
        var uiTextMarker = marker as UITextMarker;
       
        if(marker != null)
        {
            return new MarkerDrawOptions { tooltip =  uiTextMarker.Text };
        }

        return base.GetMarkerOptions(marker);
    }
 
}

#endif
