using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.TimelineSpace;
using UnityEngine;

namespace ObliqueSenastions.PunNetworking
{
    public class ObstacleAnimator : MonoBehaviour
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

        public event Action OnObstacleDestroyed;

        private void Start()
        {
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
            if (other.CompareTag("NetworkPlayer"))
            {
                DissolveObstacle();
                PlayAccident();
            }
        }

        public void DebugTriggerOnEnter()
        {
            DissolveObstacle();
            PlayAccident();
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

        private void PlayAccident()
        {
            TimeModeMachine timeModeMachine = TimeLineHandler.instance?.GetComponent<TimeModeMachine>();
            if (timeModeMachine != null)
            {
                timeModeMachine.PlayAccident(accidentName);
            }
            else
            {
                Debug.LogWarning("TimeModeMachine not found on TimeLineHandler instance");
            }
        }

        private IEnumerator AnimateBlendShape(int index)
        {
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
                OnObstacleDestroyed?.Invoke();
                Destroy(gameObject);
            }

            if (index == 0)
            {
                if (triggerCollider != null)
                {
                    triggerCollider.enabled = true;
                }
            }


        }
    }
}



