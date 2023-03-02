using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingReferenceForward : MonoBehaviour
{
    [SerializeField] Transform xrRig;
    float capturedYRotation;

    [SerializeField] Vector3 eulerAngeles;
    
    private void Awake() {
        
    }

    void Start()
    {
        capturedYRotation = xrRig.localEulerAngles.y;
    }

    
    void Update()
    {
        transform.localEulerAngles = new Vector3(0,  - xrRig.localEulerAngles.y + capturedYRotation, 0);

        eulerAngeles = xrRig.localEulerAngles;
    }

    public Vector3 GetReferenceForward(){
        return transform.forward;
    }

    public Quaternion GetReferenceRotation(){
        return transform.rotation;
    }
}
