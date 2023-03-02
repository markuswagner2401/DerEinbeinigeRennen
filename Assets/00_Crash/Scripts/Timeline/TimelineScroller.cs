using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR;


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

    [SerializeField] bool pauseTimeline = false;

    bool previousPauseTimeline = false;

    [SerializeField] bool holdTimeline = false;

    bool previousHoldTimeline;

    










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

            if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonPressed) && secondaryButtonPressed)
            {
                if (secondaryButtonPressedBefore) return;
                SwitchHoldTimeline();
            }

        }

        if(previousPauseTimeline != pauseTimeline)
        {
            PlayTimeline(!pauseTimeline);
        }

        if(previousHoldTimeline != holdTimeline)
        {
            timelineTime.OverwriteTLDeltaTimeWithTimeDeltaTime(holdTimeline);
        }
        

        

        previousHoldTimeline = holdTimeline;

        previousPauseTimeline = pauseTimeline;

    }

    public void SwitchPauseTimeline()
    {
        bool newValue = !pauseTimeline;
        SetPauseTimeline(newValue);
    }

    public void SetPauseTimeline(bool value)
    {
        pauseTimeline = value;
        if(!pauseTimeline)
        {
            holdTimeline = false;
        }
    }

    public void SwitchHoldTimeline()
    {
        bool newValue = !holdTimeline;
        HoldTimeline(newValue);

    }

    public void HoldTimeline(bool value)
    {
        pauseTimeline = value;
        holdTimeline = value;


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

    //double capturedSpeed;



    public void ScrollTimeline(float value)
    {
        if(holdTimeline)
        {
            StopScrollingTimeline();
            return;
        }

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

        if(!holdTimeline && !pauseTimeline)
        {
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed((1));
        }

        

    }



    // public void PausePlayTimeline()
    // {

    //     if (pausePlayTriggered) return;



    //     pausePlayTriggered = true;

    //     timelinePaused = !timelinePaused;

    //     if (timelinePaused)
    //     {
    //         playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0.001f);

    //     }

    //     else
    //     {
    //         playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
    //     }


    // }

    public void PlayTimeline(bool value)
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
