using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.PunNetworking
{
    public class NetworkPlayerVelocityListener : MonoBehaviour
    {
        
        [SerializeField] Role contributingRole = Role.Zuschauer;

        public bool RoleMatch(Role askingRole)
        {
            if(contributingRole == Role.None) return false;
            return (askingRole == contributingRole);
        }

    }

}


