using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using static RootMotion.FinalIK.IKSolverVR;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class VRGhostControllerFinalIK : MonoBehaviour
{
    [SerializeField] XRVelocityTracker velocityTrackerLeft;
    [SerializeField] XRVelocityTracker velocityTrackerRight;

    [SerializeField] float speedThreshold = 0.1f;
    [SerializeField] float ikThreshold = 0f;

    [SerializeField] float headIkThreshold = 0f;
    [SerializeField] float smoothingAnimation = 0.01f;
    [SerializeField] float smoothingIKWeight = 0.01f;
    [SerializeField] float handSpeedFactor = 1.1f;

    [SerializeField] float maxStep = 0.2f;

    [SerializeField] bool adjustRotation = false;
    [SerializeField] Transform rotationTarget = null;

    Animator animator;

    float leftHandSpeed;
    float rightHandSpeed;

    [SerializeField] bool inverseBehaviour = false;

    [SerializeField] AimIK aimIK = null;



    VRIK vRIK;


    void Start()
    {
        animator = GetComponent<Animator>();
        vRIK = GetComponent<VRIK>();

         
    }


    void Update()
    {
        leftHandSpeed = velocityTrackerLeft.GetSpeed();
        rightHandSpeed = velocityTrackerRight.GetSpeed();

        

        SetIKPositionWeight();

        SetLocomotion();

        SetAimIK();

        // animator.SetBool("LeftArmMoving", leftHandSpeed > speedThreshold);
        // animator.SetBool("RightArmMoving", rightHandSpeed > speedThreshold);

        // float previousLeftArmSpeed = animator.GetFloat("LeftArm");
        // float previosRightArmSpeed = animator.GetFloat("RightArm");

        // animator.SetFloat("LeftArm", Mathf.Lerp(previousLeftArmSpeed, MapHandSpeed(leftHandSpeed, true), smoothingAnimation));
        // animator.SetFloat("RightArm", Mathf.Lerp(previosRightArmSpeed, MapHandSpeed(rightHandSpeed, true), smoothingAnimation));

        if(adjustRotation && rotationTarget != null) 
        {
            AdjustRotation();
        }
    }

    private void SetAimIK()
    {
        if (aimIK == null) return;

        aimIK.solver.IKPositionWeight = vRIK.solver.IKPositionWeight;
    }

    private void SetLocomotion()
    {
        vRIK.solver.locomotion.weight = vRIK.solver.IKPositionWeight;
    }

    private void SetIKPositionWeight()
    {
        if (leftHandSpeed > ikThreshold)
        {
            float previousWeight = vRIK.solver.leftArm.positionWeight;
            float previousWeightR = vRIK.solver.leftArm.rotationWeight;
            float mappedHandSpeed = MapHandSpeed(leftHandSpeed, false);

            vRIK.solver.leftArm.positionWeight = LerpWithMaxStep(previousWeight, mappedHandSpeed, smoothingIKWeight, maxStep);
            vRIK.solver.leftArm.rotationWeight = LerpWithMaxStep(previousWeightR, mappedHandSpeed, smoothingIKWeight, maxStep);
            
            //vRIK.solver.leftArm.positionWeight = Mathf.Lerp(previousWeight, 0f, smoothingAnimation);
            
        }

        else
        {
            float previousWeight = vRIK.solver.leftArm.positionWeight;
            float previousWeightR = vRIK.solver.leftArm.rotationWeight;

            vRIK.solver.leftArm.positionWeight = LerpWithMaxStep(previousWeight, DefaultIKWeight(), smoothingIKWeight, maxStep);
            vRIK.solver.leftArm.rotationWeight = LerpWithMaxStep(previousWeightR, DefaultIKWeight(), smoothingIKWeight, maxStep);
        }

        if (rightHandSpeed > ikThreshold)
        {

            float previousWeight = vRIK.solver.rightArm.positionWeight;
            float previousWeightR = vRIK.solver.rightArm.rotationWeight;

            float mappedHandSpeed = MapHandSpeed(rightHandSpeed, false);
            

            vRIK.solver.rightArm.positionWeight = LerpWithMaxStep(previousWeight, mappedHandSpeed, smoothingIKWeight, maxStep);
            vRIK.solver.rightArm.rotationWeight = LerpWithMaxStep(previousWeightR, mappedHandSpeed, smoothingIKWeight, maxStep);
            //vRIK.solver.rightArm.positionWeight = Mathf.Lerp(previousWeight, 0f, smoothingAnimation);
            
        }

        else
        {
            float previousWeight = vRIK.solver.rightArm.positionWeight;
            float previousWeightR = vRIK.solver.rightArm.rotationWeight;

            vRIK.solver.rightArm.positionWeight = LerpWithMaxStep(previousWeight, DefaultIKWeight(), smoothingIKWeight,maxStep);
            vRIK.solver.rightArm.rotationWeight = LerpWithMaxStep(previousWeightR, DefaultIKWeight(), smoothingIKWeight, maxStep);
        }


        if (rightHandSpeed + leftHandSpeed > headIkThreshold)
        {
            float previousWeight = vRIK.solver.spine.positionWeight;
            float previousWeightR = vRIK.solver.spine.rotationWeight;

            float mappedHandSpeed = MapHandSpeed(rightHandSpeed + leftHandSpeed, false);

            vRIK.solver.spine.positionWeight = LerpWithMaxStep(previousWeight, mappedHandSpeed, smoothingIKWeight, maxStep);
            vRIK.solver.spine.rotationWeight = LerpWithMaxStep( previousWeightR, mappedHandSpeed, smoothingIKWeight, maxStep);

        }

        else
        {
            float previousWeight = vRIK.solver.spine.positionWeight;
            float previousWeightR = vRIK.solver.spine.rotationWeight;

            vRIK.solver.spine.positionWeight = LerpWithMaxStep(previousWeight, DefaultIKWeight(), smoothingIKWeight, maxStep);
            vRIK.solver.spine.rotationWeight = LerpWithMaxStep (previousWeightR, DefaultIKWeight(), smoothingIKWeight, maxStep);
        }




                if (rightHandSpeed + leftHandSpeed > headIkThreshold)
        {
            float previousWeight = vRIK.solver.IKPositionWeight;


            float mappedHandSpeed = MapHandSpeed(rightHandSpeed + leftHandSpeed, false);

            vRIK.solver.IKPositionWeight = LerpWithMaxStep(previousWeight, mappedHandSpeed, smoothingIKWeight, maxStep);


        }

        else
        {
            float previousWeight = vRIK.solver.IKPositionWeight;
  

            vRIK.solver.IKPositionWeight = LerpWithMaxStep(previousWeight, DefaultIKWeight(), smoothingIKWeight, maxStep);
         
        }


    }

    private void AdjustRotation()
    {
        Vector3 direction = (rotationTarget.position - transform.position);
        Vector3 directionOnPlane = (Vector3.ProjectOnPlane(direction, Vector3.up)).normalized;
        transform.forward = Vector3.Lerp(transform.parent.forward, directionOnPlane, (1 - vRIK.solver.IKPositionWeight));
    }

    private float MapHandSpeed(float handSpeed, bool reinverse)
    {
        if (inverseBehaviour && !reinverse)
        {
            return Mathf.Clamp01(handSpeed * handSpeedFactor);
        }

        else
        {
            return 1 - Mathf.Clamp01(handSpeed * handSpeedFactor);
        }
        
    }

    private float DefaultIKWeight()
    {
        if (inverseBehaviour)
        {
            return 0;
        }

        else
        {
            return 1;
        }
    }

    private float LerpWithMaxStep(float value1, float value2, float t, float _maxStep)
    {
        float currentValue2;

        if (value1 > value2)
        {
            
            if(value1 + _maxStep > value2)
            {
                currentValue2 = value2;
            }

            else
            {
                currentValue2 = value1 + _maxStep;
            }
        }

        else
        {
            if(value1 - _maxStep < value2)
            {
                currentValue2 = value2;
            }

            else
            {
                currentValue2 = value1 - _maxStep;
            }
        }

              return Mathf.Lerp(value1, currentValue2, t);
    }
}
