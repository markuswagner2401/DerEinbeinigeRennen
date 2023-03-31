#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{

    [CustomTimelineEditor(typeof(HaltepunktMarker))]
    public class HaltepunktMarkerEditor : MarkerEditor
    {
        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            var haltepunktMarker = marker as HaltepunktMarker;

            if (haltepunktMarker != null)
            {
                string state = haltepunktMarker.pauseOrHold ? "Stop: " : "Halt: ";
                return new MarkerDrawOptions {tooltip = state + haltepunktMarker.Note};
            }

            return base.GetMarkerOptions(marker);
        }
    }

}

#endif