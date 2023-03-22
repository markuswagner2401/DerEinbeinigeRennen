using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;

namespace ObliqueSenastions.TransformControl
{
    public class SimpleLerpTransform : MonoBehaviour
    {
        [SerializeField] Transform source;

        [Range(0f, 1f)]
        [SerializeField] float weight;

        [SerializeField] bool rotation;

        [SerializeField] bool scale;

        [SerializeField] Lerp[] lerps;

        [System.Serializable]
        public struct Lerp
        {
            public string note;

            public float targetValue;

            public float duration;

            public AnimationCurve curve;
        }

        [SerializeField] bool captureValuesAtStart = true;

        [SerializeField] bool readAtTacho = false;
        [SerializeField] Tachonadel tacho;

        [SerializeField] float minSpeed;

        [SerializeField] float maxSpeed;

        [SerializeField] float speedFactor;

        bool setStartValuesTriggered = false;

        bool isInterrupted;

        bool isLerping = false;

        Vector3 capturedStartPosition = new Vector3();

        Quaternion capturedStartRotation = new Quaternion();

        Vector3 capturedStartScale = new Vector3();

        void Start()
        {
            if (captureValuesAtStart)
            {
                CaptureValuesAtStart();
            }



        }


        void Update()
        {
            if (!captureValuesAtStart)
            {
                SetStartValuesDynamicly();
            }


            if (readAtTacho && !isLerping)
            {
                float t = ReadWeightAtTacho(tacho);
                float speed = MapSpeed(minSpeed, maxSpeed, t);
                PlayWeight(speed * speedFactor);
            }

            if (weight <= 0f) return;
            transform.position = Vector3.Lerp(capturedStartPosition, source.position, weight);

            if (rotation)
            {
                transform.rotation = Quaternion.Lerp(capturedStartRotation, source.rotation, weight);
            }

            if (scale)
            {
                transform.localScale = Vector3.Lerp(capturedStartScale, source.localScale, weight);
            }






        }

        private float ReadWeightAtTacho(Tachonadel tacho)
        {
            return tacho.GetNormedTargetPosition();
        }

        // private void LateUpdate() {

        // }

        void SetStartValuesDynamicly()
        {

            if (weight > 0f)
            {
                if (setStartValuesTriggered) return;

                setStartValuesTriggered = true;

                capturedStartPosition = transform.position;

                capturedStartRotation = transform.rotation;

                capturedStartScale = transform.localScale;


            }

            else
            {
                setStartValuesTriggered = false;
            }

        }

        void CaptureValuesAtStart()
        {
            capturedStartPosition = transform.position;

            capturedStartRotation = transform.rotation;

            capturedStartScale = transform.localScale;

        }



        public void LerpWeight(int index)
        {
            StartCoroutine(InterruptAndLerpNext(index));

        }

        IEnumerator InterruptAndLerpNext(int index)
        {
            isInterrupted = true;
            yield return new WaitForSeconds(0.1f);
            isInterrupted = false;
            StartCoroutine(LerpWeightRoutine(index));
            yield break;

        }

        IEnumerator LerpWeightRoutine(int index)
        {
            isLerping = true;
            float startValue = weight;
            float targetValue = lerps[index].targetValue;
            float newValue;
            float timer = 0f;

            while (timer <= lerps[index].duration && !isInterrupted)
            {
                timer += Time.unscaledDeltaTime;
                newValue = Mathf.Lerp(startValue, targetValue, lerps[index].curve.Evaluate(timer / lerps[index].duration));
                weight = newValue;
                yield return null;

            }

            isLerping = false;
            yield break;

        }




        public void SetWeight(float value)
        {
            weight = value;
        }

        private float MapSpeed(float minSpeed, float maxSpeed, float t)
        {
            return Mathf.Lerp(minSpeed, maxSpeed, t);
        }


        public void PlayWeight(float speed)
        {
            weight = Mathf.Clamp01(weight + Time.deltaTime * speed);
            //print("play weight: " + weight + "with speed: " + speed);
        }
    }


}


