using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.PunNetworking;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR;

namespace ObliqueSenastions.TimelineSpace
{

    public class TimelineScroller : MonoBehaviour
    {
        [SerializeField] PlayableDirector playableDirector;
        //   [SerializeField] float fastForwardSpeed = 0.001f;

        [SerializeField] bool enableScrolling = false;
        [SerializeField] bool enablePause = false;


        [SerializeField] XRNode nodeFF;

        [SerializeField] float scrollFactor = 5f;

        [SerializeField] SyncPlayableDirector syncPlayableDirector;

        [SerializeField] TimelineTime timelineTime = null;

        [SerializeField] float maxHoldtimeIfNotInNetwork = 10f; 

        float holdTimer = 0;

        bool isHolding;



        InputDevice device;

        Vector2 inputAxis;

        bool devicesSet = false;

        bool timelinePaused = false;

        bool ownershipRequestTriggered = false;
        float scrollSwitchBackTime = 1f;

        float scrollSwitchTimer = Mathf.Infinity;

        bool isScrolling = false;

        bool pausePlayTriggered = false;

        bool primaryButtonPressed;

        bool primaryButtonPressedBefore;

        bool secondaryButtonPressed;

        bool secondaryButtonPressedBefore;

      












        void Start()
        {
            if (playableDirector == null)
            {
                playableDirector = GetComponent<PlayableDirector>();
            }

            if (syncPlayableDirector == null)
            {
                syncPlayableDirector = GetComponent<SyncPlayableDirector>();
            }

            if (timelineTime == null)
            {
                timelineTime = GetComponent<TimelineTime>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(isHolding)
            {
                holdTimer += Time.deltaTime;
                if(holdTimer > maxHoldtimeIfNotInNetwork)
                SwitchHoldTimeline();
            }

            else
            {
                holdTimer = 0;
            }

            // fast forward

            if (enableScrolling)
            {
                GetDevices();



                if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
                {
                    if (Mathf.Abs(inputAxis.x) > 0.1f)
                    {

                        ScrollTimeline(inputAxis.x);

                    }

                    else
                    {
                        if (isScrolling)
                        {

                            StopScrollingTimeline();
                        }
                    }
                }



                if ((device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonPressed) && primaryButtonPressed))
                {
                    if (primaryButtonPressedBefore) return;
                    SwitchPauseTimeline();
                }

                primaryButtonPressedBefore = primaryButtonPressed;

                // if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonPressed) && secondaryButtonPressed)
                // {
                //     if (secondaryButtonPressedBefore) return;
                //     SwitchHoldTimeline();
                // }

            }

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

        public void SwitchPauseTimeline()
        {

            double speed = playableDirector.playableGraph.GetRootPlayable(0).GetSpeed();
            bool currentIsPaused = (speed < 0.005);
            bool newIsPaused = !currentIsPaused;
            PauseTimeline(newIsPaused);
            
        }

        public void PauseTimeline(bool newIsPaused)
        {
            
            PlayTimeline(!newIsPaused);

            TimelineTime timelineTime = GetComponent<TimelineTime>();
            if(timelineTime.GetMode() == TimelineTimeMode.useCustomTime) return;

            timelineTime.SetMode(TimelineTimeMode.useTimelineTime);

            
        }

        
        public void SwitchHoldTimeline()
        {
            double speed = playableDirector.playableGraph.GetRootPlayable(0).GetSpeed();
            bool currentIsHolding = (speed < 0.005);
            bool newIsHolding = !currentIsHolding;

            HoldTimeline(newIsHolding);
            
            
        }

        public void HoldTimeline(bool newIsHolding)
        {
            PlayTimeline(!newIsHolding);
            TimelineTime timelineTime = GetComponent<TimelineTime>();
            if(timelineTime.GetMode() == TimelineTimeMode.useCustomTime) return;

            if(newIsHolding)
            { 
                isHolding = true;  
                timelineTime.SetMode(TimelineTimeMode.useDeltaTime);
            }
            else
            {
                isHolding = false;
                timelineTime.SetMode(TimelineTimeMode.useTimelineTime);
            }

        }





        

        //double capturedSpeed;



        public void ScrollTimeline(float value)
        {
            // if (holdTimeline)
            // {
            //     StopScrollingTimeline();
            //     return;
            // }

            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimelineScroller: Timeline Owned by another Player. Scroll receted");
                return;
            }

            //

            isScrolling = true;

            //capturedSpeed = playableDirector.playableGraph.GetRootPlayable(0).GetSpeed();



            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed((value * scrollFactor));
        }



        public void StopScrollingTimeline()
        {


            // /// Check Network Sync

            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimelineScroller: Timeline Owned by another Player. Scroll receted");
                return;
            }

            //

            isScrolling = false;

            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed((1));


        }



        

        private void PlayTimeline(bool value)
        {
            /// Check Network Sync

            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimelineScroller: Timeline Owned by another Player. Pause/Play receted");
                return;
            }

            //

          

            double newSpeed = value ? 1d : 0.001d;

            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(newSpeed);

        }



        public void JumpToNextSceneClip()
        {
            /// Check Network Sync

            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimelineScroller: Timeline Owned by another Player. Jump receted");
                return;
            }

            //

            SceneControlByTimeline sceneControlByTimeline = GetComponent<SceneControlByTimeline>();
            int currentSceneClip = sceneControlByTimeline.GetCurrentSceneClip();
            double newTime = sceneControlByTimeline.GetStartTime(currentSceneClip + 1);
            PlayableDirector playableDirector = GetComponent<PlayableDirector>();
            playableDirector.time = newTime;
        }

        public void JumpToPrevSceneClip()
        {
            /// Check Network Sync

            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimelineScroller: Timeline Owned by another Player. Jump receted");
                return;
            }

            //

            SceneControlByTimeline sceneControlByTimeline = GetComponent<SceneControlByTimeline>();
            int currentSceneClip = sceneControlByTimeline.GetCurrentSceneClip();
            double newTime = sceneControlByTimeline.GetStartTime(currentSceneClip - 1);
            PlayableDirector playableDirector = GetComponent<PlayableDirector>();
            playableDirector.time = newTime;
        }

        public void JumpToStartOfThisSceneClip()
        {
            /// Check Network Sync

            if (syncPlayableDirector != null && !syncPlayableDirector.RequestNetworkOwnership())
            {
                Debug.Log("TimelineScroller: Timeline Owned by another Player. Jump receted");
                return;
            }

            //

            SceneControlByTimeline sceneControlByTimeline = GetComponent<SceneControlByTimeline>();
            int currentSceneClip = sceneControlByTimeline.GetCurrentSceneClip();
            double newTime = sceneControlByTimeline.GetStartTime(currentSceneClip);
            PlayableDirector playableDirector = GetComponent<PlayableDirector>();
            playableDirector.time = newTime;

        }









        // public void FastForward(float speed)
        // {
        //     if (!enableScrolling) return;
        //     playableDirector.time += speed;
        // }

        // public void Pause(bool value)
        // {
        //     if (!enablePause) return;
        //     if (value)
        //     {
        //         playableDirector.Pause();
        //     }
        //     else
        //     {
        //         playableDirector.Play();
        //     }

        // }
    }

}
