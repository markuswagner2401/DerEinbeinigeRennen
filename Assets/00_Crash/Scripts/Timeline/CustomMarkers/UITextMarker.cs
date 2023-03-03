using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ObliqueSenastions.TimelineSpace
{

    public class UITextMarker : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField] private string note;
        [SerializeField] private string text;
        [SerializeField] private Color color;
        [SerializeField] private int size;

        [SerializeField] private FontStyles fontStyle;

        [Space(20)]
        [SerializeField] private bool retroactive = false;
        [SerializeField] private bool emitOnce = false;


        public PropertyName id => new PropertyName();

        public string Note => note;
        public string Text => text;
        public Color Color => color;
        public int Size => size;
        public FontStyles FontStyle => fontStyle;

        public NotificationFlags flags =>
        (retroactive ? NotificationFlags.Retroactive : default) | (emitOnce ? NotificationFlags.TriggerOnce : default);

    }

}
