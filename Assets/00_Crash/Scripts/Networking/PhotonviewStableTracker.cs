using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ObliqueSenastions.PunNetworking
{

    public class PhotonViewStableTracker : MonoBehaviourPunCallbacks
    {
        private bool _viewsStable;

        private void Update()
        {
            ViewsStableCheck();
        }

        private void ViewsStableCheck()
        {
            if (!_viewsStable && PhotonNetwork.NetworkingClient.State == ClientState.Joined)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber <= PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    _viewsStable = true;

                    if (PhotonNetwork.IsMasterClient)
                    {
                        // Send a custom event to all other clients
                        byte eventCode = 1;
                        PhotonNetwork.RaiseEvent(eventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                    }
                }
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == 1)
            {
                // Perform actions when the custom event is received
                // ...
            }
        }
    }




}
