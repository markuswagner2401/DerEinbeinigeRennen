using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GoEventMarkerReceiver : MonoBehaviour, INotificationReceiver
{
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is GoEventMarker)
        {
            GoEventMarker marker = notification as GoEventMarker;

            StageMaster stageMaster = null;

            GameObject stageMaster_go = GameObject.FindGameObjectWithTag("StageMaster");

            

            if(stageMaster_go != null)
            {
                stageMaster = GameObject.FindGameObjectWithTag("StageMaster").GetComponent<StageMaster>();
            }

            if(stageMaster != null)
            {
                stageMaster.PlayGoEvent(marker.GoEventName());
            }

         
        }
    }
}
