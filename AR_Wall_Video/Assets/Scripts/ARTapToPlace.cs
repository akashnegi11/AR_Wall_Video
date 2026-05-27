using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARTapToPlace : MonoBehaviour
{
    public GameObject placementPrefab;

    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (raycastManager.Raycast(touch.position, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            ARPlane plane = hits[0].trackable as ARPlane;
            ARAnchor anchor = anchorManager.AttachAnchor(plane, hitPose);

            if (anchor != null)
            {
                Instantiate(placementPrefab, hitPose.position, hitPose.rotation, anchor.transform);
            }
        }
    }
}