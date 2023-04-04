using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace ObliqueSenastions.SceneSpace
{

    public class ObstaclePool : MonoBehaviour
    {
        public static ObstaclePool Instance;

        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary = null;

        public Transform obstaclePoolParent;



        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Another instance of ObstaclePool already exists!");
            }
        }

        void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    if(obstaclePoolParent != null)
                    {
                        obj.transform.SetParent(obstaclePoolParent); 
                    }
                    
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            if (PhotonNetwork.IsConnected)
            {
                PhotonView photonView = objectToSpawn.GetComponent<PhotonView>();
                if (photonView != null)
                {
                    photonView.RPC("ActivateObject", RpcTarget.Others, position, rotation);
                }
                else
                {
                    Debug.LogWarning("No PhotonView found on the object to spawn from pool. Object may not be synchronized correctly.");
                }
            }

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }
    }

}


