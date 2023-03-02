using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionRandomizer : MonoBehaviour
{
    [SerializeField] Transform[] waypoints = null;
    [SerializeField] float frequency = 5f;
    [SerializeField] float distance = 50f;
    
    

    float timeSinceRandomize = 0f;

    
    void Start()
    {
        
    }

   
    void Update()
    {
        timeSinceRandomize += Time.deltaTime;

        if (timeSinceRandomize > frequency)
        {
            RandomizePositions();
            timeSinceRandomize = 0f;
        }

    }

    private void RandomizePositions()
    {
        foreach (var waypoint in waypoints)
        {
            Vector3 newPosition =  new Vector3 (waypoint.position.x + UnityEngine.Random.Range(-distance, distance), waypoint.position.y, waypoint.position.z + UnityEngine.Random.Range(-distance, +distance));
            if(NavMesh.SamplePosition(newPosition, out NavMeshHit hit, 100f, NavMesh.AllAreas))
            {
                waypoint.position = hit.position;
            }
        }
    }
}
