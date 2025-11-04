using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float stoppingDistance = 1.5f;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;
    
    [Header("Detection")]
    [SerializeField] private float detectionRange = 15f;
    
    private Transform target;
    private CharacterController characterController;
    private float attackTimer;
    private bool isDead;
    private const float GRAVITY = -20f;
    private float verticalVelocity = 0f;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }
    
    private void Update()
    {
        if (isDead || target == null)
            return;
        
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        
        if (distanceToTarget <= detectionRange)
        {
            if (distanceToTarget > stoppingDistance)
            {
                MoveTowardsTarget();
            }
            else
            {
                RotateTowardsTarget();
            }
            
            if (distanceToTarget <= attackRange)
            {
                TryAttack();
            }
        }
        
        ApplyGravity();
        
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }
    
    private void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        
        characterController.Move(direction * moveSpeed * Time.deltaTime);
        RotateTowardsDirection(direction);
    }
    
    private void RotateTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        RotateTowardsDirection(direction);
    }
    
    private void RotateTowardsDirection(Vector3 direction)
    {
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }
        }
        else
        {
            verticalVelocity += GRAVITY * Time.deltaTime;
        }
        
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
    
    private void TryAttack()
    {
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }
    
    private void Attack()
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log($"Enemy attacked player for {attackDamage} damage!");
        }
    }
    
    public void Die()
    {
        isDead = true;
        Destroy(gameObject, 0.5f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
