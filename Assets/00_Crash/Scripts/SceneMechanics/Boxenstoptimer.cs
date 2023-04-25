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

        private float timer;
        private bool isTimerRunning;
        private bool eventTriggered;

        private void Update()
        {
            if (isTimerRunning)
            {
                timer += Time.deltaTime;
                if (timer >= durationToNextBoxenstop && !eventTriggered)
                {
                    isTimerRunning = false;
                    eventTriggered = true;
                    onTimerEnd.Invoke();
                }
            }
        }

        public void StartTimer()
        {
            isTimerRunning = true;
            timer = 0.0f;
            eventTriggered = false;
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
        }
    }


}
