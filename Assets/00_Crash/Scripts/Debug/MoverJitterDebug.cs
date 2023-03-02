using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverJitterDebug : MonoBehaviour
{
    [SerializeField] float speed = 0f;
    [SerializeField] float addSpeed = 0.1f;

    [SerializeField] Vector3 direction = Vector3.forward;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            speed += addSpeed;
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            speed -= addSpeed;
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            speed = 0f;
        }

        transform.position += direction * speed;
    }
}
