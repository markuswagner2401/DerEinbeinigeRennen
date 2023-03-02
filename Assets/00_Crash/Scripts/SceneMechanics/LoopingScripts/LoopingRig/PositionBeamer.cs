using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionBeamer : MonoBehaviour
{
    [SerializeField] Transform[] beamPositions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeamToPosition(int index)
    {
        if(index > beamPositions.Length) 
        {
            print("index of beam position not defined");
            return;
        }

        print("beam to position" + index);
        transform.position = beamPositions[index].position;
    }
}
