using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


namespace ObliqueSenastions.VRRigSpace
{
    public class VelocityChangeDetector : MonoBehaviour
    {

        [SerializeField] SimpleVelocityTracker leftHandVelocity;
        [SerializeField] SimpleVelocityTracker rightHandVelocity;

        [SerializeField] UnityEventFloat onLeftVelocityChangedWithStrength;
        [SerializeField] UnityEvent onLeftVelocityChanged;

        [SerializeField] UnityEventFloat onRightVelocityChangedWithStrength;

        [SerializeField] UnityEvent onRightVelocityChanged;

        [SerializeField] UnityEvent onLeftHandIsMoving;

        [SerializeField] UnityEvent onLeftHandIsNotMoving;

        [SerializeField] UnityEventFloat onLeftHandIsMovingWithStrength;

        [SerializeField] UnityEvent onRightHandIsMoving;

        [SerializeField] UnityEvent onRightHandIsNotMoving;

        [SerializeField] UnityEventFloat onRightHandIsMovingWithStrength;

        //[SerializeField] UnityEventFloat outputNormalizedValue;

        [SerializeField] int velocityChangesLeft = 0;
        [SerializeField] int velocityChangesRight = 0;

        [SerializeField] TextMeshProUGUI tmpChangesCounter;


        [SerializeField] float deveationThreshold = 0.5f;

        [SerializeField] float speedThreshold = 0.1f;

        [SerializeField] float mapStrengthMin = 0f;
        [SerializeField] float mapStrengthMax = 20f;

        [SerializeField] float mapSpeedMin = 0;

        [SerializeField] float mapSpeedMax = 3f;

        Vector3 lastVelocityleft = new Vector3();

        Vector3 lastVelocityright = new Vector3();

        [SerializeField] int velocityTraceLength = 20;
        Vector3[] velocityTraceLeft;
        Vector3[] velocityTraceRight;

        [SerializeField] bool detectVelocityChange = true;

        // float outputValueNormalizedLeft;

        // float outputValueNormalizedRight;

        // float outputValueNormalizedCombined;

        void Start()
        {
            if(leftHandVelocity == null || rightHandVelocity == null) return;
            lastVelocityleft = leftHandVelocity.GetLocalVelocity().normalized;

            lastVelocityright = rightHandVelocity.GetLocalVelocity().normalized;

            velocityTraceLeft = new Vector3[velocityTraceLength];
            velocityTraceRight = new Vector3[velocityTraceLength];
        }


        void FixedUpdate()
        {
            if(leftHandVelocity == null || rightHandVelocity == null) return;
            //print("leftHand speed = " + leftHandVelocity.GetSpeed());
            //print("deveation " + Vector3.Dot(leftHandVelocity.GetVelocity().normalized, lastVelocityleft));
            if (leftHandVelocity.GetLocalSpeed() > speedThreshold)
            {
                onLeftHandIsMoving.Invoke();
                onLeftHandIsMovingWithStrength.Invoke(NormalizeSpeed(leftHandVelocity.GetSpeed()));
            }

            else
            {
                onLeftHandIsNotMoving.Invoke();
            }

            if (rightHandVelocity.GetLocalSpeed() > speedThreshold)
            {
                onRightHandIsMoving.Invoke();
                onRightHandIsMovingWithStrength.Invoke(NormalizeSpeed(rightHandVelocity.GetSpeed()));
            }

            else
            {
                onRightHandIsNotMoving.Invoke();
            }

            if(!detectVelocityChange) return; // return if only checcking speed

            CheckLeftVelocityChange();

            CheckRightVelocityChange();

            UpdateVelocityTraceLeft(leftHandVelocity.GetVelocity());

            UpdateVelocityTraceRight(rightHandVelocity.GetVelocity());

            if(tmpChangesCounter != null)
            {
                tmpChangesCounter.text = Mathf.Floor((velocityChangesLeft + velocityChangesRight) / 2).ToString();
            }
            

            // outputValueNormalizedLeft = Mathf.Lerp(0, outputValueNormalizedLeft, 0.001f);

            // outputValueNormalizedRight = Mathf.Lerp(0, outputValueNormalizedRight, 0.001f);

            // outputValueNormalizedCombined = (outputValueNormalizedLeft + outputValueNormalizedRight) / 2f;

            // outputNormalizedValue.Invoke(outputValueNormalizedCombined);

        }

        

