# allpur

using UnityEngine;

public class AllPurposeCamera : Singleton<AllPurposeCamera>
{
public GameObject mainCamera; // Main camera (orthographic)
public float transitionSpeed = 5f; // Speed of the camera transition
private bool isTransitioningToTarget = false;
private bool isTransitioningBack = false;

    public Transform target;
    public float targetSize = 15f; // Target size when focusing on the target
    private float originalSize;
    private Quaternion originalRotation;
    private float transitionProgress = 0f;

    void Start()
    {
        Camera mainCam = mainCamera.GetComponent<Camera>();
        originalSize = mainCam.orthographicSize;
        originalRotation = mainCamera.transform.rotation;
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

        if (Input.GetMouseButtonDown(1))
        {
            // Call FocusOnTarget to focus on the target
            FocusOnTarget(target);
        }
    }

    public void FocusOnTarget(Transform targetTransform)
    {
        // Start transitioning the main camera to the target view
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
            TimerManager.Instance.ScheduleAction(2, ResetCamera);
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
        }
    }

    public void ActivateCameraAtPosition(Transform targetTransform)
    {
        mainCamera.transform.position = targetTransform.position;
        mainCamera.transform.rotation = targetTransform.rotation;
        mainCamera.gameObject.SetActive(true);
    }

    public void ResetCamera()
    {
        isTransitioningBack = true;
        transitionProgress = 0f;
    }

}
