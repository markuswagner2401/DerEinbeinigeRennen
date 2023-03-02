using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] GameObject[] cameraObjects;

    int currentIndex = 0;
    
    


   
    void Start()
    {
        if(cameraObjects.Length == 0) return;

        cameraObjects[0].SetActive(true);

        for (int i = 1; i < cameraObjects.Length; i++)
        {
            cameraObjects[i].SetActive(false);
        }
    }

    
    void Update()
    {
        
    }

    public void NextCamera()
    {
        currentIndex += 1;
        if (currentIndex > cameraObjects.Length -1)
        {
            currentIndex = 0;
        }

        ActivateCamera(currentIndex);
        DeactiveCameras(currentIndex);
    }

    public void PreviousCamera()
    {
        currentIndex -= 1;
        if(currentIndex < 0)
        {
            currentIndex = cameraObjects.Length - 1;
        }

        ActivateCamera(currentIndex);
        DeactiveCameras(currentIndex);

    }

    void ActivateCamera(int index)
    {
        for (int i = 0; i < cameraObjects.Length; i++)
        {
            if(i != index) continue;
            cameraObjects[i].SetActive(true);
        }
    }

    void DeactiveCameras(int indexToSkip)
    {
        for (int i = 0; i < cameraObjects.Length; i++)
        {
            if(i == indexToSkip) continue;
            cameraObjects[i].SetActive(false);
        }
    }
}
