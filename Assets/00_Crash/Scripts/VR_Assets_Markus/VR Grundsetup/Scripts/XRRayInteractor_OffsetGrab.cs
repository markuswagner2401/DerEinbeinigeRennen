using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.VRRigSpace
{

    public class XRRayInteractor_OffsetGrab : XRRayInteractor
    {
        // Start is called before the first frame update


        // Update is called once per frame
        void Update()
        {

        }

        public void ResetOriginalAttachTransform()
        {
            rayOriginTransform.localPosition = Vector3.zero;
            rayOriginTransform.localRotation = Quaternion.identity;

        }
    }

}
