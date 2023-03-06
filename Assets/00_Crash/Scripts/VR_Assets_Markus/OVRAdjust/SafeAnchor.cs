using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObliqueSensations.OVRRigSpace
{
    public class SafeAnchor : MonoBehaviour
    {
        [SerializeField] Vector3 offsetHandRotation;
        [SerializeField] Vector3 offsetControllerPosition;
        [SerializeField] Transform source;
        [Tooltip("leave empty if is head")]
        [SerializeField] OVRHand hand = null;

        Vector3 currentPosition = new Vector3();
        Quaternion currentRotation = new Quaternion();

        Vector3 lastSafePosition = new Vector3();
        Quaternion lastSafeRotation = new Quaternion();


        void Start()
        {
            currentPosition = source.position;
            currentRotation = source.rotation;
            lastSafePosition = currentPosition;
            lastSafeRotation = currentRotation;
        }


        void Update()
        {
            // head

            if (hand == null)
            {
                currentPosition = source.position;
                currentRotation = source.rotation;
                lastSafePosition = currentPosition;
                lastSafeRotation = currentRotation;

                transform.position = currentPosition;
                transform.rotation = currentRotation;

                return;
            }

            // controllers

            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
            {

                currentPosition = source.position;
                currentRotation = source.rotation;
                lastSafePosition = currentPosition;
                lastSafeRotation = currentRotation;

                transform.position = currentPosition;
                transform.Translate(offsetControllerPosition, Space.Self);
                transform.rotation = currentRotation;

            }

            // hands

            else
            {
                if (hand != null)
                {
                    if (hand.IsDataHighConfidence)
                    {

                        currentPosition = source.position;
                        currentRotation = source.rotation;
                        lastSafePosition = currentPosition;
                        lastSafeRotation = currentRotation;
                    }

                    else
                    {

                        currentPosition = lastSafePosition;
                        currentRotation = lastSafeRotation;
                    }

                    transform.position = currentPosition;
                    
                    
                    transform.rotation = currentRotation;
                    transform.Rotate(offsetHandRotation, Space.Self);
                    return;

                }

                transform.position = currentPosition;
                transform.rotation = currentRotation;

            }


        }
    }

}

