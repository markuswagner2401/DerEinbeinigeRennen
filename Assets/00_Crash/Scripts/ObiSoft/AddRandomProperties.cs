using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class AddRandomProperties : MonoBehaviour
{
    [SerializeField] bool deformationResistance = true;
    [SerializeField] float deformationResistanceMin = 0f;
    [SerializeField] float deformationResistanceMax = 1f;

    [SerializeField] bool maxDeformation = true;
    [SerializeField] float maxDeformationMin = 0f;
    [SerializeField] float maxDeformationMax = 1f;

    [SerializeField] bool plasticYield = true;
    [SerializeField] float plasticYieldMin = 0f;
    [SerializeField] float plasticYieldMax = 1f;

    [SerializeField] bool plasticCreep = true;
    [SerializeField] float plasticCreepMin = 0f;
    [SerializeField] float plasticCreepMax = 1f;

    [SerializeField] bool plasticRecovery = true;
    [SerializeField] float plasticRecoveryMin = 0f;
    [SerializeField] float plasticRecoveryMax = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void CreateRandomValues()
    {
        ObiSoftbody obiSoftbody = GetComponent<ObiSoftbody>();

        if(deformationResistance)
        {
            obiSoftbody.deformationResistance = Random.Range(deformationResistanceMin, deformationResistanceMax);
        }

        if(maxDeformation)
        {
            obiSoftbody.maxDeformation = Random.Range(maxDeformationMin, maxDeformationMax);
        }
        
        if(plasticYield)
        {
            obiSoftbody.plasticYield = Random.Range(plasticYieldMin, plasticYieldMax);
        }

        if(plasticCreep)
        {
            obiSoftbody.plasticCreep = Random.Range(plasticCreepMin, plasticCreepMax);
        }
        
        if(plasticRecovery)
        {
            obiSoftbody.plasticRecovery = Random.Range(plasticRecoveryMin, plasticRecoveryMax);
        }
        
        

        
    }

    // Update is called once per frame
    void Update()
    {

    }
}