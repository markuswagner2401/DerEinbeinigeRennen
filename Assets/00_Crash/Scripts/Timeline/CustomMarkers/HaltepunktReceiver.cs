using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using ObliqueSenastions.Animation;
using ObliqueSenastions.PunNetworking;
using TMPro;

namespace ObliqueSenastions.TimelineSpace
{

    public class HaltepunktReceiver : MonoBehaviour, INotificationReceiver
    {


        [SerializeField] TimeModeMachine timeModeMachine;

        private List<double> markerTimes = new List<double>();

        private void Start()
        {
            

            if (timeModeMachine == null)
            {
                timeModeMachine = GetComponent<TimeModeMachine>();
            }

         

            
        }
        public void OnNotify(Playable origin, INotification notification, object context)
        {

            if (notification is HaltepunktMarker haltpunktMarker)
            {

                TimelinePlayMode newMode = haltpunktMarker.pauseOrHold ? TimelinePlayMode.Pause : TimelinePlayMode.Hold;
                if(newMode == TimelinePlayMode.Pause)
                {
                    timeModeMachine.Pause();
                }
                else
                {
                    timeModeMachine.Hold();
                }

                ///

                if(haltpunktMarker.DisplayModeInInspiUI && MultiplayerConnector.instance.GetRole() == Role.Inspizient)
                {
                    HaltepunktDisplay haltepunktDisplay = FindHaltepunktDisplay();
                    if(haltepunktDisplay == null) return;
                    haltepunktDisplay.SetHaltepunktText(haltpunktMarker.Note); 
                }

                
                

            }


        }

        HaltepunktDisplay FindHaltepunktDisplay()
        {
            GameObject haltepunktDisplayGO = GameObject.FindWithTag("HaltepunktDisplay");
            if(haltepunktDisplayGO == null) return null;
            return haltepunktDisplayGO.GetComponent<HaltepunktDisplay>();
        }



        

        



    }

}