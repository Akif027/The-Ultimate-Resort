using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacetoCamera : MonoBehaviour
{
    public string cameraTag = "MainCamera"; // Tag of the camera to look for
    private Camera targetCamera;
    public string allPurposeCameraTag = "AllPurposeCamera";
    private Camera mainCamera;
    private Camera allPurposeCamera;

    void Start()
    {
        // Find the cameras with the specified tags
        GameObject mainCameraObject = GameObject.FindGameObjectWithTag(cameraTag);
        if (mainCameraObject != null)
        {
            mainCamera = mainCameraObject.GetComponent<Camera>();
        }

        GameObject allPurposeCameraObject = GameObject.FindGameObjectWithTag(allPurposeCameraTag);
        if (allPurposeCameraObject != null)
        {
            allPurposeCamera = allPurposeCameraObject.GetComponent<Camera>();
        }
    }

    void Update()
    {
        Camera activeCamera = null;

        // Determine which camera is active
        if (allPurposeCamera != null && allPurposeCamera.gameObject.activeInHierarchy)
        {
            activeCamera = allPurposeCamera;
        }
        else if (mainCamera != null && mainCamera.gameObject.activeInHierarchy)
        {
            activeCamera = mainCamera;
        }

        if (activeCamera != null)
        {
            // Calculate the direction from the camera to the UI element
            Vector3 directionToFace = activeCamera.transform.position - transform.position;

            // Apply rotation to face the camera
            Quaternion targetRotation = Quaternion.LookRotation(directionToFace);
            transform.rotation = targetRotation;
        }
    }
}
