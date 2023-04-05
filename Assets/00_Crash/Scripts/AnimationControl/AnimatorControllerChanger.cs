using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.UISpace;


namespace ObliqueSenastions.AnimatorSpace
{
    public class AnimatorControllerChanger : MonoBehaviour
    {
        [System.Serializable]
        public struct AnimationChangers
        {
            public string note;
            public string boolParameter;
            public float duration;
            public AnimationClip clip;
        }

        [SerializeField] private string scoreDisplayTag;
        [SerializeField] private AnimationChangers[] animationChangers;

        private Animator animator;
        private AnimatorOverrideController animatorOverrideController;

        private bool interrupted;

        private void Start()
        {

            /// Listen To Countdown

            GameObject targetObject = GameObject.FindGameObjectWithTag(scoreDisplayTag);
            if (targetObject != null)
            {
                ScoreDisplay scoreDisplay = targetObject.GetComponent<ScoreDisplay>();
                if (scoreDisplay != null)
                {
                    scoreDisplay.OnCountdownEnd += OnCountdownMainPlayerEnd;
                }
                else
                {
                    Debug.LogError("ScoreDisplay component not found on the object with tag " + scoreDisplayTag);
                }
            }
            else
            {
                Debug.LogError("No object found with tag " + scoreDisplayTag);
            }

            /// Listen To StageMaster
            GameObject stageMasterGO = GameObject.FindWithTag("StageMaster");


            animator = GetComponent<Animator>();
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        private void OnDestroy()
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag(scoreDisplayTag);
            if (targetObject != null)
            {
                ScoreDisplay scoreDisplay = targetObject.GetComponent<ScoreDisplay>();
                if (scoreDisplay != null)
                {
                    scoreDisplay.OnCountdownEnd -= OnCountdownMainPlayerEnd;
                }
            }
        }

        public void OnCountdownMainPlayerEnd()
        {
            int index = Random.Range(0, animationChangers.Length);
            if (index >= 0 && index < animationChangers.Length)
            {
                AnimationChangers changer = animationChangers[index];
                animatorOverrideController[animatorOverrideController.animationClips[0]] = changer.clip;
                StartCoroutine(PlayAnimationR(changer.boolParameter, changer.duration));
            }
            else
            {
                Debug.LogError("Index out of range: " + index);
            }
        }

        public void PlayAnimation(string name)
        {
            int index = -1;

            for (int i = 0; i < animationChangers.Length; i++)
            {
                if (animationChangers[i].note != name)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0 && index < animationChangers.Length)
            {
                AnimationChangers changer = animationChangers[index];
                animatorOverrideController[animatorOverrideController.animationClips[0]] = changer.clip;

                StartCoroutine(InterruptAndWaitForNextFrame());
                StartCoroutine(PlayAnimationR(changer.boolParameter, changer.duration));
            }
            else
            {
                Debug.LogError("Animation not found: " + name);
            }
        }

        private IEnumerator PlayAnimationR(string boolParameter, float duration)
        {
            animator.SetBool(boolParameter, true);
            float timer = 0;
            interrupted = false;

            while (timer < duration && !interrupted)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            animator.SetBool(boolParameter, false);
        }

        private IEnumerator InterruptAndWaitForNextFrame()
        {
            interrupted = true;
            yield return null;
            interrupted = false;
        }




    }

}

