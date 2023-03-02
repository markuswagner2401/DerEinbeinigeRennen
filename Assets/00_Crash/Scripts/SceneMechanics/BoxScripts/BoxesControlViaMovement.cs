using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesControlViaMovement : MonoBehaviour
{
    [SerializeField] XRVelocityTracker rightVelocityTracker;
    [SerializeField] XRVelocityTracker leftVelocityTracker;

    [SerializeField] BoxesAnimationControl boxesAnimationControl;

    [SerializeField] float defaultState = 0f;
    [SerializeField] float speedThreshold = 0.3f;
    [SerializeField] float speedFactorActive = 10f;
    [SerializeField] float speedFactorPassive = 10f;

    [SerializeField] float smoothing = 0.1f;

    float currentState;

    float previousSpeedL;
    float previousSpeedR;

    void Start()
    {
        currentState = defaultState;
    }

    
    void Update()
    {
        float leftHandSpeed;
        float rightHandSpeed;

        
        
        leftHandSpeed = Mathf.Lerp(previousSpeedL, leftVelocityTracker.GetSpeed(), smoothing);
        rightHandSpeed = Mathf.Lerp(previousSpeedR, rightVelocityTracker.GetSpeed(), smoothing);

        previousSpeedL = leftHandSpeed;
        previousSpeedR = leftHandSpeed;

        CalculateState(leftHandSpeed, rightHandSpeed);
        
        boxesAnimationControl.SetControlledByOther(true);

        boxesAnimationControl.SetCurrentState(currentState);


    }

    private void CalculateState(float speedL, float speedR)
    {
        if(speedL + speedR > speedThreshold)
        {
            currentState += (speedL + speedR) * Time.deltaTime * speedFactorActive;
            //currentState = Mathf.Lerp(currentState, newState, smoothing);
        }

        else
        {
            
            if(currentState > defaultState)
            {
                currentState -= Time.deltaTime * speedFactorPassive;
                //currentState = Mathf.Lerp(currentState, newState, smoothing);
            }
        }
        
    }
}
