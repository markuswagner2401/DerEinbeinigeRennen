using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoPlayWhenPrepared : MonoBehaviour
{
    VideoPlayer videoPlayer;

    bool isPlaying = false;

    

    



    
   
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.Prepare();
        

        
    }

  
    void Update()
    {
        
        
        if (isPlaying) return;

        if(videoPlayer.isPrepared)
        {
//            print("is prepared");
            videoPlayer.Play();
            isPlaying = true;
        }

        
    }

    void ChangePosition()
    {
        print("hello");
    }
}
