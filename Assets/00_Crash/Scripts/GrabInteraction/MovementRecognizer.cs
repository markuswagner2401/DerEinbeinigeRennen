using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ObliqueSenastions.GestureRecognizerControl
{

    public class MovementRecognizer : MonoBehaviour
    {
        public XRNode inputSource;
        public InputHelpers.Button inputButton;
        public float inputThreshold = 0.1f;
        public Transform movementSource;

        public float newPositionThresholdDistance = 0.05f;
        public GameObject debugCubePrefab;
        public bool creationMode = true;
        public string newGestureName;

        public float recognitionThreshold = 0.9f;

        public float recognitionDelay = 1.5f;
        private float timer = 0;

        [System.Serializable]
        public class UnityStringEvent : UnityEvent<string> { }
        public UnityStringEvent OnRecognized;

        private List<Gesture> trainingSet = new List<Gesture>();
        private bool isMoving = false;
        private List<Vector3> positionsList = new List<Vector3>();
        private int strokeID = 0;

        // Start is called before the first frame update
        void Start()
        {
            //THIS 3 LIENS ARE NOT FROM THE VIDEO AND ARE ADDED TO ADD PREMADE GESTURES MADE DURING TUTORIAL
            //THAT ARE NOT IN YOUR FILES YET

            // TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/");
            // foreach (TextAsset gestureXml in gesturesXml)
            //     trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));


            string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
            foreach (var item in gestureFiles)
            {
                trainingSet.Add(GestureIO.ReadGestureFromFile(item));
            }

            Debug.Log("Save Location: " + Application.persistentDataPath);
        }

        // Update is called once per frame
        void Update()
        {
            InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold);

            //Start The Movement
            if (!isMoving && isPressed)
            {
                strokeID = 0;
                StartMovement();
            }
            //Ending The Movement
            else if (isMoving && !isPressed)
            {
                timer += Time.deltaTime;
                if (timer > recognitionDelay)
                    EndMovement();
            }
            //Updating The Movement
            else if (isMoving && isPressed)
            {
                if (timer > 0)
                {
                    strokeID++;
                }

                timer = 0;
                UpdateMovement();
            }
        }

        void StartMovement()
        {
            Debug.Log("Start Movement");
            isMoving = true;
            positionsList.Clear();
            positionsList.Add(movementSource.position);

            if (debugCubePrefab)
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
        }

        void EndMovement()
        {
            Debug.Log("End Movement");
            isMoving = false;

            //Create The Gesture FRom The Position List
            Point[] pointArray = new Point[positionsList.Count];

            for (int i = 0; i < positionsList.Count; i++)
            {
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
                pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
            }

            Gesture newGesture = new Gesture(pointArray);

            //Add A new gesture to training set
            if (creationMode)
            {
                newGesture.Name = newGestureName;
                trainingSet.Add(newGesture);

                string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
                GestureIO.WriteGesture(pointArray, newGestureName, fileName);
            }
            //recognize
            else
            {
                Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
                Debug.Log(result.GestureClass + result.Score);
                if (result.Score > recognitionThreshold)
                {
                    OnRecognized.Invoke(result.GestureClass);
                }
            }
        }

        void UpdateMovement()
        {
            Debug.Log("Update Movement");
            Vector3 lastPosition = positionsList[positionsList.Count - 1];

            if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
            {
                positionsList.Add(movementSource.position);
                if (debugCubePrefab)
                    Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
            }
        }
    }



}
