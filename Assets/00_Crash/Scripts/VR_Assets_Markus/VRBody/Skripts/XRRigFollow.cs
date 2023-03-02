using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRRigFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float smoothing = 0.1f;

    Vector3 offset;
    
    void Start()
    {
        CalculateOffset();
    }

    private void CalculateOffset()
    {
        offset = target.position - transform.position;
    }


    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, (target.position - offset), smoothing ) ;
    }
}
