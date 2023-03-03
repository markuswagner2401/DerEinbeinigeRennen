using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Debugging
{

    public class DrawGizmostest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }

}
