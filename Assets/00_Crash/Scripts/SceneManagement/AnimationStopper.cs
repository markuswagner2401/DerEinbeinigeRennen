using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.AnimatorSpace
{

    public class AnimationStopper : MonoBehaviour
    {

        Animator animator;

        bool animationPaused = false;
        [SerializeField] string paramterName = "Speed";

        void Start()
        {
            animator = GetComponent<Animator>();

        }


        public void PauseAnimation()
        {
            animationPaused = !animationPaused;

            if (animationPaused)
            {
                animator.SetFloat(paramterName, 0);
            }

            else
            {
                animator.SetFloat(paramterName, 1);
            }
        }

        void Update()
        {

        }
    }

}
