using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace ObliqueSenastions.UISpace
{

    public class UIActivator : MonoBehaviour
    {

        [SerializeField] XRNode node;
        [SerializeField] bool useController = true;
        InputDevice device;

        [SerializeField] GameObject[] uiObjects;

        bool alreadyPressed;

        bool activateUI = false;

        void Start()
        {
            device = InputDevices.GetDeviceAtXRNode(node);

            SetActivation(activateUI);
        }


        void Update()
        {
            if (!useController) return;
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonUsage) && buttonUsage)
            {
                if (alreadyPressed) return;
                alreadyPressed = true;

                activateUI = !activateUI;

                SetActivation(activateUI);
            }

            else
            {
                alreadyPressed = false;
            }
        }

        public void SetActivation(bool value)
        {
            foreach (var item in uiObjects)
            {
                item.SetActive(value);
            }
        }
    }

}
