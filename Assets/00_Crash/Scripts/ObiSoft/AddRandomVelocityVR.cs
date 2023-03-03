using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using UnityEngine.XR;

namespace ObliqueSenastions.ObiControl
{

    [RequireComponent(typeof(ObiActor))]
    public class AddRandomVelocityVR : MonoBehaviour
    {
        [SerializeField] XRNode xRNode;

        InputDevice device;

        float triggerValue;

        bool alreadyFired;
        bool fire;

        private void Start()
        {
            device = InputDevices.GetDeviceAtXRNode(xRNode);
        }
        public float intensityUp = 5;
        public float intensityHorizontal = 1;

        void Update()
        {
            ProcessInput();

            if (fire)
            {
                GetComponent<ObiSoftbody>().AddForce(Vector3.up * intensityUp * Random.Range(-1f, 1f) + Vector3.left * Random.Range(-1f, 1f) * intensityHorizontal + Vector3.forward * Random.Range(-1f, 1f) * intensityHorizontal, ForceMode.VelocityChange);
            }



            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     GetComponent<ObiActor>().AddForce(UnityEngine.Random.onUnitSphere * intensity, ForceMode.VelocityChange);
            // }
        }

        private void ProcessInput()
        {
            if (device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue) && triggerValue > 0.1f)
            {
                if (!alreadyFired)
                {
                    fire = true;
                    alreadyFired = true;
                    print("random velocity");
                }

                else
                {
                    fire = false;
                }


            }

            else
            {
                alreadyFired = false;
                fire = false;
            }
        }
    }

}
