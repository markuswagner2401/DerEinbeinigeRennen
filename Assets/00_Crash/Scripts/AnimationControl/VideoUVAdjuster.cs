using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoUVAdjuster : MonoBehaviour
{
    
    [SerializeField] string[] screenUVFactorRefs;
    [SerializeField] SkinnedMeshRenderer smr;
    Material material;

    void Start()
    {
        material = smr.materials[0];
    }

    public void AdjustUVx(float factor, int screenIndex)
    {
        material.SetFloat(screenUVFactorRefs[screenIndex], factor);
        
    }

    public float GetUVxAdjust(int screenIndex)
    {
        return material.GetFloat(screenUVFactorRefs[screenIndex]);
    }
}
