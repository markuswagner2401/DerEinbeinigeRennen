using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectsSpawner : MonoBehaviour
{

    [SerializeField] GameObject persistentObjectsPrefab = null;

    static bool hasSpawned = false;

    private void Awake() 
    {
        if (hasSpawned) return;

        SpawnPersistentObjects();

        hasSpawned = true;
        
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObjects = Instantiate(persistentObjectsPrefab);
        DontDestroyOnLoad(persistentObjects);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