        private void CheckLeftVelocityChange()
        {
            if (leftHandVelocity.GetSpeed() < speedThreshold) return;



            if (Vector3.Dot(leftHandVelocity.GetVelocity().normalized, lastVelocityleft) < deveationThreshold)
            {
                //print("Richtungswechsel links");
                onLeftVelocityChanged.Invoke();
                StartCoroutine(EvaluateVelocityTraceLeft());
            }

            lastVelocityleft = leftHandVelocity.GetVelocity().normalized;
        }

        private void CheckRightVelocityChange()
        {
            
            if (rightHandVelocity.GetSpeed() < speedThreshold) return;

            if (Vector3.Dot(rightHandVelocity.GetVelocity().normalized, lastVelocityright) < deveationThreshold)
            {
                //print("Richtungswechsel rechts");
                onRightVelocityChanged.Invoke();
                StartCoroutine(EvaluateVelocityTraceRight());

            }

            lastVelocityright = rightHandVelocity.GetVelocity().normalized;
        }

        IEnumerator EvaluateVelocityTraceLeft()
        {
            int frameCount = 0;

            while (frameCount < velocityTraceLength / 2)
            {
                frameCount += 1;
                yield return null;
            }

            float strength = NormalizeStrength(GetAverageVelocityLeft().magnitude);

            onLeftVelocityChangedWithStrength.Invoke(strength);

            velocityChangesLeft += 1;

            // SetOutputValueLeft(strength);

            // print("velocity change left with strength : " + GetAverageVelocityLeft().magnitude);

            yield break;
        }

        IEnumerator EvaluateVelocityTraceRight()
        {
            int frameCount = 0;

            while (frameCount < velocityTraceLength / 2)
            {
                frameCount += 1;
                yield return null;
            }

            float strength = NormalizeStrength(GetAverageVelocityRight().magnitude);

//            print("strength: " + strength);

            onRightVelocityChangedWithStrength.Invoke(strength);

            velocityChangesRight += 1;

            //SetOutputValueRight(strength);

            //print("velocity change right with strength : " + GetAverageVelocityRight().magnitude);

            yield break;
        }



        private void UpdateVelocityTraceLeft(Vector3 lastVelocity)
        {
            for (int i = 1; i < velocityTraceLeft.Length; i++)
            {

                velocityTraceLeft[i] = velocityTraceLeft[i - 1];
            }

            velocityTraceLeft[0] = lastVelocity;
        }

        private void UpdateVelocityTraceRight(Vector3 lastVelocity)
        {
            for (int i = 1; i < velocityTraceRight.Length; i++)
            {

                velocityTraceRight[i] = velocityTraceRight[i - 1];
            }

            velocityTraceRight[0] = lastVelocity;
        }

        private Vector3 GetAverageVelocityLeft()
        {
            Vector3 totalVelocity = new Vector3();
            totalVelocity = Vector3.zero;

            for (int i = 0; i < velocityTraceLeft.Length; i++)
            {
                totalVelocity += velocityTraceLeft[i];
            }

            return totalVelocity / velocityTraceLeft.Length;

        }

        private Vector3 GetAverageVelocityRight()
        {
            Vector3 totalVelocity = new Vector3();
            totalVelocity = Vector3.zero;

            for (int i = 0; i < velocityTraceRight.Length; i++)
            {
                totalVelocity += velocityTraceRight[i];
            }

            return totalVelocity / velocityTraceRight.Length;
        }

        private float NormalizeStrength(float input)
        {
            return Mathf.InverseLerp(mapStrengthMin, mapStrengthMax, input);
        }

        private float NormalizeSpeed(float input)
        {
            return Mathf.InverseLerp(mapSpeedMin, mapSpeedMax, input);
        }


        /// output values

        // void SetOutputValueLeft(float value)
        // {
        //     outputValueNormalizedLeft = value;
        // }

        // void SetOutputValueRight(float value)
        // {
        //     outputValueNormalizedRight = value;
        // }

        // public float GetOutputValueNormalizedLeft()
        // {
        //     return outputValueNormalizedLeft;
        // }

        // public float GetOutputValueNormalizedright()
        // {
        //     return outputValueNormalizedRight;
        // }

        // public float GetOutputValueNormalizedCombined()
        // {
        //     return outputValueNormalizedCombined;
        // }




    }

}

