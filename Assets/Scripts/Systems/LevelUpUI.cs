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
    
    private List<GameObject> currentOptions = new List<GameObject>();
    private List<LevelUpgradeData> currentUpgradeChoices = new List<LevelUpgradeData>();
    
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
        if (UpgradeSystem.Instance != null)
        {
            UpgradeSystem.Instance.OnUpgradesOffered.AddListener(ShowUpgradeChoices);
        }
        
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        if (UpgradeSystem.Instance != null)
        {
            UpgradeSystem.Instance.OnUpgradesOffered.RemoveListener(ShowUpgradeChoices);
        }
    }
    
    public void ShowUpgradeChoices(List<LevelUpgradeData> upgrades)
    {
        if (levelUpPanel == null || optionsContainer == null || upgrades == null || upgrades.Count == 0)
        {
            Debug.LogWarning("Cannot show upgrade choices - missing references or no upgrades!");
            return;
        }
        
        ClearCurrentOptions();
        currentUpgradeChoices = upgrades;
        
        if (ExperienceSystem.Instance != null && levelTitleText != null)
        {
            int currentLevel = ExperienceSystem.Instance.CurrentLevel;
            levelTitleText.text = $"★ LEVEL {currentLevel} ★\nChoose Your Upgrade";
        }
        
        foreach (LevelUpgradeData upgrade in upgrades)
        {
            CreateUpgradeButton(upgrade);
        }
        
        levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    
    private void CreateUpgradeButton(LevelUpgradeData upgrade)
    {
        if (optionButtonPrefab == null)
        {
            Debug.LogError("Option button prefab is not assigned!");
            return;
        }
        
        GameObject optionObj = Instantiate(optionButtonPrefab, optionsContainer);
        currentOptions.Add(optionObj);
        
        TextMeshProUGUI nameText = optionObj.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descText = optionObj.transform.Find("Description")?.GetComponent<TextMeshProUGUI>();
        Image backgroundImage = optionObj.GetComponent<Image>();
        Button button = optionObj.GetComponent<Button>();
        
        if (nameText != null)
        {
            nameText.text = upgrade.GetFormattedName();
        }
        
        if (descText != null)
        {
            descText.text = GetUpgradeDescription(upgrade);
        }
        
        if (backgroundImage != null)
        {
            backgroundImage.color = GetRarityBackgroundColor(upgrade.rarity);
        }
        
        if (button != null)
        {
            button.onClick.AddListener(() => SelectUpgrade(upgrade));
        }
    }
    
    private string GetUpgradeDescription(LevelUpgradeData upgrade)
    {
        string desc = upgrade.description;
        
        List<string> bonuses = new List<string>();
        
        if (upgrade.damageBonus != 0)
            bonuses.Add($"{(upgrade.damageBonus > 0 ? "+" : "")}{upgrade.damageBonus} Damage");
        
        if (upgrade.maxHealthBonus != 0)
            bonuses.Add($"{(upgrade.maxHealthBonus > 0 ? "+" : "")}{upgrade.maxHealthBonus} Health");
        
        if (upgrade.moveSpeedBonus != 0)
            bonuses.Add($"{(upgrade.moveSpeedBonus > 0 ? "+" : "")}{upgrade.moveSpeedBonus * 100}% Speed");
        
        if (upgrade.critChanceBonus != 0)
            bonuses.Add($"{(upgrade.critChanceBonus > 0 ? "+" : "")}{upgrade.critChanceBonus * 100}% Crit Chance");
        
        if (upgrade.critDamageBonus != 0)
            bonuses.Add($"{(upgrade.critDamageBonus > 0 ? "+" : "")}{upgrade.critDamageBonus * 100}% Crit Damage");
        
        if (upgrade.attackSpeedBonus != 0)
            bonuses.Add($"{(upgrade.attackSpeedBonus > 0 ? "+" : "")}{upgrade.attackSpeedBonus * 100}% Attack Speed");
        
        if (upgrade.attackRangeBonus != 0)
            bonuses.Add($"{(upgrade.attackRangeBonus > 0 ? "+" : "")}{upgrade.attackRangeBonus} Range");
        
        if (bonuses.Count > 0)
        {
            desc += "\n\n" + string.Join("\n", bonuses);
        }
        
        if (upgrade.canStack)
        {
            int currentStacks = UpgradeSystem.Instance.GetUpgradeStacks(upgrade);
            desc += $"\n\n[Stacks: {currentStacks}/{upgrade.maxStacks}]";
        }
        
        return desc;
    }
    
    private Color GetRarityBackgroundColor(LevelUpgradeRarity rarity)
    {
        switch (rarity)
        {
            case LevelUpgradeRarity.Common:
                return new Color(0.2f, 0.2f, 0.2f, 0.9f);
            case LevelUpgradeRarity.Rare:
                return new Color(0.1f, 0.3f, 0.6f, 0.9f);
            case LevelUpgradeRarity.Legendary:
                return new Color(0.6f, 0.4f, 0f, 0.9f);
            default:
                return new Color(0.2f, 0.2f, 0.2f, 0.9f);
        }
    }
    
    private void SelectUpgrade(LevelUpgradeData upgrade)
    {
        if (UpgradeSystem.Instance != null)
        {
            UpgradeSystem.Instance.SelectUpgrade(upgrade);
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
        currentUpgradeChoices.Clear();
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
