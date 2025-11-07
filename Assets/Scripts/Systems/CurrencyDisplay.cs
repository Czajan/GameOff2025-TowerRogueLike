using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI essenceText;
    [SerializeField] private TextMeshProUGUI essenceEarnedThisRunText;
    
    [Header("Display Settings")]
    [SerializeField] private bool showGold = true;
    [SerializeField] private bool showExperience = true;
    [SerializeField] private bool showEssence = false;
    [SerializeField] private bool showEssenceEarnedThisRun = true;
    
    private void Start()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged.AddListener(UpdateGoldDisplay);
            CurrencyManager.Instance.OnExperienceChanged.AddListener(UpdateExperienceDisplay);
            CurrencyManager.Instance.OnEssenceChanged.AddListener(UpdateEssenceDisplay);
            CurrencyManager.Instance.OnEssenceEarnedThisRunChanged.AddListener(UpdateEssenceEarnedThisRunDisplay);
            
            UpdateGoldDisplay(CurrencyManager.Instance.Gold);
            UpdateExperienceDisplay(CurrencyManager.Instance.Experience);
            UpdateEssenceDisplay(CurrencyManager.Instance.Essence);
            UpdateEssenceEarnedThisRunDisplay(CurrencyManager.Instance.EssenceEarnedThisRun);
        }
    }
    
    private void UpdateGoldDisplay(int gold)
    {
        if (showGold && goldText != null)
        {
            goldText.text = $"Gold: {gold}";
        }
    }
    
    private void UpdateExperienceDisplay(int xp)
    {
        if (showExperience && experienceText != null)
        {
            if (ExperienceSystem.Instance != null)
            {
                experienceText.text = $"Level {ExperienceSystem.Instance.CurrentLevel} - XP: {xp}/{ExperienceSystem.Instance.XPRequired}";
            }
            else
            {
                experienceText.text = $"XP: {xp}";
            }
        }
    }
    
    private void UpdateEssenceDisplay(int essence)
    {
        if (showEssence && essenceText != null)
        {
            essenceText.text = $"Essence: {essence}";
        }
    }
    
    private void UpdateEssenceEarnedThisRunDisplay(int essenceEarned)
    {
        if (showEssenceEarnedThisRun && essenceEarnedThisRunText != null)
        {
            essenceEarnedThisRunText.text = $"Essence: {essenceEarned}";
        }
    }
    
    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged.RemoveListener(UpdateGoldDisplay);
            CurrencyManager.Instance.OnExperienceChanged.RemoveListener(UpdateExperienceDisplay);
            CurrencyManager.Instance.OnEssenceChanged.RemoveListener(UpdateEssenceDisplay);
            CurrencyManager.Instance.OnEssenceEarnedThisRunChanged.RemoveListener(UpdateEssenceEarnedThisRunDisplay);
        }
    }
}
