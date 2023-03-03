using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.InfiniteGround
{

    public class GroundPlatesCrashHandler : MonoBehaviour
    {

        [SerializeField] bool fallOnCollide = true;
        [SerializeField] GroundPlanesManager groundPlanesManager = null;
        [SerializeField] float radiusOfCrashHole = 50f;





        void Start()
        {
            if (fallOnCollide && groundPlanesManager == null)
            {
                groundPlanesManager = FindObjectOfType<GroundPlanesManager>();
            }

        }


        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag != "Player") return;

            print("collision with triggerer");

            Collider[] collidersAroundCrash = Physics.OverlapSphere(transform.position, radiusOfCrashHole);
            List<string> positionsAroundCrash = new List<string>();

            foreach (Collider collider in collidersAroundCrash)
            {
                if (collider.gameObject.tag != "GroundPlate") continue;
                positionsAroundCrash.Add(collider.transform.position.ToString("F0"));

            }

            groundPlanesManager.DestoyOnCrash(positionsAroundCrash);

        }


        void Update()
        {

        }



        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radiusOfCrashHole);
        }
    }

}
