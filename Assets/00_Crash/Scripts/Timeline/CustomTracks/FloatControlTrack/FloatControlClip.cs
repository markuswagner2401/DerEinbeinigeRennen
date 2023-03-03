using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{

    public class FloatControlClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        public FloatControlBehaviour template = new FloatControlBehaviour();
        public ClipCaps clipCaps
        {
            get
            {
                return ClipCaps.Blending | ClipCaps.Extrapolation;
            }
        }



        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<FloatControlBehaviour>.Create(graph, template);



            return playable;
        }
    }


}

