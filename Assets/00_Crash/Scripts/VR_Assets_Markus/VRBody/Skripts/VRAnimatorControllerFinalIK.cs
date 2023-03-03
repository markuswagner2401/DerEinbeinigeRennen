using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using static RootMotion.FinalIK.IKSolverVR;

namespace ObliqueSenastions.FinalIKControl
{

    public class VRAnimatorControllerFinalIK : MonoBehaviour
    {
        [Tooltip("PosedTracked Target")]
        [SerializeField] Transform headTarget;
        [SerializeField] bool combineVRIKLocomotion = true;

        [SerializeField] float speedThreshold = 0.6f;
        [SerializeField] float distanceThresholdforReset = 0.3f;
        [SerializeField]
        [Range(0, 1)]
        float smoothing = 0.1f;

        Animator animator;
        VRIK vrik;
        Locomotion locomotion;

        [SerializeField] float locomotionMaxWeight = 1f;
        [SerializeField] float locomotionMinWeight = 0f;

        bool isMoving = false;
        bool isWalking = false;

        bool locomotionIsReset = false;
        bool locomotionMaxWeightIsSet = false;
        bool locomotionMinWeightIsSet = false;
        bool animationIsActive = false;
        bool walkCheckIsReset = false;





        Vector3 previousHeadPos;
        Vector3 previousHeadsetSpeed = new Vector3();

        Vector3 previousTransformPos = new Vector3();

        Vector3 headPosAtWalkCheck = new Vector3();
        Vector3 headPosAtReset = new Vector3();






        // public Vector3 headBodyOffset;




        void Start()
        {
            animator = GetComponent<Animator>();

            previousHeadPos = headTarget.position;


            vrik = GetComponent<VRIK>();

            locomotion = vrik.solver.locomotion;


        }



        void FixedUpdate()
        {
            // update postion


            if (!combineVRIKLocomotion)
            {
                transform.position = new Vector3(headTarget.position.x, transform.position.y, headTarget.position.z);
                return;
            }

            // if combineVRIKLocomotion...

            if (isWalking)
            {
                previousTransformPos = transform.position;
                Vector3 newPosition = new Vector3(headTarget.position.x, transform.position.y, headTarget.position.z);
                transform.position = Vector3.Lerp(previousTransformPos, newPosition, smoothing);

            }








        }

        // Update is called once per frame
        void Update()
        {

            //Compute Speed


            Vector3 headsetSpeed = (headTarget.position - previousHeadPos) * Time.deltaTime * 5000;
            //Vector3 headsetSpeed = (headTarget.position - previousHeadPos) / Time.deltaTime;


            headsetSpeed.y = 0;



            //Local Speed

            Vector3 headsetLocalSpeed = Vector3.Lerp(previousHeadsetSpeed, transform.InverseTransformDirection(headsetSpeed), smoothing);

            previousHeadPos = headTarget.position;

            previousHeadsetSpeed = headsetLocalSpeed;



            if (!combineVRIKLocomotion)
            {
                SetAnimatorValue(headsetLocalSpeed, true);
                return;
            }

            // if combineVRIKLocomotion...



            isMoving = headsetLocalSpeed.magnitude > speedThreshold;

            isWalking = IsWalking();

            SetLocomotionWeight(isWalking);

            SetAnimatorValue(headsetLocalSpeed, isWalking);

        }


        private bool IsWalking()
        {
            if (walkCheckIsReset)
            {
                headPosAtWalkCheck = headTarget.transform.position;
                walkCheckIsReset = false;
            }

            if (!isMoving)
            {
                walkCheckIsReset = true;
                return false;
            }

            if (isMoving && Vector3.Distance(headPosAtWalkCheck, headTarget.transform.position) > distanceThresholdforReset)
            {
                return true;
            }

            return false;

        }

        private void SetAnimatorValue(Vector3 headsetLocalSpeed, bool isCurrentlyWalking)
        {
            if (isCurrentlyWalking)
            {

                // set animation value

                float previousDirectionX = animator.GetFloat("DirectionX");
                float previousDirectionY = animator.GetFloat("DirectionY");

                animator.SetBool("isMoving", headsetLocalSpeed.magnitude > speedThreshold);
                animator.SetFloat("DirectionX", Mathf.Lerp(previousDirectionX, Mathf.Clamp(headsetLocalSpeed.x, -1, 1), smoothing));
                animator.SetFloat("DirectionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headsetLocalSpeed.z, -1, 1), smoothing));

                animationIsActive = true;
            }

            else
            {
                if (animationIsActive)
                {
                    animator.SetBool("isMoving", false);
                    animationIsActive = false;
                }
            }

        }

        private void SetLocomotionWeight(bool isCurrentlyWalking)
        {
            if (isCurrentlyWalking)
            {


                //set locomotion min weight

                locomotion.weight = Mathf.Lerp(locomotion.weight, locomotionMinWeight, smoothing);



                locomotionIsReset = false;



            }

            if (!isCurrentlyWalking)
            {


                // reset locomotion

                // if (!locomotionIsReset)
                // {

                //     if (glitchyWalk) return;

                //     Transform[] references = vrik.references.GetTransforms();
                //     Vector3[] positions = new Vector3[references.Length];
                //     for (int i = 0; i < positions.Length; i++)
                //     {
                //         positions[i] = references[i].position;
                //     }
                //     Quaternion[] rotations = new Quaternion[references.Length];
                //     for (int i = 0; i < references.Length; i++)
                //     {
                //         rotations[i] = references[i].rotation;
                //     }

                //     locomotion.Reset(positions, rotations);

                //     locomotionIsReset = true;

                // }

                // set locomotion max weight

                if (locomotion.weight < locomotionMaxWeight - 0.01f)
                {

                    locomotion.weight = Mathf.Lerp(locomotion.weight, locomotionMaxWeight, smoothing);






                }











            }

        }
    }

}
