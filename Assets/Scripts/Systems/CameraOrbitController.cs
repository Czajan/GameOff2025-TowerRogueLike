using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbitController : MonoBehaviour
{
    [Header("Orbit Settings")]
    [Range(0.1f, 5f)]
    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private float minVerticalAngle = 10f;
    [SerializeField] private float maxVerticalAngle = 70f;
    [SerializeField] private float rotationSmoothing = 10f;
    
    private float currentYaw;
    private float currentPitch = 45f;
    private Vector2 mouseDelta;
    private InputAction lookAction;
    
    private void Awake()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        currentYaw = transform.eulerAngles.y;
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
    
    private void LateUpdate()
    {
        if (lookAction != null)
        {
            mouseDelta = lookAction.ReadValue<Vector2>();
        }
        
        if (mouseDelta.sqrMagnitude > 0.01f)
        {
            currentYaw += mouseDelta.x * mouseSensitivity;
            currentPitch -= mouseDelta.y * mouseSensitivity;
            currentPitch = Mathf.Clamp(currentPitch, minVerticalAngle, maxVerticalAngle);
        }
        
        Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);
    }
}
