using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.StageMasterSpace;
using UnityEngine;

namespace ObliqueSenastions.MaterialControl
{
    public class ImageSequencePlayer : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer smr;

        [SerializeField] MaterialPropertiesFader_2 materialPropertiesFader = null;

        [SerializeField] StageMaster stageMaster = null;
        MaterialPropertyBlock block = null;



        [SerializeField] ImageSequence[] sequences;

        // [SerializeField] VideoElement[] videoSequences;

        [System.Serializable]
        public struct ImageSequence
        {
            public string note;

            public int group;
            public Texture[] images;

            public int currentIndex;

            public bool play;

            public bool playTriggered;


            public float fpsMin;

            public float fpsMax;
            public int stepSizeMin;

            public int stepSizeMax;

            public string targetPropRef;

            //public SkinnedMeshRenderer smr;
        }

        [SerializeField] int currentSequenceGroup = -1;
        [SerializeField] int startSequenceGroup = -1;

        //[SerializeField] ValueChanger[] valueChangers;

        [SerializeField] ValueChangerGroup[] valueChangerGroups;

        [SerializeField] int currentChangerGroup;
        [SerializeField] int startChangerGroup;

        [System.Serializable]
        public struct ValueChanger
        {
            public string note;

            public int changerGroup;
            public string propRef;
            public float valueMin;
            public float valueMax;


            public float capturedValue;
        }

        [System.Serializable]
        struct ValueChangerGroup
        {
            public string note;
            public ValueChanger[] valueChangers;
        }

        //[SerializeField] int currentValueChangerGroupIndex = 0;

        [SerializeField] int changeValuesAfterXTimesMin = -1;
        [SerializeField] int changeValuesAfterXTimesMax = -1;

        int changeValuesAfterXTimes;





        int counter;



        // public struct VideoElement
        // {
        //     public string note;
        //     public  VideoClip videoClip;

        //     public float fps;

        //     public float scipFrames;

        //     public string targetProRef;
        // }

        [SerializeField] bool autoPlay = false;
        [SerializeField] float changeSequenceAfterXSecondsMin = Mathf.Infinity;
        [SerializeField] float changeSequenceAfterXSecondesMax = Mathf.Infinity;

        float changeSequenceAfterXSeconds = Mathf.Infinity;

        [SerializeField] bool scrubbing = false;
        [SerializeField] float scrubbingSpeedMin = 0.1f;
        [SerializeField] float scrubbingSpeedMax = 0.2f;
        float scrubbingSpeed = 0.1f;

        [SerializeField] float changeValueGroupAfterXSecondsMin = Mathf.Infinity;
        [SerializeField] float changeValueGroupAfterXSecondsMax = Mathf.Infinity;

        float changeValueGroupAfterXSeconds;




        float changeSequenceTimer;

        float scrubbingTimer = 0;

        float changeValueGroupTimer = 0;

        [SerializeField] bool stop = false;


        bool scrubbingTriggered = false;


        void Start()
        {
            StartCoroutine(GetPropertyBlock());
            currentChangerGroup = startChangerGroup;
            currentSequenceGroup = startSequenceGroup;

            if (stageMaster == null)
            {
                stageMaster = GameObject.FindWithTag("StageMaster").GetComponent<StageMaster>();
            }

            if (stageMaster != null)
            {
                //        stageMaster.onStartStopCommand += StartStopImageSequence;
            }


            if (!(changeValuesAfterXTimesMin < 0) && !(changeValuesAfterXTimesMax < 0))
            {
                changeValuesAfterXTimes = UnityEngine.Random.Range(changeValuesAfterXTimesMin, changeValuesAfterXTimesMax);
            }

            scrubbingSpeed = UnityEngine.Random.Range(scrubbingSpeedMin, scrubbingSpeedMax);

            if (autoPlay)
            {
                changeSequenceAfterXSeconds = UnityEngine.Random.Range(changeSequenceAfterXSecondsMin, changeSequenceAfterXSecondesMax);

                changeValueGroupAfterXSeconds = UnityEngine.Random.Range(changeValueGroupAfterXSecondsMin, changeValueGroupAfterXSecondsMax);


            }


            //block = new MaterialPropertyBlock();

            CaptureStartValues();

            //block.SetFloat("Vector1_1831ba61239d4da3b7c88edbf7638464", 0f);

            if (autoPlay)
            {
                ContinueWithRandomSequence(currentSequenceGroup);

            }

            lastChangerGroup = currentChangerGroup;
        }



        // private void CreateChangerGroups(){

        // }

        //  private int[] GroupIndices(){
        //     List<int> groupIndices = new List<int>();
        //     foreach (var item in valueChangers)
        //     {
        //         for (int i = 0; i < groupIndices.Count; i++)
        //         {
        //             if(groupIndices[i] == item.changerGroup) continue;
        //             groupIndices.Add(item.changerGroup);
        //         }
        //     }
        //     return groupIndices.ToArray();
        // }

