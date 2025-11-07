using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 50f;
    
    [Header("Rewards")]
    [SerializeField] private int goldReward = 5;
    [SerializeField] private int xpReward = 10;
    [SerializeField] private GameObject xpOrbPrefab;
    
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
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddGold(goldReward);
        }
        
        if (xpOrbPrefab != null)
        {
            GameObject orb = Instantiate(xpOrbPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            ExperienceOrb orbScript = orb.GetComponent<ExperienceOrb>();
            if (orbScript != null)
            {
                orbScript.Initialize(xpReward);
            }
            else
            {
                Debug.LogWarning("ExperienceOrb component not found on XP Orb prefab!");
            }
        }
        
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.AddEnemyKill();
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
