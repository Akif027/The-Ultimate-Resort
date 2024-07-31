using UnityEngine;

public class AllPurposeCamera : Singleton<AllPurposeCamera>
{
    public GameObject mainCamera; // Main follow camera
    public Camera topDownCamera; // Top-down focusing camera
    public float transitionSpeed = 5f; // Speed of the camera movement
    private Vector3 targetPosition;
    private bool isMoving = false;

    public Transform target;
    public float distance = 10f; // Camera distance from target
    public float height = 5f; // Camera height above target
    public float Fov = 60f;
    private Vector3 StartPos;
    void Update()
    {
        if (isMoving)
        {
            MoveCamera();
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Call FocusOnTarget to switch cameras and focus on target
            FocusOnTarget(target);
        }
    }

    public void FocusOnTarget(Transform targetTransform)
    {
        StartPos = target.position;
        // Calculate the target position in front of the targetTransform for a perspective camera

        targetPosition = targetTransform.position + (targetTransform.forward * -distance) + Vector3.up * height;

        // Enable the perspective camera and disable the main camera
        topDownCamera.gameObject.SetActive(true);
        mainCamera.SetActive(false);

        // Set the Field of View for the topDownCamera
        topDownCamera.fieldOfView = Fov; // Use the public float Fov defined in your class

        // Start the transition
        isMoving = true;
        TimerManager.Instance.ScheduleAction(2, ResetCamera);
    }
    public void ActivateCameraAtPosition(Transform targetTransform)
    {
        topDownCamera.transform.position = targetTransform.position;
        topDownCamera.transform.rotation = targetTransform.rotation;
        topDownCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }

    private void MoveCamera()
    {
        // Smoothly interpolate the top-down camera's position towards the target position
        topDownCamera.transform.position = Vector3.Lerp(topDownCamera.transform.position, targetPosition, Time.deltaTime * transitionSpeed);

        // Check if the camera has reached the target position
        if (Vector3.Distance(topDownCamera.transform.position, targetPosition) < 0.01f)
        {
            topDownCamera.transform.position = targetPosition; // Ensure exact positioning
            isMoving = false;
        }
    }

    public void ResetCamera()
    {
        topDownCamera.transform.position = StartPos;
        // Disable the top-down camera and enable the main camera
        topDownCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        isMoving = false; // Reset the transition flag
    }
}
