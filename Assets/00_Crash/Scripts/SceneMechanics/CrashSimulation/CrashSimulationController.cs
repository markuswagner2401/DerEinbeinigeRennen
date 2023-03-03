using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.AnimatorSpace
{

    public class CrashSimulationController : MonoBehaviour
    {

        [SerializeField] Animator animator;
        [SerializeField] float defaultSpeed = 1f;
        [SerializeField] float totalFrames;
        [SerializeField] float startFrame;
        [SerializeField] string defaultStateName;

        float speed;

        void Start()
        {
            animator = GetComponent<Animator>();
            StartAnimation("", startFrame);
        }


        void Update()
        {
            animator.SetFloat("Speed", speed);
        }

        public void StartAnimation(string stateName, float currentStartFrame)
        {
            string currentStateName;
            if (stateName == "")
            {
                currentStateName = defaultStateName;
            }
            else
            {
                currentStateName = stateName;
            }

            animator.Play(currentStateName, -1, CalculateStartPoint(currentStartFrame));
        }


        public void SetSpeed(float currentSpeed)
        {
            speed = currentSpeed;
        }


        private float CalculateStartPoint(float _startFrame)
        {
            return _startFrame / totalFrames;
        }

    }

}
