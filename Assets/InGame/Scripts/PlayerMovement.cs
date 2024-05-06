using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GameManager
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 touchStartPosition;
    private Vector3 touchEndPosition;

    protected override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleTouchInput();
        HandleKeyboardInput();
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
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
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

        // Calculate the move direction based on keyboard input
        moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
    }

    private void MovePlayer()
    {
        // Move the player in the calculated direction at the specified speed
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        // Check if the player is moving
        if (moveDirection.magnitude > 0)
        {
            // If the player is moving, play the walking animation
            AnimationManager.Instance.PlayerAnimationPlay("isWalking", true);

            // Rotate the player to face the direction of movement
            Vector3 newForward = new Vector3(moveDirection.x, 0, moveDirection.z);
            Quaternion targetRotation = Quaternion.LookRotation(newForward);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
        else
        {
            // If the player is not moving, play the idle animation
            AnimationManager.Instance.PlayerAnimationPlay("isWalking", false);
            AnimationManager.Instance.PlayerAnimationPlay("isIdle", true);
        }
    }
}