using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.Looping
{

    public class XRLoopingRotator : MonoBehaviour
    {
        [SerializeField] XRNode turnHandJoystick;
        InputDevice turnDeviceJoystick;

        [SerializeField] float turnSpeed = 10f;

        XRLoopingMover loopingMover = null;



        void Start()
        {
            turnDeviceJoystick = InputDevices.GetDeviceAtXRNode(turnHandJoystick);
            loopingMover = GetComponent<XRLoopingMover>();
        }


        void Update()
        {
            if (turnDeviceJoystick.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 valueTurn) && valueTurn.magnitude > 0.01f)
            {
                print("turn");

                loopingMover.Rotate(valueTurn.x * turnSpeed * Time.deltaTime);

            }
        }




    }
}

