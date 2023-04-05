using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace ObliqueSenastions.FinalIKControl
{

    public class IKTargetChanger : MonoBehaviour
    {
        [SerializeField] AimIK aimIK = null;

        [SerializeField] float smoothing = 0.1f;

        [SerializeField] Transform aimIKTarget = null;

        [SerializeField] bool setToMainCamera = false;
        Transform currentTarget;

        bool maincamSetup;

        private void Start()
        {
            if (aimIK == null)
            {
                aimIK = GetComponent<AimIK>();
            }

            if(setToMainCamera)
            {
                SetupMainCamTarget();
            }
        }

        private void SetupMainCamTarget()
        {
            if(maincamSetup) return;
            GameObject mainCameraGo = GameObject.FindWithTag("MainCamera");
            if (mainCameraGo != null)
            {
                maincamSetup = true;
                currentTarget = mainCameraGo.transform;
            }
            else
            {
                Debug.LogError("IKTargetChanger: Didnt find Maincam");
                maincamSetup = false;
            }
        }

        public void SetAimIKTarget(Transform target)
        {
            
            currentTarget = target;

        }

        private void Update()
        {

            if(setToMainCamera)
            {
                SetupMainCamTarget();
            }
            
            if (aimIKTarget != null && currentTarget != null)
            {
                aimIKTarget.position = Vector3.Lerp(aimIKTarget.position, currentTarget.position, smoothing);
            }
        }

        private void LateUpdate()
        {
            UpdateAimIKTarget();
        }

        private void UpdateAimIKTarget()
        {
            if (aimIK == null) return;
            aimIK.solver.IKPosition = aimIKTarget.position;

        }
    }


}
