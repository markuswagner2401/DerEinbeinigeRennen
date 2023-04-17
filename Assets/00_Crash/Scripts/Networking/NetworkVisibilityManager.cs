using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;
using System;
using Photon.Realtime;

namespace ObliqueSenastions.PunNetworking
{

    public class NetworkVisibilityManager : MonoBehaviourPunCallbacks
    {

        enum Deactivation
        {
            gameObject,
            renderer,

            rendererInChildren,
            collider,

            colliderInChildren
        }
        [SerializeField] VisibilitySubject[] visibilitySubjects;
        [System.Serializable]
        struct VisibilitySubject
        {
            public string note;

            public bool roleAware;
            public Role role;
            public GameObject[] objects;

            public Deactivation deactivation;
        }

        bool collidersReady = false;



        private void Start()
        {
            if (PhotonNetwork.InRoom)
            {
                MyOnJoinedRoom();
            }
            MultiplayerConnector.instance.my_OnJoinedRoom += MyOnJoinedRoom;
            MultiplayerConnector.instance.onPlayerCountChanged += SwichOffObjects;
        }



        private void OnDestroy()
        {
            MultiplayerConnector.instance.my_OnJoinedRoom -= MyOnJoinedRoom;
            MultiplayerConnector.instance.onPlayerCountChanged -= SwichOffObjects;
        }

        void SwichOffObjects()
        {
            Debug.Log("NetworkVisibilityManager: Swich off objects onPlyerCounterChange");
            ManageVisibilities(false);
        }

        private void MyOnJoinedRoom()
        {
            //StartCoroutine(ManageVisibilitiesR(false));
            Debug.Log("NetworkVisibilityManager: MyOnJoinedRoom: ManageVisibilities");
            ManageVisibilities(false);

        }

        private bool HandsReady()
        {


            InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (leftHand.isValid && rightHand.isValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        // public override void OnJoinedRoom()
        // {
        //     StartCoroutine(ManageVisibilitiesR(false));
        //     //ManageVisibilities(false);
        // }

        public override void OnLeftRoom()
        {
            ManageVisibilities(true);
            collidersReady = false;
        }



        private void ManageVisibilities(bool state)
        {
            
            foreach (var item in visibilitySubjects)
            {
                if (item.roleAware)
                {
                    if (MultiplayerConnector.instance.GetNumberOfPlayersOfRole(item.role) <= 0)
                    {
                        continue;
                    }


                }

                switch (item.deactivation)
                {
                    case Deactivation.gameObject:
                        foreach (var obj in item.objects)
                        {
                            if (obj != null) obj.SetActive(state);
                        }
                        break;

                    case Deactivation.renderer:
                        foreach (var obj in item.objects)
                        {
                            if (obj.TryGetComponent<Renderer>(out Renderer renderer))
                            {
                                renderer.enabled = state;
                            }
                        }
                        break;

                    case Deactivation.collider:
                        foreach (var obj in item.objects)
                        {
                            if (obj.TryGetComponent<Collider>(out Collider collider))
                            {
                                collider.enabled = state;
                            }
                        }
                        break;

                    case Deactivation.rendererInChildren:
                        foreach (var obj in item.objects)
                        {
                            if (obj != null)
                            {
                                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

                                foreach (var renderer in renderers)
                                {
                                    renderer.enabled = state;
                                }

                            }

                        }
                        break;

                    case Deactivation.colliderInChildren:
                        foreach (var obj in item.objects)
                        {
                            if (obj != null)
                            {
                                Collider[] colliders = obj.GetComponentsInChildren<Collider>();
                                foreach (var collider in colliders)
                                {
                                    collider.enabled = state;
                                }

                            }

                        }
                        break;


                    default:
                        break;
                }

            }

            collidersReady = true;
        }


        // IEnumerator ManageVisibilitiesR(bool state)
        // {
        //     bool handsReady = false;
        //     while (!handsReady)
        //     {
        //         handsReady = HandsReady();

        //         foreach (var item in visibilitySubjects)
        //         {
        //             if (item.deactivation == Deactivation.gameObject)
        //             {
        //                 foreach (var obj in item.objects)
        //                 {
        //                     if (obj == null)
        //                     {
        //                         Debug.LogError("VisibilityManager: " + obj.name + " not found, to manage Visibility. Visibility Not ready");

        //                     }
        //                     else
        //                     {
        //                         obj.SetActive(state);
        //                     }
        //                     yield return null;
        //                 }
        //             }

        //             else if (item.deactivation == Deactivation.renderer)
        //             {
        //                 foreach (var obj in item.objects)
        //                 {
        //                     if (obj == null)
        //                     {
        //                         Debug.LogError("VisibilityManager: " + obj.name + " not found, to manage Visibility. Visibility Not ready");


        //                     }

        //                     else
        //                     {
        //                         if (obj.TryGetComponent<Renderer>(out Renderer renderer))
        //                         {
        //                             renderer.enabled = state;
        //                         }

        //                         else
        //                         {
        //                             Debug.LogError("VisibilityManager: Renderer in " + obj.name + " not found, to manage Visibility. Visibility Not ready");

        //                         }
        //                     }

        //                     yield return null;
        //                 }

        //             }

        //             else if (item.deactivation == Deactivation.collider)
        //             {
        //                 foreach (var obj in item.objects)
        //                 {
        //                     if (obj.TryGetComponent<Collider>(out Collider collider))
        //                     {
        //                         collider.enabled = state;
        //                     }
        //                     yield return null;
        //                 }
        //             }

        //             else if (item.deactivation == Deactivation.rendererInChildren)
        //             {
        //                 foreach (var obj in item.objects)
        //                 {
        //                     if (obj != null)
        //                     {
        //                         Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        //                         foreach (var renderer in renderers)
        //                         {
        //                             renderer.enabled = state;
        //                         }

        //                     }
        //                     yield return null;
        //                 }

        //             }

        //             else if (item.deactivation == Deactivation.colliderInChildren)
        //             {
        //                 foreach (var obj in item.objects)
        //                 {
        //                     if (obj != null)
        //                     {
        //                         Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        //                         foreach (var collider in colliders)
        //                         {
        //                             collider.enabled = state;
        //                         }

        //                     }

        //                 }
        //                 yield return null;

        //             }
        //             yield return null;
        //         }
        //         collidersReady = true;

        //     }
        // }
        public bool GetVisibiliesReady()
        {
            return collidersReady;
        }
    }

}
