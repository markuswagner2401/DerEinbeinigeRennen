using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

namespace ObliqueSenastions.PunNetworking
{
    public class EnterRoleHandler : MonoBehaviourPun
    {
        [SerializeField] Role matchingRole;

        [SerializeField] UnityEvent doOnRoleMatch;

        

        [SerializeField] UnityEvent doOnRoleMismatch;
        
        bool roleSetUp = false;

        // Update is called once per frame
        void Update()
        {
            if (!PhotonNetwork.IsConnected) 
            {
                roleSetUp = false;
                return;
            }
            
            if(roleSetUp) return;



            if(MultiplayerConnector.instance.GetRole() == matchingRole)
            {
                doOnRoleMatch.Invoke();
                roleSetUp = true;
            }

            else
            {
                doOnRoleMismatch.Invoke();
                roleSetUp = true;
            }

            

        }
    }

}


