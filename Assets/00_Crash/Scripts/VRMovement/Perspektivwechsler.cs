using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.VRRigSpace
{
    public class Perspektivwechsler : MonoBehaviour
    {
        [SerializeField] Perspektive[] perspektiven;
        [System.Serializable]
        public struct Perspektive
        {
            public string name;
            public Transform location;

            public float minStayTime;

            public float maxStayTime;
        }

        [SerializeField] bool listenToLeftHandPinch = false;
        [SerializeField] OVRHand leftHand = null;
        bool leftHandPinchTriggered;

        [SerializeField] bool listenToRightHandPinch = false;
        [SerializeField] OVRHand rightHand = null;
        bool rightHandPinchTriggered;
        [SerializeField] int currentPerspective;

        int lastPerspective;

        Vector3 capturedPosition;

        Quaternion capturedRotation;

        bool interrupted;

        bool inNoDisturbTime = false;

        float noDistutbTimer = 0;

        bool inFlexPerspective;

        void Start()
        {

        }


        void Update()
        {
            noDistutbTimer += Time.deltaTime;

            if (currentPerspective != lastPerspective)
            {
                noDistutbTimer = 0;
            }

            lastPerspective = currentPerspective;

            /// interrupt

            if (!leftHand.IsDataHighConfidence && !rightHand.IsDataHighConfidence)
            {
                interrupted = true;
            }



            /// input left hand
            if (listenToLeftHandPinch)
            {
                if (leftHand == null) return;
                if (!leftHand.IsDataHighConfidence) return;
                if (leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
                {
                    if (leftHandPinchTriggered) return;
                    leftHandPinchTriggered = true;
                    if (!inFlexPerspective)
                    {
                        VisitFlexiblePerspective();
                    }
                    SetNextIndex();

                }
                else
                {
                    leftHandPinchTriggered = false;
                }

            }


            // input right hand

            if (listenToRightHandPinch)
            {
                if (rightHand == null) return;
                if (!rightHand.IsDataHighConfidence) return;
                if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
                {
                    if (rightHandPinchTriggered) return;
                    rightHandPinchTriggered = true;
                    if (!inFlexPerspective)
                    {
                        VisitFlexiblePerspective();
                    }
                    SetNextIndex();

                }
                else
                {
                    rightHandPinchTriggered = false;
                }

            }







        }





        public void VisitFixPerspective(int index)
        {
            if (index >= perspektiven.Length) return;
            //if(inPerspective) return;
            StartCoroutine(VisitFixPerspectiveR(index));
        }

        IEnumerator VisitFixPerspectiveR(int index)
        {
            float timer = 0;
            CaptureLocation();

            while (timer < perspektiven[index].minStayTime)
            {
                timer += Time.deltaTime;
                inNoDisturbTime = true;
                transform.position = perspektiven[index].location.position;
                transform.rotation = perspektiven[index].location.rotation;
                yield return null;
            }

            inNoDisturbTime = false;

            while (timer < perspektiven[index].maxStayTime && !interrupted)
            {
                timer += Time.deltaTime;
                transform.position = perspektiven[index].location.position;
                transform.rotation = perspektiven[index].location.rotation;
                yield return null;
            }

            //reset location to captured
            transform.position = capturedPosition;
            transform.rotation = capturedRotation;

            yield break;
        }

        ////



        ////

        public void VisitFlexiblePerspective()
        {
            StartCoroutine(VisitFlexiblePerspectiveR());
        }

        IEnumerator VisitFlexiblePerspectiveR()
        {

            inFlexPerspective = true;
            CaptureLocation();
            interrupted = false;
            while (!interrupted)
            {
                if (currentPerspective >= perspektiven.Length) yield break;
                transform.position = perspektiven[currentPerspective].location.position;
                transform.rotation = perspektiven[currentPerspective].location.rotation;
                yield return null;

            }

            //reset location to captured
            transform.position = capturedPosition;
            transform.rotation = capturedRotation;

            inFlexPerspective = false;

            yield break;
        }

        void CaptureLocation()
        {
            capturedPosition = transform.position;
            capturedRotation = transform.rotation;
        }

        ///

        void SetNextIndex()
        {
            if (perspektiven.Length <= 0) return;
            if (noDistutbTimer < perspektiven[currentPerspective].minStayTime) return;
            int newIndex = (currentPerspective + 1) % perspektiven.Length;
            currentPerspective = newIndex;
        }
    }

}


