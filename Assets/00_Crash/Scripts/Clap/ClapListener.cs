using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ObliqueSenastions.ClapSpace
{
    public class ClapListener : MonoBehaviour
    {
        


        [SerializeField] bool listenToClapAction = false;
        [SerializeField] UnityEventFloat doOnClapAction;

        [SerializeField] bool listenToOnClapCountThresholdBroke = false;

        [SerializeField] UnityEventFloat doOnClapCountThresholdBroke;

        [SerializeField] bool listenToOnClapCountChanged = false;
        [SerializeField] UnityEventInt onClapCountChanged;

        [SerializeField] TextMeshProUGUI clapCountUi = null;

        

        ClapHandler clapHandler = null;

        ClapCounter clapCounter = null;

        private void OnEnable() 
        {
            clapHandler = GameObject.FindWithTag("Traveller").GetComponent<ClapHandler>();

            clapCounter = GameObject.FindWithTag("Traveller").GetComponent<ClapCounter>();
        }

        void Start()
        {
            clapHandler.doOnColliderClap += OnColliderClap;

            clapCounter.doOnCountThresholdBrokeDelegate += OnClapCountThresholdBroke;

            clapCounter.onClapCountChanged += OnClapCountChanged;
        }


        void Update()
        {

        }

        private void OnColliderClap(float strength)
        {
            print("ClapListener: OnClapAction");
            if(listenToClapAction)
            {
                doOnClapAction.Invoke(strength);
            }
        }

        private void OnClapCountThresholdBroke(float strength)
        {
            if(listenToOnClapCountThresholdBroke)
            {
                doOnClapCountThresholdBroke.Invoke(strength);
            }

        }

        private void OnClapCountChanged(int nr)
        {
            if(listenToOnClapCountChanged)
            {
                onClapCountChanged.Invoke(nr);
            }
            
        }

        public void SetUIText(int nr)
        {
            if (clapCountUi == null) return;
            clapCountUi.text = nr.ToString();
        }

        public void ResetCountedClaps()
        {
            GameObject.FindWithTag("Traveller").GetComponent<IResetClapCount>().ResetClapCount();
        }


    }

}

