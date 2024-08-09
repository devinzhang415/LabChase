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
    public async void SpawnAnchor(BaseInteractionEventArgs args) 
    {
        // destroy old track
        if (currAnch && currObj)
        {
            Destroy(currAnch);
            Destroy(currObj);
        }
        
        rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        Pose hitPose = new Pose(hit.point, Quaternion.identity);

        var result = await anchorManager.TryAddAnchorAsync(hitPose);

        bool success = result.TryGetResult(out var anchor);
        
        if (success)
        {
            GameObject spawnedPrefab = Instantiate(prefab, anchor.pose.position, anchor.pose.rotation);
            spawnedPrefab.transform.parent = anchor.transform;
            currAnch = anchor;
            currObj = spawnedPrefab;
        }

    }
}
