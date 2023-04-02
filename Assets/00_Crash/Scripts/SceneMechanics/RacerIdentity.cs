using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.MaterialControl;
using Photon.Pun;

namespace ObliqueSenastions.PunNetworking
{
    public class RacerIdentity : MonoBehaviour
    {
        [SerializeField] Identity[] identities;
        [System.Serializable]
        public struct Identity
        {
            public string note;

        }

        [SerializeField] MaterialPropertiesFader_2 materialPropertiesFader_2 = null;

        [Tooltip("To Check if Owned By Local Player")]
        [SerializeField] PhotonView photonView = null;





        public int identityIndex = -1;

        public bool isMine = false;

        private void Start()
        {
            StartCoroutine(LateStartRoutine());
        }

        IEnumerator LateStartRoutine()
        {
            yield return new WaitForSeconds(3f);

            // Get Index
            int index = MultiplayerConnector.instance.GetClientsIndexInRole();
            if (index >= identities.Length)
            {
                Debug.LogError("RacerIdentity: No identity set up with Clietns index: " + index);
                yield break;
            }
            identityIndex = index;
            DoOnHaveNumber(identityIndex);

            // Mine Check
            if (photonView != null)
            {
                isMine = photonView.IsMine;
            }

            DoOnIsMine(isMine);


            yield break;
        }

        void DoOnHaveNumber(int number)
        {
            //print("do on have number");
            materialPropertiesFader_2.ChangeColor(number);
        }

        void DoOnIsMine(bool isMine)
        {
            if (isMine)
            {
                materialPropertiesFader_2.SetMaterial(0);
                materialPropertiesFader_2.SetMaterial(1);
            }

            else
            {
                materialPropertiesFader_2.SetMaterial(2);
                materialPropertiesFader_2.SetMaterial(3);
            }

        }


    }

}

