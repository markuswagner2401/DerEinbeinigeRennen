using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace ObliqueSenastions.TransformControl
{

    public class AudioModusPlayer : MonoBehaviour
    {


        [System.Serializable]
        public struct AudioMode
        {
            public AudioSource audioSource;
            public bool isOn;
        }

        int currentModeIndex = 0;

        [SerializeField] AudioMode[] audioModes;

        Transform currentParent = null;


        // Start is called before the first frame update
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {
            if (currentParent != null)
            {
                transform.position = currentParent.position;
            }

        }

        public void SetAudioMode(int modeIndex)
        {
            if (modeIndex == currentModeIndex) return;

            currentModeIndex = modeIndex;

            foreach (AudioMode mode in audioModes)
            {
                if (mode.audioSource.clip == null) continue;
                mode.audioSource.Pause();
            }


            if (audioModes[modeIndex].audioSource == null || audioModes[modeIndex].audioSource.clip == null) return;


            audioModes[modeIndex].audioSource.Play();
        }

        public void PauseAudioMode(int modeIndex)
        {

            audioModes[modeIndex].audioSource.Pause();
        }

        public void PlayAudioMode(int modeIndex)
        {
            audioModes[modeIndex].audioSource.Play();
        }

        public void SetTransformParent(int modeIndex, Transform parent)
        {
            // if (modeIndex == currentModeIndex) return;

            currentParent = parent;

        }






        // private void ToggleAudioMode1()
        // {
        //     if(!audioMode1.isOn)
        //     {
        //         audioMode1.audioSource.Play();
        //         audioMode1.isOn = true;
        //     }

        //     else
        //     {
        //         audioMode1.audioSource.Pause();
        //         audioMode1.isOn = false;
        //     }

        // }

        // private void ToggleAudioMode2()
        // {
        //     if (!audioMode2.isOn)
        //     {
        //         audioMode2.audioSource.Play();
        //         audioMode2.isOn = true;
        //     }

        //     else
        //     {
        //         audioMode2.audioSource.Pause();
        //         audioMode2.isOn = false;
        //     }

        // }

        // private void ToggleAudioMode3()
        // {
        //     if (!audioMode3.isOn)
        //     {
        //         audioMode3.audioSource.Play();
        //         audioMode3.isOn = true;
        //     }

        //     else
        //     {
        //         audioMode3.audioSource.Pause();
        //         audioMode3.isOn = false;
        //     }

        // }
    }

}
