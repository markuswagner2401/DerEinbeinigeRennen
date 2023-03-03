using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.VRRigSpace;
using UnityEngine;

namespace ObliqueSenastions.Looping
{

    public class XRLoopingMoveWithArmSwing : MonoBehaviour
    {
        [SerializeField] bool armSwingRight = true;
        [SerializeField] bool armSwingLeft = true;
        [SerializeField] float speedFactor = 10f;
        [SerializeField] AnimationCurve speedChangeCurve;
        [SerializeField] float speedChangeTime = 5f;
        [SerializeField] float threshold = .1f;

        [SerializeField] XRVelocityTracker velocityTrackerRight = null;
        [SerializeField] XRVelocityTracker velocityTrackerLeft = null;

        [SerializeField] LoopingControllerForwardVector forwardHead = null;

        XRLoopingMover loopingMover = null;

        float speedLeft;
        float speedRight;

        void OnEnable()
        {
            if (loopingMover == null) loopingMover = GetComponent<XRLoopingMover>();
        }


        void Update()
        {
            speedLeft = velocityTrackerLeft.GetSpeed();
            speedRight = velocityTrackerRight.GetSpeed();

            if (armSwingLeft && speedLeft > threshold)
            {
                loopingMover.Move(forwardHead.GetControllerForward() * speedLeft * speedFactor * Time.deltaTime);
            }

            if (armSwingRight && speedRight > threshold)
            {
                loopingMover.Move(forwardHead.GetControllerForward() * speedRight * speedFactor * Time.deltaTime);
            }


        }

        public void ChangeSpeed(float newSpeed)
        {
            StartCoroutine(FadeValue(speedFactor, newSpeed, speedChangeTime));
        }

        private IEnumerator FadeValue(float startValue, float targetValue, float duration)
        {
            float time = 0;
            float currentResult;
            while (time <= duration)
            {
                time += Time.deltaTime;
                currentResult = Mathf.Lerp(startValue, targetValue, speedChangeCurve.Evaluate(time / duration));
                speedFactor = currentResult;
                yield return null;
            }


            yield break;
        }
    }

}
