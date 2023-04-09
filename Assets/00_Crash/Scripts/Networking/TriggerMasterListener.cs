using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.StageMasterSpace;
using UnityEngine;
using UnityEngine.Events;


namespace ObliqueSenastions.PunNetworking
{
    public class TriggerMasterListener : MonoBehaviour
    {
        [SerializeField] TriggerMasterEvent[] triggerMasterEvents;

        [System.Serializable]
        public struct TriggerMasterEvent
        {
            public string name;

            public UnityEvent onTriggerMasterEvent;
        }

        NetworkplayerTriggerMaster networkplayerTriggerMaster = null;
        
        void Start()
        {
            if(networkplayerTriggerMaster == null)
            {
                networkplayerTriggerMaster = GameObject.FindWithTag("StageMaster")?.GetComponent<NetworkplayerTriggerMaster>();
                if(networkplayerTriggerMaster != null)
                {
                    networkplayerTriggerMaster.onTriggerEventDelegate += OnTriggerMasterEvent;
                }
            }
        }

        private void OnDestroy() 
        {
            networkplayerTriggerMaster.onTriggerEventDelegate -= OnTriggerMasterEvent;
        }

        void OnTriggerMasterEvent(string name)
        {
            for (int i = 0; i < triggerMasterEvents.Length; i++)
            {
                if(name == triggerMasterEvents[i].name)
                {
                    triggerMasterEvents[i].onTriggerMasterEvent.Invoke();
                }
            }
        }

        
        
    }

}

