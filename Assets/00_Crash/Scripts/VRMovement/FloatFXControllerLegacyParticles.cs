using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FloatFXControllerLegacyParticles : MonoBehaviour
{
    
    [SerializeField] ParticleSystem particleSystemPrefab;
    new ParticleSystem particleSystem;
    
    [SerializeField] float turbulenceIntensity;
    [SerializeField] XRController xRController;
    [SerializeField] float activationThreshold = 0.01f;
    [SerializeField] float fxForwardFactor = 10;
    [SerializeField] float spawnRate = 10f;

    bool eventAlreadySend = false;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = Instantiate(particleSystemPrefab, transform.position, transform.rotation);
        if(xRController == null)
        {
            xRController = GetComponent<XRController>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        particleSystem.transform.position = transform.position;
        particleSystem.transform.rotation = transform.rotation;

        var particleSystemMain = particleSystem.main;
        var particleSystemEmission = particleSystem.emission;

        if (xRController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > activationThreshold ) 
        {
            float forwardScaler = triggerValue * fxForwardFactor;
            Vector3 currentVelocity = xRController.transform.forward * forwardScaler;

            particleSystemEmission.rateOverTime = triggerValue * spawnRate;
            //particleSystemMain.startLifetime = forwardScaler;
            particleSystemMain.startSpeed = forwardScaler;


        }

        if (xRController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue2) && triggerValue2 < activationThreshold)
        {
            particleSystemEmission.rateOverTime = 0; 

        }

    

        
    }
}
