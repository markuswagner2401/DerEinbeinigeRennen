using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;

namespace ObliqueSenastions.VRRigSpace
{

    public class SimpleVelocityTracker : MonoBehaviour
    {


        public float currentSpeed;

        float lastSpeed;

        public float angularSpeed;

        public Vector3 currentVelocity = new Vector3();

        public Vector3 angularVelocitiy = new Vector3();

        public float currentLocalSpeed;

        float lastLocalSpeed;

        float localSpeedDuringGate;

        public Vector3 currentLocalVelocity = new Vector3();

        [SerializeField] float speedChangeGate = 0.1f;

        [SerializeField] float maxSpeed = 1.5f;

        [SerializeField] float maxSpeedGateRecoverDuration = 3f;

        float speedGateRecoverTimer;

        bool interruptspeedGate = false;

       
        



        Vector3 previousPosition = new Vector3();

        Vector3 previousVelocity = new Vector3();

        Vector3 previousAngularVelocity = new Vector3();

        Quaternion previousRotation = new Quaternion();

        Vector3 previousLocalPosition = new Vector3();

        Vector3 PreviousLocalVelocity = new Vector3();

        

        [SerializeField] float smoothing;

        bool speedGate;

        void Start()
        {
            previousPosition = transform.position;
            previousLocalPosition = transform.localPosition;
        }


        void Update()
        {
            // position

            currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
            currentVelocity = Vector3.Lerp(previousVelocity, currentVelocity, smoothing);
            currentSpeed = currentVelocity.magnitude;

            // local position

            currentLocalVelocity = (transform.localPosition - previousLocalPosition) / Time.deltaTime;
            currentLocalVelocity = Vector3.Lerp(PreviousLocalVelocity, currentLocalVelocity, smoothing);
            currentLocalSpeed = Mathf.Clamp(currentLocalVelocity.magnitude, 0, maxSpeed);
            //currentLocalSpeed = currentLocalVelocity.magnitude;

            // rotation

            Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);
            deltaRotation.ToAngleAxis(out var angle, out var axis);
            angle *= Mathf.Deg2Rad;

            angularVelocitiy = (1.0f / Time.deltaTime) * angle * axis;
            angularVelocitiy = Vector3.Lerp(previousAngularVelocity, angularVelocitiy, smoothing);
            angularSpeed = angularVelocitiy.magnitude;

            // Capturing

            if((Mathf.Abs(lastLocalSpeed - currentLocalSpeed) > speedChangeGate) && !interruptspeedGate) 
            {
                speedGate = true;

                currentLocalSpeed = Mathf.Clamp(lastLocalSpeed - 0.0001f, 0, maxSpeed);

                speedGateRecoverTimer += Time.deltaTime;

                if(speedGateRecoverTimer > maxSpeedGateRecoverDuration)
                {
                    interruptspeedGate = true;
                    
                }

            }

            else
            {
                speedGateRecoverTimer = 0;

                speedGate = false;

                interruptspeedGate = false;
            }

            //speedGate = (Mathf.Abs(lastLocalSpeed - currentLocalSpeed) > speedChangeGate) ? true : false;

            //

            

            // speedGate = (Mathf.Abs(lastLocalSpeed - currentLocalSpeed) > speedChangeGate) ? true : false;

            // speedGateRecoverTimer = speedGate ? (speedGateRecoverTimer += Time.deltaTime) : 0;

            // interruptspeedGate = speedGateRecoverTimer > maxSpeedGateRecoverDuration ? true : false;

            // currentLocalSpeed = (speedGate && !interruptspeedGate) ? Mathf.Clamp(lastLocalSpeed - 0.0001f, 0, maxSpeed) : 



            

            //

            previousPosition = transform.position;
            previousVelocity = currentVelocity;

            previousRotation = transform.rotation;
            previousAngularVelocity = angularVelocitiy;

            previousLocalPosition = transform.localPosition;
            PreviousLocalVelocity = currentLocalVelocity;

            lastSpeed = currentSpeed;
            lastLocalSpeed = currentLocalSpeed;

           

            

            



            
        }

        ///////

        


        ///////

        public float GetSpeed()
        {
            if (speedGate)
            {
                return 0;
            } 
            return currentSpeed;
        }

        

        public float GetLocalSpeed()
        {
            //print("get local speed: " + (speedGate ? lastLocalSpeed : currentLocalSpeed));
            //if (speedGate) return lastLocalSpeed;
            return currentLocalSpeed;
        }

        

        

        public Vector3 GetVelocity()
        {
            return currentVelocity;
        }

        public Vector3 GetLocalVelocity()
        {
            return currentLocalVelocity;
        }

        public Vector3 GetAngularVelocity()
        {
            return angularVelocitiy;
        }

        public float GetAngularSpeed()
        {
            return angularSpeed;
        }

        // debugging

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + currentVelocity);
        }
    }

}
