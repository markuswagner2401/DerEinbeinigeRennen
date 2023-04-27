using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ObliqueSenastions.PunNetworking;
using Photon.Pun;

public class InNetworkEvents : MonoBehaviour
{
    [SerializeField] UnityEvent doIfWeAreInNetwork;
    [SerializeField] UnityEvent doIfWeAreNotInNetwork;
    
    void Start()
    {
        MultiplayerConnector.instance.my_OnJoinedRoom += DoIfInNetwork;

        if(PhotonNetwork.IsConnected)
        {
            DoIfInNetwork();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoIfInNetwork()
    {
        doIfWeAreInNetwork.Invoke();
    }
}
