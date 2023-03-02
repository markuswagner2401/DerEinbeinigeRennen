using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TravellerControlMixer : PlayableBehaviour
{
    TravellerControlByTimeline travellerControlByTimeline;
    int currentClip = 0;







    public override void PrepareFrame(Playable playable, FrameData info)
    {
        // set current clip
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            if (playable.GetInputWeight(i) > 0f)
            {
                if (i != currentClip)
                {
                    currentClip = i;
                }
                break;
            }
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        travellerControlByTimeline = playerData as TravellerControlByTimeline;
        if (travellerControlByTimeline == null) return;

        // Get Current Clips

        ScriptPlayable<TravellerControlBehaviour> inputPlayableCurrClip = (ScriptPlayable<TravellerControlBehaviour>)playable.GetInput(currentClip);
        TravellerControlBehaviour behaviourCurrClip = inputPlayableCurrClip.GetBehaviour();

        if(behaviourCurrClip.inactive) return;


        int inputCount = playable.GetInputCount();

        // Value Of Current Clip


        float weightOfCurrentClip;
        int transPointIndexCurrClip = 0;
        string transPointNameCurrClip = "";
        bool roleAwareCurrentClip;
        Role roleCurrentClip;


        
        weightOfCurrentClip = playable.GetInputWeight(currentClip);
        transPointIndexCurrClip = behaviourCurrClip.transPointIndex;
        transPointNameCurrClip = behaviourCurrClip.transitionPointName;
        roleAwareCurrentClip = behaviourCurrClip.roleAware;
        roleCurrentClip = roleAwareCurrentClip ? behaviourCurrClip.role : Role.None;

        // // Value Of Previous Clip

        // float weightOfPrevClip = 0f;
        int transPointIndexPrevClip = 0;
        string transPointNamePrevClip = "";
        bool roleAwarePrevClip;
        Role rolePrevClip;
        

        if (currentClip > 0)
        {
            ScriptPlayable<TravellerControlBehaviour> inputPlayablePrevClip = (ScriptPlayable<TravellerControlBehaviour>)playable.GetInput(currentClip - 1);
            TravellerControlBehaviour behaviourPrevClip = inputPlayablePrevClip.GetBehaviour();
            if (behaviourPrevClip.lastClipInScene)
            {
                transPointIndexPrevClip = 0;
                transPointNamePrevClip = "";
                rolePrevClip = roleCurrentClip;
            }
            else
            {
                transPointIndexPrevClip = behaviourPrevClip.transPointIndex;
                transPointNamePrevClip = behaviourPrevClip.transitionPointName;
                roleAwarePrevClip = behaviourPrevClip.roleAware;
                rolePrevClip = roleAwarePrevClip ? behaviourPrevClip.role : Role.None;
                if(rolePrevClip != roleCurrentClip)
                {
                    Debug.LogError("TravellerControlMixer: role of previous clip doesnt match with role of current clip, please consider to put it on seperate tracks.");
                }
            }

        }

        else
        {
            transPointIndexPrevClip = 0;
            transPointNamePrevClip = "";
            rolePrevClip = roleCurrentClip;
        }

        float newT = weightOfCurrentClip;

        travellerControlByTimeline.SetCurrentT(newT, roleCurrentClip);
        travellerControlByTimeline.SetTransitionPointIndex(transPointIndexCurrClip, roleCurrentClip);
        travellerControlByTimeline.SetTransPointName(transPointNameCurrClip, roleCurrentClip);

        travellerControlByTimeline.SetPreviousPointIndex(transPointIndexPrevClip, rolePrevClip);
        travellerControlByTimeline.SetTransPointNamePrev(transPointNamePrevClip, rolePrevClip);

        if (newT > 0.999f)
        {
            travellerControlByTimeline.SetTransitionComplete(true, roleCurrentClip);
        }

        else
        {
            travellerControlByTimeline.SetTransitionComplete(false, roleCurrentClip);
        }

        if (newT < 0.001f)
        {
            travellerControlByTimeline.SetTransitioning(false, roleCurrentClip);
        }

        else
        {
            travellerControlByTimeline.SetTransitioning(true, roleCurrentClip);
        }





    }






}