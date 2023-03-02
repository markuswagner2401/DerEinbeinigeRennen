using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVelocityTracker : MonoBehaviour
{
    

    public float currentSpeed;

    public Vector3 currentVelocity = new Vector3();

    public Vector3 angularVelocitiy = new Vector3();

    public float angularSpeed;

    

    Vector3 previousPosition = new Vector3();

    Vector3 previousVelocity = new Vector3();

    Vector3 previousAngularVelocity = new Vector3();

    Quaternion previousRotation = new Quaternion();

    [SerializeField] float smoothing;

    void Start()
    {
        previousPosition = transform.position;
    }


    void Update()
    {
        // position

        currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        currentVelocity = Vector3.Lerp(previousVelocity, currentVelocity, smoothing);
        currentSpeed = currentVelocity.magnitude;

        // rotation

        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);
        deltaRotation.ToAngleAxis(out var angle, out var axis);
        angle *= Mathf.Deg2Rad;

        angularVelocitiy = (1.0f / Time.deltaTime) * angle * axis;
        angularVelocitiy = Vector3.Lerp(previousAngularVelocity, angularVelocitiy, smoothing);
        angularSpeed = angularVelocitiy.magnitude;

        // Capturing

        previousPosition = transform.position;
        previousVelocity = currentVelocity;

        previousRotation = transform.rotation;
        previousAngularVelocity = angularVelocitiy;
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }

    public Vector3 GetVelocity()
    {
        return currentVelocity;
    }

    public Vector3 GetAngularVelocity()
    {
        return angularVelocitiy;
    }

    public float GetAngularSpeed()
    {
        return angularSpeed;
    }

    // debugging

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + currentVelocity);
    }
}
