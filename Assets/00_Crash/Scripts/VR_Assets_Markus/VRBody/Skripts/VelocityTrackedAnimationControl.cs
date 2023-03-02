using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrackedAnimationControl : MonoBehaviour
{
    [SerializeField] XRVelocityTracker velocityTrackerLeft = null;
    [SerializeField] XRVelocityTracker velocityTrackerRight = null;

    [SerializeField] float highestHandSpeed = 2f;

    [SerializeField] float animMaxSpeed = 1f;
    [SerializeField] float animMinSpeed = 0f;

    [SerializeField] float smoothing = 0.1f;

    [SerializeField] bool mapOnCurve = true;

    [SerializeField] AnimationCurve mappingCurve = null;

    AnimatorClipInfo[] animatorClipInfos;

    bool reverseClip = false;




    float clipLength;
    float rawClipPosition = 0;


    float smoothedSpeed;

    float previousSpeed;

    

    [SerializeField] Animator animator = null;

    [SerializeField] string parameterName;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        animatorClipInfos = animator.GetCurrentAnimatorClipInfo(1);

        clipLength = animatorClipInfos[0].clip.length;
        

        


        
    }

    public void SetVelocityTrackers(XRVelocityTracker leftTracker, XRVelocityTracker rightTracker)
    {
        
    }


    void Update()
    {
        bool controlling = (velocityTrackerLeft != null && velocityTrackerRight != null);

        if(controlling)
        {
            smoothedSpeed = SmoothedMappedSpeed(velocityTrackerLeft.GetSpeed(), velocityTrackerRight.GetSpeed());
        }

        rawClipPosition += Time.deltaTime * smoothedSpeed;

        bool reverse = Reverse(rawClipPosition, clipLength);

        float newSpeed = Reverse(rawClipPosition, clipLength) ? - smoothedSpeed : smoothedSpeed;

        

        


        // if(rawClipPosition <= 0 && smoothedSpeed < 0)
        // {
            
        //     print("set animator 0");
        //     animator.SetFloat(parameterName, 0);
        //     return; 
            

        // } 

        

        

        animator.SetFloat(parameterName, newSpeed);

        



        


    }

    private bool Reverse(float rawClipPosition, float clipLength)
    {
        float x = Mathf.Floor(rawClipPosition / clipLength);
        return !((x % 2) == 0);
    }

    public void SetAnimationSpeed(float speed)
    {
        smoothedSpeed = speed;
        animator.SetFloat(parameterName, smoothedSpeed);

    }

    float SmoothedMappedSpeed(float leftSpeed, float rightSpeed)
    {
        float mappedSpeed = mapOnCurve ? MappedSpeedOnCurve(leftSpeed + rightSpeed, mappingCurve) : MappedSpeed(leftSpeed + rightSpeed);
        float smoothedSpeed = Mathf.Lerp(previousSpeed, mappedSpeed, smoothing);

        previousSpeed = smoothedSpeed;

        return smoothedSpeed;
    }

    private float MappedSpeed(float speedSum)
    {
        float speedNorm = Mathf.InverseLerp(0, highestHandSpeed*2, speedSum);
        return Mathf.Lerp(animMinSpeed, animMaxSpeed, speedNorm);
    }

    private float MappedSpeedOnCurve(float speedSum, AnimationCurve curve)
    {
        float speedNorm = Mathf.InverseLerp(0, highestHandSpeed*2f, speedSum);
        float mappedOnCurve = curve.Evaluate(speedNorm);
        return Mathf.Lerp(animMinSpeed, animMaxSpeed, mappedOnCurve);
    }
}
