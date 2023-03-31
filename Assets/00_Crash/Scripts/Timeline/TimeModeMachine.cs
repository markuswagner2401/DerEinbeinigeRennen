using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.PunNetworking;

using UnityEngine.XR;
using System;
using UnityEngine.Events;

namespace ObliqueSenastions.TimelineSpace
{
    [System.Serializable]
    public enum TimelinePlayMode
    {
        Play,
        Pause,
        Hold,
        FastForward,
        FastBackward
    }

    [System.Serializable]
    public enum TimelineTimeMode
    {
        useTimelineTime,
        useDeltaTime,
    }

    public class TimeModeMachine : MonoBehaviour
    {

        public TimelinePlayMode currentTimelinePlayMode;
        private TimelinePlayMode capturedTimelinePlayMode;
        public bool currentBadTracking;
        public bool inUiTime;

        public float currentAccidentTimeFactor = 1f;


        // networking

        [SerializeField] SyncPlayableDirector syncPlayableDirector;

        bool ownershipRequestTriggered = false;

        // parameters

        [SerializeField] bool enableScrolling;
        [SerializeField] float maxHoldtimeIfNotInNetwork = Mathf.Infinity;

        float holdTimer = 0;

        //maybe obsolete
        #region XRToolkitRegion
        InputDevice device;

        [SerializeField] XRNode nodeFF;

        bool devicesSet = false;

        Vector2 inputAxis;

        bool pausePlayTriggered = false;

        bool primaryButtonPressed;

        bool primaryButtonPressedBefore;

        bool secondaryButtonPressed;

        bool secondaryButtonPressedBefore;

        bool isScrolling = false;

        #endregion

        //bad Tracking
        [SerializeField] bool goIntoBreakAtBadTracking = false;

        [SerializeField] OVRHand leftHand = null;

        [SerializeField] OVRHand rightHand = null;


        [SerializeField] float acceptedTimeWithBadTracking = 1f;

        float badTrackingTimer;




        bool isHolding;


        ////

        // Accident Parameters

        [SerializeField] AccidentTime[] accidentsTimes;

        [System.Serializable]
        public struct AccidentTime
        {

            public string note;
            
            public UnityEvent doAtStart;

            [Tooltip("1 is no change. < 1 is slomo- > 1 is accceleration")]
            public float strength;
            public float goInDuration;
            public AnimationCurve goInCurve;
            public float stayInDuration;
            public float goOutDuration;
            public AnimationCurve goOutCurve;
            public UnityEvent doAtEnd;
        }

        bool inAccident = false;

        private void Start()
        {
            if (syncPlayableDirector == null)
            {
                syncPlayableDirector = GetComponent<SyncPlayableDirector>();
            }
        }

        private void Update()
        {
            if(!inAccident) currentAccidentTimeFactor = 1f;
            UnholdInSingelplayer();

            if (enableScrolling) // Scrolling with XR toolkit
            {
                GetDevices();

                ProcessXRToolkitInput();

            }

            if (goIntoBreakAtBadTracking)
            {
                if (leftHand == null)
                {
                    GameObject leftHandGo = GameObject.FindWithTag("LeftOVRHand");
                    if (leftHandGo == null) return;
                    leftHand = leftHandGo.GetComponent<OVRHand>();
                    return;

                }
                if (rightHand == null)
                {
                    GameObject rightHandGo = GameObject.FindWithTag("RightOVRHand");
                    if (rightHandGo == null) return;
                    rightHand = rightHandGo.GetComponent<OVRHand>();
                    return;
                }


                if (!leftHand.IsDataHighConfidence && !rightHand.IsDataHighConfidence)
                {
                    badTrackingTimer += Time.deltaTime;
                    if (badTrackingTimer > acceptedTimeWithBadTracking)
                    {
                        currentBadTracking = true;
                    }
                }
                else
                {
                    badTrackingTimer = 0f;
                    currentBadTracking = false;
                }
            }
        }

