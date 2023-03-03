using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.Animation
{

    public class BodyParsHandler : MonoBehaviour
    {
        [Tooltip("Shoose Left Hand or Right Hand")]
        [SerializeField] XRNode xRNode;

        // [SerializeField] XRController xRController;
        [SerializeField] GameObject[] bodyparts;
        [SerializeField] bool drivenByTriggerZones = false;
        [SerializeField] bool drivenByXrControllerButton = true;
        [SerializeField] bool drivenByTimelineSignals = true;

        InputDevice inputDevice;

        bool buttonUsage = false;


        bool buttonPressed = false;

        int actualBodyPartIndex = 0;



        private void Start()
        {
            inputDevice = InputDevices.GetDeviceAtXRNode(xRNode);
        }

        void Update()
        {
            if (drivenByXrControllerButton == true)
            {
                ProcessButtonInput();
            }

        }

        public void ZoneTriggerBodyPart()
        {
            if (!drivenByTriggerZones) return;

            ReverseExplodeActualBodyPart();
            SelectNextBodypart();
        }

        public void TimelineTriggerBodyPart()
        {
            if (!drivenByTimelineSignals) { print("timeline Trigger not anabled"); return; }

            ReverseExplodeActualBodyPart();
            SelectNextBodypart();

        }

        private void ProcessButtonInput()
        {

            if (inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonUsage) && buttonUsage)
            {

                if (buttonPressed) return;

                ReverseExplodeActualBodyPart();
                SelectNextBodypart();
                buttonPressed = true;
            }

            else
            {
                buttonPressed = false;
            }
        }

        private void ReverseExplodeActualBodyPart()
        {
            StartCoroutine(bodyparts[actualBodyPartIndex].GetComponent<BlendShapeAnimator>().ReverseExplode());
        }

        private void SelectNextBodypart()
        {
            if (actualBodyPartIndex + 1 <= bodyparts.Length - 1)
            {
                actualBodyPartIndex += 1;
            }

            else
            {
                actualBodyPartIndex = 0;
            }

        }
    }


}
