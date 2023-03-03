using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Animation
{

    public class BodyPartsTriggerer : MonoBehaviour
    {
        [SerializeField] BodyParsHandler bodyPartsHandler;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Traveller")
            {
                bodyPartsHandler.ZoneTriggerBodyPart();
            }

        }

    }


}
