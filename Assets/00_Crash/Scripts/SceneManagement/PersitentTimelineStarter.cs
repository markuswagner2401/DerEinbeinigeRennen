using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR;

public class PersitentTimelineStarter : MonoBehaviour
{
    PlayableDirector director;
    [SerializeField] StageMaster stageMaster = null;
    [SerializeField] bool checkForOtherPersitentTL;
   
 


    [SerializeField] bool fastForwardOption;
    [SerializeField]  XRNode nodeFF;
    [SerializeField] float forwardFactor = 5f;

    InputDevice device;
    Vector2 inputAxis;

    bool timelinePaused = false;
    bool alreadySwitched = false;

    bool devicesSet = false;




    void Awake()
    {
        
      
    }

    

    private void Start() 
    {
        // if(fastForwardOption)
        // {
        //       device = InputDevices.GetDeviceAtXRNode(nodeFF);
        //       //print("device found: " + device.name);
        // }

        stageMaster = FindObjectOfType<StageMaster>();

        director = GetComponent<PlayableDirector>();

    if(checkForOtherPersitentTL)
    {
        if(FindObjectsOfType<PersitentTimelineStarter>().Length > 1) return;
        if (director.time > 1f) return;
    }
 

        director.Play();
        DontDestroyOnLoad(gameObject);
    }

    private void Update() 
    {
    
        GetDevices();
        
    // pause

        if (timelinePaused)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0.001f);
            
        }

        else
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

    // fast forward

        if(fastForwardOption){
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
            {
//                print("forward");
                ScrollForward(inputAxis.x);
            }

        }
        

        


    }

    private void GetDevices()
    {
        if(devicesSet) return;
        device = InputDevices.GetDeviceAtXRNode(nodeFF);
        if (device.isValid)
        {
            devicesSet = true;
        }
    }

    public void PlayNextGoEvent()
    {
        stageMaster.PlayNextGoEvent();
    }

    public void PlaySteadyEvent(int index)
    {
        stageMaster.PlayStaticEvent(index);
    }

    public void RestoreStageMaster()
    {
        stageMaster = FindObjectOfType<StageMaster>();
    }

    void ScrollForward( float value)
    {
        double actualSpeed =  director.playableGraph.GetRootPlayable(0).GetSpeed();
        director.playableGraph.GetRootPlayable(0).SetSpeed(actualSpeed + (value * forwardFactor));
    }

    public void PausePlayTimeline()
    {
        print("pause");
        
        timelinePaused = !timelinePaused;
 
    }

    
}
