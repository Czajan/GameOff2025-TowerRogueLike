using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CinemachineMouseOrbit : CinemachineExtension
{
    [Header("Mouse Orbit Settings")]
    [Range(0.1f, 5f)]
    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private float minVerticalAngle = 10f;
    [SerializeField] private float maxVerticalAngle = 70f;
    
    private float currentYaw;
    private float currentPitch = 45f;
    private InputAction lookAction;
    
    protected override void Awake()
    {
        base.Awake();
        lookAction = InputSystem.actions.FindAction("Look");
        
        if (ComponentOwner != null)
        {
            Vector3 angles = ComponentOwner.transform.eulerAngles;
            currentYaw = angles.y;
            currentPitch = angles.x;
        }
    }
    
    protected void OnEnable()
    {
        if (lookAction != null)
        {
            lookAction.Enable();
        }
    }
    
    protected void OnDisable()
    {
        if (lookAction != null)
        {
            lookAction.Disable();
        }
    }
    
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        {
            if (lookAction != null)
            {
                Vector2 mouseDelta = lookAction.ReadValue<Vector2>();
                
                if (mouseDelta.sqrMagnitude > 0.01f)
                {
                    currentYaw += mouseDelta.x * mouseSensitivity;
                    currentPitch -= mouseDelta.y * mouseSensitivity;
                    currentPitch = Mathf.Clamp(currentPitch, minVerticalAngle, maxVerticalAngle);
                }
            }
            
            Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            state.RawOrientation = targetRotation;
        }
    }
}
