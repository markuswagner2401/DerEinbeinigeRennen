using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.UISpace;


namespace ObliqueSenastions.AnimatorSpace
{
    public class AnimatorControllerChanger : MonoBehaviour
    {
        [SerializeField] bool playChangerGroupAtStart = false;

        [SerializeField] string startChangerGroup;


        [SerializeField] AnimationGroup[] animationGroups;

        [System.Serializable]
        public struct AnimationGroup
        {
            public string name;
            public AnimationClip originalClip;
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

            if (playChangerGroupAtStart)
            {
                //PlayAnimationOfAnimationGroup(startChangerGroup);
                //StartCoroutine(DebugGroupChange());
            }
        }

        // IEnumerator DebugGroupChange()
        // {
        //     while (true)
        //     {
        //         PlayAnimationOfAnimationGroup("Waving");
        //         yield return new WaitForSeconds(10f);
        //         PlayAnimationOfAnimationGroup("Idle");
        //         yield return new WaitForSeconds(10f);

        //     }


        // }





        public void PlayAnimationOfAnimationGroup(string groupName)
        {
            int groupIndex = GetGroupIndexByName(groupName);
            if (groupIndex == -1) return;
            if (animationGroups[groupIndex].animationChangers.Length == 0) return;
            int changerIndex = Random.Range(0, animationGroups[groupIndex].animationChangers.Length);
            StartCoroutine(InterruptAndPlayAnimationR(groupIndex, changerIndex));
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

        private IEnumerator InterruptAndPlayAnimationR(int groupIndex, int changerIndex)
        {
            interrupted = true;
            yield return new WaitForSeconds(0.1f);
            interrupted = false;

            AnimationChangers changer = animationGroups[groupIndex].animationChangers[changerIndex];

            animatorOverrideController[animationGroups[groupIndex].originalClip] = changer.clip;

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

