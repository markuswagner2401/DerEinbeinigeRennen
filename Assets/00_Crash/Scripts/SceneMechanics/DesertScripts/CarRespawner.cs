using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRespawner : MonoBehaviour
{
    [SerializeField] Transform respawnTarget = null;
    Vector3 respawnPosition = new Vector3();

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "KillZone")
        {
            transform.position = respawnPosition;
            print("respawn");
        }
    }
    
    void Start()
    {
        if (respawnTarget == null)
        {
            respawnPosition = transform.position;
        }

        else
        {
            respawnPosition = respawnTarget.position;
        }
    }

    
    void Update()
    {
        
    }
}
