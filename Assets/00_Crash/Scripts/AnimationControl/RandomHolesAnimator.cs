using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.MaterialControl
{
    public class RandomHolesAnimator : MonoBehaviour
    {
        public Renderer rend = new Renderer();
        private MaterialPropertyBlock block;


        [SerializeField] FloatChanger[] floatChangers;

        [System.Serializable]
        struct FloatChanger
        {
            public string note;
            public string propertyName;

            public float randomMinValue;
            public float randomMaxValue;

            public bool getDefaultValueFromMaterial;
            public float defaultValue;

            public float currentValue;

            public bool fadeIn;
            public float fadeInTime;


            public bool fadeBackToDefault;
        }


        [SerializeField] float fadeBackTime = 1f;
        [SerializeField] AnimationCurve fadeBackCurve;

        [SerializeField] bool triggerAutomaticlyAtStart = false;
        [SerializeField] float frequencyMin = 3f;
        [SerializeField] float frequencyMax = 10f;

        [SerializeField] AnimationCurve fadeInCurve;

        float time = 0f;


        void Start()
        {

            StartCoroutine(TriggerAutomaticly(triggerAutomaticlyAtStart));

        }

        private void OnEnable()
        {
            block = new MaterialPropertyBlock();
            rend = GetComponent<Renderer>();

            for (int i = 0; i < floatChangers.Length; i++)
            {
                if (!floatChangers[i].getDefaultValueFromMaterial) continue;
                rend.GetPropertyBlock(block);
                floatChangers[i].defaultValue = block.GetFloat(floatChangers[i].propertyName);
            }
        }


        void Update()
        {


        }

        public void FadeInChanger0(float targetValue)
        {
            StartCoroutine(FadeIn(0, targetValue, floatChangers[0].fadeInTime));
        }

        IEnumerator FadeIn(int index, float targetValue, float fadeInTime)
        {
            float time = 0f;
            float valueAtStart = block.GetFloat(floatChangers[index].propertyName);

            while (time <= floatChangers[index].fadeInTime)
            {
                time += Time.deltaTime;
                floatChangers[index].currentValue = Mathf.Lerp(valueAtStart, targetValue, fadeInCurve.Evaluate(time / floatChangers[index].fadeInTime));
                block.SetFloat(floatChangers[index].propertyName, floatChangers[index].currentValue);
                rend.SetPropertyBlock(block);
                yield return null;
            }

            yield break;
        }

        public void ChangeFloatsRandomly()
        {
            for (int i = 0; i < floatChangers.Length; i++)
            {
                ChangeFloatRandomly(i);
            }
        }

        public void ChangeFloatRandomly(int floatIndex)
        {
            rend.GetPropertyBlock(block);

            float newValue = Random.Range(floatChangers[floatIndex].randomMinValue, floatChangers[floatIndex].randomMaxValue);

            floatChangers[floatIndex].currentValue = newValue;

            block.SetFloat(floatChangers[floatIndex].propertyName, floatChangers[floatIndex].currentValue);


            rend.SetPropertyBlock(block);

            if (floatChangers[floatIndex].fadeBackToDefault)
            {
                StartCoroutine(FadeBackToDefaultValue(floatIndex));
            }



        }

        private IEnumerator FadeBackToDefaultValue(int changerIndex)
        {

            float valueAtStart = block.GetFloat(floatChangers[changerIndex].propertyName);
            float timer = 0f;


            while (timer < fadeBackTime)
            {
                timer += Time.deltaTime;
                floatChangers[changerIndex].currentValue = Mathf.Lerp(valueAtStart, floatChangers[changerIndex].defaultValue, fadeBackCurve.Evaluate(timer / fadeBackTime));
                block.SetFloat(floatChangers[changerIndex].propertyName, floatChangers[changerIndex].currentValue);
                rend.SetPropertyBlock(block);
                yield return null;
            }

            yield break;
        }

        public void TriggerCorroutAutomaticly(bool value)
        {
            StartCoroutine(TriggerAutomaticly(value));
        }

        private IEnumerator TriggerAutomaticly(bool value)
        {

            while (value)
            {
                float randomWaitForSeconds = Random.Range(frequencyMin, frequencyMax);
                ChangeFloatsRandomly();
                yield return new WaitForSeconds(randomWaitForSeconds);
            }

            yield break;
        }
    }


}

