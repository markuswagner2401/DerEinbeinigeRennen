using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace ObliqueSenastions.VRRigSpace
{

    public class XRInputHandler : MonoBehaviour
    {
        [SerializeField] NodeHandler[] inputHandler;

        [System.Serializable]
        struct NodeHandler
        {
            public XRNode handNode;
            public InputDevice device;
            public bool secondaryButton;
            public bool alreadyPressed;
            public UnityEvent eventToInvoke;

        }



        void Start()
        {
            for (int i = 0; i < inputHandler.Length; i++)
            {
                inputHandler[i].device = InputDevices.GetDeviceAtXRNode(inputHandler[i].handNode);
                inputHandler[i].alreadyPressed = false;
            }


        }


        void Update()
        {
            for (int i = 0; i < inputHandler.Length; i++)
            {
                if (inputHandler[i].secondaryButton)
                {
                    if (inputHandler[i].device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool buttonUsage) && buttonUsage)
                    {
                        if (inputHandler[i].alreadyPressed) return;

                        inputHandler[i].alreadyPressed = true;

                        inputHandler[i].eventToInvoke.Invoke();
                    }

                    else
                    {
                        inputHandler[i].alreadyPressed = false;
                    }
                }

                else
                {
                    if (inputHandler[i].device.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonUsage) && buttonUsage)
                    {
                        if (inputHandler[i].alreadyPressed) return;

                        inputHandler[i].alreadyPressed = true;

                        inputHandler[i].eventToInvoke.Invoke();
                    }

                    else
                    {
                        inputHandler[i].alreadyPressed = false;
                    }
                }

            }



        }
    }

}
