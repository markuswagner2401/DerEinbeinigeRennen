using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteractorControllersimple : MonoBehaviour
{
    public XRController leftRayInteractor;
    public XRController rightRayInteractor;
    public InputHelpers.Button RayInteractActivationButton;
    public float activationThreshold = 0.1f;

    

    // Update is called once per frame
    void Update()
    {


        if(leftRayInteractor)
        {
            leftRayInteractor.gameObject.SetActive(CheckIfActivated(leftRayInteractor));
        }

        if (rightRayInteractor)
        {
            
            rightRayInteractor.gameObject.SetActive(CheckIfActivated(rightRayInteractor));
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, RayInteractActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
