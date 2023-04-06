using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.PunNetworking
{
    public class ScoringByDistance : MonoBehaviour
    {

        [SerializeField] RacerIdentity racerIdentity = null;

        [SerializeField] float distanceForOnePoint = 10f;

        [SerializeField] int points = 0;

        int lastPoints;

        float measuredTotalDistance = 0;

        Vector3 lastPosition = new Vector3();


        private void Start() 
        {
            if(racerIdentity == null)
            {
                racerIdentity = GetComponent<RacerIdentity>();
            }

            lastPoints = points;

            lastPosition = transform.position;
        }

        private void FixedUpdate() 
        {
            measuredTotalDistance += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;

            points = (int)Mathf.Floor(measuredTotalDistance / distanceForOnePoint);

            if(points != lastPoints)
            {
                racerIdentity?.AddScore();
            }

            lastPoints = points;
        }
    }

}

