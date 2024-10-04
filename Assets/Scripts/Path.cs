using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Runtime.CompilerServices;

namespace UnityEngine
{


    public class Path : MonoBehaviour
    {
        public Transform[] Points;
        public InputActionReference toggleReference;
        public InputActionReference rotateReference;
        public GameObject path;
        public GameObject pathParent;

        public GameObject pathStart;
        public displayObject displayObject;

        [SerializeField]
        private float rotationSpeed = 30f;
        [SerializeField]
        private float startThreshold = 0.1f;

        private string filePath;
        private string outputString;
        private Transform mainCameraTransform;
        private float[] moveSpeed = { 1.579f, 1.817f, 1.649f, 1.466f, 1.237f, 1.209f, 1.306f, 1.728f, 1.277f, 1.335f, 1.581f, 1.741f, 1.459f, 1.361f, 1.763f, 1.822f, 1.402f, 1.853f, 1.457f, 1.864f, 1.245f, 1.758f, 1.082f, 1.843f, 1.691f };
        private int speedIndex;
        private int pointsIndex;
        private float tDelt;
        private bool toggleState = true;
        private bool isRotating = false;
        private static bool isMoving = true;
        private static bool previousMovingState = true;
        void Start()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            isMoving = false;
            previousMovingState = isMoving;
            tDelt = 0;
            filePath = System.IO.Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("HH-mm-ss") + ".csv");
            Debug.Log("Filepath is: " + filePath);
            pointsIndex = 0;
            transform.position = Points[pointsIndex].transform.position;
            outputString = "";
            isRotating = false;
            toggleState = true;
            speedIndex = 0;
            mainCameraTransform = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Main Camera").transform;
            if (!mainCameraTransform)
            {
                Debug.LogError("No camera found");
            }
            if (!displayObject)
            {
                displayObject = GameObject.Find("XR Origin (XR Rig)/Canvas/Holder").GetComponent<displayObject>();
            }
            toggleReference.action.started += TogglePathMesh;
            rotateReference.action.started += RotatePath;
        }

        // Update is called once per frame
        void Update()
        {
            if (tDelt >= 3)
            {
                print("speed Changed");
                tDelt = 0;
                speedIndex = (speedIndex + 1) % moveSpeed.Length;
            }
            if (pointsIndex <= Points.Length - 1)
            {
                if (isMoving)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Points[pointsIndex].transform.position, moveSpeed[speedIndex] * Time.deltaTime);
                }


                if (transform.position == Points[pointsIndex].transform.position)
                {
                    pointsIndex++;
                }

                if (pointsIndex == Points.Length)
                {
                    pointsIndex = 0;
                }
            }

            if (isRotating)
            {
                float rotationAmount = rotationSpeed * Time.deltaTime;
                pathParent.transform.Rotate(Vector3.up, rotationAmount);
            }

            outputString = "";

            float distance = Vector3.Distance(mainCameraTransform.position, this.transform.position);
            Debug.Log(distance);
            outputString += distance.ToString() + ',';

            float distanceFromStart = Vector3.Distance(mainCameraTransform.position, pathStart.transform.position);
            if (distanceFromStart <= startThreshold)
            {
                outputString += "Within start thresh " + System.DateTime.Now.ToString("HH-mm-ss") + ',';
            }


            if (displayObject.flashingToggle == displayObject.FlashingToggle.FlashingOn)
            {
                outputString += "Flashing On" + ',';
                displayObject.flashingToggle = displayObject.FlashingToggle.NoToggle;
            }
            else if (displayObject.flashingToggle == displayObject.FlashingToggle.FlashingOff)
            {
                outputString += "Flashing Off" + ',';
                displayObject.flashingToggle = displayObject.FlashingToggle.NoToggle;
            }
            if (isMoving != previousMovingState)
            {
                //if the moving state has changed then add it to the output string
                outputString += (isMoving) ? "Start" : "Stop";
                previousMovingState = isMoving;
            }
            //only executes this part of the code when outside of yth
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(outputString);

            if (!File.Exists(filePath))
                File.WriteAllText(filePath, sb.ToString());
            else
                File.AppendAllText(filePath, sb.ToString());
            tDelt += Time.deltaTime;

        }
        private void OnDestroy()
        {
            toggleReference.action.started -= TogglePathMesh;
            rotateReference.action.performed -= RotatePath;
        }

        private void TogglePathMesh(InputAction.CallbackContext context)
        {
            if (toggleState)
            {
                path.SetActive(false);
                toggleState = !toggleState;
            }
            else
            {
                path.SetActive(true);
                toggleState = !toggleState;
            }
        }

        private void RotatePath(InputAction.CallbackContext context)
        {
            isRotating = !isRotating;

        }

        public static void toggleTrackerMovement() // New method to toggle movement
        {
            isMoving = !isMoving;
        }
    }
}