        private void UnholdInSingelplayer()
        {
            if (currentTimelinePlayMode == TimelinePlayMode.Hold || currentTimelinePlayMode == TimelinePlayMode.Pause)
            {
                holdTimer += Time.deltaTime;
                if (holdTimer > maxHoldtimeIfNotInNetwork)
                    SwitchHoldPlay();
            }

            else
            {
                holdTimer = 0;
            }
        }

        private void ProcessXRToolkitInput()
        {
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
            {
                if (Mathf.Abs(inputAxis.x) > 0.1f)
                {

                    if (inputAxis.x > 0)
                    {
                        FastForward(true);
                    }

                    else
                    {
                        FastBackward(false);
                    }

                }

                else
                {
                    if (isScrolling)
                    {
                        if (currentTimelinePlayMode == TimelinePlayMode.FastForward)
                        {
                            FastForward(false);
                        }
                        else if (currentTimelinePlayMode == TimelinePlayMode.FastBackward)
                        {
                            FastBackward(false);
                        }
                    }
                }
            }

            if ((device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonPressed) && primaryButtonPressed))
            {
                if (primaryButtonPressedBefore) return;
                SwitchPausePlay();
            }

            primaryButtonPressedBefore = primaryButtonPressed;
        }

        private void GetDevices()
        {
            if (devicesSet) return;
            device = InputDevices.GetDeviceAtXRNode(nodeFF);
            if (device.isValid)
            {
                devicesSet = true;
            }
        }





        public void SetUiTime(bool value)
        {
            inUiTime = value;
        }



        public void SwitchPausePlay()
        {
            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimeModeMachine: Timeline Owned by another Player. Change Rejected");
                return;
            }

