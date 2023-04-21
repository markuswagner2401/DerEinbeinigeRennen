using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TimelineSpace
{
    public class Unfallbremsung : MonoBehaviour
    {
        [SerializeField] Bremsung[] bremsungen;

        [SerializeField] int currentBremsung = 0;

        [System.Serializable]
        public struct Bremsung
        {
            public string name;


            public string otherTag;

        }

        private void Start()
        {

        }

        public void SetBremsung(string name)
        {
            int index = GetIndexByName(name);
            if (index < 0) return;
            currentBremsung = index;
        }

        int GetIndexByName(string name)
        {
            for (int i = 0; i < bremsungen.Length; i++)
            {
                if (name == bremsungen[i].name)
                {
                    return i;
                }
            }
            Debug.LogError("No Bremsung found with name: " + name);
            return -1;
        }

        // private void OnCollisionEnter(Collision other)
        // {
        //     if (other.gameObject.tag == bremsungen[currentBremsung].otherTag)
        //     {
        //         print("Unfallbremsung: Sending to TimeModeMachine: " + bremsungen[currentBremsung].name);
        //         TimeLineHandler.instance.GetComponent<TimeModeMachine>().PlayAccident(bremsungen[currentBremsung].name);
        //     }

        // }

        private void OnTriggerEnter(Collider other)
        {
            if(!this.enabled) return;
            if (other.gameObject.tag == bremsungen[currentBremsung].otherTag)
            {
                print("Unfallbremsung: Sending to TimeModeMachine: " + bremsungen[currentBremsung].name);
                TimeLineHandler.instance.GetComponent<TimeModeMachine>().PlayAccident(bremsungen[currentBremsung].name);
            }

        }
    }

}


