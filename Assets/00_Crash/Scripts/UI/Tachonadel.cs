using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.UISpace
{

    public class Tachonadel : MonoBehaviour
    {
        [SerializeField] float positionsMin = 0f;
        [SerializeField] float poitionsMax = 12f;

        [SerializeField] float targetPosition;


        [SerializeField] float rotMin = -90f;
        [SerializeField] float rotMax = 90f;

        [SerializeField] HingeJoint joint;





        JointSpring spring;

        float mappedRotation;

        void Start()
        {
            if (joint == null)
            {
                joint = GetComponent<HingeJoint>();
            }

            spring = joint.spring;
        }


        void Update()
        {
            spring.targetPosition = MappedRotation(targetPosition);
            joint.spring = spring;


        }

        public void SetTargetPosition(float position)
        {
            targetPosition = position;
        }

        public void SetTargetPositionNorm(float positionNorm) // Gets Set By XR Speed Peak Tracker
        {
            //print("Tachonadel: Set Target Position " + positionNorm);
            targetPosition = Mathf.Lerp(positionsMin, poitionsMax, positionNorm);

        }

        public float GetTargetPosition()
        {
            return targetPosition;
        }

        public float GetTargetPositionNorm()
        {
//            print("Tachonadel: get Target Position Norm. " + this.gameObject + "   " + targetPosition + "   " + Mathf.InverseLerp(positionsMin, poitionsMax, targetPosition));
            return Mathf.InverseLerp(positionsMin, poitionsMax, targetPosition);
        }





        private float MappedRotation(float position)
        {
            float normalizedPosition = Mathf.InverseLerp(positionsMin, poitionsMax, position);
            return Mathf.Lerp(rotMin, rotMax, normalizedPosition);
        }




    }

}
