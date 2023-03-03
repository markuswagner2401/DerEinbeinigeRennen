using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Animation
{
    public class ColliderUpdater : MonoBehaviour
    {
        MeshCollider coll;
        SkinnedMeshRenderer rend;
        Mesh mesh;

        void Start()
        {
            coll = GetComponent<MeshCollider>();
            rend = GetComponent<SkinnedMeshRenderer>();
            mesh = new Mesh();
            UpdateCollider();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateCollider()
        {
            //print("update collider");
            rend.BakeMesh(mesh);
            coll.sharedMesh = mesh;
        }
    }
}


