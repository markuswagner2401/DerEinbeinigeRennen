using UnityEngine;
using ObliqueSenastions.VRRigSpace;
using System.Collections;

public class CustomCarSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource gearSwitchAudioSource;

    [SerializeField] SimpleVelocityTracker simpleVelocityTracker;
    public AudioClip lowGearSound;
    public AudioClip midGearSound;
    public AudioClip highGearSound;
    public AudioClip gearSwitchSound;

    public float minPitch = 0.5f;
    public float maxPitch = 2.0f;
    public float minVelocity = 0.0f;
    public float maxVelocity = 30.0f;

    public float midGearThreshold = 10.0f;
    public float highGearThreshold = 20.0f;

    public float pitchSmoothing = 2.0f;
    public float gearSwitchDuration = 0.3f;
    public float gearSwitchPitchOffset = 0.5f;

    private float currentPitch;
    private int currentGear = 0;
    private bool isSwitchingGears = false;

    private void Update()
    {
        if (isSwitchingGears) return;

        float currentVelocity = simpleVelocityTracker.GetSpeed();
        float normalizedVelocity = Mathf.InverseLerp(minVelocity, maxVelocity, currentVelocity);
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, normalizedVelocity);

        int targetGear = 0;
        AudioClip targetClip = lowGearSound;

        if (currentVelocity < midGearThreshold)
        {
            targetGear = 1;
            targetClip = lowGearSound;
        }
        else if (currentVelocity >= midGearThreshold && currentVelocity < highGearThreshold)
        {
            targetGear = 2;
            targetClip = midGearSound;
        }
        else
        {
            targetGear = 3;
            targetClip = highGearSound;
        }

        if (currentGear != targetGear)
        {
            StartCoroutine(SwitchGears(targetClip));
            currentGear = targetGear;
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * pitchSmoothing);
            audioSource.pitch = currentPitch;
        }
    }

    private IEnumerator SwitchGears(AudioClip targetClip)
    {
        isSwitchingGears = true;
        audioSource.Stop();
        gearSwitchAudioSource.clip = gearSwitchSound;
        gearSwitchAudioSource.Play();

        yield return new WaitForSeconds(gearSwitchDuration);

        currentPitch -= gearSwitchPitchOffset;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        audioSource.clip = targetClip;
        audioSource.Play();

        isSwitchingGears = false;
    }
}