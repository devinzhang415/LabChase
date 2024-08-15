using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.ARFoundation;

public class SpawnAnchorFromRaySelect : MonoBehaviour
{
    public GameObject prefab;
    public XRRayInteractor rayInteractor;
    public ARAnchorManager anchorManager;
    // Start is called before the first frame update
    private static ARAnchor currAnch;
    private static GameObject currObj;
    void Start()
    {
        rayInteractor.selectEntered.AddListener(SpawnAnchor);
    }

    // Update is called once per frame
    public void SpawnAnchor(BaseInteractionEventArgs args) 
    {
        // destroy old track
        if (currObj)
        {
            //Destroy(currAnch);
            Destroy(currObj);
        }
        
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {

            GameObject instance = Instantiate(prefab, hit.point, Quaternion.identity);
            currObj = instance;
            if (instance.GetComponent<ARAnchor>() == null)
            {
                instance.AddComponent<ARAnchor>();
            }
        }

        /*Pose hitPose = new Pose(hit.point, Quaternion.identity);
        
        var anchor = anchorManager.AddAnchor(hitPose);

        if (anchor)
        {
            GameObject spawnedPrefab = Instantiate(prefab, anchor.transform.position, anchor.transform.rotation);
            spawnedPrefab.transform.parent = anchor.transform;
            currAnch = anchor;
            currObj = spawnedPrefab;
        }*/

    }
}
