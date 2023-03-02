using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

public class VRTravellerMarkerReceiver : MonoBehaviour, INotificationReceiver
{
    
    [SerializeField] PlayableDirector playableDirector = null;

    

    List<float> markerTimes = new List<float>();

    private void Start() 
    {
        

        if(playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        CreateMarkerTimeList();
    }

    

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if(notification is VRTravellerMarker vRTravellerMarker)
        {
            StageMaster stageMaster = GameObject.FindGameObjectWithTag("StageMaster").GetComponent<StageMaster>();

            if(stageMaster == null) return;

            float duration;

            bool changeTransitionDuration = vRTravellerMarker.ChangeTransitionDuration;

            duration = vRTravellerMarker.TransitionDuration;


            if(vRTravellerMarker.SetDurationToNextMarker)
            {
                for (int i = 0; i < markerTimes.Count - 1; i++)
                {
                    if( Mathf.Abs((float)vRTravellerMarker.time - markerTimes[i]) < 0.001f )
                    {
                        duration = markerTimes[i+1] - markerTimes[i];
                        changeTransitionDuration = true;
                    }
                }
            }

            if(vRTravellerMarker.Note == ""){
                stageMaster.SetVRRig(vRTravellerMarker.TransitionPointIndex, 
                                    changeTransitionDuration, 
                                    duration, 
                                    vRTravellerMarker.ChangeTransitionCurve, 
                                    vRTravellerMarker.TransitionCurve );
            }

            else{
                stageMaster.SetVRRig(vRTravellerMarker.Note, 
                                    changeTransitionDuration, 
                                    duration, 
                                    vRTravellerMarker.ChangeTransitionCurve, 
                                    vRTravellerMarker.TransitionCurve );
            }


            
            
            
        }
    }

    

    private void CreateMarkerTimeList()
    {
        var timelineAsset = playableDirector.playableAsset as TimelineAsset;
        
        var tracks = timelineAsset.GetOutputTracks();

        foreach (var track in tracks)
        {
            if(track is VRTravellerTrack)
            {
                var markers = track.GetMarkers();

                int index = -1;

                foreach (var marker in markers)
                {
                    if(marker is VRTravellerMarker)
                    {
                        index += 1;
                        markerTimes.Add((float)marker.time);
                        
                    }
                }
                
                
                markerTimes.Sort();

                
                
            }
        }
    }
}
