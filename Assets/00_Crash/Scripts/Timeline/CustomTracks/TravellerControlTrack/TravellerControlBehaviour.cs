// Traveller control Behaviour

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TravellerControlBehaviour : PlayableBehaviour
{
    [Tooltip("name gets used to determine transition point; only if its '', the index will be used")]
    public string transitionPointName;

    public int transPointIndex;

    public bool roleAware;

    public Role role;
    public bool lastClipInScene;

    [Tooltip("No Input from Clip/Track")]
    public bool inactive;

    


}