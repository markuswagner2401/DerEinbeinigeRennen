using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{

    public class SceneControlClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        public SceneControlBehaviour template = new SceneControlBehaviour();
        public ClipCaps clipCaps
        {
            get
            {
                return ClipCaps.Extrapolation;
            }
        }
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<SceneControlBehaviour>.Create(graph, template);


        }
    }
}
