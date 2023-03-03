using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.Looping;
using ObliqueSenastions.VRRigSpace;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.UISpace
{

    public class HUDFollowHead : MonoBehaviour
    {
        [SerializeField] Transform rig;

        [SerializeField] Transform head;
        [SerializeField] float smoothingRotStill = 0.1f;
        [SerializeField] float smoothingRotMoving = 0.99f;
        [SerializeField] float smoothingPosStill = 0.1f;
        [SerializeField] float smoothingPosMoving = 0.99f;

        float smoothingRot;
        float smoothingPos;
        [SerializeField] Transform hudTransform;
        // [SerializeField] Vector3 rotationOffset;
        // [SerializeField] Vector3 positionOffset;
        // [SerializeField] bool setupMode = false;
        [SerializeField] float rotationTolerance = 0.1f;
        [SerializeField] LoopingControllerForwardVector headForward;

        bool adjustingRotation = false;

        Vector3 previousRigPosition;
        Vector3 previousPosition;

        [SerializeField] float thresholdBeforePosAdjust = 0.1f;

        [Tooltip("Update by Traveller")]
        [SerializeField] bool manualUpdate = false;

        [SerializeField] cameraTraveller traveller = null;



        void Start()
        {
            // if(!setupMode)
            // {
            //     hudTransform.position += positionOffset;
            //     hudTransform.localEulerAngles = new Vector3(transform.localEulerAngles.x + rotationOffset.x, transform.localEulerAngles.y + rotationOffset.y, transform.localEulerAngles.z + rotationOffset.z);
            // }

            if (traveller == null)
            {
                traveller = GameObject.FindWithTag("Traveller").GetComponent<cameraTraveller>();
            }

            if (traveller != null)
            {
                traveller.onTravellerUpdateReady += ManualUpdate;
            }



        }


        void LateUpdate()
        {
            if (manualUpdate) return;
            ManualUpdate();

        }

        public void ManualUpdate()
        {
            if (RigIsMoving())
            {
                smoothingPos = smoothingPosMoving;
                smoothingRot = smoothingRotMoving;
            }

            else
            {
                smoothingPos = smoothingPosStill;
                smoothingRot = smoothingRotStill;
            }

            if ((Mathf.Abs(Vector3.Distance(previousPosition, head.position))) > thresholdBeforePosAdjust)
            {
                transform.position = Vector3.Lerp(transform.position, head.position, smoothingPos);
            }



            //transform.position = Vector3.Lerp(transform.position, rig.cameraGameObject.transform.position, smoothingPos);




            if (RotationOverThreshold())
            {
                if (adjustingRotation) return;
                StartCoroutine(AdjustRotation());
            }

            previousRigPosition = rig.transform.position;
            previousPosition = transform.position;




            // if(setupMode)
            // {
            //     hudTransform.position += positionOffset;
            //     hudTransform.localEulerAngles = new Vector3(transform.localEulerAngles.x + rotationOffset.x, transform.localEulerAngles.y + rotationOffset.y, transform.localEulerAngles.z + rotationOffset.z);
            // }
        }

        private bool RigIsMoving()
        {
            return (Mathf.Abs(Vector3.Distance(rig.transform.position, previousPosition))) > 0.1f;
        }

        private bool RotationOverThreshold()
        {
            Vector3 currentHeadForward = headForward.GetControllerForward();


            if ((Vector3.Dot(currentHeadForward.normalized,
                   transform.forward.normalized) > (1 - rotationTolerance)))
            {

                return false;
            }


            else
            {

                return true;
            }

        }

        IEnumerator AdjustRotation()
        {


            // Vector3 rigForwardOnPlane = new Vector3();
            // Vector3 forwardOnPlane = new Vector3();

            // rigForwardOnPlane = Vector3.ProjectOnPlane(rig.cameraGameObject.transform.forward, Vector3.up);
            // forwardOnPlane = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

            while (Vector3.Dot(headForward.GetControllerForward().normalized,
                   transform.forward) < (1 - 0.01f))
            {
                // print(Vector3.Dot(Vector3.ProjectOnPlane(rig.cameraGameObject.transform.forward, Vector3.up).normalized,
                //    Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized));

                adjustingRotation = true;
                transform.rotation = Quaternion.Lerp(transform.rotation, headForward.GetControllerRotation(), smoothingRot);

                // transform.localEulerAngles = new Vector3(rig.transform.localEulerAngles.x, transform.localEulerAngles.y, rig.transform.localEulerAngles.z);
                //transform.localEulerAngles = new Vector3(rig.transform.localEulerAngles.x, transform.localEulerAngles.y, rig.transform.localEulerAngles.z);

                yield return null;

            }



            adjustingRotation = false;
        }

        // private void OnDrawGizmos() 
        // {
        //     Gizmos.DrawRay(transform.position, Vector3.ProjectOnPlane(rig.cameraGameObject.transform.forward, Vector3.up));
        //     Gizmos.DrawRay (transform.position, Vector3.ProjectOnPlane(transform.forward, Vector3.up));
        // }

        // private bool RotationOverThreshold()
        // {
        //     Vector3 currentHeadForward = headForward.GetControllerForward();


        //     if ((Vector3.Dot(Vector3.ProjectOnPlane(rig.cameraGameObject.transform.forward, Vector3.up).normalized,
        //            Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized) > (1 - rotationTolerance)))
        //     {

        //         return false;
        //     }


        //     else
        //     {
        //         print("adjust rotation");
        //         return true;
        //     }
    }

}
