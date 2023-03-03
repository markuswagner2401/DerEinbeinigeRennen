using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.InfiniteGround
{

    public class GroundPlate : MonoBehaviour
    {


        // [SerializeField] GroundPlanesManager groundPlanesManager = null;

        // [SerializeField] string positionDebug;

        int triggererIndex;

        float age = 0f;

        [SerializeField] bool initialPlate = false;

        bool activatet = true;










        private void Start()
        {

            activatet = true;

            // if(groundPlanesManager == null)
            // {
            //     groundPlanesManager = GetComponentInParent<GroundPlanesManager>();
            // }

            // triggererIndex = groundPlanesManager.GetIndex(transform.parent);

            // positionDebug = transform.position.ToString("F0");

        }


        void Update()
        {
            age += Time.deltaTime;
        }

        public float GetAge()
        {
            return age;
        }

        public int GetIndex()
        {
            return triggererIndex;
        }

        private void OnTriggerEnter(Collider other)
        {
            // if (groundPlanesManager == null) // backup
            // {
            //     groundPlanesManager = FindObjectOfType<GroundPlanesManager>();
            //     Debug.Log("Ground Planes Manager found late");
            // }

            activatet = true;

        }

        private void OnTriggerStay(Collider other)
        {
            if (!activatet) return;

            // if (groundPlanesManager == null) // backup
            // {
            //     groundPlanesManager = FindObjectOfType<GroundPlanesManager>();
            //     Debug.Log("Ground Planes Manager found late");
            // }

            if (other.gameObject.tag == "Player")
            {
                GroundPlanesTriggerer currentGroundPlanesTriggerer = other.GetComponentInParent<GroundPlanesTriggerer>();
                int triggererIndex = currentGroundPlanesTriggerer.GetIndex();
                GroundPlanesManager currentGroundPlanesManager = currentGroundPlanesTriggerer.GetGroundPlanesManager();
                currentGroundPlanesManager.HandleGroundRequest(gameObject, triggererIndex);
            }

            if (initialPlate)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            activatet = false;

        }







    }

}
