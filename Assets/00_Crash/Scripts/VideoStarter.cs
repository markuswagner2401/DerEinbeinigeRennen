using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace ObliqueSenastions.VideoControl
{

    public class VideoStarter : MonoBehaviour
    {
        [SerializeField] VideoPlayer videoPlayer;

        [SerializeField] Transform cinemaPosition;

        [SerializeField] Transform vrRig;







        // Start is called before the first frame update
        void Start()
        {



        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                print("collision");
                videoPlayer.Play();
                vrRig.transform.position = cinemaPosition.position;
            }


        }



        // Update is called once per frame
        void Update()
        {

        }
    }

}
