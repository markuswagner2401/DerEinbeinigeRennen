using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.AnimatorSpace;
using UnityEngine;

namespace ObliqueSenastions.Looping
{
    
public class LoopingKabine_Lenkrad : MonoBehaviour
{
    [SerializeField] HingeJoint steeringWheel = null;

    [SerializeField] CrashSimulationController crashSimulationController;

    [SerializeField] float maxSpeed = +10f;
    [SerializeField] float minSpeed = -10f;

    [SerializeField] float maxAngle = 180f;
    [SerializeField] float minAngle = -180f;

    [SerializeField] Transform rotationReference = null;

    
    void Start()
    {
        steeringWheel = GetComponent<HingeJoint>();
    }

    void Update()
    {
        

        

        

        if (crashSimulationController != null)
        {
            crashSimulationController.SetSpeed(CalculateSpeed(steeringWheel.angle));
            
            
        }
    }

    public float GetSteeringWheelAngle()
    {
        return steeringWheel.angle;
    }

    private float CalculateSpeed(float currentAngle)
    {
        
        float normal = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);
        float calculatedSpeed = Mathf.Lerp(minSpeed, maxSpeed, normal);
        return calculatedSpeed;
    }



    
    
}

}
