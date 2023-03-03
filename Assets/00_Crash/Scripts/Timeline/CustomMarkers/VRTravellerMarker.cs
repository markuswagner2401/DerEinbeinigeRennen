using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{

    public class VRTravellerMarker : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField] private int transitionPointIndex = 0;

        [SerializeField] private string note;

        [SerializeField] private bool setDurationToNextMarker = false;
        [SerializeField] private bool changeTransitionDuration = false;
        [SerializeField] private float transitionDuration = 10f;

        [SerializeField] private bool changeTransitionCurve = false;

        [SerializeField] private AnimationCurve transitionCurve;

        [Space(20)]
        [SerializeField] private bool retroactive;
        [SerializeField] private bool emitOnce;

        public PropertyName id => new PropertyName();

        public int TransitionPointIndex => transitionPointIndex;

        public bool ChangeTransitionDuration => changeTransitionDuration;

        public float TransitionDuration => transitionDuration;

        public bool ChangeTransitionCurve => changeTransitionCurve;

        public AnimationCurve TransitionCurve => transitionCurve;

        public bool SetDurationToNextMarker => setDurationToNextMarker;

        public string Note => note;

        public NotificationFlags flags =>
        (retroactive ? NotificationFlags.Retroactive : default) |
        (emitOnce ? NotificationFlags.TriggerOnce : default);
    }

}
