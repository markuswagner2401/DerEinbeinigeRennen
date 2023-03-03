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
        Transform currentTarget;

        private void Start()
        {
            if (aimIK == null)
            {
                aimIK = GetComponent<AimIK>();
            }
        }


        public void SetAimIKTarget(Transform target)
        {
            currentTarget = target;

        }

        private void Update()
        {
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
