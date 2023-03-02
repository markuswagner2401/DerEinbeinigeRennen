using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlanesTriggerer : MonoBehaviour
{
    [SerializeField] int containerIndex;
    [SerializeField] GroundPlanesManager groundPlanesManager = null;
    
    
    private void Start() 
    {
        if(groundPlanesManager == null)
        {
            groundPlanesManager = FindObjectOfType<GroundPlanesManager>();
        }
    }
    


    public int GetIndex()
    {
        return containerIndex;
    }

    public GroundPlanesManager GetGroundPlanesManager()
    {
        return groundPlanesManager;
    }

    
}
