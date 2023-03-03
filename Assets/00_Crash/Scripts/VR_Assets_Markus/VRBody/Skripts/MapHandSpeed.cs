using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ObliqueSenastions.VRRigSpace;

namespace ObliqueSenastions.AnimatorSpace
{

    public class MapHandSpeed : MonoBehaviour
    {

        [SerializeField] XRVelocityTracker velocityTrackerLeft;
        [SerializeField] XRVelocityTracker velocityTrackerRight;


        float leftHandSpeed;
        float rightHandSpeed;

        [SerializeField] bool inverseBehaviour = false;

        [SerializeField] float changeValueMin = -0.001f;
        [SerializeField] float changeValueMax = 0.01f;

        [SerializeField] float handSpeedMin = 0;
        [SerializeField] float handSpeedMax = .5f;

        [SerializeField] float mappedValueLeftStart;
        [SerializeField] float mappedValueRightStart;

        float mappedValueLeft;
        float mappedValueRight;

        void Start()
        {
            if (inverseBehaviour)
            {
                changeValueMin *= -1;
                changeValueMax *= -1;
            }

            mappedValueLeft = mappedValueLeftStart;
            mappedValueRight = mappedValueRightStart;

        }


        void Update()
        {
            leftHandSpeed = velocityTrackerLeft.GetSpeed();
            rightHandSpeed = velocityTrackerRight.GetSpeed();

            if (IsBetween0And1(mappedValueLeft))
            {
                mappedValueLeft = Mathf.Clamp01(mappedValueLeft + MapChangeValue(leftHandSpeed));
            }

            if (IsBetween0And1(mappedValueRight))
            {
                mappedValueRight = Mathf.Clamp01(mappedValueRight + MapChangeValue(rightHandSpeed));
            }
        }

        public float GetMappedValueLeft()
        {
            return mappedValueLeft;
        }

        public float GetMappedValueRight()
        {
            return mappedValueRight;
        }

        private bool IsBetween0And1(float value)
        {
            if (value >= -0.01 && value <= 1.01)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        private float MapChangeValue(float handSpeed)
        {
            float handSpeedNorm = Mathf.InverseLerp(handSpeedMin, handSpeedMax, handSpeed);
            return Mathf.Lerp(changeValueMin, changeValueMax, handSpeedNorm);
        }
    }

}
