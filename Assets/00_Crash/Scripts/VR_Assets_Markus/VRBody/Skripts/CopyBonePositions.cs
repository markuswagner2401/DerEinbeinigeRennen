using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyBonePositions : MonoBehaviour
{
    [SerializeField] Transform[] sourceRig = null;
    [SerializeField] Transform[] targetRig = null;

    Dictionary<Transform, Transform> bonesDictionary = new Dictionary<Transform, Transform>();

    
    void Start()
    {
        CreateBonesDictionary();
    }

    private void CreateBonesDictionary()
    {
        if (sourceRig == null || targetRig == null) return;

        foreach (Transform sourceBone in sourceRig)
        {
            foreach (Transform targetBone in targetRig)
            {
                if (sourceBone.name != targetBone.name) continue;

                bonesDictionary.Add(sourceBone, targetBone);

                
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bonesDictionary.Count == 0) return;

        foreach (var bonesPair in bonesDictionary)
        {
            bonesPair.Value.position = bonesPair.Key.position;
            bonesPair.Value.rotation = bonesPair.Key.rotation;
            // bonesPair.Value.localScale = bonesPair.Key.localScale;
            
        }
    }

    private void LateUpdate()
    {

    }
}