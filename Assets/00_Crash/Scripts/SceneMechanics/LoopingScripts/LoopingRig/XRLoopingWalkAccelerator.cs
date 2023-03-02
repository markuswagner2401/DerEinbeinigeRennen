using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRLoopingWalkAccelerator : MonoBehaviour
{
    [SerializeField] LoopingControllerForwardVector forwardHead = null;
    [SerializeField] Transform headVelForwardObject = null;
    [SerializeField] XROrigin rig;


    [SerializeField] float smoothing = 0.1f;

    [SerializeField] float moveFactor = 10f;

    [SerializeField] bool directionByHeadVelocity = false;

    [SerializeField] bool invertVelocityDirection = false; // TODO: Elegantere Lösung mit invert position oder invert direction

    XRLoopingMover loopingMover;

    Vector3 headPosition = new Vector3();
    Vector3 previousHeadPosition = new Vector3();

    Vector3 headVelocity = new Vector3();

    float headSpeed;

    


    void Start()
    {
        
        rig = GetComponent<XROrigin>();

        headPosition = rig.Camera.transform.position;

        previousHeadPosition = headPosition;
        

        loopingMover = GetComponent<XRLoopingMover>();

        


    }

    
    void Update()
    {
        headVelocity = CalculateHeadVelocity();

        if (directionByHeadVelocity && headVelForwardObject != null)
        {
            headVelForwardObject.forward = headVelocity;

            if(invertVelocityDirection) // TODO elegantere Lösung
            {
                headVelocity *= -1;
            }
            
            headVelForwardObject.localEulerAngles = new Vector3(0, headVelForwardObject.localEulerAngles.y, 0);

            
                print("move by velocity");
                loopingMover.Move(headVelForwardObject.forward * headVelocity.magnitude * moveFactor * Time.deltaTime);
            
            
        }

        
        else
        {
            print("move by direction");
            loopingMover.Move(forwardHead.GetControllerForward() * headVelocity.magnitude * moveFactor * Time.deltaTime);
        }

        
    
    }

    private Vector3 CalculateHeadVelocity()
    {
        Vector3 headVelocity = new Vector3();

        headPosition = rig.CameraInOriginSpacePos;

        // Vector3 localHeadPosition = transform.InverseTransformPoint(headPosition);

        // headPosition = rig.cameraGameObject.transform.position;

        

        headVelocity = headPosition - previousHeadPosition;

        previousHeadPosition = headPosition;

        return headVelocity;

        

        

    }
}
