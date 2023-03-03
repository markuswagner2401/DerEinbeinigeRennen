using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;
using UnityEngine.Playables;

namespace ObliqueSenastions.TimelineSpace
{

    public class UITextReceiver : MonoBehaviour, INotificationReceiver
    {

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is UITextMarker uITextMarker)
            {
                GameObject stageMaster_go = GameObject.FindGameObjectWithTag("StageMaster");
                UITextStreamer textStreamer = null;
                if (stageMaster_go != null)
                {
                    textStreamer = stageMaster_go.GetComponent<UITextStreamer>();
                }

                if (textStreamer == null) return;
                textStreamer.ShowText(uITextMarker.Text, uITextMarker.Color, uITextMarker.FontStyle, uITextMarker.Size);
                //textStreamer.ShowText(uITextMarker.Text);

            }
        }
    }

}
