using UnityEngine;

public enum LevelUpgradeRarity
{
    Common,
    Rare,
    Legendary
}

public enum LevelUpgradeType
{
    StatBoost,
    Functionality
}

[CreateAssetMenu(fileName = "New Level Upgrade", menuName = "Game/Level Upgrade")]
public class LevelUpgradeData : ScriptableObject
{
    [Header("Basic Info")]
    public string upgradeName = "New Upgrade";
    [TextArea(3, 5)]
    public string description = "Upgrade description";
    public Sprite icon;
    public LevelUpgradeRarity rarity = LevelUpgradeRarity.Common;
    public LevelUpgradeType upgradeType = LevelUpgradeType.StatBoost;
    
    [Header("Stack Settings")]
    public bool canStack = true;
    public int maxStacks = 5;
    
    [Header("Stat Modifications")]
    public float damageBonus = 0f;
    public float maxHealthBonus = 0f;
    public float moveSpeedBonus = 0f;
    public float critChanceBonus = 0f;
    public float critDamageBonus = 0f;
    public float attackSpeedBonus = 0f;
    public float attackRangeBonus = 0f;
    
    [Header("Special Effects")]
    public bool grantsDoubleJump = false;
    public bool grantsDash = false;
    public bool grantsLifesteal = false;
    public float lifestealPercent = 0f;
    public bool grantsThorns = false;
    public float thornsPercent = 0f;
    public bool grantsExplosiveHits = false;
    public float explosionRadius = 0f;
    public float explosionDamagePercent = 0f;
    
    public string GetRarityColor()
    {
        switch (rarity)
        {
            case LevelUpgradeRarity.Common: return "#CCCCCC";
            case LevelUpgradeRarity.Rare: return "#4A90E2";
            case LevelUpgradeRarity.Legendary: return "#FFD700";
            default: return "#FFFFFF";
        }
    }
    
    public string GetFormattedName()
    {
        return $"<color={GetRarityColor()}>{upgradeName}</color>";
    }
}
