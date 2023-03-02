using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatControlByTimelineTestGetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TimeLineHandler.instance.GetComponent<FloatControlByTimeline>().SetStartValue(0, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0f,TimeLineHandler.instance.GetComponent<FloatControlByTimeline>().GetValue(0), 0f) ;
    }
}
