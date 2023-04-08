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

    }


}
