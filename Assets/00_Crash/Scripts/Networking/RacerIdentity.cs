using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObliqueSenastions.MaterialControl;
using Photon.Pun;
using ObliqueSenastions.Animation;
using ObliqueSenastions.UISpace;

namespace ObliqueSenastions.PunNetworking
{
    public class RacerIdentity : MonoBehaviour
    {
        [SerializeField] Identity[] identities;
        [System.Serializable]
        public struct Identity
        {
            public string note;

            public int blendShapesIndex;

        }

        [SerializeField] MaterialPropertiesFader_2 materialPropertiesFader_2 = null;

        [Tooltip("To Check if Owned By Local Player")]
        [SerializeField] PhotonView photonView = null;

        [SerializeField] BodyBlendShapesAnimator bodyBlendShapesAnimator = null;

        [SerializeField] ScoreDisplay scoreDisplay = null;





        public int identityIndex = -1;

        public bool isMine = false;

        public int score;

        int lastScore;

        private void Start()
        {
            GameObject scoreDisplayGo = GameObject.FindWithTag("ScoreDisplay");
            {
                if (scoreDisplayGo != null)
                {
                    scoreDisplay = scoreDisplayGo.GetComponent<ScoreDisplay>();
                }
            }

            StartCoroutine(LateStartRoutine());

            lastScore = score;
        }

        private void Update() 
        {
            if(lastScore != score)
            {
                scoreDisplay.SetScore(identityIndex, score);
            }

            lastScore = score;
        }

        IEnumerator LateStartRoutine()
        {
            yield return new WaitForSeconds(3f);

            // Get Index
            int index = MultiplayerConnector.instance.GetClientsIndexInRole();
            if (index >= identities.Length)
            {
                Debug.LogError("RacerIdentity: No identity set up with Clietns index: " + index);
                yield break;
            }
            identityIndex = index;
            

            // Mine Check
            if (photonView != null)
            {
                isMine = photonView.IsMine;
            }

            SetMaterial(isMine);
            SetColor(identityIndex);
            SetBlendShape(identityIndex, isMine);




            yield break;
        }

        void SetColor(int number)
        {
            //print("do on have number");
            materialPropertiesFader_2.ChangeColor(number);
            
        }

        void SetMaterial(bool isMine)
        {
            if (isMine)
            {
                materialPropertiesFader_2.SetMaterial(0);
                materialPropertiesFader_2.SetMaterial(1);
            }

            else
            {
                materialPropertiesFader_2.SetMaterial(2);
                materialPropertiesFader_2.SetMaterial(3);
            }

        }

        void SetBlendShape(int number, bool isMine)
        {
            if(isMine)
            {
                if(bodyBlendShapesAnimator != null)
                {
                    //bodyBlendShapesAnimator.GoIntoIsMineShape(true);
                    bodyBlendShapesAnimator.PlayBSShapesState(0);
                }
            }

            else
            {
                bodyBlendShapesAnimator.PlayBSShapesState(identities[identityIndex].blendShapesIndex);
            }
        }


        /// Scoring

        public void AddScore()
        {
            score += 1;
        }

        public void AddScore(int value)
        {
            score += value;
        }


    }

}

