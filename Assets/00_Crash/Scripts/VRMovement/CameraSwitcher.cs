using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.PunNetworking;
using Photon.Pun;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] Camera trackedCamera = null;
    [SerializeField] Camera fixCamera = null;

    [SerializeField] bool useFixCamera = false;

    bool lastUseFixCamera;

    [SerializeField] bool enableFixCameraUse = true;
    [SerializeField] Role fixCameraRole = Role.None;

    void Start()
    {
        if(PhotonNetwork.IsConnected && MultiplayerConnector.instance.GetRole() == fixCameraRole)
        {
            useFixCamera = true;
        }

        else
        {
            useFixCamera = false;
        }

        SetFixCamera(useFixCamera);

        lastUseFixCamera = useFixCamera;
    }

    // Update is called once per frame
    void Update()
    {
        SetFixCamera(useFixCamera);
        // if(lastUseFixCamera != useFixCamera)
        // {
            
        // }
        // lastUseFixCamera = useFixCamera;
    }

    private void SetFixCamera(bool useFixCamera)
    {
        if (trackedCamera == null || fixCamera == null) return;

        if (useFixCamera)
        {
            fixCamera.enabled = true;
            trackedCamera.enabled = false;
        }
        else
        {
            fixCamera.enabled = false;
            trackedCamera.enabled = true;
        }
    }

    void UseFixCamera(bool value)
    {
        useFixCamera = value;
    }
}
