using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using RootMotion.FinalIK;

namespace ObliqueSenastions.PunNetworking
{

    public class SyncPuppet : MonoBehaviourPunCallbacks
    {
        [SerializeField] VRIK vrik;

        public override void OnEnable()
        {
            if (PhotonNetwork.InRoom)
            {
                OnJoinedRoom();
            }

            base.OnEnable();
        }
        // void Start()
        // {
            

        // }

        public override void OnJoinedRoom()
        {
            SetupPuppetSync();
        }







        void SetupPuppetSync()
        {
            if (MultiplayerConnector.instance.GetRole() != Role.Rennfahrer)
            {
                vrik.solver.IKPositionWeight = 0f;
            }

            else
            {
                photonView.RequestOwnership();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
