#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{

    [CustomTimelineEditor(typeof(GoEventMarker))]
    public class GoEventMarkerEditor : MarkerEditor
    {
        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            var goEventMarker = marker as GoEventMarker;

            if (marker != null)
            {
                return new MarkerDrawOptions { tooltip = goEventMarker.GoEventName() + " (GoEvent)" };
            }

            return base.GetMarkerOptions(marker);
        }
    }
}


#endif
