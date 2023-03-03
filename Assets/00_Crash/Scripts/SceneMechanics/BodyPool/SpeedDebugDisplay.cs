using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Debugging
{

    public class SpeedDebugDisplay : MonoBehaviour
    {


        [SerializeField] float factor = 1f;

        Vector3 displayPosition = new Vector3();


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = displayPosition;
        }

        public void DisplaySpeed(float speed)
        {

            displayPosition.y = speed * factor;
        }
    }

}
