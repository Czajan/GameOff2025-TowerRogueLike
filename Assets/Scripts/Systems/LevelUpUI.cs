using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelUpUI : MonoBehaviour
{
    public static LevelUpUI Instance { get; private set; }
    
    [Header("UI References")]
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private TextMeshProUGUI levelTitleText;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private GameObject optionButtonPrefab;
    
    [Header("Stat Upgrade Options - Small Boosts")]
    [SerializeField] private StatUpgradeOption[] smallBoosts;
    
    [Header("Stat Upgrade Options - Significant Boosts (Every 5 Levels)")]
    [SerializeField] private StatUpgradeOption[] milestoneBoosts;
    
    private List<GameObject> currentOptions = new List<GameObject>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.OnLevelUp.AddListener(ShowLevelUpOptions);
        }
        
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
    }
    
    private void ShowLevelUpOptions(int newLevel)
    {
        if (levelUpPanel == null || optionsContainer == null) return;
        
        ClearCurrentOptions();
        
        bool isMilestone = newLevel % 5 == 0;
        
        if (levelTitleText != null)
        {
            if (isMilestone)
            {
                levelTitleText.text = $"★ LEVEL {newLevel} - MILESTONE! ★";
            }
            else
            {
                levelTitleText.text = $"LEVEL {newLevel}";
            }
        }
        
        StatUpgradeOption[] availableOptions = isMilestone ? milestoneBoosts : smallBoosts;
        
        List<StatUpgradeOption> selectedOptions = SelectRandomOptions(availableOptions, 3);
        
        foreach (StatUpgradeOption option in selectedOptions)
        {
            CreateOptionButton(option);
        }
        
        levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    
    private List<StatUpgradeOption> SelectRandomOptions(StatUpgradeOption[] options, int count)
    {
        List<StatUpgradeOption> selected = new List<StatUpgradeOption>();
        List<StatUpgradeOption> available = new List<StatUpgradeOption>(options);
        
        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, available.Count);
            selected.Add(available[randomIndex]);
            available.RemoveAt(randomIndex);
        }
        
        return selected;
    }
    
    private void CreateOptionButton(StatUpgradeOption option)
    {
        if (optionButtonPrefab == null) return;
        
        GameObject optionObj = Instantiate(optionButtonPrefab, optionsContainer);
        currentOptions.Add(optionObj);
        
        TextMeshProUGUI nameText = optionObj.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descText = optionObj.transform.Find("Description")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI valueText = optionObj.transform.Find("Value")?.GetComponent<TextMeshProUGUI>();
        Button button = optionObj.GetComponent<Button>();
        
        if (nameText != null)
            nameText.text = option.upgradeName;
        
        if (descText != null)
            descText.text = option.description;
        
        if (valueText != null)
            valueText.text = option.GetValueText();
        
        if (button != null)
        {
            button.onClick.AddListener(() => SelectOption(option));
        }
    }
    
    private void SelectOption(StatUpgradeOption option)
    {
        if (PlayerStats.Instance != null)
        {
            option.ApplyUpgrade(PlayerStats.Instance);
            Debug.Log($"<color=green>Applied upgrade: {option.upgradeName}</color>");
        }
        
        CloseLevelUpPanel();
    }
    
    private void CloseLevelUpPanel()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
        
        Time.timeScale = 1f;
        ClearCurrentOptions();
    }
    
    private void ClearCurrentOptions()
    {
        foreach (GameObject option in currentOptions)
        {
            if (option != null)
            {
                Destroy(option);
            }
        }
        currentOptions.Clear();
    }
}

[System.Serializable]
public class StatUpgradeOption
{
    public string upgradeName;
    public string description;
    public UpgradeStatType statType;
    public float value;
    
    public string GetValueText()
    {
        switch (statType)
        {
            case UpgradeStatType.MaxHealth:
            case UpgradeStatType.Damage:
                return $"+{value}";
            case UpgradeStatType.MoveSpeed:
            case UpgradeStatType.AttackSpeed:
                return $"+{value}%";
            case UpgradeStatType.CritChance:
            case UpgradeStatType.CritDamage:
                return $"+{value}%";
            default:
                return $"+{value}";
        }
    }
    
    public void ApplyUpgrade(PlayerStats playerStats)
    {
        switch (statType)
        {
            case UpgradeStatType.MaxHealth:
                playerStats.AddTemporaryMaxHealth(value);
                break;
            case UpgradeStatType.Damage:
                playerStats.AddTemporaryDamage(value);
                break;
            case UpgradeStatType.MoveSpeed:
                playerStats.AddTemporaryMoveSpeed(value / 100f);
                break;
            case UpgradeStatType.CritChance:
                playerStats.AddTemporaryCritChance(value / 100f);
                break;
            case UpgradeStatType.CritDamage:
                playerStats.AddTemporaryCritDamage(value / 100f);
                break;
            case UpgradeStatType.AttackSpeed:
                playerStats.AddTemporaryAttackSpeed(value / 100f);
                break;
        }
    }
}

public enum UpgradeStatType
{
    MaxHealth,
    Damage,
    MoveSpeed,
    CritChance,
    CritDamage,
    AttackSpeed
}
