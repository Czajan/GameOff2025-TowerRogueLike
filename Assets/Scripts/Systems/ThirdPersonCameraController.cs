using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform followTarget;
    [SerializeField] private CinemachineCamera virtualCamera;
    
    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float minVerticalAngle = -20f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float cameraDistance = 10f;
    [SerializeField] private float cameraHeight = 5f;
    
    [Header("Smoothing")]
    [SerializeField] private float rotationSmoothing = 5f;
    
    private float currentYaw;
    private float currentPitch;
    private Vector2 mouseDelta;
    private InputAction lookAction;
    
    private void Awake()
    {
        if (followTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                followTarget = player.transform;
            }
        }
        
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineCamera>();
        }
        
        lookAction = InputSystem.actions.FindAction("Look");
    }
    
    private void OnEnable()
    {
        if (lookAction != null)
        {
            lookAction.Enable();
        }
    }
    
    private void OnDisable()
    {
        if (lookAction != null)
        {
            lookAction.Disable();
        }
    }
    
    private void Start()
    {
        if (followTarget != null)
        {
            currentYaw = followTarget.eulerAngles.y;
            currentPitch = 30f;
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void LateUpdate()
    {
        if (followTarget == null) return;
        
        if (lookAction != null)
        {
            mouseDelta = lookAction.ReadValue<Vector2>();
        }
        
        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            currentYaw += mouseDelta.x * mouseSensitivity;
            currentPitch -= mouseDelta.y * mouseSensitivity;
            currentPitch = Mathf.Clamp(currentPitch, minVerticalAngle, maxVerticalAngle);
        }
        
        Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);
        
        Vector3 offset = transform.rotation * new Vector3(0f, cameraHeight, -cameraDistance);
        transform.position = followTarget.position + offset;
    }
}
