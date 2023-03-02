using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SphereScaler : MonoBehaviour
{
    [SerializeField] float scaleSpeed = 0.1f;
    [SerializeField] float minSize = 0.1f;
    [SerializeField] float increaseScaleSpeedOverTime = 1f;

    [SerializeField] XRNode inputSourceleft;
    [SerializeField] XRNode inputSourceRight;

    InputDevice inputDeviceLeft;
    InputDevice inputDeviceRight;

    Vector2 inputAxisL;
    Vector2 inputAxisR;


    bool scaleEnabled = false;

    float currentScaleSpeed;
    
    

    // Start is called before the first frame update
    void Start()
    {
        inputDeviceLeft = InputDevices.GetDeviceAtXRNode(inputSourceleft);
        inputDeviceRight = InputDevices.GetDeviceAtXRNode(inputSourceRight);

        currentScaleSpeed = scaleSpeed;
    }

    void Update()
    {
        if (inputDeviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxisL) && scaleEnabled)
        {
            float scaleAmount = inputAxisL.y;
            if (Mathf.Pow(scaleAmount,2) > 0.05f)
            {
                AddScale(scaleAmount);
            }
            
        }

        if (inputDeviceRight.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxisR) && scaleEnabled)
        {
            float scaleAmount = inputAxisR.y;

            if (Mathf.Pow(scaleAmount, 2) > 0.05f)
            {
                AddScale(scaleAmount);
            }
        }





    }

    public void EnableScale()
    {
        scaleEnabled = true;
    }

    public void DisableScale()
    {
        scaleEnabled = false;

        currentScaleSpeed = scaleSpeed;
    }

    private void AddScale(float scaleAmount)
    {

        currentScaleSpeed += Time.deltaTime * increaseScaleSpeedOverTime;

        transform.localScale = new Vector3 (transform.localScale.x + scaleAmount * currentScaleSpeed * Time.deltaTime,
                                            transform.localScale.y + scaleAmount * currentScaleSpeed * Time.deltaTime,
                                            transform.localScale.z + scaleAmount * currentScaleSpeed * Time.deltaTime);
    }



 

}
