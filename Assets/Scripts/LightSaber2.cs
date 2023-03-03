using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.AudioControl;
using UnityEngine;

public class LightSaber2 : MonoBehaviour
{

    [SerializeField] AudioClip beamAudio;
    [SerializeField] float beamAudioFXFadeOutDuration = 0.1f;
    [SerializeField] float beamAudioFXFadeInDuration = 0.1f;

    AudioSource audioSource;
    Animator animator;

    SchwertCollision schwertCollision;

    bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        schwertCollision = GetComponentInChildren<SchwertCollision>();
        schwertCollision.schwertAktiv = false;
    }

    // public void TriggerBeam()
    // {
        
    //     print ("TriggerBeam");
    //     bool isOn = animator.GetBool("SchwertAktiv");
    //     if (!isOn)
    //     {
    //         audioSource.PlayOneShot(beamAudio);
    //     }

    //     else
    //     {
    //         audioSource.Stop();
    //     }
    //     animator.SetBool("SchwertAktiv", !isOn);

    //     if (isOn) 
    //     {
    //         schwertCollision.schwertAktiv = false;
    //     }

    //     else
    //     {
    //         schwertCollision.schwertAktiv = true; // bools verdreht?
    //     }
        
    // }

    public void StartBeam()
    {
        isOn = true;
        ProcessBeam(isOn);
        ControlCollisionWithBeam(isOn);
    }

    public void StopBeam()
    {
        isOn = false;
        ProcessBeam(isOn);
        ControlCollisionWithBeam(isOn);
    }


    private void ProcessBeam(bool isOn)
    {
        animator.SetBool("SchwertAktiv", isOn);

        if (isOn)
        {
            audioSource.PlayOneShot(beamAudio);
            StartCoroutine(FadeAudioSource.StartFade(audioSource, beamAudioFXFadeInDuration, 1));
        }
        else
        {
            StartCoroutine(FadeAudioSource.StartFade(audioSource, beamAudioFXFadeOutDuration, 0));
            //Invoke ("StopAudioSource" , beamAudioFXFadeOutDuration);
        }
    }

    private void StopAudioSource()
    {
        audioSource.Stop();
    }

    private void ControlCollisionWithBeam(bool isOn)
    {
        schwertCollision.schwertAktiv = isOn;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
