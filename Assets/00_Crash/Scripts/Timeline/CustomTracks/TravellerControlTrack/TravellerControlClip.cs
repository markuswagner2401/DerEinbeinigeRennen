using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TravellerControlClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    public TravellerControlBehaviour template = new TravellerControlBehaviour();
    public ClipCaps clipCaps
    {
        get
        {
            return ClipCaps.Blending | ClipCaps.Extrapolation;
        }
    }
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TravellerControlBehaviour>.Create(graph, template);

        return playable;
    }
}