using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ObliqueSenastions.TimelineSpace;

public class HaltepunktDisplay : MonoBehaviour
{
    TextMeshProUGUI tmp = null;

    TimeModeMachine timeModeMachine = null;

    string haltepunktText;

    void Start()
    {
        if(tmp == null)
        {
            tmp = GetComponent<TextMeshProUGUI>();
        } 

        timeModeMachine = TimeLineHandler.instance.GetComponent<TimeModeMachine>();
    }

    
    void Update()
    {
        switch (timeModeMachine.GetTimelinePlayMode())
        {
            case TimelinePlayMode.Hold:
            tmp.text = haltepunktText;
            break;

            case TimelinePlayMode.Pause:
            tmp.text = haltepunktText;
            break;


            default:
            tmp.text = "";
            break;
        }
        
    }

    public void SetHaltepunktText(string text)
    {
        haltepunktText = text;
    }
}
