using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelUpgradeAssetGenerator : EditorWindow
{
    [MenuItem("Tools/Generate Level Upgrade Assets")]
    public static void GenerateLevelUpgradeAssets()
    {
        string folderPath = "Assets/Data/LevelUpgrades";
        
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            string dataFolder = "Assets/Data";
            if (!AssetDatabase.IsValidFolder(dataFolder))
            {
                AssetDatabase.CreateFolder("Assets", "Data");
            }
            AssetDatabase.CreateFolder(dataFolder, "LevelUpgrades");
        }
        
        int createdCount = 0;
        
        createdCount += CreateUpgrade(folderPath, "PowerSurge", "Power Surge", 
            "Significantly increases your damage output", 
            LevelUpgradeRarity.Common, LevelUpgradeType.StatBoost,
            damageBonus: 10f);
        
        createdCount += CreateUpgrade(folderPath, "Vitality", "Vitality", 
            "Grants a large health boost to survive longer", 
            LevelUpgradeRarity.Common, LevelUpgradeType.StatBoost,
            maxHealthBonus: 50f);
        
        createdCount += CreateUpgrade(folderPath, "SwiftStrike", "Swift Strike", 
            "Attack faster to overwhelm your enemies", 
            LevelUpgradeRarity.Common, LevelUpgradeType.StatBoost,
            attackSpeedBonus: 0.15f);
        
        createdCount += CreateUpgrade(folderPath, "Velocity", "Velocity", 
            "Grants increased movement speed", 
            LevelUpgradeRarity.Common, LevelUpgradeType.StatBoost,
            moveSpeedBonus: 0.1f);
        
        createdCount += CreateUpgrade(folderPath, "CriticalMastery", "Critical Mastery", 
            "Increases critical hit chance and damage", 
            LevelUpgradeRarity.Rare, LevelUpgradeType.StatBoost,
            critChanceBonus: 0.1f, critDamageBonus: 0.3f, maxStacks: 3);
        
        createdCount += CreateUpgrade(folderPath, "VampiricTouch", "Vampiric Touch", 
            "Heal for 15% of damage dealt", 
            LevelUpgradeRarity.Rare, LevelUpgradeType.Functionality,
            lifesteal: true, lifestealPercent: 0.15f, maxStacks: 3);
        
        createdCount += CreateUpgrade(folderPath, "AirWalker", "Air Walker", 
            "Grants the ability to jump twice in mid-air", 
            LevelUpgradeRarity.Rare, LevelUpgradeType.Functionality,
            doubleJump: true, canStack: false, maxStacks: 1);
        
        createdCount += CreateUpgrade(folderPath, "ShadowStep", "Shadow Step", 
            "Grants a quick dash ability to evade enemies", 
            LevelUpgradeRarity.Rare, LevelUpgradeType.Functionality,
            dash: true, canStack: false, maxStacks: 1);
        
        createdCount += CreateUpgrade(folderPath, "BerserkRage", "Berserk Rage", 
            "Massive damage and speed boost, but reduced health", 
            LevelUpgradeRarity.Legendary, LevelUpgradeType.StatBoost,
            damageBonus: 25f, maxHealthBonus: -30f, moveSpeedBonus: 0.2f, 
            attackSpeedBonus: 0.25f, maxStacks: 2);
        
        createdCount += CreateUpgrade(folderPath, "Executioner", "Executioner", 
            "Extreme critical damage at the cost of hit chance", 
            LevelUpgradeRarity.Legendary, LevelUpgradeType.StatBoost,
            critChanceBonus: -0.05f, critDamageBonus: 1f, maxStacks: 2);
        
        createdCount += CreateUpgrade(folderPath, "GlassCannonI", "Glass Cannon", 
            "Huge damage increase but lower max health", 
            LevelUpgradeRarity.Rare, LevelUpgradeType.StatBoost,
            damageBonus: 20f, maxHealthBonus: -25f);
        
        createdCount += CreateUpgrade(folderPath, "Fortress", "Fortress", 
            "Massive health increase but slower movement", 
            LevelUpgradeRarity.Rare, LevelUpgradeType.StatBoost,
            maxHealthBonus: 80f, moveSpeedBonus: -0.1f);
        
        createdCount += CreateUpgrade(folderPath, "Sharpshooter", "Sharpshooter", 
            "Increased attack range and critical chance", 
            LevelUpgradeRarity.Common, LevelUpgradeType.StatBoost,
            attackRangeBonus: 1f, critChanceBonus: 0.05f);
        
        createdCount += CreateUpgrade(folderPath, "Whirlwind", "Whirlwind", 
            "Greatly increases attack speed", 
            LevelUpgradeRarity.Common, LevelUpgradeType.StatBoost,
            attackSpeedBonus: 0.25f);
        
        createdCount += CreateUpgrade(folderPath, "IronSkin", "Iron Skin", 
            "Moderate health boost with no downsides", 
            LevelUpgradeRarity.Common, LevelUpgradeType.StatBoost,
            maxHealthBonus: 30f);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"<color=green>✓ Generated {createdCount} Level Upgrade assets in {folderPath}</color>");
        EditorUtility.DisplayDialog("Success", $"Created {createdCount} Level Upgrade assets!\n\nCheck: {folderPath}", "OK");
    }
    
    private static int CreateUpgrade(
        string folderPath, 
        string fileName, 
        string upgradeName, 
        string description, 
        LevelUpgradeRarity rarity, 
        LevelUpgradeType upgradeType,
        float damageBonus = 0f,
        float maxHealthBonus = 0f,
        float moveSpeedBonus = 0f,
        float critChanceBonus = 0f,
        float critDamageBonus = 0f,
        float attackSpeedBonus = 0f,
        float attackRangeBonus = 0f,
        bool doubleJump = false,
        bool dash = false,
        bool lifesteal = false,
        float lifestealPercent = 0f,
        bool canStack = true,
        int maxStacks = 5)
    {
        string assetPath = $"{folderPath}/{fileName}.asset";
        
        if (AssetDatabase.LoadAssetAtPath<LevelUpgradeData>(assetPath) != null)
        {
            Debug.Log($"Asset already exists, skipping: {fileName}");
            return 0;
        }
        
        LevelUpgradeData upgrade = ScriptableObject.CreateInstance<LevelUpgradeData>();
        upgrade.upgradeName = upgradeName;
        upgrade.description = description;
        upgrade.rarity = rarity;
        upgrade.upgradeType = upgradeType;
        upgrade.canStack = canStack;
        upgrade.maxStacks = maxStacks;
        upgrade.damageBonus = damageBonus;
        upgrade.maxHealthBonus = maxHealthBonus;
        upgrade.moveSpeedBonus = moveSpeedBonus;
        upgrade.critChanceBonus = critChanceBonus;
        upgrade.critDamageBonus = critDamageBonus;
        upgrade.attackSpeedBonus = attackSpeedBonus;
        upgrade.attackRangeBonus = attackRangeBonus;
        upgrade.grantsDoubleJump = doubleJump;
        upgrade.grantsDash = dash;
        upgrade.grantsLifesteal = lifesteal;
        upgrade.lifestealPercent = lifestealPercent;
        
        AssetDatabase.CreateAsset(upgrade, assetPath);
        Debug.Log($"Created: {fileName}");
        return 1;
    }
}
