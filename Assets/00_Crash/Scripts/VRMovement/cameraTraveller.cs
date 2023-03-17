using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
//using UnityEngine.XR.Interaction.Toolkit;
using RPG.Saving;
using Photon.Pun;
using ObliqueSenastions.PunNetworking;
using ObliqueSenastions.TimelineSpace;


namespace ObliqueSenastions.VRRigSpace
{


    public class cameraTraveller : MonoBehaviour, ISaveable
    {

        [SerializeField] Transform[] cameraOffsetTargets; // Nur Noch um vorherige ziele zu rekonstruieren.

        [SerializeField] TransitionPoints[] transitionPoints;

        [SerializeField] bool transitionPaused = false; // to do: refactor

        [System.Serializable]
        struct TransitionPoints
        {
            [Tooltip("has to match name in traveller control clip")]
            public string note;
            [Tooltip("insert XRRigOrigin")]
            public Transform cameraOffsetTarget;
            public AnimationCurve transitionCurve;
            public float transitionTime;
            public bool immediatelyGoOn;
            public float waitTimeBeforeGoOn;

            [Tooltip("for Multiplayers")]
            public Transform[] offsetFormation;
        }

        Dictionary<string, int> lookup = new Dictionary<string, int>();

        public enum TransitionMode
        {
            TransitionByTime,
            TransitionByTimeline,
            TransitionBySmoothing
        }

        [SerializeField] TransitionMode transitionMode;



        [SerializeField] float posTransitionSmoothing = 0.01f;
        [SerializeField] float rotTransitionSmoothing = 0.01f;
        [SerializeField] float lockInThreshold = 0.01f;

        [Tooltip("Choose left hand or right hand - secondary button at that hand will be used")]
        [SerializeField] XRNode xrNode;
        InputDevice inputDevice;

        [SerializeField] float multiplayerOffset = 1f;

        Vector3 velocityRot = Vector3.zero;

        bool buttonPressed;
        bool switchPosition;

        bool transitioningBySmoothing = false;
        bool transitioningByTime = false;

        bool transitionByTimeTriggered = false;

        bool transitioningByTimeline = false;

        bool transitionModeChangeTriggered = false;




        [SerializeField] int currTransPointIndex = 0;

        int prevTransPointIndex = 0;

        MultiplayerConnector multiplayerConnector = null;


        [SerializeField] Role role;



        Role previousRole;

        [SerializeField] int playerIndex;
        int previousPlayerIndex = 0;





        public delegate void OnTravellerUpdated();

        public OnTravellerUpdated onTravellerUpdateReady;



        private void Awake()
        {

            SetupLookup();

            foreach (var item in lookup)
            {
                print("traveller lookup: " + item.Key + " - " + item.Value);
            }

        }

        private void OnEnable()
        {
            onTravellerUpdateReady += PlaceholderOnTravellerUpdated;

        }

        private void OnDisable()
        {
            onTravellerUpdateReady -= PlaceholderOnTravellerUpdated;
        }



        void Start()
        {
            // currentXRRig = cameraOffsetTargets[xrRigIndex];
            inputDevice = InputDevices.GetDeviceAtXRNode(xrNode);
            multiplayerConnector = MultiplayerConnector.instance;

            if (PhotonNetwork.IsConnected)
            {
                role = multiplayerConnector.GetRole();
            }

            // Set Multiplayer Index by avatar
            // if (multiplayerConnector != null)
            // {
            //     multiplayerConnector.my_OnJoinedRoom += SetMultiplayerIndex;
            // }

            previousRole = role;
            previousPlayerIndex = playerIndex;


        }

        private void SetupLookup()
        {
            lookup.Clear();
            for (int i = 0; i < transitionPoints.Length; i++)
            {
                string name = transitionPoints[i].note;
                if (name == "" || lookup.ContainsKey(name))
                {
                    name = i.ToString();
                }
                lookup.Add(name, i);
            }
        }


