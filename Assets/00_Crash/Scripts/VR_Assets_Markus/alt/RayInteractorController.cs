using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteractorController : MonoBehaviour
{
    public XRController leftRayInteractor;
    public XRController rightRayInteractor;
    public InputHelpers.Button RayInteractActivationButton;
    public float activationThreshold = 0.1f;

    public XRRayInteractor leftInteractorRay;
    public XRRayInteractor rightInteractorRay;

    public bool EnableLeftRayInteraction { get; set; } = true;
    public bool EnableRightRayInteraction { get; set; } = true;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;

        if(leftRayInteractor)
        {
            bool isLeftInteractorRayHovering = leftInteractorRay.TryGetHitInfo(out pos, out norm, out index, out validTarget);
            leftRayInteractor.gameObject.SetActive(EnableLeftRayInteraction && CheckIfActivated(leftRayInteractor) && !isLeftInteractorRayHovering);
        }

        if (rightRayInteractor)
        {
            bool isRightInteractorRayHovering = rightInteractorRay.TryGetHitInfo(out pos, out norm, out index, out validTarget);
            rightRayInteractor.gameObject.SetActive(EnableRightRayInteraction && CheckIfActivated(rightRayInteractor) && !isRightInteractorRayHovering);
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, RayInteractActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
