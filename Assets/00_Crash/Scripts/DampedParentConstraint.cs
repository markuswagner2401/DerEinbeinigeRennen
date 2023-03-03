using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.TransformControl
{

    public class DampedParentConstraint : MonoBehaviour
    {

        [SerializeField] Transform sourceObject;
        [SerializeField] float smoothing = 1f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.Slerp(transform.position, sourceObject.transform.position, smoothing);
            transform.rotation = Quaternion.Slerp(transform.rotation, sourceObject.transform.rotation, smoothing);

        }
    }

}
