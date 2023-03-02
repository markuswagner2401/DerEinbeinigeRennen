using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxDisplayer : MonoBehaviour
{
    [SerializeField] Mesh mesh;
    [SerializeField] SkinnedMeshRenderer smr;

    Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        //mesh = GetComponent<MeshFilter>().mesh;
        bounds = smr.sharedMesh.bounds;
    }

    private void OnDrawGizmos() {
        //Gizmos.DrawCube(bounds.center, bounds.size);

        //Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

    void Update()
    {
        print(bounds.size);
    }
}
