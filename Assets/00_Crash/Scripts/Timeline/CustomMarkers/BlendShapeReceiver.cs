using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using ObliqueSenastions.Animation;

namespace ObliqueSenastions.TimelineSpace
{

    public class BlendShapeReceiver : MonoBehaviour, INotificationReceiver
    {


        [SerializeField] PlayableDirector playableDirector;

        private List<double> markerTimes = new List<double>();

        private void Start()
        {
            // if(blendShapesStageMaster == null)
            // {
            //     blendShapesStageMaster = GameObject.FindGameObjectWithTag("StageMaster").GetComponent<BlendShapesStageMaster>();
            // }

            if (playableDirector == null)
            {
                playableDirector = GetComponent<PlayableDirector>();
            }

            CreateListOfMarkerTimes();

            // foreach (var item in markerTimes)
            // {
            //     print("markerTime " + item);
            // }
        }
        public void OnNotify(Playable origin, INotification notification, object context)
        {

            if (notification is BlendShapeMarker blendShapeMarker && FindTribunenPlayer() != null)
            {

                TribuenenPlayer tribunePlayer = FindTribunenPlayer();

                if (blendShapeMarker.ChangeDuration() && !blendShapeMarker.ChangeDurationToNextMarker())
                {
                    tribunePlayer.ChangeDuration(blendShapeMarker.SpaceStateIndex(), blendShapeMarker.Duration());
                }

                if (blendShapeMarker.ChangeCurve())
                {
                    tribunePlayer.ChangeCurve(blendShapeMarker.SpaceStateIndex(), blendShapeMarker.Curve());
                }

                if (blendShapeMarker.ChangeDurationToNextMarker())
                {
                    for (int i = 0; i < (markerTimes.Count - 1); i++)
                    {
                        if (Mathf.Abs((float)markerTimes[i] - (float)blendShapeMarker.time) < 0.001f)
                        {
                            //print("set duration to next marker: " + (float)(markerTimes[i+1] - markerTimes[i]));
                            tribunePlayer.ChangeDuration(blendShapeMarker.SpaceStateIndex(), (float)(markerTimes[i + 1] - markerTimes[i]));
                        }
                    }
                }

                tribunePlayer.ChangeTribunesBlendShapes(blendShapeMarker.SpaceStateIndex());


            }


        }

        private TribuenenPlayer FindTribunenPlayer()
        {
            GameObject tribunePlayerGO = GameObject.FindGameObjectWithTag("TribuenenPlayer");

            if (tribunePlayerGO == null) return null;

            return tribunePlayerGO.GetComponent<TribuenenPlayer>();
        }

        private void CreateListOfMarkerTimes()
        {

            var timelineAsset = playableDirector.playableAsset as TimelineAsset;

            var tracks = timelineAsset.GetOutputTracks();





            foreach (var track in tracks)
            {
                if (track is BlendShapeTrack)
                {

                    var _blendShapeMarkers = track.GetMarkers();


                    foreach (var marker in _blendShapeMarkers)
                    {


                        if (marker is BlendShapeMarker)
                        {
                            markerTimes.Add(marker.time);
                        }
                    }

                    markerTimes.Sort();
                }
            }




        }



    }

}
