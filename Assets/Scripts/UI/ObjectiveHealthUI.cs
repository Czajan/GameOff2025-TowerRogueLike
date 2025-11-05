using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DefenseObjective targetObjective;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI objectiveNameText;
    
    [Header("Colors")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color damagedColor = Color.yellow;
    [SerializeField] private Color criticalColor = Color.red;
    
    private void Start()
    {
        if (targetObjective != null)
        {
            targetObjective.OnHealthChanged.AddListener(UpdateHealthDisplay);
            targetObjective.OnObjectiveDestroyed.AddListener(OnObjectiveDestroyed);
            
            if (objectiveNameText != null)
            {
                objectiveNameText.text = targetObjective.name;
            }
            
            UpdateHealthDisplay(targetObjective.HealthPercentage);
        }
    }
    
    private void OnDestroy()
    {
        if (targetObjective != null)
        {
            targetObjective.OnHealthChanged.RemoveListener(UpdateHealthDisplay);
            targetObjective.OnObjectiveDestroyed.RemoveListener(OnObjectiveDestroyed);
        }
    }
    
    private void UpdateHealthDisplay(float healthPercentage)
    {
        if (healthSlider != null)
        {
            healthSlider.value = healthPercentage;
        }
        
        if (healthText != null)
        {
            float currentHealth = targetObjective.CurrentHealth;
            float maxHealth = targetObjective.MaxHealth;
            healthText.text = $"{currentHealth:F0} / {maxHealth:F0}";
        }
        
        if (fillImage != null)
        {
            if (healthPercentage > 0.5f)
            {
                fillImage.color = Color.Lerp(damagedColor, healthyColor, (healthPercentage - 0.5f) * 2f);
            }
            else if (healthPercentage > 0.25f)
            {
                fillImage.color = Color.Lerp(criticalColor, damagedColor, (healthPercentage - 0.25f) * 4f);
            }
            else
            {
                fillImage.color = criticalColor;
            }
        }
    }
    
    private void OnObjectiveDestroyed()
    {
        gameObject.SetActive(false);
    }
    
    public void SetTargetObjective(DefenseObjective objective)
    {
        if (targetObjective != null)
        {
            targetObjective.OnHealthChanged.RemoveListener(UpdateHealthDisplay);
            targetObjective.OnObjectiveDestroyed.RemoveListener(OnObjectiveDestroyed);
        }
        
        targetObjective = objective;
        
        if (targetObjective != null)
        {
            targetObjective.OnHealthChanged.AddListener(UpdateHealthDisplay);
            targetObjective.OnObjectiveDestroyed.AddListener(OnObjectiveDestroyed);
            
            if (objectiveNameText != null)
            {
                objectiveNameText.text = targetObjective.name;
            }
            
            gameObject.SetActive(true);
            UpdateHealthDisplay(targetObjective.HealthPercentage);
        }
    }
}
