using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.VRRigSpace
{
    public class ControlPerspective : MonoBehaviour
    {
        [SerializeField] Perspektivwechsler perspektivwechsler;

        [SerializeField] Transform transformOfObjectToFollow;

        [SerializeField] Transform transformOfPerspective;

        bool beeingVisited = false;

        [SerializeField] float smoothingOnBeeingVisited = 0.001f;

        [SerializeField] float smoothingOnNotBeeingVisited = 0.8f;

        float actualSmoothing;

        
        void Start()
        {

        }

        
        void Update()
        {
            beeingVisited = (perspektivwechsler.GetCurrentPerspectiveTransform() == transformOfPerspective);

            actualSmoothing = beeingVisited ? smoothingOnBeeingVisited : smoothingOnNotBeeingVisited;

            transform.position = Vector3.Lerp(transform.position, transformOfObjectToFollow.position, actualSmoothing);

            transform.rotation = Quaternion.Lerp(transform.rotation, transformOfObjectToFollow.rotation, actualSmoothing);

            


        }
    }

}

