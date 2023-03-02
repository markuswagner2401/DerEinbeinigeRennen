using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BlendShapeMarker : Marker, INotification, INotificationOptionProvider, IMarker
{
    [SerializeField] string note = "";

    [SerializeField] int spaceStateIndex = 0;

    public bool changeDurationToNextMarker = false;
    [SerializeField] bool changeDuration = false;
    [SerializeField] float duration = 10f;
    [SerializeField] bool changeCurve = false;
    [SerializeField] AnimationCurve curve;

    [Space(20)]
    [SerializeField] bool retroactive = false;
    [SerializeField] bool emitOnce = false;

    
    public PropertyName id { get { return new PropertyName(); } }

    public NotificationFlags flags => 
        (retroactive ? NotificationFlags.Retroactive : default) |
        (emitOnce ? NotificationFlags.TriggerOnce : default);

    public int SpaceStateIndex() { return spaceStateIndex; }

    public bool ChangeDuration() { return changeDuration; }
    public float Duration() { return duration; }

    public bool ChangeCurve() { return changeCurve; }

    public bool ChangeDurationToNextMarker() { return changeDurationToNextMarker; }

    public AnimationCurve Curve() { return curve; }



}
