using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Looping
{

    public class XRLoopinAutomaticMove : MonoBehaviour
    {
        [SerializeField] Transform direction;
        [SerializeField] float startSpeed = 5f;
        float currentSpeed;
        [SerializeField] float changeTime = 5f;
        [SerializeField] AnimationCurve changeCurve;


        XRLoopingMover loopingMover;

        void Start()
        {
            loopingMover = GetComponent<XRLoopingMover>();
            currentSpeed = startSpeed;
        }




        void Update()
        {
            loopingMover.Move(direction.forward * currentSpeed * Time.deltaTime);

        }

        public void ChangeSpeed(float value)
        {
            StartCoroutine(FadeToSpeed(value));
        }

        IEnumerator FadeToSpeed(float targetValue)
        {
            float time = 0f;
            float valueAtStart = currentSpeed;
            while (time <= changeTime)
            {
                time += Time.deltaTime;
                currentSpeed = Mathf.Lerp(valueAtStart, targetValue, changeCurve.Evaluate(time / changeTime));
                yield return null;
            }
            yield break;
        }
    }

}
