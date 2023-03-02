using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;
    [SerializeField] int[] matIndices;
    [SerializeField] Material[] materials;

    [SerializeField] int currentIndex = 0;
    

   
    void Start()
    {
        
    }

    public void SetMaterial (Material newMaterial)
    {

        foreach (var _object in gameObjects)
        {
            
            MeshRenderer mesh = _object.GetComponent<MeshRenderer>();
            Material[] matArray = mesh.materials;

            foreach (var index in matIndices)
            {
                matArray[index] = newMaterial;
            }

            mesh.materials = matArray;
        }
    }

    public void SwitchMaterial()
    {
        

        if(currentIndex + 1 > materials.Length - 1)
        {
            currentIndex = 0;
        }

        else
        {
            currentIndex += 1;
        }

        SetMaterial(materials[currentIndex]);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
