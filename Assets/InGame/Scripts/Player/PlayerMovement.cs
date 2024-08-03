using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 touchStartPosition;
    private Vector3 touchEndPosition;
    [SerializeField] Animation animator;
    public SortSlot sortSlot;
    public LayerMask groundLayer;


    public Camera mainCamera;
    public Camera allPurposeCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animation>();
        // animator.ChangeState(AnimationState.Clean);
    }

    void Update()
    {
        HandleTouchInput();

#if UNITY_EDITOR
        HandleKeyboardInput();
#endif
    }

    void FixedUpdate()
    {
        MovePlayer();

    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = GetWorldPositionFromTouch(touch);
                    break;
                case TouchPhase.Moved:
                    touchEndPosition = GetWorldPositionFromTouch(touch);
                    CalculateMoveDirectionFromTouch();
                    break;
                case TouchPhase.Ended:
                    ResetTouchPositionsAndMoveDirection();
                    break;
            }
        }
    }

    private Vector3 GetWorldPositionFromTouch(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void CalculateMoveDirectionFromTouch()
    {
        moveDirection = (touchEndPosition - touchStartPosition).normalized;
    }

    private void ResetTouchPositionsAndMoveDirection()
    {
        touchStartPosition = Vector3.zero;
        touchEndPosition = Vector3.zero;
        moveDirection = Vector3.zero; // Stop the movement
    }

#if UNITY_EDITOR
    void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

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
            // Get the camera's forward and right vectors, ignoring the y component
            Vector3 cameraForward = Vector3.Scale(activeCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = activeCamera.transform.right;

            // Calculate the move direction based on keyboard input and camera orientation
            moveDirection = (verticalInput * cameraForward + horizontalInput * cameraRight).normalized;
        }
        else
        {
            Debug.LogError("No active camera found. Ensure there are cameras tagged as 'MainCamera' and 'AllPurposeCamera' in the scene.");
        }
    }
#endif

    private void MovePlayer()
    {
        // Move the player in the calculated direction at the specified speed
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        // Check if the player is moving
        if (moveDirection.magnitude > 0)
        {
            // Rotate the player to face the direction of movement
            Vector3 newForward = new Vector3(moveDirection.x, 0, moveDirection.z);
            Quaternion targetRotation = Quaternion.LookRotation(newForward);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
        if (animator.CurrentState != AnimationState.Clean)
        {


            // Handle animations
            if (moveDirection.magnitude > 0)
            {
                animator.ChangeState(AnimationState.Walk);
            }
            else
            {
                animator.ChangeState(AnimationState.Idle);
            }
            animator.SetBlendIdle(sortSlot.SortObjects.Count > 0 ? 1 : 0);
            animator.SetBlendMove(sortSlot.SortObjects.Count > 0 ? 1 : 0);
        }

    }


}
