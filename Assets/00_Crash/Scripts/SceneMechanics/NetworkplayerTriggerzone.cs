using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.PunNetworking;
using ObliqueSenastions.StageMasterSpace;
using UnityEngine;

public class NetworkplayerTriggerzone : MonoBehaviour
{
    [Tooltip("-1 for all players")]
    [SerializeField] int playerIndex;

    [SerializeField] string otherTag = "NetworkPlayer";
    [SerializeField] string nameOfEnterEvent;

    [SerializeField] string nameOfExitEvent;

    [SerializeField] bool triggerOnce = false;

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
        if (other.gameObject.tag == otherTag)
        {

            if (playerIndex == -1 || MultiplayerConnector.instance.GetClientsIndexInRole() == playerIndex)
            {
                GameObject.FindWithTag("StageMaster").GetComponent<NetworkplayerTriggerMaster>().TriggerEvent(nameOfEnterEvent);
                if(triggerOnce)
                {
                    gameObject.SetActive(false);
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == otherTag)
        {
            print("triggerd by: " + otherTag);

            if (playerIndex == -1 || MultiplayerConnector.instance.GetClientsIndexInRole() == playerIndex)
            {
                GameObject.FindWithTag("StageMaster").GetComponent<NetworkplayerTriggerMaster>().TriggerEvent(nameOfExitEvent);
                print("trigger: " + nameOfExitEvent);
                if(triggerOnce)
                {
                    gameObject.SetActive(false);
                }
            }

        }

    }
}
