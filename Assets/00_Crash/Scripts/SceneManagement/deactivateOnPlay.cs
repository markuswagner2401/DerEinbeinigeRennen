using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TransformControl
{

    public class deactivateOnPlay : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
