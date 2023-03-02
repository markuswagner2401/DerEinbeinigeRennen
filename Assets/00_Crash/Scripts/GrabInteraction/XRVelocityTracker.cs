using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRVelocityTracker : XRDirectInteractor
{
    private XRController controller = null;
    [SerializeField] XRRig xrRig = null;

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<XRController>();
    }

    public float GetSpeed()
    {
        
        return GetValue(CommonUsages.deviceVelocity).magnitude;
    }

    public float GetRotationSpeed()
    {
        
        return GetValue(CommonUsages.deviceAngularVelocity).magnitude;
    }

    public Vector3 GetVelocity()
    {
        return GetValue(CommonUsages.deviceVelocity);
    }

    public Vector3 GetVelocityInWorldSpace()
    {

        Vector3 velocity = GetValue(CommonUsages.deviceVelocity);
        if (xrRig == null) return velocity;

        Quaternion rotation = xrRig.transform.rotation;
        return rotation * velocity;


    }

    

    public Vector3 GetAngularVelocity()
    {
        return GetValue(CommonUsages.deviceAngularVelocity);
    }

    private Vector3 GetValue(InputFeatureUsage<Vector3> usage)
    {
        controller.inputDevice.TryGetFeatureValue(usage, out Vector3 value);
        return value;
    }

    
}
