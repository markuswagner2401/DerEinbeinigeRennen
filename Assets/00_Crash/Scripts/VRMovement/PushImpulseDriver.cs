using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ObliqueSenastions.TimelineSpace;

namespace ObliqueSenastions.VRRigSpace
{
    public class PushImpulseDriver : MonoBehaviour
    {
        Rigidbody rb;

        [SerializeField] UnityEventFloat setNormalizedHeight;
        [SerializeField] float impulseFactor = 10f;

        [SerializeField] float thrustImpulseFactor = 5f;

        [SerializeField] float thrustImpulseDurationFactor = 5f;

        [SerializeField] float thrustFactor = 1f;

        [SerializeField] float myGravity = -1.2f;

        [SerializeField] float maxSpeed = 10f;

        [SerializeField] Transform targetHeight = null;

        [SerializeField] Transform botton = null;

        [SerializeField] FloatChanger[] floatChangers;



        [System.Serializable]
        public struct FloatChanger
        {
            public string name;

            public string floatName;
            public float targetValue;
            public float duration;
            public AnimationCurve curve;

            public bool interrupted;
        }

        Vector3 nullPosition = new Vector3();



        [SerializeField] float totalHeight = 10f;

        [SerializeField] bool setNullPositionAtBotton = true;

        [SerializeField] bool freeze;

        [SerializeField] bool holdTime;

        [SerializeField] bool useTimelineTime = true;

        TimelineTime timelineTime = null;



        float currentNormaizedHeight = 0f;

        bool thrustInterrupted = false;

        bool nullPositionSet = false;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;

            if (!setNullPositionAtBotton)
            {
                StartCoroutine(SetNullPosition());
            }

            else
            {
                if (botton == null)
                {
                    StartCoroutine(SetNullPosition());
                }
                else
                {
                    nullPosition = new Vector3(botton.position.x, botton.position.y + (transform.localScale.x / 2f), botton.position.z);
                    if (targetHeight != null)
                    {
                        totalHeight = (targetHeight.position - nullPosition).magnitude;
                    }
                    nullPositionSet = true;
                }
            }

            if (useTimelineTime)
            {
                timelineTime = TimeLineHandler.instance.GetComponent<TimelineTime>();
            }




        }

        IEnumerator SetNullPosition()
        {
            while (rb.velocity.magnitude > 0)
            {
                yield return null;
            }

            if (targetHeight != null)
            {
                totalHeight = (targetHeight.position - transform.position).magnitude;
            }

            nullPosition = transform.position;

            nullPositionSet = true;

            yield break;
        }

        // Update is called once per frame
        void Update()
        {
            // if(Input.GetMouseButtonDown(0))
            // {
            //     ImulseUp(1f);
            // }




        }

