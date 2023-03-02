using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class VRIKEasyCalibrate : MonoBehaviour
{
    [SerializeField] VRIK ik;
    [SerializeField] float correctionFactor = 1;
    [SerializeField] bool setupMode = false;

    // custom Input
    [SerializeField] XRNode node;
    InputDevice calibrationDevice;
    [SerializeField] float secondsToHoldButton = 3f;

    [SerializeField] bool calibrateByButton = false;

    [SerializeField] bool autoCalibrateAfterStart;

   
    float buttonHoldTime = 0f;
    bool pressedLongEnoughToActivate = false;
    bool calibrationTriggered = false;
    bool calibrateByEvent;

    StageMaster stageMaster = null;



    private void Start()
    {
        calibrationDevice = InputDevices.GetDeviceAtXRNode(node);

        if(autoCalibrateAfterStart)
        {
            StartCoroutine(CalibrateAfterStartRoutine());
        }


        // float sizeF = (ik.solver.spine.headTarget.position.y - ik.references.root.position.y) / ((ik.references.head.position.y ) - ik.references.root.position.y);
        // ik.references.root.localScale *= sizeF;

    }

    IEnumerator CalibrateAfterStartRoutine()
    {
        yield return new WaitForSeconds(5f);
        Calibrate();
        yield break;
    }

    private void Update()
    {
        

    }

    private void LateUpdate()
    {

        if (calibrateByButton && (calibrationDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed) && pressed))
        {
            buttonHoldTime += Time.deltaTime;
        }

        else
        {
            buttonHoldTime = 0;
            calibrationTriggered = false;
        }

        if (buttonHoldTime > secondsToHoldButton)
        {
            if (calibrationTriggered) return;
            calibrationTriggered = true;
            print("calibrate VR Body");
        }



        if (calibrationTriggered || calibrateByEvent)
        {
            float sizeF = (ik.solver.spine.headTarget.position.y - ik.references.root.position.y) / ((ik.references.head.position.y) - ik.references.root.position.y);
            ik.references.root.localScale *= (sizeF * correctionFactor);

            calibrateByEvent = false;
        }




    }


    public void Calibrate()
    {
        print("calibrate");
        calibrateByEvent = true;
    }



}
