using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementTuner : MonoBehaviour
{
    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer = ~0;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = false;
    [SerializeField] private Color debugColor = Color.green;
    
    private CharacterController characterController;
    private bool isGroundedVisual;
    private float distanceToGround;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        CheckGroundDistance();
    }
    
    private void CheckGroundDistance()
    {
        Vector3 bottom = transform.position + Vector3.down * (characterController.height / 2f);
        
        if (Physics.Raycast(bottom, Vector3.down, out RaycastHit hit, groundCheckDistance + 0.5f, groundLayer))
        {
            distanceToGround = hit.distance;
            isGroundedVisual = distanceToGround < groundCheckDistance;
        }
        else
        {
            distanceToGround = -1f;
            isGroundedVisual = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (!showDebugInfo) return;
        
        CharacterController cc = GetComponent<CharacterController>();
        if (cc == null) return;
        
        Vector3 bottom = transform.position + Vector3.down * (cc.height / 2f);
        Vector3 top = transform.position + Vector3.up * (cc.height / 2f);
        
        Gizmos.color = isGroundedVisual ? Color.green : Color.red;
        Gizmos.DrawLine(bottom, bottom + Vector3.down * groundCheckDistance);
        
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(bottom, cc.radius);
        Gizmos.DrawWireSphere(top, cc.radius);
        Gizmos.DrawLine(bottom, top);
    }
}
