using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ObliqueSenastions.PunNetworking;

namespace ObliqueSenastions.Animation
{

    public class BodyBlendShapesAnimator : MonoBehaviourPun
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
            public string name;
            public int BSIndex;
            public float targetValueMin;

            public float targetValueMax;

            public float durationMin;
            public float durationMax;

            public AnimationCurve curve;
        }

        [Tooltip("indeces < startIndexMin are reserved for if isMine")]
        [SerializeField] int startIndexMin = 1;
        [SerializeField] int startIndexMax;

        [SerializeField] SkinnedMeshRenderer smr;

        bool interrupted = false;


        ///

        [Tooltip("Assign view of Transform Sync at Network Player Parent")]
        [SerializeField] PhotonView photonView;

        bool isMine = false;

        

        private void Start()
        {
            if(PhotonNetwork.IsConnected)
            {
                MultiplayerSetup();
            }

            // int checkedStartIndexMax = (startIndexMax >= blendShapesChangers.Length) ? blendShapesChangers.Length - 1 : startIndexMax;
            // //print("start index max " + checkedStartIndexMax);
            // int checkedStartIndexMin = (startIndexMin >= blendShapesChangers.Length) ? 0 : startIndexMin;
            // //print("start index min " + checkedStartIndexMin);

            // int newStartIndex = Random.Range(checkedStartIndexMin, checkedStartIndexMax);
            // //print("start index: " + newStartIndex);

            // PlayBSShapesState(newStartIndex, 1f); // Lerp to start values in 1 second

            MultiplayerConnector.instance.my_OnJoinedRoom += MultiplayerSetup;
        }

        private void OnDestroy() 
        {
            MultiplayerConnector.instance.my_OnJoinedRoom -= MultiplayerSetup;
        }

        private void MultiplayerSetup()
        {
            if(photonView.IsMine)
            {
                isMine = true;
                GoIntoIsMineShape(true);

            }
            else
            {
                isMine = false;
                

            }
        }

        void GoIntoIsMineShape(bool value)
        {
            print("GoIntoMineState");
            int inMineIndex = value ? Random.Range(0, startIndexMin) : Random.Range(startIndexMin, startIndexMax);
            
            PlayBSShapesState(inMineIndex);
        }

        public void PlayBSShapesState(int index)
        {
            if(isMine)
            {
                if(index >= startIndexMin) return;
            }
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
            print("coroutine Play BS..." + index);
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
