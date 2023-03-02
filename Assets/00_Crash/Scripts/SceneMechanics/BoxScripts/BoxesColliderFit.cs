using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesColliderFit : MonoBehaviour
{

    [SerializeField] Renderer renderer;

    [SerializeField] float sizeOfMesh = 10f;

    Vector3 scale = new Vector3();
    
 
    void Start()
    {
       
    }


    void Update()
    {
        scale.x = CalculateScaleFactor(renderer.bounds.extents.x);
        scale.y = CalculateScaleFactor(renderer.bounds.extents.y);
        scale.z = CalculateScaleFactor(renderer.bounds.extents.z);

        transform.localScale = scale;
        
    }

    private float CalculateScaleFactor(float boundExtend)
    {
        return (boundExtend * 2) / sizeOfMesh;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(renderer.bounds.center, renderer.bounds.extents.magnitude);
    }
}
