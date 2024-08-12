using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityEngine
{


    public class Path : MonoBehaviour
    {
        public Transform[] Points;
        public InputActionReference toggleReference;
        public InputActionReference rotateReference;
        public GameObject path;
        public GameObject pathParent;

        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float rotationSpeed = 30f;

        private Transform mainCameraTransform;
        private int pointsIndex;
        private bool toggleState = true;
        private bool isRotating = false;
        // Start is called before the first frame update
        void Start()
        {

            pointsIndex = 0;
            transform.position = Points[pointsIndex].transform.position;

            isRotating = false;
            toggleState = true;
            mainCameraTransform = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Main Camera").transform;
            if (!mainCameraTransform)
            {
                Debug.Log("No camera found");
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

            Debug.Log(Vector3.Distance(mainCameraTransform.position, this.transform.position));
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
