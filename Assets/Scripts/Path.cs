using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Path : MonoBehaviour
{
    public Transform[] Points;
    public InputActionReference toggleReference;
    public GameObject path;

    [SerializeField]
    private float moveSpeed;

    private int pointsIndex;
    private bool toggleState = true;
    // Start is called before the first frame update
    void Start()
    {
        pointsIndex = 0;
        transform.position = Points[pointsIndex].transform.position;

        toggleState = true;
        toggleReference.action.started += TogglePathMesh;
    }

    // Update is called once per frame
    void Update()
    {
        if(pointsIndex <= Points.Length - 1)
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
    }
    private void OnDestroy()
    {
        toggleReference.action.started -= TogglePathMesh;
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
}
