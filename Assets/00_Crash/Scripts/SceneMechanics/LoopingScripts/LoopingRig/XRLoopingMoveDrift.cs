using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRLoopingMoveDrift : MonoBehaviour
{

    [SerializeField] bool drift = false;
    

    //[SerializeField] float capturedYRotationAtStart;

    XRLoopingMover loopingMover;

    [SerializeField] float speed;

    [SerializeField] DriftChanger[] driftChangers;

    [System.Serializable]
    public struct DriftChanger
    {
        public string note;
        public float targetValue;
        public float duration;
        public AnimationCurve curve;
    }

    [SerializeField] bool readAtTimeline = false;
    [SerializeField] string refAtTimeline;

    FloatControlByTimeline floatControlByTimeline = null;



    bool interrupted;

    private void Awake() {
        //capturedYRotationAtStart = transform.eulerAngles.y;
    }
    
    void Start()
    {
        loopingMover = GetComponent<XRLoopingMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if(readAtTimeline)
        {
            if(ReferenceEquals(floatControlByTimeline,null))
            {
                Debug.Log("XRLoopingMoveDrift: Get FloatControlByTimeline");
                floatControlByTimeline = TimeLineHandler.instance.GetComponent<FloatControlByTimeline>();
                return;
            }

            speed = floatControlByTimeline.GetValue(refAtTimeline);

         
        }

        if(!drift || speed == 0) return;
        

        Drift(speed);
    }

    public void ChangeDrift(int index){
        StartCoroutine(InterruptAndChangeDrift(index));
    }

    IEnumerator InterruptAndChangeDrift(int index){
        interrupted = true;
        yield return new WaitForSeconds(0.01f);
        interrupted = false;
        StartCoroutine(ChangeDriftRoutine(index));
        yield break;
    }

    IEnumerator ChangeDriftRoutine(int index){
        float timer = 0f;
        float startValue = speed;
        while (timer < driftChangers[index].duration && !interrupted)
        {
            timer += Time.unscaledDeltaTime;
            float newValue = Mathf.Lerp(startValue, driftChangers[index].targetValue, driftChangers[index].curve.Evaluate(timer / driftChangers[index].duration));
            SetSpeed(newValue);
            yield return null;
        }
        yield break;
    }

    public void SetDrift(bool value){
        drift = value;
    }

    public void SetSpeed(float newSpeed){
        speed = newSpeed;
    }


    public void Drift(float speed){
        loopingMover.Move(transform.forward * speed);
        
        // loopingMover.Move();
    }
}
