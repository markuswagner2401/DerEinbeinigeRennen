using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ObliqueSenastions.VRRigSpace
{
    public class AverageHandSpeedMapper : MonoBehaviour
    {
        

        [SerializeField] List<SimpleVelocityTracker> simpleVelocityTrackers;

        [SerializeField] float mapMinSpeed = 0f;

        [SerializeField] float mapMaxSpeed = 1f;

        [SerializeField] AnimationCurve mappingCurve;

        float mappedSpeed;

       


        
        public void AddTracker(SimpleVelocityTracker newTracker)
        {
            simpleVelocityTrackers.Add(newTracker);
        }

        

        public float GetAverageSpeed(bool mapped)
        {
            float speedSum = 0;
            foreach (var item in simpleVelocityTrackers)
            {
                speedSum += item.GetLocalSpeed();
            }

            float averageSpeed = speedSum / simpleVelocityTrackers.Count;

            return mapped ? MapSpeed(averageSpeed) : speedSum / simpleVelocityTrackers.Count;
        }

        private float MapSpeed(float t)
        {
            float speedNorm = Mathf.InverseLerp(mapMinSpeed, mapMaxSpeed, t);
            return mappingCurve.Evaluate(speedNorm);

        }
    }

}



