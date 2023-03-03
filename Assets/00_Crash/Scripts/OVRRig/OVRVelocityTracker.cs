using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.OVRRigSpace
{
    public class OVRVelocityTracker : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            OVRInput.Update();
            
            OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LHand);
        }
    }

}

