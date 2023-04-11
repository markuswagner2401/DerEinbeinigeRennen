using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Looping
{

    public class ToTargetDirection : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float threshold = 0.01f;

        Vector3 rawDirection = new Vector3();
        Vector3 lastDirection = new Vector3();

        Vector3 outputDirection = new Vector3();

        private void Start() 
        {
            rawDirection = target.position - transform.position;
            lastDirection = rawDirection;
        }

        // Update is called once per frame
        void Update()
        {
            //if(target.gameObject.activeInHierarchy == false) return;
            rawDirection = target.position - transform.position;
            outputDirection = (rawDirection.magnitude < threshold) ? lastDirection : rawDirection;
            
            transform.forward = outputDirection;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

            lastDirection = outputDirection;
        }

        public Vector3 GetParallelForward()
        {
            return transform.forward;
        }



        public Quaternion GetParallelRotation()
        {
            return transform.rotation;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }

}
