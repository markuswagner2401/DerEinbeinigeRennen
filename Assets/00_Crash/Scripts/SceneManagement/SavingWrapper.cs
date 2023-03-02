using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save";
    
    

   
    void Update()
    {
        
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Delete() 
    {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }
}