        private void FixedUpdate()
        {
            if (useTimelineTime)
            {
                
                if (timelineTime.GetModeDependentTimelineDeltaTime() < 0.005f)
                {
                    holdTime = true;
                }
                else
                {
                    holdTime = false;
                }
            }




            if (!freeze && !holdTime)
            {
                rb.AddForce(Vector3.down * rb.mass * myGravity);

            }

            setNormalizedHeight.Invoke(GetNormalizedHeight());



            //rigidbody.velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }

        public void ChangeFloat(string name)
        {
            int index = GetIndexOfName(name);
            if (index < 0) return;
            ChangeFloat(index);

        }

        int GetIndexOfName(string name)
        {

            for (int i = 0; i < floatChangers.Length; i++)

            {
                if (floatChangers[i].name == name)
                {
                    return i;
                }
                else continue;
            }

            print("no floatchanger found with name: " + name);
            return -1;
        }

        public void ChangeFloat(int index)
        {
            StartCoroutine(InterruptAndChangeFloatRoutine(index));
        }

        IEnumerator InterruptAndChangeFloatRoutine(int index)
        {
            for (int i = 0; i < floatChangers.Length; i++)
            {
                if (floatChangers[index].floatName == floatChangers[i].floatName)
                {
                    floatChangers[i].interrupted = true;
                }
            }
            yield return new WaitForSeconds(0.01f);
            floatChangers[index].interrupted = false;
            StartCoroutine(ChangeFloatRoutine(index));
            yield break;
        }

        IEnumerator ChangeFloatRoutine(int index)
        {

            float startValue = GetFloat(floatChangers[index].floatName);
            float timer = 0;
            //        print("Start Float Changer: " + floatChangers[index].name + ". Start Value: " + startValue + ". target Value: " + floatChangers[index].targetValue);
            while (timer < floatChangers[index].duration && !floatChangers[index].interrupted)
            {
                timer += Time.deltaTime;
                float newValue = Mathf.Lerp(startValue, floatChangers[index].targetValue, floatChangers[index].curve.Evaluate(timer / floatChangers[index].duration));
                SetFloat(floatChangers[index].floatName, newValue);
                yield return null;
            }


            yield break;
        }

        public void SetFloat(string floatName, float value)
        {

            switch (floatName)
            {
                case "myGravity":
                    myGravity = value;
                    break;

                case "thrustFactor":
                    thrustFactor = value;
                    break;

                case "impulseFactor":
                    impulseFactor = value;
                    break;
                case "thrustImpulseFactor":
                    thrustImpulseFactor = value;
                    break;
                case "thrustImpulseDurationFactor":
                    thrustImpulseDurationFactor = value;
                    break;

                default:
                    print("no float with name: " + floatName + " found");
                    break;
            }

        }

        public float GetFloat(string floatName)
        {
            float value;
            switch (floatName)
            {
                case "myGravity":
                    value = myGravity;
                    break;

                case "thrustFactor":
                    value = thrustFactor;
                    break;

                case "impulseFactor":
                    value = impulseFactor;
                    break;
                case "thrustImpulseFactor":
                    value = thrustImpulseFactor;
                    break;
                case "thrustImpulseDurationFactor":
                    value = thrustImpulseDurationFactor;
                    break;

                default:
                    print("no float with name: " + floatName + " found");
                    value = 0;
                    break;


            }

            return value;

        }

        public void SetFreeze(bool value)
        {
            freeze = value;
        }

        public void SetImpulseFactor(float value)
        {
            print("set Impulse factor to" + value);
            impulseFactor = value;
        }

        public void SetThrustImpulseFactor(float value)
        {
            thrustImpulseFactor = value;

        }

        public void SetThrustImpulseDurationFactor(float value)
        {
            thrustImpulseDurationFactor = value;
        }

        public void SetThrustFactor(float value)
        {
            thrustFactor = value;
        }



        public void ImulseUp(float strength)
        {
            if (!nullPositionSet) return;
            if (freeze || holdTime) return;

            //print("impulse up + " + strength);
            rb.AddForce(Vector3.up * strength * impulseFactor);
        }

        // thrust routine

        public void ThrustImpulseUp(float strength)
        {
            if (!nullPositionSet) return;


//            print("thrust up for " + strength * thrustImpulseDurationFactor);


            StartCoroutine(InterruptAndThrustImpulseUp(strength * thrustImpulseDurationFactor));

        }

        IEnumerator InterruptAndThrustImpulseUp(float duration)
        {
            thrustInterrupted = true;

            yield return new WaitForSeconds(0.01f);

            StartCoroutine(ThrustImpulseUpRoutine(duration));


            yield break;
        }

        IEnumerator ThrustImpulseUpRoutine(float duration)
        {
            thrustInterrupted = false;

            float timer = 0f;

            while (timer <= duration && !thrustInterrupted)
            {
                timer += Time.unscaledDeltaTime;
                if (!freeze && !holdTime)
                {
                    rb.AddForce(Vector3.up * thrustImpulseFactor);
                }


                yield return null;
            }


            yield break;
        }


        //// continuous thust

        public void ThrustUp(float strength)
        {
            if (!nullPositionSet) return;

            //print("thust up");
            if (!freeze && !holdTime)
            {
                rb.AddForce(Vector3.up * strength * thrustFactor);
            }

        }




        /////

        public float GetNormalizedHeight()
        {
            return CalculateNormalizedHeight(transform.position);
        }



        float CalculateNormalizedHeight(Vector3 currentPosition)
        {
            float currentHeightOverNull = (currentPosition - nullPosition).magnitude;
            float normalizedHeight = currentHeightOverNull / totalHeight;
            return normalizedHeight;
        }




    }

}
