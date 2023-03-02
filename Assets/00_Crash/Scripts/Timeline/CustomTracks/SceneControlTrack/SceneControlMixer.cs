using UnityEngine;
using UnityEngine.Playables;

public class SceneControlMixer : PlayableBehaviour
{
    SceneControlByTimeline sceneControlByTimeline;

    SceneClip[] sceneClip;
    struct SceneClip
    {
        public string sceneName;
        public int sceneIndex;
        public double startTime;
        public bool jumpToStartOnGoingBack;
        public string roomSection;
    }



    int currentClipIndex;

    int capturedClipIndex;

    bool scenesSet = false;



    public override void OnGraphStart(Playable playable)
    {
        scenesSet = false;
        capturedClipIndex = currentClipIndex;





        //Debug.Log("Scene Start Times Set: " + string.Join(", ", startTimes));



    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        // set current clip
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            if (playable.GetInputWeight(i) > 0f)
            {
                if (i != currentClipIndex)
                {
                    currentClipIndex = i;
                }
                break;
            }
        }
    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        sceneControlByTimeline = playerData as SceneControlByTimeline;
        if (sceneControlByTimeline == null)
        {
            Debug.Log("SceneControlMixer: SceneControlByTimeline not found; not assigned in Timeline Track?");
            return;
        }

        int inputCount = playable.GetInputCount();


        // set scene array

        if (!scenesSet)
        {

            sceneClip = new SceneClip[inputCount];
            double totalTime = 0;

            for (int i = 0; i < inputCount; i++)
            {


                ScriptPlayable<SceneControlBehaviour> input = (ScriptPlayable<SceneControlBehaviour>)playable.GetInput(i);
                SceneControlBehaviour behaviour = input.GetBehaviour();

                sceneClip[i].startTime = totalTime;
                sceneClip[i].sceneName = behaviour.sceneName;
                sceneClip[i].sceneIndex = behaviour.sceneIndex;
                sceneClip[i].jumpToStartOnGoingBack = behaviour.jumpToClipStartOnGoingBackInTL;
                sceneClip[i].roomSection = behaviour.roomSection;

                totalTime += behaviour.clipDuration;
            }

            for (int i = 0; i < sceneClip.Length; i++)
            {
                sceneControlByTimeline.SetupSceneParameters(i, sceneClip[i].sceneIndex, sceneClip[i].sceneName, sceneClip[i].startTime, sceneClip[i].jumpToStartOnGoingBack, sceneClip[i].roomSection);
            }
            scenesSet = true;

            Debug.Log("SceneControlMixer: Scenes array Set");
        }


        // current Clip Infos

        ScriptPlayable<SceneControlBehaviour> inputPlayableCurrentClip = (ScriptPlayable<SceneControlBehaviour>)playable.GetInput(currentClipIndex);
        SceneControlBehaviour behaviourCurrentClip = inputPlayableCurrentClip.GetBehaviour();

        //

        double currentTime = playable.GetTime();
        double clipDuration = behaviourCurrentClip.clipDuration;



        //double startTime = playable.GetPreviousTime();


        if (capturedClipIndex != currentClipIndex)
        {

            if (capturedClipIndex < currentClipIndex)
            {
                if (behaviourCurrentClip.useSceneName)
                {
                    sceneControlByTimeline.ChangeSceneOnGoingForwardInTL(behaviourCurrentClip.sceneName);
                }
                else
                {
                    sceneControlByTimeline.ChangeSceneOnGoingForwardInTL(behaviourCurrentClip.sceneIndex);
                }
                
            }
            else
            {
                sceneControlByTimeline.ChangeSceneOnGoingBackInTL(behaviourCurrentClip.sceneIndex, behaviourCurrentClip.jumpToClipStartOnGoingBackInTL, currentClipIndex);
            }

        }

        capturedClipIndex = currentClipIndex;

        sceneControlByTimeline.SetCurrentSceneClip(currentClipIndex);




    }
}