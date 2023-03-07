using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.OVRRigSpace
{
    public class OffsetTransform : MonoBehaviour
    {
        [SerializeField] Offset offsetOnHands;

        [SerializeField] Offset offsetOnControllers;


        [System.Serializable]
        struct Offset
        {
            public Vector3 position;
            public Vector3 eulers;
        }
      
        void Start()
        {

        }

     
        void Update()
        {
            if(OVRInput.GetActiveController() == OVRInput.Controller.Touch)
            {
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                transform.Translate(offsetOnControllers.position, Space.Self);
                transform.Rotate(offsetOnControllers.eulers, Space.Self);
                
            }

            else
            {
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                transform.Translate(offsetOnHands.position, Space.Self);
                transform.Rotate(offsetOnHands.eulers, Space.Self);
                
            }
        }
    }

}

