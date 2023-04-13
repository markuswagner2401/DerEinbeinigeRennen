using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using System;
using ExitGames.Client.Photon;
using System.Linq;
using ObliqueSenastions.SceneSpace;
using ObliqueSenastions.TimelineSpace;
using ObliqueSenastions.VRRigSpace;


public enum Role
{
    Rennfahrer,
    Zuschauer,
    Auto,
    None,
    Inspizient
}

namespace ObliqueSenastions.PunNetworking
{

    public class MultiplayerConnector : MonoBehaviourPunCallbacks
    {
        public static MultiplayerConnector instance = null;

        [SerializeField] int sendRate = 20;

        int lastSendingRate;

        [SerializeField] int serializationRate = 10;

        int lastSerializationRate;

        [SerializeField] Role clientsRole = Role.None;

        [SerializeField] int clientsPlayerIndex = 0;

        [SerializeField] GameMode clientsGameMode;



        [SerializeField] int maxRacers = 1;
        [SerializeField] int maxZuschauer = 20;
        [SerializeField] int maxAutos = 1;
        [SerializeField] int maxInspizienten = 1;

        [Tooltip("to handle racer and zuschauer indices in prolog and for debugging")]
        [SerializeField] int indexOffsetZuschauer = 0;

        [SerializeField] bool joinedInOfflineMode;




        public delegate void My_OnJoinedRoom();
        public My_OnJoinedRoom my_OnJoinedRoom;


        public delegate void OnConnectorMessage(string message);
        public OnConnectorMessage onConnectorMessage;

        public delegate void OnJoinedOffline();

        public OnJoinedOffline onJoinedOffline;

        // [SerializeField] MeetingRoom meetingRoom;

        // [System.Serializable]
        // public struct MeetingRoom
        // {
        //     [Tooltip("Leave Empty to uns Scene Name for Room Name")]
        //     public string name;
        //     public int maxPlayers;

        // }

        // [Tooltip("If the scene name starts with this indicator, a new room will be created/loaded on change Scene")]
        // [SerializeField] string changeRoomIndicator = "NewR";



        [SerializeField] string currentRoomSection = "";

        int playerCount = 0;

        int racerCounter = 0;
        int zuschauerCounter = 0;
        int autoCounter = 0;
        int inspizientCounter = 0;




        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            else if (instance != this)
            {
                Destroy(gameObject);
            }

            lastSendingRate = sendRate;

            lastSerializationRate = serializationRate;
        }


        public override void OnEnable()
        {
            //SceneManager.sceneLoaded += OnSceneLoaded;
            my_OnJoinedRoom += PlaceholderOnJoinedRoom;
            onConnectorMessage += PlaceholderOnConnectorMessage;
            base.OnEnable();
        }



        private void Start()
        {


            //SceneManager.activeSceneChanged += OnSceneChanged;
            // if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            // {
            //     my_OnJoinedRoom.Invoke();
            // }


            PhotonNetwork.SendRate = sendRate; // 20 (how mny packets a player sends per second)
            PhotonNetwork.SerializationRate = serializationRate; //10 (how often the data gets set per second, has to be lower that send rate


        }

        private void OnDestroy()
        {
            //SceneManager.sceneLoaded -= OnSceneLoaded;
            my_OnJoinedRoom -= PlaceholderOnJoinedRoom;
            onConnectorMessage -= PlaceholderOnConnectorMessage;
        }

        private void Update()
        {
            if (lastSendingRate != sendRate)
            {
                Debug.Log("change send rate to: " + sendRate);
                PhotonNetwork.SendRate = sendRate;
            }

            if (lastSerializationRate != serializationRate)
            {
                Debug.Log("Change Serialization rate to: " + serializationRate);
                PhotonNetwork.SerializationRate = serializationRate;
            }

            lastSendingRate = serializationRate;
            lastSendingRate = sendRate;
        }



        public override void OnDisable()
        {
            base.OnDisable();
            //SceneManager.sceneLoaded -= OnSceneLoaded;
            my_OnJoinedRoom -= PlaceholderOnJoinedRoom;

        }

        public void SetRole(Role role) // Gets set by connector UI (Connect Button Script) when connecting
        {
            clientsRole = role;
        }

        public Role GetRole()
        {
            return clientsRole;
        }

