using UnityEngine;
using UnityEngine.Events;

public class DefenseObjective : MonoBehaviour
{
    [Header("Objective Settings")]
    [SerializeField] private float maxHealth = 500f;
    [SerializeField] private string objectiveName = "Defense Point";
    
    [Header("Visual Feedback")]
    [SerializeField] private Renderer objectiveRenderer;
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color damagedColor = Color.yellow;
    [SerializeField] private Color criticalColor = Color.red;
    
    [Header("Events")]
    public UnityEvent<float> OnHealthChanged = new UnityEvent<float>();
    public UnityEvent OnObjectiveDestroyed = new UnityEvent();
    
    private float currentHealth;
    private DefenseZone parentZone;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        parentZone = GetComponentInParent<DefenseZone>();
        
        if (objectiveRenderer == null)
        {
            objectiveRenderer = GetComponent<Renderer>();
        }
    }
    
    private void Start()
    {
        UpdateVisuals();
        OnHealthChanged?.Invoke(HealthPercentage);
    }
    
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
        OnHealthChanged?.Invoke(HealthPercentage);
        UpdateVisuals();
        
        Debug.Log($"{objectiveName} took {damage} damage! Health: {currentHealth}/{maxHealth} ({HealthPercentage * 100:F0}%)");
        
        if (currentHealth <= 0)
        {
            DestroyObjective();
        }
    }
    
    private void DestroyObjective()
    {
        Debug.Log($"<color=red>⚠️ {objectiveName} DESTROYED! Falling back...</color>");
        
        OnObjectiveDestroyed?.Invoke();
        
        if (parentZone != null)
        {
            parentZone.OnObjectiveDestroyed();
        }
        
        gameObject.SetActive(false);
    }
    
    private void UpdateVisuals()
    {
        if (objectiveRenderer == null) return;
        
        float healthPercent = HealthPercentage;
        Color targetColor;
        
        if (healthPercent > 0.5f)
        {
            targetColor = Color.Lerp(damagedColor, healthyColor, (healthPercent - 0.5f) * 2f);
        }
        else if (healthPercent > 0.25f)
        {
            targetColor = Color.Lerp(criticalColor, damagedColor, (healthPercent - 0.25f) * 4f);
        }
        else
        {
            targetColor = criticalColor;
        }
        
        objectiveRenderer.material.color = targetColor;
    }
    
    public void ResetObjective()
    {
        currentHealth = maxHealth;
        gameObject.SetActive(true);
        UpdateVisuals();
        OnHealthChanged?.Invoke(HealthPercentage);
        Debug.Log($"<color=green>{objectiveName} reset to full health</color>");
    }
    
    public float HealthPercentage => currentHealth / maxHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDestroyed => currentHealth <= 0;
    public Vector3 Position => transform.position;
}
