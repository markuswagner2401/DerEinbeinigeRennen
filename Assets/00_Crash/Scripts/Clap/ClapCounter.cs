using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.ClapSpace
{
    interface IResetClapCount
    {
        void ResetClapCount();
    }
    public class ClapCounter : MonoBehaviour, IResetClapCount
    {
        [SerializeField] float timeBetweenClaps = 1f;

        [SerializeField] bool ResetAdEveluateAfterTimeBetweenClaps = true;

        [SerializeField] ClapCountAction[] clapCountActions;

        [System.Serializable]
        struct ClapCountAction
        {
            public string note;
            public int claps;

            [Tooltip("0 for figure, 1 for picture")]
            public int[] signalListenerIndexes;

            public float signalStrength;
            public UnityEvent DoOnXClaps;


            public string uiText;
            public int ClapFXPoolIndex;
        }

        [SerializeField] string resetText = "0";

        [SerializeField] bool resetClapsOnResetLoadingBar = true;

        [Tooltip("cycling off when negative value")]
        [SerializeField] int cycleClapsAfterXClaps = -1;

        [SerializeField] UnityEvent doOnClapsStart;

        public delegate void OnEmitSignal(float strength, int listenerIndex);

        public OnEmitSignal onEmitSignal = null;

        [SerializeField] int countThreshold = 100;

        [SerializeField] UnityEvent doOnCountThresholdBroke;

        public delegate void DoOnCountThresholdBrokeDelegate(float strength);

        public DoOnCountThresholdBrokeDelegate doOnCountThresholdBrokeDelegate;

        [SerializeField] UnityEventFloat doEveryClapOverThreshold;

        bool countThresholdBrokeEventTriggered = false;

        [SerializeField] int[] afterCountingSignalListenerIndices;






        [SerializeField] UnityEvent doOnClapsResetted;

        //[SerializeField] Text clapCounterUIText;
        [SerializeField] TextMeshProUGUI clapCounterUIText = null;
        [SerializeField] LoadingBar loadingBar = null;

        [SerializeField] LoadingBar haudDenLukasBar = null;
        [SerializeField] ClapCounterFX clapCounterFX = null;

        [SerializeField] bool enableLoadingBar = true;

        [SerializeField] bool enableHauDenLukasBar = false;
        [SerializeField] bool enableUIText = true;
        [SerializeField] bool enableClapCountFX = true;
        [SerializeField] bool enableClapCountActions = true;

        [SerializeField] float defaultStrength = 0.5f; // TODO pass value from clap handler
        float currentStrength;


        public delegate void OnClapCountChanged(int claps);
        public OnClapCountChanged onClapCountChanged;





        float timer;

        int claps = 0;

       
        bool isInterrupted;
        bool resetted = false;



        void Start()
        {
            if (!ReferenceEquals(loadingBar, null))
            {
                loadingBar.SetClapFrameTime(timeBetweenClaps);
            }

            onEmitSignal += PlaceholderDoonEmitSignal;

            doOnCountThresholdBrokeDelegate += PlaceholderDoonCountThresholdBrokeDelegate;

            onClapCountChanged += PlaceholderOnClapcountChanged;

            
        }

        private void OnDisable()
        {
            onEmitSignal -= PlaceholderDoonEmitSignal;

            doOnCountThresholdBrokeDelegate -= PlaceholderDoonCountThresholdBrokeDelegate;

            onClapCountChanged -= PlaceholderOnClapcountChanged;

        }



        void Update()
        {
            //ProcessResetting();

            
        }

        private void ProcessResetting()
        {
            timer += Time.unscaledDeltaTime;

            if (!ResetAdEveluateAfterTimeBetweenClaps) return;

            if (timer <= timeBetweenClaps)
            {
                resetted = false;
                return;
            }

            else
            {
                if (resetted) return;




                EvaluateAndReset(claps);



                SetUiText(resetText);
                EndLoadingBar();
                resetted = true;

                if (resetClapsOnResetLoadingBar)
                {
                    claps = 0;
                }


            }
        }

        private void EvaluateAndReset(int accumulatedClaps)
        {
            if (enableClapCountActions)
            {
                doOnClapsResetted.Invoke();
            }


            foreach (var action in clapCountActions)
            {
                if (accumulatedClaps == action.claps)
                {
                    if (enableClapCountActions)
                    {
                        action.DoOnXClaps.Invoke();

                        foreach (var listener in action.signalListenerIndexes)
                        {
                            onEmitSignal.Invoke(action.signalStrength, listener);
                        }

                    }

                    if (enableClapCountFX)
                    {
                        SpawnClapCountFX(action.ClapFXPoolIndex, action.signalStrength);
                    }

                }
            }

        }

        public void EnableResetAndEvaluate(bool value)
        {
            ResetAdEveluateAfterTimeBetweenClaps = value;
        }





        public void CountClap()
        {

            currentStrength = defaultStrength;
            CountClap(defaultStrength);



        }

        public void CountClap(float strength)
        {

            //print("Count clap " + strength);
            timer = 0f;

            claps += 1;

            if (cycleClapsAfterXClaps > 0)
            {
                
                
                if(claps > cycleClapsAfterXClaps)
                {
                    claps = 0;
                    print("cycle claps");
                }
                
            }

            onClapCountChanged.Invoke(claps);

            StartLoadingBar(timeBetweenClaps);

            foreach (var action in clapCountActions)
            {
                if (claps == action.claps)
                {
                    SetUiText(action.uiText);
                }
            }

            if (claps == 1)
            {
                if (enableClapCountActions)
                {
                    doOnClapsStart.Invoke();
                }
            }

            if (claps > countThreshold)
            {


                doEveryClapOverThreshold.Invoke(strength);

                if (!countThresholdBrokeEventTriggered)
                {
                    doOnCountThresholdBroke.Invoke();
                    countThresholdBrokeEventTriggered = true;
                }


                foreach (var listener in afterCountingSignalListenerIndices)
                {
                    onEmitSignal.Invoke(strength, listener);
                }


            }

            else
            {
                countThresholdBrokeEventTriggered = false;
            }
        }



        /// enabling

        public void EnableAllClapCountUI(bool value)
        {
            EnableLoadingBar(value);
            EnableUIText(value);

        }

        public void EnableLoadingBar(bool value)
        {
            if (value == false)
            {
                EndLoadingBar();
            }

            enableLoadingBar = value;

        }

        public void EnableHauDenLukasBar(bool value)
        {
            if (value == false)
            {
                EndHauDenLukasBar();
            }

            enableHauDenLukasBar = value;

            if (value == true)
            {
                StartHauDenLukasBar();
            }


        }



        public void EnableUIText(bool value)
        {
            SetUiText("");
            enableUIText = value;

        }

        public void EnableClapCountActions(bool value)
        {
            enableClapCountActions = value;
        }

        public void EnableClapCountFX(bool value)
        {
            enableClapCountFX = value;
        }

        public void SetCountThreshold(int value)
        {
            countThreshold = value;
        }

        public void ResetCountedClaps()
        {
            claps = 0;
        }







        ///

        private void SetUiText(string text)
        {
            if (!enableUIText) return;

            if (ReferenceEquals(clapCounterUIText, null)) return;
            clapCounterUIText.text = text;
        }

        private void StartLoadingBar(float loadingTime)
        {
            if (!enableLoadingBar) return;

            if (!ReferenceEquals(loadingBar, null))
            {
                loadingBar.gameObject.SetActive(true);
                loadingBar.StartLoadingBar(loadingTime);
            }
        }

        private void EndLoadingBar()
        {
            if (!enableLoadingBar) return;

            if (!ReferenceEquals(loadingBar, null))
            {
                loadingBar.SetLoadingBarValue(0f);
                loadingBar.gameObject.SetActive(false);
            }
        }

        private void StartHauDenLukasBar()
        {
            if (!enableHauDenLukasBar) return;
            if (!ReferenceEquals(haudDenLukasBar, null))
            {
                haudDenLukasBar.gameObject.SetActive(true);
                haudDenLukasBar.ListenForClapAccumulation(true);
            }
        }

        private void EndHauDenLukasBar()
        {
            if (!enableHauDenLukasBar) return;

            if (!ReferenceEquals(haudDenLukasBar, null))
            {
                haudDenLukasBar.SetLoadingBarValue(0f);
                haudDenLukasBar.ListenForClapAccumulation(false);
                haudDenLukasBar.gameObject.SetActive(false);
            }
        }

        private void SpawnClapCountFX(int index, float strength)
        {
            if (!enableClapCountFX) return;

            if (ReferenceEquals(clapCounterFX, null)) return;

            clapCounterFX.SpawnFX(index, strength);

        }

        ////
        private void PlaceholderDoonEmitSignal(float strength, int listenerIndex)
        {
        }

        private void PlaceholderDoonCountThresholdBrokeDelegate(float strength)
        {
        }

        private void PlaceholderOnClapcountChanged(int claps)
        {
        }

        //IResetClapCount

        public void ResetClapCount()
        {
            claps = 0;
            onClapCountChanged.Invoke(claps);

        }
    }


}


