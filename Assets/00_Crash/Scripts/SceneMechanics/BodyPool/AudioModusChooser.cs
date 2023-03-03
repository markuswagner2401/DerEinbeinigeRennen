using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace ObliqueSenastions.TransformControl
{

    public class AudioModusChooser : MonoBehaviour
    {
        [Tooltip("for debugging")][SerializeField] XRNode xRNode;
        [SerializeField] bool debuggingWithDevice = false;
        InputDevice device;



        bool buttonUsage;
        bool buttonAlreadyPressed;

        [SerializeField] AudioModusPlayer audioModusPlayer;

        [SerializeField] Collider zone2;
        [SerializeField] Collider zone3;
        [SerializeField] Collider killZone;

        [SerializeField] int indexSoundModeNull = 0;
        [SerializeField] int indexSoundMode1 = 1;
        [SerializeField] int indexSoundMode2 = 2;
        [SerializeField] int indexSoundMode3 = 3;


        [SerializeField] int modeIndex = 0;



        bool inZone1;
        bool inZone2;
        bool inZone3;

        [SerializeField] float smoothingSchiebedetektor = 0.01f;
        Vector3 previousPosition = new Vector3();
        float smoothSpeed;












        // Start is called before the first frame update
        void Start()
        {

            device = InputDevices.GetDeviceAtXRNode(xRNode);
            audioModusPlayer.SetAudioMode(indexSoundMode1);

            audioModusPlayer.SetTransformParent(indexSoundMode1, this.transform);



        }

        // Update is called once per frame
        void Update()
        {
            if (debuggingWithDevice) PlayViaXRInput(); // for debugging




        }


        public void SetAudioSchiebeMode(int index)
        {

            if (!inZone2) return;

            print("set audio schiebe mode" + index);

            audioModusPlayer.SetAudioMode(index);
        }






        private void OnTriggerEnter(Collider other)
        {
            if (other == zone2)
            {

                inZone1 = false;
                inZone2 = true;
                inZone3 = false;


                // audioModusPlayer.SetAudioMode(indexSoundZone2);

                // audioModusPlayer.SetTransformParent(indexSoundZone2, this.transform);


            }

            if (other == zone3)
            {

                inZone1 = false;
                inZone2 = false;
                inZone3 = true;

                audioModusPlayer.SetAudioMode(indexSoundMode1);
                audioModusPlayer.SetTransformParent(indexSoundMode1, this.transform);

            }

            if (other == killZone)
            {
                inZone1 = false;
                inZone2 = false;
                inZone3 = false;
                print("destroy");

                //audioModusPlayer.SetTransformParent(indexSoundMode3, null);
                // Destroy(this.gameObject);
            }




        }








        private void PlayViaXRInput() // for debugging
        {


            if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonUsage) && buttonUsage)
            {
                if (buttonAlreadyPressed) return;

                audioModusPlayer.SetAudioMode(modeIndex);


                buttonAlreadyPressed = true;
            }

            else
            {
                buttonAlreadyPressed = false;
            }
        }
    }

}
