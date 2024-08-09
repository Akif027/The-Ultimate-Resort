
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 moveDirection;
    [SerializeField] private Animation animator;
    public SortSlot sortSlot;
    public LayerMask groundLayer;

    public Camera mainCamera;
    public Camera allPurposeCamera;

    // Joystick variables
    public FloatingJoystick floatingJoystick; // Reference to the Floating Joystick

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animation>();
    }

    void Update()
    {
        HandleJoystickInput();

#if UNITY_EDITOR
        //  HandleKeyboardInput();
#endif
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleJoystickInput()
    {
        Vector2 joystickInput = floatingJoystick.Direction;


        // Determine which camera is active
        Camera activeCamera = GetActiveCamera();

        if (activeCamera != null)
        {
            // Get the camera's forward and right vectors, ignoring the y component
            Vector3 cameraForward = Vector3.Scale(activeCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = activeCamera.transform.right;

            // Convert joystick input to world space moveDirection
            moveDirection = (joystickInput.y * cameraForward + joystickInput.x * cameraRight).normalized;

        }
    }

#if UNITY_EDITOR
    void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Camera activeCamera = GetActiveCamera();

        if (activeCamera != null)
        {
            Vector3 cameraForward = Vector3.Scale(activeCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = activeCamera.transform.right;

            moveDirection = (verticalInput * cameraForward + horizontalInput * cameraRight).normalized;
        }
        else
        {
            Debug.LogError("No active camera found. Ensure there are cameras tagged as 'MainCamera' and 'AllPurposeCamera' in the scene.");
        }
    }
#endif

    private Camera GetActiveCamera()
    {
        if (allPurposeCamera != null && allPurposeCamera.gameObject.activeInHierarchy)
        {
            return allPurposeCamera;
        }
        else if (mainCamera != null && mainCamera.gameObject.activeInHierarchy)
        {
            return mainCamera;
        }
        return null;
    }

    private void MovePlayer()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);


        if (moveDirection.magnitude > 0)
        {
            Vector3 newForward = new Vector3(moveDirection.x, 0, moveDirection.z);
            Quaternion targetRotation = Quaternion.LookRotation(newForward);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }

        if (animator.CurrentState != AnimationState.Clean)
        {
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
