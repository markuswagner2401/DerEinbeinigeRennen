using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObliqueSenastions.ClapSpace
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MiddleRayHandler : MonoBehaviour
    {
        [SerializeField] float lineLength = 10f;
        [SerializeField] Transform leftHandForward = null;
        [SerializeField] Transform rightHandForward = null;

        [SerializeField] LineRenderer lineRendererInteractionRay;
        [SerializeField] LineRenderer lineRendererMiddleRay;

        Vector3[] linePositions = new Vector3[2];
        Vector3 middlePosition = new Vector3();
        Vector3 offsetMiddlePosition = new Vector3();
        Vector3 middleDirection = new Vector3();
        Vector3 secondMiddlePosition = new Vector3();

        [SerializeField] bool drawMiddleRay = false;
        Ray middleRay;

        Vector3 rayEndPoint = new Vector3();

        [SerializeField] float forwardOffset = 0.3f;
        [SerializeField] float smoothing = 0.1f;

        [SerializeField] bool showRayAtStart = true;
        [SerializeField] bool enableInteractionRay = true;
        bool showRay;

        [SerializeField] bool raycast = true;
        [SerializeField] float raycastMaxDist = 100f;

        [SerializeField] LayerMask layerMasks;

        //[SerializeField] BadTrackingHandler badTrackingHandler;
        [SerializeField] bool pauseOnBadTracking = true;
        bool trackingBroke = false;
        Ray capturedRay;

        RaycastHit hit;
        bool hasHit;

        bool rayEndpointOverwrite = false;


        void Start()
        {
            capturedRay.origin = Vector3.zero;
            capturedRay.direction = Vector3.forward;

            lineRendererInteractionRay = GetComponent<LineRenderer>();
            lineRendererInteractionRay.positionCount = 2;

            if (!enableInteractionRay)
            {
                lineRendererInteractionRay.enabled = false;
            }

            // IShowRay(showRayAtStart);

            // if (badTrackingHandler == null)
            // {
            //     badTrackingHandler = GameObject.FindWithTag("Player").GetComponent<BadTrackingHandler>();
            // }

            // if (badTrackingHandler != null)
            // {
            //     badTrackingHandler.onTrackingBroke += BreakTracking;
            //     badTrackingHandler.onTrackingBack += TrackingBack;
            // }
        }


        void Update()
        {
            // GameObject.FindWithTag("Debug").GetComponent<MyDebugUIControl>().MyDebugLog("Captured Ray: " + capturedRay.origin);

            if (!trackingBroke)
            {
                middlePosition = Vector3.Lerp(leftHandForward.position, rightHandForward.position, 0.5f);
                middleDirection = leftHandForward.forward + rightHandForward.forward;
                offsetMiddlePosition = middlePosition + middleDirection * forwardOffset;


                middleRay.origin = Vector3.Lerp(middleRay.origin, offsetMiddlePosition, smoothing);
                middleRay.direction = Vector3.Lerp(middleRay.direction, middleDirection, smoothing);

                //capturedRay = middleRay;

            }

            // else
            // {
            //     middleRay = capturedRay;
            // }







            if (raycast)
            {
                hasHit = Physics.Raycast(middleRay, out hit, raycastMaxDist, layerMasks);

                if (hasHit)
                {
                    SetRayEndPoint(true, hit.point);
                }
            }

            if (showRay)
            {

                DrawInteractionRay(rayEndPoint);
            }

            DrawMiddleRay();




        }

        private void LateUpdate()
        {
            if (trackingBroke)
            {
                // if(Vector3.Distance(middleRay.origin, capturedRay.origin) > 1f)
                // {
                //         print("threshold broke when traking broken");
                //     return;
                // }
                middleRay = capturedRay;
            }

            else
            {
                if (Vector3.Distance(middleRay.origin, capturedRay.origin) > 1f)
                {
                    //print("threshold broke when traking working");
                    return;
                }

                capturedRay = middleRay;
            }
        }



        public Ray GetMiddleRay()
        {
            return middleRay;
        }

        public bool GetHasHit()
        {
            return hasHit;
        }

        public RaycastHit GetHit()
        {
            return hit;
        }



        private void SetRayEndPoint(bool hasHit, Vector3 endPoint)
        {

            if (!showRay) return;
            if (rayEndpointOverwrite) return;

            if (hasHit)
            {
                rayEndPoint = endPoint;
            }

            else
            {
                rayEndPoint = middleRay.origin + middleRay.direction * lineLength;
            }

        }

        public void OverwriteRayEndPoint(bool overwrite, Vector3 point)
        {
            rayEndpointOverwrite = overwrite;
            rayEndPoint = point;

        }

        private void DrawInteractionRay(Vector3 secondPoint)
        {
            if (!enableInteractionRay) return;

            linePositions[0] = middleRay.origin;
            // linePositions[1] = middleRay.direction * lineLength;
            linePositions[1] = secondPoint;
            lineRendererInteractionRay.SetPositions(linePositions);
        }

        private void DrawMiddleRay()
        {
            if (!drawMiddleRay) return;
            if (ReferenceEquals(middleRay, null)) return;
            Vector3[] positions = new Vector3[2];
            positions[0] = middleRay.origin;
            positions[1] = middleRay.origin + (middleRay.direction * 10f);

            lineRendererMiddleRay.SetPositions(positions);
        }


        private void BreakTracking()
        {
            // GameObject.FindWithTag("Debug").GetComponent<MyDebugUIControl>().MyDebugLog("BreakTracking");

            trackingBroke = true;
        }

        private void TrackingBack()
        {
            // GameObject.FindWithTag("Debug").GetComponent<MyDebugUIControl>().MyDebugLog("Tracking Back");
            middleRay = capturedRay;
            trackingBroke = false;
        }

        // /// IClapActions

        // public void IActivateShoot(bool value)
        // {
        //     return;
        // }

        // public void IShowRay(bool value)
        // {
        //     if (!enableInteractionRay) return;

        //     showRay = value;
        //     lineRendererInteractionRay.enabled = value;
        // }

        // public void IButtonClapActivation(bool value)
        // {
        //     return;
        // }

        // public void IResetObservedButton()
        // {
        //     return;
        // }
    }


}