        private void Update()
        {
            ProcessButtonInput();

            if (prevTransPointIndex != currTransPointIndex)
            {
                TriggerTransition();
            }

            if (transitionMode == TransitionMode.TransitionBySmoothing)
            {
                transitioningByTime = false;
                transitioningByTimeline = false;
                TransitionBySmoothing();
            }

            else if (transitionMode == TransitionMode.TransitionByTime)
            {
                transitioningBySmoothing = false;
                transitioningByTimeline = false;
                TransitionByTime();
            }

            prevTransPointIndex = currTransPointIndex;

            if (previousPlayerIndex != playerIndex || previousRole != role)
            {
                //if(previousRole == role)
                OffsetMultiplayerLocations(playerIndex);
            }

            previousRole = role;
            previousPlayerIndex = playerIndex;





        }


        void LateUpdate()
        {

            if (transitionMode == TransitionMode.TransitionByTimeline)
            {
                transitioningByTime = false;
                transitioningBySmoothing = false;

                TransitionByTimeline();
            }


            if (!transitioningByTime && !transitioningBySmoothing && !transitioningByTimeline)
            {
                transform.position = transitionPoints[currTransPointIndex].cameraOffsetTarget.position;
                transform.rotation = transitionPoints[currTransPointIndex].cameraOffsetTarget.rotation;
            }

            onTravellerUpdateReady.Invoke();

            // print(" traveller updated");

        }


        // TODO: Inplement offset formations

        public void SetRoleIdentifier(Role _role, int _index)
        {
            Debug.Log("camerTraveller: Set Role Identifier in camera traveller: " + _role + " , " + _index);
            role = _role;
            playerIndex = _index;

        }
        public void OffsetMultiplayerLocations(int playerIndex)
        {
            Debug.Log("CameraTraveller: Offset Positions for player with index: " + playerIndex);
            //if(playerIndex == 0) return;

            foreach (var item in transitionPoints)  // no multiplayer formation assigned -> use default
            {
                if (item.offsetFormation.Length == 0)
                {
                    DefaultFormation(playerIndex, item.cameraOffsetTarget);
                }
                else if (item.offsetFormation.Length <= playerIndex)  // use last item of offsetFormation
                {
                    DefaultFormation(playerIndex, item.offsetFormation[item.offsetFormation.Length - 1]);
                }
                else // set up formation
                {
                    item.cameraOffsetTarget.position = item.offsetFormation[playerIndex].position;
                    item.cameraOffsetTarget.rotation = item.offsetFormation[playerIndex].rotation;
                }


            }
        }

        private void DefaultFormation(int playerIndex, Transform location)
        {
            //default formation;

            Vector3 right = location.right;
            if (playerIndex % 2 == 0)
            {
                location.position += right * multiplayerOffset * (playerIndex / 2);
                //Debug.Log("cameraTraveller: offset transition point of " + playerIndex + " for " + multiplayerOffset * (playerIndex / 2));
            }
            else
            {
                location.position -= right * multiplayerOffset * ((playerIndex + 1) / 2);
                //Debug.Log("cameraTraveller: offset transition point of " + playerIndex + " for " + multiplayerOffset * (playerIndex + 1 / 2));
            }
        }

        /// public getters and setters

        public Transform GetCurrentXRRigTransform()
        {
            //Debug.Log("cameraTraveller: GetCurrentXRRigTransform: current index: " + currTransPointIndex);
            return transitionPoints[currTransPointIndex].cameraOffsetTarget;
        }

        public Transform GetXRRigTransform(int transitionPointIndex)
        {
            if (transitionPointIndex >= transitionPoints.Length)
            {
                return null;
            }
            return transitionPoints[transitionPointIndex].cameraOffsetTarget;
        }

        public float GetTransitionTime(int transitionPointIndex)
        {
            return transitionPoints[transitionPointIndex].transitionTime;
        }

        public void PauseTransition()
        {
            transitionPaused = !transitionPaused;
        }

        public int GetCurrentTransitionPoint()
        {
            return currTransPointIndex;
        }


        public void ChangeTransitionMode(TransitionMode newMode)
        {
            if (newMode == transitionMode) return;

            if (transitionModeChangeTriggered) return;
            transitionModeChangeTriggered = true;
            StartCoroutine(WaitForTransitionEndAndChangeMode(newMode));
        }

