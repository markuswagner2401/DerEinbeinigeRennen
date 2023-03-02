#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(TextureFaderMarker))]
public class TextureFaderMarkerEditor : MarkerEditor
{
    public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
    {
        var textureFaderMarker = marker as TextureFaderMarker;

        if(textureFaderMarker != null)
        {
            return new MarkerDrawOptions { tooltip = "tribuneTexChanger " + textureFaderMarker.TextureChangerIndex().ToString()};
        }

        return base.GetMarkerOptions(marker);
    }
}

#endif