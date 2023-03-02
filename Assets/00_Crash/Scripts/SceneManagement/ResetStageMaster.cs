using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStageMaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetStageMasters(int index)
    {
        StageMaster[] stageMasters = FindObjectsOfType<StageMaster>();
        foreach (var master in stageMasters)
        {
            master.SetCurrentIndex(index);
        }
    }
}
