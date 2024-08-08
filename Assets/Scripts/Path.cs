using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Transform[] Points;

    [SerializeField]
    private float moveSpeed;

    private int pointsIndex;

    // Start is called before the first frame update
    void Start()
    {
        pointsIndex = 0;
        transform.position = Points[pointsIndex].transform.position;
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
}
