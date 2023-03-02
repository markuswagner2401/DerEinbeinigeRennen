using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GroundRotator : MonoBehaviour
{
    [SerializeField] CharacterController characterController = null;

    [SerializeField] CapsuleCollider characterCapsule;
    
    [SerializeField] LayerMask groundLayer;

    

    Vector3 currentLot;
    
    void Start()
    {
        
        if(characterCapsule == null)characterCapsule = GetComponent<CapsuleCollider>();
        
        currentLot = -transform.up;
    }

    
    void Update()
    {
        
        //hit detect ground

        // get normal of ground

        Vector3 normalDirection = GetNormal();

        transform.up = normalDirection;
        currentLot = normalDirection;

        Debug.DrawRay(transform.position, currentLot, Color.red);

        characterCapsule.transform.up = normalDirection;

        
        // rotate rig to ground
        
    }

    private Vector3 GetNormal()
    {
        Vector3 rayStart = transform.TransformPoint(characterCapsule.center);
        float rayLength = characterCapsule.center.y + 0.01f;

        Debug.DrawRay(rayStart, currentLot, Color.red);
        
        Physics.SphereCast(rayStart, characterCapsule.radius, -transform.up, out RaycastHit hitInfo, groundLayer);

        

        // Debug.DrawRay(hitInfo.point, hitInfo.normal);

        return hitInfo.normal;

        
    }
}
