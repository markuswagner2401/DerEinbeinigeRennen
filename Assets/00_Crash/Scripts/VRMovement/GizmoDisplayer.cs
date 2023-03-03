using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.GizmoControl
{

    public class GizmoDisplayer : MonoBehaviour
    {
        [SerializeField] Visual visual = Visual.cube;
        enum Visual
        {
            cube,

            sphere,

        }
        private void OnDrawGizmos()
        {
            if (visual == Visual.cube)
            {
                Gizmos.color = Color.green;
                Gizmos.matrix = this.transform.localToWorldMatrix;
                Gizmos.DrawWireCube(new Vector3(Vector3.zero.x, Vector3.zero.y + 1f, Vector3.zero.z), new Vector3(2f, 2f, 2f));
            }

            else if (visual == Visual.sphere)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(transform.position, 0.1f);
            }




        }
    }

}
