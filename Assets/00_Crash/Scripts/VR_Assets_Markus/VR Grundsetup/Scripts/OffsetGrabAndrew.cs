using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.VRRigSpace
{

    public class OffsetGrabAndrew : XRGrabInteractable
    {
        private Vector3 interactorPosition = Vector3.zero;
        private Quaternion interactorRotation = Quaternion.identity;
        [SerializeField] bool rayInteractable = true;
        [SerializeField] bool velocityTrackSetParentTrick = false;



        protected override void OnSelectEntered(XRBaseInteractor interactor)
        {
            if (velocityTrackSetParentTrick)
            {
                SetParentToXRRig();
            }

            base.OnSelectEntered(interactor);
            StoreInteractor(interactor);
            MatchAttachmentPoints(interactor);
            if (rayInteractable)
            {
                SetRayLineRenderer(interactor);
            }

        }



        private void StoreInteractor(XRBaseInteractor interactor)
        {
            interactorPosition = interactor.attachTransform.localPosition;
            interactorRotation = interactor.attachTransform.localRotation;


        }

        private void MatchAttachmentPoints(XRBaseInteractor interactor)
        {

            bool hasAttach = attachTransform != null;
            interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
            interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
        }

        protected override void OnSelectExited(XRBaseInteractor interactor)
        {
            SetParentToWorld();

            base.OnSelectExited(interactor);
            ResetAttachmentPoint(interactor);
            ClearInteractor(interactor);

            if (rayInteractable)
            {
                ResetRayVisual(interactor);
            }


        }



        private void ResetAttachmentPoint(XRBaseInteractor interactor)
        {
            interactor.attachTransform.localPosition = interactorPosition;
            interactor.attachTransform.localRotation = interactorRotation;
        }

        private void ClearInteractor(XRBaseInteractor interactor)
        {
            interactorPosition = Vector3.zero;
            interactorRotation = Quaternion.identity;
        }

        // Ray Ineractor line visual

        // protected override void OnHoverEnter(XRBaseInteractor interactor)
        // {
        //     base.OnHoverEnter(interactor);
        //     OffsetGrabLineVisual lineVisual = GetLineVisual(interactor);
        //     if (lineVisual == null) return;
        //     lineVisual.SetHover(true);
        //     print("set hover");
        // }

        // protected override void OnHoverExit(XRBaseInteractor interactor)
        // {
        //     base.OnHoverExit(interactor);
        //     OffsetGrabLineVisual lineVisual = GetLineVisual(interactor);
        //     if (lineVisual == null) return;
        //     lineVisual.SetHover(false);
        //     print("set hover false");

        // }

        private void SetRayLineRenderer(XRBaseInteractor interactor)
        {
            OffsetGrabLineVisual lineVisual = GetLineVisual(interactor);
            if (lineVisual == null) return;
            lineVisual.SetTarget(gameObject);
        }



        private void ResetRayVisual(XRBaseInteractor interactor)
        {
            OffsetGrabLineVisual lineVisual = GetLineVisual(interactor);
            if (lineVisual == null) return;
            lineVisual.ResetTarget();
        }

        private static OffsetGrabLineVisual GetLineVisual(XRBaseInteractor interactor)
        {
            return interactor.transform.GetComponentInChildren<OffsetGrabLineVisual>();
        }

        // Velocity Track Trick

        public void SetParentToXRRig()
        {
            transform.SetParent(selectingInteractor.transform);
        }

        public void SetParentToWorld()
        {
            transform.SetParent(null);
        }
    }

}
