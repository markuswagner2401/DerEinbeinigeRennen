using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.PunNetworking;
using ObliqueSenastions.StageMasterSpace;
using UnityEngine;

public class NetworkplayerTriggerzone : MonoBehaviour
{
    [SerializeField] int playerIndex;
    [SerializeField] string nameOfEvent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NetworkPlayer")
        {
            if(MultiplayerConnector.instance.GetClientsIndexInRole() == playerIndex)
            {
                GameObject.FindWithTag("StageMaster").GetComponent<NetworkplayerTriggerMaster>().TriggerEvent(nameOfEvent);
            }
            
        }

    }
}
