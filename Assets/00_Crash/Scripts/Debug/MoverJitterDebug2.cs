using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.VRRigSpace;
using UnityEngine;

namespace ObliqueSenastions.Debugging
{

    public class MoverJitterDebug2 : MonoBehaviour
    {
        [SerializeField] Transform source;
        [SerializeField] float smoothing = 0.1f;

        [SerializeField] Vector3 bufferPosition = new Vector3();

        [SerializeField] cameraTraveller traveller;
        // Start is called before the first frame update
        void Start()
        {
            traveller.onTravellerUpdateReady += MyUpdate;

        }

        private void MyUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, source.position, smoothing);
        }

        // Update is called once per frame
        void LateUpdate()
        {



        }




    }


}
