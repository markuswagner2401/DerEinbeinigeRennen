using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ObliqueSenastions.UISpace;

namespace ObliqueSenastions.PunNetworking
{
    public class SyncTachonadel : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] Role role;

        [SerializeField] Tachonadel tachonadel = null;

        

        Role localPlayerRole;

        float streamedNormedValue;
        

        
        void Start()
        {
            if(PhotonNetwork.IsConnected)
            {
                 localPlayerRole = MultiplayerConnector.instance.GetRole();

                 SetupSync(localPlayerRole == role);

                 
            }
           

        }

        
        void Update()
        {

        }

        public override void OnJoinedRoom()
        {
            localPlayerRole = MultiplayerConnector.instance.GetRole();

            SetupSync(localPlayerRole == role);
        }

        

    

        void SetupSync(bool isRole)
        {
            if(isRole)
            {
                photonView.RequestOwnership();
            }

            else
            {
                tachonadel.SetTachomodeToNetworkSync();
            }
        }



        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                streamedNormedValue = tachonadel.GetNormedTargetPosition();
                stream.SendNext(streamedNormedValue);
            }

            else
            {
                streamedNormedValue = (float)stream.ReceiveNext();
                tachonadel.SyncTargetPositionWithNormedValue(streamedNormedValue);
            }
        }
    }


}
