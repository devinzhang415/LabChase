using System;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPlacement : MonoBehaviour
{
    public static AnchorPlacement instance;

    [SerializeField]
    private GameObject _saveableAnchorPrefab;

    [SerializeField]
    private Transform _saveableTransform;

    private List<OVRSpatialAnchor> _anchorInstances = new(); // Active instances (red and green)

    private HashSet<Guid> _anchorUuids = new(); // Simulated external location, like PlayerPrefs

    private Action<bool, OVRSpatialAnchor.UnboundAnchor> _onLocalized;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _onLocalized = OnLocalized;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) // Create a green capsule
        {
            // Send out a raycast and spawn track at collision
            RaycastHit hit;
            if (Physics.Raycast(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles, Color.yellow);
               // Create a green (savable) spatial anchor
               var go = Instantiate(_saveableAnchorPrefab, hit.point, Quaternion.identity); // Anchor A
                SetupAnchorAsync(go.AddComponent<OVRSpatialAnchor>(), saveAnchor: true);
            }

        }

        if (OVRInput.GetDown(OVRInput.Button.Three)) // x button
        {
            // Destroy all anchors from the scene, but don't erase them from storage
            foreach (var anchor in _anchorInstances)
            {
                Destroy(anchor.gameObject);
            }

            // Clear the list of running anchors
            _anchorInstances.Clear();
        }

        if (OVRInput.GetDown(OVRInput.Button.One)) // a button
        {
            LoadAllAnchors(); // Load saved anchors
        }

        // Erase all saved (green) anchors
        if (OVRInput.GetDown(OVRInput.Button.Four)) // y button
        {
            EraseAllAnchors();
        }
    }

    public async void EraseAllAnchors()
    {
        var result = await OVRSpatialAnchor.EraseAnchorsAsync(anchors: null, uuids: _anchorUuids);
        if (result.Success)
        {
            // Erase our reference lists
            _anchorUuids.Clear();

            Debug.Log($"Anchors erased.");
        }
        else
        {
            Debug.LogError($"Anchors NOT erased {result.Status}");
        }
    }

    private async void SetupAnchorAsync(OVRSpatialAnchor anchor, bool saveAnchor)
    {
        // Keep checking for a valid and localized anchor state
        if (!await anchor.WhenLocalizedAsync())
        {
            Debug.LogError($"Unable to create anchor.");
            Destroy(anchor.gameObject);
            return;
        }

        // Add the anchor to the list of all instances
        _anchorInstances.Add(anchor);

        // save the savable (green) anchors only
        if (saveAnchor && (await anchor.SaveAnchorAsync()).Success)
        {
            // Remember UUID so you can load the anchor later
            _anchorUuids.Add(anchor.Uuid);
        }
    }

    public async void LoadAllAnchors()
    {
        // Load and localize
        var unboundAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();
        var result = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(_anchorUuids, unboundAnchors);

        if (result.Success)
        {
            foreach (var anchor in unboundAnchors)
            {
                anchor.LocalizeAsync().ContinueWith(_onLocalized, anchor);
            }
        }
        else
        {
            Debug.LogError($"Load anchors failed with {result.Status}.");
        }
    }

    private void OnLocalized(bool success, OVRSpatialAnchor.UnboundAnchor unboundAnchor)
    {
        var pose = unboundAnchor.Pose;
        var go = Instantiate(_saveableAnchorPrefab, pose.position, pose.rotation);
        var anchor = go.AddComponent<OVRSpatialAnchor>();

        unboundAnchor.BindTo(anchor);

        // Add the anchor to the running total
        _anchorInstances.Add(anchor);
    }
}
