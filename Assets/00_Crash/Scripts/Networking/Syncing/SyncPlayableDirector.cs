using System.Collections;
using ObliqueSenastions.TimelineSpace;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Playables;

namespace ObliqueSenastions.PunNetworking
{

    public class SyncPlayableDirector : MonoBehaviourPun, IPunObservable, IPunOwnershipCallbacks
    {

        [SerializeField] NetworkOwnership networkOwnership = NetworkOwnership.alwaysOnAction;

        NetworkOwnership capturedOwnership = NetworkOwnership.onlyIfCanGetOw;

        [System.Serializable]
        public enum NetworkOwnership
        {
            alwaysOnAction,
            onlyIfCanGetOw,
            never,


        }

        [SerializeField] TimeModeMachine timeModeMachine;





        [SerializeField] bool playerCanGetOwnership = true;
        bool capturedCanGetOwnership;


        [SerializeField] private PlayableDirector director;

        [SerializeField] bool initialSync = false;

        [SerializeField] bool turnOutAudioIfNotMine = true;

        [Tooltip("to turn out if view is not mine")]
        [SerializeField] AudioSource audioSource = null;

        float capturedVolume;

        float currentTime;

        float currentTimeOfOwner;
        int currentPlayModeOfOwner; // 0-Play, 1-Pause-, 2-Hold, 3-FastForward, 4-FastBackward

        float currentSpeed;
        float currentSpeedOfOwner;
        float lastTime;

        [SerializeField] float jumpThreshold = 1f;

        [SerializeField] float speedJumpThreshold = 0.1f;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
            PhotonNetwork.AddCallbackTarget(this);
            if(timeModeMachine == null) timeModeMachine = GetComponent<TimeModeMachine>();
        }

        private void Start()
        {
            if (turnOutAudioIfNotMine)
            {
                if (audioSource == null)
                {
                    audioSource = GetComponent<AudioSource>();
                }

                capturedVolume = audioSource.volume;
            }


        }

        private void Update()
        {

            Role clientsRole = MultiplayerConnector.instance.GetRole();
            int inspizientCounter = MultiplayerConnector.instance.GetInspizentenCount();

            if (clientsRole == Role.Inspizient)
            {
                playerCanGetOwnership = true;
            }

            else
            {
                if (inspizientCounter > 0)
                {
                    playerCanGetOwnership = false;
                }

            }

            currentTime = (float)director.time;
            currentSpeed = (float)director.playableGraph.GetRootPlayable(0).GetSpeed();

            if (PhotonNetwork.IsConnected && !photonView.IsMine)
            {
                if (Mathf.Abs(currentTime - currentTimeOfOwner) > jumpThreshold)
                {
                    print("matchTime: current time: " + currentTime + "currentTimeOfOwner: " + currentTimeOfOwner);
                    director.time = currentTimeOfOwner;
                }

                if (Mathf.Abs(currentSpeed - currentSpeedOfOwner) > speedJumpThreshold)
                {
                    print("matchSpeed");
                    director.playableGraph.GetRootPlayable(0).SetSpeed((double)currentSpeedOfOwner);

                }

                timeModeMachine.SetTimelinePlayMode(currentPlayModeOfOwner);

            }

            else if (!PhotonNetwork.IsConnected)
            {
                currentTimeOfOwner = (float)director.time;
            }



            if (turnOutAudioIfNotMine)
            {
                audioSource.volume = photonView.IsMine ? capturedVolume : 0f;
            }




        }



        private void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }



        public void FixOwnershipToLocalPlayer()
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            photonView.OwnershipTransfer = OwnershipOption.Fixed;
        }

        


        public void SetCanGetOwnership(bool value)
        {
            playerCanGetOwnership = value;
        }

        public bool RequestNetworkOwnership()
        {
            if (photonView.IsMine || !PhotonNetwork.IsConnected) return true;

            if(photonView.OwnershipTransfer == OwnershipOption.Fixed && PhotonNetwork.IsConnected) return false;

            if (networkOwnership == NetworkOwnership.alwaysOnAction)
            {
                // PhotonView photonView = GetComponent<PhotonView>();
                // if(photonView == null) return;
                // Debug.Log("Request Ownership");
                // photonView.RequestOwnership();
                base.photonView.RequestOwnership();

            }

            else if (networkOwnership == NetworkOwnership.never)
            {
                return false;
            }

            else if (networkOwnership == NetworkOwnership.onlyIfCanGetOw)
            {
                if (playerCanGetOwnership)
                {
                    base.photonView.RequestOwnership();
                }
                else
                {
                    return false;
                }
            }

            return photonView.IsMine;
        }

        public bool GetIsMine()
        {
            if(!PhotonNetwork.IsConnected) return true;
            return photonView.IsMine;
        }


        //Interface Ownership

        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            if (targetView != photonView) return;

            if (base.photonView.OwnershipTransfer == OwnershipOption.Fixed)
            {
                Debug.Log("SyncPlayableDirector: Ownership has been fixed before -> Ownershiprequest recected");
            }

            Debug.Log("TimelineScroller: TransferOwnership. new Owner: " + requestingPlayer);
            photonView.TransferOwnership(requestingPlayer);
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            if (targetView != photonView) return;
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
            Debug.Log("TimelineScroller: OnOwnershipTransferFailed");
        }



        /// Photon Stream

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            //PlayableDirector director = GetComponent<PlayableDirector>();
            if (stream.IsWriting)
            {
                double speed = director.playableGraph.GetRootPlayable(0).GetSpeed();
                double time = director.time;
                //Debug.Log("send speed: " + speed);
                stream.SendNext((float)speed);
                stream.SendNext((float)time);
                stream.SendNext((int)timeModeMachine.GetTimelinePlayMode());
            }
            else
            {
                float speed = (float)stream.ReceiveNext();
                float time = (float)stream.ReceiveNext();
                int playMode = (int)stream.ReceiveNext();
                //Debug.Log("receive speed: " + speed);
                // director.playableGraph.GetRootPlayable(0).SetSpeed((double)speed);
                currentSpeedOfOwner = speed;
                currentTimeOfOwner = time;
                currentPlayModeOfOwner = playMode;

            }
        }



        //// RPC Methods




        private void SyncTimelineJump(float newTime)
        {
            if (PhotonNetwork.IsConnected && photonView.IsMine)
            {
                photonView.RPC("SyncTimeline", RpcTarget.Others, newTime);
            }
        }

        [PunRPC]
        private void SyncTimeline(float newTime)
        {
            Debug.Log("SyncPlayableDirctor: OnTimelineJump");
            director.time = newTime;
        }


    }

}
