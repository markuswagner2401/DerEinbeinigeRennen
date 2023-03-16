using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.ClapSpace
{
    public class ClapDetector : MonoBehaviour
    {
        public delegate void OnClap();
        public OnClap clap;

        private void Start()
        {
            clap += DoOnClap;
        }




        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "ClapDetector")
            {

                clap.Invoke();

            }
        }



        void DoOnClap()
        {

        }







    }


}

