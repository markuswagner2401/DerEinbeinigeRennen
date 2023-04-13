using UnityEngine;
using ObliqueSenastions.VRRigSpace;
using System.Collections;
using ObliqueSenastions.UISpace;


public class CustomCarSoundController : MonoBehaviour
{
    [System.Serializable]
    public struct Gear
    {
        public AudioClip gearSound;
        public float minPitch;
        public float maxPitch;
        public float startVolume;
        [Range(0, 1)] public float gearSwitch;
    }

    [SerializeField] float volume = 1f;

    public AudioSource audioSource;
    public AudioSource gearSwitchAudioSource;

    [SerializeField][Range(0, 1)] float inputValue;

    [SerializeField] bool readAtTacho;

    [SerializeField] Tachonadel[] tachos = null;
    public AudioClip gearSwitchSound;
    public float minVelocity = 0.0f;
    public float maxVelocity = 30.0f;

    [SerializeField] private Gear[] gears;

    public float pitchSmoothing = 2.0f;
    public float gearSwitchDuration = 0.3f;
    public float volumeFadeDuration = 0.1f;

    public bool motorRunning = true;
    public int fixGear = -1;

    private float currentPitch;
    private int currentGear = 0;
    private bool isSwitchingGears = false;

    private void Start()
    {
        if (motorRunning)
        {
            audioSource.clip = gears[0].gearSound;
            audioSource.Play();
        }
    }

    private void Update()
    {

        if(readAtTacho && tachos.Length > 0)
        {
            float tachoSum = 0;
            for (int i = 0; i < tachos.Length; i++)
            {
                tachoSum += tachos[i].GetNormedTargetPosition();
            }
            inputValue = tachoSum / tachos.Length;
        }


        HandleMotorRunning();

        if (!motorRunning) return;

        if (isSwitchingGears) return;

        float currentVelocity = inputValue;
        float normalizedVelocity = Mathf.InverseLerp(minVelocity, maxVelocity, currentVelocity);

        int targetGear = 0;
        AudioClip targetClip = gears[0].gearSound;

        if (fixGear >= 0)
        {
            if (fixGear >= gears.Length)
            {
                targetGear = gears.Length - 1;
            }
            else
            {
                targetGear = fixGear;
            }
            targetClip = gears[targetGear].gearSound;
        }
        else
        {
            for (int i = 0; i < gears.Length - 1; i++)
            {
                float switchPoint = Mathf.Lerp(minVelocity, maxVelocity, gears[i].gearSwitch);
                float nextSwitchPoint = Mathf.Lerp(minVelocity, maxVelocity, gears[i + 1].gearSwitch);

                if (currentVelocity >= switchPoint && currentVelocity < nextSwitchPoint)
                {
                    targetGear = i;
                    targetClip = gears[i].gearSound;
                    break;
                }
                if (nextSwitchPoint <= switchPoint)
                {
                    Debug.LogWarning("Gear[" + (i + 1) + "] has a smaller gearSwitch value than Gear[" + i + "]. Ignoring Gear[" + (i + 1) + "].");
                }
            }
            if (currentVelocity >= Mathf.Lerp(minVelocity, maxVelocity, gears[gears.Length - 1].gearSwitch))
            {
                targetGear = gears.Length - 1;
                targetClip = gears[gears.Length - 1].gearSound;
            }
        }

        float targetPitch = Mathf.Lerp(gears[targetGear].minPitch, gears[targetGear].maxPitch, normalizedVelocity);

        if (currentGear != targetGear && fixGear == -1)
        {
            StartCoroutine(SwitchGears(targetGear, targetClip));
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

    private void HandleMotorRunning()
    {
        if (motorRunning)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = gears[0].gearSound;
                audioSource.Play();
            }
            if (audioSource.volume < volume)
            {
                audioSource.volume += Time.deltaTime / volumeFadeDuration;
            }
        }
        else
        {
            if (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime / volumeFadeDuration;
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

    public void StartMotor(bool value)
    {
        motorRunning = value;
    }

    private IEnumerator SwitchGears(int targetGear, AudioClip targetClip)
    {
        isSwitchingGears = true;
        gearSwitchAudioSource.clip = gearSwitchSound;
        gearSwitchAudioSource.Play();

        float transitionStartTime = Time.time;
        float initialPitch = currentPitch;
        float targetMinPitch = gears[targetGear].minPitch;

        while (Time.time - transitionStartTime < gearSwitchDuration)
        {
            currentPitch = Mathf.Lerp(initialPitch, targetMinPitch, (Time.time - transitionStartTime) / gearSwitchDuration);
            audioSource.pitch = currentPitch;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = targetClip;
        audioSource.Play();

        // Fade in the volume after the gear change
        audioSource.volume = gears[targetGear].startVolume;
        float volumeFadeStartTime = Time.time;
        while (Time.time - volumeFadeStartTime < volumeFadeDuration)
        {
            audioSource.volume = Mathf.Lerp(gears[targetGear].startVolume, volume, (Time.time - volumeFadeStartTime) / volumeFadeDuration);
            yield return null;
        }

        isSwitchingGears = false;
    }
}
