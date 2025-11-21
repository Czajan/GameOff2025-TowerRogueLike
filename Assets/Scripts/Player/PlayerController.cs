using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float groundStickForce = -2f;
    
    private float statMoveSpeed;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;
    
    [Header("Special Abilities")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    
    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isSprinting;
    private bool movementEnabled = true;
    private bool overrideAnimationSpeed = false;
    private float animationSpeedOverride = 0f;
    
    private bool doubleJumpUnlocked = false;
    private bool hasDoubleJumped = false;
    
    private bool dashUnlocked = false;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        statMoveSpeed = moveSpeed;
        
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        
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
        if (!movementEnabled)
        {
            HandleGravity();
            UpdateAnimator();
            return;
        }
        
        if (isDashing)
        {
            HandleDash();
        }
        else
        {
            HandleMovement();
            HandleGravity();
        }
        
        UpdateAnimator();
        
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
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
        if (characterController.isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = groundStickForce;
            }
            hasDoubleJumped = false;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        
        characterController.Move(velocity * Time.deltaTime);
    }
    
    private void HandleDash()
    {
        dashTimer += Time.deltaTime;
        
        float dashSpeed = dashDistance / dashDuration;
        characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
        
        if (dashTimer >= dashDuration)
        {
            isDashing = false;
            dashTimer = 0f;
        }
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            hasDoubleJumped = false;
            
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
        else if (doubleJumpUnlocked && !hasDoubleJumped)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            hasDoubleJumped = true;
            
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
            
            Debug.Log("<color=cyan>Double Jump!</color>");
        }
    }
    
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || !dashUnlocked || isDashing || dashCooldownTimer > 0)
            return;
        
        Vector3 dashDir = GetMovementDirection();
        
        if (dashDir.magnitude < 0.1f)
        {
            dashDir = transform.forward;
        }
        
        dashDirection = dashDir.normalized;
        isDashing = true;
        dashTimer = 0f;
        dashCooldownTimer = dashCooldown;
        
        Debug.Log("<color=cyan>Dash!</color>");
    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed || context.started;
    }
    
    public void SetMoveSpeed(float speed)
    {
        statMoveSpeed = speed;
    }
    
    public void EnableDoubleJump()
    {
        doubleJumpUnlocked = true;
        Debug.Log("<color=green>★ Double Jump Unlocked!</color>");
    }
    
    public void EnableDash()
    {
        dashUnlocked = true;
        Debug.Log("<color=green>★ Dash Unlocked!</color>");
    }
    
    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
        if (!enabled)
        {
            moveInput = Vector2.zero;
        }
        Debug.Log($"<color=cyan>Player movement {(enabled ? "ENABLED" : "DISABLED")}</color>");
    }
    
    public void SetAnimationSpeedOverride(float speed)
    {
        overrideAnimationSpeed = true;
        animationSpeedOverride = speed;
    }
    
    public void ClearAnimationSpeedOverride()
    {
        overrideAnimationSpeed = false;
        animationSpeedOverride = 0f;
    }
    
    private void UpdateAnimator()
    {
        if (animator == null) return;
        
        float speedValue;
        
        if (overrideAnimationSpeed)
        {
            speedValue = animationSpeedOverride;
        }
        else
        {
            speedValue = moveInput.magnitude;
            if (isSprinting && speedValue > 0.1f)
            {
                speedValue = speedValue * sprintMultiplier;
            }
        }
        
        animator.SetFloat("Speed", speedValue);
        animator.SetBool("IsGrounded", characterController.isGrounded);
        
        if (characterController.isGrounded && velocity.y <= 0)
        {
            animator.ResetTrigger("Jump");
        }
    }
    
    public bool HasDoubleJump => doubleJumpUnlocked;
    public bool HasDash => dashUnlocked;
    public bool IsMoving => moveInput.magnitude > 0.1f;
    public float MoveSpeed => statMoveSpeed;
}
