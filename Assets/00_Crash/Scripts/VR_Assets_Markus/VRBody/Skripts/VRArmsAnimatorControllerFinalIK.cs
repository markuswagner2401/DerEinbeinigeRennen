using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using static RootMotion.FinalIK.IKSolverVR;
using System;

public class VRArmsAnimatorControllerFinalIK : MonoBehaviour
{
    [SerializeField] XRVelocityTracker velocityTrackerLeft;
    [SerializeField] XRVelocityTracker velocityTrackerRight;

    // [SerializeField] float speedThreshold = 0.1f;
    // [SerializeField] float ikThreshold = 0f;
    
    // [SerializeField] float handSpeedFactor = 1.1f;

    //[SerializeField] float handSpeedFactorLiniear = 0.01f;



    Animator animator;

    float leftHandSpeed;
    float rightHandSpeed;

    [SerializeField] bool inverseBehaviour = false;

    [SerializeField] float changeValueMin = -0.01f;
    [SerializeField] float changeValueMax = 0.01f;

    [SerializeField] float handSpeedMin = 0;
    [SerializeField] float handSpeedMax = 2f;

    [SerializeField] bool mapOnCurve = false;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float smoothing = 0.1f;
    



    VRIK vRIK;


    void Start()
    {
        animator = GetComponent<Animator>();
        vRIK = GetComponent<VRIK>();

        if(inverseBehaviour)
        {
            changeValueMin *= -1;
            changeValueMax *= -1;
        }

         
    }


    void Update()
    {
        leftHandSpeed = velocityTrackerLeft.GetSpeed();
        rightHandSpeed = velocityTrackerRight.GetSpeed();

        

        // SetIKPositionWeight();
        if(!mapOnCurve)
        {
            ChangeIKPositionWeight();
        }

        if(mapOnCurve)
        {
            MapIKPositionWeight();
        }



    }

    private void MapIKPositionWeight()
    {
        vRIK.solver.leftArm.positionWeight = Mathf.Lerp(vRIK.solver.leftArm.positionWeight, MapOnCurve(leftHandSpeed), smoothing);
        vRIK.solver.leftArm.rotationWeight = Mathf.Lerp(vRIK.solver.leftArm.rotationWeight, MapOnCurve(leftHandSpeed), smoothing);

        vRIK.solver.rightArm.positionWeight = Mathf.Lerp(vRIK.solver.rightArm.positionWeight, MapOnCurve(rightHandSpeed), smoothing);
        vRIK.solver.rightArm.rotationWeight = Mathf.Lerp(vRIK.solver.rightArm.rotationWeight, MapOnCurve(rightHandSpeed), smoothing);
    }

    private float MapOnCurve(float handSpeed)
    {
        float handSpeedNorm = Mathf.InverseLerp(handSpeedMin, handSpeedMax, handSpeed);
        return curve.Evaluate(handSpeedNorm);

    }


    private void ChangeIKPositionWeight()
    {
        if(IsBetween0And1(vRIK.solver.leftArm.positionWeight))
        {

            vRIK.solver.leftArm.positionWeight = Mathf.Clamp01(vRIK.solver.leftArm.positionWeight + MapChangeValue(leftHandSpeed));
             
            
        }

        if(IsBetween0And1(vRIK.solver.leftArm.rotationWeight))
        {
            vRIK.solver.leftArm.rotationWeight = Mathf.Clamp01(vRIK.solver.leftArm.rotationWeight + MapChangeValue(leftHandSpeed)) ;
        }

        if(IsBetween0And1(vRIK.solver.rightArm.positionWeight))
        {
            vRIK.solver.rightArm.positionWeight = Mathf.Clamp01(vRIK.solver.rightArm.positionWeight + MapChangeValue(rightHandSpeed) );
        }

        if(IsBetween0And1(vRIK.solver.rightArm.rotationWeight))
        {
            vRIK.solver.rightArm.rotationWeight = Mathf.Clamp01(vRIK.solver.rightArm.rotationWeight + MapChangeValue(rightHandSpeed)) ;
        }
        
    }

