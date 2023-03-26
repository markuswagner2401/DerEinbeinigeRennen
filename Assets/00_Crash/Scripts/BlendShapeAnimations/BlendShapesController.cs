using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;
using ObliqueSenastions.VFXSpace;
using ObliqueSenastions.ClapSpace;
using UnityEngine.VFX;
using UnityEngine.Events;

namespace ObliqueSenastions.Animation
{

    public class BlendShapesController : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer skinnedMesh;
        [SerializeField] Mesh mesh;



        [SerializeField] float weightTachoInputNormaized = 0;

        [SerializeField] int currentIndex;

        [SerializeField] bool healing;

        [Tooltip("time necessary to stay on index before healing")]
        [SerializeField] float healingthreshold = 0.6f;

        float healingthresholdtimer;

        [SerializeField] float pauseOnHealing = 5f;

        [SerializeField] UnityEvent doOnHealing;

        [SerializeField] UnityEvent doOnNotHealing;

        float healingTimer = Mathf.Infinity;

        [Tooltip("match count with states of index tacho count")]
        [SerializeField] ShapeControl[] shapeControls;


        [System.Serializable]
        struct ShapeControl
        {
            // blend shapes

            public bool resetAllWeights;
            public int blendShapeIndex;
            public float minWeight;
            public float maxWeight;
            public float currentWeight;
            public float targetWeight;
            public float smoothing;

            // VFX Effects

            public bool vfxEffect;
            public VFXControl vFXControl;

            public bool vfxTriggered;


        }

        [SerializeField] Tachonadel indexTacho = null;
        [SerializeField] bool readIndextAtLoadingBar = false;
        [SerializeField] LoadingBar indexLoadingBar = null;


        [SerializeField] Tachonadel weightTacho = null;
        [SerializeField] bool readWeightAtLoadingBar = false;
        [SerializeField] LoadingBar weightLoadingBar = null;

        [SerializeField] bool setupTachosOnSpawn = false;



        bool allWeightsResetted = false;



        int previousIndex;

        bool indexOutOfRange = false;

        float threshold = 0.001f;

        bool tachosSet = false;


        void Start()
        {
            if (skinnedMesh == null)
            {
                skinnedMesh = GetComponent<SkinnedMeshRenderer>();
            }



        }


        void Update()
        {
            if (!tachosSet && setupTachosOnSpawn)
            {

                if (!SetupTachos())
                {
                    Debug.LogError("Tachos not set");
                    return;
                }
            }

            if (healing)
            {
                currentIndex = 8;
            }
            else
            {
                if (readIndextAtLoadingBar)
                {
                    currentIndex = Mathf.RoundToInt(Mathf.Lerp(0, shapeControls.Length - 1, indexLoadingBar.GetHauDenLukasValue()));
                }
                else
                {

                    currentIndex = Mathf.RoundToInt(Mathf.Lerp(0, shapeControls.Length - 1, indexTacho.GetNormedTargetPosition()));
                }

            }

            //SetupTachos();

            // Set current Index


            if (shapeControls[currentIndex].resetAllWeights)
            {
                healingthresholdtimer += Time.deltaTime;

                if (healingthresholdtimer > healingthreshold)
                {
                    if (!healing)
                    {
                        StartCoroutine(HealingRoutine());
                    }
                }
            }

            else
            {
                healingthresholdtimer = 0f;
            }


            MultiplayerMethod();

            return;


            // Check Reset Option at index

            if (shapeControls[currentIndex].resetAllWeights && !allWeightsResetted)
            {
                allWeightsResetted = true;
                ResetAllWeights();
                ResetAllVFX();
                return;
            }

            allWeightsResetted = false;

            // Revert Last Weight Value if Index changed

            // if (currentIndex != previousIndex) /// TODO: ???
            // {
            //     // blendShapesWeightTacho.SetTargetPositionNorm(Mathf.InverseLerp(shapeControls[currentIndex].minWeight,
            //     //                                                         shapeControls[currentIndex].maxWeight,
            //     //                                                         shapeControls[currentIndex].currentWeight));



            // }

            previousIndex = currentIndex;

            // Set Target Weight with Tacho-Value

            if (!shapeControls[currentIndex].vfxEffect)
            {
                shapeControls[currentIndex].targetWeight = Mathf.Lerp(shapeControls[currentIndex].minWeight,
                                                                              shapeControls[currentIndex].maxWeight,
                                                                              weightTacho.GetNormedTargetPosition());
            }

            else
            {
                shapeControls[currentIndex].targetWeight = Mathf.Lerp(shapeControls[currentIndex].vFXControl.vfxParameters[0].valueMin,
                                                                       shapeControls[currentIndex].vFXControl.vfxParameters[0].valueMax,
                                                                       weightTacho.GetNormedTargetPosition());
            }




            // Lerp current weight to target Weight

            for (int i = 0; i < shapeControls.Length; i++)
            {
                if (shapeControls[i].resetAllWeights)
                {
                    continue;
                }

                if (Mathf.Abs(shapeControls[i].currentWeight - shapeControls[i].targetWeight) > threshold)
                {
                    shapeControls[i].currentWeight = Mathf.Lerp(shapeControls[i].currentWeight, shapeControls[i].targetWeight, shapeControls[i].smoothing);

                    // apply current weight

                    if (shapeControls[currentIndex].blendShapeIndex < mesh.blendShapeCount)
                    {
                        skinnedMesh.SetBlendShapeWeight(shapeControls[i].blendShapeIndex, shapeControls[i].currentWeight);
                    }

                    if (shapeControls[currentIndex].vfxEffect && shapeControls[currentIndex].vFXControl.visualEffect != null)
                    {
                        ControlVFXEffect();
                    }

                }
            }

        }

