using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.PunNetworking;

using UnityEngine.XR;
using System;

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

        private void Start()
        {
            if (syncPlayableDirector == null)
            {
                syncPlayableDirector = GetComponent<SyncPlayableDirector>();
            }
        }

        private void Update()
        {
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


    }

}

