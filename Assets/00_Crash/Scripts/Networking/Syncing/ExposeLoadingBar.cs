using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.ClapSpace;
using UnityEngine;

namespace ObliqueSenastions.PunNetworking
{
    public class ExposeLoadingBar : MonoBehaviour
    {
        [SerializeField] LoadingBar sourceBar = null;
        [SerializeField] float exposedValue;

        private void Update()
        {
            exposedValue = sourceBar.GetLoadingValue();
        }

        public float GetValue()
        {
            return exposedValue;
        }





    }

}


