using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace ObliqueSenastions.SceneSpace
{

    public class ObstaclePool : MonoBehaviourPunCallbacks
    {
        public static ObstaclePool Instance;

        [System.Serializable]
        public class Pool
        {
            public string note;
            public GameObject prefab;
            public int size;
        }

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary = null;

        public Transform obstaclePoolParent;

        [SerializeField] bool waitForNetwork = true;



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
            if(!waitForNetwork)
            {
                CreatePool();
            }
            
        }

        public override void OnJoinedRoom()
        {
            if(waitForNetwork)
            {
                CreatePool();
            }
        }

        

        private void CreatePool()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                GameObject obj;

                for (int i = 0; i < pool.size; i++)
                {


                    if (PhotonNetwork.IsConnected)
                    {
                        obj = PhotonNetwork.Instantiate(pool.prefab.name, Vector3.zero, Quaternion.identity);
                    }
                    else
                    {
                        obj = Instantiate(pool.prefab);
                    }

                    obj.SetActive(false);
                    if (obstaclePoolParent != null)
                    {
                        obj.transform.SetParent(obstaclePoolParent);
                    }

                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.prefab.name, objectPool);
            }
        }

        public GameObject SpawnFromPool(string prefabName, Vector3 position, Quaternion rotation)
        {
            if(poolDictionary == null) return null;
            if (!poolDictionary.ContainsKey(prefabName))
            {
                Debug.LogWarning("Pool with key " + prefabName + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[prefabName].Dequeue();

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

            poolDictionary[prefabName].Enqueue(objectToSpawn);

            return objectToSpawn;
        }
    }

}


