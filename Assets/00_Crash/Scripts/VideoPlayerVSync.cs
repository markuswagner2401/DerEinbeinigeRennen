using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerVSync : MonoBehaviour
{
    VideoPlayer videoPlayer;
    
   
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Update()
    {
        videoPlayer.Pause();
        if (videoPlayer.frame / videoPlayer.frameRate < Time.time)
        {
            videoPlayer.StepForward();
        }
    }

}
