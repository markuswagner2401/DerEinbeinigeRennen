using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TransformControl
{

    public class TachonadelActivator : MonoBehaviour
    {
        [SerializeField] GameObject[] tachonadeln;


        public void ShowTachonadeln(bool value)
        {
            foreach (var item in tachonadeln)
            {
                item.SetActive(value);
            }
        }
    }

}
