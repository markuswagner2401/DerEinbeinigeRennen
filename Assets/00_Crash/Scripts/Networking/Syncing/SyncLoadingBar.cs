using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.ClapSpace;
using UnityEngine;
using Photon.Pun;
using ObliqueSenastions.UISpace;

namespace ObliqueSenastions.PunNetworking
{
    public class SyncLoadingBar : MonoBehaviourPunCallbacks, IPunObservable
    {
        // SyncLoadingBar -> NetworkLoadingBar -> ExposeLoadingBar (On traveller) -> LoadingBar.GetHaudenlukasValue
        // SyncLoadingBar -> AverageLoadinValue: AddContributingLoadingBar (TODO: would be better with interface because like this it is circular)
   
        [SerializeField] float sourceValue;

        private void Start() 
        {
            
        }

        

        public void SetValue(float value)
        {
            if(!photonView.IsMine) return;
            
            sourceValue = value;
        }

        

        public float GetStreamedBarValue()
        {
            return sourceValue;
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(!this.enabled) return;

            if(stream.IsWriting)
            {
                
                stream.SendNext(sourceValue);
            }

            else
            {
                sourceValue = (float)stream.ReceiveNext();
                
            }
        }
    }

}

