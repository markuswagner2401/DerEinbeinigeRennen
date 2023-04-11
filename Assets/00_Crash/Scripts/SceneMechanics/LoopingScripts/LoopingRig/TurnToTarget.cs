using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Looping
{
    public class TurnToTarget : MonoBehaviour
    {
        [SerializeField] Transform target;
        

        [SerializeField] float yRotationOffset;

        [SerializeField] float smoothing = 0.1f;
    
        void Start()
        {

        }

   
        void Update()
        {
            if(target != null)
            {
                transform.forward = (target.position - transform.position).normalized;

                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + yRotationOffset, 0);
            }
        }
    }

}


