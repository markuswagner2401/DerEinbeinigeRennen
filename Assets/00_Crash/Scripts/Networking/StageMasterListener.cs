using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.StageMasterSpace;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.PunNetworking
{

    public class StageMasterListener : MonoBehaviour
    {
        [SerializeField] StageMasterEvent[] stageMasterEvents;

        [System.Serializable]
        struct StageMasterEvent
        {
            public string name;
            public UnityEvent unityEvent;
        }

        StageMaster stageMaster = null;

        bool setup = false;



        void Start()
        {

        }



        // Update is called once per frame
        void Update()
        {



        }


        public void SetupStageMasterListener()  // gets setup by NetworkPlayerSpawner
        {

            if (ReferenceEquals(stageMaster, null))
            {
                setup = false;
                stageMaster = GameObject.FindWithTag("StageMaster").GetComponent<StageMaster>();
            }

            if (!ReferenceEquals(stageMaster, null) && !setup)
            {
                stageMaster.onGoEvent += PlayGoEvent;
                setup = true;
                return;
            }


        }

        void PlayGoEvent(string name)
        {
            foreach (var item in stageMasterEvents)
            {
                if (name == item.name)
                {
                    item.unityEvent.Invoke();
                }
            }
        }
    }

}
