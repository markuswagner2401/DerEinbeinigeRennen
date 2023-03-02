using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class SceneControlBehaviour : PlayableBehaviour
{
    [Tooltip("sceneName only for Debugging, sceneIndex is decisive")]
    public string sceneName;

    public bool useSceneName;
    
    public int sceneIndex;

    public bool jumpToClipStartOnGoingBackInTL;

    public double clipDuration;

    public string roomSection;

    

   

    public override void OnGraphStart(Playable playable)
    {
        clipDuration = playable.GetDuration();
       
    }
}