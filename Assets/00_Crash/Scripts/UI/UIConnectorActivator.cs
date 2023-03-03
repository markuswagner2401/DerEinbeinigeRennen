using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.XR;

using TMPro;
using Photon.Pun;

using ObliqueSenastions.PunNetworking;

namespace ObliqueSenastions.UISpace
{


    public class UIConnectorActivator : MonoBehaviour
    {


        [SerializeField] bool showConnectorAtStart = false;


        [Tooltip("Put here Ray Interactors and Connector UI")]
        [SerializeField] GameObject[] connectorUiGos;


        [Tooltip("after pressing secondary keys of both controllers for that duration, the ConnectorUI will be shown; Infinite Value for disabling")]
        [SerializeField] float durationBeforeConnectorUI;

        [SerializeField] GameObject messageObject;
        [SerializeField] TextMeshProUGUI messageTMP;

        [SerializeField] float messageDuration = 3f;







        float counter = 0;

        InputDevice leftHandDevice;


        InputDevice rightHandDevice;



        bool connectorUIActive = false;

        private bool leftButtonPressed;
        private bool leftPrimaryButtonPressed;
        private bool rightButtonPressed;
        private bool rightPrimaryButtonPressed;

        private void OnEnable()
        {
            ShowConnector(showConnectorAtStart);
        }
        private void Start()
        {


            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            MultiplayerConnector.instance.onConnectorMessage += ShowMessage;
        }

        private void OnDestroy()
        {
            MultiplayerConnector.instance.onConnectorMessage -= ShowMessage;
        }
        private void Update()
        {
            ProcessInput();
        }
        void ProcessInput()
        {
            if (PhotonNetwork.IsConnected)
            {
                counter = 0f;
                return;
            }

            if (SceneManager.GetActiveScene().name == "TransferScene") return;

            if (!leftHandDevice.isValid)
            {
                leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            }

            if (!rightHandDevice.isValid)
            {
                rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            }



            leftHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool leftButtonPressed);
            //leftHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimaryButtonPressed);
            rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rightButtonPressed);
            //rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimaryButtonPressed);

            if (leftButtonPressed && rightButtonPressed)
            {
                counter += Time.deltaTime;
            }


            else
            {
                counter = 0f;
            }
            if (counter > durationBeforeConnectorUI)
            {
                if (!connectorUIActive)
                {
                    ShowConnector(true);
                    connectorUIActive = true;
                }

            }
        }

        public void ShowConnector(bool value)
        {

            Debug.Log("show connector: " + value);

            if (connectorUiGos.Length <= 0) return;


            foreach (var item in connectorUiGos)
            {
                item.gameObject.SetActive(value);
            }

            connectorUIActive = value;
        }

        public void ShowMessage(string message)
        {
            if (messageObject == null || messageTMP == null)
            {
                Debug.LogError("UIConnectorActivator: No message Object assigned");
                return;
            }

            StartCoroutine(ShowMessageRoutine(message));
        }

        IEnumerator ShowMessageRoutine(string message)
        {
            messageObject.SetActive(true);
            messageTMP.text = message;
            yield return new WaitForSeconds(messageDuration);
            messageTMP.text = "";
            messageObject.SetActive(false);
            yield break;
        }

    }

}
