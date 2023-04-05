using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.UISpace
{
    public class CountdownListener : MonoBehaviour
    {
        [SerializeField] UnityEvent onCountdownStartListenerEvents;
        [SerializeField] UnityEvent onCountownEndListenerEvents;

        [SerializeField] string scoreDisplayTag = "ScoreDisplay";


        void Start()
        {
            /// Listen To Countdown

            GameObject scoreDisplayGO = GameObject.FindGameObjectWithTag(scoreDisplayTag);
            if (scoreDisplayGO != null)
            {
                ScoreDisplay scoreDisplay = scoreDisplayGO.GetComponent<ScoreDisplay>();
                if (scoreDisplay != null)
                {
                    scoreDisplay.OnCountdownEndDelegate += OnCountdownMainPlayerEnd;
                    scoreDisplay.OnCountdownStartDelegate += OnCountdownMainPlayerStart;
                }
                else
                {
                    Debug.LogError("ScoreDisplay component not found on the object with tag " + scoreDisplayTag);
                }
            }
            else
            {
                Debug.LogError("No object found with tag " + scoreDisplayTag);
            }

        }

        

        private void OnDestroy()
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag(scoreDisplayTag);
            if (targetObject != null)
            {
                ScoreDisplay scoreDisplay = targetObject.GetComponent<ScoreDisplay>();
                if (scoreDisplay != null)
                {
                    scoreDisplay.OnCountdownEndDelegate -= OnCountdownMainPlayerEnd;
                    scoreDisplay.OnCountdownStartDelegate += OnCountdownMainPlayerStart;
                }
            }
        }

        private void OnCountdownMainPlayerStart()
        {
            onCountdownStartListenerEvents.Invoke();
        }

        private void OnCountdownMainPlayerEnd()
        {
            onCountownEndListenerEvents.Invoke();
        }

        
    }

}

