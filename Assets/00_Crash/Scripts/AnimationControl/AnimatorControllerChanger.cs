using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.UISpace;


namespace ObliqueSenastions.AnimatorSpace
{
    public class AnimatorControllerChanger : MonoBehaviour
    {
        [SerializeField] AnimationGroup[] animationGroups;

        [System.Serializable]
        public struct AnimationGroup
        {
            public string name;
            public AnimationChangers[] animationChangers;
        }

        [System.Serializable]
        public struct AnimationChangers
        {
            public string note;
            public string boolParameter;
            public float duration;
            public AnimationClip clip;
        }

        [SerializeField] private string scoreDisplayTag;
        

        private Animator animator;
        private AnimatorOverrideController animatorOverrideController;

        private bool interrupted;

        private void Start()
        {

            animator = GetComponent<Animator>();
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        

        

        public void PlayAnimationOfAnimationGroup(string groupName)
        {
            int groupIndex = GetGroupIndexByName(groupName);
            if (groupIndex == -1) return;
            if (animationGroups[groupIndex].animationChangers.Length == 0) return;
            int changerIndex = Random.Range(0, animationGroups[groupIndex].animationChangers.Length);
            AnimationChangers changer = animationGroups[groupIndex].animationChangers[changerIndex];
            StartCoroutine(InterruptAndPlayAnimationR(changer));


        }

        int GetGroupIndexByName(string name)
        {
            for (int i = 0; i < animationGroups.Length; i++)
            {
                if (name == animationGroups[i].name) return i;
            }

            Debug.LogError("AnimationControlChanger: No animation group found with name " + name);
            return -1;
        }

        private IEnumerator InterruptAndPlayAnimationR(AnimationChangers changer)
        {
            interrupted = true;
            yield return new WaitForSeconds(0.1f);
            interrupted = false;
            StartCoroutine(PlayAnimationR(changer.boolParameter, changer.duration));
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



    }

}

