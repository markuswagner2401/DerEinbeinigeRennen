using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TimelineSpace
{

    public class TravellerControlByTimeline : MonoBehaviour
    {
        [SerializeField] TravellerStates travellerControlForRacer;

        [SerializeField] TravellerStates travellerControlForZuschauer;

        [SerializeField] TravellerStates travellerControlForInspizient;



        [System.Serializable]
        struct TravellerStates
        {
            public int transitionPointIndexCurrentClip;
            public int transitionPointIndexPrevClip;
            public string transitionPointNameCurrentClip;
            public string transitionPointNamePrevClip;
            public float currentT;
            public bool transitioning;
            public bool transitionComplete;

        }

        [SerializeField] bool roleAware = false;

        [SerializeField] Role role = Role.None;


        [SerializeField] int transPointICurrClip;

        [SerializeField] string transPointNameCurrClip;

        [SerializeField] int transPointIPrevClip;

        [SerializeField] string transPointNamePrevClip;
        [SerializeField] float currentT;

        [SerializeField] bool transitioning = false;
        [SerializeField] bool transitionComplete;



        private void Start()
        {

        }



        // Set By PlayableBehaviour (TravellerControlMixer)





        public void SetTransitionPointIndex(int index, Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    travellerControlForRacer.transitionPointIndexCurrentClip = index;
                    break;

                case Role.Zuschauer:
                    travellerControlForZuschauer.transitionPointIndexCurrentClip = index;
                    break;

                case Role.Inspizient:
                    travellerControlForZuschauer.transitionPointIndexCurrentClip = index;
                    break;

                case Role.None:
                    travellerControlForRacer.transitionPointIndexCurrentClip = index;
                    travellerControlForZuschauer.transitionPointIndexCurrentClip = index;
                    break;

                default:
                    break;
            }
        }



        public void SetTransPointName(string name, Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    travellerControlForRacer.transitionPointNameCurrentClip = name;
                    break;

                case Role.Zuschauer:
                    travellerControlForZuschauer.transitionPointNameCurrentClip = name;
                    break;

                case Role.Inspizient:
                    travellerControlForInspizient.transitionPointNameCurrentClip = name;
                    break;

                case Role.None:
                    travellerControlForRacer.transitionPointNameCurrentClip = name;
                    travellerControlForZuschauer.transitionPointNameCurrentClip = name;
                    break;

                default:
                    break;
            }
        }



        public void SetPreviousPointIndex(int index, Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    travellerControlForRacer.transitionPointIndexPrevClip = index;
                    break;

                case Role.Zuschauer:
                    travellerControlForZuschauer.transitionPointIndexPrevClip = index;
                    break;

                case Role.Inspizient:
                    travellerControlForInspizient.transitionPointIndexPrevClip = index;
                    break;

                case Role.None:
                    travellerControlForRacer.transitionPointIndexPrevClip = index;
                    travellerControlForZuschauer.transitionPointIndexPrevClip = index;
                    break;

                default:
                    break;
            }
        }



        public void SetTransPointNamePrev(string name, Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    travellerControlForRacer.transitionPointNamePrevClip = name;
                    break;

                case Role.Zuschauer:
                    travellerControlForZuschauer.transitionPointNamePrevClip = name;
                    break;

                case Role.Inspizient:
                    travellerControlForInspizient.transitionPointNamePrevClip = name;
                    break;

                case Role.None:
                    travellerControlForRacer.transitionPointNamePrevClip = name;
                    travellerControlForZuschauer.transitionPointNamePrevClip = name;
                    break;

                default:
                    break;
            }
        }



        public void SetCurrentT(float t, Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    travellerControlForRacer.currentT = t;
                    break;

                case Role.Zuschauer:
                    travellerControlForZuschauer.currentT = t;
                    break;

                case Role.Inspizient:
                    travellerControlForInspizient.currentT = t;
                    break;

                case Role.None:
                    travellerControlForRacer.currentT = t;
                    travellerControlForZuschauer.currentT = t;
                    break;

                default:
                    break;
            }
        }



        public void SetTransitionComplete(bool value, Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    travellerControlForRacer.transitionComplete = value;
                    break;

                case Role.Zuschauer:
                    travellerControlForZuschauer.transitionComplete = value;
                    break;

                case Role.Inspizient:
                    travellerControlForZuschauer.transitionComplete = value;
                    break;

                case Role.None:
                    travellerControlForRacer.transitionComplete = value;
                    travellerControlForZuschauer.transitionComplete = value;
                    break;

                default:
                    break;
            }
        }



        public void SetTransitioning(bool value, Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    travellerControlForRacer.transitioning = value;
                    break;

                case Role.Zuschauer:
                    travellerControlForZuschauer.transitioning = value;
                    break;

                case Role.Inspizient:
                    travellerControlForInspizient.transitioning = value;
                    break;

                case Role.None:
                    travellerControlForRacer.transitioning = value;
                    travellerControlForZuschauer.transitioning = value;
                    break;

                default:
                    break;
            }
        }

        // Get By Traveller



        public int GetTransitionPointIndexCurrClip(Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    return travellerControlForRacer.transitionPointIndexCurrentClip;


                case Role.Zuschauer:
                    return travellerControlForZuschauer.transitionPointIndexCurrentClip;

                case Role.Inspizient:
                    return travellerControlForInspizient.transitionPointIndexCurrentClip;

                case Role.None:
                    return travellerControlForZuschauer.transitionPointIndexCurrentClip;



                default:
                    return travellerControlForZuschauer.transitionPointIndexCurrentClip;
            }

            //return transPointICurrClip;
        }



        public string GetTransPointNameCurrClip(Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    return travellerControlForRacer.transitionPointNameCurrentClip;


                case Role.Zuschauer:
                    return travellerControlForZuschauer.transitionPointNameCurrentClip;

                case Role.Inspizient:
                    return travellerControlForInspizient.transitionPointNameCurrentClip;

                case Role.None:
                    return travellerControlForZuschauer.transitionPointNameCurrentClip;

                default:
                    return travellerControlForZuschauer.transitionPointNameCurrentClip;
            }
        }





        public int GetTransitionPointIndexPrevClip(Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    return travellerControlForRacer.transitionPointIndexPrevClip;


                case Role.Zuschauer:
                    return travellerControlForZuschauer.transitionPointIndexPrevClip;

                case Role.Inspizient:
                    return travellerControlForInspizient.transitionPointIndexPrevClip;

                case Role.None:
                    return travellerControlForZuschauer.transitionPointIndexPrevClip;

                default:
                    return travellerControlForZuschauer.transitionPointIndexPrevClip;
            }
            //return transPointIPrevClip;
        }



        public string GetTransPointNamePrevClip(Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    return travellerControlForRacer.transitionPointNamePrevClip;


                case Role.Zuschauer:
                    return travellerControlForZuschauer.transitionPointNamePrevClip;

                case Role.Inspizient:
                    return travellerControlForInspizient.transitionPointNamePrevClip;

                case Role.None:
                    return travellerControlForZuschauer.transitionPointNamePrevClip;

                default:
                    return travellerControlForZuschauer.transitionPointNamePrevClip;
            }
        }



        public float GetCurrentT(Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    return travellerControlForRacer.currentT;


                case Role.Zuschauer:
                    return travellerControlForZuschauer.currentT;

                case Role.Inspizient:
                    return travellerControlForInspizient.currentT;

                case Role.None:
                    return travellerControlForZuschauer.currentT;

                default:
                    return travellerControlForZuschauer.currentT;
            }
        }


        public Boolean GetTransitioning(Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    return travellerControlForRacer.transitioning;


                case Role.Zuschauer:
                    return travellerControlForZuschauer.transitioning;

                case Role.Inspizient:
                    return travellerControlForInspizient.transitioning;

                case Role.None:
                    return travellerControlForZuschauer.transitioning;

                default:
                    return travellerControlForZuschauer.transitioning;
            }
        }



        public Boolean GetTransitionComplete(Role role)
        {
            switch (role)
            {
                case Role.Rennfahrer:
                    return travellerControlForRacer.transitionComplete;


                case Role.Zuschauer:
                    return travellerControlForZuschauer.transitionComplete;

                case Role.Inspizient:
                    return travellerControlForZuschauer.transitionComplete;

                case Role.None:
                    return travellerControlForZuschauer.transitionComplete;


                default:
                    return travellerControlForZuschauer.transitionComplete;

            }
        }
    }

}
