using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachEntkoppler : MonoBehaviour
{
    

    [SerializeField] TargetAimPair[] targetAimPairs;

    [System.Serializable]
    struct TargetAimPair
    {
        public Transform control;
        public Transform target;
        
    }
    
    [SerializeField] [Range(0.001f, 1f)] float entkopplungsEndDelay = 0.01f;

    
    [SerializeField] [Range(0, 1)] float entkopplungsstandfaktor = 0.99f;

    
    [SerializeField] bool EffectRotation = false;
    float startTime = 0f;
    bool entkopplungAktiviert = false;
    bool entkoppelt = false;

    

    void Start()
    {
        foreach (var targetAimPair in targetAimPairs)
        {

            targetAimPair.target.transform.position = targetAimPair.control.transform.position;
        }
        
        
    }

   
    void Update()
    {
        

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     entkopplungAktiviert = !entkopplungAktiviert;
        // }

        if (EffectRotation == false)
        {
            foreach (var targetAimPair in targetAimPairs)
            {
                targetAimPair.target.transform.rotation = targetAimPair.control.transform.rotation;
            }
            
        }

        if (!entkopplungAktiviert)
        {
            foreach (var targetAimPair in targetAimPairs)
            {
                targetAimPair.target.transform.position = targetAimPair.control.transform.position;
                targetAimPair.target.transform.rotation = targetAimPair.control.transform.rotation;
            }
            
            
        }

        if (entkopplungAktiviert && !entkoppelt)
        {
            entkoppelt = true;
            StartCoroutine(Entkoppler());
        }

    }

    public void Entkoppeln()
    {
        entkopplungAktiviert = !entkopplungAktiviert;
    }

    public bool GetEntkopplung()
    {
        return entkopplungAktiviert;
    }

    IEnumerator Entkoppler()
    {
        print("Start Entkopplung");
        startTime = 0f;
        float entkopplungsstand = entkopplungsstandfaktor;


        while (entkopplungAktiviert)
        {
            foreach (var targetAimPair in targetAimPairs)
            {
                targetAimPair.target.transform.position = Vector3.Lerp(targetAimPair.target.transform.position, targetAimPair.control.transform.position, entkopplungsstand) ;
                
            }

            

            if (EffectRotation)
            {
                foreach (var targetAimPair in targetAimPairs)
                {
                    targetAimPair.target.transform.rotation = Quaternion.Lerp(targetAimPair.target.transform.rotation, targetAimPair.control.transform.rotation, entkopplungsstand);
                    
                }
                
            }
            
            if (entkopplungsstand > entkopplungsEndDelay)
            {
                entkopplungsstand *= entkopplungsstandfaktor;
            }
 
            yield return null;
        }

        entkoppelt = false;
        yield return null;
        
    }
}
