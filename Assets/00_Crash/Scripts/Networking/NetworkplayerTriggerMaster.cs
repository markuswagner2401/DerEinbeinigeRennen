using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.StageMasterSpace
{
    public class NetworkplayerTriggerMaster : MonoBehaviour
    {
        [SerializeField] EventTriggerer[] eventTriggerers;

        [System.Serializable]
        public struct EventTriggerer
        {
            public string name;
            public UnityEvent onTiggerEvents;
        }

        public delegate void OnTriggerEventDelegate(string name);
        public OnTriggerEventDelegate onTriggerEventDelegate;



        public void TriggerEvent(string name)
        {
            for (int i = 0; i < eventTriggerers.Length; i++)
            {
                if(eventTriggerers[i].name == name)
                {
                    eventTriggerers[i].onTiggerEvents.Invoke();
                    
                }
            }
        }

        public void SendDelegateTriggerEvent(string name)
        {
            if(onTriggerEventDelegate == null) return;
            onTriggerEventDelegate.Invoke(name);
        }

    }


}
