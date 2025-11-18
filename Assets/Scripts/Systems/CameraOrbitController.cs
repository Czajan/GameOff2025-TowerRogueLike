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
    
    [Header("Camera Distance")]
    [SerializeField] private float desiredDistance = 10f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float cameraHeight = 5f;
    
    [Header("Collision Settings")]
    [SerializeField] private bool enableCollisionAvoidance = true;
    [SerializeField] private float collisionBuffer = 0.3f;
    [SerializeField] private LayerMask collisionLayers = -1;
    [SerializeField] private bool showDebugRays = false;
    
    private float currentYaw;
    private float currentPitch = 45f;
    private Vector2 mouseDelta;
    private InputAction lookAction;
    private Transform followTarget;
    private float currentDistance;
    
    private void Awake()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        currentYaw = transform.eulerAngles.y;
        currentDistance = desiredDistance;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            followTarget = player.transform;
        }
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
        if (followTarget == null) return;
        
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
        
        Vector3 targetPoint = followTarget.position + Vector3.up * cameraHeight;
        
        Vector3 directionFromTarget = targetRotation * Vector3.back;
        Vector3 desiredCameraPosition = targetPoint + directionFromTarget * desiredDistance;
        
        float targetDistance = desiredDistance;
        
        if (enableCollisionAvoidance)
        {
            Vector3 directionToCamera = desiredCameraPosition - targetPoint;
            float checkDistance = directionToCamera.magnitude;
            
            if (Physics.SphereCast(targetPoint, 0.3f, directionToCamera.normalized, out RaycastHit hit, checkDistance, collisionLayers, QueryTriggerInteraction.Ignore))
            {
                targetDistance = Mathf.Max(hit.distance - collisionBuffer, minDistance);
                
                if (showDebugRays)
                {
                    Debug.DrawLine(targetPoint, hit.point, Color.red, 0f, false);
                    Debug.DrawLine(hit.point, desiredCameraPosition, Color.yellow, 0f, false);
                }
            }
            else if (showDebugRays)
            {
                Debug.DrawLine(targetPoint, desiredCameraPosition, Color.green, 0f, false);
            }
        }
        
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, rotationSmoothing * Time.deltaTime);
        
        Vector3 finalDirection = targetRotation * Vector3.back;
        Vector3 finalPosition = targetPoint + finalDirection * currentDistance;
        
        transform.position = finalPosition;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (followTarget == null) return;
        
        Vector3 targetPoint = followTarget.position + Vector3.up * cameraHeight;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetPoint, 0.3f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(targetPoint, transform.position);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}

