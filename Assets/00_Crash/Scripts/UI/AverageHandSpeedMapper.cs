using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;

namespace ObliqueSenastions.VRRigSpace
{
    public class AverageHandSpeedMapper : MonoBehaviour, IVelocityListener
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

        

        public float GetOutputValueNormaized(bool mappedOnCurve)
        {
            if(simpleVelocityTrackers.Count <= 0) return 0;
            float speedSum = 0;
            foreach (var item in simpleVelocityTrackers)
            {
                speedSum += item.GetLocalSpeed();
            }

            float averageSpeed = speedSum / simpleVelocityTrackers.Count;

            return mappedOnCurve ? MapSpeed(averageSpeed) : speedSum / simpleVelocityTrackers.Count;
        }

        private float MapSpeed(float t)
        {
            float speedNorm = Mathf.InverseLerp(mapMinSpeed, mapMaxSpeed, t);
            return mappingCurve.Evaluate(speedNorm);

        }


        //// IVelocityListener

        public void AddVelocityContributor(SimpleVelocityTracker[] velocityContributors) // gets called by network player on setup avatar
        {
            foreach (var item in velocityContributors)
            {
                AddTracker(item);
            }
        }


    }

}



