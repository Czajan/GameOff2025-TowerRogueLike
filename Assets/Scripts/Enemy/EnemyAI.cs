using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stoppingDistance = 1.5f;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;
    
    [Header("Detection")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float playerPriorityRange = 20f;
    
    [Header("Target Priority")]
    [SerializeField] private bool prioritizePlayer = true;
    
    private Transform target;
    private DefenseObjective objectiveTarget;
    private Transform playerTarget;
    private NavMeshAgent navAgent;
    private float attackTimer;
    private bool isDead;
    
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = moveSpeed;
        navAgent.stoppingDistance = stoppingDistance;
    }
    
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
        
        FindDefenseObjective();
        UpdateTarget();
        
        if (RadarSystem.Instance != null)
        {
            RadarSystem.Instance.RegisterEnemy(transform);
        }
    }
    
    private void Update()
    {
        if (isDead)
            return;
        
        UpdateTarget();
        
        if (target == null)
            return;
        
        float distanceToTarget = GetHorizontalDistance(transform.position, target.position);
        
        bool isTargetingObjective = (objectiveTarget != null && target == objectiveTarget.transform);
        bool shouldMove = isTargetingObjective || distanceToTarget <= detectionRange;
        
        if (shouldMove)
        {
            navAgent.SetDestination(target.position);
            
            if (distanceToTarget <= attackRange)
            {
                TryAttack();
            }
        }
        else
        {
            navAgent.ResetPath();
        }
        
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }
    
    private float GetHorizontalDistance(Vector3 from, Vector3 to)
    {
        Vector3 fromFlat = new Vector3(from.x, 0, from.z);
        Vector3 toFlat = new Vector3(to.x, 0, to.z);
        return Vector3.Distance(fromFlat, toFlat);
    }
    
    private void FindDefenseObjective()
    {
        DefenseZone[] zones = FindObjectsByType<DefenseZone>(FindObjectsSortMode.None);
        foreach (DefenseZone zone in zones)
        {
            if (zone.IsActive)
            {
                objectiveTarget = zone.GetDefenseObjective();
                if (objectiveTarget != null && !objectiveTarget.IsDestroyed)
                {
                    Debug.Log($"Enemy found active objective: {objectiveTarget.name}");
                    return;
                }
            }
        }
    }
    
    private void UpdateTarget()
    {
        if (!prioritizePlayer)
        {
            if (objectiveTarget != null && !objectiveTarget.IsDestroyed)
            {
                target = objectiveTarget.transform;
            }
            return;
        }
        
        if (playerTarget != null)
        {
            float distanceToPlayer = GetHorizontalDistance(transform.position, playerTarget.position);
            
            if (distanceToPlayer <= playerPriorityRange)
            {
                target = playerTarget;
                return;
            }
        }
        
        if (objectiveTarget != null && !objectiveTarget.IsDestroyed)
        {
            target = objectiveTarget.transform;
        }
        else
        {
            FindDefenseObjective();
            if (objectiveTarget != null && !objectiveTarget.IsDestroyed)
            {
                target = objectiveTarget.transform;
            }
            else if (playerTarget != null)
            {
                target = playerTarget;
            }
        }
    }
    
    public void SetDefenseZone(DefenseZone zone)
    {
        if (zone != null)
        {
            objectiveTarget = zone.GetDefenseObjective();
            UpdateTarget();
        }
    }
    
    private void TryAttack()
    {
        if (attackTimer <= 0)
        {
            float distanceToTarget = GetHorizontalDistance(transform.position, target.position);
            Debug.Log($"<color=cyan>Enemy TryAttack: Distance={distanceToTarget:F2}, AttackRange={attackRange}, Target={target.name}</color>");
            Attack();
            attackTimer = attackCooldown;
        }
    }
    
    private void Attack()
    {
        if (target == playerTarget)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log($"Enemy attacked player for {attackDamage} damage!");
            }
        }
        else if (objectiveTarget != null && target == objectiveTarget.transform)
        {
            objectiveTarget.TakeDamage(attackDamage);
            Debug.Log($"<color=orange>Enemy attacked {objectiveTarget.name} for {attackDamage} damage!</color>");
        }
        else
        {
            Debug.LogWarning($"Enemy tried to attack but target is invalid! Target: {(target != null ? target.name : "null")}, PlayerTarget: {(playerTarget != null ? playerTarget.name : "null")}, ObjectiveTarget: {(objectiveTarget != null ? objectiveTarget.name : "null")}");
        }
    }
    
    public void Die()
    {
        isDead = true;
        
        if (RadarSystem.Instance != null)
        {
            RadarSystem.Instance.UnregisterEnemy(transform);
        }
        
        Destroy(gameObject, 0.5f);
    }
    
    private void OnDestroy()
    {
        if (RadarSystem.Instance != null)
        {
            RadarSystem.Instance.UnregisterEnemy(transform);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
