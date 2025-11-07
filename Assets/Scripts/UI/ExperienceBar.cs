using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    
    [Header("Visual Settings")]
    [SerializeField] private Color normalColor = new Color(0.2f, 0.8f, 1f);
    [SerializeField] private Color milestoneColor = new Color(1f, 0.84f, 0f);
    [SerializeField] private bool smoothFill = true;
    [SerializeField] private float fillSpeed = 5f;
    
    private float targetFillAmount = 0f;
    private float currentFillAmount = 0f;
    
    private void Start()
    {
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.OnXPChanged.AddListener(UpdateBar);
            ExperienceSystem.Instance.OnLevelUp.AddListener(OnLevelUp);
            
            UpdateBar(ExperienceSystem.Instance.CurrentXP, ExperienceSystem.Instance.XPRequired);
        }
    }
    
    private void Update()
    {
        if (smoothFill && fillImage != null)
        {
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * fillSpeed);
            fillImage.fillAmount = currentFillAmount;
        }
    }
    
    private void UpdateBar(int currentXP, int requiredXP)
    {
        if (requiredXP <= 0) return;
        
        targetFillAmount = Mathf.Clamp01((float)currentXP / requiredXP);
        
        if (!smoothFill && fillImage != null)
        {
            fillImage.fillAmount = targetFillAmount;
            currentFillAmount = targetFillAmount;
        }
        
        if (levelText != null && ExperienceSystem.Instance != null)
        {
            levelText.text = $"Level {ExperienceSystem.Instance.CurrentLevel}";
        }
        
        if (xpText != null)
        {
            xpText.text = $"{currentXP} / {requiredXP}";
        }
        
        UpdateBarColor();
    }
    
    private void OnLevelUp(int newLevel)
    {
        currentFillAmount = 0f;
        targetFillAmount = 0f;
        
        if (fillImage != null)
        {
            fillImage.fillAmount = 0f;
        }
        
        UpdateBarColor();
    }
    
    private void UpdateBarColor()
    {
        if (fillImage == null || ExperienceSystem.Instance == null) return;
        
        if (ExperienceSystem.Instance.CurrentLevel % 5 == 0)
        {
            fillImage.color = milestoneColor;
        }
        else
        {
            fillImage.color = normalColor;
        }
    }
    
    private void OnDestroy()
    {
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.OnXPChanged.RemoveListener(UpdateBar);
            ExperienceSystem.Instance.OnLevelUp.RemoveListener(OnLevelUp);
        }
    }
}
