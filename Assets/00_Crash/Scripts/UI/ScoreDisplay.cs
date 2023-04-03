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

            public string overwriteText;
            public int lastScore;
            public int countdownStartNumber;
            public float countdownTiming;

            public String endOfCountdownText;
            public bool inCountdown;


            public TextMeshProUGUI tmp;
        }

        [SerializeField] int currentCoundownEndGroupIndex = -1;

        [SerializeField] CoundownEndEventGroup[] onCountdownEndEventGroups;

        //[SerializeField] CoundownEndEvent[] countdownEndEventZuschauer;

        [System.Serializable]
        public struct CoundownEndEventGroup
        {
            public string note;

            public UnityEvent[] countdownEndEvents;

            public bool playOnce;

            public bool played;

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
                if (scores[i].currentScore != scores[i].lastScore || scores[i].showText != scores[i].lastShowText)
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
            if (scores[index].inCountdown) return;
            if (index > scores.Length) return;
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
            if (scores[index].inCountdown) return;
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

            if (index == 0) // main racer end events
            {
                if (currentCoundownEndGroupIndex >= 0 && currentCoundownEndGroupIndex < onCountdownEndEventGroups.Length)
                {
                    if (!(onCountdownEndEventGroups[currentCoundownEndGroupIndex].playOnce && onCountdownEndEventGroups[currentCoundownEndGroupIndex].played))
                    {
                        for (int i = 0; i < onCountdownEndEventGroups[currentCoundownEndGroupIndex].countdownEndEvents.Length; i++)
                        {
                            onCountdownEndEventGroups[currentCoundownEndGroupIndex].countdownEndEvents[i].Invoke();
                            yield return null;
                        }

                        onCountdownEndEventGroups[currentCoundownEndGroupIndex].played = true;
                    }
                }
            }

            // Last Countdown Text

            scores[index].overwriteText = scores[index].endOfCountdownText;
            yield return new WaitForSeconds(scores[index].countdownTiming);
            scores[index].overwriteText = "";

            // Out of countdown mode

            scores[index].currentScore = capturedScoreBeforeCountdown;
            scores[index].inCountdown = false;
            scores[index].showText = capturedShowText;

             yield break;

        }

        public void SetCountdownEndEventGroup(int index)
        {

            if (index >= onCountdownEndEventGroups.Length) return;
            currentCoundownEndGroupIndex = index;

        }





        /// display

        public void ShowAllText(bool value)
        {
            for (int i = 0; i < scores.Length; i++)
            {
                scores[i].showText = value;
            }
        }

        public void SetShowText(int index, bool value)
        {
            if (index >= scores.Length) return;
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

