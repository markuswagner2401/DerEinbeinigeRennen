using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.Looping
{

    public class LoopingControllerForwardVector : MonoBehaviour
    {

        [Tooltip("for hand put pointer pose in ray interactor")]
        [SerializeField] Transform controller;


        





        void Update()
        {
            

            transform.forward = controller.transform.forward;

            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);


        }

        public Vector3 GetControllerForward()
        {
            return transform.forward;
        }

        public Quaternion GetControllerRotation()
        {
            return transform.rotation;
        }
    }

}
