using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;

namespace ObliqueSenastions.UISpace

{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] Score[] scores;

        [System.Serializable]
        public struct Score
        {
            public string note;

            public Color color;

            public bool showText;

            public bool lastShowText;
            public int currentScore;
            public int lastScore;
            public int countdownStartNumber;
            public float countdownTiming;
            public bool inCountdown;

            public UnityEvent onCountdownEnd;
            public TextMeshProUGUI tmp;
        }

        [SerializeField] CoundownEndEvent[] countdownEndEventsRacer;

        //[SerializeField] CoundownEndEvent[] countdownEndEventZuschauer;

        [System.Serializable]
        public struct CoundownEndEvent
        {
            public string note;

            public UnityEvent countdownEndEvent;

        }



        private void Start() 
        {
            for (int i = 0; i < scores.Length; i++)
            {
                UpdateDisplay(i);
            }
        }

        private void Update() 
        {
            for (int i = 0; i < scores.Length; i++)
            {
                if(scores[i].currentScore != scores[i].lastScore || scores[i].showText != scores[i].lastShowText)
                {
                    UpdateDisplay(i);
                }

                scores[i].lastScore = scores[i].currentScore;
                scores[i].lastShowText = scores[i].showText;
            }
        }

        // scoring

        // RacerIdentity sets score

        public void SetScore(int index, int newScore)
        {
            if(scores[index].inCountdown) return;
            if(index > scores.Length) return;
            scores[index].currentScore = newScore;
        }

        // public void AddScore(int index)
        // {
            
        //     if(index >= scores.Length) return;
        //     int newScore =scores[index].currentScore + 1;
        //     SetScore(index, newScore);
        // }

        // public void AddScore(int index, int count)
        // {
        //     if(index >= scores.Length) return;
        //     int newScore = scores[index].currentScore + count;
        //     SetScore(index, newScore);
        // }

        // public void SubtractScore(int index)
        // {
            
        //     if(index > scores.Length) return;
        //     int newScore = scores[index].currentScore - 1;
        //     SetScore(index, newScore);
        // }

        /// countdown

        

        public void PlayCountdown(int index)
        {
            if(scores[index].inCountdown) return;
            StartCoroutine(CountdownRoutine(index));

        }

        public void PlayAllCountdowns()
        {
            for (int i = 0; i < scores.Length; i++)
            {
                PlayCountdown(i);
            }
        }

        IEnumerator CountdownRoutine(int index)
        {
            scores[index].inCountdown = true;
            int capturedScoreBeforeCountdown = scores[index].currentScore;
            bool capturedShowText = scores[index].showText;
            scores[index].showText = true;
            
            int startNumber = scores[index].countdownStartNumber;
            for (int i = startNumber; i > 0; i--)
            {
                scores[index].currentScore = i;
                yield return new WaitForSeconds(scores[index].countdownTiming);
            }
            scores[index].currentScore = capturedScoreBeforeCountdown;
            scores[index].inCountdown = false;
            scores[index].showText = capturedShowText;
            scores[index].onCountdownEnd.Invoke();
            yield break;
        }

        public void SetCountdownEndEventRacer(int index)
        {
            if(index >= countdownEndEventsRacer.Length) return;
            scores[0].onCountdownEnd = countdownEndEventsRacer[index].countdownEndEvent;
        }

        

        

        /// display

        public void ShowAllText(bool value)
        {
            for (int i = 0; i < scores.Length; i++)
            {
                scores[i].showText = value;
            }
        }

        public void SetShowText(int index ,bool value)
        {
            if(index >= scores.Length) return;
            scores[index].showText = value;
        }

        private void UpdateDisplay(int index)
        {
            string newText = scores[index].showText ? scores[index].currentScore.ToString() : "";
            //if(newText == scores[index].tmp.text) return; // safes performance not to set the text every frame (?)
            scores[index].tmp.text = newText;
        }
        
        
    }

}

