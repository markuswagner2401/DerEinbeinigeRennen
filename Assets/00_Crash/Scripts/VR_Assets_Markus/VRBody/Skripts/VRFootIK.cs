using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFootIK : MonoBehaviour
{
    Animator animator;

    [Range(0,1)]
    public float rightFootPosWeight = 1f;

    [Range(0, 1)]
    public float rightFootRotWeight = 1f;

    [Range(0, 1)]
    public float leftFootPosWeight = 1f;

    [Range(0, 1)]
    public float leftFootRotWeight = 1f;


    public Vector3 footOffset;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex) 
    
    {
        Vector3 rightFootPos = animator.GetIKPosition(AvatarIKGoal.RightFoot);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(rightFootPos + Vector3.up , Vector3.down , out hit);
        if(hasHit && hit.transform.tag != "BodyPart")
        {
            
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot , rightFootPosWeight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + footOffset);

            Quaternion rightFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotWeight);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);

        }
        else
        {
            
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }


        Vector3 leftFootPos = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        RaycastHit hitL;

        bool hasHitL = Physics.Raycast(leftFootPos + Vector3.up, Vector3.down, out hitL);
        if (hasHitL && hitL.transform.tag != "BodyPart")
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPosWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, hitL.point + footOffset);

            Quaternion leftFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitL.normal), hitL.normal);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotWeight);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, leftFootRotation);

        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }


    }

  

    // Update is called once per frame
    void Update()
    {
        
    }
}
