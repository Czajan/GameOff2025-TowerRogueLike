using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    private float statMoveSpeed;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -15f;
    
    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isSprinting;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        statMoveSpeed = moveSpeed;
        
        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform = mainCam.transform;
            }
            else
            {
                cameraTransform = FindFirstObjectByType<Camera>()?.transform;
            }
        }
    }
    
    private void Update()
    {
        HandleMovement();
        HandleGravity();
    }
    
    private void HandleMovement()
    {
        Vector3 move = GetMovementDirection();
        
        float currentSpeed = isSprinting ? statMoveSpeed * sprintMultiplier : statMoveSpeed;
        characterController.Move(move * currentSpeed * Time.deltaTime);
        
        if (move.magnitude > 0.1f)
        {
            RotateTowardsMovement(move);
        }
    }
    
    private Vector3 GetMovementDirection()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        
        return forward * moveInput.y + right * moveInput.x;
    }
    
    private void RotateTowardsMovement(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    private void HandleGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed || context.started;
    }
    
    public void SetMoveSpeed(float speed)
    {
        statMoveSpeed = speed;
    }
    
    public bool IsMoving => moveInput.magnitude > 0.1f;
}
