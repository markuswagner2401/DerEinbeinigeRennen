using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.TriggerControl
{
    public class TriggerZoneEventTriggerer : MonoBehaviour
    {
        [SerializeField] UnityEvent My_OnTriggerzoneEnter;
        [SerializeField] string tagName = "Traveller";



        private void OnTriggerEnter(Collider other)
        {
            print("trigger zone enter ");
            if (other.gameObject.tag == tagName)
            {
                My_OnTriggerzoneEnter.Invoke();
            }

        }

    }

}

