using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class WallPlacementManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private GameObject _videoQuadPrefab;

    private GameObject _spawnedQuad;
    private static readonly List<ARRaycastHit> _hits = new();

    void Update()
    {
        // Ignore if no touch or touch isn't starting
        if (Input.touchCount == 0) return;
        if (Input.GetTouch(0).phase != TouchPhase.Began) return;

        Vector2 touchPos = Input.GetTouch(0).position;

        bool hitFound = _raycastManager.Raycast(
            touchPos,
            _hits,
            TrackableType.PlaneWithinPolygon
        );

        if (!hitFound) return;

        // Filter: only accept vertical plane hits
        var hit = _hits[0];
        var plane = _planeManager.GetPlane(hit.trackableId);
        if (plane == null || plane.alignment != PlaneAlignment.Vertical) return;

        Pose pose = hit.pose;

        if (_spawnedQuad == null)
        {
            // First placement — instantiate
            _spawnedQuad = Instantiate(
                _videoQuadPrefab,
                pose.position,
                pose.rotation
            );
        }
        else
        {
            // Subsequent taps — smoothly reposition (prevents jitter)
            _spawnedQuad.transform.position = Vector3.Lerp(
                _spawnedQuad.transform.position,
                pose.position,
                Time.deltaTime * 12f
            );
            _spawnedQuad.transform.rotation = Quaternion.Slerp(
                _spawnedQuad.transform.rotation,
                pose.rotation,
                Time.deltaTime * 12f
            );
        }

        // Hide plane mesh visuals once content is placed
        SetPlanesVisible(false);
    }

    private void SetPlanesVisible(bool visible)
    {
        foreach (var plane in _planeManager.trackables)
            plane.gameObject.SetActive(visible);
    }
}