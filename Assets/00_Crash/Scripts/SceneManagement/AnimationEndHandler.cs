using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.AnimatorSpace
{

    public class AnimationEndHandler : MonoBehaviour
    {

        Animator animator;

        AnimatorClipInfo[] animatorClipInfos;

        [SerializeField] int animationLayerToHandle = 1;
        float clipLength;
        float clipPosition = 0;

        float normalizedClipPosition;

        float speed;

        [SerializeField] bool reverseAtEnd = true;
        [SerializeField] float reverseSpeedFactor;

        [SerializeField] bool reverseAtStart = true;

        bool animationPaused = false;
        [SerializeField] string paramterName = "Speed";

        void Start()
        {
            animator = GetComponent<Animator>();

            animatorClipInfos = animator.GetCurrentAnimatorClipInfo(animationLayerToHandle);

            clipLength = animatorClipInfos[0].clip.length;

            speed = animator.GetFloat(paramterName);

        }




        void Update()
        {

            clipPosition += Time.deltaTime * speed;

            normalizedClipPosition = Mathf.InverseLerp(0, clipLength, clipPosition);

            if (reverseAtEnd && normalizedClipPosition >= 1f)
            {


                ReverseClip();

            }

            if (reverseAtStart && normalizedClipPosition <= 0f)
            {
                ReverseClip();
            }
        }

        private void ReverseClip()
        {
            print("ReverseClip");
            speed *= (-1 * reverseSpeedFactor);
            animator.SetFloat(paramterName, speed);
        }
    }

}