        IEnumerator HealingRoutine()
        {
            healing = true;
            doOnHealing.Invoke();
            yield return new WaitForSeconds(pauseOnHealing);
            healing = false;
            doOnNotHealing.Invoke();
            yield break;

        }

        bool SetupTachos()
        {

            print("setup tachos");
            // setup index tacho

            bool indexTachoSet = false;

            GameObject indexTachoGo = GameObject.FindWithTag("IndexTacho");
            if (indexTachoGo == null)
            {
                Debug.LogError("index tacho not found");
                return false;
            }
            else
            {
                if (readIndextAtLoadingBar)
                {
                    if (indexTachoGo.TryGetComponent<LoadingBar>(out LoadingBar foundIndexLoadingBar))
                    {
                        print("foundloadingbar: " + foundIndexLoadingBar);
                        indexLoadingBar = foundIndexLoadingBar;
                        indexTachoSet = true;
                    }
                    else
                    {
                        indexTachoSet = false;
                    }
                }
                else
                {
                    if (indexTachoGo.TryGetComponent<Tachonadel>(out Tachonadel foundIndexTacho))
                    {
                        print("found index tacho: " + foundIndexTacho);
                        indexTacho = foundIndexTacho;
                        indexTachoSet = true;
                    }
                    else
                    {
                        indexTachoSet = false;
                    }

                }

                // setup weight tacho


                bool weightTachoSet;
                GameObject weightTachoGO = GameObject.FindWithTag("WeightTacho");
                if (weightTachoGO == null)
                {
                    print("Weight tacho not found");
                    return false;
                }
                else
                {
                    if (readWeightAtLoadingBar)
                    {
                        if (weightTachoGO.TryGetComponent<LoadingBar>(out LoadingBar foundWeightLoadingBar))
                        {
                            weightLoadingBar = foundWeightLoadingBar;
                            weightTachoSet = true;
                        }
                        else
                        {
                            weightTachoSet = false;
                        }
                    }

                    else
                    {
                        if (weightTachoGO.TryGetComponent<Tachonadel>(out Tachonadel foundWeightTacho))
                        {
                            weightTacho = foundWeightTacho;
                            weightTachoSet = true;
                        }
                        else
                        {
                            weightTachoSet = false;
                        }
                    }
                }
                tachosSet = (indexTachoSet && weightTachoSet);
                return tachosSet;


            }
        }



        private void MultiplayerMethod()
        {
            if (healing)
            {
                weightTachoInputNormaized = 0f;
            }

            else
            {
                if (readWeightAtLoadingBar)
                {
                    weightTachoInputNormaized = weightLoadingBar.GetHauDenLukasValue();
                }
                else
                {
                    weightTachoInputNormaized = weightTacho.GetNormedTargetPosition();
                }

            }








            for (int i = 0; i < shapeControls.Length; i++)
            {
                if (i == currentIndex)
                {
                    shapeControls[i].targetWeight = Mathf.Lerp(shapeControls[i].minWeight, shapeControls[i].maxWeight, weightTachoInputNormaized);
                }

                else
                {
                    //shapeControls[i].targetWeight = shapeControls[i].minWeight;
                    shapeControls[i].targetWeight = 0f;
                }
            }

            for (int i = 0; i < shapeControls.Length; i++)
            {
                // if (shapeControls[i].resetAllWeights)
                // {
                //     continue;
                // }

                if (Mathf.Abs(shapeControls[i].currentWeight - shapeControls[i].targetWeight) > threshold)
                {
                    shapeControls[i].currentWeight = Mathf.Lerp(shapeControls[i].currentWeight, shapeControls[i].targetWeight, shapeControls[i].smoothing);

                    // apply current weight

                    if (shapeControls[i].blendShapeIndex < mesh.blendShapeCount)
                    {
                        skinnedMesh.SetBlendShapeWeight(shapeControls[i].blendShapeIndex, shapeControls[i].currentWeight);
                    }

                    if (shapeControls[currentIndex].vfxEffect && shapeControls[currentIndex].vFXControl.visualEffect != null)
                    {
                        if (shapeControls[currentIndex].vfxTriggered) return;

                        PlayVFXEffect(currentIndex);

                        shapeControls[currentIndex].vfxTriggered = true;
                    }
                }
            }


        }

