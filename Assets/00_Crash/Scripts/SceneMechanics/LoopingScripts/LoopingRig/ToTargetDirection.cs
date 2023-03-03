using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Looping
{

    public class ToTargetDirection : MonoBehaviour
    {
        [SerializeField] Transform target;



        // Update is called once per frame
        void Update()
        {
            transform.forward = target.position - transform.position;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
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
