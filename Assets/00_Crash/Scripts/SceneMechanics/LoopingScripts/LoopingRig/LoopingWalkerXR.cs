using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


// TODO: get headset rotation

public class LoopingWalkerXR : MonoBehaviour
{
    [SerializeField] XRNode moveHandJoystick;
    [SerializeField] XRNode turnHandJoystick;

    [SerializeField] XRNode moveHandTriggerLeft;
    [SerializeField] XRNode moveHandTriggerRight;

    InputDevice moveDeviceJoystick;
    InputDevice turnDeviceJoystick;

    InputDevice moveDeviceTriggerLeft;
    InputDevice moveDeviceTriggerRight;

    [SerializeField] LoopingControllerForwardVector forwardControllerLeft = null;
    [SerializeField] LoopingControllerForwardVector forwardControllerRight = null;
    [SerializeField] LoopingControllerForwardVector forwardHead = null;

    

    [SerializeField] bool moveByJoystick = false;
    [SerializeField] bool moveByTriggerLeft = true;
    [SerializeField] bool moveByTriggerRight = true;

    [SerializeField] float speed = 0.1f;
    [SerializeField] float turnSpeed = 0.1f;
    // [SerializeField] float gravity = -9.81f;

    float fallingSpeed = -9.81f; // TODO: Implement acceleration of gravity

     float my_rotation = 0f;

    [SerializeField]  float correctForward = 90f;

    [SerializeField] float height = 1f;

    [SerializeField] float smoothing = 0.01f;

    [SerializeField] float additionalHeight = 0.2f;

    [SerializeField] XRRig rig;

    Vector3 previousForward = new Vector3();

    Vector3 currentNormal = new Vector3();

    
    CapsuleCollider capsule;

    
    

    

    // TODO Implement Beckett turn

    // [SerializeField] float beckettTurnValue = 90f;

    // float beckettTurn = 0f;
    // float newBeckettTurn = 0f;

    // bool triggerAlreadyPressed = false;
    // bool trigger2AlreadyPressed = false;




    

    
    void Start()
    {

        
        capsule = GetComponent<CapsuleCollider>();
        rig = GetComponent<XRRig>();

        
        moveDeviceJoystick = InputDevices.GetDeviceAtXRNode(moveHandJoystick);
        turnDeviceJoystick = InputDevices.GetDeviceAtXRNode(turnHandJoystick);

        moveDeviceTriggerLeft = InputDevices.GetDeviceAtXRNode(moveHandTriggerLeft);
        moveDeviceTriggerRight = InputDevices.GetDeviceAtXRNode(moveHandTriggerRight);


        currentNormal = GetNormal();
    }

