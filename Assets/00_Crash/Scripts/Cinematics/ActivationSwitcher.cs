using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ActivationSwitcher : MonoBehaviour
{
    [SerializeField] GameObject[] activationGroupA;
    [SerializeField] GameObject[] activationGroupB;

    [SerializeField] bool groupAActiveOnStart = true;
    [SerializeField] bool groubBActiveOnStart = false;



    private void Start()
    {
        ActivationGroupA(groubBActiveOnStart);
        ActivationGroupB(groubBActiveOnStart);

    }

    public void ActivationGroupA(bool value)
    {
        foreach (GameObject gameObject in activationGroupA)
        {
            gameObject.SetActive(value);
            
        }
    }

    public void ActivationGroupB(bool value)
    {
        foreach (GameObject gameObject in activationGroupB)
        {
            gameObject.SetActive(value);
        }
    }

    public void ActivationGroupSwitchA()
    {
        foreach (GameObject item in activationGroupA)
        {
            bool activeSelf = item.activeSelf;
            item.SetActive(!activeSelf);
        }
    }

    public void ActivationGroupSwitchB()
    {
        foreach (GameObject item in activationGroupB)
        {
            bool activeSelf = item.activeSelf;
            item.SetActive(!activeSelf);
        }
    }


    
}
