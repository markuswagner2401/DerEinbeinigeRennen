using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;
using ObliqueSenastions.PunNetworking;
using Photon.Pun;

namespace ObliqueSenastions.VRRigSpace
{
    public class MultiplayerLerpTransform : MonoBehaviour
    {
        [SerializeField] Transform source;

        [SerializeField] Transform[] multiplayerSources;

        [Range(0f, 1f)]
        [SerializeField] float weight;

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

        void Start()
        {
            CaptureValuesAtStart();

            if(PhotonNetwork.IsConnected)
            {
                SetupMultiplayerTransform();
            }

            else
            {
                MultiplayerConnector.instance.my_OnJoinedRoom += SetupMultiplayerTransform;
            }
            
            
            

        }

        void SetupMultiplayerTransform()
        {
            multiplayerIndex = MultiplayerConnector.instance.GetClientsIndexInRole();
            source = multiplayerSources[multiplayerIndex];

        }


        void Update()
        {
            


            if (readAtTacho)
            {
                float t = ReadWeightAtTacho(tacho);
                float speed = MapSpeed(mapMinSpeed, mapMaxSpeed, t);
                PlayWeight(speed * speedFactor);
            }

            if (weight <= 0f) return;
            transform.position = Vector3.Lerp(capturedStartPosition, source.position, weight);

            if (rotation)
            {
                transform.rotation = Quaternion.Lerp(capturedStartRotation, source.rotation, weight);
            }

            if (scale)
            {
                transform.localScale = Vector3.Lerp(capturedStartScale, source.localScale, weight);
            }






        }

        private float ReadWeightAtTacho(Tachonadel tacho)
        {
            return tacho.GetNormedTargetPosition();
        }

        // private void LateUpdate() {

        // }

        

        void CaptureValuesAtStart()
        {
            capturedStartPosition = transform.position;

            capturedStartRotation = transform.rotation;

            capturedStartScale = transform.localScale;

        }



        




        public void SetWeight(float value)
        {
            weight = value;
        }

        private float MapSpeed(float minSpeed, float maxSpeed, float t)
        {
            return Mathf.Lerp(minSpeed, maxSpeed, t);
        }


        public void PlayWeight(float speed)
        {
            weight = Mathf.Clamp01(weight + Time.deltaTime * speed);
            //print("play weight: " + weight + "with speed: " + speed);
        }


        ///

        

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


