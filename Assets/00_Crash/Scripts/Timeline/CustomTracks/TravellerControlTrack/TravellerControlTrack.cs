using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{

    [TrackColor(1f, 1f, 1f)]
    [TrackBindingType(typeof(TravellerControlByTimeline))]
    [TrackClipType(typeof(TravellerControlClip))]
    public class TravellerControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var clip in m_Clips) // naming clips
            {
                TravellerControlClip travellerControlClip = clip.asset as TravellerControlClip;
                TravellerControlBehaviour travellerControlBehaviour = travellerControlClip.template;
                clip.displayName = "TP: " + travellerControlBehaviour.transitionPointName;
            }
            return ScriptPlayable<TravellerControlMixer>.Create(graph, inputCount);
        }

    }
}
