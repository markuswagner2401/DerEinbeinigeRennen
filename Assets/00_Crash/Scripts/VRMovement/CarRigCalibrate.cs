using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using RPG.Saving;

public class CarRigCalibrate : MonoBehaviour, ISaveable
{
    [SerializeField] XRRig rig;
    [SerializeField] Transform controllerLeft;
    [SerializeField] Transform controllerRight;
    [SerializeField] Transform steeringWheelReferencePos;
    [SerializeField] float adjustingTime = 1f;
 
    [SerializeField] XRNode hand;
    InputDevice device;

    bool alreadyPressed = false;
    
    [SerializeField] int counts = 3;

    Vector3 capturedPointBeweenHands = new Vector3();
    
    // hello
   
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(hand);
    }

    
    void Update()
    {  
        if(counts < 1) return;

        if(device.TryGetFeatureValue(CommonUsages.gripButton, out bool usage) && usage)
        {
            if (alreadyPressed) return;
            Calibrate();
            counts -= 1;
            alreadyPressed = true;
        }

        else
        {
            alreadyPressed = false;
        }
    }

    public void Calibrate()
    {
        capturedPointBeweenHands = SetPointBeweenHands();
        StartCoroutine(AdjustHight(capturedPointBeweenHands));
    }


    public void EnableCalibrate(int value)
    {
        counts = value;
    }


    private Vector3 SetPointBeweenHands()
    {
        return Vector3.Lerp(controllerLeft.position, controllerRight.position, 0.5f);
    }
 

    IEnumerator AdjustHight(Vector3 pointBetweenHands)
    {
        
        Vector3 offset = pointBetweenHands - rig.transform.position;

        float time = 0f;
        while (time <= adjustingTime)
        {
            rig.transform.position = Vector3.Lerp(pointBetweenHands, steeringWheelReferencePos.position, time / adjustingTime) - offset;
            time += Time.deltaTime;
            yield return null;
        }

        yield break;
    }

    //Interface ISavable

    public object CaptureState()
    {
        return new SerializableVector3(transform.localPosition);
    }

    public void RestoreState(object state)
    {
        SerializableVector3 savedPosition =  (SerializableVector3)state;
        transform.localPosition = savedPosition.ToVector();
    }
}
