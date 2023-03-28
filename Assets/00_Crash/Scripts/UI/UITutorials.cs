using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ObliqueSenastions.VRRigSpace;
using UnityEngine.Events;
using ObliqueSenastions.TimelineSpace;
using ObliqueSenastions.PunNetworking;

namespace ObliqueSenastions.UISpace
{
    public class UITutorials : MonoBehaviour
    {
        [SerializeField] bool playAtStart = false;
        [SerializeField] bool enableTutorialsOnArmsNotMoving = true;

        [SerializeField] bool welcomeNetworkPlayer = true;

        [SerializeField] GameObject[] deactivateAtStart;
        [SerializeField] Tutorial[] tutorials;

        [System.Serializable]
        public enum TutorialLoopModes
        {
            loopUntilNextTut,
            playOnce,
            playCompleteThenLoopUntilArmsMove,
            goOutImmediatelyWhenArmsMove
        }


        [System.Serializable]
        public struct Tutorial
        {
            public string name;

            public bool eraseAtStart;

            public Role[] excludeRoles;

            public TutorialLoopModes loopMode;

            public bool goIntoUiTime;

            public UnityEvent doOnStartTutorial;

            public float durationBeforefirstMessage;

            public GameObject[] objectsToActivate;

            public Message[] messages;

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

        [SerializeField] int reminderTutIndex = 1;

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

            MultiplayerConnector.instance.my_OnJoinedRoom += PlayNetworkWelcomeTutorial;
        }

        private void OnDisable()
        {
            MultiplayerConnector.instance.my_OnJoinedRoom -= PlayNetworkWelcomeTutorial;
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
                //print("arms not moving for more than 5 seconds");
                if (!motivationTriggered)
                {

                    PlayTutorial(reminderTutIndex); // index 1 for reminder tutorial
                }
            }




        }

        public void SetReminderIndex(string name)
        {
            int index = GetIndexByName(name);
            if (index < 0)
            {
                print("no tutorial found with name: " + name);
                return;
            }
            SetReminderIndex(index);

        }

        public void SetReminderIndex(int index)
        {
            reminderTutIndex = index;
        }

        public void ObserveArmsMoving(bool value)
        {
            enableTutorialsOnArmsNotMoving = value;
        }

        bool ArmMoving(SimpleVelocityTracker tracker, OVRHand hand)
        {
            if (tracker == null) return true;
            if (hand != null && !hand.IsDataHighConfidence) return false;
            if (onlyChekcforArmsTracked) return true;
            return tracker.GetLocalSpeed() > speedThreshold;
        }

        public void PlayTutorial(int index)
        {
            if (!this.enabled) return;
            if (tutorials[index].repetitionsCounter > tutorials[index].maxRepetitions) return;
            foreach (var role in tutorials[index].excludeRoles)
            {
                if (MultiplayerConnector.instance.GetRole() == role) return;
            }
            motivationTriggered = true;
            StopAllCoroutines();
            StartCoroutine(PlayMotivationR(index));
        }

        void PlayNetworkWelcomeTutorial()
        {
            if (!welcomeNetworkPlayer)
            {
                return;
            }
            else
            {
                this.enabled = true;
            }

            if (MultiplayerConnector.instance.GetRole() == Role.Rennfahrer)
            {
                PlayTutorial("WelcomeRacer");
            }

            else if (MultiplayerConnector.instance.GetRole() == Role.Zuschauer)
            {
                PlayTutorial("WelcomeZuschauer");
                print("welcome zuschauer");
            }
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

            bool stayInLoop = true;

            while (stayInLoop)
            {

                tmPro.text = "\n" + "<color=#DADADA>" + tutorials[index].messages[i].text + "\n" + "<color=#585858>" + capturedText;
                capturedText = "\n" + tutorials[index].messages[i].text + "\n" + capturedText;







                /// determine loop

                if (tutorials[index].loopMode == TutorialLoopModes.loopUntilNextTut)
                {
                    stayInLoop = true;
                }
                else if (tutorials[index].loopMode == TutorialLoopModes.playOnce)
                {
                    stayInLoop = messageComplete ? false : true;
                }
                else if (tutorials[index].loopMode == TutorialLoopModes.playCompleteThenLoopUntilArmsMove)
                {
                    if (!messageComplete)
                    {
                        stayInLoop = true;
                    }
                    else
                    {
                        //if message complete ->
                        stayInLoop = armsMoving ? false : true;
                        if (!stayInLoop) continue;
                    }
                }
                else if (tutorials[index].loopMode == TutorialLoopModes.goOutImmediatelyWhenArmsMove)
                {
                    stayInLoop = armsMoving ? false : true;
                    if (!stayInLoop) continue;

                }

                yield return new WaitForSeconds(tutorials[index].messages[i].duration);

                if (i >= tutorials[index].messages.Length - 1)
                {
                    messageComplete = true;
                    print("message complete");
                }
                i += 1;
                i %= tutorials[index].messages.Length;
                yield return null;

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

        private void GoIntoUITime(bool value)
        {
            TimeLineHandler.instance.GetComponent<TimeModeMachine>().SetUiTime(value);
        }


    }
}






