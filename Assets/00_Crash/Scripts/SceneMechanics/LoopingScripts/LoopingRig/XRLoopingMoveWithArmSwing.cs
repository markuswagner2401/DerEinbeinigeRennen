using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.VRRigSpace;
using Oculus.Interaction.Input;
using UnityEngine;
using ObliqueSenastions.UISpace;

namespace ObliqueSenastions.Looping
{

    public class XRLoopingMoveWithArmSwing : MonoBehaviour
    {
        enum InputMode
        {
            ovrHand,
            readAtTacho,
            xrToolkit
        }

        [SerializeField] InputMode inputMode;
        [SerializeField] bool armSwingRight = true;
        [SerializeField] bool armSwingLeft = true;
        [SerializeField] float speedFactor = 10f;

        [SerializeField] float speedChangersFactor;

        [SerializeField] float speedBoostersFactor;
        [SerializeField] AnimationCurve speedChangeCurve;
        [SerializeField] float speedChangeTime = 5f;

        [SerializeField] SpeedChanger[] speedChangers;

        [System.Serializable]
        public struct SpeedChanger
        {
            public string name;

            public float targetValue;
            public float duration;
            public AnimationCurve curve;
        }

        
        [SerializeField] SpeedBooster[] speedBoosters;

        [System.Serializable]
        public struct SpeedBooster
        {
            public string name;

            public float pushValue;

            public float increaseValue;

            public float maxValue;

          

        }

        bool inSpeedBooster = false;

       

        [SerializeField] float threshold = .1f;

        [SerializeField] bool usingOVR = false;

        [SerializeField] bool readAtTacho = false;

        [SerializeField] Tachonadel tachoLeft = null;
        [SerializeField] Tachonadel tachoRight = null;

        [SerializeField] XRVelocityTracker velocityTrackerRight = null;
        [SerializeField] XRVelocityTracker velocityTrackerLeft = null;

        [Tooltip("will be used on Handtracking")]
        [SerializeField] SimpleVelocityTracker simpleVelocityTrackerRight = null;

        [SerializeField] SimpleVelocityTracker simpleVelocityTrackerLeft = null;

        [Tooltip("used to get tracking confidece")]
        [SerializeField] OVRHand leftHand = null;

        [SerializeField] OVRHand rightHand = null;

        [SerializeField] LoopingControllerForwardVector forwardHead = null;
        [SerializeField] float smoothing;



        bool speedChangeInterrupted;

        XRLoopingMover loopingMover = null;

        float speedLeft;

        float prevSpeedLeft;
        float speedRight;

        float prevSpeedRight;

        float speedSum;
        float prevSpeedSum;

        void OnEnable()
        {
            if (loopingMover == null) loopingMover = GetComponent<XRLoopingMover>();

            prevSpeedLeft = speedLeft;
            prevSpeedRight = speedRight;

            if(inputMode == InputMode.readAtTacho)
            {
                if(tachoLeft == null || tachoRight == null)
                {
                    inputMode = InputMode.ovrHand;
                }
            }
        }


        void FixedUpdate()
        {
            


            switch (inputMode)
            {
                case InputMode.ovrHand:
                    ProcessOVRInput();
                    break;

                case InputMode.xrToolkit:
                    speedLeft = velocityTrackerLeft.GetSpeed();
                    speedRight = velocityTrackerRight.GetSpeed();
                    break;

                case InputMode.readAtTacho:
                    speedLeft = tachoLeft.GetNormedTargetPosition();
                    speedRight = tachoRight.GetNormedTargetPosition();
                  
                    break;

                default:
                    break;
            }


            ////

            speedSum = 0;

            if (armSwingLeft && speedLeft > threshold)
            {
                speedSum += speedLeft;
            }

            if (armSwingRight && speedRight > threshold)
            {
                speedSum += speedRight;
            }

            speedSum = Mathf.Lerp(prevSpeedSum, speedSum, smoothing);

            speedFactor = inSpeedBooster ? speedBoostersFactor : speedChangersFactor;

            loopingMover.Move(forwardHead.GetControllerForward() * speedSum * speedFactor * Time.deltaTime);





            

            ////




            // if (armSwingLeft && speedLeft > threshold)
            // {
            //     speedLeft = Mathf.Lerp(prevSpeedLeft, speedLeft, smoothing);
            //     loopingMover.Move(forwardHead.GetControllerForward() * speedLeft * speedFactor * Time.deltaTime);
            // }

            // if (armSwingRight && speedRight > threshold)
            // {
            //     speedRight = Mathf.Lerp(prevSpeedRight, speedRight, smoothing);
            //     loopingMover.Move(forwardHead.GetControllerForward() * speedRight * speedFactor * Time.deltaTime);
            // }

            prevSpeedLeft = speedLeft;
            prevSpeedRight = speedRight;
            prevSpeedSum = speedSum;


        }

