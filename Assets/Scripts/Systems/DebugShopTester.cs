using UnityEngine;
using UnityEngine.InputSystem;

public class DebugShopTester : MonoBehaviour
{
    [Header("Quick Test Controls")]
    [SerializeField] private UpgradeData[] upgradesForTesting;
    [SerializeField] private WeaponData[] weaponsForTesting;
    
    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                AddTestCurrency();
            }
            
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                BuyUpgrade(UpgradeType.MoveSpeed);
            }
            
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                BuyUpgrade(UpgradeType.MaxHealth);
            }
            
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                BuyUpgrade(UpgradeType.Damage);
            }
            
            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                BuyUpgrade(UpgradeType.CritChance);
            }
            
            if (Keyboard.current.digit5Key.wasPressedThisFrame)
            {
                BuyUpgrade(UpgradeType.CritDamage);
            }
            
            if (Keyboard.current.digit6Key.wasPressedThisFrame)
            {
                BuyUpgrade(UpgradeType.AttackRange);
            }
            
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                LogCurrentStats();
            }
            
            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                ShowHelp();
            }
        }
    }
    
    private void AddTestCurrency()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.AddCurrency(100);
            Debug.Log("✓ Added 100 currency for testing!");
        }
    }
    
    private void BuyUpgrade(UpgradeType type)
    {
        if (PlayerStats.Instance == null || GameProgressionManager.Instance == null)
        {
            Debug.LogWarning("PlayerStats or GameProgressionManager not found!");
            return;
        }
        
        UpgradeData upgrade = FindUpgradeByType(type);
        if (upgrade == null)
        {
            Debug.LogWarning($"No upgrade found for type {type}. Assign upgrades to the tester!");
            return;
        }
        
        int currentLevel = GetCurrentLevel(type);
        
        if (currentLevel >= upgrade.maxLevel)
        {
            Debug.Log($"✗ {upgrade.upgradeName} is already at MAX level ({upgrade.maxLevel})!");
            return;
        }
        
        int cost = upgrade.GetCostForLevel(currentLevel);
        
        if (GameProgressionManager.Instance.SpendCurrency(cost))
        {
            ApplyUpgrade(type);
            Debug.Log($"✓ Purchased {upgrade.upgradeName}! Level {currentLevel} → {currentLevel + 1} (Cost: ${cost})");
        }
        else
        {
            int current = GameProgressionManager.Instance.Currency;
            Debug.Log($"✗ Not enough currency! Have ${current}, need ${cost}");
        }
    }
    
    private UpgradeData FindUpgradeByType(UpgradeType type)
    {
        foreach (var upgrade in upgradesForTesting)
        {
            if (upgrade != null && upgrade.upgradeType == type)
            {
                return upgrade;
            }
        }
        return null;
    }
    
    private int GetCurrentLevel(UpgradeType type)
    {
        if (PlayerStats.Instance == null) return 0;
        
        switch (type)
        {
            case UpgradeType.MoveSpeed: return PlayerStats.Instance.GetMoveSpeedLevel();
            case UpgradeType.MaxHealth: return PlayerStats.Instance.GetMaxHealthLevel();
            case UpgradeType.Damage: return PlayerStats.Instance.GetDamageLevel();
            case UpgradeType.CritChance: return PlayerStats.Instance.GetCritChanceLevel();
            case UpgradeType.CritDamage: return PlayerStats.Instance.GetCritDamageLevel();
            case UpgradeType.AttackRange: return PlayerStats.Instance.GetAttackRangeLevel();
            default: return 0;
        }
    }
    
    private void ApplyUpgrade(UpgradeType type)
    {
        if (PlayerStats.Instance == null) return;
        
        switch (type)
        {
            case UpgradeType.MoveSpeed:
                PlayerStats.Instance.UpgradeMoveSpeed();
                break;
            case UpgradeType.MaxHealth:
                PlayerStats.Instance.UpgradeMaxHealth();
                break;
            case UpgradeType.Damage:
                PlayerStats.Instance.UpgradeDamage();
                break;
            case UpgradeType.CritChance:
                PlayerStats.Instance.UpgradeCritChance();
                break;
            case UpgradeType.CritDamage:
                PlayerStats.Instance.UpgradeCritDamage();
                break;
            case UpgradeType.AttackRange:
                PlayerStats.Instance.UpgradeAttackRange();
                break;
        }
    }
    
    private void LogCurrentStats()
    {
        if (PlayerStats.Instance == null || GameProgressionManager.Instance == null)
        {
            Debug.LogWarning("Stats not available!");
            return;
        }
        
        Debug.Log("════════════════════════════════");
        Debug.Log("        CURRENT STATS");
        Debug.Log("════════════════════════════════");
        Debug.Log($"Currency: ${GameProgressionManager.Instance.Currency}");
        Debug.Log("────────────────────────────────");
        Debug.Log($"Move Speed:   {PlayerStats.Instance.GetMoveSpeed():F1} (Lv {PlayerStats.Instance.GetMoveSpeedLevel()})");
        Debug.Log($"Max Health:   {PlayerStats.Instance.GetMaxHealth():F0} (Lv {PlayerStats.Instance.GetMaxHealthLevel()})");
        Debug.Log($"Damage:       {PlayerStats.Instance.GetDamage():F0} (Lv {PlayerStats.Instance.GetDamageLevel()})");
        Debug.Log($"Crit Chance:  {(PlayerStats.Instance.GetCritChance() * 100):F1}% (Lv {PlayerStats.Instance.GetCritChanceLevel()})");
        Debug.Log($"Crit Damage:  {PlayerStats.Instance.GetCritDamage():F2}x (Lv {PlayerStats.Instance.GetCritDamageLevel()})");
        Debug.Log($"Attack Range: {PlayerStats.Instance.GetAttackRange():F1} (Lv {PlayerStats.Instance.GetAttackRangeLevel()})");
        Debug.Log("════════════════════════════════");
    }
    
    private void ShowHelp()
    {
        Debug.Log("════════════════════════════════");
        Debug.Log("      DEBUG CONTROLS");
        Debug.Log("════════════════════════════════");
        Debug.Log("[C] - Add 100 currency");
        Debug.Log("[1] - Buy Move Speed upgrade");
        Debug.Log("[2] - Buy Max Health upgrade");
        Debug.Log("[3] - Buy Damage upgrade");
        Debug.Log("[4] - Buy Crit Chance upgrade");
        Debug.Log("[5] - Buy Crit Damage upgrade");
        Debug.Log("[6] - Buy Attack Range upgrade");
        Debug.Log("[L] - Log current stats");
        Debug.Log("[H] - Show this help");
        Debug.Log("════════════════════════════════");
        Debug.Log("Assign upgrade assets in Inspector!");
        Debug.Log("════════════════════════════════");
    }
    
    private void Start()
    {
        Debug.Log("Debug Shop Tester Active! Press [H] for controls.");
    }
}
