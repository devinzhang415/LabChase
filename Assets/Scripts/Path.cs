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
        private float moveSpeed;
        [SerializeField]
        private float rotationSpeed = 30f;
        [SerializeField]
        private float startThreshold = 0.1f;

        private string filePath;
        private Transform mainCameraTransform;
        private int pointsIndex;
        private bool toggleState = true;
        private bool isRotating = false;
        // Start is called before the first frame update
        void Start()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            filePath = System.IO.Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("HH-mm-ss") + ".csv");
            Debug.Log("Filepath is: " + filePath);
            pointsIndex = 0;
            transform.position = Points[pointsIndex].transform.position;

            isRotating = false;
            toggleState = true;
            mainCameraTransform = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Main Camera").transform;
            if (!mainCameraTransform)
            {
                Debug.LogError("No camera found");
            }
            toggleReference.action.started += TogglePathMesh;
            rotateReference.action.started += RotatePath;
        }

        // Update is called once per frame
        void Update()
        {
            if (pointsIndex <= Points.Length - 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, Points[pointsIndex].transform.position, moveSpeed * Time.deltaTime);

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

            string outputString = "";

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

            //only executes this part of the code when outside of yth
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(outputString);

            if (!File.Exists(filePath))
                File.WriteAllText(filePath, sb.ToString());
            else
                File.AppendAllText(filePath, sb.ToString());


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
    }
}
