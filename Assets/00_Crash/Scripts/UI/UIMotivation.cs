using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ObliqueSenastions.VRRigSpace;
using UnityEngine.Events;


namespace ObliqueSenastions.UISpace
{
    public class UIMotivation : MonoBehaviour
    {
        [SerializeField] bool playAtStart = false;
        [SerializeField] Motivation[] motivationen;


        [System.Serializable]
        public struct Motivation
        {
            public string name;

            public bool eraseAtStart;

            public UnityEvent doOnStartMotivation;

            public GameObject[] objectsToActivate;

            public Message[] messages;

            public bool loopUntilArmsMove;

            public float durationAfterMessages;

            public UnityEvent doOnEndMotivation;

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

        [SerializeField] float speedThreshold = 0.05f;

        float timer;

        bool armsMoving = false;

        //bool interrupted;



        bool motivationTriggered = false;


        void Start()
        {
            if (playAtStart)
            {
                PlayMotivation(0);
            }
        }


        void Update()
        {
            if (!ArmMoving(observedTrackerLeft, leftHand) || !ArmMoving(observedTrackerRight, rightHand))
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
                    
                    PlayMotivation(0);
                }



            }


        }

        bool ArmMoving(SimpleVelocityTracker tracker, OVRHand hand)
        {
            if (tracker == null) return true;
            if(hand != null && !hand.IsDataHighConfidence) return false;
            return tracker.GetLocalSpeed() > speedThreshold;
        }

        public void PlayMotivation(int index)
        {
            motivationTriggered = true;
            StopAllCoroutines();
            StartCoroutine(PlayMotivationR(index));
        }

        public void EndMotivation()
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
            motivationen[index].doOnStartMotivation.Invoke();
            bool messageComplete = false;

            if(motivationen[index].eraseAtStart)
            {
                tmPro.text = "";
            }
            

            foreach (var item in motivationen[index].objectsToActivate)
            {
                item.SetActive(true);
                yield return null;
            }


            string capturedText = tmPro.text;

            int i = 0;

            while (!messageComplete || (motivationen[index].loopUntilArmsMove && !armsMoving))
            {
                
                tmPro.text = "\n" + "<color=#DADADA>" + motivationen[index].messages[i].text + "\n" + "<color=#585858>" + capturedText;
                capturedText = "\n" + motivationen[index].messages[i].text + "\n" + capturedText;
                
                i += 1; 

                if(i == motivationen[index].messages.Length)
                {
                    messageComplete = true;
                    print("message complete");
                }

                i %= motivationen[index].messages.Length;

                

                yield return new WaitForSeconds(motivationen[index].messages[i].duration);

            }



            yield return new WaitForSeconds(motivationen[index].durationAfterMessages);

            foreach (var item in motivationen[index].objectsToActivate)
            {
                item.SetActive(false);
            }

            motivationen[index].doOnEndMotivation.Invoke();


            motivationTriggered = false;
            timer = 0;
            yield break;

        }


    }
}






