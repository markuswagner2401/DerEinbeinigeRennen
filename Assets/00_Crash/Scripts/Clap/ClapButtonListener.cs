using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.ClapSpace
{
    public class ClapButtonListener : MonoBehaviour
    {
        [SerializeField] UnityEvent onHoverEnter = null;
    [SerializeField] UnityEvent onHoverExit = null;
    [SerializeField] UnityEvent onActivate = null;
    public UnityEvent onDeactivate = null;

    [SerializeField] string projectileTag = "UIProjectile";
    [SerializeField] bool activateOnClap = true;
    [SerializeField] float clapActivationThreshold = 0.1f;
    [SerializeField] bool activateOnShootHit = true;
    

    private void OnCollisionEnter(Collision other) 
    {
        if(!activateOnShootHit) return;

        if(other.gameObject.tag == projectileTag)
        {
            
            if(ReferenceEquals(onActivate,null)) return;

            onActivate.Invoke();
        }
    }

    public void HoverEnter(bool value)
    {
        if(value)
        {
            if (ReferenceEquals(onHoverEnter,null)) return;
            onHoverEnter.Invoke();
        }

        if(!value)
        {
            if(ReferenceEquals(onHoverExit,null)) return;
            onHoverExit.Invoke();
        }
        
    }

    public void ActivateOnClap(float strength)
    {
      
        if(!activateOnClap) return;

        if (ReferenceEquals(onActivate, null)) return;

        if(strength > clapActivationThreshold)
        {
            onActivate.Invoke();
        }
    }

    public void DeactivateAfterSeconds(float seconds)
    {
        StartCoroutine(DeactivationRoutine(seconds));
    }

    IEnumerator DeactivationRoutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        onDeactivate.Invoke();

        yield break;
    }
    }

}


