using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.StageMasterSpace;
using UnityEngine;

namespace ObliqueSenastions.ClapSpace
{

    public class ClapCounterFX : MonoBehaviour
    {
        [SerializeField] Transform uiLocation;
        [SerializeField] Transform uiRig;
        [SerializeField] Transform xRRig;
        [SerializeField] Transform head;
        [SerializeField] float shootFactor = 5f;
        [SerializeField] bool adjustFactorToMass;

        [SerializeField] StageMaster stageMaster;

        void Start()
        {
            if (stageMaster != null)
            {
                //stageMaster.onTriggerClapcountFX += SpawnFX;
            }

            else
            {
                Debug.Log("no stage master assigned in " + gameObject.name + " -> no delgate action");
            }
        }


        void Update()
        {

        }

        public void SpawnFX(int poolIndex)
        {
            Vector3 spawnPoint = new Vector3();

            Vector3 localUIPosition = uiRig.InverseTransformPoint(uiLocation.position);
            spawnPoint = xRRig.position + (xRRig.transform.right * localUIPosition.x) + (xRRig.transform.up * localUIPosition.y) + (xRRig.transform.forward * localUIPosition.z);



            GameObject fx = ProjectilePool.Take(poolIndex, spawnPoint, Quaternion.LookRotation(head.forward));
            Rigidbody[] rbs = fx.GetComponent<InteractProjectile>().GetRigidbodies();
            foreach (var rb in rbs)
            {
                float massAdjustFactor = 1f;
                if (adjustFactorToMass)
                {
                    massAdjustFactor = rb.mass;
                }
                rb.AddForce(fx.transform.up * shootFactor + fx.transform.forward * 1f * massAdjustFactor, ForceMode.Impulse);
            }

        }

        public void SpawnFX(int poolIndex, float strength)
        {
            Vector3 spawnPoint = new Vector3();

            Vector3 localUIPosition = uiRig.InverseTransformPoint(uiLocation.position);
            spawnPoint = xRRig.position + (xRRig.transform.right * localUIPosition.x) + (xRRig.transform.up * localUIPosition.y) + (xRRig.transform.forward * localUIPosition.z);



            GameObject fx = ProjectilePool.Take(poolIndex, spawnPoint, Quaternion.LookRotation(head.forward));

            Rigidbody[] rbs = fx.GetComponent<InteractProjectile>().GetRigidbodies();
            foreach (var rb in rbs)
            {
                //rb.AddForce(fx.transform.up * shootFactor + fx.transform.forward * strength * shootFactor, ForceMode.Impulse);

                rb.AddForce(fx.transform.up * shootFactor + uiLocation.transform.forward * strength * shootFactor, ForceMode.Impulse);
            }

        }
    }


}


