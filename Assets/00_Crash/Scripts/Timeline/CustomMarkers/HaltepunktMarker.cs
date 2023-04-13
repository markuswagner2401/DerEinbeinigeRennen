using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{


    public class HaltepunktMarker : Marker, INotification, IMarker
    {
        [SerializeField]  string note;
        [SerializeField] bool displayModeInInspiUI;
        [SerializeField] bool stopOrHold;

        [SerializeField] bool haltInOfflineMode;

        [Tooltip("Set -1 for infinite wait time")]
        [SerializeField] float waitDurationInOfflineMode;
        

        


        public PropertyName id { get { return new PropertyName(); } }
        public string Note => note;
        public bool pauseOrHold => stopOrHold;

        public bool DisplayModeInInspiUI => displayModeInInspiUI;

        public bool HaltInOfflineMode => haltInOfflineMode;

        public float WaitDurationInOfflineMode => waitDurationInOfflineMode;
        



    }

}
