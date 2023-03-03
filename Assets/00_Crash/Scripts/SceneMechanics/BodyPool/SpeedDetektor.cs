using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.Debugging;
using UnityEngine;
using UnityEngine.Events;

namespace ObliqueSenastions.TransformControl
{

    public class SpeedDetektor : MonoBehaviour
    {

        Vector3 previousPosition;
        float speed = 0f;

        [SerializeField] float thresholdMax = 10f;

        [SerializeField] float thresholdMin = 0.1f;

        bool thresholdMinBroken;
        bool thresholdMaxBroken;

        [SerializeField] float smoothing = 0.01f;

        [SerializeField] UnityEvent my_onThresholdMinBroken = null;
        [SerializeField] UnityEvent my_onThresholdMaxBroken = null;


        [Tooltip("for debugging")]
        [SerializeField] SpeedDebugDisplay speedDisplay = null; // Debugging



        void Start()
        {
            previousPosition = transform.position;
        }


        void Update()
        {
            Vector3 actualVelocity = (transform.position - previousPosition) / Time.deltaTime;

            speed = Mathf.Lerp(speed, actualVelocity.magnitude, smoothing);

            previousPosition = transform.position;

            DisplaySpeed(speed);

            ThresholdHandler(speed);




        }

        private void ThresholdHandler(float speed)
        {


            if (speed < thresholdMin)
            {
                thresholdMaxBroken = false;
                if (thresholdMinBroken) return;

                print("threshold Min Broken");
                if (my_onThresholdMinBroken != null) my_onThresholdMinBroken.Invoke();

                thresholdMinBroken = true;
            }



            if (speed > thresholdMax)
            {
                thresholdMinBroken = false;
                if (thresholdMaxBroken) return;

                print("threshold max broken");
                if (my_onThresholdMaxBroken != null) my_onThresholdMaxBroken.Invoke();

                thresholdMaxBroken = true;
            }


        }


        // Debugging

        private void DisplaySpeed(float magnitude)
        {


            if (speedDisplay != null)
            {
                speedDisplay.DisplaySpeed(magnitude);
            }


        }
    }
}
