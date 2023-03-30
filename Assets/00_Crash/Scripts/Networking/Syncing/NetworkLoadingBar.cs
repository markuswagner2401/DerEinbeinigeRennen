using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;
using Photon.Pun;


namespace ObliqueSenastions.PunNetworking
{
    public class NetworkLoadingBar : MonoBehaviourPun
    {
        ExposeLoadingBar exposeLoadingBar = null;

        [SerializeField] Role contributingRole;

        SyncLoadingBar syncLoadingBar = null;

        float loadingBarValue;

        bool setupSource = false;
        bool setupTarget = false;

        void Start()
        {
            SetupSource();
            SetupTarget();

        }

        private void SetupTarget()
        {
            if (MultiplayerConnector.instance.GetRole() == contributingRole)
            {
                GameObject averageLoadingValueGo = GameObject.FindWithTag("WeightTacho");
                if (averageLoadingValueGo == null) return;
                AverageLoadingValue averageLoadingValue = averageLoadingValueGo.GetComponent<AverageLoadingValue>();
                if (averageLoadingValue == null) return;

                syncLoadingBar = GetComponent<SyncLoadingBar>();
                if (syncLoadingBar == null) return;

                averageLoadingValue.AddContributingLoadingBar(syncLoadingBar);

                setupTarget = true;
            }
        }

        private void SetupSource()
        {
            if (!this.photonView.IsMine) return;

            GameObject traveller = GameObject.FindWithTag("Traveller");
            if (traveller == null) return;
            exposeLoadingBar = traveller.GetComponent<ExposeLoadingBar>();
            if (exposeLoadingBar == null) return;



            setupSource = true;

        }




        void Update()
        {
            if (!setupSource || !setupTarget || !photonView.IsMine) return;
            
            loadingBarValue = exposeLoadingBar.GetValue();
            SetSourceValue(loadingBarValue);
        }

        public void SetSourceValue(float value)
        {
            syncLoadingBar.SetValue(value);
        }
    }

}


