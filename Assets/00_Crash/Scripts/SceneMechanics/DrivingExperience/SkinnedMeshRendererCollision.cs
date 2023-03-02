using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshRendererCollision : MonoBehaviour
{
    SkinnedMeshRenderer meshRenderer;
    MeshCollider meshCollider;

    
    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        UpdateMeshCollider();
    }

    
    void Update()
    {
        
    }

    void UpdateMeshCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = colliderMesh;
    }
}
