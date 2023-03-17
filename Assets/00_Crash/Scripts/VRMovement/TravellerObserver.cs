using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.VRRigSpace
{
    public class TravellerObserver : MonoBehaviour
    {
        cameraTraveller traveller = null;

        Transform myself;

        bool currentAmTravellerPos = false;
        bool prevAmTravellerPos = false;

        [SerializeField] UnityEvent DoOnAmTravellerPos;

        [SerializeField] UnityEvent DoOnAmNotTravellerPos;

        private void OnEnable()
        {
            traveller = GameObject.FindWithTag("Traveller").GetComponent<cameraTraveller>();
            myself = transform;
        }

        private void Start()
        {
            currentAmTravellerPos = AmCurrentTravellerPosition();

            prevAmTravellerPos = currentAmTravellerPos;

            if (currentAmTravellerPos)
            {
                DoOnAmTravellerPos.Invoke();
                print("DoOnAmTravellerPos");
            }

            else
            {
                DoOnAmNotTravellerPos.Invoke();
                print("DoOnAmNotTravellerPos");
            }
        }

        private void Update()
        {
            currentAmTravellerPos = AmCurrentTravellerPosition();

            if (prevAmTravellerPos != currentAmTravellerPos)
            {
                if (currentAmTravellerPos)
                {
                    DoOnAmTravellerPos.Invoke();
                    print("DoOnAmTravellerPos");
                }

                else
                {
                    DoOnAmNotTravellerPos.Invoke();
                    print("DoOnAmNotTravellerPos");
                }

            }


            prevAmTravellerPos = currentAmTravellerPos;

        }

        public bool AmCurrentTravellerPosition()
        {
            // if (traveller == null)
            // {
            //     Debug.LogError("TravellerObserver: No Traveller in Scene");
            //     return false;
            // }

            return (traveller.GetCurrentXRRigTransform() == myself);
        }



    }

}

