using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObliqueSenastions.TransformControl
{
    [ExecuteAlways]
    public class Array : MonoBehaviour
    {

        [SerializeField] Transform[] elements;
        [SerializeField] float radialOffset;

        

        float lastradialOffset;

        



        private void Update()
        {
            if (radialOffset == lastradialOffset) return;

            for (int i = 0; i < elements.Length; i++)
            {
                float offset = i * radialOffset;
                elements[i].eulerAngles = new Vector3(elements[0].eulerAngles.x, elements[0].eulerAngles.y + offset, elements[0].eulerAngles.z);
            }

            lastradialOffset = radialOffset;
        }
    }

}


