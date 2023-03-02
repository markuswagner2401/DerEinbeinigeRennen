using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAdjuster : MonoBehaviour
{
    [SerializeField] TransformAdjust[] adjusts;

    [System.Serializable]
    struct TransformAdjust
    {
        public string note;
        public int blendShapeIndex;
        public TransformMatrix transformMatrix;
    }

    [System.Serializable]
    struct TransformMatrix
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }

    [SerializeField] bool continuouslyAdjust = false;
    [SerializeField] Transform followTarget;

    [SerializeField] FollowAxis followAxis;

    [System.Serializable]
    struct FollowAxis
    {
        public bool followX;

        public float offsetX;
        public bool followY;

        public float offsetY;
        public bool followZ;

        public float offsetZ;
    }

    TransformMatrix nullMatrix;

    bool interrupted = false;
   
    void Start()
    {
        nullMatrix.position = transform.position;
        nullMatrix.scale = transform.localScale;
        nullMatrix.rotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        if(continuouslyAdjust)
        {
            StartCoroutine(FollowTargetRoutine());
        }
    }


    IEnumerator FollowTargetRoutine()
    {
        Vector3 offset = new Vector3();
        offset = followTarget.position - transform.position;
        Vector3 newPosition = new Vector3();
        while (continuouslyAdjust)
        {
            if(followAxis.followX)
            {
                newPosition.x = followTarget.position.x + offset.x + followAxis.offsetX;
            }

            if(followAxis.followY)
            {
                newPosition.y = followTarget.position.y + offset.y + followAxis.offsetY;
            }

            if(followAxis.followZ)
            {
                newPosition.z = followTarget.position.z + offset.z + followAxis.offsetZ;
            }

            transform.position = newPosition;

            yield return null;
        }
            
        

        yield break;
    }

   public void AdustMatrix(int blendShapeIndex, float blendTime, AnimationCurve curve)
   {
       foreach (var adjust in adjusts)
       {
           if (adjust.blendShapeIndex == blendShapeIndex)
           {

               // interrupt and move transform to adjust.matrix
               StartCoroutine(InterruptAndAdjustTransformR(blendTime, curve, adjust.transformMatrix));
               print("interrupt and adjust bsindex " + blendShapeIndex);
               return;
           }
       }
    
       // interrupt and move transfrom to adjust matrix
       StartCoroutine(InterruptAndAdjustTransformR(blendTime, curve, nullMatrix));
     
   }

   IEnumerator InterruptAndAdjustTransformR(float time, AnimationCurve curve, TransformMatrix transformMatrix)
   {
       interrupted = true;
       yield return new WaitForSeconds(0.1f);
       StartCoroutine(AdjustTransformR(time, curve, transformMatrix));
       yield break;
   }

   IEnumerator AdjustTransformR(float blendTime, AnimationCurve curve, TransformMatrix transformMatrix)
   {
     
       interrupted = false;

       Vector3 startPosition = new Vector3();
       startPosition = transform.position;
       Vector3 newPosition = new Vector3();

       Vector3 startRotation = new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
       Vector3 newRotation = new Vector3();

       Vector3 startScale = new Vector3();
       startScale = transform.localScale;
       Vector3 newScale = new Vector3();

       float timer = 0f;

       while (timer <= blendTime && !GetInterrupted())
       {
            timer += Time.deltaTime;
            
            float valueOnCurve = curve.Evaluate(timer / blendTime);

            

            newPosition = Vector3.Lerp(startPosition, transformMatrix.position, valueOnCurve);
            newRotation = Vector3.Lerp(startRotation, transformMatrix.rotation, valueOnCurve);
            newScale = Vector3.Lerp(startScale, transformMatrix.scale, valueOnCurve);

           
            
            transform.position = newPosition;
            transform.eulerAngles = newRotation;
            transform.localScale = newScale;

            yield return null;

            // if (CheckDistance(startPosition, transformMatrix.position))
            // {
            //     // transform position
                
             
            // }

            // if (CheckDistance(startRotation, transformMatrix.rotation))
            // {
            //     // transform rotation;
                
            // }

            // if (CheckDistance(startScale, transformMatrix.scale))
            // {
            //     // transform scale;
            //     transform.localScale = Vector3.Lerp(startScale, transformMatrix.scale, valueOnCurve);
            // }
            
       }

       yield break;
   }

   private bool CheckDistance(Vector3 A, Vector3 B)
   {
       if ((B-A).magnitude > 0.1)
       {
           return true;
       }

       else 
       {
           return false;
       }
   }

   private bool GetInterrupted()
   {
       return interrupted;
   }
    
    

}
