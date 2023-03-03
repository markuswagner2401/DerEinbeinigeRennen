using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.Looping
{

    public class TeleportProjectile : MonoBehaviour
    {
        [SerializeField] float gravityStrength = -9.81f;
        [SerializeField] float fallFactor = 1f;
        // [SerializeField] LayerMask groundLayer;

        [SerializeField] string areaTag = "TeleportationArea";
        [SerializeField] int contactIndex = 0;
        //[SerializeField] float additionalHeight = 1f;
        [SerializeField] bool updateGravityDirectionOnHit = true;

        Vector3 gravityDirection = new Vector3();

        XROrigin xrRig = null;

        bool alreadyTeleportet = false;





        void Update()
        {
            GetComponent<Rigidbody>().AddForce(gravityDirection * gravityStrength, ForceMode.Force);

            // transform.position += gravityDirection * gravityStrength * Time.deltaTime * fallFactor;
        }


        private void OnCollisionEnter(Collision other)
        {


            if (other.gameObject.tag == areaTag)
            {


                if (alreadyTeleportet) return;

                xrRig.GetComponent<XRLoopingMover>().Teleport(other.GetContact(contactIndex).point, other.GetContact(contactIndex).normal);

                if (updateGravityDirectionOnHit)
                {
                    SetGravityDirection(other.GetContact(contactIndex).normal);
                }

                alreadyTeleportet = true;

                //Teleport(other.GetContact(contactIndex).point, other.GetContact(contactIndex).normal);
            }
        }

        public void SetGravityDirection(Vector3 currentGravityDirection)
        {
            gravityDirection = currentGravityDirection;
        }

        public void SetXRRig(XROrigin currentXrRig)
        {
            xrRig = currentXrRig;
        }

        // void Teleport(Vector3 destination, Vector3 normal)
        // {
        //     alreadyTeleportet = true;

        //     print("teleport to contact point");

        //     xrRig.transform.position = destination;
        //     xrRig.transform.up = normal;

        //Vector3 heightAdjustment = normal * xrRig.cameraInRigSpaceHeight * 1.2f;

        //Vector3 cameraDestination = destination + heightAdjustment;

        //xrRig.MoveCameraToWorldLocation(cameraDestination);
        // }
    }

}
