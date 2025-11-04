using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 50f;
    
    [Header("Rewards")]
    [SerializeField] private int currencyReward = 10;
    
    private float currentHealth;
    private EnemyAI enemyAI;
    private VisualFeedback visualFeedback;
    
    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        visualFeedback = GetComponent<VisualFeedback>();
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        
        visualFeedback?.FlashDamage();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.AddCurrency(currencyReward);
        }
        
        if (enemyAI != null)
        {
            enemyAI.Die();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
}
