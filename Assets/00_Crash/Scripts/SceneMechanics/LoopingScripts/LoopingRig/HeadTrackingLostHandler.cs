using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class HeadTrackingLostHandler : MonoBehaviour
{

    [SerializeField] UnityEvent onHeadsetTrackingLost;
    [SerializeField] UnityEvent onHeadsetTrackingAquired;
    private XRNodeState headsetState;
    private bool isTrackingLost = false;

    bool isTracked;






    InputDevice headDevice;

    private void Start()
    {
        OVRManager.VrFocusLost += OnVRFocusLost;
        OVRManager.VrFocusAcquired += OnVRFocusAquired;
        // isTrackingLost = OVRManager.hasVrFocus;
        // if(isTrackingLost)
        // {
        //     onHeadsetTrackingLost.Invoke();
        // }

        // else
        // {
        //     onHeadsetTrackingAquired.Invoke();
        // }
    }

    private void OnVRFocusAquired()
    {
        onHeadsetTrackingAquired.Invoke();
        isTracked = true;
    }

    private void OnVRFocusLost()
    {
        onHeadsetTrackingLost.Invoke();
        isTracked = false;
    }

    // private void Update()
    // {

    //     // headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
    //     // headDevice.TryGetFeatureValue(CommonUsages.isTracked, out isTracked);

    //     OVRPose headPose;
    //     bool poseValid = OVRManager.VrFocusLost
    //     //isTracked = headDevice.isValid;


    //     if (!isTracked && !isTrackingLost)
    //     {
    //         isTrackingLost = true;
    //         OnHeadsetTrackingLost();
    //     }
    //     else if (isTracked && isTrackingLost)
    //     {
    //         isTrackingLost = false;
    //         OnHeadsetTrackingAcquired();
    //     }
    // }

    // private void OnHeadsetTrackingLost()
    // {
    //     Debug.Log("Headset tracking lost");
    //     onHeadsetTrackingLost.Invoke();

    // }

    // private void OnHeadsetTrackingAcquired()
    // {
    //     Debug.Log("Headset tracking acquired");
    //     onHeadsetTrackingAquired.Invoke();

    // }
}
