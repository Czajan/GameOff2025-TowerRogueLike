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
    [SerializeField] private float collisionSphereRadius = 0.5f;
    [SerializeField] private float collisionBuffer = 0.5f;
    [SerializeField] private LayerMask collisionLayers = -1;
    [SerializeField] private bool showDebugRays = false;
    [SerializeField] private float collisionPushSpeed = 15f;
    
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
        bool hitDetected = false;
        Vector3 hitPoint = Vector3.zero;
        
        if (enableCollisionAvoidance)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                targetPoint,
                collisionSphereRadius,
                directionFromTarget,
                desiredDistance,
                collisionLayers,
                QueryTriggerInteraction.Ignore
            );
            
            float closestHitDistance = desiredDistance;
            
            foreach (RaycastHit hit in hits)
            {
                if (hit.distance < closestHitDistance)
                {
                    closestHitDistance = hit.distance;
                    hitDetected = true;
                    hitPoint = hit.point;
                }
            }
            
            if (hitDetected)
            {
                targetDistance = Mathf.Max(closestHitDistance - collisionBuffer, minDistance);
                
                if (showDebugRays)
                {
                    Debug.DrawLine(targetPoint, hitPoint, Color.red, 0f, false);
                    Debug.DrawLine(hitPoint, desiredCameraPosition, Color.yellow, 0f, false);
                }
            }
            else if (showDebugRays)
            {
                Debug.DrawLine(targetPoint, desiredCameraPosition, Color.green, 0f, false);
            }
        }
        
        if (hitDetected)
        {
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, collisionPushSpeed * Time.deltaTime);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, rotationSmoothing * Time.deltaTime);
        }
        
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
        Gizmos.DrawWireSphere(transform.position, collisionSphereRadius);
        
        Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 directionFromTarget = targetRotation * Vector3.back;
        
        for (int i = 1; i <= 10; i++)
        {
            float distance = (desiredDistance / 10f) * i;
            Vector3 spherePos = targetPoint + directionFromTarget * distance;
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
            Gizmos.DrawWireSphere(spherePos, collisionSphereRadius);
        }
    }
}

