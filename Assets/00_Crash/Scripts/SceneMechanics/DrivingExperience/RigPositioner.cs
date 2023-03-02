using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigPositioner : MonoBehaviour
{
    [SerializeField] Transform target = null;

    
    void Start()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
            
            transform.SetParent(target);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(target != null)
        // {
        //     transform.position = target.position;
        //     transform.rotation = target.rotation;
        // }

        
    }
}
