using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.Looping
{

    public class VRCarKabine : MonoBehaviour
    {

        [SerializeField] Transform headForward;
        [SerializeField] XRRig rig;
        [SerializeField] float smoothing = 0.1f;
        [SerializeField] CapsuleCollider capsule;




        void Start()
        {

            //transform.position = headForward.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            //transform.position = Vector3.Lerp(transform.position, SetCurrentPosition(), smoothing);
            //transform.forward = Vector3.Lerp(transform.forward, headForward.forward, smoothing);
            Vector3 capsuleCenterWorld = new Vector3();
            capsuleCenterWorld = rig.transform.TransformPoint(capsule.center);
            transform.position = capsuleCenterWorld;
            transform.rotation = Quaternion.Lerp(transform.rotation, headForward.rotation, smoothing);


        }

        private Vector3 SetCurrentPosition()
        {
            Vector3 currentPosition = new Vector3();
            currentPosition = rig.cameraGameObject.transform.position;
            currentPosition.y = rig.transform.position.y;

            return currentPosition;

        }
    }

}
