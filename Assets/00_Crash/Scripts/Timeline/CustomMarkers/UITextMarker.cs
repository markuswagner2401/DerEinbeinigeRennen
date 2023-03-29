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

        [SerializeField] private bool roleAware = true;

        [SerializeField] private Role role = Role.Rennfahrer;

        [Space(20)]
        [SerializeField] private bool retroactive = false;
        [SerializeField] private bool emitOnce = false;


        public PropertyName id => new PropertyName();

        public string Note => note;
        public string Text => text;
        public Color Color => color;
        public int Size => size;
        public FontStyles FontStyle => fontStyle;

        public bool RoleAware => roleAware;

        public Role Role => role;

        public NotificationFlags flags =>
        (retroactive ? NotificationFlags.Retroactive : default) | (emitOnce ? NotificationFlags.TriggerOnce : default);

    }

}
