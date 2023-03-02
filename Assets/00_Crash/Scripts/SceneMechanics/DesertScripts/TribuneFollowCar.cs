using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribuneFollowCar : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothing = 0.01f;

    Vector3 offset = new Vector3();

    Quaternion yaw;
    Quaternion lookRotation;

  
    
   
    void Start()
    {
        offset =  transform.position - target.position;
        //offset.y = 0;
    }


    void Update()
    {
        
        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, smoothing);

        lookRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
        yaw = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        
        transform.rotation = yaw;
        
    }
}
