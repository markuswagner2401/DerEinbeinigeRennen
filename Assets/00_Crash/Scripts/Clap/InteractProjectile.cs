using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.ClapSpace
{
    public class InteractProjectile : MonoBehaviour
    {
        [SerializeField] Rigidbody[] rigidbodies;

        public Rigidbody[] GetRigidbodies()
        {
            return rigidbodies;
        }
    }

}


