using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetGrabLineVisual : MonoBehaviour
{
    [SerializeField] float lineLength = 100f;
    [SerializeField] Vector3 rayOffset;
    [SerializeField] Color normalColorStart;
    [SerializeField] Color normalColorEnd;
    [SerializeField] Color hoverColorStart;
    [SerializeField] Color hoverColorEnd;
    [SerializeField] Color selectColorStart;
    [SerializeField] Color selectColorEnd;
    
    

    LineRenderer lineRenderer;

    GameObject rayTarget = null;

    Vector3[] linePoints;

    bool hasTarget = false;
    bool isHovering = false;

    

    
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        linePoints = new Vector3[2];
        rayTarget = new GameObject("rayTarget");
    }

    private void Start() 
    {
        ResetRayTarget();
        SetColor(normalColorStart, normalColorEnd);  
    }

    

    void Update()
    {
        UpdateFirstLinePosition();

        UpdateSecondLinePosition();

        UpdateLine(linePoints);
    }

    public void ResetTarget()
    {
        SetColor(normalColorStart, normalColorEnd);
        ResetRayTarget();
        hasTarget = false;
        

    }

    public void SetTarget(GameObject currentTarget)
    {
        
        StartCoroutine(FindHitOnTarget(currentTarget));

        
        
    }

    IEnumerator  FindHitOnTarget(GameObject currentTarget)
    {
        bool hasHitTarget = false;
        while (!hasHitTarget)
        {
            RaycastHit hitInfo;
            
            Physics.Raycast(transform.position, transform.forward, out hitInfo);
            
            if (hitInfo.transform.gameObject == currentTarget)
                {
                    
                    rayTarget.transform.position = hitInfo.point;
                    
                    rayTarget.transform.SetParent(currentTarget.transform);
                    
                    hasTarget = true;
                    hasHitTarget = true;
                    yield return null;

                }

                yield return null;


            

        }
        
        

    }

  



    

  

    private void ResetRayTarget()
    {
        
        rayTarget.transform.parent = this.transform;
    }



    private void UpdateFirstLinePosition()
    {
        linePoints[0] = transform.position;
    }


    private void UpdateSecondLinePosition()
    {
        if (!hasTarget)
        {
            linePoints[1] = transform.position + transform.forward * lineLength;
        }

        if (hasTarget)
        {
            linePoints[1] = rayTarget.transform.position;
        }
    }


    private void UpdateLine(Vector3[] linePoints)
    {
        lineRenderer.SetPositions(linePoints);
    }


    

    

    private void SetColor(Color colorStart, Color colorEnd)
    {
        lineRenderer.startColor = colorStart;
        lineRenderer.endColor = colorEnd;
    }
}
