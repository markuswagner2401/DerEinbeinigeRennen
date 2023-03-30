using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ObliqueSenastions.Animation
{
    public class SetVFXParameter : MonoBehaviour
    {
        [SerializeField] VisualEffect visualEffect;

        [SerializeField] string parameterName;

        [SerializeField] float onValue = 1f;

        [SerializeField] float offValue = 0;

        public void PlayVFx(bool value)
        {
            if(value)
            {
                visualEffect.SetFloat(parameterName, onValue);
            }

            else
            {
                visualEffect.SetFloat(parameterName, offValue);
            }
        }



    }

}


