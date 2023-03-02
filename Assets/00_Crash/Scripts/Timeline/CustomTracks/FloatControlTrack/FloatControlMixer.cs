using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FloatControlMixer : PlayableBehaviour
{
    FloatControlByTimeline floatControlByTimeline;
    int currentClip = 0;
    int parameterIndex = 0;
    bool parameterIndexSet = false;
    float capturedLerpValue = 0;

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
                    // Debug.Log("Clip: " + i + "has Started");
                }
                break;
            }
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {

        floatControlByTimeline = playerData as FloatControlByTimeline;
        if (floatControlByTimeline == null) return;

        int inputCount = playable.GetInputCount();

        // Value Of Current Clip

        float valueOfCurrentClip;
        float weightIfCurrentClip;

        ScriptPlayable<FloatControlBehaviour> inputPlayableCurrClip = (ScriptPlayable<FloatControlBehaviour>)playable.GetInput(currentClip);
        FloatControlBehaviour behaviourCurrClip = inputPlayableCurrClip.GetBehaviour();
        valueOfCurrentClip = behaviourCurrClip.value;
        weightIfCurrentClip = playable.GetInputWeight(currentClip);

        if(behaviourCurrClip.firstClipInTrack)
        {
            parameterIndexSet = false;
        }


        // set parameter Index based on name of first clip in track and scene

        if (!parameterIndexSet)
        {
            ScriptPlayable<FloatControlBehaviour> inputPlayableClip0 = (ScriptPlayable<FloatControlBehaviour>)playable.GetInput(0);
            FloatControlBehaviour behaviourClip0 = inputPlayableClip0.GetBehaviour();
            parameterIndex = floatControlByTimeline.GetParameterIndexByName(behaviourClip0.parameterName);
            parameterIndexSet = true;
        }

        


        // Value Of Previous Clip
        float valueOfPrevClip = 0f;
        float weightOfPrevClip = 0f;

        if (currentClip > 0)
        {
            ScriptPlayable<FloatControlBehaviour> inputPlayablePrevClip = (ScriptPlayable<FloatControlBehaviour>)playable.GetInput(currentClip - 1);
            FloatControlBehaviour behaviourPrevClip = inputPlayablePrevClip.GetBehaviour();
            if (behaviourCurrClip.firstClipInTrack)
            {
                valueOfPrevClip = floatControlByTimeline.GetStartValue(behaviourCurrClip.parameterName);
                capturedLerpValue = valueOfPrevClip;
            }
            else
            {
                valueOfPrevClip = behaviourPrevClip.value;
                weightOfPrevClip = playable.GetInputWeight(currentClip - 1);
            }

        }

        else
        {
            valueOfPrevClip = floatControlByTimeline.GetStartValue(behaviourCurrClip.parameterName);
            capturedLerpValue = valueOfPrevClip;
        }

        // Lerped Value
        float totalWeight = weightOfPrevClip + weightIfCurrentClip;
        float lerpedPrevCurrent;

        if (totalWeight > 0.001f)
        {
            lerpedPrevCurrent = Mathf.LerpUnclamped(valueOfPrevClip, valueOfCurrentClip, playable.GetInputWeight(currentClip));
        }
        else
        {
            lerpedPrevCurrent = capturedLerpValue;
        }

        capturedLerpValue = lerpedPrevCurrent;
        floatControlByTimeline.SetValue(behaviourCurrClip.parameterName, lerpedPrevCurrent);

    }
}