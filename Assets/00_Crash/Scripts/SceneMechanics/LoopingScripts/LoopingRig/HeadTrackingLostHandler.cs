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
    private bool isTrackingLost = false;

    bool isTracked;

    private void Start()
    {
        OVRManager.VrFocusLost += OnVRFocusLost;
        OVRManager.VrFocusAcquired += OnVRFocusAquired;

        // Set initial state of isTracked
        InitializeTrackingState();
    }

    private void InitializeTrackingState()
    {
        isTracked = OVRManager.hasVrFocus;

        if (isTracked)
        {
            onHeadsetTrackingAquired.Invoke();
        }
        else
        {
            onHeadsetTrackingLost.Invoke();
        }
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
}

    

