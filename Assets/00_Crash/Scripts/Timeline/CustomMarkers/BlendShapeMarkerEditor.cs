#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(BlendShapeMarker))]
public class BlendShapeMarkerEditor : MarkerEditor
{
    public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
    {
        var blendShapeMarker = marker as BlendShapeMarker;

        if(blendShapeMarker != null)
        {
            return new MarkerDrawOptions { tooltip = "blend shape " + blendShapeMarker.SpaceStateIndex().ToString()};
        }

        return base.GetMarkerOptions(marker);
    }
}

#endif
