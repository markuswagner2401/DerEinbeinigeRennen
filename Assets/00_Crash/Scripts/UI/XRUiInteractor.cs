using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRUiInteractor : MonoBehaviour
{
    [SerializeField] XRNode node;
    InputDevice device;
    
    [SerializeField] UIInteractionsHandler[] interactionsHandler;
    [SerializeField] int interactableLayerIndex = 5;




    private void Start() 
    {
        device = InputDevices.GetDeviceAtXRNode(node);
    }

    private void Update() 
    {
        if (device.TryGetFeatureValue(CommonUsages.gripButton, out bool triggerUsage))
        {
            if(triggerUsage)
            {
                foreach (var handler in interactionsHandler)
                {
                    handler.processTriggerUsage(true);
                }
                  
            }

            // else
            // {
            //     foreach (var handler in interactionsHandler)
            //     {
            //         handler.processTriggerUsage(false);
            //     }
            // }


        }
    }


    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer != interactableLayerIndex) return;


        foreach (var handler in interactionsHandler)
        {
            handler.processHover(other);
        }

    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.layer != interactableLayerIndex) return;

        foreach (var handler in interactionsHandler)
        {
            handler.processHoverExit(other);
        }

    }

    


    
}
