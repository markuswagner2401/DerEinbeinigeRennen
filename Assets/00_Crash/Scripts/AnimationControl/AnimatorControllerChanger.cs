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

        private void Start()
        {
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
            int index = Random.Range(0,animationChangers.Length);
            if (index >= 0 && index < animationChangers.Length)
            {
                AnimationChangers changer = animationChangers[index];
                animatorOverrideController[animatorOverrideController.animationClips[0]] = changer.clip;
                StartCoroutine(PlayAnimation(changer.boolParameter, changer.duration));
            }
            else
            {
                Debug.LogError("Index out of range: " + index);
            }
        }

        private IEnumerator PlayAnimation(string boolParameter, float duration)
        {
            animator.SetBool(boolParameter, true);
            yield return new WaitForSeconds(duration);
            animator.SetBool(boolParameter, false);
        }
    }

}

