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
        

        


        public PropertyName id { get { return new PropertyName(); } }

        
        public string Note => note;
        public bool pauseOrHold => stopOrHold;

        public bool DisplayModeInInspiUI => displayModeInInspiUI;
        



    }

}
