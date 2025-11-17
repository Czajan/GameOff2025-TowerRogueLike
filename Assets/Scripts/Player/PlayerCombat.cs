using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 25f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    
    [Header("References")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator animator;
    
    [Header("Aiming")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private bool aimWithCursor = true;
    
    private float attackTimer;
    private PlayerController playerController;
    private float statDamage;
    private float statAttackRange;
    
    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        
        playerController = GetComponent<PlayerController>();
        statDamage = attackDamage;
        statAttackRange = attackRange;
    }
    
    private void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        
        if (aimWithCursor)
        {
            UpdateCursorAim();
        }
    }
    
    private void UpdateCursorAim()
    {
        if (playerController != null && playerController.IsMoving)
        {
            return;
        }
        
        if (mainCamera == null || Mouse.current == null)
        {
            return;
        }
        
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            Vector3 aimDirection = (worldPoint - transform.position);
            aimDirection.y = 0;
            
            if (aimDirection.magnitude > 0.5f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && attackTimer <= 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            
            PerformAttack();
            
            float finalCooldown = attackCooldown;
            if (PlayerStats.Instance != null)
            {
                finalCooldown /= PlayerStats.Instance.GetAttackSpeedMultiplier();
            }
            
            attackTimer = finalCooldown;
        }
    }
    
    private void PerformAttack()
    {
        Vector3 attackPosition = attackPoint != null ? attackPoint.position : transform.position + transform.forward;
        
        float finalDamage = statDamage;
        float finalRange = statAttackRange;
        
        if (PlayerStats.Instance != null)
        {
            finalDamage = PlayerStats.Instance.CalculateFinalDamage();
        }
        
        if (WeaponSystem.Instance != null)
        {
            finalDamage *= WeaponSystem.Instance.GetDamageMultiplier();
            finalRange *= WeaponSystem.Instance.GetRangeMultiplier();
        }
        
        Collider[] hitEnemies = Physics.OverlapSphere(attackPosition, finalRange, enemyLayer);
        
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(finalDamage);
                
                if (WeaponSystem.Instance != null)
                {
                    WeaponSystem.Instance.TryApplyWeaponEffect(enemy.gameObject);
                }
                
                Debug.Log($"Hit enemy for {finalDamage} damage!");
            }
        }
    }
    
    public void SetDamage(float damage)
    {
        statDamage = damage;
    }
    
    public void SetAttackRange(float range)
    {
        statAttackRange = range;
    }
    
    private void OnDrawGizmosSelected()
    {
        Vector3 attackPosition = attackPoint != null ? attackPoint.position : transform.position + transform.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, statAttackRange);
    }
}
