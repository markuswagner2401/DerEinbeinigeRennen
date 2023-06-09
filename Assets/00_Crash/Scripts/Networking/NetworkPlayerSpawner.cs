using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;
using ObliqueSenastions.VRRigSpace;

namespace ObliqueSenastions.PunNetworking
{


    public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
    {
        enum SpawnSituation
        {
            initial,
            onSceneStart
        }

        SpawnSituation spawnSituation;


        [SerializeField] ContinuePoints continuePoints = ContinuePoints.addLast;
        enum ContinuePoints
        {
            cycle,
            addLast
        }

        [System.Serializable]
        public struct Avatar
        {
            public GameObject avatarPrefab;

            [Tooltip("-1 for Binding at xrRig, else, this index will be used to bind the Avatar to the VR Rig at the transitionPointIndex in the traveller")]
            public int transitionPointIndex;





            public bool spawnOnSceneStart;


            // public bool destroyOnSceneEnd; -> Set in Prefab

        }

        [System.Serializable]
        public struct AvatarSpawner
        {
            public string note;



            public Avatar[] avatars;

            //public GameObject playerPrefab;
        }

        public AvatarSpawner[] avatarSpawnersRennfahrer;

        public AvatarSpawner[] avatarSpawnersZuschauer;

        public AvatarSpawner[] avatarSpawnersInspizient;

        [SerializeField] bool debugSpawn = false;

        [SerializeField] int debugRacers = 1;

        [SerializeField] int debugZuschauer = 15;

        [SerializeField] int debugInspizienten = 1;



        //GameObject[] PlayerPrefabsInRoom;
        private GameObject spawnedPlayerPrefab;





        private void Start()
        {

            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            {
                StartCoroutine(OnSceneStartLate());
            }

            MultiplayerConnector.instance.my_OnJoinedRoom += MyOnJoinedRoom;
        }

        void MyOnJoinedRoom() // gets called by MultiplayerConnector after player inventur
        {
            SpawnPrefab(SpawnSituation.initial);
            Debug.Log("NetworkPlayerSpawner: SpawnPrefab On MyOnJoinedRoom");
        }

        public override void OnJoinedRoom()
        {

        }

        public override void OnDisable()
        {
            base.OnDisable();
            MultiplayerConnector.instance.my_OnJoinedRoom -= MyOnJoinedRoom;
        }

        private void OnDestroy()
        {
            
        }







        IEnumerator OnSceneStartLate() // it is necessary to wait some seconds, so that all network players are in the same scene
        {
            yield return new WaitForSeconds(3f);
            SpawnPrefab(SpawnSituation.onSceneStart);

            yield break;
        }




        private void SpawnPrefab(SpawnSituation spawnSituation)
        {

            StartCoroutine(SpawnPrefabRoutine(spawnSituation));
        }

        IEnumerator SpawnPrefabRoutine(SpawnSituation spawnSituation)
        {
            print("spawn routine: " + spawnSituation);
            Role role = MultiplayerConnector.instance.GetRole();
            int index = MultiplayerConnector.instance.GetClientsIndexInRole();

            /// TODO: Wait until timeline match is ready. and Scene is loaded





            /// Get Traveller

            GameObject travellerGo = GameObject.FindGameObjectWithTag("Traveller");

            while (transform == null)
            {
                Debug.Log("trying to get traveller rig");
                travellerGo = GameObject.FindGameObjectWithTag("Traveller");
                yield return null;
            }

            Debug.Log("traveller go found");

            cameraTraveller camTrav = travellerGo.GetComponent<cameraTraveller>();

            while (camTrav == null)
            {
                Debug.Log("trying to get cam trav");
                camTrav = travellerGo.GetComponent<cameraTraveller>();
                yield return null;
            }

            Debug.Log("cameraTraveller found: " + camTrav.gameObject.name);

            camTrav.SetRoleIdentifier(role, index);

            camTrav.OffsetMultiplayerLocations(index);

            /// Wait for Visibities Manager (because of colliders)

            while (!GetComponent<NetworkVisibilityManager>().GetVisibiliesReady())
            {
                yield return null;
            }

            /// Spawn

            if (debugSpawn)
            {
                for (int i = 0; i < debugRacers; i++)
                {
                    yield return SetupXRRigAndSpawn(i, camTrav, avatarSpawnersRennfahrer, spawnSituation);
                    yield return null;
                }

                for (int j = 0; j < debugZuschauer; j++)
                {
                    yield return SetupXRRigAndSpawn(j, camTrav, avatarSpawnersZuschauer, spawnSituation);
                    yield return null;
                }

                for (int k = 0; k < debugInspizienten; k++)
                {
                    SetupXRRigAndSpawn(k, camTrav, avatarSpawnersInspizient, spawnSituation);
                }

                yield break;
            }



            if (role == Role.Rennfahrer)
            {

                yield return SetupXRRigAndSpawn(index, camTrav, avatarSpawnersRennfahrer, spawnSituation);

            }
            else if (role == Role.Zuschauer)
            {

                yield return SetupXRRigAndSpawn(index, camTrav, avatarSpawnersZuschauer, spawnSituation);

            }

            else if (role == Role.Inspizient)
            {
                yield return SetupXRRigAndSpawn(index, camTrav, avatarSpawnersInspizient, spawnSituation);
            }



            yield break;
        }

        IEnumerator SetupXRRigAndSpawn(int index, cameraTraveller camTrav, AvatarSpawner[] avatarSpawnerOfRole, SpawnSituation spawnSituation)
        {

            /// setup XR Rig

            int spawnerIndex;

            if (continuePoints == ContinuePoints.cycle)
            {
                spawnerIndex = index % avatarSpawnerOfRole.Length - 1;
            }

            else
            {
                spawnerIndex = avatarSpawnerOfRole.Length - 1;
            }


            if (avatarSpawnerOfRole[spawnerIndex].avatars[0].transitionPointIndex >= 0)
            {
                camTrav.SetTransitionPoint(avatarSpawnerOfRole[spawnerIndex].avatars[0].transitionPointIndex);

                float timer2 = 0f;
                while (timer2 < camTrav.GetTransitionTime(avatarSpawnerOfRole[spawnerIndex].avatars[0].transitionPointIndex))
                {
                    timer2 += Time.deltaTime;
                    print("spawner: Wait for transition: " + camTrav.GetCurrentTransitionPoint());
                    yield return null;

                }
                //yield return new WaitForSeconds(camTrav.GetTransitionTime(avatarSpawnersZuschauer[index].avatars[0].transitionPointIndex));

            }

            /// Spawn


            for (int i = 0; i < avatarSpawnerOfRole[spawnerIndex].avatars.Length; i++)
            {
                Avatar avatar = avatarSpawnerOfRole[spawnerIndex].avatars[i];

                if (spawnSituation == SpawnSituation.initial)
                {
                    SpawnAvatar(avatar, camTrav);
                }

                else if (spawnSituation == SpawnSituation.onSceneStart)
                {
                    if (avatar.spawnOnSceneStart)
                    {
                        SpawnAvatar(avatar, camTrav);
                    }
                }





                yield return null;

            }
            yield break;
        }


        private void SpawnAvatar(Avatar avatar, cameraTraveller camTrav)
        {
            GameObject avatarPrefab = avatar.avatarPrefab;

            if (avatarPrefab == null)
            {
                Debug.LogError("The player prefab is not assigned. Please make sure to assign a prefab in the inspector.");
                return;
            }

            Transform currentTravellerLocation = null;

            if (avatar.transitionPointIndex < 0)
            {
                currentTravellerLocation = camTrav.GetCurrentXRRigTransform();
            }

            else
            {
                currentTravellerLocation = camTrav.GetXRRigTransform(avatar.transitionPointIndex);
            }

            if (currentTravellerLocation == null)
            {
                Debug.LogError("No Transition Point could be assigned.", this);
                return;
            }

            Vector3 newPosition = currentTravellerLocation.transform.position;
            Quaternion newRotation = currentTravellerLocation.transform.rotation;

            Debug.Log("new position + new rotation set");

            if (PhotonNetwork.IsConnected)
            {
                spawnedPlayerPrefab = PhotonNetwork.Instantiate(avatarPrefab.name, newPosition, newRotation);
            }

            else
            {
                spawnedPlayerPrefab = Instantiate(avatarPrefab, newPosition, newRotation);
            }



            //spawnedPlayerPrefab = PhotonNetwork.Instantiate(avatarPrefab.name, newPosition, newRotation);

            spawnedPlayerPrefab.name = MultiplayerConnector.instance.GetRole().ToString();

            spawnedPlayerPrefab.GetComponent<NetworkPlayer>().SetTransitionPointIndex(avatar.transitionPointIndex);



            Debug.Log("spawnedPlayerPrefab: " + spawnedPlayerPrefab.name);
        }

        public override void OnLeftRoom()
        {
            //base.OnLeftRoom();
            if (spawnedPlayerPrefab != null)
            {
                PhotonNetwork.Destroy(spawnedPlayerPrefab);
                print("NetworkPlayerSpawner: A Player Left Room");
            }
        }



    }


}
