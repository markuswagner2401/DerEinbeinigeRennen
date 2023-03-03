using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.MaterialControl;
using UnityEngine;

namespace ObliqueSenastions.Animation
{
    public class TribuenenPlayer : MonoBehaviour
    {
        [SerializeField] Tribuene[] tribuenen;

        [SerializeField] bool activateAllGOs = false;


        [System.Serializable]
        public struct Tribuene
        {
            public string note;
            public BlendShapesPlayer blendShapePlayer;
            public MaterialPropertiesFader_2 materialPropertiesFader;
        }

        [SerializeField] TribunesBlendShapeChanger[] tribunesBlendShapeChangers;

        [System.Serializable]
        public struct TribunesBlendShapeChanger
        {
            public string note;
            public int[] tribuneIndices;

            public bool all;

            public float duration;

            public AnimationCurve curve;
            public string blendShapeName;
        }



        [SerializeField] TribunesTextureChanger[] tribunesTextureChangers;
        [System.Serializable]
        public struct TribunesTextureChanger
        {
            public string note;

            public int[] tribuneIndices;

            public bool all;

            public bool randomTextureChange;
            public int[] textureChangerIndices;

            public bool fade;
            public float fadeDuration;

            public AnimationCurve curve;

        }



        [SerializeField] int bSChangerAtStart = -1;
        [SerializeField] int tChangerAtStart = -1;
        [SerializeField] int activatorAtStart = -1;

        [SerializeField] TribuneActivator[] activators;

        [System.Serializable]
        public struct TribuneActivator
        {
            public string note;
            public int[] activeTribunes;

            public bool all;

            public bool accumulate;
            public bool flicker;
            public int numberOfActive;
            public float frequencyMin;
            public float frequencyMax;
        }

        bool activatorInterrupted = false;


        void Start()
        {
            if (activateAllGOs)
            {
                for (int i = 0; i < tribuenen.Length; i++)
                {
                    tribuenen[i].blendShapePlayer.gameObject.SetActive(true);
                }
            }


            if (bSChangerAtStart >= 0 && bSChangerAtStart < tribunesBlendShapeChangers.Length)
            {
                ChangeTribunesBlendShapes(bSChangerAtStart);
            }

            if (tChangerAtStart >= 0 && tChangerAtStart < tribunesTextureChangers.Length)
            {
                ChangeTribunesTextures(tChangerAtStart);
            }

            if (activatorAtStart >= 0 && activatorAtStart < activators.Length)
            {
                PlayActivator(activatorAtStart);
            }
        }

        void Update()
        {

        }

        ///

        public void ChangeDuration(int bSChangerIndex, float newDuration)
        {
            tribunesBlendShapeChangers[bSChangerIndex].duration = newDuration;
        }

        public void ChangeCurve(int bSChangerIndex, AnimationCurve newCurve)
        {
            tribunesBlendShapeChangers[bSChangerIndex].curve = newCurve;
        }

        public void ChangeTSDuration(int index, float newDuration)
        {
            tribunesTextureChangers[index].fadeDuration = newDuration;
        }

        public void ChangeTSCurve(int index, AnimationCurve newCurve)
        {
            tribunesTextureChangers[index].curve = newCurve;
        }

        ///public blend shapes methods

        public void ChangeTribunesBlendShapes(int changerIndex)
        {
            if (tribunesBlendShapeChangers[changerIndex].all)
            {
                tribunesBlendShapeChangers[changerIndex].tribuneIndices = CreateAllTribuneIndices();
            }
            foreach (int tribuneIndex in tribunesBlendShapeChangers[changerIndex].tribuneIndices)
            {
                ChangeTribuneBlendShape(tribuneIndex, tribunesBlendShapeChangers[changerIndex].duration, tribunesBlendShapeChangers[changerIndex].curve, tribunesBlendShapeChangers[changerIndex].blendShapeName);
            }
        }

        private int[] CreateAllTribuneIndices()
        {
            List<int> allIndices = new List<int>();
            for (int i = 0; i < tribuenen.Length; i++)
            {
                allIndices.Add(i);
            }
            return allIndices.ToArray();
        }

        public void ChangeTribunesBlendShapes(string name)
        {
            int index = GetBSChangerIndexByName(name);
            if (index < 0) return;
            ChangeTribunesBlendShapes(index);
        }

        ///public texture methods

