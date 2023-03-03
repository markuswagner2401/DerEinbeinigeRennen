using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.RCCControl
{

    public class XRCarController : MonoBehaviour
    {

        // for custom xr Input

        [SerializeField] XRNode nodeThrottle;
        InputDevice throttleDevice;
        float throttleInput;

        [SerializeField] XRNode nodeBreak;
        InputDevice breakDevice;
        float breakInput;

        [SerializeField] bool steeringWithJoystick = false;
        [SerializeField] XRNode nodeSteering;
        InputDevice steeringDevice;
        float steeringInput;

        [SerializeField] bool steeringWithSteeringWheel = true;
        [SerializeField] HingeJoint steeringWheel = null;

        [SerializeField] float drosselFaktor = 1f;




        private void Start()
        {
            throttleDevice = InputDevices.GetDeviceAtXRNode(nodeThrottle);
            breakDevice = InputDevices.GetDeviceAtXRNode(nodeBreak);
            steeringDevice = InputDevices.GetDeviceAtXRNode(nodeSteering);
        }

        public void SetDrosselfaktor(float newDrosselfaktor)
        {
            drosselFaktor = newDrosselfaktor;
        }



        void Update()
        {
            // steering

            if (steeringWithJoystick)
            {
                if (steeringDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 steeringVector))
                {
                    steeringInput = steeringVector.x;
                }
            }

            if (steeringWithSteeringWheel)
            {
                float steeringNormalized = Mathf.InverseLerp(steeringWheel.limits.min, steeringWheel.limits.max, steeringWheel.angle);

                steeringInput = Mathf.Lerp(1, -1, steeringNormalized);


            }



            if (throttleDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                throttleInput = triggerValue;
            }

            if (breakDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue2))
            {
                breakInput = triggerValue2;
            }

            // RCC_InputManager.SetSteeringInput(steeringInput);
            // RCC_InputManager.SetBreakInput(breakInput);
            // RCC_InputManager.SetThrottleInput(throttleInput* drosselFaktor);

        }
    }

}
