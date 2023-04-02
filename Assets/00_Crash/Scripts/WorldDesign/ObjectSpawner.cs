using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TransformControl
{


    public class ObjectSpawner : MonoBehaviour
    {
        public enum SpawnMode
        {
            Cycle,
            Random
        }

        public Transform[] targetTransforms;
        public GameObject[] objectsToSpawn;
        public SpawnMode spawnMode = SpawnMode.Cycle;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
        [HideInInspector]
        public GameObject[] spawnedObjects;
    }
}