    // Update is called once per frame
    void Update()
    {

        
        CapsuleFollowHeadset();

        RaycastHit currentHit = GetHitInfo();

        // rotate to normal
        
        transform.up = Vector3.Lerp(transform.up, currentHit.normal, smoothing);

        
        //correct forward
        

        transform.rotation *= Quaternion.Euler(0, correctForward, 0);

        //apply my rotation
        

        transform.rotation *= Quaternion.Euler(0, my_rotation, 0);

        // apply beckett turn

        // if (!Mathf.Approximately(beckettTurn, newBeckettTurn))
        // {
        //     beckettTurn = Mathf.Lerp(beckettTurn, newBeckettTurn, smoothing);
        // }

        // transform.rotation *= Quaternion.Euler(0, beckettTurn, 0);


        // Grounding
        if (GetDistanceFromGround() > height + 0.1f)
        {
            transform.position += transform.up * fallingSpeed * Time.deltaTime;
            print("fall");
        }

        if (GetDistanceFromGround() < height - 0.1f)
        {
            transform.position += transform.up * -fallingSpeed * Time.deltaTime;
            print("rise");
        }


        // process input

        if(moveByJoystick)
        {
            if (moveDeviceJoystick.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value) && value.magnitude > 0.01f)
            {
                print("move");
                transform.position += transform.forward * value.y * speed * Time.deltaTime;
                transform.position += transform.right * value.x * speed * Time.deltaTime;

            }

            if (turnDeviceJoystick.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 valueTurn) && valueTurn.magnitude > 0.01f)
            {
                print("turn");
                my_rotation += valueTurn.x * turnSpeed * Time.deltaTime;

            }

        }

        if(moveByTriggerLeft)
        {
            if (moveDeviceTriggerLeft.TryGetFeatureValue(CommonUsages.trigger, out float moveTriggerValueL) && moveTriggerValueL > 0.01f)
            {
                transform.position += GetControllerForwardDirection(forwardControllerLeft) * moveTriggerValueL * speed * Time.deltaTime;
            } 
            
        }

        if(moveByTriggerRight)
        {
            if(moveDeviceTriggerRight.TryGetFeatureValue(CommonUsages.trigger, out float moveTriggerValueR) && moveTriggerValueR > 0.01f)
            {
                transform.position += GetControllerForwardDirection(forwardControllerRight) * moveTriggerValueR * speed * Time.deltaTime;
            }
        }

        




        

        // Beckett Quadrat

        

        // if(turnDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerUsage) && triggerUsage)
        // {
        //     if(!triggerAlreadyPressed)
        //     {
        //         rig.MoveCameraToWorldLocation(transform.position);
        //         newBeckettTurn += beckettTurnValue;
        //         triggerAlreadyPressed = true;
        //         RecenterRig();

            
                
        //     }
        // }

        // else
        // {
        //     triggerAlreadyPressed = false;
        // }

        // if(moveDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerUsage2) && triggerUsage2)
        // {
        //     if(!trigger2AlreadyPressed)
        //     {
        //         rig.MoveCameraToWorldLocation(transform.position);
        //         newBeckettTurn -= beckettTurnValue;
        //         trigger2AlreadyPressed = true;
        //         print("trigger 2 pressed");
        //         RecenterRig();
        //     }


        // }

        // else
        // {
        //     trigger2AlreadyPressed = false;
        // }





    }

    private void FixedUpdate() 
    {
        // TODO Implement acceleration of gravity
        //gravity 

        // bool isGrounded = CheckIfGrounded();
        // if (isGrounded)
        //     fallingSpeed = 0;
        // else
        //     fallingSpeed += gravity * Time.fixedDeltaTime;
    }

    private Vector3 GetControllerForwardDirection(LoopingControllerForwardVector controller)
    {
        return controller.GetControllerForward();
    }

    
    void CapsuleFollowHeadset()
    {
        capsule.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        capsule.center = new Vector3(capsuleCenter.x, capsule.height / 2, capsuleCenter.z);
    }


    private void RecenterRig()
    {
        Vector3 captureCameraPosition = rig.cameraGameObject.transform.position;
        transform.position = captureCameraPosition;
        
        
    }

    private RaycastHit GetHitInfo()
    {
        Vector3 rayStart = transform.TransformPoint(capsule.center);
        Physics.SphereCast(rayStart,0.2f, -transform.up, out RaycastHit hitInfo);

        return hitInfo;
    }

    private Vector3 GetNormal()
    {
        Vector3 rayStart = transform.TransformPoint(capsule.center);
        Physics.SphereCast(rayStart, 0.2f, -transform.up, out RaycastHit hitInfo);

        currentNormal = hitInfo.normal;

        return currentNormal;
    }

    private Vector3 GetRotationPoint()
    {
        Vector3 rayStart = transform.TransformPoint(capsule.center);
        Physics.Raycast(rayStart, -transform.up, out RaycastHit hitInfo);

        return hitInfo.point;
    }

    private bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(capsule.center);
        bool hasHit;
        hasHit = Physics.SphereCast(rayStart, 0.2f, -transform.up, out RaycastHit hitInfo, height);

        return hasHit;

    }

    private float GetDistanceFromGround()
    {
        Vector3 rayStart = transform.TransformPoint(capsule.center);
        bool hasHit;
        hasHit = Physics.SphereCast(rayStart, 0.2f, -transform.up, out RaycastHit hitInfo);
        return hitInfo.distance;

    }

    // Gizmos

    // void OnDrawGizmos()
    // {
    //     // Draw a yellow sphere at the transform's position
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(GetHitInfo().point, 1);
    // }

    
}
