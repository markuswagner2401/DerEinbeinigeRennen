using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using RootMotion.FinalIK;

public class SyncPuppet : MonoBehaviourPunCallbacks
{
    [SerializeField] VRIK vrik;
    void Start()
    {
        if(PhotonNetwork.InRoom)
        {
            OnJoinedRoom();
        }
        
    }

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
