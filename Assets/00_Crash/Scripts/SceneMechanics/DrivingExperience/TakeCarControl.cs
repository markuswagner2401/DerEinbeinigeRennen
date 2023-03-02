using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCarControl : MonoBehaviour
{

    [SerializeField] float throttleValue = 0;
    [SerializeField] float breakInput = 1f;
    [SerializeField] XRCarController xrCarContoller = null;
    
    void Start()
    {
        if (xrCarContoller == null)
        {
            xrCarContoller = GetComponent<XRCarController>();
        }
    }

   

    public void OvertakeCarControl(Vector2 throttleAndBreak)
    {
        if(xrCarContoller != null)
        {
            xrCarContoller.enabled = false;
        }

        // RCC_InputManager.SetThrottleInput(throttleValue);
        // RCC_InputManager.SetBreakInput(breakInput);
    }

    public void LetGoCarControl()
    {
        if (xrCarContoller != null)
        {
            xrCarContoller.enabled = true;
        }
    }
}
