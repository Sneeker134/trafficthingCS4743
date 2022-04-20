using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ARPlacement : MonoBehaviour
{
    public GameObject arObjectToSpawn;
    public GameObject placementIndicator;
    public Camera arCamera;

    private Pose placementPose;
    private ARRaycastManager arRayCastManager;
    private bool placementPoseIsValid = false;
    private GameObject spawnedObject;

    // Start is called before the first frame update
    void Start()
    {
        arRayCastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject();
        }
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    // Set the placement pose to be the location of the AR camera center in the real world and oriented to that surface.
    void UpdatePlacementPose()
    {
        // Find the center of the display.
        var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        // Set up the empty list for the raycast hits.
        var hits = new List<ARRaycastHit>();
        // Raycast into the scene from the center of the display.
        arRayCastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        // The placement pose is valid only if there were raycast hits.
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            // Set the placement pose to be the first raycast hit point.
            placementPose = hits[0].pose;
        }
    }

    void UpdatePlacementIndicator()
    {
        if (spawnedObject == null && placementPoseIsValid)
        {
            // Enable the placement indicator so it appears in the scene.
            placementIndicator.SetActive(true);
            // Update the placement indicator pose to be the placement pose.
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            // Disable the placement indicator so it is hidden in the scene.
            placementIndicator.SetActive(false);
        }
    }

    void ARPlaceObject()
    {
        // Spawn the object.
        spawnedObject = Instantiate(arObjectToSpawn, placementPose.position, placementPose.rotation);
    }
}