        IEnumerator WaitForTransitionEndAndChangeMode(TransitionMode newMode)
        {
            while (transitioningBySmoothing || transitioningByTime || TimeLineHandler.instance.GetComponent<TravellerControlByTimeline>().GetTransitioning(role))
            {
                yield return null;
            }
            transitionMode = newMode;
            transitionModeChangeTriggered = false;

            yield break;
        }

        // Trigger Transition

        void TriggerTransition() // gets triggered when previosTransPoint != currentTranspoint
        {
            switch (transitionMode)
            {
                case TransitionMode.TransitionByTime:
                    transitionByTimeTriggered = true;
                    break;
                // case TransitionMode.TransitionByTimeline:
                //     transitioningByTimeline = true;
                //     break;
                case TransitionMode.TransitionBySmoothing:
                    transitioningBySmoothing = true;
                    break;
                default:
                    // Handle unexpected values
                    break;
            }
        }

        // Process Button Input
        private void ProcessButtonInput()
        {
            if (inputDevice == null)
            {
                inputDevice = InputDevices.GetDeviceAtXRNode(xrNode);
            }

            if (inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool primaryButtonUsage) && primaryButtonUsage)
            {

                if (buttonPressed) return;
                SetNextTransitionPoint();

                // transitioningBySmoothing = true;

                buttonPressed = true;

            }
            if (inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool primaryButtonUsageSwichBack) && !primaryButtonUsageSwichBack)
            {
                buttonPressed = false;
            }
        }

        /// Set Transition Point (changes index and triggeres Transition)

        public void SetNextTransitionPoint()
        {
            if (currTransPointIndex + 1 == transitionPoints.Length)
            {
                currTransPointIndex = 0;
            }

            else
            {
                currTransPointIndex += 1;
            }

            print("new rig index = " + currTransPointIndex);

        }


        public void SetTransitionPoint(int index)
        {
            print("set transition point: " + index);
            if (index >= transitionPoints.Length || index < 0) return;

            currTransPointIndex = index;
            print("current transition point index: " + currTransPointIndex);
        }

        public void SetTransitionPoint(int index, bool changeDuration, float duration, bool changeCurve, AnimationCurve curve)
        {
            print("set transition point: " + index);
            //print("set vr rig " + gameObject.name);
            if (index >= transitionPoints.Length || index < 0) return;
            currTransPointIndex = index;

            if (changeDuration)
            {
                SetTransitionDuration(index, duration);
            }

            if (changeCurve)
            {
                SetTransitionCurve(index, curve);
            }


        }

        public void SetTransitionPoint(string name, bool changeDuration, float duration, bool changeCurve, AnimationCurve curve)
        {
            int index = GetTransitionIndexFromString(name);
            if (index < 0) return;
            SetTransitionPoint(index, changeDuration, duration, changeCurve, curve);
        }

        int GetTransitionIndexFromString(string name)
        {
            for (int i = 0; i < transitionPoints.Length; i++)
            {
                if (name == transitionPoints[i].note)
                {
                    return i;
                }
            }

            return -1;
        }


        // Adjust Transition
        public void SetTransitionDuration(int index, float duration)
        {

            transitionPoints[index].transitionTime = duration;
        }

        public void SetTransitionCurve(int index, AnimationCurve curve)
        {

            transitionPoints[index].transitionCurve = curve;
        }





        ///// Transition By Timeline
        TravellerControlByTimeline travContr = null;
        private void TransitionByTimeline()
        {
            if (travContr == null)
            {
                travContr = TimeLineHandler.instance.GetComponent<TravellerControlByTimeline>();
                return;
            }

            string transPointNamePrevClip = travContr.GetTransPointNamePrevClip(role); // Returns "" if there is no prev Clip
                                                                                       //Debug.Log("camera traveller: transition point name prev clip: " + transPointNamePrevClip);
            int transPointPrevClip = transPointNamePrevClip == "" ? travContr.GetTransitionPointIndexPrevClip(role) : lookup[transPointNamePrevClip];

            string transPointNameCurrClip = travContr.GetTransPointNameCurrClip(role);
            //Debug.Log("camera traveller: TransitionPoint name current clip: " + transPointNameCurrClip);
            int transPointCurrClip = transPointNameCurrClip == "" ? travContr.GetTransitionPointIndexCurrClip(role) : lookup[transPointNameCurrClip];


            if (transPointCurrClip >= transitionPoints.Length || transPointPrevClip >= transitionPoints.Length)
            {
                Debug.Log("camera Traveller: no transition Point Index with index defined in clip");
                return;
            }

            float t = travContr.GetCurrentT(role);

            transitioningByTimeline = (Mathf.Approximately(t, 0f)) ? false : true;

            if (transitioningByTimeline)
            {

                Vector3 t0Pos = transitionPoints[transPointPrevClip].cameraOffsetTarget.position;
                Quaternion t0Rot = transitionPoints[transPointPrevClip].cameraOffsetTarget.rotation;

                Vector3 t1Pos = transitionPoints[transPointCurrClip].cameraOffsetTarget.position;
                Quaternion t1Rot = transitionPoints[transPointCurrClip].cameraOffsetTarget.rotation;

                Vector3 newPos = Vector3.Lerp(t0Pos, t1Pos, t);
                Quaternion newRot = Quaternion.Lerp(t0Rot, t1Rot, t);

                transform.position = newPos;
                transform.rotation = newRot;

                if (t > 0.9f) currTransPointIndex = transPointCurrClip;
                if (t < 0.1f) currTransPointIndex = transPointPrevClip;

            }


        }

        /// Transition By time

        private void TransitionByTime()
        {
            if (transitionByTimeTriggered)
            {
                transitionByTimeTriggered = false;
                StopAllCoroutines();
                StartCoroutine(MakeTransition(transform.position, transform.rotation));
            }
        }

        private IEnumerator MakeTransition(Vector3 startPosition, Quaternion startRotation)
        {
            print("make transition");
            transitioningByTime = true;
            float timer = 0f;


            while (timer < transitionPoints[currTransPointIndex].transitionTime && !(currTransPointIndex >= transitionPoints.Length))
            {
                if (!transitionPaused)
                {
                    timer += Time.unscaledDeltaTime;
                }

                float progressX = timer / transitionPoints[currTransPointIndex].transitionTime;
                float valueAtX = transitionPoints[currTransPointIndex].transitionCurve.Evaluate(progressX);
                transform.position = Vector3.Lerp(startPosition, transitionPoints[currTransPointIndex].cameraOffsetTarget.position, valueAtX);
                transform.rotation = Quaternion.Lerp(startRotation, transitionPoints[currTransPointIndex].cameraOffsetTarget.rotation, valueAtX);
                yield return null;

            }

            transitioningByTime = false;

            if (transitionPoints[currTransPointIndex].immediatelyGoOn)
            {
                yield return new WaitForSeconds(transitionPoints[currTransPointIndex].waitTimeBeforeGoOn);
                SetNextTransitionPoint();
            }

            yield break;
        }

        // Transition By smoothing

        private void TransitionBySmoothing()
        {

            if (transitioningBySmoothing && Vector3.Distance(transform.position, transitionPoints[currTransPointIndex].cameraOffsetTarget.position) > lockInThreshold)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, transitionPoints[currTransPointIndex].cameraOffsetTarget.rotation, rotTransitionSmoothing);
                transform.position = Vector3.Lerp(transform.position, transitionPoints[currTransPointIndex].cameraOffsetTarget.position, posTransitionSmoothing);
            }

            else
            {
                transitioningBySmoothing = false;

            }
        }







        /////

        public object CaptureState()
        {
            return currTransPointIndex;
        }

        public void RestoreState(object state)
        {
            currTransPointIndex = (int)state;
        }

        // public void SetCurrentTransitionValues()
        // {
        //     if (xrRigIndex + 1 == transitionPoints.Length)
        //     {
        //         xrRigIndex = 0;
        //     }

        //     else
        //     {
        //         xrRigIndex += 1;
        //     }
        // }

        ///

        private void PlaceholderOnTravellerUpdated()
        {
        }


    }

}
