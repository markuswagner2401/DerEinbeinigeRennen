using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace ObliqueSenastions.UISpace
{

    public class UIInteractionsHandler : MonoBehaviour
    {


        [SerializeField] UiInteration[] uiInteractables;

        [System.Serializable]
        struct UiInteration
        {
            public Transform position;
            public Collider collider;

            public UnityEvent onUiTriggerEnter;
            public UnityEvent onUiTriggerExit;
            public UnityEvent onUiActivate;
            public UnityEvent onUiDeactivate;

            public bool hovering;
            public bool activated;
        }



        void Start()
        {
            for (int i = 0; i < uiInteractables.Length; i++)
            {
                uiInteractables[i].collider = uiInteractables[i].position.GetComponentInChildren<Collider>();
                uiInteractables[i].collider.isTrigger = true;
            }

        }



        void Update()
        {

        }

        public void processHover(Collider collider)
        {
            //print("process hover");

            for (int i = 0; i < uiInteractables.Length; i++)
            {
                if (uiInteractables[i].collider != collider) continue;
                // if(uiInteractables[i].activated) return;
                uiInteractables[i].onUiTriggerEnter.Invoke();
                uiInteractables[i].hovering = true;
            }
        }

        public void processHoverExit(Collider collider)
        {
            //print("process hoverExit");

            for (int i = 0; i < uiInteractables.Length; i++)
            {
                if (uiInteractables[i].collider != collider) continue;
                uiInteractables[i].hovering = false;
                if (uiInteractables[i].activated) return;
                uiInteractables[i].onUiTriggerExit.Invoke();

            }

        }

        public void processTriggerUsage(bool triggerUsage)
        {
            //print("process TriggerUsage");

            if (triggerUsage)
            {
                for (int i = 0; i < uiInteractables.Length; i++)
                {
                    if (!uiInteractables[i].hovering) continue;
                    // if(uiInteractables[i].activated) continue;

                    uiInteractables[i].activated = true;
                    uiInteractables[i].onUiActivate.Invoke();
                    DeactivateOthers(i);
                }
            }

            // else
            // {
            //     for (int i = 0; i < uiInteractables.Length; i++)
            //     {
            //         if (!uiInteractables[i].hovering) continue;
            //         if (!uiInteractables[i].activated) continue;

            //         uiInteractables[i].activated = false;
            //         uiInteractables[i].onUiDeactivate.Invoke();

            //     }
            // }
        }

        private void DeactivateOthers(int i)
        {
            for (int j = 0; j < uiInteractables.Length; j++)
            {
                if (i == j) continue;
                uiInteractables[j].activated = false;
                uiInteractables[j].onUiDeactivate.Invoke();
            }
        }
    }

}
