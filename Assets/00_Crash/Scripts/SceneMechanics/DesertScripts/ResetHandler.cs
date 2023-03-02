using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHandler : MonoBehaviour
{
    [SerializeField] Transform[] transformsToReset;
    
    PositionToReset[] positionsToReset;

    struct PositionToReset
    {
        public Transform transform;
        public Vector3 capturedPosition;
    }


    [SerializeField] ObjectToRespawn[] objectsToRespawn;

    [System.Serializable]
    struct ObjectToRespawn
    {
        public GameObject prefab;
        public Transform transform;
    }

    
    void Start()
    {
        CapturePositions();
    }

    

    private void CapturePositions()
    {
        positionsToReset = new PositionToReset[transformsToReset.Length];

        for (int i = 0; i < positionsToReset.Length; i++)
        {
            positionsToReset[i].transform = transformsToReset[i];
            positionsToReset[i].capturedPosition = transformsToReset[i].position;
        }
    }

    public void ResetPositionsAndObjects()
    {
        print("reset");
        ResetPositions();
        RespawnObjects();
    }
    

    public void ResetPositions()
    {
        foreach (var item in positionsToReset)
        {
            item.transform.position = item.capturedPosition;
        }
    }

    public void RespawnObjects()
    {
        foreach (var prefab in objectsToRespawn)
        {
            Instantiate(prefab.prefab, prefab.transform.position, prefab.transform.rotation);
        }
    }

    
}
