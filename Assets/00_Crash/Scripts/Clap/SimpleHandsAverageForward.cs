using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.ClapSpace
{
    public class SimpleHandsAverageForward : MonoBehaviour
    {
        [SerializeField] Transform leftHandForward = null;
        [SerializeField] Transform rightHandForward = null;

        






        void Update()
        {
            transform.position = Vector3.Lerp(leftHandForward.position, rightHandForward.position, 0.5f);
            transform.forward = (leftHandForward.forward + rightHandForward.forward).normalized;


        }

        
    }

}


