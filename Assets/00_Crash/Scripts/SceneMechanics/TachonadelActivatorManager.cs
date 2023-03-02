using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TachonadelActivatorManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    public void ShowTachonadeln(bool value)
    {
        print("show tachonadeln");
        GameObject[] networkPlayers = GameObject.FindGameObjectsWithTag("NetworkPlayer");
        foreach (var item in networkPlayers)
        {
            if(item.TryGetComponent<TachonadelActivator>(out TachonadelActivator tachonadelActivator))
            {
                print("activate tachonadel");
                tachonadelActivator.ShowTachonadeln(value);
            }
            
        }
    }
}
