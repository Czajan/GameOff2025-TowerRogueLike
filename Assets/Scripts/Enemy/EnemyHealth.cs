using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 50f;
    
    [Header("Rewards")]
    [SerializeField] private int goldReward = 5;
    [SerializeField] private int xpReward = 10;
    [SerializeField] private GameObject xpOrbPrefab;
    [SerializeField] private float xpOrbSpawnDelay = 0.8f;
    
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
        
        if (xpOrbPrefab != null && CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.StartCoroutine(SpawnXPOrbDelayed(transform.position, xpReward));
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
    
    private IEnumerator SpawnXPOrbDelayed(Vector3 enemyPosition, int xp)
    {
        Vector3 spawnPosition = enemyPosition + Vector3.up * 0.5f;
        
        yield return new WaitForSeconds(xpOrbSpawnDelay);
        
        GameObject orb = Instantiate(xpOrbPrefab, spawnPosition, Quaternion.identity);
        ExperienceOrb orbScript = orb.GetComponent<ExperienceOrb>();
        if (orbScript != null)
        {
            orbScript.Initialize(xp);
        }
        else
        {
            Debug.LogWarning("ExperienceOrb component not found on XP Orb prefab!");
        }
    }
    
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
}
