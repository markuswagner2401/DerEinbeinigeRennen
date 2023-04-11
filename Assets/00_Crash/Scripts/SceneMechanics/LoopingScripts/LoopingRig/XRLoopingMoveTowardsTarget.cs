using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Looping
{
    public class XRLoopingMoveTowardsTarget : MonoBehaviour
    {
        [SerializeField] bool moveTowardsTarget = false;

        [SerializeField] float distanceToTarget = 2f;

        [SerializeField] ToTargetDirection toTargetDirection = null;

        [SerializeField] Transform target = null;

        [Tooltip("if true, direction to target is parallel to floor - > ToTargetDirection is used, else, taget is used")]
        [SerializeField] bool stayOnGround = true;



        [SerializeField] float speedFactorMoveTowardTarget = 0.01f;

        [SerializeField] XRLoopingMover loopingMover = null;
        

        void Start()
        {
            if(loopingMover == null)
            {
                loopingMover = GetComponent<XRLoopingMover>();
            }

        }


        void FixedUpdate()
        {
            MoveTowardsTarget();
        }

        public void EnableMoveTowardsTarget(bool value)
        {
            moveTowardsTarget = value;
        }


        void MoveTowardsTarget()
        {
            if (!moveTowardsTarget || ReferenceEquals(target, null)) return;

            if (Vector3.Distance(transform.position, target.position) < distanceToTarget) return;

            Vector3 direction = new Vector3();
            if (stayOnGround)
            {
                if (ReferenceEquals(toTargetDirection, null)) return;
                direction = toTargetDirection.GetParallelForward();

            }

            else
            {
                direction = (target.position - transform.position);
            }
            

            loopingMover.Move(direction * Time.fixedDeltaTime * speedFactorMoveTowardTarget);

        }
    }


}
