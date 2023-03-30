using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.PunNetworking;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR;


namespace ObliqueSenastions.TimelineSpace
{
    public class TimelineScroller2 : MonoBehaviour
    {
        [SerializeField] PlayableDirector playableDirector;

        [SerializeField] SyncPlayableDirector syncPlayableDirector = null;
        //   [SerializeField] float fastForwardSpeed = 0.001f;

        [SerializeField] TimelineTime timelineTime = null;

        [SerializeField] TimeModeMachine timeModeMachine = null;

        [SerializeField] float scrollFactor = 5f;

        double currentSpeed;

        double lastSpeed;





        void Start()
        {
            if (playableDirector == null)
            {
                playableDirector = GetComponent<PlayableDirector>();
            }

            if (timeModeMachine == null)
            {
                timeModeMachine = GetComponent<TimeModeMachine>();
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

       
        void Update()
        {
            if( !syncPlayableDirector.GetIsMine()) return;

            switch (timeModeMachine.GetTimelinePlayMode())
            {
                case TimelinePlayMode.Play:
                    currentSpeed = 1;
                    break;

                case TimelinePlayMode.Pause:
                    currentSpeed = 0f;
                    break;

                case TimelinePlayMode.Hold:
                    currentSpeed = 0f;
                    break;

                case TimelinePlayMode.FastForward:
                    currentSpeed = scrollFactor;
                    break;

                case TimelinePlayMode.FastBackward:
                    currentSpeed = -scrollFactor;
                    break;

                default:
                    break;
            }

            if (currentSpeed != lastSpeed)
            {
                playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(currentSpeed);
            }

            lastSpeed = currentSpeed;

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

    }







}




