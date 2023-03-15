using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ObliqueSenastions.UISpace;

namespace ObliqueSenastions.PunNetworking
{
    public class SyncTachonadel : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] Role ownerRole;

        [SerializeField] Tachonadel tachonadel = null;

        

        Role localPlayerRole;

        float streamedNormedValue;
        

        
        void Start()
        {
            if(PhotonNetwork.IsConnected)
            {
                 localPlayerRole = MultiplayerConnector.instance.GetRole();

                 SetupSync(localPlayerRole == ownerRole);

                 
            }
           

        }

        
        void Update()
        {

        }

        public override void OnJoinedRoom()
        {
            localPlayerRole = MultiplayerConnector.instance.GetRole();

            SetupSync(localPlayerRole == ownerRole);
        }

        

    

        void SetupSync(bool isOwnerRole)
        {
            if(isOwnerRole)
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
