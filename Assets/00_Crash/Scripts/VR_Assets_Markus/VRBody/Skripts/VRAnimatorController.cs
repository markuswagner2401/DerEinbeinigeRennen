using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.VRRigSpace;

namespace ObliqueSenastions.AnimatorSpace
{

    public class VRAnimatorController : MonoBehaviour
    {

        float speedTreshold = 0.5f;
        Animator animator;
        Vector3 previousPos;
        Vector3 previousHeadsetSpeed;
        VRRig vRRig;


        float smoothing = 0.1f;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            vRRig = GetComponent<VRRig>();
            previousPos = vRRig.head.vrTarget.position;
        }

        // Update is called once per frame
        void Update()
        {
            //Compute Speed


            Vector3 headsetSpeed = (vRRig.head.vrTarget.position - previousPos) / Time.deltaTime;
            headsetSpeed.y = 0;

            //Vector3 headsetSpeedLerp = Vector3.Lerp(previousHeadsetSpeed, headsetSpeed, 0.01f);

            //previousHeadsetSpeed = headsetSpeedLerp;

            //print("headsetSpeedLerp = " + headsetSpeedLerp);
            //print(headsetSpeed);

            //Local Speed

            Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);

            previousPos = vRRig.head.vrTarget.position;

            // Set Animator Value

            float previousDirectionX = animator.GetFloat("DirectionX");
            float previousDirectionY = animator.GetFloat("DirectionY");

            animator.SetBool("isMoving", headsetLocalSpeed.magnitude > speedTreshold);
            animator.SetFloat("DirectionX", Mathf.Lerp(previousDirectionX, Mathf.Clamp(headsetLocalSpeed.x, -1, 1), smoothing));
            animator.SetFloat("DirectionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headsetLocalSpeed.z, -1, 1), smoothing));



        }
    }

}
