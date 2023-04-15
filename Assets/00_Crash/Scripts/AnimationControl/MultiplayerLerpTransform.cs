using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;
using ObliqueSenastions.PunNetworking;
using Photon.Pun;
using ObliqueSenastions.TimelineSpace;

namespace ObliqueSenastions.VRRigSpace
{
    public class MultiplayerLerpTransform : MonoBehaviour
    {
        [SerializeField] Formation[] formations;
        
        [System.Serializable]
        public struct Formation
        {
            public string name;
            public Transform source;
            public Transform[] multiplayerSources;
            public float weight;

            public bool switchToNextFormation;

            
        }

        [SerializeField] bool useTimelineTime;

        [SerializeField] TimelineTime timelineTime = null;

        [SerializeField] float thresholdtoSwitchToNextFormation = 0.99f;

        [SerializeField] int currentFormation = 0;

        [SerializeField] bool rotation;

        [SerializeField] bool scale;
        [SerializeField] bool readAtTacho = true;
        [SerializeField] Tachonadel tacho;

        [SerializeField] float mapMinSpeed;

        [SerializeField] float mapMaxSpeed;

        [SerializeField] float speedFactor;

        [SerializeField] Lerp[] lerps;

        [System.Serializable]
        public struct Lerp
        {
            public string note;

            public float targetValue;

            public float duration;

            public AnimationCurve curve;
        }


        Vector3 capturedStartPosition = new Vector3();

        Quaternion capturedStartRotation = new Quaternion();

        Vector3 capturedStartScale = new Vector3();

        int multiplayerIndex;
        private bool isInterrupted;
        private bool isLerping;

        [SerializeField] bool setupAtStart = false;

        

        

        void Start()
        {
            if(setupAtStart)
            {
                CapturePosition();
            }
            

            if(PhotonNetwork.IsConnected)
            {
                SetupMultiplayerSources();
            }

            else
            {
                MultiplayerConnector.instance.my_OnJoinedRoom += SetupMultiplayerSources;
            }

            
            
            
            

        }

        


        void Update()
        {
            
            if(useTimelineTime)
            {
                if(timelineTime == null)
                {
                    timelineTime = TimeLineHandler.instance.GetComponent<TimelineTime>();
                }
            }

            if(currentFormation >= formations.Length) return;
            if(formations[currentFormation].source == null) return;

            if (readAtTacho)
            {
                float t = ReadWeightAtTacho(tacho);
                float speed = MapSpeed(mapMinSpeed, mapMaxSpeed, t);
                PlayWeight(currentFormation ,speed * speedFactor);
            }

            if(formations[currentFormation].switchToNextFormation && formations[currentFormation].weight > thresholdtoSwitchToNextFormation)
            {
                CapturePositionAndChangeIndex(currentFormation + 1);
            }

            

            if (formations[currentFormation].weight <= 0f) return;

            transform.position = Vector3.Lerp(capturedStartPosition, formations[currentFormation].source.position, formations[currentFormation].weight);

            if (rotation)
            {
                transform.rotation = Quaternion.Lerp(capturedStartRotation, formations[currentFormation].source.rotation, formations[currentFormation].weight);
            }

            // if (scale)
            // {
            //     transform.localScale = Vector3.Lerp(capturedStartScale, source.localScale, weight1);
            // }

        }

        /// setup

        void SetupMultiplayerSources()
        {
            multiplayerIndex = MultiplayerConnector.instance.GetClientsIndexInRole();
            if(formations.Length <= 0) return;
            for (int i = 0; i < formations.Length; i++)
            {
                int index;
                if(formations[i].multiplayerSources.Length == 0)
                {
                    return;
                }

                else
                {
                    index = multiplayerIndex % formations[i].multiplayerSources.Length;
                }
                
                formations[i].source = formations[i].multiplayerSources[index];
            }

        }

        public void CapturePosition()
        {
            capturedStartPosition = transform.position;

            capturedStartRotation = transform.rotation;

            capturedStartScale = transform.localScale;

        }

        /// index

        void CapturePositionAndChangeIndex(int index)
        {
            
            if(index >= formations.Length) return;
            CapturePosition();
            formations[index].weight = 0;
            currentFormation = index;
        }

        private void SetFormationIndex(int index)
        {
            currentFormation = index;
        }


        /// input


        private float ReadWeightAtTacho(Tachonadel tacho)
        {
            return tacho.GetNormedTargetPosition();
        }

        
        /// weight


        public void SetWeight(int formationIndex,float value)
        {
            formations[formationIndex].weight = value;
        }

        private float MapSpeed(float minSpeed, float maxSpeed, float t)
        {
            return Mathf.Lerp(minSpeed, maxSpeed, t);
        }


        public void PlayWeight(int index, float speed)
        {
            float deltaTime = useTimelineTime ? timelineTime.GetModeDependentTimelineDeltaTime() : Time.deltaTime;
            formations[index].weight = Mathf.Clamp01(Mathf.Lerp(formations[index].weight, formations[index].weight + deltaTime * speed, 0.1f )) ;
            
            
            //print("play weight: " + weight + "with speed: " + speed);
        }


        /// Lerp Speed

        public void CapturePositionThenLerp(int index)
        {
            CapturePosition();
            LerpSpeed(index);
        }

        public void LerpSpeed(int index)
        {
            StartCoroutine(InterruptAndLerpNext(index));
        }

        IEnumerator InterruptAndLerpNext(int index)
        {
            isInterrupted = true;
            yield return new WaitForSeconds(0.1f);
            isInterrupted = false;
            StartCoroutine(LerpSpeedtRoutine(index));
            yield break;

        }

        IEnumerator LerpSpeedtRoutine(int index)
        {
            isLerping = true;
            float startValue = speedFactor;
            float targetValue = lerps[index].targetValue;
            float newValue;
            float timer = 0f;

            while (timer <= lerps[index].duration && !isInterrupted)
            {
                timer += Time.unscaledDeltaTime;
                newValue = Mathf.Lerp(startValue, targetValue, lerps[index].curve.Evaluate(timer / lerps[index].duration));
                speedFactor = newValue;
                yield return null;

            }

            isLerping = false;
            yield break;

        }
    }

}


