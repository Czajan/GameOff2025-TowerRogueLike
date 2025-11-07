using UnityEngine;
using UnityEngine.Events;

public class UpgradeShop : MonoBehaviour
{
    [Header("Available Upgrades")]
    [SerializeField] private UpgradeData[] availableUpgrades;
    
    [Header("Available Weapons")]
    [SerializeField] private WeaponData[] availableWeapons;
    
    [Header("UI References")]
    [SerializeField] private GameObject shopUI;
    
    public UnityEvent<UpgradeData> OnUpgradePurchased = new UnityEvent<UpgradeData>();
    public UnityEvent<WeaponData> OnWeaponPurchased = new UnityEvent<WeaponData>();
    
    private void Start()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
        
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnEnteredBase.AddListener(OpenShop);
            GameProgressionManager.Instance.OnExitedBase.AddListener(CloseShop);
        }
    }
    
    public void OpenShop()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(true);
        }
        
        Time.timeScale = 0f;
        Debug.Log("Shop opened! Time to upgrade.");
    }
    
    public void CloseShop()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
        
        Time.timeScale = 1f;
    }
    
    public bool TryPurchaseUpgrade(UpgradeData upgrade, int currentLevel)
    {
        if (upgrade == null) return false;
        
        if (currentLevel >= upgrade.maxLevel)
        {
            Debug.Log($"{upgrade.upgradeName} is already at max level!");
            return false;
        }
        
        int cost = upgrade.GetCostForLevel(currentLevel);
        
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendEssence(cost))
        {
            ApplyUpgrade(upgrade.upgradeType);
            OnUpgradePurchased?.Invoke(upgrade);
            Debug.Log($"<color=cyan>Purchased {upgrade.upgradeName} for {cost} Essence (meta-currency)!</color>");
            return true;
        }
        
        Debug.Log($"Not enough Essence! Need {cost}");
        return false;
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
    
    public bool TryPurchaseWeapon(WeaponData weapon)
    {
        if (weapon == null) return false;
        
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendEssence(weapon.purchaseCost))
        {
            if (WeaponSystem.Instance != null)
            {
                WeaponSystem.Instance.EquipWeapon(weapon);
            }
            
            OnWeaponPurchased?.Invoke(weapon);
            Debug.Log($"<color=cyan>Purchased weapon: {weapon.weaponName} for {weapon.purchaseCost} Essence!</color>");
            return true;
        }
        
        Debug.Log($"Not enough Essence! Need {weapon.purchaseCost}");
        return false;
    }
    
    public void ExitShop()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.ExitBase();
        }
    }
}
