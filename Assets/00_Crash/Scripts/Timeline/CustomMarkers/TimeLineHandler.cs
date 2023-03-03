using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.MaterialControl;
using ObliqueSenastions.SceneSpace;
using ObliqueSenastions.StageMasterSpace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace ObliqueSenastions.TimelineSpace
{

    public class TimeLineHandler : MonoBehaviour
    {
        public static TimeLineHandler instance;

        //    [SerializeField] bool persitentTimeline = false;

        [SerializeField] float startTime = 0f;

        [SerializeField] UnityEvent doOnInitialize;

        [Tooltip("if true jumps to 'startTime' ")]
        [SerializeField] bool jumpToStartFrameOfScene = false;

        [SerializeField] AudioSource audioSource = null;



        [SerializeField] bool fadeInAudioAtStart = true;

        [SerializeField] float fadeInTime;
        [SerializeField] AnimationCurve fadeInCurve;
        bool audioFadeInterrupted = false;

        [SerializeField] int viewId = 100;




        bool initialized = false;
        // [SerializeField] StageMaster stageMaster;

        private void Awake()
        {
            // singleton pattern



            if (instance == null)
            {
                instance = this;

                PhotonView photonView = GetComponent<PhotonView>();
                if (photonView != null)
                {
                    print("set photon view to: " + viewId);
                    photonView.ViewID = 0;
                    photonView.ViewID = viewId;
                    // photonView.ViewID = 0;
                    // if (photonView.ViewID == 0)
                    // {
                    //     PhotonNetwork.AllocateViewID(photonView);
                    // }
                }


                DontDestroyOnLoad(gameObject);
            }

            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            //         TimeLineHandler[] timelineHandlers = FindObjectsOfType<TimeLineHandler>();
            //         {
            // //            print("foundTimelines: " + timelineHandlers.Length);
            //             foreach (var timeline in timelineHandlers)
            //             {

            //                 if(timeline != this && timeline.GetInitialized())
            //                 {
            //                     print("found other timeline that is initialized");
            //                     Destroy(gameObject);
            //                 }
            //             }
            //         }

            //        stageMaster = FindObjectOfType<StageMaster>();

            // if(persitentTimeline)
            // {
            //     DontDestroyOnLoad(gameObject);

            // }

            if (!initialized)
            {
                InitializeTimeline();
            }



            if (fadeInAudioAtStart)
            {
                audioSource.volume = 0f;
                FadeAudio(1f, fadeInTime, fadeInCurve);
            }





        }

        // public bool GetInitialized()
        // {
        //     return initialized;
        // }


        void InitializeTimeline()
        {


            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }


            startTime = jumpToStartFrameOfScene ? startTime : 0;

            GetComponent<PlayableDirector>().time = startTime;

            doOnInitialize.Invoke();

            if (fadeInAudioAtStart)
            {
                FadeAudio(1f, fadeInTime, fadeInCurve);
            }

            initialized = true;

        }

        public void FadeAudio(float targetValue, float duration, AnimationCurve curve)
        {
            StartCoroutine(InterruptAndFadeAudioR(targetValue, duration, curve));
        }

        IEnumerator InterruptAndFadeAudioR(float targetValue, float duration, AnimationCurve curve)
        {
            audioFadeInterrupted = true;
            yield return new WaitForSeconds(0.1f);
            audioFadeInterrupted = false;
            StartCoroutine(FadeAudioR(targetValue, duration, curve));
            yield break;
        }


        IEnumerator FadeAudioR(float targetValue, float duration, AnimationCurve curve)
        {
            float startValue = audioSource.volume;
            float timer = 0f;

            while (timer < duration && !audioFadeInterrupted)
            {
                timer += Time.unscaledDeltaTime;
                float newValue = Mathf.Lerp(startValue, targetValue, curve.Evaluate(timer / duration));
                audioSource.volume = newValue;

                yield return null;
            }

            yield break;
        }

        public void FadeIn(float fadeInTime)
        {

            FindStageMaster().FadeIn(fadeInTime); //TO DO: Implement FadeIn
        }



        public void FadeOut(float fadeOutTime)
        {

            FindStageMaster().FadeOut(fadeOutTime); // TODO: Implement FadeOut
        }

        public void GoToNextScene()
        {
            //     if (stageMaster == null)
            //     {
            //         FindStageMaster().GoToNextScene();
            //     }
            //    stageMaster.GoToNextScene();

            GetComponent<SceneChanger>().GoToNextScene();
        }

        public void ChangeRig()
        {
            // if (stageMaster == null)
            // {
            //     FindStageMaster().SetNextRig();
            // }
            FindStageMaster().SetNextRig();
        }

        public void SetVRRig(int index, bool changeDuration, float duration, bool changeCurve, AnimationCurve curve)
        {

            FindStageMaster().SetVRRig(index, changeDuration, duration, changeCurve, curve);
        }

        public void NextGoEvent()
        {
            // if (stageMaster == null)
            // {
            //     FindStageMaster().PlayNextGoEvent();
            // }

            FindStageMaster().PlayNextGoEvent();
        }

        //    public void StartCommand()
        //    {
        //         // if (stageMaster == null)
        //         // {
        //         //     FindStageMaster().onStartStopCommand(true);
        //         // }
        //         FindStageMaster().onStartStopCommand(true);
        //    }

        //    public void StopCommand()
        //    {
        //         // if (stageMaster == null)
        //         // {
        //         //     FindStageMaster().onStartStopCommand(false);
        //         // }
        //         FindStageMaster().onStartStopCommand(false);
        //    }

        public void FadeInNextSky()
        {

            FindStageMaster().gameObject.GetComponent<SkyVJ>().FadeToNextSky();



        }

        //    public void PlayNextSpaceState()
        //    {
        //         // if(stageMaster == null)
        //         // {
        //         //     FindStageMaster().gameObject.GetComponent<BlendShapesStageMaster>().PlayNextSpaceState();
        //         // }
        //         FindStageMaster().gameObject.GetComponent<BlendShapesStageMaster>().PlayNextSpaceState();
        //         //print("play next space state");
        //    }

        //    public void Paukenschlag()
        //    {

        //         // if (stageMaster == null)
        //         // {
        //         //     FindStageMaster().onPaukenschlag.Invoke();
        //         // }
        //         stageMaster = FindStageMaster();
        //         if(stageMaster != null){
        //             stageMaster.onPaukenschlag.Invoke();
        //         } 

        //    }

        //    public void TriggerClapCountFX(int claps, float strength)
        //    {
        //         stageMaster = FindStageMaster();
        //         if(stageMaster != null){
        //             stageMaster.onTriggerClapcountFX(claps, strength);
        //         } 

        //    }

        private StageMaster FindStageMaster()
        {

            GameObject go = GameObject.FindGameObjectWithTag("StageMaster");
            if (go != null)
            {
                return go.GetComponent<StageMaster>();
            }

            Debug.Log("TimelineHandler couldnt find Stage Master");
            return null;
        }

    }

}
