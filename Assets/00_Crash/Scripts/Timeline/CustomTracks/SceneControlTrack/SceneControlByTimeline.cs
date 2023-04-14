using System;
using ObliqueSenastions.SceneSpace;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace ObliqueSenastions.TimelineSpace
{

    public class SceneControlByTimeline : MonoBehaviour
    {
        [SerializeField] SceneClip[] sceneClips;

        [System.Serializable]
        struct SceneClip
        {
            public string name;
            public int sceneIndex;
            public double startTime;
            public bool jumpOnStartOnGoingBack;

            public string roomSection;
        }

        int currentSceneClip = -1;

        private void Update()
        {
            //        Debug.Log("current scene clip: " + currentSceneClip);
        }

        /// set by behaviour

        bool CheckSceneChange(string sceneName)
        {
            if(sceneName == SceneManager.GetActiveScene().name) return false;
            
            //Check if Scene Exists
            return GetComponent<SceneListManager>().CheckIfSceneExists(sceneName);

        }

        public void ChangeSceneOnGoingForwardInTL(string sceneName)
        {
            
            if(!CheckSceneChange(sceneName)) return;

            //

            Debug.Log("SceneControlByTimeline: ChangeSceneOnGoingForwardInTL, going to: " + sceneName);
            if (Application.isPlaying)
            {
                GetComponent<SceneChanger>().ChangeScene(sceneName);
            }

        }

        public void ChangeSceneOnGoingForwardInTL(int sceneIndex)
        {
            //if(sceneIndex == SceneManager.GetActiveScene().buildIndex) return;

            Debug.Log("SceneControlByTimeline: ChangeSceneOnGoingForwardInTL, going to: " + sceneIndex);
            if (Application.isPlaying)
            {
                GetComponent<SceneChanger>().ChangeScene(sceneIndex);
            }
        }

        public void ChangeSceneOnGoingBackInTL(int sceneIndex, bool jumpToSceneStart, int clipIndex)
        {

            //if(sceneIndex == SceneManager.GetActiveScene().buildIndex) return;

            Debug.Log("SceneControlByTimeline: ChangeSceneOnGoingBackInTL, going to: " + sceneIndex + ". jump To Start of Clip: " + jumpToSceneStart);
            if (Application.isPlaying)
            {
                GetComponent<SceneChanger>().ChangeScene(sceneIndex);
                if (jumpToSceneStart)
                {
                    PlayableDirector playableDirector = GetComponent<PlayableDirector>();
                    playableDirector.time = sceneClips[clipIndex].startTime + 1d; // putting the playhead one second after the clip start, to make sure that the playhead is in the intended scene
                }
            }

        }

        public void ChangeSceneOnGoingBackInTL(string sceneName, bool jumpToSceneStart, int clipIndex)
        {
            if(!CheckSceneChange(sceneName)) return;
            
            //if(sceneIndex == SceneManager.GetActiveScene().buildIndex) return;

            Debug.Log("SceneControlByTimeline: ChangeSceneOnGoingBackInTL, going to: " + sceneName + ". jump To Start of Clip: " + jumpToSceneStart);
            if (Application.isPlaying)
            {
                GetComponent<SceneChanger>().ChangeScene(sceneName);
                if (jumpToSceneStart)
                {
                    PlayableDirector playableDirector = GetComponent<PlayableDirector>();
                    playableDirector.time = sceneClips[clipIndex].startTime + 1d; // putting the playhead one second after the clip start, to make sure that the playhead is in the intended scene
                }
            }

        }

        public void SetupSceneParameters(int clip, int sceneIndex, string name, double startTime, bool jumpOnStartOnGoingBack, string roomSection)
        {
            if (clip >= sceneClips.Length)
            {
                Debug.LogError("ScenecontrolBytimeline: SetScene: index out of bounds, please create new Scene array element in Inspector");
                return;
            }
            sceneClips[clip].name = name;
            sceneClips[clip].sceneIndex = sceneIndex;
            sceneClips[clip].startTime = startTime;
            sceneClips[clip].jumpOnStartOnGoingBack = jumpOnStartOnGoingBack;
            sceneClips[clip].roomSection = roomSection;

        }

        public void SetCurrentSceneClip(int index)
        {
            currentSceneClip = index;
        }

        //// get by component

        public int GetCurrentSceneClip()
        {
            return currentSceneClip;
        }



        public double GetStartTime(int sceneClip)
        {
            int index = sceneClip;

            if (index >= sceneClips.Length)
            {
                index = sceneClips.Length - 1;
            }

            else if (index < 0)
            {
                index = 0;
            }

            Debug.Log("clip index: " + index);

            return sceneClips[index].startTime;
        }

        public string GetRoomSectionThisClip() // used for naming the room
        {
            if (currentSceneClip == -1) return "defaultSection";
            return sceneClips[currentSceneClip].roomSection;
        }


    }
}
