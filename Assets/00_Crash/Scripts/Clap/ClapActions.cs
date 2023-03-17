using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.ClapSpace
{
    public class ClapActions : MonoBehaviour
    {

        [SerializeField] ClapHandler clapHandler = null;


        [SerializeField] MiddleRayHandler middleRayHandler = null;

        [Tooltip("as alternative to middle ray Handler")]
        [SerializeField] SimpleHandsAverageForward simpleHandsAverageForward = null;
        [SerializeField] bool raycast = true;
        [SerializeField] float raycastMaxDist = 10f;
        [Tooltip("DotProduct -> 1 for sameDirection, 0 for perpendicular,, -1 for opposite directions")]
        [SerializeField] float raycastDeviationThreshold = 0.9f;

        [SerializeField] string raycastTag = "ShootButton";

        [SerializeField] bool shootProjectiles = true;
        [SerializeField] float shootFactor = 10f;

        [SerializeField] bool activateButtonWithClap = true;

        [SerializeField][Tooltip("otherwise on Clap detected by collider")] bool invokeEventOnheared = true;
        [SerializeField] UnityEventFloat clapEventsA = null;

        public delegate void OnClapAction(float strength);

        public OnClapAction onClapAction;


        [SerializeField] bool clapEventsEnabled = true;

        public delegate void OnEmitClapSignal(float strength, int listenerIndex);
        public OnEmitClapSignal onEmitClapSignal;

        [SerializeField] int[] clapSignalListeners;



        Ray ray;
        GameObject observedButton = null;
        GameObject previousObservedButton = null;
        bool isHitting = false;
        Vector3 lastHitPoint = new Vector3();
        Ray lastHitRay;



        void Start()
        {

            if (clapHandler == null)
            {
                clapHandler = GetComponent<ClapHandler>();

            }



            // if (middleRayHandler == null)
            // {
            //     middleRayHandler = GetComponentInChildren<MiddleRayHandler>();
            // }





            clapHandler.doOnColliderClap += ShootProjectile;

            clapHandler.doOnColliderClap += ActivateButton;

            // clapHandler.doOnClapHeared += ShootProjectile;
            //clapHandler.doOnClapHeared += ActivateButton;

            if (invokeEventOnheared)
            {
                //clapHandler.doOnClapHeared += InvokeClapEventsA;
            }

            else
            {
                clapHandler.doOnColliderClap += InvokeClapEventsA;

            }


            

            onEmitClapSignal += OnEmitClapSignalPlaceholder;

            onClapAction += PlaceholderOnClapAction;

        }

        private void OnDisable() 
        {
            onEmitClapSignal -= OnEmitClapSignalPlaceholder;

            onClapAction -= PlaceholderOnClapAction;

            clapHandler.doOnColliderClap -= ShootProjectile;

            clapHandler.doOnColliderClap -= ActivateButton;

            if (invokeEventOnheared)
            {
                //clapHandler.doOnClapHeared += InvokeClapEventsA;
            }

            else
            {
                clapHandler.doOnColliderClap -= InvokeClapEventsA;

            }
            
        }



        private void UpdateOnClapDelagates()
        {

        }

        private void Update()
        {
            if (middleRayHandler != null)
            {
                ray = middleRayHandler.GetMiddleRay();
            }

            else if (simpleHandsAverageForward != null)
            {
                ray = simpleHandsAverageForward.GetRay();
            }

            else
            {
                raycast = false;
                shootProjectiles = false;
            }


            if (raycast)
            {

                bool hasHit = middleRayHandler.GetHasHit();
                RaycastHit hit = middleRayHandler.GetHit();



                if (hasHit && hit.collider.gameObject.tag == raycastTag)
                {

                    lastHitPoint = hit.point;
                    lastHitRay = ray;
                    observedButton = hit.collider.gameObject;

                    if (ReferenceEquals(observedButton, previousObservedButton)) return;

                    observedButton.GetComponent<ClapButtonListener>().HoverEnter(true);

                    if (!ReferenceEquals(previousObservedButton, null))
                    {
                        previousObservedButton.GetComponent<ClapButtonListener>().HoverEnter(false);
                    }


                    previousObservedButton = observedButton;

                }

                else
                {
                    if (!ReferenceEquals(observedButton, null))
                    {
                        if (CalculateDeviation(lastHitRay, ray) > raycastDeviationThreshold) // threshold not broken
                        {
                            middleRayHandler.OverwriteRayEndPoint(true, lastHitPoint);
                            return;
                        }

                    }


                    middleRayHandler.OverwriteRayEndPoint(false, Vector3.zero);

                    if (ReferenceEquals(observedButton, null) && ReferenceEquals(previousObservedButton, null)) return;
                    observedButton.GetComponent<ClapButtonListener>().HoverEnter(false);
                    previousObservedButton.GetComponent<ClapButtonListener>().HoverEnter(false);
                    observedButton = null;
                    previousObservedButton = null;

                }

            }

        }

        public void SetClapEvents(UnityEventFloat unityEventFloat)
        {
            clapEventsA = unityEventFloat;
        }

        // public void EnableEmitClapSignals(bool value)
        // {
        //     if(value)
        //     {
        //         clapHandler.doOnColliderClap += EmitClapSignal;
        //     }

        //     else
        //     {
        //         clapHandler.doOnColliderClap -= EmitClapSignal;
        //     }
        // }

        public void SetClapSignalListener(int[] indexes)
        {
            clapSignalListeners = indexes;
        }

        void EmitClapSignal(float strength)
        {
            foreach (var listener in clapSignalListeners)
            {
                onEmitClapSignal.Invoke(strength, listener);
            }

        }





        private float CalculateDeviation(Ray lastHitRay, Ray currentRay)
        {
            float deviation = Vector3.Dot(lastHitRay.direction, currentRay.direction);
            return deviation;
        }

        void ShootProjectile(float strength)
        {
            if (!shootProjectiles) return;

            if (ProjectilePool.instance == null) return;



            GameObject projectile = ProjectilePool.Take(ray.origin, Quaternion.LookRotation(ray.direction));
            Rigidbody[] rigidbodies = projectile.GetComponent<InteractProjectile>().GetRigidbodies();
            foreach (var rb in rigidbodies)
            {
                rb.AddForce(ray.direction * strength * shootFactor, ForceMode.Impulse);
            }

        }

        void ActivateButton(float strength)
        {
            if (!activateButtonWithClap) return;
            if (ReferenceEquals(observedButton, null)) return;
            observedButton.GetComponent<ClapButtonListener>().ActivateOnClap(strength);
        }

        void InvokeClapEventsA(float strength)
        {

            if (!clapEventsEnabled) return;
            clapEventsA.Invoke(strength);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(ray);
        }


        /// IClapActions

        // public void IActivateShoot(bool value)
        // {
        //     shootProjectiles = value;

        // }

        // public void IShowRay(bool Value)
        // {
        //     return;
        // }

        // public void IButtonClapActivation(bool value)
        // {
        //     activateButtonWithClap = value;

        // }

        // public void IResetObservedButton()
        // {
        //     previousObservedButton = null;
        //     observedButton = null;
        // }


        // ///

        private void OnEmitClapSignalPlaceholder(float strength, int listenerIndex)
        {

        }

        private void PlaceholderOnClapAction(float strength)
        {
        }
    }


}


