using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TimelineSpace
{
    public class Absturzbremsung : MonoBehaviour
    {

        private void OnCollisionEnter(Collision other)
        {
            TimeLineHandler.instance.GetComponent<TimeModeMachine>().PlayAccident("Absturzsicherung");
        }
    }

}


