using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchwertCollision : MonoBehaviour
{

    RaycastHit hit;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] float saberlength = 1f;
    [SerializeField] GameObject hitEffect;

    public bool schwertAktiv;

   

    
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (schwertAktiv)
        {
            PlayHitEffect();
        }
        
    }

    private void PlayHitEffect()
    {
        bool hasHit = Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, saberlength);
        if (!hasHit) {return;} 
        else
        {
            var hitEffectVFX = Instantiate(hitEffect, hit.point, Quaternion.identity);
            Destroy(hitEffectVFX, 2f);
        }
        
    }

    
}
