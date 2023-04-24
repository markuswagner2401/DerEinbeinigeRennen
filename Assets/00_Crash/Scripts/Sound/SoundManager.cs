using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.AudioControl
{
    public class SoundManager : MonoBehaviour
    {
        public enum ContinueMode { Stop, Pause, Run }
        public enum HoldOnMode { Continue, FadeDown, FadeDownAndPause }

        [System.Serializable]
        public struct Sound
        {
            public string name;
            public AudioClip soundClip;
            public AudioSource audioSource;
            public bool spacialized;
            public bool looped;
            public float maxVolume;
            public float fadeInDuration;
            public float fadeOutDuration;
            public AnimationCurve fadeInCurve;
            public AnimationCurve fadeOutCurve;
            public float playDuration;
            public ContinueMode stopMode;

            public HoldOnMode holdOnMode;

            [HideInInspector] public Coroutine activeCoroutine;
        }

        [SerializeField] bool persistentObject = false;

        [Tooltip("negative value for no sound at start")]
        [SerializeField] int soundAtStart = -1;

        [SerializeField] private Sound[] sounds;

        [Header("Testing and Debugging")]
        [SerializeField] public int testSound;

        private bool inHoldOnMode = false;



        private void Awake()
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].audioSource = gameObject.AddComponent<AudioSource>();
                sounds[i].audioSource.clip = sounds[i].soundClip;
                sounds[i].audioSource.spatialize = sounds[i].spacialized;
                sounds[i].audioSource.loop = sounds[i].looped;
                sounds[i].audioSource.volume = 0;
            }
        }

        private void Start()
        {
            if (persistentObject)
            {
                DontDestroyOnLoad(this);
            }

            if (soundAtStart >= 0)
            {
                PlaySound(soundAtStart);
            }
        }

        public void PlaySound(int index)
        {
            if (index >= 0 && index < sounds.Length)
            {
                Sound sound = sounds[index];
                if (!sound.looped)
                {
                    sound.playDuration = sound.soundClip.length;
                }

                if (sound.activeCoroutine != null)
                {
                    StopCoroutine(sound.activeCoroutine);
                }
                sound.activeCoroutine = StartCoroutine(PlaySoundRoutine(sound));
            }
        }

        public void PlaySound(string name)
        {
            int index = FindSoundIndexByName(name);
            if (index != -1)
            {
                PlaySound(index);
            }
        }

        public void PlaySound(int index, AudioSource audioSource)
        {
            if (index < 0 || index >= sounds.Length)
            {
                Debug.LogWarning($"Sound index {index} not found!");
                return;
            }

            Sound sound = sounds[index];
            sound.audioSource = audioSource;
            PlaySound(index);
        }

        public void PlaySound(string name, AudioSource audioSource)
        {
            int index = FindSoundIndexByName(name);

            if (index < 0)
            {
                Debug.LogWarning($"Sound with name '{name}' not found!");
                return;
            }

            PlaySound(index, audioSource);
        }

        public void StopSound(int index)
        {
            if (index >= 0 && index < sounds.Length)
            {
                if (sounds[index].activeCoroutine != null)
                {
                    StopCoroutine(sounds[index].activeCoroutine);
                }
                sounds[index].activeCoroutine = StartCoroutine(StopSoundRoutine(sounds[index]));
            }
        }

        public void StopSound(string name)
        {
            int index = FindSoundIndexByName(name);
            if (index != -1)
            {
                StopSound(index);
            }
        }

        private IEnumerator PlaySoundRoutine(Sound sound)
        {
            sound.audioSource.Play();

            float elapsedTime = 0;
            float startVolume = sound.audioSource.volume;
            float targetVolume = sound.maxVolume;

            while (elapsedTime < sound.fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / sound.fadeInDuration;
                float volume = Mathf.Lerp(startVolume, targetVolume, sound.fadeInCurve.Evaluate(progress));

                sound.audioSource.volume = inHoldOnMode ? 0 : volume;
                yield return null;
            }

            if (sound.playDuration >= 0)
            {
                yield return new WaitForSeconds(sound.playDuration);
                StartCoroutine(StopSoundRoutine(sound));
            }
        }

        private IEnumerator StopSoundRoutine(Sound sound)
        {
            float elapsedTime = 0;
            float startVolume = sound.audioSource.volume;
            float targetVolume = 0;

            while (elapsedTime < sound.fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / sound.fadeOutDuration;
                sound.audioSource.volume = Mathf.Lerp(startVolume, targetVolume, sound.fadeOutCurve.Evaluate(progress));
                yield return null;
            }

            switch (sound.stopMode)
            {
                case ContinueMode.Stop:
                    sound.audioSource.Stop();
                    sound.audioSource.time = 0;
                    break;
                case ContinueMode.Pause:
                    sound.audioSource.Pause();
                    break;
                case ContinueMode.Run:
                    sound.audioSource.volume = 0;
                    break;
            }
        }

        private int FindSoundIndexByName(string name)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].name == name)
                {
                    return i;
                }
            }

            return -1;
        }

        /// hold on

        private IEnumerator HandleHoldOn(int index, bool holdOn)
        {
            Sound sound = sounds[index];
            float fadeDuration = 0.1f;

            if (holdOn)
            {
                switch (sound.holdOnMode)
                {
                    case HoldOnMode.Continue:
                        // Let it run as is
                        break;
                    case HoldOnMode.FadeDown:
                        yield return StartCoroutine(FadeVolumeRoutine(sound, 0, fadeDuration));
                        break;
                    case HoldOnMode.FadeDownAndPause:
                        yield return StartCoroutine(FadeVolumeAndPauseRoutine(sound, 0, fadeDuration));
                        break;
                }

                inHoldOnMode = true;
            }
            else
            {
                switch (sound.holdOnMode)
                {
                    case HoldOnMode.Continue:
                        // Nothing changes
                        break;
                    case HoldOnMode.FadeDown:
                        yield return StartCoroutine(FadeVolumeRoutine(sound, sound.maxVolume, fadeDuration));
                        break;
                    case HoldOnMode.FadeDownAndPause:
                        yield return StartCoroutine(FadeVolumeAndPlayRoutine(sound, sound.maxVolume, fadeDuration));
                        break;
                }

                inHoldOnMode = false;
            }
        }

        public void HoldOn(bool value)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].activeCoroutine != null)
                {
                    StartCoroutine(HandleHoldOn(i, value));
                }
            }
        }



        private IEnumerator FadeVolumeRoutine(Sound sound, float targetVolume, float duration)
        {
            float elapsedTime = 0;
            float startVolume = sound.audioSource.volume;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / duration;
                sound.audioSource.volume = Mathf.Lerp(startVolume, targetVolume, progress);
                yield return null;
            }
        }

        private IEnumerator FadeVolumeAndPauseRoutine(Sound sound, float targetVolume, float duration)
        {
            yield return StartCoroutine(FadeVolumeRoutine(sound, targetVolume, duration));
            sound.audioSource.Pause();
        }

        private IEnumerator FadeVolumeAndPlayRoutine(Sound sound, float targetVolume, float duration)
        {
            sound.audioSource.Play();
            yield return StartCoroutine(FadeVolumeRoutine(sound, targetVolume, duration));
        }
    }

}

