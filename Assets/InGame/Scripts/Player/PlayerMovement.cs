using System.Collections;
using System.Collections.Generic;
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
    public float raycastDistance = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animation>();
    }

    void Update()
    {
        HandleTouchInput();
        HandleKeyboardInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
        KeepPlayerGrounded();
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

    private void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Get the camera's forward and right vectors, ignoring the y component
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = Camera.main.transform.right;

        // Calculate the move direction based on keyboard input and camera orientation
        moveDirection = (verticalInput * cameraForward + horizontalInput * cameraRight).normalized;
    }

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

    private void KeepPlayerGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
        {
            Vector3 position = rb.position;
            position.y = hit.point.y;
            rb.position = position;
        }
    }
}
