using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    
    [SerializeField] VideoPlayer videoPlayer = null;
    //[SerializeField] MeshRenderer screen;
    //[SerializeField] Material videoMaterial;
    //[SerializeField] Material stopMaterial;
    [SerializeField] string triggerTag = "Player";
 
    void Start()
    {
        //screen.material = stopMaterial;

        
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
    }



    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == triggerTag)
        {
            print ("trigger");
            //videoPlayer.Play();
            // screen.material = videoMaterial;
            videoPlayer.Prepare();
            StartCoroutine(PlayWhenPrepared());

        }
    }

    IEnumerator PlayWhenPrepared()
    {
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }
        videoPlayer.Play();
        yield break;
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == triggerTag)
        {
            print("trigger exit");
            videoPlayer.Stop();
            // screen.material = stopMaterial;
        }
    }



    private void OnCollisionExit(Collision other) 
    {
        if(other.gameObject.tag == triggerTag)
        {
            print("trigger exit");
            videoPlayer.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