        public int GetClientsIndexInRole()
        {
            return clientsPlayerIndex;
        }

        public int GetNumberOfPlayersOfRole(Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    return racerCounter;


                case Role.Auto:
                    return autoCounter;


                case Role.Zuschauer:
                    return zuschauerCounter;

                case Role.Inspizient:
                    return inspizientCounter;

                case Role.None:
                    return 0;

                default:
                    break;
            }

            return 0;
        }

        public int GetInspizentenCount()
        {
            return inspizientCounter;
        }

        private void SetRoleIdentifier(Role role, int index)
        {
            clientsRole = role;
            if (role == Role.Zuschauer)
            {
                index += indexOffsetZuschauer;
            }
            clientsPlayerIndex = index;
        }

        public void SetGameMode(GameMode mode)
        {
            clientsGameMode = mode;
        }

        public GameMode GetClientsGameMode()
        {
            return clientsGameMode;
        }




        public void Connect(bool value)
        {
            if (value)
            {
                Connect();
            }
            else
            {
                Disconnect();
            }
        }

        private void Connect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                Debug.Log("MultiplayerConnector: ConnectUsingSettings");
            }

            Debug.Log("MultiplayerConnector. Connect -> Already Connected to Server");
        }

        

        private void Disconnect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                Debug.Log("MultiplayerConnector: PhotonNetwork.Disconnect");

            }
        }

        // public void SetNetworkPlayerAvatar(GameObject networkPlayer)
        // {
        //     networkAvatar = networkPlayer;
        // }

        // public GameObject GetNetworkPlayerAvatar()
        // {
        //     if(networkAvatar == null)
        //     {
        //         Debug.Log("MultiplayerConnector: No Network Avatar");
        //     }
        //     return networkAvatar;
        // }

        public int GetNetworkPlayerIndex()
        {
            return clientsPlayerIndex;
        }

        public void RequestTimelineOwnership()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                SyncPlayableDirector syncPlayableDirector = GetComponent<SyncPlayableDirector>();
                if (syncPlayableDirector != null)
                {
                    syncPlayableDirector.RequestNetworkOwnership();
                }
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("MultiplayerConnector: OnConnectedToMaster.");
            ChangeRoom();
        }



        public void ChangeRoom()
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();//Connect to Master
                clientsPlayerIndex = -1;
                Debug.Log("MultiplayerConnector: LeaveRoom");
            }
            if (PhotonNetwork.IsConnectedAndReady)
            {
                string roomName;
                if (TimeLineHandler.instance == null)
                {
                    roomName = "noTimelineRoom";
                }

                else
                {
                    roomName = TimeLineHandler.instance.GetComponent<SceneControlByTimeline>().GetRoomSectionThisClip();
                }

                currentRoomSection = roomName;
                RoomOptions roomOptions = new RoomOptions { MaxPlayers = 18 };
                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
                Debug.Log("MultiplayerConnector: JoinOrCreateRoom: " + roomName);
            }
        }







        public override void OnJoinedRoom()
        {
            Room room = PhotonNetwork.CurrentRoom;
            PhotonNetwork.LocalPlayer.NickName = clientsRole.ToString();
            Debug.Log("Player joined Room: " + room.Name + " . Playername: " + PhotonNetwork.LocalPlayer.NickName);



            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (clientsPlayerIndex < 0) // assign a new index if got disconnected
            {
                clientsPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            }




            Debug.Log("Number of players in the room: " + playerCount);
            Debug.Log("Local player index: " + clientsPlayerIndex);

            Player[] players = PhotonNetwork.PlayerList;

            PlayerInventur(players);


            int roleCount;

            if (clientsRole == Role.Auto)
            {
                roleCount = autoCounter;
            }

            else if (clientsRole == Role.Rennfahrer)
            {
                roleCount = racerCounter;
            }

            else if (clientsRole == Role.Zuschauer)
            {
                roleCount = zuschauerCounter;
            }

            else if (clientsRole == Role.Inspizient)
            {
                roleCount = inspizientCounter;
            }

            else
            {
                Debug.LogError("role couldn t be counted");
                roleCount = -1;
            }

            Debug.Log("Set Role identifier: Role: " + clientsRole + " index: " + (roleCount - 1));

            SetRoleIdentifier(clientsRole, roleCount - 1);

            onConnectorMessage.Invoke("Player joined Room: " + room.Name + " . Playername: " + PhotonNetwork.LocalPlayer.NickName + "\n" +
                                    "Role: " + clientsRole + " index: " + (roleCount - 1) + "\n" +
                                    "Total Players in Room: " + playerCount);


            my_OnJoinedRoom.Invoke();



        }

        //// Fallback

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            JoinInOfflineMode();
            
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            joinedInOfflineMode = true;
            JoinInOfflineMode();
        }

        public void JoinInOfflineMode()
        {
            print("join in offline mode");
            playerCount = 1;
            onConnectorMessage.Invoke("Connection failed, in Offline Mode.\n" +
                                    "Role: " + clientsRole + "\n" +
                                    "Total Players in Room: " + playerCount);

            joinedInOfflineMode = true;

            

            //my_OnJoinedRoom.Invoke();
            if(onJoinedOffline != null)
            {
                onJoinedOffline.Invoke();
            }
            
        }

        public bool GetJoinedInOfflineMode()
        {
            return joinedInOfflineMode;
        }

        /////

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Player[] players = PhotonNetwork.PlayerList;
            PlayerInventur(players);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Player[] players = PhotonNetwork.PlayerList;
            PlayerInventur(players);

        }

        void PlayerInventur(Player[] playersInRoom)
        {
            racerCounter = 0;
            zuschauerCounter = 0;
            autoCounter = 0;
            inspizientCounter = 0;


            foreach (var player in playersInRoom)
            {
                //if(player.IsLocal) continue;
                if (player.NickName == Role.Auto.ToString())
                {
                    autoCounter += 1;

                    if (autoCounter > maxAutos)
                    {
                        Debug.Log("NetworkPlayerSpawner: Too many autos in room: Disconnecting. Try to connect in another role");
                        onConnectorMessage.Invoke("ABGELEHNT | ROLLE BESETZT");
                        Connect(false);
                    }
                }

                else if (player.NickName == Role.Rennfahrer.ToString())
                {
                    racerCounter += 1;
                    if (racerCounter > maxRacers)
                    {
                        Debug.Log("NetworkPlayerSpawner: Too many racers in room: Disconnecting. Try to connect in another role");
                        onConnectorMessage.Invoke("ABGELEHNT | ROLLE BESETZT");
                        Connect(false);

                    }
                }
                else if (player.NickName == Role.Zuschauer.ToString())
                {
                    zuschauerCounter += 1;
                    if (zuschauerCounter > maxZuschauer)
                    {
                        Debug.Log("NetworkPlayerSpawner: Too many zuschauer in room: Disconnecting. Try to connect in another role");
                        onConnectorMessage.Invoke("ABGELEHNT | ROLLE BESETZT");
                        Connect(false);

                    }
                }
                else if (player.NickName == Role.Inspizient.ToString())
                {
                    inspizientCounter += 1;
                    if (inspizientCounter > maxInspizienten)
                    {
                        Debug.Log("NetworkPlayerSpawner: Too many Inspizienten in room: Disconnecting. Try to connect in another role");
                        onConnectorMessage.Invoke("ABGELEHNT | ROLLE BESETZT");
                        Connect(false);
                    }
                }


            }

            Debug.Log("NetworkPlayerSpawner: " + autoCounter + " autos in room");
            Debug.Log("NetworkPlayerSpawner: " + racerCounter + " racer in room");
            Debug.Log("NetworkPlayerSpawner: " + zuschauerCounter + " zuschauer in room");
            Debug.Log("NetworkPlayerSpawner: " + inspizientCounter + " inspizienten in room");

        }



        // todo: find solution for transfer looping to monocoque


        // private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        // {
        //     if (arg0.name == "TransferScene") return;



        //     string newRoomSection = TimeLineHandler.instance.GetComponent<SceneControlByTimeline>().GetRoomSectionThisClip();

        //     if (newRoomSection == currentRoomSection)
        //     {
        //         my_OnJoinedRoom.Invoke();
        //         return;
        //     }
        //     else
        //     {
        //         ChangeRoom();
        //     }


        // }



        private void PlaceholderOnJoinedRoom()
        {

        }

        private void PlaceholderOnConnectorMessage(string message)
        {
        }

        public override void OnLeftRoom()
        {
            Debug.Log("MultiplayerConnector: local Player left room");

        }







    }

}