        public void PlayVFXEffect(int index)
        {
            print("play vfx effect");
            StartCoroutine(VFXRoutine(index));
        }

        IEnumerator VFXRoutine(int index)
        {
            float duration = shapeControls[index].vFXControl.duration;


            for (int i = 0; i < shapeControls[index].vFXControl.vfxParameters.Length; i++)
            {
                float timer = 0;
                VisualEffect vfx = shapeControls[index].vFXControl.visualEffect;
                //vfx.gameObject.SetActive(true);
                string parameterRef = shapeControls[index].vFXControl.vfxParameters[i].parameterName;
                float startValue = shapeControls[index].vFXControl.vfxParameters[i].valueMin;
                float targetValue = shapeControls[index].vFXControl.vfxParameters[i].valueMax;
                AnimationCurve curve = shapeControls[index].vFXControl.vfxParameters[i].curve;
                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    shapeControls[index].vFXControl.vfxParameters[i].currentValue =
                        Mathf.Lerp(startValue, targetValue,
                        curve.Evaluate(timer / duration));
                    vfx.SetFloat(parameterRef, shapeControls[index].vFXControl.vfxParameters[i].currentValue);
                    yield return null;
                }

                //vfx.gameObject.SetActive(false);
                print("vfx played");

                yield return null;

            }

            shapeControls[index].vfxTriggered = true;





            yield break;
        }



        private void ResetAllWeights()
        {
            for (int i = 0; i < shapeControls.Length; i++)
            {
                //shapeControls[i].targetWeight = shapeControls[i].minWeight;
                shapeControls[i].targetWeight = 0;
            }
        }

        private void ResetAllVFX()
        {
            for (int i = 0; i < shapeControls.Length; i++)
            {
                if (!shapeControls[i].vfxEffect || shapeControls[i].vFXControl.visualEffect == null) continue;
                shapeControls[i].vFXControl.visualEffect.gameObject.SetActive(false);
            }
        }

        private void ControlVFXEffect()
        {
            if (shapeControls[currentIndex].currentWeight > 0)
            {
                shapeControls[currentIndex].vFXControl.visualEffect.gameObject.SetActive(true);
            }

            else
            {
                shapeControls[currentIndex].vFXControl.visualEffect.gameObject.SetActive(false);
            }
        }



        // private void SetBlendShapeControlIndex(int index)
        // {
        //     if(blendShapeControls.Length <= index)
        //     {
        //         indexOutOfRange = true;
        //         currentControlIndex = 0;
        //     }

        //     else
        //     {
        //         currentControlIndex = index;
        //         indexOutOfRange = false;
        //     }

        // }


        // private void SetBSTargetWeightAtCurrentIndex( float targetWeightNorm01)
        // {
        //     float targetWeightMinMax = Mathf.Lerp(blendShapeControls[currentControlIndex].minWeight, blendShapeControls[currentControlIndex].maxWeight, targetWeightNorm01);
        //     blendShapeControls[currentControlIndex].targetWeight = targetWeightMinMax;
        // }

        // private IEnumerator LerpToTargetWeight(int controlIndex ,float targetWeight)
        // {
        //     float currentWeight = blendShapeControls[controlIndex].currentWeight;
        //     float startWeight = currentWeight;
        //     float fadeTime = blendShapeControls[controlIndex].fadeTime;
        //     float currentTime = 0;

        //     while(currentTime < fadeTime)
        //     {
        //         currentTime += Time.deltaTime;
        //         currentWeight = Mathf.Lerp(startWeight, targetWeight, currentTime/fadeTime);
        //         blendShapeControls[controlIndex].currentWeight = currentWeight;

        //     }

        //     mesh.SetBlendShapeWeight(blendShapeControls[controlIndex].blendShapeIndex, )
        //     yield return null;
        // }






    }


}
