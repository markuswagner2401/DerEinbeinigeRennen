using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using System;
using ObliqueSenastions.SceneSpace;

namespace ObliqueSenastions.PunNetworking
{
    public class ObstacleSpawner : MonoBehaviourPunCallbacks
    {

        [System.Serializable]
        public struct SpawnSituation
        {
            public string name;
            public float minDuration;
            public float maxDuration;
            public GameObject[] prefabs;
            public Transform[] spawnPoints;
            public bool spawnOne;
        }

        [System.Serializable]
        public struct DissolveSituation
        {
            public string name;
            public int targetSpawnSituationIndex;
            public float dissolveFrequencyMin;
            public float dissolveFrequencyMax;
        }

        public SpawnSituation[] spawnSituations;
        public DissolveSituation[] dissolveSituations;

        private Dictionary<int, Coroutine> activeSpawnRoutines;

        private Dictionary<Transform, GameObject> occupiedSpawnPoints;

        private void Start()
        {
            if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
            {
                enabled = false;
                return;
            }
            activeSpawnRoutines = new Dictionary<int, Coroutine>();
            occupiedSpawnPoints = new Dictionary<Transform, GameObject>();

            // debugging

            //PlaySpawnSituation(0);
        }

        private void UpdateSpawnerActivity()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                enabled = true;
            }
            else
            {
                enabled = false;
            }
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            UpdateSpawnerActivity();
        }

        public override void OnJoinedRoom()
        {
            UpdateSpawnerActivity();
        }

        public void PlaySpawnSituation(string name)
        {
            int index = Array.FindIndex(spawnSituations, situation => situation.name == name);
            if (index != -1)
            {
                PlaySpawnSituation(index);
            }
        }

        public void PlaySpawnSituation(int index)
        {
            print("play spawn situation");
            if (!activeSpawnRoutines.ContainsKey(index))
            {
                Coroutine routine = StartCoroutine(PlaySpawnSituationRoutine(index));
                activeSpawnRoutines.Add(index, routine);
            }
        }

        private IEnumerator PlaySpawnSituationRoutine(int index)
        {
            SpawnSituation situation = spawnSituations[index];
            int[] spawnCounts = new int[situation.spawnPoints.Length];

            while (true)
            {
                int newIndex = UnityEngine.Random.Range(0, situation.spawnPoints.Length);

                // Increment spawn count for this spawn point
                if (situation.spawnOne)
                {
                    spawnCounts[newIndex]++;
                    if (spawnCounts.All(count => count >= 1))
                    {
                        print("Obstacles spawn on all points");
                        break;
                    }
                }

                Transform spawnPoint = situation.spawnPoints[newIndex];

                //print("try point: " + spawnPoint + " index " + newIndex);

                // Check if the spawn point is occupied
                if (occupiedSpawnPoints.ContainsKey(spawnPoint) && occupiedSpawnPoints[spawnPoint] != null)
                {
                    yield return null;
                    continue;
                }

                GameObject prefab = situation.prefabs[UnityEngine.Random.Range(0, situation.prefabs.Length)];

                GameObject spawnedObstacle;

                if (PhotonNetwork.IsConnected)
                {
                    spawnedObstacle = PhotonNetwork.Instantiate(prefab.name, spawnPoint.position, spawnPoint.rotation);
                }
                else
                {
                    spawnedObstacle = ObstaclePool.Instance.SpawnFromPool(prefab.name, spawnPoint.position, spawnPoint.rotation);
                }



                occupiedSpawnPoints[spawnPoint] = spawnedObstacle;

                if (spawnedObstacle != null)
                {
                    ObstacleAnimator obstacleAnimator = spawnedObstacle.GetComponent<ObstacleAnimator>();
                    obstacleAnimator.OnObstacleDestroyed += () =>
                {
                    occupiedSpawnPoints[spawnPoint] = null;
                };
                }








                float waitTime = UnityEngine.Random.Range(situation.minDuration, situation.maxDuration);
                yield return new WaitForSeconds(waitTime);
            }
        }

        public void StopSpawnSituation(string name)
        {
            int index = Array.FindIndex(spawnSituations, situation => situation.name == name);
            if (index != -1)
            {
                StopSpawnSituation(index);
            }
        }

        public void StopSpawnSituation(int index)
        {
            if (activeSpawnRoutines.ContainsKey(index))
            {
                StopCoroutine(activeSpawnRoutines[index]);
                activeSpawnRoutines.Remove(index);
            }
        }

        public void StopAllSpawnSituations()
        {
            foreach (var routine in activeSpawnRoutines.Values)
            {
                StopCoroutine(routine);
            }
            activeSpawnRoutines.Clear();
        }

        public void PlayDissolveSituation(string name)
        {
            int index = Array.FindIndex(dissolveSituations, situation => situation.name == name);
            if (index != -1)
            {
                StartCoroutine(PlayDissolveSituationRoutine(index));
            }
        }

        private IEnumerator PlayDissolveSituationRoutine(int index)
        {
            DissolveSituation situation = dissolveSituations[index];
            SpawnSituation targetSituation = spawnSituations[situation.targetSpawnSituationIndex];

            while (true)
            {
                GameObject[] targetObstacles = GameObject.FindGameObjectsWithTag("Obstacle")
                    .Where(go => targetSituation.prefabs.Contains(go))
                    .ToArray();

                if (targetObstacles.Length > 0)
                {
                    GameObject targetObstacle = targetObstacles[UnityEngine.Random.Range(0, targetObstacles.Length)];
                    ObstacleAnimator obstacleAnimator = targetObstacle.GetComponent<ObstacleAnimator>();
                    obstacleAnimator.DissolveObstacle();
                }

                float waitTime = UnityEngine.Random.Range(situation.dissolveFrequencyMin, situation.dissolveFrequencyMax);
                yield return new WaitForSeconds(waitTime);
            }
        }

        public void StopDissolveSituation(string name)
        {
            int index = Array.FindIndex(dissolveSituations, situation => situation.name == name);
            if (index != -1)
            {
                StopCoroutine(PlayDissolveSituationRoutine(index));
            }
        }









    }

}


