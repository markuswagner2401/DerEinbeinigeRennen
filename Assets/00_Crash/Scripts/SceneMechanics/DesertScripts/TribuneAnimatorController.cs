using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.AnimatorSpace
{

    public class TribuneAnimatorController : MonoBehaviour
    {

        [SerializeField] Animator animator = null;
        [SerializeField] float smoothing = 0.1f;

        [SerializeField] float minSpeed = 0f;
        [SerializeField] float maxSpeed = 100f;

        Vector3 previousPosition = new Vector3();

        float previousSpeed;

        void Start()
        {
            if (animator == null) GetComponent<Animator>();
            previousPosition = transform.position;
        }


        void Update()
        {
            // Get Speed
            float speed = ((transform.position - previousPosition).magnitude) / Time.deltaTime;



            float smoothedSpeed = Mathf.Lerp(previousSpeed, speed, smoothing);



            SetAnimator(smoothedSpeed);

            previousSpeed = speed;
            previousPosition = transform.position;

        }

        private void SetAnimator(float speed)
        {
            float normSpeed = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
            animator.SetFloat("Moving", normSpeed);

        }
    }

}
