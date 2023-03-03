#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{


    [CustomTimelineEditor(typeof(VRTravellerMarker))]
    public class VRTravellerMarkerEditor : MarkerEditor
    {
        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            var vrTravellerMarker = marker as VRTravellerMarker;

            if (vrTravellerMarker != null)
            {
                return new MarkerDrawOptions { tooltip = "rig " + vrTravellerMarker.TransitionPointIndex.ToString() + " : " + vrTravellerMarker.Note };
            }

            return base.GetMarkerOptions(marker);
        }
    }
}


#endif
