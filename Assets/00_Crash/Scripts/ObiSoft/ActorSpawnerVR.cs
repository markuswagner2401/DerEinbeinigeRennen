using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using UnityEngine.XR;
//using System;

namespace ObliqueSenastions.ObiControl
{

    public class ActorSpawnerVR : MonoBehaviour
    {

        public ObiActor template;

        [SerializeField] XRNode xRNode;
        InputDevice device;
        [SerializeField] bool VRMode;
        [SerializeField] bool randomRotation = true;
        [SerializeField] bool randomProperties;

        public int basePhase = 2;
        public int maxInstances = 32;
        public float spawnDelay = 0.3f;

        private int phase = 0;
        private int instances = 0;
        private float timeFromLastSpawn = 0;

        float triggerValue;

        bool alreadyFired = false;
        bool fire = false;

        private void Start()
        {
            device = InputDevices.GetDeviceAtXRNode(xRNode);
        }

        Quaternion rotation;

        // Update is called once per frame
        void Update()
        {
            ProcessInput();

            timeFromLastSpawn += Time.deltaTime;

            if (fire && instances < maxInstances && timeFromLastSpawn > spawnDelay)
            {
                CreateRotation();

                GameObject go = Instantiate(template.gameObject, transform.position, rotation);

                AddRandomProperties randomProperties = go.GetComponent<AddRandomProperties>();
                if (randomProperties != null && randomProperties)
                {
                    print("create random values");
                    randomProperties.CreateRandomValues();
                }

                go.transform.SetParent(transform.parent);

                //go.GetComponent<ObiActor>().SetPhase(basePhase + phase);



                phase++;
                instances++;
                timeFromLastSpawn = 0;
            }
        }

        private void CreateRotation()
        {
            if (randomRotation)
            {
                rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(-1f, 361f), Random.Range(-2, 362f));
            }

            else
            {
                rotation = Quaternion.identity;
            }
        }

        private void ProcessInput()
        {
            if (VRMode)
            {
                if (device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue) && triggerValue > 0.1f)
                {
                    if (!alreadyFired)
                    {
                        fire = true;
                        print("fire");
                        alreadyFired = true;
                    }

                    else
                    {
                        fire = false;
                    }

                }

                else
                {
                    fire = false;
                    alreadyFired = false;
                }
            }

            else
            {
                fire = Input.GetMouseButtonDown(0);
            }
        }
    }
}
