using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRImpulseReceiverStraight : MonoBehaviour
{



    [SerializeField] float speedThreshold = 0.5f;
    
    [SerializeField] XRRig xrRig = null;
    [SerializeField] XRVelocityTracker velocityTracker = null;
    [SerializeField] XRController xRController = null;
    [SerializeField] float impulseFactor = 10f;

    bool impulseActive;





    void Update()
    {
        if (xrRig == null || velocityTracker == null) return;

        if ( velocityTracker.GetVelocity().magnitude > speedThreshold)
        {
            SetImpulse(velocityTracker.GetVelocityInWorldSpace());
            
        }

    }


    private void SetImpulse(Vector3 impulse)
    {
        

        GetComponent<Rigidbody>().AddForce(impulse * impulseFactor , ForceMode.Impulse);
        
    }


    public void VelocityImpulseActive(bool value) //???
    {
        transform.position = transform.parent.position;
        impulseActive = value;
    }



    private Vector3 CalculateForwardPush(Vector3 forwardDirection, Vector3 impulse)
    {
        Vector3 forwardPush = new Vector3();

        float forwardMagnitude = Vector3.Dot(forwardDirection.normalized, impulse);

        forwardPush = forwardDirection * forwardMagnitude;

        return forwardPush;

    }

    private Vector3 GetHeadForwardHeadDirection()
    {

        return xrRig.cameraGameObject.transform.forward;
    }

    private Vector3 GetFowardControllerDirection()
    {
        return xRController.transform.forward;

    }

}
