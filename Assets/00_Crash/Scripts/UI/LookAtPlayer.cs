using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSensations.OVRRigSpace
{
    public class LookAtPlayer : MonoBehaviour
    {
        [SerializeField] Transform player = null;

        void Start()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Traveller").GetComponent<Transform>();
            }
        }


        void Update()
        {
            transform.forward = (player.transform.position - transform.position).normalized;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

}

