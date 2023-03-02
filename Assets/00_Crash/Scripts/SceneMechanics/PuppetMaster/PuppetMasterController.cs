using System.Collections;
using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.XR;

public class PuppetMasterController : MonoBehaviour
{
    [SerializeField] float pinStartValue = 1f;
    [SerializeField] float pinTargetValue = 0f;
    [SerializeField] float lerpTimePinOnStart = 10f;
    float currentPinValue;

    [SerializeField] float muscleStartValue = 1f;
    [SerializeField] float muscleTargetValue = 0f;
    [SerializeField] float lerpTimeMuscleOnStart = 10f;
    float currentMuscleValue;

    [SerializeField] PuppetMaster puppetMaster;

    [SerializeField] XRNode node;
    InputDevice device;
    bool alreadyPressed = false;



    void Start()
    {
        currentPinValue = pinStartValue;
        currentMuscleValue = muscleStartValue;

        device = InputDevices.GetDeviceAtXRNode(node);

        StartCoroutine(LerpMuscleToTargetValue(muscleTargetValue, lerpTimeMuscleOnStart));
        StartCoroutine(LerpPinToTargetValue(pinTargetValue, lerpTimePinOnStart));



    }


    void Update()
    {
        puppetMaster.pinWeight = currentPinValue;
        puppetMaster.muscleWeight = currentMuscleValue;

        // if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed))
        // {
        //     if (alreadyPressed) return;
        //     StartCoroutine(LerpMuscleToTargetValue(muscleTargetValue, lerpTimeMuscleOnStart));
        //     StartCoroutine(LerpPinToTargetValue(pinTargetValue, lerpTimePinOnStart));
        //     alreadyPressed = true;

        // }
    }






    public IEnumerator LerpMuscleToTargetValue(float targetValue, float time)
    {
        float currentTime = 0f;
        while (currentTime <= time)
        {
            currentTime += Time.deltaTime;
            currentMuscleValue = Mathf.Lerp(currentMuscleValue, targetValue, currentTime / time);
            yield return null;
        }
    }

    public IEnumerator LerpPinToTargetValue(float targetValue, float time)
    {
        float currentTime = 0f;
        while (currentTime <= time)
        {
            currentTime += Time.deltaTime;
            currentPinValue = Mathf.Lerp(currentPinValue, targetValue, currentTime / time);
            yield return null;
        }
    }
}
