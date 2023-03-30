using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.TimelineSpace;
using Photon.Pun;
using ObliqueSenastions.PunNetworking;
using TMPro;

namespace ObliqueSenastions.UISpace
{
    public class InspizientHUD : MonoBehaviour
    {
        
        [SerializeField] GameObject play;
        [SerializeField] GameObject pause;
        [SerializeField] GameObject hold;

        [SerializeField] TextMeshProUGUI zuschauerzahl;
        [SerializeField] TextMeshProUGUI rennfahrerzahl;

        TimeModeMachine timeModeMachine = null;




        
        void Start()
        {
            timeModeMachine = TimeLineHandler.instance.GetComponent<TimeModeMachine>();
            if(PhotonNetwork.IsConnected && MultiplayerConnector.instance.GetRole() == Role.Inspizient)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

       
        void Update()
        {
            switch (timeModeMachine.GetTimelinePlayMode())
            {
                case TimelinePlayMode.Play:
                play.SetActive(true);
                pause.SetActive(false);
                hold.SetActive(false);
                break;

                case TimelinePlayMode.Pause:
                play.SetActive(false);
                pause.SetActive(true);
                hold.SetActive(false);
                break;

                case TimelinePlayMode.Hold:
                play.SetActive(false);
                pause.SetActive(false);
                hold.SetActive(true);
                break;
                
                default:
                play.SetActive(false);
                pause.SetActive(false);
                hold.SetActive(false);

                break;
            } 

            zuschauerzahl.text = MultiplayerConnector.instance.GetNumberOfPlayersOfRole(Role.Zuschauer).ToString();
            rennfahrerzahl.text = MultiplayerConnector.instance.GetNumberOfPlayersOfRole(Role.Rennfahrer).ToString(); 


        }
    }

}

