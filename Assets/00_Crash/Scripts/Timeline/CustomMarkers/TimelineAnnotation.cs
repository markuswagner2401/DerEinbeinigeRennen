using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CustomStyle("AnnotationStyle")]
public class TimelineAnnotation : Marker
{
    public string title;
    public Color color;
    public string description;
    public bool showLineOverlay;

    public string Title => title;
}
