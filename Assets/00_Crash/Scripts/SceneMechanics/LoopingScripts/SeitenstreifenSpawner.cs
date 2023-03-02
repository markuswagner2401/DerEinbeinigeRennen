using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeitenstreifenSpawner : MonoBehaviour
{
    [SerializeField] Transform[] placeholders;
    [SerializeField] GameObject[] prefabs;

    [SerializeField] bool random;
    [SerializeField] bool iterate;
    [SerializeField] bool serial;

    IndexedPrefab[] indexedPrefabs;

    struct IndexedPrefab
    {
        public int index;
        public GameObject prefab;
        public Transform placeholder;
    }





    void Start()
    {
        CreateIndexedPrefabs();
        SpawnPrefabs();
    }

    
    void Update()
    {
        
    }

    private void CreateIndexedPrefabs()
    {
        indexedPrefabs = new IndexedPrefab[placeholders.Length];

        if (random)
        {
            for (int i = 0; i < indexedPrefabs.Length; i++)
            {
                indexedPrefabs[i].index = i;
                indexedPrefabs[i].prefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
                indexedPrefabs[i].placeholder = placeholders[i];
            }
        }

        if(iterate)
        {
            int iteratingIndex = 0;
            for (int i = 0; i < indexedPrefabs.Length; i++)
            {
                indexedPrefabs[i].index = i;
                indexedPrefabs[i].prefab = prefabs[iteratingIndex];
                indexedPrefabs[i].placeholder = placeholders[i];
                
                if (iteratingIndex + 1 > prefabs.Length - 1)
                {
                    iteratingIndex = 0;
                }

                else
                {
                    iteratingIndex += 1;
                }
                
            }
        }
    }

    private void SpawnPrefabs()
    {
        for (int i = 0; i < indexedPrefabs.Length; i++)
        {
            Instantiate(indexedPrefabs[i].prefab, indexedPrefabs[i].placeholder.position, indexedPrefabs[i].placeholder.rotation);
            indexedPrefabs[i].placeholder.gameObject.SetActive(false);
        }
    }

    
}
