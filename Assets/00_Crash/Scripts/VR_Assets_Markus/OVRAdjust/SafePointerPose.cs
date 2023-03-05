using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.OVRRigSpace
{
    public class SafePointerPose : MonoBehaviour
    {
        [SerializeField] OVRHand hand;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = hand.PointerPose.localPosition;
        }
    }

}

