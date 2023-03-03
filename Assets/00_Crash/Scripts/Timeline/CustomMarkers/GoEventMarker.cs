using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{


    public class GoEventMarker : Marker, INotification, INotificationOptionProvider
    {


        [SerializeField] string goEventName = " ";




        [Space(20f)]
        [SerializeField] bool retroactive = false;
        [SerializeField] bool emitOnce = false;
        public PropertyName id => new PropertyName();


        public string GoEventName() { return goEventName; }





        public NotificationFlags flags => (retroactive ? NotificationFlags.Retroactive : default) |
                                          (emitOnce ? NotificationFlags.TriggerOnce : default);

    }

}