        public void ChangeTribunesTextures(int changerIndex)
        {
            if (tribunesTextureChangers[changerIndex].all)
            {
                tribunesTextureChangers[changerIndex].tribuneIndices = CreateAllTribuneIndices();
            }
            if (tribunesTextureChangers[changerIndex].randomTextureChange)
            {
                foreach (int tribuneIndex in tribunesTextureChangers[changerIndex].tribuneIndices)
                {
                    if (tribunesTextureChangers[changerIndex].fade)
                    {
                        FadeInTribuneRandomTextur(tribuneIndex, tribunesTextureChangers[changerIndex].fadeDuration, tribunesTextureChangers[changerIndex].curve);
                    }
                    else
                    {
                        SetTribuneRandomTexture(tribuneIndex);
                    }

                }
                return;
            }

            if (tribunesTextureChangers[changerIndex].textureChangerIndices.Length == 1)
            {
                foreach (var tribuneIndex in tribunesTextureChangers[changerIndex].tribuneIndices)
                {
                    if (tribunesTextureChangers[changerIndex].fade)
                    {
                        FadeInTribuneTexture(tribuneIndex, tribunesTextureChangers[changerIndex].textureChangerIndices[0], tribunesTextureChangers[changerIndex].fadeDuration, tribunesTextureChangers[changerIndex].curve);
                    }
                    else
                    {
                        SetTribuneTexture(tribuneIndex, tribunesTextureChangers[changerIndex].textureChangerIndices[0]);
                    }

                }
            }

            else if (tribunesTextureChangers[changerIndex].textureChangerIndices.Length > 1)
            {
                foreach (var tribuneIndex in tribunesTextureChangers[changerIndex].tribuneIndices)
                {
                    int randomIndex = Random.Range(0, tribunesTextureChangers[changerIndex].textureChangerIndices.Length);
                    int textureChangerIndex = tribunesTextureChangers[changerIndex].textureChangerIndices[randomIndex];
                    if (tribunesTextureChangers[changerIndex].fade)
                    {
                        FadeInTribuneTexture(tribuneIndex, textureChangerIndex, tribunesTextureChangers[changerIndex].fadeDuration, tribunesTextureChangers[changerIndex].curve);

                    }
                    else
                    {
                        SetTribuneTexture(tribuneIndex, textureChangerIndex);
                    }

                }
            }

        }



        public void ChangeTribunesTextures(string name)
        {
            int index = GetTSChangerIndexByName(name);
            if (index < 0) return;
            ChangeTribunesTextures(index);
        }



        private int GetTSChangerIndexByName(string name)
        {
            for (int i = 0; i < tribunesTextureChangers.Length; i++)
            {
                if (name == tribunesTextureChangers[i].note)
                {
                    return i;
                }
            }

            print("No Texture Changer found with name: " + name);
            return -1;
        }

        private int GetBSChangerIndexByName(string name)
        {
            for (int i = 0; i < tribunesBlendShapeChangers.Length; i++)
            {
                if (name == tribunesBlendShapeChangers[i].note)
                {
                    return i;
                }
            }

            print("no BSChanger Index Found with name: " + name);
            return -1;
        }



        /// blend shapes

        private void ChangeTribuneBlendShape(int tribuneIndex, float fadeInTime, AnimationCurve curve, string blendShapeName)
        {
            if (!tribuenen[tribuneIndex].blendShapePlayer.gameObject.activeInHierarchy) return;
            tribuenen[tribuneIndex].blendShapePlayer.FadeInBlendShapeByName(fadeInTime, curve, blendShapeName);
        }

        private void ChangeTribuneBlendShape(string tribuneName, float fadeInTime, AnimationCurve curve, string blendShapeName)
        {
            int index = GetTribuneIndexByName(tribuneName);
            if (index < 0) return;
            ChangeTribuneBlendShape(index, fadeInTime, curve, blendShapeName);
        }



        /// shader

        private void SetTribuneTexture(int tribuneIndex, int textureChangerIndex)
        {
            if (!tribuenen[tribuneIndex].materialPropertiesFader.gameObject.activeInHierarchy) return;
            tribuenen[tribuneIndex].materialPropertiesFader.SetTexture(textureChangerIndex);
        }

        private void SetTribuneTexture(string tribuneName, int textureChangerIndex)
        {
            int index = GetTribuneIndexByName(tribuneName);
            if (index < 0) return;
            SetTribuneTexture(index, textureChangerIndex);
        }



        private void SetTribuneRandomTexture(int tribuneIndex)
        {
            if (!tribuenen[tribuneIndex].materialPropertiesFader.gameObject.activeInHierarchy) return;
            tribuenen[tribuneIndex].materialPropertiesFader.SetRandomTexture();
        }

        private void SetTribuneRandomTexture(string tribuneName)
        {
            int index = GetTribuneIndexByName(tribuneName);
            if (index < 0) return;
            SetTribuneRandomTexture(index);
        }

        private void FadeInTribuneTexture(int tribuneIndex, int textureChangerIndex, float duration, AnimationCurve curve)
        {
            if (!tribuenen[tribuneIndex].materialPropertiesFader.gameObject.activeInHierarchy) return;
            tribuenen[tribuneIndex].materialPropertiesFader.FadeInTexture(textureChangerIndex, duration, curve);
        }

        private void FadeInTribuneRandomTextur(int tribuneIndex, float duration, AnimationCurve curve)
        {
            if (!tribuenen[tribuneIndex].materialPropertiesFader.gameObject.activeInHierarchy) return;
            tribuenen[tribuneIndex].materialPropertiesFader.FadeInRandomTexture(duration, curve);
        }



