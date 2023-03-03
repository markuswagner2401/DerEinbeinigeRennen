using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.Looping;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.VRRigSpace
{

    public class ContinuousMovementFloatingOnGround : MonoBehaviour
    {
        public float speedFactorController = 1;


        public XRController leftController;
        public XRController rightController;

        [SerializeField] LoopingControllerForwardVector leftForwardOnPlane;
        [SerializeField] LoopingControllerForwardVector rightForwardOnPlane;


        public float gravity = 0f;
        public LayerMask groundLayer;
        public float additionalHeight = 0.2f;

        [SerializeField] float lineLengthFactor = 10f;
        [SerializeField] float lineWith = 0.1f;

        [Tooltip("for Debugging")]
        [SerializeField] LineRenderer leftLineRenderer = null;
        [Tooltip("for Debugging")]
        [SerializeField] LineRenderer rightLineRenderer = null;

        private float fallingSpeed;
        bool isFloating = false;

        [SerializeField] XROrigin rig;

        private float triggerValue;
        [SerializeField] CharacterController character;

        [SerializeField] bool moveTowardsTarget = false;

        [SerializeField] float distanceToTarget = 2f;

        [SerializeField] ToTargetDirection toTargetDirection = null;

        [SerializeField] Transform target = null;

        [SerializeField] float speedFactorMoveTowardTarget = 0.01f;






        float lineLengthLeft = 0;
        float lineLengthRight = 0;

        bool stayOnGround = true;




        // Start is called before the first frame update
        void Start()
        {
            if (character == null)
            {
                character = GetComponent<CharacterController>();
            }

            if (rig == null)
            {
                rig = GetComponent<XROrigin>();
            }




        }

        // Update is called once per frame
        void Update()
        {
            CheckControlerInput();

        }

        public void SetSpeedFactor(float newSpeed)
        {
            speedFactorController = newSpeed;
        }

        private void CheckControlerInput()
        {
            if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
            {
                lineLengthLeft = triggerValue * lineLengthFactor;
            }

            else
            {
                lineLengthLeft = 0;
            }

            if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
            {
                lineLengthRight = triggerValue * lineLengthFactor;
            }

            else
            {
                lineLengthRight = 0;
            }


        }

        private void FixedUpdate()
        {
            CapsuleFollowHeadset();

            FollowTrigger();

            SinkToGround();

            MoveTowardsTarget();

        }

        void CapsuleFollowHeadset()
        {
            character.height = rig.CameraInOriginSpaceHeight + additionalHeight;
            Vector3 capsuleCenter = transform.InverseTransformPoint(rig.Camera.transform.position);
            character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
        }


        private void FollowTrigger()
        {



            if (lineLengthLeft + lineLengthRight > 0)
            {
                isFloating = true;

                //draw lines (debug)
                if (leftLineRenderer != null && rightLineRenderer != null)
                {
                    // Calculate Left Ray

                    Ray directionRayLeft = new Ray(leftController.transform.position, leftController.transform.forward);
                    Vector3 lineOriginL = leftController.transform.position;
                    Vector3 lineDirection = leftController.transform.forward;
                    Vector3 lineTargetL = lineOriginL + lineDirection * lineLengthLeft;

                    // Calculate Right Ray
                    Ray directionRayRight = new Ray(rightController.transform.position, rightController.transform.forward);
                    Vector3 lineOriginR = rightController.transform.position;
                    Vector3 lineDirectionR = rightController.transform.forward;
                    Vector3 lineTargetR = lineOriginR + lineDirectionR * lineLengthRight;



                    DrawDirectionLines(lineOriginL, lineTargetL, leftLineRenderer);
                    DrawDirectionLines(lineOriginR, lineTargetR, rightLineRenderer);
                }

                Vector3 directionSum = new Vector3();

                // average vector
                if (stayOnGround)
                {
                    directionSum = leftForwardOnPlane.GetControllerForward() * speedFactorController * lineLengthLeft +
                                    rightForwardOnPlane.GetControllerForward() * speedFactorController * lineLengthRight;

                }

                else
                {
                    directionSum = leftController.transform.forward * speedFactorController * lineLengthLeft +
                                        rightController.transform.forward * speedFactorController * lineLengthRight;
                }





                character.Move(directionSum * Time.fixedDeltaTime);
            }

            if (lineLengthLeft + lineLengthRight <= 0.01)
            {
                isFloating = false;
            }

        }

        private void DrawDirectionLines(Vector3 lineOrigin, Vector3 lineTarget, LineRenderer lineRenderer)
        {
            Vector3[] positions = new Vector3[2] { lineOrigin, lineTarget };

            lineRenderer.startWidth = lineWith;
            lineRenderer.endWidth = lineWith;

            lineRenderer.SetPositions(positions);
        }





        private void SinkToGround()
        {
            bool isGrounded = CheckIfGrounded();


            if (isGrounded || isFloating)
                fallingSpeed = 0;
            else
                fallingSpeed += gravity * Time.fixedDeltaTime;

            character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
        }

        bool CheckIfGrounded()
        {
            //tells us if on ground
            Vector3 rayStart = transform.TransformPoint(character.center);
            float rayLength = character.center.y + 0.01f;
            bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
            return hasHit;
        }

        //// Move Towards Target

        public void SetMoveTowardsTarget(bool value)
        {
            moveTowardsTarget = value;
        }


        void MoveTowardsTarget()
        {
            if (!moveTowardsTarget || ReferenceEquals(target, null)) return;

            if (Vector3.Distance(transform.position, target.position) < distanceToTarget) return;

            Vector3 direction = new Vector3();
            if (stayOnGround)
            {
                if (ReferenceEquals(toTargetDirection, null)) return;
                direction = toTargetDirection.GetParallelForward();

            }

            else
            {
                direction = (target.position - transform.position);
            }

            character.Move(direction * Time.fixedDeltaTime * speedFactorMoveTowardTarget);

        }




    }

}
