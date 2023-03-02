using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxMaterialChanger : MonoBehaviour
{
    [SerializeField] Material[] skyboxMaterials;
    int currentIndex = 0;

    public void ChangeSkyboxMat(int index) 
    {
        if(index > skyboxMaterials.Length - 1) return;
        RenderSettings.skybox = skyboxMaterials[index];
    }

    public void SetNextSkyboxMaterial()
    {
        SetNextIndex();
        RenderSettings.skybox = skyboxMaterials[currentIndex];

    }

    void SetNextIndex()
    {
        if(currentIndex + 1 > skyboxMaterials.Length - 1)
        {
            currentIndex = 0;
        }

        else
        {
            currentIndex += 1;
        }
    }
}
