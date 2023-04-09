using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.VRRigSpace;

public class VeloTrackerDebug : MonoBehaviour
{
    [SerializeField] SimpleVelocityTracker veloTracker;
    [SerializeField] float factor;
    void Update()
    {
        transform.position = new Vector3(0, veloTracker.GetLocalSpeed() * factor, 0);
    }
}
