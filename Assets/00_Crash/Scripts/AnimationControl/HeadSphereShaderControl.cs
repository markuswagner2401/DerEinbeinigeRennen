using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.MaterialControl
{
    public class HeadSphereShaderControl : MonoBehaviour
    {
        [SerializeField] Renderer rend;

        [SerializeField] FloatChanger[] floatChangers;

        [System.Serializable]
        struct FloatChanger
        {
            public string note;
            public string propRef;
            public float targetValue;

            public float duration;

            public AnimationCurve curve;
            public bool interrupted;

        }

        [System.Serializable]
        public struct ColorChanger
        {
            public string note;
            public string propRef;
            public Color newColor;

            public AnimationCurve curve;
            public bool interrupted;
        }

        bool setFloatsToCurrent;


        MaterialPropertyBlock block; // Get Block By MaterialPropControlByTimeline

        private void Start()
        {
            block = GetComponent<MaterialPropControlByTimeline>().GetBlock();

            if (setFloatsToCurrent)
            {
                //if(changeMaterialAsset) return;

                foreach (var parameter in floatChangers)
                {
                    block.SetFloat(parameter.propRef, rend.sharedMaterial.GetFloat(parameter.propRef));
                }
            }


        }

        private void Update()
        {
            // rend.SetPropertyBlock(block); is set by matPropControlByTL
        }

        public void ChangeFloat(string name)
        {
            int index = GetFloatChangerIndexOfName(name);
            if (index >= 0)
            {
                ChangeFloat(index);
            }

            else
            {
                print("no float changer with this name found at" + gameObject.name);
            }

        }

        public void ChangeFloat(int index)
        {
            StartCoroutine(InterruptAndChangeFloatR(index));
        }

        IEnumerator InterruptAndChangeFloatR(int index)
        {
            floatChangers[index].interrupted = true;
            yield return new WaitForSecondsRealtime(0.01f);
            floatChangers[index].interrupted = false;
            StartCoroutine(ChangeFloatR(index));
            yield break;
        }

        IEnumerator ChangeFloatR(int index)
        {
 
            float timer = 0f;

            //       print("change float " + floatChangers[index].note);

            float startValue = block.GetFloat(floatChangers[index].propRef);

            while (timer <= floatChangers[index].duration && !floatChangers[index].interrupted)
            {

                timer += Time.deltaTime;
                float newValue = Mathf.Lerp(startValue, floatChangers[index].targetValue, floatChangers[index].curve.Evaluate(timer / floatChangers[index].duration));
                block.SetFloat(floatChangers[index].propRef, newValue);

                //  print("change float " + floatChangers[index].note + " " + block.GetFloat(floatChangers[index].propRef));
                // print("Set Float: " + floatChangers[index].note  +  block.GetFloat(floatChangers[index].propRef));

                yield return null;
            }

            yield break;
        }

        private int GetFloatChangerIndexOfName(string name)
        {
            for (int i = 0; i < floatChangers.Length; i++)
            {
                if (floatChangers[i].note == name)
                {
                    print("float changer found: " + name);
                    return i;

                }
                else
                {
                    print("not matching: " + name + " " + floatChangers[i].note);
                    continue;
                }
            }

            return -1;
        }
    }

}


