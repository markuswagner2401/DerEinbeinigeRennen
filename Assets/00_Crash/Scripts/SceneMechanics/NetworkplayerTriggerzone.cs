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

    [SerializeField] bool triggerEnterOnce;

    [SerializeField] bool enterActive;

    [SerializeField] string nameOfExitEvent;

    [SerializeField] bool triggerExitOnce;

    [SerializeField] bool exitAcitive;

   

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
        if(!enterActive) return;

        if (other.gameObject.tag == otherTag)
        {
            if (playerIndex == -1 || MultiplayerConnector.instance.GetClientsIndexInRole() == playerIndex)
            {
                GameObject.FindWithTag("StageMaster").GetComponent<NetworkplayerTriggerMaster>().TriggerEvent(nameOfEnterEvent);
                if(triggerEnterOnce)
                {
                    enterActive = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!exitAcitive) return;

        if (other.gameObject.tag == otherTag)
        {
            
            
            if (playerIndex == -1 || MultiplayerConnector.instance.GetClientsIndexInRole() == playerIndex)
            {
                GameObject.FindWithTag("StageMaster").GetComponent<NetworkplayerTriggerMaster>().TriggerEvent(nameOfExitEvent);
                
                if(triggerExitOnce)
                {
                    exitAcitive = false;
                }
            }
        }
    }

    public void ActivateTriggerEnter(bool value)
    {
        enterActive = value;
    }

    public void ActivateTriggerExit(bool value)
    {
        exitAcitive = value;
    }
}
