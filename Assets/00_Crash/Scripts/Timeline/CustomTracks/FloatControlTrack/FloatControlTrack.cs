using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[TrackColor(1f, 1f, 1f)]
[TrackBindingType(typeof(FloatControlByTimeline))]
[TrackClipType(typeof(FloatControlClip))]
public class FloatControlTrack : TrackAsset
{
    
    
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        foreach (var clip in m_Clips) // naming clips
        {
            FloatControlClip floatControlClip =  clip.asset as FloatControlClip;
            FloatControlBehaviour floatControlBehaviour = floatControlClip.template;
            clip.displayName = floatControlBehaviour.parameterName;
            
        }
        return ScriptPlayable<FloatControlMixer>.Create(graph, inputCount);
    }

    
}