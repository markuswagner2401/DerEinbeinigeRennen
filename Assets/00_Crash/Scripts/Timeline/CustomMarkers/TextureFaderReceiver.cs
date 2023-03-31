using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using ObliqueSenastions.Animation;

namespace ObliqueSenastions.TimelineSpace
{

    public class TextureFaderReceiver : MonoBehaviour, INotificationReceiver
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

            if (notification is TextureFaderMarker textureFaderMarker && FindTribunenPlayer() != null)
            {

                TribuenenPlayer tribunePlayer = FindTribunenPlayer();

                if (textureFaderMarker.ChangeDurationToNextMarker())
                {
                    for (int i = 0; i < (markerTimes.Count - 1); i++)
                    {
                        if (Mathf.Abs((float)markerTimes[i] - (float)textureFaderMarker.time) < 0.001f)
                        {
                            //print("set duration to next marker: " + (float)(markerTimes[i+1] - markerTimes[i]));
                            tribunePlayer.ChangeTSDuration(textureFaderMarker.TextureChangerIndex(), (float)(markerTimes[i + 1] - markerTimes[i]));
                        }
                    }
                }

                else if (textureFaderMarker.ChangeDuration())
                {
                    tribunePlayer.ChangeTSDuration(textureFaderMarker.TextureChangerIndex(), textureFaderMarker.Duration());
                }

                if (textureFaderMarker.ChangeCurve())
                {
                    tribunePlayer.ChangeTSCurve(textureFaderMarker.TextureChangerIndex(), textureFaderMarker.Curve());
                }



                tribunePlayer.ChangeTribunesTextures(textureFaderMarker.TextureChangerIndex());


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
                if (track is TextureFaderTrack)
                {

                    var _textureFaderMarkers = track.GetMarkers();


                    foreach (var marker in _textureFaderMarkers)
                    {


                        if (marker is TextureFaderMarker)
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
