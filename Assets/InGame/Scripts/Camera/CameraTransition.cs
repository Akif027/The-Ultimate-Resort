using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : Singleton<CameraTransition>
{

    public GameObject mainCamera; // Main camera (orthographic)
    public Camera ObjectFocusCamera; // Top-down focusing camera
    public float transitionSpeed = 5f; // Speed of the camera transition
    public float targetSize = 15f; // Target size when focusing on the target

    public Transform target;
    private bool isTransitioningToTarget = false;
    private bool isTransitioningBack = false;


    private float originalSize;
    private Quaternion originalRotation;
    private float transitionProgress = 0f;
    private CameraController cameraController; // Assuming you have this component on your main camera

    void Start()
    {
        Camera mainCam = mainCamera.GetComponent<Camera>();
        originalSize = mainCam.orthographicSize;
        originalRotation = mainCamera.transform.rotation;
        cameraController = mainCamera.GetComponent<CameraController>();
    }

    void Update()
    {
        if (isTransitioningToTarget)
        {
            UpdateCameraTransitionToTarget();
        }
        else if (isTransitioningBack)
        {
            UpdateCameraTransitionBack();
        }

        if (Input.GetMouseButtonDown(1) && target != null)
        {
            // Call FocusOnTarget to focus on the target
            FocusOnTarget(target);
        }
    }

    public void FocusOnTarget(Transform targetTransform)
    {
        if (cameraController != null)
        {
            cameraController.enabled = false; // Disable camera controller if necessary
        }

        target = targetTransform; // Update the target
        isTransitioningToTarget = true;
        transitionProgress = 0f;
    }

    private void UpdateCameraTransitionToTarget()
    {
        transitionProgress += Time.deltaTime * transitionSpeed;

        // Interpolate the orthographic size
        Camera mainCam = mainCamera.GetComponent<Camera>();
        mainCam.orthographicSize = Mathf.Lerp(originalSize, targetSize, transitionProgress);

        // Interpolate the rotation to look at the target
        Vector3 direction = target.position - mainCamera.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        mainCamera.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, transitionProgress);

        if (transitionProgress >= 1f)
        {
            isTransitioningToTarget = false;
            TimerManager.Instance.ScheduleAction(2, ResetCamera); // Schedule camera reset after a delay
        }
    }

    private void UpdateCameraTransitionBack()
    {
        transitionProgress += Time.deltaTime * transitionSpeed;

        // Interpolate the orthographic size back to original
        Camera mainCam = mainCamera.GetComponent<Camera>();
        mainCam.orthographicSize = Mathf.Lerp(targetSize, originalSize, transitionProgress);

        // Interpolate the rotation back to original
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, originalRotation, transitionProgress);

        if (transitionProgress >= 1f)
        {
            mainCam.orthographicSize = originalSize;
            mainCamera.transform.rotation = originalRotation;

            isTransitioningBack = false;
            if (cameraController != null)
            {
                cameraController.enabled = true; // Re-enable camera controller
            }
        }
    }
    public void ActivateCameraAtPosition(Transform targetTransform)
    {
        ObjectFocusCamera.transform.position = targetTransform.position;
        ObjectFocusCamera.transform.rotation = targetTransform.rotation;
        ObjectFocusCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }

    public void ResetTopdown()
    {

        ObjectFocusCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
    public void ResetCamera()
    {
        isTransitioningBack = true;
        transitionProgress = 0f;
    }
}

