using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.SceneSpace
{
    
public class PersistentObject : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}


 

}
