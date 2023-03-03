using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.Animation
{
    public class BlendShapeAnimator : MonoBehaviour
    {


        [SerializeField] float explodedBlendShapeValue = 100f;
        [SerializeField] float lerpDuration = 3f;


        [Range(0.0001f, 1f)]
        [SerializeField] float interpolationSmoothing = 0.1f;

        [SerializeField] bool isActiveAtBeginning = false;

        SkinnedMeshRenderer meshRenderer;
        float startBlendShapeValue;





        void Start()
        {

            meshRenderer = GetComponent<SkinnedMeshRenderer>();


            HideOrShowMeshAtStart();

        }






        void Update()
        {



        }



        private void HideOrShowMeshAtStart()
        {
            if (isActiveAtBeginning == false)
            {
                meshRenderer.enabled = false;
            }

            else
            {
                meshRenderer.enabled = true;
            }
        }



        public IEnumerator ReverseExplode()
        {
            EnableBlendShapeOscillator(false);

            meshRenderer.enabled = true;
            startBlendShapeValue = meshRenderer.GetBlendShapeWeight(0);
            meshRenderer.SetBlendShapeWeight(0, explodedBlendShapeValue);
            float currentLerpTime = 0f;

            while (currentLerpTime < lerpDuration)
            {
                currentLerpTime += Time.deltaTime;

                float t = currentLerpTime / lerpDuration;
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                float currentBlendShapeWeight = Mathf.Lerp(explodedBlendShapeValue, startBlendShapeValue, t);

                meshRenderer.SetBlendShapeWeight(0, currentBlendShapeWeight);

                yield return null;
            }

            meshRenderer.SetBlendShapeWeight(0, startBlendShapeValue);

            EnableBlendShapeOscillator(true);



        }

        private void EnableBlendShapeOscillator(bool value)
        {
            BlendShapeOscillator blendShapeOscillator = GetComponent<BlendShapeOscillator>();
            if (blendShapeOscillator == null) return;
            blendShapeOscillator.enabled = value;
        }
    }


}