            switch (currentTimelinePlayMode)
            {
                case TimelinePlayMode.Play:
                    currentTimelinePlayMode = TimelinePlayMode.Pause;
                    break;

                case TimelinePlayMode.Pause:
                    currentTimelinePlayMode = TimelinePlayMode.Play;
                    break;

                case TimelinePlayMode.Hold:
                    currentTimelinePlayMode = TimelinePlayMode.Pause;
                    break;

                case TimelinePlayMode.FastForward:
                    currentTimelinePlayMode = TimelinePlayMode.Pause;
                    break;

                case TimelinePlayMode.FastBackward:
                    currentTimelinePlayMode = TimelinePlayMode.Pause;
                    break;

                default:
                    break;
            }
        }

        public void SwitchHoldPlay()
        {
            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimeModeMachine: Timeline Owned by another Player. Change Rejected");
                return;
            }

            switch (currentTimelinePlayMode)
            {
                case TimelinePlayMode.Play:
                    currentTimelinePlayMode = TimelinePlayMode.Hold;
                    break;

                case TimelinePlayMode.Pause:
                    currentTimelinePlayMode = TimelinePlayMode.Play;
                    break;

                case TimelinePlayMode.Hold:
                    currentTimelinePlayMode = TimelinePlayMode.Play;
                    break;

                case TimelinePlayMode.FastForward:
                    currentTimelinePlayMode = TimelinePlayMode.Hold;
                    break;

                case TimelinePlayMode.FastBackward:
                    currentTimelinePlayMode = TimelinePlayMode.Hold;
                    break;

                default:
                    break;

            }
        }

        public void Play()
        {
            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimeModeMachine: Timeline Owned by another Player. Change Rejected");
                return;
            }

            SetTimelinePlayMode(TimelinePlayMode.Play);
        }

        public void Pause()
        {
            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimeModeMachine: Timeline Owned by another Player. Change Rejected");
                return;
            }

            SetTimelinePlayMode(TimelinePlayMode.Pause);
        }

        public void Hold()
        {
            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimeModeMachine: Timeline Owned by another Player. Change Rejected");
                return;
            }

            SetTimelinePlayMode(TimelinePlayMode.Hold);
        }

        public void FastForward(bool value)
        {
            if (!enableScrolling) return;
            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimeModeMachine: Timeline Owned by another Player. Change Rejected");
                return;
            }

            if (value)
            {
                capturedTimelinePlayMode = currentTimelinePlayMode;
                SetTimelinePlayMode(TimelinePlayMode.FastForward);
            }
            else
            {
                SetTimelinePlayMode(capturedTimelinePlayMode);
            }
        }

        public void FastBackward(bool value)
        {
            if (!enableScrolling) return;
            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimeModeMachine: Timeline Owned by another Player. Change Rejected");
                return;
            }

            if (value)
            {
                capturedTimelinePlayMode = currentTimelinePlayMode;
                SetTimelinePlayMode(TimelinePlayMode.FastBackward);
            }

            else
            {
                SetTimelinePlayMode(capturedTimelinePlayMode);
            }



        }


        private void SetTimelinePlayMode(TimelinePlayMode mode)
        {
            currentTimelinePlayMode = mode;
        }


        // Getter

        public TimelinePlayMode GetTimelinePlayMode()
        {
            return currentTimelinePlayMode;
        }

        public bool GetBadTracking()
        {
            return currentBadTracking;
        }

        public bool GetInUiTime()
        {
            return inUiTime;
        }

        // Setter (syncPlayableDirectio of !isMine)

        public void SetTimelinePlayMode(int playModeIndex)
        {
            if (Enum.IsDefined(typeof(TimelinePlayMode), playModeIndex))
            {
                currentTimelinePlayMode = (TimelinePlayMode)playModeIndex;
            }
            else
            {
                throw new ArgumentException("Invalid play mode index", nameof(playModeIndex));
            }
        }


        //// Accident Factor


        public void PlayAccident(string name)
        {
            int index = GetAccIndexByName(name);
            if(index < 0) return;
            PlayAccident(index);
        }

        private int GetAccIndexByName(string name)
        {
            
            for (int i = 0; i < accidentsTimes.Length; i++)
            {
                if(name == accidentsTimes[i].note)
                {
                    return i;
                }
            }

            Debug.LogError("TimeModeMachine: No Accident found with name:" + name);

            return -1;
        }

        public void PlayAccident(int index)
        {
            if (inAccident) return;
            StartCoroutine(AccidentTimeRoutine(index));
        }

        IEnumerator AccidentTimeRoutine(int index)
        {
            print("play accident: " + accidentsTimes[index].note);
            inAccident = true;
            accidentsTimes[index].doAtStart.Invoke();

            float startValue = currentAccidentTimeFactor;
            float targetValue = accidentsTimes[index].strength;
            float goInDuration = accidentsTimes[index].goInDuration;
            AnimationCurve goInCurve = accidentsTimes[index].goInCurve;
            float stayDuration = accidentsTimes[index].stayInDuration;
            float goOutDuration = accidentsTimes[index].goInDuration;
            AnimationCurve goOutCurve = accidentsTimes[index].goOutCurve;
            float goInTimer = 0;
            float goOutTimer = 0;
            float outputValue;

            while (goInTimer < goInDuration)
            {
                goInTimer += Time.deltaTime;
                outputValue = Mathf.Lerp(startValue, targetValue, goInCurve.Evaluate(goInTimer / goInDuration));
                currentAccidentTimeFactor = outputValue;
                yield return null;
            }

            yield return new WaitForSeconds(stayDuration);

            startValue = currentAccidentTimeFactor;
            targetValue = 1f;

            while (goOutTimer < goOutDuration)
            {
                goOutTimer += Time.deltaTime;
                outputValue = Mathf.Lerp(startValue, targetValue, goOutCurve.Evaluate(goOutTimer / goOutDuration));
                currentAccidentTimeFactor = outputValue;
                yield return null;
            }
            
            accidentsTimes[index].doAtEnd.Invoke();
            inAccident = false;
            yield break;
        }

        public float GetCurrentAccidentTimeFactor()
        {
            return currentAccidentTimeFactor;
        }


    }

}

