using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemySpawnValidator : MonoBehaviour
{
    [Header("Spawn Validation")]
    [SerializeField] private float maxGroundCheckDistance = 5f;
    [SerializeField] private float minHeightAboveGround = 0.1f;
    [SerializeField] private LayerMask groundLayer = ~0;
    
    private NavMeshAgent navAgent;
    private bool validated = false;
    
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
    
    private void Start()
    {
        ValidateSpawnPosition();
    }
    
    private void ValidateSpawnPosition()
    {
        if (validated) return;
        
        Vector3 currentPos = transform.position;
        
        if (Physics.Raycast(currentPos + Vector3.up * maxGroundCheckDistance, Vector3.down, out RaycastHit hit, maxGroundCheckDistance * 2f, groundLayer))
        {
            Vector3 groundPos = hit.point + Vector3.up * minHeightAboveGround;
            
            if (NavMesh.SamplePosition(groundPos, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
            {
                transform.position = navHit.position;
                
                if (navAgent != null)
                {
                    navAgent.Warp(navHit.position);
                }
                
                float distanceFromOriginal = Vector3.Distance(currentPos, navHit.position);
                if (distanceFromOriginal > 0.5f)
                {
                    Debug.Log($"<color=yellow>Enemy spawn corrected by {distanceFromOriginal:F2} units to valid NavMesh position</color>");
                }
            }
            else
            {
                Debug.LogWarning($"<color=red>Enemy spawned outside NavMesh! Attempting to find nearest valid position...</color>");
                
                if (NavMesh.SamplePosition(currentPos, out NavMeshHit nearestNavHit, 10f, NavMesh.AllAreas))
                {
                    transform.position = nearestNavHit.position;
                    navAgent.Warp(nearestNavHit.position);
                    Debug.Log($"<color=green>Moved enemy to nearest NavMesh at distance {Vector3.Distance(currentPos, nearestNavHit.position):F2}</color>");
                }
                else
                {
                    Debug.LogError($"<color=red>CRITICAL: Cannot find valid NavMesh position for enemy! Destroying...</color>");
                    Destroy(gameObject);
                    return;
                }
            }
        }
        
        validated = true;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 checkStart = transform.position + Vector3.up * maxGroundCheckDistance;
        Gizmos.DrawLine(checkStart, checkStart + Vector3.down * maxGroundCheckDistance * 2f);
        
        if (navAgent != null && navAgent.isOnNavMesh)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
