using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 1f, 1f)]
[TrackBindingType(typeof(SceneControlByTimeline))]
[TrackClipType(typeof(SceneControlClip))]
public class SceneControlTrack : TrackAsset
{

    // protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
    // {
    //     SceneControlClip sceneControlClip = clip.asset as SceneControlClip;
    //     SceneControlBehaviour sceneControlBehaviour = sceneControlClip.template;
    //     clip.displayName = "S: " + sceneControlBehaviour.sceneIndex + " (" + sceneControlBehaviour.sceneName + " )";



    //     return base.CreatePlayable(graph, gameObject, clip);
    // }

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        foreach (var clip in m_Clips)
        {
            SceneControlClip sceneControlClip = clip.asset as SceneControlClip;
            SceneControlBehaviour sceneControlBehaviour = sceneControlClip.template;
            string scene = sceneControlBehaviour.useSceneName ? sceneControlBehaviour.sceneName : sceneControlBehaviour.sceneIndex.ToString();
            clip.displayName = "Scene: " + scene + " Room: " + sceneControlBehaviour.roomSection;
        }
        return ScriptPlayable<SceneControlMixer>.Create(graph, inputCount);
    }

}