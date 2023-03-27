using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;
using Photon.Pun;
using ObliqueSenastions.ClapSpace;

namespace ObliqueSenastions.VRRigSpace
{
    public class AverageHandSpeedMapper : MonoBehaviour, IVelocityListener
    {
        

        [SerializeField] List<SimpleVelocityTracker> simpleVelocityTrackersMultiplayer;

        [SerializeField] List<SimpleVelocityTracker> simpleVelocityTrackersSingleplayer;

        [SerializeField] float mapMinSpeed = 0f;

        [SerializeField] float mapMaxSpeed = 1f;

        [SerializeField] AnimationCurve mappingCurve;

        float mappedSpeed;

        [SerializeField] bool useNetworkPlayer = false;

       


        
        public void AddTracker(SimpleVelocityTracker newTracker)
        {
            simpleVelocityTrackersMultiplayer.Add(newTracker);
        }

        public void SetUseNetworkPlayer(bool value) /// gets set by syn component
        {
            useNetworkPlayer = value;
        }

        

        public float GetOutputValueNormaized(bool mappedOnCurve)
        {
            List<SimpleVelocityTracker> trackerlist = (PhotonNetwork.IsConnected && useNetworkPlayer) ? simpleVelocityTrackersMultiplayer : simpleVelocityTrackersSingleplayer;
            if(trackerlist.Count <= 0) return 0;
            float speedSum = 0;
            foreach (var item in trackerlist)
            {
                speedSum += item.GetLocalSpeed();
            }

            if(trackerlist.Count == 0) return 0;

            float averageSpeed = speedSum / trackerlist.Count;

            return mappedOnCurve ? MapSpeed(averageSpeed) : speedSum / trackerlist.Count;
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

        public void AddLoadingBarContributor(LoadingBar[] loadingBarContributor)
        {
            
        }
    }

}



