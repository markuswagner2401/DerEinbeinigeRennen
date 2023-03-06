using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using Photon.Pun;
using System;
using ObliqueSenastions.VRRigSpace;

namespace ObliqueSenastions.PunNetworking
{


    public class NetworkPlayer : MonoBehaviourPunCallbacks
    {
        [Tooltip("Set to same as XR Rig")]
        [SerializeField] LayerMask mineLayer;
        [SerializeField] Transform origin;
        [SerializeField] Transform headTarget;

        [SerializeField] string pathToHeadSource = "InteractionRigOVR_TravellerUI/SafeAnchors/SafeHead";
        [SerializeField] Transform leftHandTarget;

        [SerializeField] string pathToLeftHandSource = "InteractionRigOVR_TravellerUI/SafeAnchors/SafeLeftHand";
        [SerializeField] Transform rightHandTarget;

        [SerializeField] string pathToRightHandSource = "InteractionRigOVR_TravellerUI/SafeAnchors/SafeRightHand";

        [SerializeField] bool syncHandAnimation = false;
        [SerializeField] Animator npLeftHandAnimator;
        [SerializeField] Animator npRightHandAnimator;

        [SerializeField] bool manualUpdate = true;

        [SerializeField] bool listenToStageMaster = true;

        [SerializeField] CapsuleCollider capsuleCollider = null;

        [SerializeField] float additionalColliderHeight = 0.2f;

        [SerializeField] MeshRenderer[] meshsToSwitchOffIfIsMine;

        [SerializeField] SkinnedMeshRenderer[] smrsToSwitchOffIfIsMine;

        [SerializeField] bool destroyOnSceneEnd = false;




        public delegate void OnPlayerMappingUpdated();
        public OnPlayerMappingUpdated onPlayerMappingUpdated;


        [Tooltip("Gets Set by NetworkPlayerSpawner")]
        [SerializeField] int transitionPointIndexForBinding = -1; // Gets used if Binding Type is not traveller 



        cameraTraveller currentCameraTraveller = null;


        //[SerializeField] float smoothing = 0.1f;


        Transform originRig;
        Transform headRig;
        Transform leftHandRig;
        Transform rightHandRig;

        Transform currentXROrigin = null;


        private void Awake()
        {
            if (manualUpdate)
            {
                onPlayerMappingUpdated += PlaceholderOnMappingReady;
            }

            if (!destroyOnSceneEnd)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }





        void Start()
        {






            if (capsuleCollider == null)
            {
                capsuleCollider = GetComponent<CapsuleCollider>();

            }






        }



        private void Update()
        {


            // if (headRig == null) // Happens on change scene
            // {
            //     if ((PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer.ActorNumber > 0 && photonView.IsMine))
            //     {
            //         SetupAvatar();
            //         return;
            //     }
            // }
        }



        public void Destroy()
        {
            Destroy(this.gameObject);
        }





        void LateUpdate()
        {
            if (manualUpdate) return;
            ManualUpdate();
        }