        // private ValueChanger[] ValueChangerGroups(int groupIndex){
        //     List<ValueChanger> valueChangerGroup = new List<ValueChanger>();

        //     foreach (var item in valueChangers){
        //         if (item.changerGroup == groupIndex){
        //             valueChangerGroup.Add(item);
        //         }  
        //     }
        //     return valueChangerGroup.ToArray();
        // }


        int lastChangerGroup;
        void Update()
        {

            if (autoPlay && !stop)
            {
                if (currentChangerGroup != lastChangerGroup)
                {
                    ContinueWithRandomSequence(currentChangerGroup);
//                    print("continue with random sequence");
                }

            }

            lastChangerGroup = currentChangerGroup;

            if (autoPlay && !stop)
            {
                //changeSequenceTimer += Time.unscaledDeltaTime;
                changeValueGroupTimer += Time.unscaledDeltaTime;
            }

            if (scrubbing && !stop)
            {
                scrubbingTimer += Time.unscaledDeltaTime;
            }

            // if(changeSequenceTimer > changeSequenceAfterXSeconds){
            //     changeSequenceTimer = 0;

            //     if(currentSequenceGroup < 0){

            //         ContinueWithRandomSequence();
            //     }
            //     else{
            //         ContinueWithRandomSequence(currentSequenceGroup);
            //     }


            //     changeSequenceAfterXSeconds = UnityEngine.Random.Range(changeSequenceAfterXSecondsMin, changeSequenceAfterXSecondesMax);
            // }

            if (changeValueGroupTimer > changeValueGroupAfterXSeconds)
            {
                changeValueGroupTimer = 0;

                changeValueGroupAfterXSeconds = UnityEngine.Random.Range(changeValueGroupAfterXSecondsMin, changeValueGroupAfterXSecondsMax);

                string currentGroupName = valueChangerGroups[currentChangerGroup].note;
                SetValueChangerGroupIndex(currentGroupName);

            }

            // if(scrubbingTimer > scrubbingSpeed){
            //     scrubbingTimer = 0;
            //     ContinueWithRandomSequence(currentSequenceGroup); // scrub through all except 0
            //     scrubbingSpeed = UnityEngine.Random.Range(scrubbingSpeedMin, scrubbingSpeedMax);

            // }

            if (scrubbing)
            {
                if (scrubbingTriggered) return;
                scrubbingTriggered = true;
                StartCoroutine(ScrubRoutine());
            }

            else
            {

                scrubbingTriggered = false;
            }





            // if(Input.GetKeyDown(KeyCode.P)){
            //     ContinueWithRandomSequence();
            // }

            // if(Input.GetKeyDown(KeyCode.S)){
            //     PauseAllSequences();
            // }

            if (!ReferenceEquals(block, null))
            {
                smr.SetPropertyBlock(block);
            }

        }

        IEnumerator ScrubRoutine()
        {
            while (scrubbing)
            {
                ChangeValue(currentChangerGroup);

                int sequenceIndex = UnityEngine.Random.Range(0, sequences.Length);
                int imageIndex = sequences[sequenceIndex].currentIndex + UnityEngine.Random.Range(sequences[sequenceIndex].stepSizeMin, sequences[sequenceIndex].stepSizeMax);
                SetImage(sequenceIndex, imageIndex);
                yield return new WaitForSeconds(0.1f);
            }



            yield break;
        }

        //In Order to Work together with material properties fader and use the same block

        IEnumerator GetPropertyBlock()
        {
            if (materialPropertiesFader == null) yield break;
            while (block == null)
            {
                block = materialPropertiesFader.GetPropertyBlock();
                yield return null;
            }



            yield break;
        }

        public void SetValueChangerGroupIndex(string groupName)
        {
            int tempIndex = currentChangerGroup;
            int counter = 0;

            do
            {
                counter++;
                //tempIndex++;
                // if(tempIndex >= valueChangerGroups.Length){
                //     tempIndex = 0;
                // }
                tempIndex = UnityEngine.Random.Range(0, valueChangerGroups.Length);

            } while (valueChangerGroups[tempIndex].note != groupName || !(counter > 100));
            //        print("set new index: " + tempIndex);
            currentChangerGroup = tempIndex;
        }



        public void SetValueChangerGroupIndex(int index)
        {
            if (index >= valueChangerGroups.Length)
            {
                currentChangerGroup = 0;
                return;
            }
            currentChangerGroup = index;

        }
        public void SetScrubbing(bool value)
        {
            scrubbing = value;
        }

