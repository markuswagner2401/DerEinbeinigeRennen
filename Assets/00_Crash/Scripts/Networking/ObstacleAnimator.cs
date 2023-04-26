using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.AudioControl;
using ObliqueSenastions.TimelineSpace;
using Photon.Pun;
using UnityEngine;

namespace ObliqueSenastions.PunNetworking
{
    public class ObstacleAnimator : MonoBehaviourPun
    {
        [System.Serializable]
        public struct BlendShapeChanger
        {
            public string name;
            public int blendShapeIndex;

            public float targetValue;
            public AnimationCurve curve;
            public float duration;
        }

        public SkinnedMeshRenderer skinnedMesh;
        public string accidentName;
        public BlendShapeChanger[] blendShapeChangers;
        public Collider triggerCollider;

        public Collider unfalltrigger = null;

        public event Action OnObstacleDestroyed;

        [SerializeField] SoundManager soundManager = null;

        [SerializeField] AudioSource crashAudioSource;

        [SerializeField] string crashSoundName = "Crash";

        [SerializeField] bool destroyAfterDissolve = false;

        [SerializeField] float reappearAfterSec = 10f;






        private void Start()
        {
            soundManager = GameObject.FindWithTag("SoundManager")?.GetComponent<SoundManager>();

            triggerCollider = GetComponent<Collider>();

            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }
            else
            {
                Debug.LogWarning("Collider not found on the Obstacle Animator GameObject");
            }

            skinnedMesh.SetBlendShapeWeight(1, 100f);

            StartCoroutine(AnimateBlendShape(0));
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "NetworkPlayer")
            {
                print("obstakle trigger: other network player");
                DissolveObstacle();
                //PlayAccident();
            }

            // if(other.gameObject.tag == "Player")
            // {
            //     print("obstakle trigger: other player");
            //     PlayCrashSound();
            // }
        }



        public void DebugTriggerOnEnter()
        {
            DissolveObstacle();
            PlayCrashSound();
            //PlayAccident();

        }

        public void DissolveObstacle()
        {
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }
            else
            {
                Debug.LogWarning("Trigger Collider not assigned in ObstacleAnimator");
            }
            StartCoroutine(AnimateBlendShape(1));

            OnObstacleDestroyed?.Invoke();
        }

        // private void PlayAccident()
        // {
        //     TimeModeMachine timeModeMachine = TimeLineHandler.instance?.GetComponent<TimeModeMachine>();
        //     if (timeModeMachine != null)
        //     {
        //         timeModeMachine.PlayAccident(accidentName);
        //     }
        //     else
        //     {
        //         Debug.LogWarning("TimeModeMachine not found on TimeLineHandler instance");
        //     }
        // }

        private IEnumerator AnimateBlendShape(int index)
        {
            print("Animate Blendshape: " + index);
            skinnedMesh.enabled = true;
            BlendShapeChanger blendShapeChanger = blendShapeChangers[index];
            float timer = 0f;
            int blendShapeIndex = blendShapeChanger.blendShapeIndex;
            float startValue = skinnedMesh.GetBlendShapeWeight(blendShapeIndex);
            float targetValue = blendShapeChanger.targetValue;

            while (timer < blendShapeChanger.duration)
            {
                timer += Time.deltaTime;
                float normalizedTime = timer / blendShapeChanger.duration;
                float curveValue = blendShapeChanger.curve.Evaluate(normalizedTime);
                float weight = Mathf.Lerp(startValue, targetValue, curveValue);
                skinnedMesh.SetBlendShapeWeight(blendShapeIndex, weight);
                yield return null;
            }

            if (index == 1)
            {
                if (destroyAfterDissolve)
                {
                    OnObstacleDestroyed?.Invoke();
                    //Destroy(gameObject);
                    gameObject.SetActive(false);
                    photonView.RPC("ActivateObject", RpcTarget.Others, false);
                }
                else
                {
                    triggerCollider.enabled = false;
                    unfalltrigger.enabled = false;
                    skinnedMesh.enabled = false;
                    StartCoroutine(ReappearRoutine(reappearAfterSec));
                }

            }

            if (index == 0)
            {
                if (triggerCollider != null)
                {
                    triggerCollider.enabled = true;
                    unfalltrigger.enabled = true;
                }


            }

        }

        IEnumerator ReappearRoutine(float duration)
        {
            print("reappear routine " + duration);
            yield return new WaitForSeconds(duration);
            
            StartCoroutine(AnimateBlendShape(0));
        }

        void PlayCrashSound()
        {
            GameObject.FindWithTag("SoundManager")?.GetComponent<SoundManager>().PlaySound(crashSoundName);
            // if(soundManager != null)
            // {
            //     //soundManager.PlaySound(crashSoundName, crashAudioSource);

            //     soundManager.PlaySound(crashSoundName);
            // }
        }

        [PunRPC]
        public void ActivateObject(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
        }
    }
}



