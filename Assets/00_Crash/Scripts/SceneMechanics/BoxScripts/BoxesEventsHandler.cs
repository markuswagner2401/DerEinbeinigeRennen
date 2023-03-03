using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.AnimatorSpace
{

    public class BoxesEventsHandler : MonoBehaviour
    {
        [SerializeField] BoxesAnimationControl animationController;
        [SerializeField] float maxThresholdState = 22f;
        [SerializeField] float minThresholdState = 1f;
        [SerializeField] int maxthresholdBreaksBeforeInvoke = 3;
        [SerializeField] int minThresholdBreaksBeforeInvoke = 3;

        [SerializeField] UnityEvent onMaxThresholdsBroken;
        [SerializeField] UnityEvent onMinThresholdsBroken;

        int currentMaxThresholdBreaks = 0;
        int currentMinThresholdBreaks = 0;

        float currentState;

        bool maxThresholdBroken = false;
        bool minThresholdBroken = false;

        void Start()
        {
            if (animationController == null)
            {
                animationController = GetComponent<BoxesAnimationControl>();
            }


        }


        void Update()
        {
            currentState = animationController.GetCurrentState();

            // handle max threshold

            if (currentState > maxThresholdState)
            {
                if (maxThresholdBroken) return;
                maxThresholdBroken = true;
                currentMaxThresholdBreaks += 1;
                print("max Threshold Broken x: " + currentMaxThresholdBreaks);
            }

            else
            {
                maxThresholdBroken = false;
            }

            if (currentMaxThresholdBreaks > maxthresholdBreaksBeforeInvoke)
            {
                print("Invoke Change");
                onMaxThresholdsBroken.Invoke();
            }

            // handle min threshold

            if (currentState < minThresholdState)
            {
                if (minThresholdBroken) return;
                minThresholdBroken = true;
                currentMinThresholdBreaks += 1;

            }

            else
            {
                minThresholdBroken = false;
            }

            if (currentMinThresholdBreaks > minThresholdBreaksBeforeInvoke)
            {
                onMinThresholdsBroken.Invoke();
            }


        }
    }

}
