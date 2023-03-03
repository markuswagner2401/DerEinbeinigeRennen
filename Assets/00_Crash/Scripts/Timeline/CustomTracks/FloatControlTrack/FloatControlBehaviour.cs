using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace ObliqueSenastions.TimelineSpace
{


    [Serializable]
    public class FloatControlBehaviour : PlayableBehaviour
    {

        public string parameterName;
        public float value;

        public bool firstClipInTrack;

    }

}
