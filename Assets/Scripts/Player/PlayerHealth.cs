using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    
    [Header("Events")]
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnDeath;
    
    private float currentHealth;
    private float statMaxHealth;
    private VisualFeedback visualFeedback;
    
    private void Awake()
    {
        visualFeedback = GetComponent<VisualFeedback>();
        statMaxHealth = maxHealth;
        currentHealth = statMaxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        
        visualFeedback?.FlashDamage();
        
        OnHealthChanged?.Invoke(currentHealth / statMaxHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, statMaxHealth);
        OnHealthChanged?.Invoke(currentHealth / statMaxHealth);
    }
    
    public void SetMaxHealth(float newMaxHealth)
    {
        float healthPercentage = currentHealth / statMaxHealth;
        statMaxHealth = newMaxHealth;
        currentHealth = statMaxHealth * healthPercentage;
        OnHealthChanged?.Invoke(currentHealth / statMaxHealth);
    }
    
    private void Die()
    {
        OnDeath?.Invoke();
        Debug.Log("Player died!");
    }
    
    public float CurrentHealth => currentHealth;
    public float MaxHealth => statMaxHealth;
    public float HealthPercentage => currentHealth / statMaxHealth;
}