        /// Activator

        int runningRoutine;
        public void SetTribuneActive(bool value, int tribuneIndex)
        {

            //tribuenen[tribuneIndex].blendShapePlayer.gameObject.SetActive(value);
            tribuenen[tribuneIndex].materialPropertiesFader.GetSkinnedMeshRenderer(0).enabled = value;
        }

        public void PlayActivator(int index)
        {
            StartCoroutine(InterruptAndPlayActivator(index));
        }

        public void PlayActivator(string name)
        {
            int index = GetActivatorIndexByName(name);
            if (index >= 0)
            {
                PlayActivator(index);
            }
        }

        private int GetActivatorIndexByName(string name)
        {
            for (int i = 0; i < activators.Length; i++)
            {
                if (name == activators[i].note)
                {
                    return i;
                }
            }
            print("no activator found with name: " + name);
            return -1;
        }

        IEnumerator InterruptAndPlayActivator(int index)
        {
            activatorInterrupted = true;
            yield return new WaitForSeconds(activators[runningRoutine].frequencyMax);
            activatorInterrupted = false;
            runningRoutine = index;
            StartCoroutine(ActivationRoutine(index));
            StartCoroutine(DeactivationRoutine(index));
            yield break;
        }

        IEnumerator ActivationRoutine(int index)
        {

            List<int> alreadyActivated = new List<int>();
            List<int> indicesToActivate = new List<int>();
            if (activators[index].all)
            {
                for (int i = 0; i < tribuenen.Length; i++)
                {
                    indicesToActivate.Add(i);
                }
            }
            else
            {
                indicesToActivate = new List<int>(activators[index].activeTribunes);
            }

            int capturedCount = indicesToActivate.Count;

            //print("activation: " + index + " indicsToActivate: " + ListToString(indicesToActivate));

            if (!activators[index].accumulate)
            {
                foreach (var item in indicesToActivate)
                {
                    SetTribuneActive(true, item);
                }
                yield break;
            }

            while (alreadyActivated.Count < capturedCount && !activatorInterrupted)
            {

                int j = Random.Range(0, indicesToActivate.Count);

                int newCandidate = indicesToActivate[j];

                indicesToActivate.Remove(newCandidate);


                SetTribuneActive(true, newCandidate);
                alreadyActivated.Add(newCandidate);





                float waiting = Random.Range(activators[index].frequencyMin, activators[index].frequencyMax);

                yield return new WaitForSeconds(waiting);

            }
            yield break;
        }



        IEnumerator DeactivationRoutine(int index)
        {
            List<int> alreadyDeactivated = new List<int>();
            List<int> indicesToDeactivate = new List<int>();

            if (activators[index].all)
            {
                indicesToDeactivate = new List<int>();
            }

            else
            {
                indicesToDeactivate = TribuneIndicesToDeactivate(index);
            }



            int capturedCount = indicesToDeactivate.Count;

            //print("deactivation: " + index + " indicsToDeActivate: " + ListToString(indicesToDeactivate));
            if (!activators[index].accumulate)
            {
                foreach (var item in indicesToDeactivate)
                {
                    SetTribuneActive(false, item);
                }
                yield break;
            }

            while (alreadyDeactivated.Count < capturedCount && !activatorInterrupted)
            {

                int j = Random.Range(0, indicesToDeactivate.Count);

                int newCandidate = indicesToDeactivate[j];
                indicesToDeactivate.Remove(newCandidate);
                SetTribuneActive(false, newCandidate);
                alreadyDeactivated.Add(newCandidate);

                if (!activators[index].accumulate) yield return null;

                float waiting = Random.Range(activators[index].frequencyMin, activators[index].frequencyMax);


                yield return new WaitForSeconds(waiting);
            }

            yield break;
        }

        List<int> TribuneIndicesToDeactivate(int index)
        {
            List<int> tribuneIndicesToDeactivate = new List<int>();

            for (int i = 0; i < tribuenen.Length; i++)
            {
                bool shouldBeActive = false;
                foreach (var item in activators[index].activeTribunes)
                {
                    if (i == item) shouldBeActive = true;
                }

                //print("tribune " + i + "shouldBeActive " + shouldBeActive + " active in hierarchy: " + (tribuenen[i].blendShapePlayer.gameObject.activeInHierarchy) );

                if (shouldBeActive) continue;

                if (tribuenen[i].blendShapePlayer.gameObject.activeInHierarchy)
                {
                    tribuneIndicesToDeactivate.Add(i);
                }
            }

            return tribuneIndicesToDeactivate;
        }

        ///

        private int GetTribuneIndexByName(string name)
        {
            for (int i = 0; i < tribuenen.Length; i++)
            {
                if (name == tribuenen[i].note)
                {
                    return i;
                }
            }
            print("no tribune index found with name: " + name);
            return -1;
        }

        string ListToString(List<int> list)
        {
            string result = "List Content ";
            foreach (var item in list)
            {
                result += ", " + item.ToString();
            }
            return result;
        }

    }


}

