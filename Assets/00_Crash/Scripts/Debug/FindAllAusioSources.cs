using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAllAusioSources : MonoBehaviour
{
    [SerializeField] AudioSource[] audioSources;

    void Start()
    {
        // Find all Audio Source components in the scene
        audioSources = FindObjectsOfType<AudioSource>();

        // Iterate through the array and display the game object names with an Audio Source component
        foreach (AudioSource audioSource in audioSources)
        {
            Debug.Log("Game Object with Audio Source: " + audioSource.gameObject.name);
        }
    }
}
