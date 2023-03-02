using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChaseModeChanger : MonoBehaviour
{
    [SerializeField] RCC_AICarController aICarController = null;
    [SerializeField] RCC_AICarController.NavigationMode[] navigationModes = null;

    // [SerializeField] Transform target = null;
    
  
    void Start()
    {
        if (aICarController == null) 
        {
            aICarController = GetComponent<RCC_AICarController>();
        }

        if(navigationModes == null)
        {
            navigationModes = new RCC_AICarController.NavigationMode[3];
            navigationModes[0] = RCC_AICarController.NavigationMode.ChaseTarget;
            navigationModes[1] = RCC_AICarController.NavigationMode.FollowTarget;
            navigationModes[2] = RCC_AICarController.NavigationMode.FollowWaypoints;
        }
        
    }

    public void ChangeAINavigationMode(int navigationMode)
    {
        print("change mode");
        aICarController.navigationMode = navigationModes[navigationMode];
        // if (aICarController.navigationMode = RCC_AICarController.NavigationMode.FollowTarget || ) 
    }

   
    void Update()
    {
        
    }
}
