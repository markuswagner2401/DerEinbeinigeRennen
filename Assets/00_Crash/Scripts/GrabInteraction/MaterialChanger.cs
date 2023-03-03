using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.VRRigSpace;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.MaterialControl
{

    public class MaterialChanger : MonoBehaviour
    {


        [SerializeField]
        Material materialState1 = null;

        [SerializeField]
        Material materialState2 = null;

        [SerializeField]
        MeshRenderer meshRenderer = null;

        [SerializeField]
        AttachEntkoppler attachEntkoppler = null;

        private void Awake()
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        private void Update()
        {
            if (attachEntkoppler == null) return;
            if (meshRenderer == null) return;

            if (attachEntkoppler.GetEntkopplung() == true)
            {
                meshRenderer.material = materialState1;
            }

            else
            {
                meshRenderer.material = materialState2;
            }
        }



    }


}
