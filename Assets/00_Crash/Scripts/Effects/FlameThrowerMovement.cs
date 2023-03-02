using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FlameThrowerMovement : MonoBehaviour
{
    ParticleSystem particleSystem;
    [SerializeField] XRNode node;
    InputDevice device;

    float exhaustMinValue = 0.1f;
    float effectFactor = 30f;

 
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(node);
        print("device found " + device.name);
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.01f)
        {
            DriveEffect(exhaustMinValue, triggerValue * effectFactor);
        }
        else
        {
            DriveEffect(0,0);
        }
    }

    void DriveEffect(float minValue,float maxValue)
    {
        var emission = particleSystem.emission;
        emission.rateOverTime = Random.Range(minValue, maxValue);
    }
}
