using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace ObliqueSenastions.AnimatorSpace
{

    public class AnimatorControllerDebug : MonoBehaviour
    {
        Animator animator;
        public XRNode inputSource;
        InputDevice device;
        Vector2 inputAxis;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            device = InputDevices.GetDeviceAtXRNode(inputSource);

        }

        // Update is called once per frame
        void Update()
        {
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
            print("InputAxis = " + inputAxis);

            animator.SetBool("isMoving", inputAxis.magnitude > 0.1);
            animator.SetFloat("DirectionX", inputAxis.y);
            animator.SetFloat("DirectionY", inputAxis.y);


        }
    }

}
