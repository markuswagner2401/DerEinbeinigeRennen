using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ObliqueSenastions.ClapSpace
{


    public class LoadingBar : MonoBehaviour
    {
        [SerializeField] string valueReference = "";
        [SerializeField] Image uiImage = null;
        [SerializeField] Material material;

        
        [SerializeField] LoadingBarMaterialBlock loadingBarMaterialBlock;
        [SerializeField] float clapFrameTime;

        [SerializeField] UnityEvent onLoadingComlete;

        [SerializeField] bool enableClapAccumulation = false;
        [SerializeField] bool listenforClapAccumulation = false;

        [SerializeField] float accumulateFactor = 0.5f;
        [SerializeField] float deAccumulationPerFrame = 0.001f;
        [SerializeField] float accumulativeBarValue;

        [SerializeField] HauDenLukasThreshold[] hauDenLukasThresholds;

        [System.Serializable]
        public struct HauDenLukasThreshold
        {
            public string note;
            public float threshold;

            public bool thresholdBroke;

            public UnityEvent onThresholdBroke;
            public UnityEvent onthresholdReturn;
        }

        bool accumulateCompleteTriggered = false;



        bool interrupted;

        [SerializeField] bool getSetExternally = false;

        


        void Start()
        {
            if (uiImage == null)
            {
                uiImage = GetComponent<Image>();
            }
            material = uiImage.material;
        }

        private void FixedUpdate()
        {
            if(getSetExternally) return;

            if (enableClapAccumulation)
            {
                if (accumulativeBarValue > 0f)
                {
                    accumulativeBarValue = Mathf.Clamp01(accumulativeBarValue - deAccumulationPerFrame);

                }

                SetLoadingBarValue(accumulativeBarValue);


                CheckBarThresholds();

            }



        }

        public void SetLoadingBarValueExternally(float value)
        {
            getSetExternally = true;
            SetLoadingBarValue(value);
        }

        public void SetClapFrameTime(float time)
        {
            clapFrameTime = time;
        }

        public void SetLoadingBarValue(float value)
        {
            //print("set loading bar value: " + value);
            if(loadingBarMaterialBlock != null)
            {
                loadingBarMaterialBlock.SetFloat(valueReference, value);
            }

            else
            {
                material.SetFloat(valueReference, value);
            }
            
            

        }

        public void InterruptLoading()
        {
            interrupted = true;
        }

        public void StartLoadingBar(float loadingTime)
        {
            gameObject.SetActive(true);
            if(!gameObject.activeInHierarchy) return;
            StartCoroutine(InterruptandStartLoadingBar(loadingTime));
        }

        IEnumerator InterruptandStartLoadingBar(float loadingTime)
        {
            interrupted = true;
            yield return new WaitForSecondsRealtime(0.1f);
            StartCoroutine(LoadingBarR(loadingTime));
            yield break;
        }

        IEnumerator LoadingBarR(float loadingTime)
        {
            interrupted = false;
            float timer = 0;
            while (timer <= loadingTime && !interrupted)
            {
                timer += Time.unscaledDeltaTime;

                float newValue = Mathf.Lerp(0, 1f, timer / loadingTime);

                material.SetFloat(valueReference, newValue);

                if (newValue > 0.999f)
                {
                    onLoadingComlete.Invoke();
                    SetLoadingBarValue(0f);
                    yield break;
                }

                yield return null;
            }




            yield break;
        }

        public void EndLoadingBar()
        {
            SetLoadingBarValue(0f);
            gameObject.SetActive(false);
        }

        // Accumulative Loading Bar

        public void ListenForClapAccumulation(bool value)
        {
            if (!value)
            {
                accumulativeBarValue = 0f;
            }

            enableClapAccumulation = value;
            listenforClapAccumulation = value;


        }

        public void SetAccumulativeBarValue(float value)
        {
            if (!listenforClapAccumulation) return;
            if (accumulativeBarValue > value) return;
            accumulativeBarValue = value;
        }

        public void AddToLoadingBar(float value)
        {
            if (!listenforClapAccumulation) return;

            accumulativeBarValue += accumulateFactor * value;

        }

        private void CheckBarThresholds()
        {
            for (int i = 0; i < hauDenLukasThresholds.Length; i++)
            {
                if (accumulativeBarValue > hauDenLukasThresholds[i].threshold)
                {
                    if (!hauDenLukasThresholds[i].thresholdBroke)
                    {
                        hauDenLukasThresholds[i].thresholdBroke = true;
                        hauDenLukasThresholds[i].onThresholdBroke.Invoke();

                    }

                }

                else
                {

                    if (hauDenLukasThresholds[i].thresholdBroke)
                    {
                        hauDenLukasThresholds[i].thresholdBroke = false;
                        hauDenLukasThresholds[i].onthresholdReturn.Invoke();

                    }
                }
            }





            if (accumulativeBarValue > 0.999f)
            {
                if (accumulateCompleteTriggered) return;
                accumulateCompleteTriggered = true;
                onLoadingComlete.Invoke();
            }

            else
            {
                accumulateCompleteTriggered = false;
            }
        }

        public float GetLoadingValue()
        {
            if(material == null) return 0;
            return material.GetFloat(valueReference);
            //return accumulativeBarValue;
        }

        public void SetDeAccululationPerFrame(float value)
        {
            deAccumulationPerFrame = value;
        }





    }



}


