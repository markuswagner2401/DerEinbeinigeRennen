using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AnimationTag : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] string animationParameterName = null;

    public void SetAnimationParameter(bool value)
    {
        if (animator == null || animationParameterName == null) return;
        
        animator.SetBool(animationParameterName, value);


    }
    
}
