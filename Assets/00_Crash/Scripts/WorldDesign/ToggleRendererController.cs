using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TransformControl
{
    public class ToggleRendererController : MonoBehaviour
    {
        private void Start()
        {
            ToggleSecondChilds();
        }

        private void ToggleSecondChilds()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform firstChild = transform.GetChild(i);
                for (int j = 0; j < firstChild.childCount; j++)
                {
                    Transform secondChild = firstChild.GetChild(j);
                    secondChild.gameObject.SetActive(false);
                }
            }
        }
    }

}

