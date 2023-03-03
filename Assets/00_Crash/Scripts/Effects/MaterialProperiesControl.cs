using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.MaterialControl
{
    public class MaterialProperiesControl : MonoBehaviour
    {

        [SerializeField] TexturesChanger[] texturesChangers;
        [SerializeField] FloatChanger[] floatChangers;
        [SerializeField] ColorChanger[] colorChangers;

        [System.Serializable]
        struct TexturesChanger
        {
            public string name;
            public string propertyName;
            public int propertyID;

            public float changeFrequencyMin;
            public float changeFrequencyMax;

            public float currentTimer;
            public float currentRandomMoment;
            public bool currentRandomMomentSet;

            public Texture currentTexture;

            public Texture[] textures;
        }

        [System.Serializable]
        struct FloatChanger
        {
            public string name;
            public string propertyRef;

            public int propertyID;

            public float changeFrequencyMin;
            public float changeFrequencyMax;

            public float changeOverTime;

            public float currentTimer;
            public float currentRandomMoment;
            public bool currentRandomMomentSet;

            public float currentValue;

            public float[] values;

        }

        [System.Serializable]
        struct ColorChanger
        {
            public string name;
            public string propertyName;

            public int propertyID;

            public float changeFrequencyMin;
            public float changeFrequencyMax;

            public float currentTimer;
            public float currentRandomMoment;
            public bool currentRandomMomentSet;


            public Color currentColor;

            public Color[] colors;


        }



        public Renderer renderer = new Renderer();




        private MaterialPropertyBlock block;

        void OnEnable()
        {
            block = new MaterialPropertyBlock();

            renderer = GetComponent<Renderer>();

            InitialSetup();
        }

        void Start()
        {

        }

        private void InitialSetup()
        {
            for (int i = 0; i < colorChangers.Length; i++)
            {
                colorChangers[i].propertyID = Shader.PropertyToID(colorChangers[i].propertyName);
                colorChangers[i].currentRandomMomentSet = false;
            }



            for (int i = 0; i < texturesChangers.Length; i++)
            {
                texturesChangers[i].propertyID = Shader.PropertyToID(texturesChangers[i].propertyName);
                texturesChangers[i].currentRandomMomentSet = false;
            }

            for (int i = 0; i < floatChangers.Length; i++)
            {
                floatChangers[i].propertyID = Shader.PropertyToID(floatChangers[i].propertyRef);
                floatChangers[i].currentRandomMomentSet = false;
            }
        }






        void Update()
        {

            renderer.GetPropertyBlock(block);


            // progress Time
            for (int i = 0; i < colorChangers.Length; i++)
            {
                colorChangers[i].currentTimer += Time.deltaTime;
            }

            for (int i = 0; i < texturesChangers.Length; i++)
            {
                texturesChangers[i].currentTimer += Time.deltaTime;
            }

            for (int i = 0; i < floatChangers.Length; i++)
            {
                floatChangers[i].currentTimer += Time.deltaTime;
            }


            // Set Random Moments 


            for (int i = 0; i < colorChangers.Length; i++)
            {
                if (!colorChangers[i].currentRandomMomentSet)
                {
                    colorChangers[i].currentRandomMoment = SetCurrentRandomMoment(colorChangers[i].changeFrequencyMin, colorChangers[i].changeFrequencyMax);
                    colorChangers[i].currentRandomMomentSet = true;
                }

            }

            for (int i = 0; i < texturesChangers.Length; i++)
            {
                if (!texturesChangers[i].currentRandomMomentSet)
                {
                    texturesChangers[i].currentRandomMoment = SetCurrentRandomMoment(texturesChangers[i].changeFrequencyMin, texturesChangers[i].changeFrequencyMax);
                    texturesChangers[i].currentRandomMomentSet = true;
                }
            }

            for (int i = 0; i < floatChangers.Length; i++)
            {
                if (!floatChangers[i].currentRandomMomentSet)
                {
                    floatChangers[i].currentRandomMoment = SetCurrentRandomMoment(floatChangers[i].changeFrequencyMin, floatChangers[i].changeFrequencyMax);
                    floatChangers[i].currentRandomMomentSet = true;
                }
            }

            // Manage Moments

            for (int i = 0; i < colorChangers.Length; i++)
            {
                if (colorChangers[i].currentTimer < colorChangers[i].currentRandomMoment) continue;

                int randomInteger = UnityEngine.Random.Range(0, colorChangers[i].colors.Length);
                colorChangers[i].currentColor = colorChangers[i].colors[randomInteger];

                block.SetColor(colorChangers[i].propertyID, colorChangers[i].currentColor);

                colorChangers[i].currentTimer = 0;
                colorChangers[i].currentRandomMomentSet = false;

            }

            for (int i = 0; i < texturesChangers.Length; i++)
            {
                if (texturesChangers[i].currentTimer < texturesChangers[i].currentRandomMoment) continue;

                int randomInteger = UnityEngine.Random.Range(0, texturesChangers[i].textures.Length);
                texturesChangers[i].currentTexture = texturesChangers[i].textures[randomInteger];

                block.SetTexture(texturesChangers[i].propertyID, texturesChangers[i].currentTexture);

                texturesChangers[i].currentTimer = 0;
                texturesChangers[i].currentRandomMomentSet = false;

            }

            for (int i = 0; i < floatChangers.Length; i++)
            {
                if (floatChangers[i].currentTimer < floatChangers[i].currentRandomMoment) continue;

                int randomInteger = UnityEngine.Random.Range(0, floatChangers[i].values.Length);
                floatChangers[i].currentValue = floatChangers[i].values[randomInteger];

                block.SetFloat(floatChangers[i].propertyID, floatChangers[i].currentValue);

                floatChangers[i].currentTimer = 0;
                floatChangers[i].currentRandomMomentSet = false;
            }

            // Change over time

            for (int i = 0; i < floatChangers.Length; i++)
            {
                if (floatChangers[i].changeOverTime == 0) continue;
                floatChangers[i].currentValue += floatChangers[i].changeOverTime;

                block.SetFloat(floatChangers[i].propertyID, floatChangers[i].currentValue);
            }


            renderer.SetPropertyBlock(block);






        }



        private float SetCurrentRandomMoment(float changeFrequencyMin, float changeFrequencyMax)
        {
            return UnityEngine.Random.Range(changeFrequencyMin, changeFrequencyMax);
        }
    }


}
