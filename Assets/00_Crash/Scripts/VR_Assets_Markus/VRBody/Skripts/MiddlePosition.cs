using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TransformControl
{
    public class MiddlePosition : MonoBehaviour
    {
        [SerializeField] Transform posA;
        [SerializeField] Transform posB;
        [SerializeField] Transform posC;

        

        void Start()
        {

        }


        void Update()
        {
            transform.position = (posA.position + posB.position + posC.position) / 3f;
        }
    }

}

