using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Animation
{

    public class BodyBlendShapesAnimator : MonoBehaviour
    {
        [SerializeField] BlendShapesChanger[] blendShapesChangers;


        [System.Serializable]
        public struct BlendShapesChanger
        {
            public string name;
            public BlendShapeState[] blendShapeStates;

            public float duration;

        }


        [System.Serializable]
        public struct BlendShapeState
        {
            public int BSIndex;
            public float targetValueMin;

            public float targetValueMax;

            public float durationMin;
            public float durationMax;

            public AnimationCurve curve;
        }

        [SerializeField] int startIndexMin = 0;
        [SerializeField] int startIndexMax;

        [SerializeField] SkinnedMeshRenderer smr;

        bool interrupted = false;

        private void Start()
        {
            int checkedStartIndexMax = (startIndexMax >= blendShapesChangers.Length) ? blendShapesChangers.Length - 1 : startIndexMax;
            //print("start index max " + checkedStartIndexMax);
            int checkedStartIndexMin = (startIndexMin >= blendShapesChangers.Length) ? 0 : startIndexMin;
            //print("start index min " + checkedStartIndexMin);

            int newStartIndex = Random.Range(checkedStartIndexMin, checkedStartIndexMax);
            //print("start index: " + newStartIndex);

            PlayBSShapesState(newStartIndex, 1f); // Lerp to start values in 1 second
        }

        public void PlayBSShapesState(int index)
        {
            StartCoroutine(InterruptAndPlayBSShapesState(index, true, 1f));
        }

        public void PlayBSShapesState(int index, float overwriteDuration)
        {
            StartCoroutine(InterruptAndPlayBSShapesState(index, false, overwriteDuration));
        }

        public void PlayBSShapesState(int index, bool useIndividualDurations)
        {
            StartCoroutine(InterruptAndPlayBSShapesState(index, useIndividualDurations, blendShapesChangers[index].duration));
        }

        IEnumerator InterruptAndPlayBSShapesState(int index, bool useIndividualDurations, float overwriteDuration)
        {
            interrupted = true;
            yield return new WaitForSeconds(0.1f);
            interrupted = false;
            StartCoroutine(PlayBSShapesRoutine(index, useIndividualDurations, overwriteDuration));
            yield break;

        }

        IEnumerator PlayBSShapesRoutine(int index, bool useIndividualDurations, float overwriteDuration)
        {
            for (int j = 0; j < blendShapesChangers[index].blendShapeStates.Length; j++)
            {
                BlendShapeState blendShapeState = blendShapesChangers[index].blendShapeStates[j];
                int blendShapeIndex = blendShapeState.BSIndex;
                float startValue = smr.GetBlendShapeWeight(blendShapeIndex);
                float targetValue = Random.Range(blendShapeState.targetValueMin, blendShapeState.targetValueMax);
                float duration = useIndividualDurations ? Random.Range(blendShapeState.durationMin, blendShapeState.durationMax) : overwriteDuration;
                AnimationCurve curve = blendShapeState.curve;
                StartCoroutine(LerpStartValueToTargetValueR(blendShapeIndex, startValue, targetValue, duration, curve));
                yield return null;
            }



            yield break;
        }

        IEnumerator LerpStartValueToTargetValueR(int blendShapeIndex, float startValue, float targetValue, float duration, AnimationCurve curve)
        {
            float timer = 0f;
            while (timer < duration && !interrupted)
            {
                timer += Time.deltaTime;
                float newValue = Mathf.Lerp(startValue, targetValue, curve.Evaluate(timer / duration));
                smr.SetBlendShapeWeight(blendShapeIndex, newValue);
                yield return null;
            }
            yield break;
        }

    }


}
