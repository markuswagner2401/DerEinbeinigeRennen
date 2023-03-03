using UnityEngine;
using Photon.Pun;

namespace ObliqueSenastions.PunNetworking
{
    public class MySyncTransform : MonoBehaviourPun, IPunObservable
    {
        enum Smoothing
        {
            lerp,

            dynamicLerp,
            moveToward,

            none
        }

        [SerializeField] Smoothing positionSmoothing;

        [SerializeField] float lerpPosition = 0.5f;

        [SerializeField] Smoothing rotationSmoothing;
        [SerializeField] float lerpRotation = 0.5f;
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private double lastUpdateTime;

        Vector3[] positions = new Vector3[2];
        Quaternion[] rotations = new Quaternion[2];
        float currentDistance;

        float currentAngle;

        [SerializeField]
        private bool useWorldTransform = false;

        [Tooltip("Manual Update when Network Player Mapping is ready")]
        [SerializeField] bool manualUpdate = false;

        [SerializeField] NetworkPlayer networkPlayer = null;



        [Tooltip("Units (Meter) per Second")]
        [SerializeField] float moveTowardSpeed = 5f;

        [Tooltip("Radiant per second (ca 3.14 für 180)")]
        [SerializeField] float rotateTowardSpeed = 20f;

        float serializationTimeframe;

        float timer;

        private void Start()
        {
            serializationTimeframe = (float)(1f / PhotonNetwork.SerializationRate);

            if (manualUpdate)
            {
                if (networkPlayer == null)
                {
                    networkPlayer = GetComponentInParent<NetworkPlayer>();
                }
                networkPlayer.onPlayerMappingUpdated += ManualUpdate;
            }




        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // This is the local player, send the current transform data
                Vector3 position = useWorldTransform ? transform.position : transform.localPosition;
                Quaternion rotation = useWorldTransform ? transform.rotation : transform.localRotation;

                stream.SendNext(position);
                stream.SendNext(rotation);
            }
            else
            {
                // This is a remote player, receive the transform data
                targetPosition = (Vector3)stream.ReceiveNext();
                targetRotation = (Quaternion)stream.ReceiveNext();
                lastUpdateTime = info.SentServerTime;
                positions[0] = transform.position;
                positions[1] = targetPosition;
                rotations[0] = transform.rotation;
                rotations[1] = targetRotation;
                currentDistance = Vector3.Distance(transform.position, targetPosition);
                currentAngle = Quaternion.Angle(transform.rotation, targetRotation);
                timer = 0;

            }
        }




        private void LateUpdate()
        {
            if (manualUpdate) return;
            ManualUpdate();

        }

        void ManualUpdate()
        {

            if (!photonView.IsMine)
            {
                Vector3 position = new Vector3();
                Quaternion rotation = new Quaternion();

                float elapsedTime = (float)(PhotonNetwork.Time - lastUpdateTime);

                timer += Time.deltaTime;


                // position
                if (positionSmoothing == Smoothing.lerp)
                {
                    // Interpolate the position and rotation of the remote player  
                    //position = Vector3.Lerp(transform.position, targetPosition, elapsedTime * lerpPosition);
                    position = Vector3.Lerp(transform.position, targetPosition, lerpPosition);

                }

                else if (positionSmoothing == Smoothing.moveToward)
                {

                    //position = Vector3.MoveTowards(transform.position, targetPosition, currentDistance * ( 1f / PhotonNetwork.SerializationRate ) );
                    if (useWorldTransform)
                    {
                        position = Vector3.MoveTowards(transform.position, targetPosition, currentDistance * Time.deltaTime * moveTowardSpeed);
                    }
                    else
                    {
                        position = Vector3.MoveTowards(transform.localPosition, targetPosition, currentDistance * Time.deltaTime * moveTowardSpeed);
                    }

                }

                else if (positionSmoothing == Smoothing.dynamicLerp)
                {

                    position = Vector3.Lerp(positions[0], targetPosition, timer / serializationTimeframe);
                    // if((timer / serializationTimeframe) > 1f || (timer / serializationTimeframe) < 0f)
                    // {
                    //     print("dynamic lerp alert: " + timer / serializationTimeframe);
                    // }
                }

                else
                {
                    position = targetPosition;
                }

                // rotation

                if (rotationSmoothing == Smoothing.lerp)
                {
                    // Interpolate the position and rotation of the remote player
                    //rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime * lerpRotation);
                    rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpRotation);
                }

                else if (positionSmoothing == Smoothing.moveToward)
                {
                    if (useWorldTransform)
                    {
                        rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentAngle * Time.deltaTime * rotateTowardSpeed);
                    }

                    else
                    {
                        rotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, currentAngle * Time.deltaTime * rotateTowardSpeed);
                    }
                    //rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentAngle * (1f / PhotonNetwork.SerializationRate ) );

                }

                else
                {
                    rotation = targetRotation;
                }


                if (useWorldTransform)
                {
                    transform.position = position;
                    transform.rotation = rotation;
                }
                else
                {
                    transform.localPosition = position;
                    transform.localRotation = rotation;
                }
            }
        }
    }

}
