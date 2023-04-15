using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarMaterialBlock : MonoBehaviour
{
    private Material material;
    private MaterialPropertyBlock propertyBlock;
    private Image image;

    private float test;

    private void Start()
    {
        image = GetComponent<Image>();
        material = Instantiate(image.material); // Create a new material instance to avoid modifying the shared material
        image.material = material;
    }

    public void SetFloat(string propertyName, float value)
    {
        material.SetFloat(propertyName, value);
    }

    public float GetFloat(string propertyName)
    {
        if(material == null) return 0;
        return material.GetFloat(propertyName);
    }
}
