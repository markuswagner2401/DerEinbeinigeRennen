using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;
using ObliqueSenastions.VFXSpace;



namespace ObliqueSenastions.Animation
{

    public class BlendShapesController : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer skinnedMesh;
        [SerializeField] Mesh mesh;

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


        }

        [SerializeField] Tachonadel blendShapesIndexTacho = null;
        [SerializeField] Tachonadel blendShapesWeightTacho = null;

        bool tachosSet = false;

        bool allWeightsResetted = false;

        [SerializeField] int currentIndex;

        int previousIndex;

        bool indexOutOfRange = false;

        float threshold = 0.001f;


        void Start()
        {
            if (skinnedMesh == null)
            {
                skinnedMesh = GetComponent<SkinnedMeshRenderer>();
            }



        }


        void Update()
        {
            if (!tachosSet)
            {

                if (!ReferenceEquals(blendShapesIndexTacho, null) && !ReferenceEquals(blendShapesWeightTacho, null))
                {
                    tachosSet = true;
                }

                else
                {
                    
                    GameObject tachoLinksGO = GameObject.FindWithTag("TachoPlayerLinks");
                    GameObject tachoRechtsGO = GameObject.FindWithTag("TachoPlayerRechts");
                    if(tachoLinksGO == null)
                    {
                        Debug.LogError("BlendShapesController: Cant Find Tachonadel Links; Tacho not set");
                        tachosSet = false;
                        return;
                    }

                    else
                    {
                        blendShapesIndexTacho = tachoLinksGO.GetComponent<Tachonadel>();
                    }

                    if(tachoRechtsGO == null)
                    {
                        Debug.LogError("BlendShapesController: Cant Find Tachonadel Rechts; Tacho not set");
                        tachosSet = false;
                        return;
                    }

                    else
                    {
                        blendShapesWeightTacho = tachoRechtsGO.GetComponent<Tachonadel>();
                    }

                    
                    return;
                }
            }

            // Set current Index

            currentIndex = Mathf.RoundToInt(Mathf.Lerp(0, shapeControls.Length - 1, blendShapesIndexTacho.GetTargetPositionNorm()));




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

            if (currentIndex != previousIndex)
            {
                blendShapesWeightTacho.SetTargetPositionNorm(Mathf.InverseLerp(shapeControls[currentIndex].minWeight,
                                                                        shapeControls[currentIndex].maxWeight,
                                                                        shapeControls[currentIndex].currentWeight));



            }

            previousIndex = currentIndex;

            // Set Target Weight with Tacho-Value

            if (!shapeControls[currentIndex].vfxEffect)
            {
                shapeControls[currentIndex].targetWeight = Mathf.Lerp(shapeControls[currentIndex].minWeight,
                                                                              shapeControls[currentIndex].maxWeight,
                                                                              blendShapesWeightTacho.GetTargetPositionNorm());
            }

            else
            {
                shapeControls[currentIndex].targetWeight = Mathf.Lerp(shapeControls[currentIndex].vFXControl.vfxParameters[0].valueMin,
                                                                       shapeControls[currentIndex].vFXControl.vfxParameters[0].valueMax,
                                                                       blendShapesWeightTacho.GetTargetPositionNorm());
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




        private void ResetAllWeights()
        {
            for (int i = 0; i < shapeControls.Length; i++)
            {
                shapeControls[i].targetWeight = shapeControls[i].minWeight;
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
