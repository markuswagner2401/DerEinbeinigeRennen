using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.OVRRigSpace
{
    public class TrackingSpaceOffset : MonoBehaviour
    {
        [SerializeField] float waitAfterStart = 3f;
        [SerializeField] Vector3 offset;
        void Start()
        {
            StartCoroutine(OffsetTrackingSpace(offset));
        }

        IEnumerator OffsetTrackingSpace(Vector3 offset)
        {
            yield return new WaitForSeconds(waitAfterStart);
            transform.position += offset;
            yield break;
        }
    }

}

