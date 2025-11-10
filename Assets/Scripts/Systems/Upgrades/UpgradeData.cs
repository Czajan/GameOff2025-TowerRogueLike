using UnityEngine;

public enum UpgradeRarity
{
    Common,
    Rare,
    Legendary
}

public enum UpgradeType
{
    StatBoost,
    Functionality
}

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Game/Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("Basic Info")]
    public string upgradeName = "New Upgrade";
    [TextArea(3, 5)]
    public string description = "Upgrade description";
    public Sprite icon;
    public UpgradeRarity rarity = UpgradeRarity.Common;
    public UpgradeType upgradeType = UpgradeType.StatBoost;
    
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
            case UpgradeRarity.Common: return "#CCCCCC";
            case UpgradeRarity.Rare: return "#4A90E2";
            case UpgradeRarity.Legendary: return "#FFD700";
            default: return "#FFFFFF";
        }
    }
    
    public string GetFormattedName()
    {
        return $"<color={GetRarityColor()}>{upgradeName}</color>";
    }
}
