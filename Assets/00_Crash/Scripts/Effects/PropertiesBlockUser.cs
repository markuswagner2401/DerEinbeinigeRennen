using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.MaterialControl
{
    public class PropertiesBlockUser : MonoBehaviour
    {
        [SerializeField] Renderer rend = null;

        MaterialPropertyBlock block = null;

        [SerializeField] MaterialPropertiesFader_2 materialPropertiesFader;
      
        void Start()
        {
            if(rend == null)
            {
                rend = GetComponent<Renderer>();
            }

            StartCoroutine(GetPropertyBlock());
        }

        
        void Update()
        {
            if(block != null && rend != null)
            {
                rend.SetPropertyBlock(block);
            }
            

        }

        public void SetPropertiesBlock(MaterialPropertyBlock otherBlock)
        {
            block = otherBlock;
        }

        //In Order to Work together with material properties fader and use the same block

        IEnumerator GetPropertyBlock()
        {
            if (materialPropertiesFader == null) yield break;
            while (block == null)
            {
                block = materialPropertiesFader.GetPropertyBlock();
                yield return null;
            }



            yield break;
        }
    }

}

