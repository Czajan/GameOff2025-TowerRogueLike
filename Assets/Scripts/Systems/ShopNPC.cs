using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum ShopNPCType
{
    WeaponVendor,
    StatUpgradeVendor,
    ConsumableVendor,
    SpecialVendor
}

public class ShopNPC : MonoBehaviour
{
    [Header("NPC Configuration")]
    [SerializeField] private ShopNPCType npcType = ShopNPCType.StatUpgradeVendor;
    [SerializeField] private string npcName = "Merchant";
    [SerializeField] private float interactionRange = 3f;
    
    [Header("Weapon Vendor")]
    [SerializeField] private WeaponData[] availableWeapons;
    
    [Header("Stat Upgrade Vendor")]
    [SerializeField] private UpgradeData[] availableUpgrades;
    
    [Header("UI References")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private SimpleShopUI shopUI;
    
    [Header("Visual Feedback")]
    [SerializeField] private Transform interactionIndicator;
    [SerializeField] private Color highlightColor = Color.yellow;
    
    public UnityEvent<UpgradeData> OnUpgradePurchased = new UnityEvent<UpgradeData>();
    public UnityEvent<WeaponData> OnWeaponPurchased = new UnityEvent<WeaponData>();
    public UnityEvent OnPlayerEnterRange = new UnityEvent();
    public UnityEvent OnPlayerExitRange = new UnityEvent();
    
    private Transform player;
    private bool playerInRange = false;
    private bool shopOpen = false;
    private Renderer npcRenderer;
    private Color originalColor;
    
    private void Awake()
    {
        npcRenderer = GetComponent<Renderer>();
        if (npcRenderer != null)
        {
            originalColor = npcRenderer.material.color;
        }
    }
    
    private void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        if (shopUI != null)
        {
            shopUI.gameObject.SetActive(false);
        }
        
        if (interactionIndicator != null)
        {
            interactionIndicator.gameObject.SetActive(false);
        }
        
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    
    private void Update()
    {
        if (player == null) return;
        
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (distance <= interactionRange && !playerInRange)
        {
            OnPlayerEnteredRange();
        }
        else if (distance > interactionRange && playerInRange)
        {
            OnPlayerExitedRange();
        }
        
        if (Keyboard.current != null)
        {
            if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
            {
                ToggleShop();
            }
            
            if (shopOpen && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseShop();
            }
        }
    }
    
    private void OnPlayerEnteredRange()
    {
        playerInRange = true;
        
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
        
        if (interactionIndicator != null)
        {
            interactionIndicator.gameObject.SetActive(true);
        }
        
        if (npcRenderer != null)
        {
            npcRenderer.material.color = highlightColor;
        }
        
        OnPlayerEnterRange?.Invoke();
        Debug.Log($"Near {npcName}. Press E to interact.");
    }
    
    private void OnPlayerExitedRange()
    {
        playerInRange = false;
        
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        if (interactionIndicator != null)
        {
            interactionIndicator.gameObject.SetActive(false);
        }
        
        if (npcRenderer != null)
        {
            npcRenderer.material.color = originalColor;
        }
        
        if (shopOpen)
        {
            CloseShop();
        }
        
        OnPlayerExitRange?.Invoke();
    }
    
    private void ToggleShop()
    {
        if (shopOpen)
        {
            CloseShop();
        }
        else
        {
            OpenShop();
        }
    }
    
    public void OpenShop()
    {
        if (shopUI != null && !shopOpen)
        {
            shopOpen = true;
            shopUI.OpenShop(this);
            Debug.Log($"Opened {npcName}'s shop ({npcType})");
        }
    }
    
    public void CloseShop()
    {
        if (shopUI != null && shopOpen)
        {
            shopUI.CloseShop();
            shopOpen = false;
            Debug.Log($"Closed {npcName}'s shop");
        }
    }
    
    public bool TryPurchaseUpgrade(UpgradeData upgrade, int currentLevel)
    {
        if (upgrade == null || npcType != ShopNPCType.StatUpgradeVendor)
        {
            Debug.Log("This NPC doesn't sell stat upgrades!");
            return false;
        }
        
        if (currentLevel >= upgrade.maxLevel)
        {
            Debug.Log($"{upgrade.upgradeName} is already at max level!");
            return false;
        }
        
        int cost = upgrade.GetCostForLevel(currentLevel);
        
        if (GameProgressionManager.Instance != null && GameProgressionManager.Instance.SpendCurrency(cost))
        {
            ApplyUpgrade(upgrade.upgradeType);
            OnUpgradePurchased?.Invoke(upgrade);
            Debug.Log($"Purchased {upgrade.upgradeName} from {npcName} for {cost} currency!");
            return true;
        }
        
        Debug.Log($"Not enough currency! Need {cost}");
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
        if (weapon == null || npcType != ShopNPCType.WeaponVendor)
        {
            Debug.Log("This NPC doesn't sell weapons!");
            return false;
        }
        
        if (GameProgressionManager.Instance != null && GameProgressionManager.Instance.SpendCurrency(weapon.purchaseCost))
        {
            if (WeaponSystem.Instance != null)
            {
                WeaponSystem.Instance.EquipWeapon(weapon);
            }
            
            OnWeaponPurchased?.Invoke(weapon);
            Debug.Log($"Purchased weapon: {weapon.weaponName} from {npcName} for {weapon.purchaseCost} currency!");
            return true;
        }
        
        Debug.Log($"Not enough currency! Need {weapon.purchaseCost}");
        return false;
    }
    
    public ShopNPCType GetNPCType() => npcType;
    public string GetNPCName() => npcName;
    public WeaponData[] GetAvailableWeapons() => availableWeapons;
    public UpgradeData[] GetAvailableUpgrades() => availableUpgrades;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
