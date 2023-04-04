using UnityEngine;

public class FindAudioListeners : MonoBehaviour
{
    void Start()
    {
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();
        Debug.Log("Number of Audio Listeners in scene: " + audioListeners.Length);

        foreach (AudioListener audioListener in audioListeners)
        {
            Debug.Log("Audio Listener found: " + audioListener.name);
        }
    }
}