using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.StageMasterSpace
{


    

    public class Boxenstoptimer : MonoBehaviour
    {
        [SerializeField] private float durationToNextBoxenstop;
        [SerializeField] private UnityEvent onStartTimer;
        [SerializeField] private UnityEvent onTimerEnd;

        [SerializeField] bool inForceBoxenstop = false;

        private float timer;
        private bool isTimerRunning;
        private bool eventTriggered;

        [SerializeField] UnityEvent repeatInBoxenstop;
        [SerializeField] float frequency = 10f;

        float repeatTimer;

        private void Update()
        {
            if (isTimerRunning)
            {
                timer += Time.deltaTime;
                if (timer >= durationToNextBoxenstop && !eventTriggered)
                {
                    isTimerRunning = false;
                    eventTriggered = true;
                    inForceBoxenstop = true;
                    onTimerEnd.Invoke();
                }
            }

            if(inForceBoxenstop)
            {
                repeatTimer += Time.deltaTime;
                if(repeatTimer > frequency)
                {
                    repeatInBoxenstop.Invoke();
                    repeatTimer = 0;
                }
            }
            else
            {
                repeatTimer = 0;
            }
        }

        public void StartTimer()
        {
            isTimerRunning = true;
            timer = 0.0f;
            eventTriggered = false;
            inForceBoxenstop = false;
            onStartTimer.Invoke();
        }

        public void HoldTimer()
        {
            isTimerRunning = false;
        }

        public void StopTimer()
        {
            isTimerRunning = false;
            timer = 0.0f;
            eventTriggered = false;
            inForceBoxenstop = false;
        }
    }


}
