using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ObliqueSenastions.AnimatorSpace;

namespace ObliqueSenastions.PunNetworking
{

    public class VelocityTrackedAnimationControlSync : MonoBehaviourPunCallbacks, IPunObservable
    {

        [SerializeField] Role targetRole;

        [SerializeField] Animator animator = null;

        [SerializeField] string parameterRef;

        float sourceValue;


        [SerializeField] VelocityTrackedAnimationControl velocityTrackedAnimationControl = null;



        public override void OnEnable()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            base.OnEnable();
        }

        void Start()
        {
            if (PhotonNetwork.InRoom)
            {
                OnJoinedRoom();
            }
        }

        public override void OnJoinedRoom()
        {
            SetupAnimationControl();
        }




        // Update is called once per frame


        void SetupAnimationControl()
        {
            Role currentRole = MultiplayerConnector.instance.GetRole();
            if (currentRole == targetRole)
            {
                velocityTrackedAnimationControl.enabled = true;
                photonView.RequestOwnership();
            }

            else
            {
                velocityTrackedAnimationControl.SetAnimationSpeed(0f);
                velocityTrackedAnimationControl.enabled = false;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                sourceValue = animator.GetFloat(parameterRef);
                stream.SendNext(sourceValue);
            }

            else
            {
                float targetValue = (float)stream.ReceiveNext();
                animator.SetFloat(parameterRef, targetValue);
            }
        }
    }

}
