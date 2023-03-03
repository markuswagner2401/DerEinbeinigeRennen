using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.AnimatorSpace
{

    public class BoxesAnimationControl : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] float statesOnLiniearCurve = 10;

        [SerializeField] float targetState = 0;
        [SerializeField] float startState = 0;
        float currentState = 0;

        [SerializeField] bool initializeAtStart = true;
        [SerializeField] float waitBeforeInitialize = 5f;

        [SerializeField] float startSpeed = 5f;
        [SerializeField] float speedIncreaseSummand = 0f;
        [SerializeField] float speedIncreaseFactor = 1f;
        [SerializeField] float speed;


        [SerializeField] bool locked = false;
        bool capturedLocked;

        [SerializeField] bool controlledByOther = false;



        private void Awake()
        {
            animator = GetComponent<Animator>();
            currentState = startState;
            capturedLocked = locked;
            speed = startSpeed;
        }


        void Start()
        {
            SetAnimator(currentState);
        }


        void Update()
        {

            if (locked) return;

            if (!controlledByOther) LerpToTargetState();

            SetAnimator(currentState);

        }

        public float GetCurrentState()
        {
            return currentState;
        }

        public void SetCurrentState(float state)
        {
            currentState = state;
        }

        public void ChangeState(float state)
        {
            locked = false;
            targetState = state;
        }

        public void SetControlledByOther(bool value)
        {
            controlledByOther = value;
        }



        private void LerpToTargetState()
        {
            if ((Mathf.Pow(currentState - targetState, 2) > 0.001f))
            // if (!Mathf.Approximately(currentState, targetState))
            {

                currentState += Time.deltaTime * speed * Direction();

                speed += speedIncreaseSummand;
                speed *= speedIncreaseFactor;

            }

            else
            {
                currentState = Mathf.Round(currentState);
                speed = startSpeed;
                locked = capturedLocked;
            }
        }






        private int Direction()
        {
            if (targetState - currentState > 0) return 1;
            else return -1;

        }



        private void SetAnimator(float state)
        {
            float value = (1 / statesOnLiniearCurve) * state;

            animator.SetFloat("BoxesControl", value);
        }


    }

}
