using System;
using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.TimelineSpace;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.Looping
{


    public class XRLoopingMover : MonoBehaviour
    {
        CapsuleCollider capsule;
        [SerializeField] XROrigin rig = null;

        [SerializeField] bool usingOVR = false;

        [Tooltip("if using ovr")]
        [SerializeField] Transform ovrHead;

 

        [SerializeField] bool useTimelineTime = false;


        [SerializeField] float correctForward = 0f;
        [SerializeField] float racastStartpointOverGround = 2f;

        // [SerializeField] bool raycastFromDown = false;
        [SerializeField] float smoothing = 0.1f;
        [SerializeField] float teleportTime = 0.5f;
        [SerializeField] float heightOverHead = 0.2f;
        [SerializeField] float maxRaycastDistance = 30f;
        [SerializeField] float spherecastRadius = 0.2f;

        [SerializeField] LayerMask groundLayer;
        [SerializeField] string groundTag = "Ground";
        [SerializeField] string absturzsicherungTag = "Absturzsicherung";
        [SerializeField] bool continuousGrounding = true;

        

        // enum BehaviorOnLostGround
        // {
        //     rise,
        //     fall
        // }

        // [SerializeField] BehaviorOnLostGround behaviorOnLostGround = BehaviorOnLostGround.rise;
        // bool triggerGround = false;
        // bool alreadyGround = false;


        [SerializeField] Transform teleportationAim = null;

        bool hitMargin = false;




        [SerializeField] float fallingSpeed = -1f; // TODO: Implement acceleration of gravity
        float my_rotation = 0f;

        bool isTeleporting = false;

        RaycastHit currentHit;

        [SerializeField] bool teleportEnabled = true; // only false for training Mode


        TimelineTime timelineTime = null;


        void Start()
        {
            capsule = GetComponent<CapsuleCollider>();

            if (!usingOVR)
            {
                rig = GetComponent<XROrigin>();
            }




            if (timelineTime == null)
            {
                TimeLineHandler timeLineHandler = TimeLineHandler.instance;
                if(timeLineHandler != null)
                {
                    timelineTime = timeLineHandler.GetComponent<TimelineTime>();
                }
                
            }

            

            // triggerGround = true;

        }




        void FixedUpdate()
        {
            // if (usingOVR && oVRCameraRig!= null)
            // {
            //     OVRInput.Update();
                
            
                
            // }

            UpdateTransform();


        }

        

        

        void UpdateTransform()
        {
            float headHeight = usingOVR ? ovrHead.localPosition.y : rig.CameraInOriginSpaceHeight;

            Vector3 headPosition = usingOVR ? ovrHead.position : rig.Camera.transform.position;


            CapsuleFollowHeadset(headHeight, headPosition);

            if(!continuousGrounding) return;

            UpdateHitInfo();

            //currentHit = GetHitInfo();

            // rotate to normal



            transform.up = Vector3.Lerp(transform.up, currentHit.normal, smoothing);

            //correct forward

            transform.rotation *= Quaternion.Euler(0, correctForward, 0);

            //apply my rotation

            transform.rotation *= Quaternion.Euler(0, my_rotation, 0);



            Grounding();

        }

        private void Grounding()
        {
            // if (isTeleporting || triggerGround) return;

            if (continuousGrounding)
            {
                float distanceFromGround = GetDistanceFromGround();
                //print("dist from ground" + distanceFromGround);
                //if (Mathf.Abs(distanceFromGround - heightOverGround) < 0.01f) return;

                if (distanceFromGround > racastStartpointOverGround) // Fall
                {
                    transform.position += transform.up * -Mathf.Abs(distanceFromGround - racastStartpointOverGround); // fallingSpeed * Time.deltaTime;Mathf.Abs(distanceFromGround-heightOverGround)
                                                                                                                      //print("fall");
                }

                else // Rise
                {
                    transform.position -= transform.up * -Mathf.Abs(distanceFromGround - racastStartpointOverGround); // fallingSpeed * Time.deltaTime;
                                                                                                                      //print("rise");
                }




            }


        }

        private void UpdateHitInfo()
        {

            Vector3 rayStart = transform.position + (transform.up * racastStartpointOverGround);
            // bool hasHit;
            RaycastHit[] hits = Physics.SphereCastAll(rayStart, spherecastRadius, -transform.up, maxRaycastDistance, groundLayer);

            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.tag == groundTag)
                {
                    currentHit = hit;
//                    print("has hit");
                    return;
                }

            }


            // hasHit = Physics.SphereCast(rayStart, spherecastRadius, -transform.up, out RaycastHit hitInfo);

            // if (!hasHit || hitInfo.transform.tag != groundTag) return;
            // currentHit = hitInfo;

        }

        public RaycastHit GetCurrentGroundInfo()
        {
            return currentHit;
        }

        public void Move(Vector3 velocity)
        {
//            print("move : " + velocity);
            if (useTimelineTime && timelineTime != null)
            {
                //transform.position += velocity * timelineTime.GetTimelineDeltaTime() * 72f;
                transform.position += velocity * timelineTime.GetModeDependentTimelineDeltaTime() * 72f;
            }

            else
            {
                transform.position += velocity * (Time.deltaTime * 72f);
            }

        }

        public void Rotate(float amount)
        {
            my_rotation += amount;
        }

        public void EnableTeleport(bool value) // for training Mode
        {
            teleportEnabled = value;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == absturzsicherungTag)
            {
                print("trigger Absturzsicherung");
            }
        }

        public void HitMargin(bool value)
        {
            hitMargin = value;
        }

        public void Teleport(Vector3 destination, Vector3 upVector)
        {
            StopAllCoroutines();
            if (!teleportEnabled) return;

            print("teleport to contact point");

            teleportationAim.position = destination;
            teleportationAim.up = upVector;
            teleportationAim.localPosition = new Vector3(teleportationAim.localPosition.x, teleportationAim.localPosition.y + racastStartpointOverGround, teleportationAim.localPosition.z);
            StartCoroutine(Teleportation(teleportationAim));

        }

        private IEnumerator Teleportation(Transform currentTeleportationAim)
        {
            isTeleporting = true;
            float timer = 0f;

            while (timer <= teleportTime && !hitMargin)
            {
                //teleportationProcess = Mathf.Lerp(teleportationProcess, 1, teleportTime);
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, currentTeleportationAim.position, timer / teleportTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, currentTeleportationAim.rotation, timer / teleportTime);
                yield return null;
            }

            hitMargin = false;

            isTeleporting = false;
            yield break;
        }



        void CapsuleFollowHeadset(float headHeight, Vector3 headPosition)
        {
            capsule.height = headHeight + heightOverHead;
            Vector3 capsuleCenter = transform.InverseTransformPoint(headPosition);
            capsule.center = new Vector3(capsuleCenter.x, capsule.height / 2, capsuleCenter.z);
        }

        // private RaycastHit GetHitInfo()
        // {
        //     // Vector3 rayStart = transform.TransformPoint(capsule.center);
        //     Vector3 rayStart = transform.TransformPoint(rig.Camera.transform.position);
        //     Physics.SphereCast(rayStart, 0.2f, -transform.up, out RaycastHit hitInfo);
        //     if (hitInfo.transform.tag != groundTag) return currentHit;
        //     return hitInfo;
        // }

        private float GetDistanceFromGround()
        {

            return currentHit.distance;
        }

        private void OnDrawGizmos()
        {

            Vector3 rayStart = transform.position + (transform.up * racastStartpointOverGround);
            // bool hasHit;


            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(rayStart, currentHit.point);

            Gizmos.DrawSphere(currentHit.point, .05f);
            Gizmos.DrawSphere(rayStart, .05f);

        }


    }

}
