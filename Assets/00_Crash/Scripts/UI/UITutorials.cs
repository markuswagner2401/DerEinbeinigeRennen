using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ObliqueSenastions.VRRigSpace;
using UnityEngine.Events;
using ObliqueSenastions.TimelineSpace;

namespace ObliqueSenastions.UISpace
{
    public class UITutorials : MonoBehaviour
    {
        [SerializeField] bool playAtStart = false;
        [SerializeField] bool enableTutorialsOnArmsNotMoving = true;

        [SerializeField] GameObject[] deactivateAtStart;
        [SerializeField] Tutorial[] tutorials;


        [System.Serializable]
        public struct Tutorial
        {
            public string name;

            public bool eraseAtStart;

            public bool goOutImmediatly;

            public bool goIntoUiTime;

            public UnityEvent doOnStartTutorial;

            public float durationBeforefirstMessage;

            public GameObject[] objectsToActivate;

            public Message[] messages;

            public bool loopUntilArmsMove;

            public Message[] successMessages;

            public float durationAfterMessages;

            public UnityEvent doOnEndTutorial;

            public int maxRepetitions;

            public int repetitionsCounter;

        }

        [System.Serializable]
        public struct Message
        {
            public string text;
            public float duration;
        }

        [SerializeField] TextMeshProUGUI tmPro = null;

        [SerializeField] SimpleVelocityTracker observedTrackerLeft = null;

        [SerializeField] SimpleVelocityTracker observedTrackerRight = null;

        [Tooltip("to detect bad tracking")]
        [SerializeField] OVRHand leftHand = null;

        [Tooltip("to detect bad tracking")]
        [SerializeField] OVRHand rightHand = null;

        [SerializeField] float acceptedDurationWithoutMovement = 5f;

        [SerializeField] float speedThreshold = 0.01f;

        [SerializeField] bool onlyChekcforArmsTracked = true;

        float timer;

        bool armsMoving = false;

        //bool interrupted;



        bool motivationTriggered = false;


        void Start()
        {
            if (playAtStart)
            {
                PlayTutorial(0);
            }

            foreach (var item in deactivateAtStart)
            {
                item.SetActive(false);
            }
        }


        void Update()
        {
            if (!enableTutorialsOnArmsNotMoving) return;

            if (!ArmMoving(observedTrackerLeft, leftHand) && !ArmMoving(observedTrackerRight, rightHand))
            {
                timer += Time.deltaTime;
                armsMoving = false;
            }

            else
            {
                timer = 0;
                armsMoving = true;
            }

            if (timer > acceptedDurationWithoutMovement)
            {
                if (!motivationTriggered)
                {

                    PlayTutorial(1); // index 1 for reminder tutorial
                }
            }


        }

        public void ObserveArmsMoving(bool value)
        {
            enableTutorialsOnArmsNotMoving = value;
        }

        bool ArmMoving(SimpleVelocityTracker tracker, OVRHand hand)
        {
            if (tracker == null) return true;
            if (hand != null && !hand.IsDataHighConfidence) return false;
            if(onlyChekcforArmsTracked) return true;
            return tracker.GetLocalSpeed() > speedThreshold;
        }

        public void PlayTutorial(int index)
        {
            if(tutorials[index].repetitionsCounter > tutorials[index].maxRepetitions) return;
            motivationTriggered = true;
            StopAllCoroutines();
            StartCoroutine(PlayMotivationR(index));
        }

        public void PlayTutorial(string name)
        {
            int index = GetIndexByName(name);
            if (index < 0)
            {
                print("no tutorial found with name: " + name);
                return;
            }
            PlayTutorial(index);
        }

        private int GetIndexByName(string name)
        {
            int index = -1;
            for (int i = 0; i < tutorials.Length; i++)
            {
                if (name == tutorials[i].name)
                {
                    return index = i;
                }
            }
            return index;
        }

        public void EndTutorial()
        {
            StopAllCoroutines();
            motivationTriggered = false;
        }

        // IEnumerator InterruptAndPlayMotivation(int index)
        // {
        //     interrupted = true;
        //     yield return new WaitForSeconds(0.1f);
        //     interrupted = false;
        //     StartCoroutine(PlayMotivationR(index));
        //     yield break;
        // }

        IEnumerator PlayMotivationR(int index)
        {
            tutorials[index].doOnStartTutorial.Invoke();

            tutorials[index].repetitionsCounter += 1;


            
            bool messageComplete = false;

            if (tutorials[index].eraseAtStart)
            {
                tmPro.text = "";
            }

            if (tutorials[index].goIntoUiTime)
            {
                GoIntoUITime(true);
            }


            foreach (var item in tutorials[index].objectsToActivate)
            {
                item.SetActive(true);
                yield return null;
            }

            yield return new WaitForSeconds(tutorials[index].durationBeforefirstMessage);


            string capturedText = tmPro.text;

            int i = 0;

            while ((tutorials[index].goOutImmediatly && !messageComplete) || (tutorials[index].loopUntilArmsMove && !armsMoving))
            {

                tmPro.text = "\n" + "<color=#DADADA>" + tutorials[index].messages[i].text + "\n" + "<color=#585858>" + capturedText;
                capturedText = "\n" + tutorials[index].messages[i].text + "\n" + capturedText;

                i += 1;

                if (i == tutorials[index].messages.Length)
                {
                    messageComplete = true;
                    print("message complete");
                }

                i %= tutorials[index].messages.Length;

                if(!messageComplete || (tutorials[index].loopUntilArmsMove && !armsMoving))
                {
                     yield return null;
                }

                yield return new WaitForSeconds(tutorials[index].messages[i].duration);

            }

            if (tutorials[index].successMessages.Length > 0)
            {
                int j = 0;
                bool successComplete = false;
                while (armsMoving && !successComplete)
                {
                    tmPro.text = "";
                    tmPro.text = "\n\n\n" + "<color=#38f51b>" + tutorials[index].successMessages[j].text;
                    yield return new WaitForSeconds(tutorials[index].successMessages[j].duration);
                    j += 1;

                    successComplete = (j >= tutorials[index].successMessages.Length);
                    
                    yield return null;
                    
                }

            }



            yield return new WaitForSeconds(tutorials[index].durationAfterMessages);

            foreach (var item in tutorials[index].objectsToActivate)
            {
                item.SetActive(false);
            }

            tutorials[index].doOnEndTutorial.Invoke();

            if (tutorials[index].goIntoUiTime)
            {
                GoIntoUITime(false);
            }


            motivationTriggered = false;
            timer = 0;
            yield break;

        }

        private void GoIntoUITime(bool yes)
        {
            TimeLineHandler.instance.GetComponent<TimelineTime>().UseCustomTime(yes);
        }


    }
}






