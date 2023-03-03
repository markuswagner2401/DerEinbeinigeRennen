using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.MaterialControl
{

    public class MaterialPropAnimator : MonoBehaviour
    {
        [SerializeField] string propertyName;
        [SerializeField] int matIndex;
        [SerializeField] AnimationCurve curve;
        [SerializeField] float animationTime = 3f;

        [SerializeField] float currentHoleSize = 0f;
        [SerializeField] Renderer renderer;

        [SerializeField] bool ossilateValue;
        [SerializeField] float ossilOffset;

        [SerializeField] float ossilSpeed = 1f;
        [SerializeField] bool jiggleValue;
        [SerializeField] float jiggleTimeMin = 0.2f;
        [SerializeField] float jiggletimeMax = 0.4f;
        [SerializeField] float jiggleMinOffset = -0.01f;
        [SerializeField] float jiggleMaxOffset = 0.01f;





        void Start()
        {
            renderer = GetComponent<Renderer>();



        }


        void Update()
        {


        }

        public void StartOssilate()
        {
            StartCoroutine(Ossilator());
        }

        public void StartJiggle()
        {
            StartCoroutine(Jiggle());
        }

        public void AnimateToValue(float value)
        {
            StartCoroutine(AnimateValueTo(value));
        }

        public IEnumerator AnimateValueTo(float targetValue)
        {

            float timer = 0f;
            float startValue = renderer.materials[matIndex].GetFloat(propertyName);
            while (timer < animationTime)
            {
                timer += Time.deltaTime;

                float currentValue = Mathf.Lerp(startValue, targetValue, curve.Evaluate(timer / animationTime));

                renderer.materials[matIndex].SetFloat(propertyName, currentValue);

                yield return null;

            }
            yield break;
        }

        public IEnumerator Ossilator()
        {
            ossilateValue = true;
            float time = 0f;
            float startValue = renderer.materials[matIndex].GetFloat(propertyName);



            print("startValue : " + startValue);
            print("first sin value = " + ((Mathf.Sin(time - (3.14159f / 2)) + 1f) / 2f));


            while (ossilateValue)
            {
                time += Time.deltaTime * ossilSpeed;
                float normSinValue = ((Mathf.Sin(time - (3.14159f / 2)) + 1f) / 2f);
                float currentOffset = ossilOffset * normSinValue;
                renderer.materials[matIndex].SetFloat(propertyName, startValue + currentOffset);
                yield return null;
            }

            yield break;
        }



        public IEnumerator Jiggle()
        {
            jiggleValue = true;

            while (jiggleValue)
            {
                float time = 0f;
                float currentJigglePeriod = Random.Range(jiggleTimeMin, jiggletimeMax);
                while (time < currentJigglePeriod)
                {
                    time += Time.deltaTime;
                    yield return null;
                }
                float startValue = renderer.materials[matIndex].GetFloat(propertyName);
                float randomOffset = Random.Range(jiggleMinOffset, jiggleMaxOffset);
                renderer.materials[matIndex].SetFloat(propertyName, startValue + jiggleMaxOffset);
                yield return null;

            }
            yield break;
        }

        public void StopOssilate()
        {
            ossilateValue = false;
        }

        public void StopJiggle()
        {
            jiggleValue = false;
        }
    }

}
