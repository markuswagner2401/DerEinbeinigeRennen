using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.VFXSpace
{

    public class FloatFXController : MonoBehaviour
    {

        [SerializeField] VisualEffect visualEffectPrefab;
        VisualEffect visualEffect;

        [SerializeField] float turbulenceIntensity;
        [SerializeField] XRController xRController;
        [SerializeField] float activationThreshold = 0.01f;
        [SerializeField] float fxForwardFactor = 10;
        [SerializeField] float spawnRate = 10f;

        bool eventAlreadySend = false;

        // Start is called before the first frame update
        void Start()
        {
            visualEffect = Instantiate(visualEffectPrefab, transform.position, transform.rotation);

        }

        // Update is called once per frame
        void Update()
        {


            if (xRController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > activationThreshold)
            {
                float forwardScaler = triggerValue * fxForwardFactor;
                Vector3 currentVelocity = xRController.transform.forward * forwardScaler;

                visualEffect.SetFloat("Ratefloat", spawnRate);
                visualEffect.SetVector3("Position", xRController.transform.position);
                visualEffect.SetVector3("Velocity", currentVelocity);


            }

            if (xRController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue2) && triggerValue2 < activationThreshold)
            {
                visualEffect.SetFloat("Ratefloat", 0);

            }




        }
    }

}
