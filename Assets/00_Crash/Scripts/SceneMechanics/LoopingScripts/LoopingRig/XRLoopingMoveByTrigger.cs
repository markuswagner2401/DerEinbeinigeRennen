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

        [SerializeField] OVRHand leftHand = null;

        [SerializeField] OVRHand rightHand = null;

        [SerializeField] float increaseValueOnPinch = 0.01f;

        [SerializeField] float maxSpeedOnPinch = 1f;

        [SerializeField] bool moveByTriggerLeft = true;
        [SerializeField] bool moveByTriggerRight = true;

        [SerializeField] LoopingControllerForwardVector forwardControllerLeft = null;
        [SerializeField] LoopingControllerForwardVector forwardControllerRight = null;

        [SerializeField] LoopingControllerForwardVector forwardHandLeft = null;

        [SerializeField] LoopingControllerForwardVector forwardHandRight = null;
        [SerializeField] LoopingControllerForwardVector forwardHead = null;



        [SerializeField] float speed = 10f;

        InputDevice moveDeviceTriggerLeft;
        InputDevice moveDeviceTriggerRight;

        XRLoopingMover loopingMover;

        bool devicesSet = false;

        float moveTriggerValueL;

        float moveTriggerValueR;

        Vector3 directionLeftController = new Vector3();

        Vector3 directionRightController = new Vector3();




        void Start()
        {
            moveDeviceTriggerLeft = InputDevices.GetDeviceAtXRNode(moveHandTriggerLeft);
            moveDeviceTriggerRight = InputDevices.GetDeviceAtXRNode(moveHandTriggerRight);

            loopingMover = GetComponent<XRLoopingMover>();

            if(leftHand == null)
            {
                leftHand = GameObject.FindWithTag("LeftOVRHand").GetComponent<OVRHand>();
            }

            if(rightHand == null)
            {
                rightHand = GameObject.FindWithTag("RightOVRHand").GetComponent<OVRHand>();
            }

            
            

        }


        void FixedUpdate()
        {
            if (usingOVR)
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
                if (moveDeviceTriggerLeft.TryGetFeatureValue(CommonUsages.trigger, out moveTriggerValueL) && moveTriggerValueL > 0.01f)
                {
                    loopingMover.Move(GetControllerForwardDirection(forwardControllerLeft) * moveTriggerValueL * speed * Time.deltaTime);

                    //transform.position += GetControllerForwardDirection(forwardControllerLeft) * moveTriggerValueL * speed * Time.deltaTime;
                }

            }

            if (moveByTriggerRight)
            {
                if (moveDeviceTriggerRight.TryGetFeatureValue(CommonUsages.trigger, out moveTriggerValueR) && moveTriggerValueR > 0.01f)
                {
                    loopingMover.Move(GetControllerForwardDirection(forwardControllerRight) * moveTriggerValueR * speed * Time.deltaTime);

                    //transform.position += GetControllerForwardDirection(forwardControllerRight) * moveTriggerValueR * speed * Time.deltaTime;
                }
            }


        }

        

        private void ProcessOVRInput()
        {



            if (OVRInput.GetActiveController() == OVRInput.Controller.Hands)
            {
                bool leftIsPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
                bool rightIsPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);


                if (leftIsPinching)
                {
                    moveTriggerValueL += increaseValueOnPinch;
                    directionLeftController = GetControllerForwardDirection(forwardHandLeft);
                    
                }

                else
                {
                    moveTriggerValueL = 0f;
                }

                if (rightIsPinching)
                {
                    moveTriggerValueR += increaseValueOnPinch;
                    directionRightController = GetControllerForwardDirection(forwardHandRight);
                }

                else
                {
                    moveTriggerValueR = 0f;
                }


            }

            else
            {
                moveTriggerValueL = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
                moveTriggerValueR = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
                directionLeftController = GetControllerForwardDirection(forwardControllerLeft);
                directionRightController = GetControllerForwardDirection(forwardControllerRight);
            }


            if (moveByTriggerLeft)
            {
                
                loopingMover.Move(directionLeftController * moveTriggerValueL  * speed * Time.deltaTime);
            }

            if (moveByTriggerRight)
            {

                loopingMover.Move(directionRightController * moveTriggerValueR * speed * Time.deltaTime);
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
