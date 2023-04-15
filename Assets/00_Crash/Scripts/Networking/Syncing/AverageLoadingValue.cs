using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.ClapSpace;
using ObliqueSenastions.VRRigSpace;
using ObliqueSenastions.PunNetworking;
using Photon.Pun;

namespace ObliqueSenastions.UISpace
{
    public class AverageLoadingValue : MonoBehaviourPun
    {
        [SerializeField] List<SyncLoadingBar> contibutingLoadingBars;

        [SerializeField] Role[] excludeRoles;

        [SerializeField] LoadingBar targetBar = null;

        int currentActiveBar;

        float currentOutput;

        

        void Start()
        {

        }


        void Update()
        {
            if(!PhotonNetwork.InRoom) return;

            // foreach (var item in excludeRoles)
            // {
            //     if(MultiplayerConnector.instance.GetRole() == item)
            //     {
            //         return;
            //     }
            // }

            if(contibutingLoadingBars.Count <= 0) return;

            if(contibutingLoadingBars[currentActiveBar].GetStreamedBarValue() < 0.01)
            {
                int newBar = PickRandomActiveBar();
                currentActiveBar = newBar;
            }

            currentOutput = contibutingLoadingBars[currentActiveBar].GetStreamedBarValue();

            //print("averageLoadingValue");

            
        	targetBar.SetLoadingBarValueExternally(currentOutput);

        }

        

        public void AddContributingLoadingBar(SyncLoadingBar newContributer)
        {
            contibutingLoadingBars.Add(newContributer);
        }

        public void RemoveContributinLoadingBar(SyncLoadingBar bar)
        {
            contibutingLoadingBars.Remove(bar);
        }

        float CalculateAverageValue()
        {
            float sum = 0;
            foreach (var item in contibutingLoadingBars)
            {
                sum += item.GetStreamedBarValue();
            }
            return sum / contibutingLoadingBars.Count;
        }

        int PickFirstActiveBar()
        {
            for (int i = 0; i < contibutingLoadingBars.Count; i++)
            {
                if(contibutingLoadingBars[i].GetStreamedBarValue() > 0.01f)
                {
                    return i;
                }
            }

            return 0;
        }

        int PickRandomActiveBar()
        {
            int randomNr = Random.Range(0, contibutingLoadingBars.Count);
            if(contibutingLoadingBars[randomNr].GetStreamedBarValue() > 0.01f)
            {
                return randomNr;
            }
            else
            {
                
                return 0;
            }
        }

        
    }

}