        void CaptureStartValues()
        {


            for (int i = 0; i < valueChangerGroups.Length; i++)
            {
                for (int j = 0; j < valueChangerGroups[i].valueChangers.Length; j++)
                {
                    valueChangerGroups[i].valueChangers[j].capturedValue = smr.materials[0].GetFloat(valueChangerGroups[i].valueChangers[j].propRef);
                }

                block.SetFloat(valueChangerGroups[i].valueChangers[0].propRef, valueChangerGroups[i].valueChangers[0].capturedValue);
            }
        }



        public void PauseAllSequences()
        {
            for (int i = 0; i < sequences.Length; i++)
            {
                PauseSequence(i);
            }
        }

        public void ContinueWithRandomSequence()
        {

            int randomNr = UnityEngine.Random.Range(0, sequences.Length);
            //        print("continue with random sequence: " + randomNr);
            ContinueSequence(randomNr);
        }

        public void ContinueWithRandomSequence(int group)
        {
            int index = GetRandomIndexOfGroup(group);
            //       print("continue with random sequence " + index + "of group " + group);
            ContinueSequence(index);
        }

        private int GetRandomIndexOfGroup(int group)
        {

            int randomNr;
            int counter = 0;
            do
            {
                randomNr = UnityEngine.Random.Range(0, sequences.Length);
                //print("test new random number: " + randomNr + "to " + sequences[randomNr].group);
                counter++;
            } while (sequences[randomNr].group != group && counter < 100);
            //print("counter: "+ counter);
            return randomNr;

        }

        public void SetSequenceGroup(int index)
        {
            currentSequenceGroup = index;
        }


        public void ContinueSequence(int index)
        {
            if(sequences[index].play == true) return;

            PauseOtherSequences(index);

            StartCoroutine(ContinueSequenceR(index));
        
            
        }

       

        private void PauseOtherSequences(int index)
        {
            for (int i = 0; i < sequences.Length; i++)
            {

                if (i == index)
                {
                    continue;
                }
                if (sequences[index].targetPropRef == sequences[i].targetPropRef)
                {
                    sequences[i].play = false;
                }

            }
        }

        public void PauseSequence(int index)
        {
            sequences[index].play = false;
        }


        IEnumerator ContinueSequenceR(int sequenceIndex)
        {
            print("continue sequence ");
            sequences[sequenceIndex].play = true;
            int currentIndex = sequences[sequenceIndex].currentIndex;

            while (sequences[sequenceIndex].play)
            {
                SetImage(sequenceIndex, currentIndex);
                ProcessValueChangers();
                yield return new WaitForSeconds(1f / UnityEngine.Random.Range(sequences[sequenceIndex].fpsMin, sequences[sequenceIndex].fpsMax));

                currentIndex += UnityEngine.Random.Range(sequences[sequenceIndex].stepSizeMin, sequences[sequenceIndex].stepSizeMax); ;
                if (currentIndex >= sequences[sequenceIndex].images.Length)
                {
                    currentIndex = currentIndex - (sequences[sequenceIndex].images.Length);
                }
                if (currentIndex < 0)
                {
                    currentIndex = currentIndex * -1;
                }

                //yield break;
            }

            print("stop sequence r");

            yield break;
        }

        public void SetImage(int sequenceIndex, int imageIndex)
        {

            if (sequences[sequenceIndex].images.Length <= imageIndex) return;

            block.SetTexture(sequences[sequenceIndex].targetPropRef, sequences[sequenceIndex].images[imageIndex]);

            sequences[sequenceIndex].currentIndex = imageIndex;

        }



        private void ProcessValueChangers()
        {
            
            if (changeValuesAfterXTimesMin < 0 || changeValuesAfterXTimesMax < 0) return;
            counter += 1;
            if (counter > changeValuesAfterXTimes)
            {
                

                print("change value");
                // int index = UnityEngine.Random.Range(0, valueChangerGroups[currentChangerGroup].valueChangers.Length);
                // ChangeValue(index);

                

                for (int i = 0; i < valueChangerGroups[currentChangerGroup].valueChangers.Length; i++)
                {
                    ChangeValue(i);
                }


                counter = 0;
                changeValuesAfterXTimes = UnityEngine.Random.Range(changeValuesAfterXTimesMin, changeValuesAfterXTimesMax);

            }

        }
        private void ChangeValue(int index)
        {
            float newValue = UnityEngine.Random.Range(valueChangerGroups[currentChangerGroup].valueChangers[index].valueMin, valueChangerGroups[currentChangerGroup].valueChangers[index].valueMax);
            block.SetFloat(valueChangerGroups[currentChangerGroup].valueChangers[index].propRef, newValue);
        }


        private void StartStopImageSequence(bool value)
        {
            if (!value)
            {
                stop = true;
                PauseAllSequences();
            }
            else
            {
                stop = false;
                ContinueWithRandomSequence();
            }
        }


    }

}