    private bool IsBetween0And1(float value)
    {
        if ( value >= -0.01 && value <= 1.01)
        {
            return true;
        }

        else{
            return false;
        }
    }


    private float MapChangeValue(float handSpeed)
    {
        float handSpeedNorm = Mathf.InverseLerp(handSpeedMin, handSpeedMax, handSpeed);
        return Mathf.Lerp(changeValueMin, changeValueMax, handSpeedNorm);
    }





    // private void SetIKPositionWeight()
    // {
    //     if (leftHandSpeed > ikThreshold)
    //     {
    //         // float previousWeight = vRIK.solver.leftArm.positionWeight;
    //         // float previousWeightR = vRIK.solver.leftArm.rotationWeight;
    //         // float mappedHandSpeed = MapHandSpeed(leftHandSpeed, false);
    //         // vRIK.solver.leftArm.positionWeight = Mathf.Lerp(previousWeight, mappedHandSpeed, smoothingAnimation);
    //         // vRIK.solver.leftArm.rotationWeight = Mathf.Lerp(previousWeightR, mappedHandSpeed, smoothingIKWeight);

    //         if (vRIK.solver.leftArm.positionWeight < 1f)
    //         {
    //             vRIK.solver.leftArm.positionWeight += leftHandSpeed * handSpeedFactorLiniear;
    //             vRIK.solver.leftArm.rotationWeight += leftHandSpeed * handSpeedFactorLiniear;
    //         }
            


            
    //     }

    //     else
    //     {
    //         float previousWeight = vRIK.solver.leftArm.positionWeight;
    //         float previousWeightR = vRIK.solver.leftArm.rotationWeight;

    //         vRIK.solver.leftArm.positionWeight = Mathf.Lerp(previousWeight, 1f, smoothingPosition);
    //         vRIK.solver.leftArm.rotationWeight = Mathf.Lerp(previousWeightR, 1f, smoothingRotation);
    //     }

    //     if (rightHandSpeed > ikThreshold)
    //     {

    //         // float previousWeight = vRIK.solver.rightArm.positionWeight;
    //         // float previousWeightR = vRIK.solver.rightArm.rotationWeight;

    //         // float mappedHandSpeed = MapHandSpeed(rightHandSpeed, false);
            

    //         // vRIK.solver.rightArm.positionWeight = Mathf.Lerp(previousWeight, mappedHandSpeed, smoothingPosition);
    //         // vRIK.solver.rightArm.rotationWeight = Mathf.Lerp(previousWeightR, mappedHandSpeed, smoothingRotation);

    //         if(vRIK.solver.rightArm.positionWeight < 1f)
    //         {
    //             vRIK.solver.rightArm.positionWeight += rightHandSpeed * handSpeedFactorLiniear;
    //             vRIK.solver.rightArm.rotationWeight += rightHandSpeed * handSpeedFactorLiniear;
    //         }
            
            
    //     }

    //     else
    //     {
    //         float previousWeight = vRIK.solver.rightArm.positionWeight;
    //         float previousWeightR = vRIK.solver.rightArm.rotationWeight;

    //         vRIK.solver.rightArm.positionWeight = Mathf.Lerp(previousWeight, 1f, smoothingPosition);
    //         vRIK.solver.rightArm.rotationWeight = Mathf.Lerp(previousWeightR, 1f, smoothingRotation);
    //     }
    // }

    // private float MapHandSpeed(float handSpeed, bool reinverse)
    // {
    //     if (inverseBehaviour && !reinverse)
    //     {
    //         return Mathf.Clamp01(handSpeed * handSpeedFactor);
    //     }

    //     else
    //     {
    //         return 1 - Mathf.Clamp01(handSpeed * handSpeedFactor);
    //     }
        
    // }

    
}
