using UnityEngine;
using Photon.Pun;
using System;

public class SyncTransformRPC : MonoBehaviourPun
{
    enum Smoothing
    {
        lerp,

        dynamicLerp,
        moveToward,

        none
    }

    [SerializeField] Smoothing positionSmoothing;


    [SerializeField]
    private bool useWorldTransform = false;

    [Tooltip("Manual Update when Network Player Mapping is ready")]
    [SerializeField] bool manualUpdate = false;

    [SerializeField] NetworkPlayer networkPlayer = null;
    Vector3[] positions = new Vector3[2]; // [0] for last position [1] for current target position
    Quaternion[] rotations = new Quaternion[2];

    float currentDistance;

    float currentAngle;

    float serializationTimeframe;

    float timer;


    private void Start()
    {
        if(manualUpdate)
        {
            if(networkPlayer == null)
            {
                networkPlayer = GetComponentInParent<NetworkPlayer>();
            }
            networkPlayer.onPlayerMappingUpdated += ManualUpdate;
        }

    }

    private void ManualUpdate()
    {
        throw new NotImplementedException();
    }

    public void SendTransform(Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsConnected) return;
        photonView.RPC("UpdateTransform", RpcTarget.Others, position, rotation);
    }

    [PunRPC]
    private void UpdateTransform(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine)
        {
            return;
        }

        //Debug.Log("receive SendTransform");

        positions[0] = positions[1];
        positions[1] = position;
        rotations[0] = rotations[1];
        rotations[1] = rotation;
    }

    private void Update()
    {

    }


}

