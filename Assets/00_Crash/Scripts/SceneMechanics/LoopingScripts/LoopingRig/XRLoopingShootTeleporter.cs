using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static OVRInput;

namespace ObliqueSenastions.Looping
{

    public class XRLoopingShootTeleporter : MonoBehaviour
    {
        [SerializeField] bool usingOVR;

        [System.Serializable]
        struct ShootTeleporter
        {
            public XRNode node;
            public InputDevice device;

            public Button ovrButton;





            public bool alreadyShot;
            public Transform shotTransform;
            public GameObject[] projectiles;
            public int currentProjectileIndex;
            public bool randomProjectile;
            public float shootSpeed;
            public float spinMin;
            public float spinMax;
        }

        bool devicesSet;


        [SerializeField] ShootTeleporter[] shootTeleporters;






        // [SerializeField] GameObject projectile = null;
        // [SerializeField] Transform shotTransform = null;
        [SerializeField] float shootThreshold = 0.1f;
        // [SerializeField] float shootSpeed = 10f;
        [SerializeField] XROrigin rig;



        XRLoopingMover loopingMover;





        void Start()
        {
            for (int i = 0; i < shootTeleporters.Length; i++)
            {
                shootTeleporters[i].device = InputDevices.GetDeviceAtXRNode(shootTeleporters[i].node);
                shootTeleporters[i].alreadyShot = false;

            }


            loopingMover = GetComponent<XRLoopingMover>();

            rig = GetComponent<XROrigin>();
        }


        void FixedUpdate()
        {
            if (!usingOVR)
            {
                if (!devicesSet)
                {
                    for (int i = 0; i < shootTeleporters.Length; i++)
                    {
                        shootTeleporters[i].device = InputDevices.GetDeviceAtXRNode(shootTeleporters[i].node);
                    }
                    devicesSet = true;
                    for (int j = 0; j < shootTeleporters.Length; j++)
                    {
                        if (!shootTeleporters[j].device.isValid)
                        {
                            devicesSet = false;
                        }
                    }

                }

            }

            else
            {
                OVRInput.FixedUpdate();
            }






            for (int i = 0; i < shootTeleporters.Length; i++)
            {
                if (!usingOVR)
                {
                    if (shootTeleporters[i].device.TryGetFeatureValue(CommonUsages.trigger, out float value) && value > shootThreshold)
                    {
                        print("trigger usage");
                        if (shootTeleporters[i].alreadyShot == true) return;

                        ShootTeleportProjectile(shootTeleporters[i]);
                        shootTeleporters[i].alreadyShot = true;
                    }

                    else
                    {
                        shootTeleporters[i].alreadyShot = false;
                    }

                }

                else
                {
                    if (OVRInput.GetDown(shootTeleporters[i].ovrButton))
                    {
                        print("ovr get button down");
                        ShootTeleportProjectile(shootTeleporters[i]);
                    }

                }

            }



        }

        private void ShootTeleportProjectile(ShootTeleporter teleporter)
        {
            float spinX = Random.Range(teleporter.spinMin, teleporter.spinMax);
            float spinY = Random.Range(teleporter.spinMin + 0.1f, teleporter.spinMax + 0.1f);
            float spinZ = Random.Range(teleporter.spinMin - 0.1f, teleporter.spinMin - 0.1f);

            teleporter.currentProjectileIndex = DefineRandomProjectile(teleporter);





            var teleportProjektile = Instantiate(teleporter.projectiles[teleporter.currentProjectileIndex], teleporter.shotTransform.position, teleporter.shotTransform.rotation);
            teleportProjektile.GetComponent<Rigidbody>().AddForce(teleporter.shotTransform.forward * teleporter.shootSpeed, ForceMode.Impulse);
            teleportProjektile.GetComponent<Rigidbody>().AddTorque(new Vector3(spinX, spinY, spinZ));
            RaycastHit groundHit = loopingMover.GetCurrentGroundInfo();
            teleportProjektile.GetComponent<TeleportProjectile>().SetGravityDirection(groundHit.normal);
            teleportProjektile.GetComponent<TeleportProjectile>().SetXRRig(rig);


        }

        private int DefineRandomProjectile(ShootTeleporter teleporter)
        {
            if (teleporter.randomProjectile)
            {

                int randomValue = Random.Range(0, teleporter.projectiles.Length);



                return randomValue;
            }

            return 0;
        }

        public void SetProjectileIndex(int index)
        {
            for (int i = 0; i < shootTeleporters.Length; i++)
            {
                shootTeleporters[i].randomProjectile = false;
                shootTeleporters[i].currentProjectileIndex = index;
            }
        }

        public void EnableRandomProjectileIndex()
        {
            for (int i = 0; i < shootTeleporters.Length; i++)
            {
                shootTeleporters[i].randomProjectile = true;
            }
        }
    }

}
