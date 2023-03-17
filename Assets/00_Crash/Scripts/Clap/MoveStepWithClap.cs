using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.Looping;

namespace ObliqueSenastions.ClapSpace
{
    public class MoveStepWithClap : MonoBehaviour
    {
        [SerializeField] XRLoopingMover mover = null;
        [SerializeField] LoopingControllerForwardVector forward = null;
        [SerializeField] AnimationCurve curve;
        [SerializeField] float stepTime = 2f;
        [SerializeField] float velocityFactor = 1f;


        [SerializeField] Transform moveAxisTransform = null;
        [SerializeField] Vector3 moveAxis;
        [SerializeField] bool moveOnAxis = true;

        float stepSpeed;
        Vector3 movingDirection = new Vector3();

        void Start()
        {
            if (mover == null)
            {
                mover = GetComponent<XRLoopingMover>();
            }

            if (moveAxisTransform != null)
            {
                moveAxis = moveAxisTransform.forward;
            }

        }

        public void MoveClapStep(float strength)
        {
            //        print("move with" + strength);
            StartCoroutine(StepMoveRoutine(strength));
        }


        IEnumerator StepMoveRoutine(float strength)
        {

            

            if (moveOnAxis)
            {
                if ((Vector3.Dot(movingDirection, moveAxis) > 0f))
                {
                    movingDirection = moveAxis;
                }

                else
                {
                    movingDirection = -moveAxis;
                }
            }

            else
            {
                movingDirection = forward.GetControllerForward();
            }


            float timer = 0f;

            while (timer < stepTime)
            {
                timer += Time.deltaTime;
                stepSpeed = Mathf.Clamp(strength, 0, 1f) * curve.Evaluate(timer / stepTime);
                mover.Move(movingDirection * stepSpeed * velocityFactor);
                yield return null;
            }

            yield break;
        }

        public void SetDirection(Vector3 direction)
        {
            print("set direction");
            movingDirection = direction;
        }

        public void SetVelocityFactor(float value)
        {
            velocityFactor = value;
        }





    }

}


