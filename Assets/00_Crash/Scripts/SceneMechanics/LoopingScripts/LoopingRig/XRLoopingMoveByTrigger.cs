using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.Looping
{

    public class XRLoopingMoveByTrigger : MonoBehaviour
    {
        [SerializeField] XRNode moveHandTriggerLeft;
        [SerializeField] XRNode moveHandTriggerRight;

        [SerializeField] bool usingOVR = false;

        [SerializeField] bool moveByTriggerLeft = true;
        [SerializeField] bool moveByTriggerRight = true;

        [SerializeField] LoopingControllerForwardVector forwardControllerLeft = null;
        [SerializeField] LoopingControllerForwardVector forwardControllerRight = null;
        [SerializeField] LoopingControllerForwardVector forwardHead = null;



        [SerializeField] float speed = 10f;

        InputDevice moveDeviceTriggerLeft;
        InputDevice moveDeviceTriggerRight;

        XRLoopingMover loopingMover;

        bool devicesSet = false;




        void Start()
        {
            moveDeviceTriggerLeft = InputDevices.GetDeviceAtXRNode(moveHandTriggerLeft);
            moveDeviceTriggerRight = InputDevices.GetDeviceAtXRNode(moveHandTriggerRight);

            loopingMover = GetComponent<XRLoopingMover>();

        }


        void FixedUpdate()
        {
            if(usingOVR)
            {
                OVRInput.FixedUpdate();
                ProcessOVRInput();
                
                return;
            }

            if (!devicesSet)
            {
                moveDeviceTriggerLeft = InputDevices.GetDeviceAtXRNode(moveHandTriggerLeft);
                moveDeviceTriggerRight = InputDevices.GetDeviceAtXRNode(moveHandTriggerRight);
                if (moveDeviceTriggerLeft.isValid && moveDeviceTriggerRight.isValid)
                {
                    devicesSet = true;
                }
            }

            if (moveByTriggerLeft)
            {
                if (moveDeviceTriggerLeft.TryGetFeatureValue(CommonUsages.trigger, out float moveTriggerValueL) && moveTriggerValueL > 0.01f)
                {
                    loopingMover.Move(GetControllerForwardDirection(forwardControllerLeft) * moveTriggerValueL * speed * Time.deltaTime);

                    //transform.position += GetControllerForwardDirection(forwardControllerLeft) * moveTriggerValueL * speed * Time.deltaTime;
                }

            }

            if (moveByTriggerRight)
            {
                if (moveDeviceTriggerRight.TryGetFeatureValue(CommonUsages.trigger, out float moveTriggerValueR) && moveTriggerValueR > 0.01f)
                {
                    loopingMover.Move(GetControllerForwardDirection(forwardControllerRight) * moveTriggerValueR * speed * Time.deltaTime);

                    //transform.position += GetControllerForwardDirection(forwardControllerRight) * moveTriggerValueR * speed * Time.deltaTime;
                }
            }


        }

        private void ProcessOVRInput()
        {
            print("process ofr input");
            if(moveByTriggerLeft)
            {
                print(OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger));
                // print("moveTriggerValue: " + moveTriggerValueL);
                loopingMover.Move(GetControllerForwardDirection(forwardControllerLeft) * OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) * speed * Time.deltaTime);
            }

            if(moveByTriggerRight)
            {
                
                loopingMover.Move(GetControllerForwardDirection(forwardControllerLeft) * OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) * speed * Time.deltaTime);
            }
        }

        private Vector3 GetControllerForwardDirection(LoopingControllerForwardVector controller)
        {
            return controller.GetControllerForward();
        }

        public void Move(Vector3 velocity)
        {
            transform.position += velocity * (Time.deltaTime * 72f);
            //print("move");
            

        }


    }

}
