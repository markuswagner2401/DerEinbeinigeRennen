using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.VRRigSpace
{


    public class ContinuousMovementFloating : MonoBehaviour
    {
        public float speedFactor = 1;


        public XRController leftController;
        public XRController rightController;


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

        private XROrigin rig;

        private float triggerValue;
        private CharacterController character;




        float lineLengthLeft = 0;
        float lineLengthRight = 0;




        // Start is called before the first frame update
        void Start()
        {
            character = GetComponent<CharacterController>();
            rig = GetComponent<XROrigin>();



        }

        // Update is called once per frame
        void Update()
        {
            CheckControlerInput();

        }

        public void SetSpeedFactor(float newSpeed)
        {
            speedFactor = newSpeed;
        }

        public void SetGravity(float newGravity)
        {
            gravity = newGravity;
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
            //CapsuleFollowHeadset();

            FollowTrigger();

            SinkToGround();

        }

        void CapsuleFollowHeadset()
        {
            character.height = rig.CameraInOriginSpaceHeight + additionalHeight;
            Vector3 capsuleCenter = transform.InverseTransformPoint(rig.Camera.transform.position);
            character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
        }


        private void FollowTrigger()
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





            if (lineLengthLeft + lineLengthRight > 0)
            {
                isFloating = true;

                //draw lines (debug)
                if (leftLineRenderer != null && rightLineRenderer != null)
                {
                    DrawDirectionLines(lineOriginL, lineTargetL, leftLineRenderer);
                    DrawDirectionLines(lineOriginR, lineTargetR, rightLineRenderer);
                }


                // average vector
                Vector3 directionSum = leftController.transform.forward * speedFactor * lineLengthLeft +
                                        rightController.transform.forward * speedFactor * lineLengthRight;


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




    }

}
