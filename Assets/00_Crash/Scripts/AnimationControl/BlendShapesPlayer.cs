using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapesPlayer : MonoBehaviour
{




    [SerializeField] SkinnedMeshRenderer mesh = null;
    

    [SerializeField] bool randomShapeAtStart = false;

    [SerializeField] string setShapeAtStart = "";
    [SerializeField] RepeatMode repeatMode;
    [SerializeField] AnimationCurve autoplayAnimCurve;
    [SerializeField] float autoplayTime = 5f;
    [SerializeField] float autoplayRestTime = 3f;

    [SerializeField] TransformAdjuster transformAdjuster = null;
    [SerializeField] bool adjustTransforms = true;

    bool updateColliderAtEndOfChange = true;
    bool continuousUpdateCollider = false;
    [SerializeField] float colliderUpdateFequency = 0.1f;
    [SerializeField] ColliderUpdater colliderUpdater = null;
    [SerializeField] VideoUVAdjuster uvAdjuster = null;
    

    [SerializeField] bool resetAtStart = true;

    [SerializeField] int shapeAtStart = 0;

    bool timeRuns = true;

    [System.Serializable]
    public enum RepeatMode
    {
        repeat,
        playOnce,
        pingPong
    }

    int currentIndex;
    int lastIndex;
    int shapesCount;

    bool forward = true;
    bool end = false;
    bool interrupted = false;

    bool autoplayInterrupted = false;

    bool updateCollider = false;



    void Awake()
    {
        if (ReferenceEquals(mesh, null))
        {
            mesh = GetComponent<SkinnedMeshRenderer>();
            if (ReferenceEquals(mesh, null)) return;
        }

        // if(ReferenceEquals(transformSphereMatrix, null))
        // {
        //     transformSphereMatrix = GetComponent<TransformSphereMatrix>();
        // }


        shapesCount = mesh.sharedMesh.blendShapeCount;
        currentIndex = GetIndexOfBlendShapeName(setShapeAtStart);

    }

    private void Start() 
    {
        if(resetAtStart)
        {
            for (int i = 0; i < shapesCount; i++)
            {
                mesh.SetBlendShapeWeight(i,0);
            }
        }

        if(randomShapeAtStart){
            FadeInBlendShape(0.1f, AnimationCurve.Constant(0f,1f,1f), UnityEngine.Random.Range(0, shapesCount), -1f, -1f);
        }

        else {
            if(setShapeAtStart != ""){
            
            FadeInBlendShapeByName(0.01f, AnimationCurve.Constant(0f,1f,1f), setShapeAtStart);
            }
        }
    }

    private void Update()
    {
        // if(Input.GetMouseButtonDown(0))
        // {
        //     FadeInNextBlendShape(0.5f, defaultAnimCurve);
        // }
    }


    ///// autoplay (TODO: Move to Nlend Shapes Stage Master)

    public void AutoPlayShapes(bool value)
    {
        if (value)
        {
            autoplayInterrupted = false;
            StartCoroutine(AutoplayRoutine(autoplayTime, autoplayAnimCurve));
        }

        else
        {
            InterruptAutoplay();
        }
        
    }

    private void InterruptAutoplay()
    {
        autoplayInterrupted = true;
        
    }

    private bool GetAutoplayInterrupted()
    {
        return autoplayInterrupted;
    }

    IEnumerator AutoplayRoutine(float fadeInTime, AnimationCurve curve)
    {
        while (!GetAutoplayInterrupted())
        {
             FadeInNextBlendShape(fadeInTime, curve, 1f, 1f);
             yield return new WaitForSeconds(fadeInTime + autoplayRestTime);
        }

        yield break;
    }

    //////

    public void SetColliderUpdate(bool continuosUpdate, bool updateAtEnd)
    {
        continuousUpdateCollider = continuosUpdate;
        updateColliderAtEndOfChange = updateAtEnd;
    }


    public void FadeInNextBlendShape(float fadeInTime, AnimationCurve curve, float firstUVAdjust, float secondUVAdjust)
    {
        if (end || mesh == null) return;


        SetNextIndex();
        StartCoroutine(InterruptAndPlayNext(fadeInTime, curve, firstUVAdjust, secondUVAdjust));
    }

    public void RunTime(bool value)
    {
        timeRuns = value;
    }

    ////

    public void FadeInBlendShapeByName(float fadeInTime, AnimationCurve curve, string name, float firstUVvAdjust, float secondUVAdjust)
    {
        
        int index = GetIndexOfBlendShapeName(name);
        
        FadeInBlendShape(fadeInTime, curve, index, firstUVvAdjust, secondUVAdjust);
    }

    public void FadeInBlendShapeByName(float fadeInTime, AnimationCurve curve, string name){
        FadeInBlendShapeByName(fadeInTime, curve, name, -1, -1);
    }


    

    private int GetIndexOfBlendShapeName(string name)
    {
        for (int i = 0; i < shapesCount; i++)
        {
            if (mesh.sharedMesh.GetBlendShapeName(i) == name)
            {
                return i;
            }
        }
        Debug.Log("no blend shape with  name " + name + " found, returning 0");
        return 0;
    }

    public void FadeInBlendShape(float fadeInTime, AnimationCurve curve, int index, float firstUvAdjust, float secondUVAdjust)
    {
        //print("fade in blend shape " + gameObject.name);
        lastIndex = currentIndex;
        currentIndex = index;
        // print("play space State: " + currentIndex + "last Index: " + lastIndex);
        StartCoroutine(InterruptAndPlayNext(fadeInTime, curve, firstUvAdjust, secondUVAdjust));
    }

    

    

    



    IEnumerator InterruptAndPlayNext(float fadeInTime, AnimationCurve curve, float firstUvAdjust, float secondUVAdjust)
    {
        interrupted = true;
        yield return new WaitForSeconds(0.1f);
        interrupted = false;
        
        yield return WaitUntilTimeIsRunning();

        if(interrupted) yield break;
        


        

        if (adjustTransforms && !ReferenceEquals(transformAdjuster, null))
        {
            transformAdjuster.AdustMatrix(currentIndex, fadeInTime, curve);
        }

        
        
        StartCoroutine(FadeInBlendShapeR(currentIndex, fadeInTime, curve, firstUvAdjust, secondUVAdjust));
        
        for (int i = 0; i < mesh.sharedMesh.blendShapeCount; i++)
        {
            if(i == currentIndex) continue;
            if(mesh.GetBlendShapeWeight(i) == 0f) continue;
            StartCoroutine(FadeOutBlendShapeR(i, fadeInTime, curve));
        }

        // if(currentIndex != lastIndex)
        // {
        //     StartCoroutine(FadeOutBlendShapeR(lastIndex, fadeInTime, curve));
        // }
        
        yield break;
    }

    IEnumerator WaitUntilTimeIsRunning()
    {
        while (!timeRuns)
        {
            yield return null;
        }

        yield break;
    }



    IEnumerator FadeInBlendShapeR(int index, float fadeInTime, AnimationCurve curve, float firstScreenUvAdjust, float secondScreenUVAdjust)
    {
        //interrupted = false;
        float timer = 0f;
        float fadeInStartValue = mesh.GetBlendShapeWeight(index);
    
        float firstUvAdjustStartValue = 1f;
        float secondUvAdjustStartValue = 1f;
        if(!ReferenceEquals(uvAdjuster, null))
        {

            firstUvAdjustStartValue = uvAdjuster.GetUVxAdjust(0);
            secondUvAdjustStartValue = uvAdjuster.GetUVxAdjust(1);
        }

        


        while (timer <= fadeInTime && !interrupted)
        {
            if(timeRuns)
            {
                timer += Time.fixedDeltaTime;
            }
            
            float fadeValue = Mathf.Lerp(fadeInStartValue, 100, curve.Evaluate(timer / fadeInTime));

            mesh.SetBlendShapeWeight(index, fadeValue);

            if(continuousUpdateCollider && !ReferenceEquals(colliderUpdater, null))
            {
                updateCollider = true;
                StartCoroutine(UpdateColliderR());
                
            }

            if(!ReferenceEquals(uvAdjuster, null))
            {
                if(firstScreenUvAdjust > 0){
                    uvAdjuster.AdjustUVx(Mathf.Lerp(firstUvAdjustStartValue, firstScreenUvAdjust, fadeValue/100f), 0);
                }
                
                if(secondScreenUVAdjust > 0){
                    uvAdjuster.AdjustUVx(Mathf.Lerp(secondUvAdjustStartValue, secondScreenUVAdjust, fadeValue / 100f), 1);
                }
                
                
            }

            yield return new WaitForFixedUpdate();
        }

        if (continuousUpdateCollider && !ReferenceEquals(colliderUpdater, null))
        {
            updateCollider = false;
        }

        if (updateColliderAtEndOfChange && !ReferenceEquals(colliderUpdater, null))
        {
            colliderUpdater.UpdateCollider();
        }

        yield break;
    }

    IEnumerator UpdateColliderR() // TODO Fixen (die Framerate geht in den keller)
    {
       
        while (updateCollider)
        {
            
            colliderUpdater.UpdateCollider();
            yield return new WaitForSecondsRealtime(colliderUpdateFequency);

        }
        yield break;
    }

    IEnumerator FadeOutBlendShapeR(int index, float fadeOutTime, AnimationCurve curve)
    {
        float timer = 0;
        float fadeOutStartValue = mesh.GetBlendShapeWeight(index);
        while (timer <= fadeOutTime && !interrupted)
        {
            if(timeRuns)
            {
                timer += Time.fixedDeltaTime;
            }
            
            float fadeValue = Mathf.Lerp(fadeOutStartValue, 0f, curve.Evaluate(timer / fadeOutTime));
            mesh.SetBlendShapeWeight(index, fadeValue);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    

    private bool GetInterrupted()
    {
        return interrupted;
    }

    private void SetNextIndex()
    {
        lastIndex = Mathf.Clamp(currentIndex, 0, shapesCount - 1);

        if (forward)
        {
            currentIndex += 1;
        }

        else
        {
            currentIndex -= 1;
        }


        if (currentIndex >= shapesCount || currentIndex < 0)
        {
            if (repeatMode == RepeatMode.repeat)
            {
                currentIndex = 0;
            }

            if (repeatMode == RepeatMode.pingPong)
            {
                forward = !forward;
                SetNextIndex();
            }

            if (repeatMode == RepeatMode.playOnce)
            {
                end = true;
            }

        }




    }




}