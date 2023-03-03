using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ObliqueSenastions.PunNetworking
{

    public class SyncPuppetOwnershipManagement : MonoBehaviour
    {
        [SerializeField] PhotonView photonView = null;

        private void OnCollisionEnter(Collision other)
        {

            if (other.gameObject.tag == "Tachonadel")
            {
                photonView.RequestOwnership();

            }
        }
    }

}
