using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAtStart : MonoBehaviour
{
    [Tooltip("for Level Design purpose")]
    [SerializeField] GameObject[] activateAtStartGO;
    void Start()
    {
        foreach (var item in activateAtStartGO)
        {
            item.SetActive(true);
        }
    }

  
}
