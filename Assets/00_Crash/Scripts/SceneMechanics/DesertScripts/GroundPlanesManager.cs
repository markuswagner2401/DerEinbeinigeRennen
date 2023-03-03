using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ObliqueSenastions.InfiniteGround
{


    public class GroundPlanesManager : MonoBehaviour
    {
        [SerializeField] GroundContainer[] groundContainers;

        [System.Serializable]
        private struct GroundContainer
        {
            public int index;
            public Transform transform;
            public GameObject[] groundPrefabs;
            public string[] positionsToProtect;
            public string centerOfProtect;
            public Dictionary<string, Transform> containerLookup;



        }

        Dictionary<string, Transform> allPlates = new Dictionary<string, Transform>();


        bool stoppGroundSpawning = false;



        [SerializeField] float groundSize = 100f;



        // public NavMeshSurface navMeshSurface;






        private void Awake()
        {


            for (int i = 0; i < groundContainers.Length; i++)
            {
                groundContainers[i].positionsToProtect = new string[9];
            }

            for (int i = 0; i < groundContainers.Length; i++)
            {
                groundContainers[i].containerLookup = new Dictionary<string, Transform>(0);
            }





        }


        private void Start()
        {

        }






        public void HandleGroundRequest(GameObject centerGameObject, int containerIndex)
        {
            if (stoppGroundSpawning) return;

            Vector3 centerPosition = centerGameObject.transform.position;

            if (groundContainers[containerIndex].centerOfProtect != null && centerPosition.ToString() == groundContainers[containerIndex].centerOfProtect) return;


            Vector3[] positionsAroundCenter = CalculatePositionsAroundCenter(centerPosition);

            Dictionary<string, Vector3> dictionaryPosAroundCenter = DictPosAroundCenter(positionsAroundCenter);




            // if (SafeCheck(dictionaryPosAroundCenter, containerIndex)) return;



            UpdatePositionsToProtect(dictionaryPosAroundCenter, containerIndex);

            groundContainers[containerIndex].centerOfProtect = centerPosition.ToString();



            Dictionary<string, Vector3> platesToCreate = PlatesToCreate(dictionaryPosAroundCenter, containerIndex);

            Dictionary<string, Transform> platesToDestroy = PlatesToDestroy(dictionaryPosAroundCenter, groundContainers[containerIndex].containerLookup);



            CreatePlates(platesToCreate, containerIndex);

            DestroyPlates(platesToDestroy, containerIndex);


            allPlates = DictAllPlates(groundContainers);



            // UpdateNavMeshSurface();

        }


        public void DestoyOnCrash(List<string> positionsAroundCrash)
        {
            stoppGroundSpawning = true;

            print("destroy on crash");


            Dictionary<string, Transform> crashPlates = new Dictionary<string, Transform>();

            for (int containerIndex = 0; containerIndex < groundContainers.Length; containerIndex++)
            {
                foreach (var lookupItem in groundContainers[containerIndex].containerLookup)
                {
                    foreach (var position in positionsAroundCrash)
                    {
                        if (crashPlates.ContainsKey(position)) continue;

                        if (lookupItem.Key == position)
                        {
                            crashPlates.Add(lookupItem.Key, lookupItem.Value);
                        }
                    }
                }

            }



            foreach (var plate in crashPlates)
            {

                Destroy(plate.Value.gameObject);
            }


            for (int i = 0; i < groundContainers.Length; i++)
            {
                foreach (var plate in crashPlates)
                {
                    if (groundContainers[i].containerLookup.ContainsKey(plate.Key))
                    {
                        groundContainers[i].containerLookup.Remove(plate.Key);
                    }
                }
            }

            stoppGroundSpawning = false;


        }





        private GroundContainer FindGroundContainer(int index)
        {
            foreach (var container in groundContainers)
            {
                if (container.index == index)
                {
                    return container;
                }
            }

            print("no groundcontainer found with corresponding index");
            return groundContainers[0];

        }


        private Vector3[] CalculatePositionsAroundCenter(Vector3 centerPosition)
        {
            Vector3[] positionsAroundCenter = new Vector3[9];

            positionsAroundCenter[0] = new Vector3(centerPosition.x - groundSize, centerPosition.y, centerPosition.z - groundSize);
            positionsAroundCenter[1] = new Vector3(centerPosition.x, centerPosition.y, centerPosition.z - groundSize);
            positionsAroundCenter[2] = new Vector3(centerPosition.x + groundSize, centerPosition.y, centerPosition.z - groundSize);

            positionsAroundCenter[3] = new Vector3(centerPosition.x - groundSize, centerPosition.y, centerPosition.z);
            positionsAroundCenter[4] = centerPosition;
            positionsAroundCenter[5] = new Vector3(centerPosition.x + groundSize, centerPosition.y, centerPosition.z);

            positionsAroundCenter[6] = new Vector3(centerPosition.x - groundSize, centerPosition.y, centerPosition.z + groundSize);
            positionsAroundCenter[7] = new Vector3(centerPosition.x, centerPosition.y, centerPosition.z + groundSize);
            positionsAroundCenter[8] = new Vector3(centerPosition.x + groundSize, centerPosition.y, centerPosition.z + groundSize);

            return positionsAroundCenter;
        }



        private Dictionary<string, Vector3> DictPosAroundCenter(Vector3[] positionsAroundCenter)
        {
            Dictionary<string, Vector3> dictPosAroundCenter = new Dictionary<string, Vector3>();
            foreach (Vector3 position in positionsAroundCenter)
            {
                dictPosAroundCenter.Add(position.ToString("F0"), position);

            }

            return dictPosAroundCenter;
        }


        // private bool SafeCheck(Dictionary<string, Vector3> dictionaryPosAroundCenter, int containerIndex)
        // {
        //     foreach (var item in groundContainers[containerIndex].positionsToProtect)
        //     {
        //         if(item == null) return false;
        //         if(!dictionaryPosAroundCenter.ContainsKey(item)) return false;
        //     }

        //     return true;
        // }


        private void UpdatePositionsToProtect(Dictionary<string, Vector3> positionsAroundCenter, int containerIndex)
        {
            int index = 0;

            foreach (var position in positionsAroundCenter)
            {
                groundContainers[containerIndex].positionsToProtect[index] = position.Key;
                index++;
            }



        }


        private Dictionary<string, Vector3> PlatesToCreate(Dictionary<string, Vector3> dictionaryPosAroundCenter, int index)
        {

            Dictionary<string, Vector3> newPositions = new Dictionary<string, Vector3>();

            foreach (var position in dictionaryPosAroundCenter)
            {

                if (allPlates.ContainsKey(position.Key)) continue;

                newPositions.Add(position.Key, position.Value);

            }

            return newPositions;

        }



        private Dictionary<string, Transform> PlatesToDestroy(Dictionary<string, Vector3> dictionaryPosAroundCenter, Dictionary<string, Transform> containerLookup)
        {
            Dictionary<string, Transform> platesToDestroy = new Dictionary<string, Transform>();


            foreach (var position in containerLookup)
            {
                platesToDestroy.Add(position.Key, position.Value);
            }

            foreach (var container in groundContainers)
            {
                foreach (string item in container.positionsToProtect)
                {
                    if (item == null) continue;
                    if (platesToDestroy.ContainsKey(item))
                    {
                        platesToDestroy.Remove(item);
                    }
                }
            }

            return platesToDestroy;
        }


        private Dictionary<string, Transform> DictAllPlates(GroundContainer[] groundcontainers)
        {
            Dictionary<string, Transform> allPlates = new Dictionary<string, Transform>();

            foreach (var container in groundContainers)
            {
                foreach (var item in container.containerLookup)
                {
                    if (!allPlates.ContainsKey(item.Key))
                    {
                        allPlates.Add(item.Key, item.Value);
                    }
                }
            }

            return allPlates;




        }



        private void CreatePlates(Dictionary<string, Vector3> platesToCreate, int containerIndex)
        {
            var container = FindGroundContainer(containerIndex);

            int randomPrefab = UnityEngine.Random.Range(0, container.groundPrefabs.Length);

            foreach (var item in platesToCreate)
            {
                var plate = Instantiate(container.groundPrefabs[randomPrefab], item.Value, Quaternion.identity);

                plate.transform.SetParent(container.transform);

                groundContainers[containerIndex].containerLookup.Add(item.Key, plate.transform);
            }
        }



        private void DestroyPlates(Dictionary<string, Transform> platesToDestroy, int containerIndex)
        {
            foreach (var item in platesToDestroy)
            {
                Destroy(item.Value.gameObject);
                groundContainers[containerIndex].containerLookup.Remove(item.Key);

            }
        }










        public int GetIndex(Transform container)
        {
            foreach (var groundContainer in groundContainers)
            {
                if (groundContainer.transform == container)
                {
                    return groundContainer.index;
                }
            }

            return 0;
        }















        // public void UpdateNavMeshSurface()
        // {
        //     navMeshSurface.BuildNavMesh();
        // }












    }

}