        public void ManualUpdate()
        {
            if (originRig == null) // Happens on change scene
            {
                if ((PhotonNetwork.IsConnected && PhotonNetwork.InRoom && photonView.IsMine))
                {
                    SetupAvatar();
                    return;
                }
            }

            // Debug.Log("NetworkPlayer: ManualUpdate, invoked by traveller");
            if (photonView.IsMine)
            {

                // rightHand.gameObject.SetActive(false);
                // leftHand.gameObject.SetActive(false);
                // head.gameObject.SetActive(false);


                MapPositionGlobal(origin, originRig);
                MapPositionLocal(headTarget, headRig);
                MapPositionLocal(leftHandTarget, leftHandRig);
                MapPositionLocal(rightHandTarget, rightHandRig);

                if (syncHandAnimation)
                {
                    UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), npLeftHandAnimator);
                    UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), npRightHandAnimator);
                }





            }

            //        CapsuleFollowRig();

            //        onPlayerMappingUpdated.Invoke();
        }

        public void SetTransitionPointIndex(int index) // Gets Set By NetworkPlayerSpawner
        {
            transitionPointIndexForBinding = index;
        }

        private void SetupAvatar()
        {
            if (!photonView.IsMine) return;



            // Setup Traveller (Transition Points)


            GameObject traveller = GameObject.FindWithTag("Traveller");
            if (traveller == null) return;



            currentCameraTraveller = traveller.GetComponent<cameraTraveller>();
            // currentCameraTraveller.SetNetworkPlayerIndex(MultiplayerConnector.instance.GetNetworkPlayerIndex()); Gets Set By Spawner

            if (manualUpdate)
            {
                currentCameraTraveller.onTravellerUpdateReady += ManualUpdate;
            }

            Transform xROrigin = traveller.transform;

            // Setup Mapping Sources



            if (transitionPointIndexForBinding == -1)
            {
                originRig = xROrigin;
            }

            else
            {
                originRig = currentCameraTraveller.GetXRRigTransform(transitionPointIndexForBinding);
                if (originRig == null)
                {
                    originRig = xROrigin;
                }
            }

            headRig = xROrigin.transform.Find(pathToHeadSource);
            leftHandRig = xROrigin.transform.Find(pathToLeftHandSource);
            rightHandRig = xROrigin.transform.Find(pathToRightHandSource);




            // Manage Visibility
            // Visibility on Local Rig // todo: handle with seperate component

            // GameObject[] hideInNetwork = GameObject.FindGameObjectsWithTag("HideInNetwork");
            // foreach (var item in hideInNetwork)
            // {
            //     if (item.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skm))
            //     {
            //         skm.enabled = false;
            //     }

            //     if (item.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            //     {
            //         renderer.enabled = false;
            //     }
            // }

            // Visibility on Network Avatar

            foreach (var item in meshsToSwitchOffIfIsMine)
            {
                item.enabled = false;
            }

            foreach (var item in smrsToSwitchOffIfIsMine)
            {
                item.enabled = false;
            }

            // Manage Collisions



            this.gameObject.layer = Mathf.RoundToInt(Mathf.Log(mineLayer.value, 2)); // converting a bit mask to the int

            //GetComponent<Collider>().enabled = false;

            currentXROrigin = xROrigin; // set current XROrigin in order of the CapsuleFollowRig to work;

            // Manage Stage Manager Events for the network player

            if (listenToStageMaster)
            {
                GetComponent<StageMasterListener>().SetupStageMasterListener();
            }
        }


        void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                handAnimator.SetFloat("Trigger", triggerValue);
            }
            else
            {
                handAnimator.SetFloat("Trigger", 0);
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                handAnimator.SetFloat("Grip", gripValue);
            }
            else
            {
                handAnimator.SetFloat("Grip", 0);
            }
        }

        void MapPositionGlobal(Transform target, Transform source)
        {
            // target.position = Vector3.Lerp(target.position, rigTransform.position, smoothing) ;
            // target.rotation = Quaternion.Lerp(target.rotation ,rigTransform.rotation, smoothing);

            target.position = source.position;
            target.rotation = source.rotation;
        }

        void MapPositionLocal(Transform target, Transform rigTransform)
        {
            target.localPosition = rigTransform.localPosition;
            target.localRotation = rigTransform.localRotation;
        }

        private void PlaceholderOnMappingReady()
        {
        }

        void CapsuleFollowRig()
        {
            float height = headRig.transform.position.y + additionalColliderHeight;  // to do: different way to calculate height in order to function not only locally
            Vector3 capsuleLocalPosition = originRig.position;
            capsuleLocalPosition += originRig.up * (height / 2f);

            capsuleCollider.height = height;
            capsuleCollider.center = transform.InverseTransformPoint(capsuleLocalPosition);
            //Vector3 capsuleCenter = transform.InverseTransformPoint(currentXROrigin.Camera.transform.position);
            // capsule.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);



            //capsuleCollider.center = new Vector3(originRig.position.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
        }

        private void OnDestroy()
        {

            //        currentCameraTraveller.onTravellerUpdateReady -= ManualUpdate;
        }
    }

}

// using UnityEngine.XR.Interaction.Toolkit;
//using Unity.XR.CoreUtils;