        private void ProcessOVRInput()
        {
            OVRInput.Update();

            if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
            {
                print("hands not active");
                speedLeft = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude;
                speedRight = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;

            }

            else
            {
                //speedLeft = leftHand.IsDataHighConfidence ? simpleVelocityTrackerLeft.GetLocalSpeed() : Mathf.Lerp(prevSpeedLeft, 0, smoothing);
                //speedRight = rightHand.IsDataHighConfidence ? simpleVelocityTrackerRight.GetLocalSpeed() : Mathf.Lerp(prevSpeedRight, 0, smoothing);
                speedLeft = simpleVelocityTrackerLeft.GetLocalSpeed();
                speedRight = simpleVelocityTrackerRight.GetLocalSpeed();
            }

        }

        //// speed change

        public void ChangeSpeed(string name)
        {
            int index = GetSpeedChangerIndexByName(name);
            if (index < 0) return;
            ChangeSpeed(index);
        }

        int GetSpeedChangerIndexByName(string name)
        {
            for (int i = 0; i < speedChangers.Length; i++)
            {
                if (speedChangers[i].name == name)
                {
                    return i;
                }
            }
            Debug.LogError("XRLoopingMoveWithArmSwing: No speed Changer found with name: " + name);
            return -1;
        }

        public void ChangeSpeed(int index)
        {
            StartCoroutine(InterruptAndChangeSpeed(index));
        }

        public IEnumerator InterruptAndChangeSpeed(int index)
        {
            speedChangeInterrupted = true;
            
            yield return new WaitForSeconds(0.1f);
            speedChangeInterrupted = false;
            StartCoroutine(FadeValue(speedChangersFactor, speedChangers[index].targetValue, speedChangers[index].duration, speedChangers[index].curve));
            yield break;
        }


        //// Speed Booster

        public void PlaySpeedBooster(int index)
        {
            print("Play Speed Booster");

            if(index < 0 || index >= speedBoosters.Length)
            {
                Debug.Log("LoopingMoveWithArmSwing: No Speedbosster with index: " + index);
                return;
            }

            speedBoostersFactor += speedBoosters[index].pushValue;

            if(inSpeedBooster) return;

            inSpeedBooster = true;

            StartCoroutine(PlaySpeedBoosterRoutine(index));
        }

        // IEnumerator InterruptAndPlaySpeedBooster(int index)
        // {
        //     speedBoosterInterruptedByBooster = true;
        //     yield return new WaitForSeconds(0.1f);
        //     speedBoosterInterruptedByBooster = false;
        //     speedBoosterInterrupted = false;
        //     StartCoroutine(PlaySpeedBoosterRoutine(index));
        //     yield break;
        // }

        IEnumerator PlaySpeedBoosterRoutine(int index)
        {

            speedBoostersFactor = speedChangersFactor;
    
            while (inSpeedBooster)
            {
                if(speedBoostersFactor < speedBoosters[index].maxValue)
                {
                    speedBoostersFactor += speedBoosters[index].increaseValue;
                }
                
                yield return null;
            }

            yield break;
        }


        public void InterruptSpeedBoost()
        {
            print("interrupt speed booster");
            inSpeedBooster = false;
        }

        ///

        ///

        public void ChangeSpeed(float newSpeed)
        {
            StartCoroutine(FadeValue(speedChangersFactor, newSpeed, speedChangeTime, speedChangeCurve));
        }

        private IEnumerator FadeValue(float startValue, float targetValue, float duration, AnimationCurve speedChangeCurve)
        {
            float time = 0;
            float currentResult;
            while (time <= duration && !speedChangeInterrupted)
            {
                time += Time.deltaTime;
                currentResult = Mathf.Lerp(startValue, targetValue, speedChangeCurve.Evaluate(time / duration));
                speedChangersFactor = currentResult;
                yield return null;
            }


            yield break;
        }


    }

}
