using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.ARFoundation;

public class SpawnAnchorFromRaySelect : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public ARAnchorManager anchorManager;
    // Start is called before the first frame update
    void Start()
    {
        rayInteractor.selectEntered.AddListener(SpawnAnchor);
    }

    // Update is called once per frame
    public async void SpawnAnchor(BaseInteractionEventArgs args) 
    {
        rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        Pose hitPose = new Pose(hit.point, Quaternion.identity);

        var result = await anchorManager.TryAddAnchorAsync(hitPose);

        bool success = result.TryGetResult(out var anchor);

    }
}
