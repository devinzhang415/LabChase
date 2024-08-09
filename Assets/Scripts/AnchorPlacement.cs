using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPlacement : MonoBehaviour
{
    public GameObject anchorPrefab;
    private GameObject instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = Instantiate(anchorPrefab, Vector3.zero, Quaternion.identity);
        instance.AddComponent<OVRSpatialAnchor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            MoveSpatialAnchor();
        }
    }

    private void MoveSpatialAnchor()
    {
        //raycast and use collision point to move anchor
        RaycastHit hit;
        //shoot a raycast from the right controller
       if (Physics.Raycast(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles * hit.distance, Color.red);
            instance.transform.position = hit.point;
        }
    }
}
