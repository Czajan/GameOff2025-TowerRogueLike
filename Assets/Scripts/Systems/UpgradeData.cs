using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Game/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Upgrade Info")]
    public string upgradeName = "Speed Boost";
    public string description = "Increases movement speed";
    public Sprite icon;
    public UpgradeType upgradeType;
    
    [Header("Cost")]
    public int baseCost = 50;
    public float costIncreasePerLevel = 1.5f;
    
    [Header("Max Level")]
    public int maxLevel = 10;
    
    public int GetCostForLevel(int currentLevel)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costIncreasePerLevel, currentLevel));
    }
}

public enum UpgradeType
{
    MoveSpeed,
    MaxHealth,
    Damage,
    CritChance,
    CritDamage,
    AttackRange
}
