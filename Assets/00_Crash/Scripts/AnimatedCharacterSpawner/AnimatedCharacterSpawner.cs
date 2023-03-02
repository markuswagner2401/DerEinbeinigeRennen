using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCharacterSpawner : MonoBehaviour
{
    [SerializeField] SpawnedCharacter[] animatedCharacterPrefabs;
    [SerializeField] GameObject[] triggerVisuals;
    
  

    [SerializeField] bool playAtAwake = false;

    bool hasAlreadyPlayed = false;


    void Start()
    {

        if (playAtAwake) 
        {
            StartCoroutine(WaitandSpawn());
        }
    }
   

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag != "Traveller") return;
        print("triggered by traveller");
        if (hasAlreadyPlayed == false)
        {
            print("starting Coroutine");
            StartCoroutine(WaitandSpawn());

        }

        foreach (GameObject triggerVisual in triggerVisuals)
        {
            triggerVisual.SetActive(false);
        }
    }
    

    private IEnumerator WaitandSpawn()
    {
        foreach (SpawnedCharacter spawnedCharacter in animatedCharacterPrefabs)
        {
            print("tra spawning" + spawnedCharacter);
            float waitTime = GetWaitTime(spawnedCharacter);
            yield return new WaitForSeconds(waitTime);
            Instantiate(spawnedCharacter.prefabToSpawn, spawnedCharacter.transform.position, spawnedCharacter.transform.rotation);
            
            
        }

        hasAlreadyPlayed = true;

        
        
    }

    private float GetWaitTime(SpawnedCharacter spawnedCharacter)
    {
        if (spawnedCharacter.GetComponent<SpawnedCharacter>() == null)
        {
            return 0f;
        }

        else
        {
            return spawnedCharacter.GetComponent<SpawnedCharacter>().waitTimeAfterSpawn;
        }
    }
    
}
