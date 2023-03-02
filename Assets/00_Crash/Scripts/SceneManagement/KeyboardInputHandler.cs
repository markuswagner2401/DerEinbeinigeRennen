using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class KeyboardInputHandler : MonoBehaviour
{

    public enum KeyTrigger
    {
        onKeyDown,
        onKeyUp,
        onKey,
    }
    [SerializeField] KeyboardCommand[] keyboardCommands;

    [System.Serializable]
    struct KeyboardCommand
    {
        public string note;
        public KeyCode keycode;
        public KeyTrigger keyTrigger;
        public UnityEvent unityEvent;
    }

    private void Update()
    {
        foreach (var item in keyboardCommands)
        {
            if (item.keyTrigger == KeyTrigger.onKeyDown)
            {
                if (Input.GetKeyDown(item.keycode))
                {
                    item.unityEvent.Invoke();
                    Debug.Log("KeyboardInputHandler: Keycommand: " + item.keycode.ToString());
                }
            }

            else if (item.keyTrigger == KeyTrigger.onKeyUp)
            {
                if (Input.GetKeyUp(item.keycode))
                {
                    item.unityEvent.Invoke();
                    Debug.Log("KeyboardInputHandler: Keycommand: " + item.keycode.ToString());
                }

            }

            else
            {
                if (Input.GetKey(item.keycode))
                {
                    item.unityEvent.Invoke();
                }
            }

        }
    }
}