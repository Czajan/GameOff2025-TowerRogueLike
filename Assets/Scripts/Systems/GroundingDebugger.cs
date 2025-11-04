using UnityEngine;

public class GroundingDebugger : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private Color gizmoColor = Color.green;
    
    private CharacterController characterController;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    private void OnDrawGizmos()
    {
        if (!showDebugInfo)
            return;
        
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
        
        if (characterController != null)
        {
            Gizmos.color = gizmoColor;
            
            float halfHeight = characterController.height / 2f;
            Vector3 center = transform.position + characterController.center;
            
            Vector3 bottom = center - Vector3.up * halfHeight;
            Vector3 top = center + Vector3.up * halfHeight;
            
            Gizmos.DrawWireSphere(bottom, characterController.radius);
            Gizmos.DrawWireSphere(top, characterController.radius);
            Gizmos.DrawLine(bottom, top);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.5f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(bottom, 0.1f);
        }
    }
}
