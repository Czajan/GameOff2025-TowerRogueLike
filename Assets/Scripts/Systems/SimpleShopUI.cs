using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SimpleShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform itemListContainer;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private TextMeshProUGUI shopTitleText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private Button closeButton;
    
    private ShopNPC currentNPC;
    
    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseShop);
        }
        
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnCurrencyChanged.AddListener(UpdateCurrencyDisplay);
        }
        
        gameObject.SetActive(false);
    }
    
    public void OpenShop(ShopNPC npc)
    {
        currentNPC = npc;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(OpenShopCoroutine(npc));
    }
    
    private IEnumerator OpenShopCoroutine(ShopNPC npc)
    {
        if (shopTitleText != null)
        {
            shopTitleText.text = npc.GetNPCName();
        }
        
        PopulateShop();
        UpdateCurrencyDisplay(GameProgressionManager.Instance?.Currency ?? 0);
        
        yield return null;
        
        if (itemListContainer != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(itemListContainer as RectTransform);
        }
        
        Canvas.ForceUpdateCanvases();
        
        Time.timeScale = 0f;
    }
    
    private void PopulateShop()
    {
        if (itemListContainer == null || currentNPC == null) return;
        
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in itemListContainer)
        {
            children.Add(child.gameObject);
        }
        
        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }
        
        if (currentNPC.GetNPCType() == ShopNPCType.WeaponVendor)
        {
            Debug.Log("Populating Weapons Shop");
            PopulateWeapons();
        }
        else if (currentNPC.GetNPCType() == ShopNPCType.StatUpgradeVendor)
        {
            Debug.Log("Populating Stat Upgrades Shop");
            PopulateUpgrades();
        }
    }
    
    private void PopulateWeapons()
    {
        WeaponData[] weapons = currentNPC.GetAvailableWeapons();
        
        Debug.Log($"Populating {weapons.Length} weapons");
        
        foreach (WeaponData weapon in weapons)
        {
            if (weapon == null) continue;
            
            GameObject itemObj = CreateItemButton();
            
            Transform iconTransform = itemObj.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null && weapon.icon != null)
                {
                    iconImage.sprite = weapon.icon;
                    iconImage.enabled = true;
                }
                else if (iconImage != null)
                {
                    iconImage.enabled = false;
                }
            }
            
            TextMeshProUGUI nameText = itemObj.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descText = itemObj.transform.Find("Description")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI costText = itemObj.transform.Find("Cost")?.GetComponent<TextMeshProUGUI>();
            Button buyButton = itemObj.GetComponent<Button>();
            
            if (nameText != null)
                nameText.text = weapon.weaponName;
            
            if (descText != null)
                descText.text = weapon.description;
            
            if (costText != null)
                costText.text = $"${weapon.purchaseCost}";
            
            if (buyButton != null)
            {
                buyButton.onClick.AddListener(() => PurchaseWeapon(weapon));
            }
            
            Debug.Log($"Created weapon button: {weapon.weaponName} - ${weapon.purchaseCost}");
        }
    }
    
    private void PopulateUpgrades()
    {
        UpgradeData[] upgrades = currentNPC.GetAvailableUpgrades();
        
        Debug.Log($"Populating {upgrades.Length} upgrades");
        
        foreach (UpgradeData upgrade in upgrades)
        {
            if (upgrade == null) continue;
            
            int currentLevel = GetCurrentUpgradeLevel(upgrade.upgradeType);
            int cost = upgrade.GetCostForLevel(currentLevel);
            
            GameObject itemObj = CreateItemButton();
            
            Transform iconTransform = itemObj.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null && upgrade.icon != null)
                {
                    iconImage.sprite = upgrade.icon;
                    iconImage.enabled = true;
                }
                else if (iconImage != null)
                {
                    iconImage.enabled = false;
                }
            }
            
            TextMeshProUGUI nameText = itemObj.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descText = itemObj.transform.Find("Description")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI costText = itemObj.transform.Find("Cost")?.GetComponent<TextMeshProUGUI>();
            Button buyButton = itemObj.GetComponent<Button>();
            
            if (nameText != null)
                nameText.text = $"{upgrade.upgradeName} (Lv {currentLevel})";
            
            if (descText != null)
                descText.text = upgrade.description;
            
            if (costText != null)
            {
                if (currentLevel >= upgrade.maxLevel)
                    costText.text = "MAX";
                else
                    costText.text = $"${cost}";
            }
            
            if (buyButton != null)
            {
                buyButton.onClick.AddListener(() => PurchaseUpgrade(upgrade));
                buyButton.interactable = currentLevel < upgrade.maxLevel;
            }
            
            Debug.Log($"Created upgrade button: {upgrade.upgradeName} Lv{currentLevel} - ${cost}");
        }
    }
    
    private GameObject CreateItemButton()
    {
        if (itemButtonPrefab == null)
        {
            Debug.LogError("itemButtonPrefab is NULL! Cannot create item button.");
            return null;
        }
        
        if (itemListContainer == null)
        {
            Debug.LogError("itemListContainer is NULL! Cannot parent item button.");
            return null;
        }
        
        GameObject itemObj = Instantiate(itemButtonPrefab, itemListContainer);
        
        if (itemObj == null)
        {
            Debug.LogError("Failed to instantiate itemButtonPrefab!");
            return null;
        }
        
        Debug.Log($"Created button: {itemObj.name}, parent: {itemListContainer.name}, active: {itemObj.activeSelf}");
        
        LayoutElement layoutElement = itemObj.GetComponent<LayoutElement>();
        if (layoutElement == null)
        {
            layoutElement = itemObj.AddComponent<LayoutElement>();
        }
        layoutElement.preferredHeight = 80;
        layoutElement.minHeight = 80;
        
        return itemObj;
    }
    
    private void PurchaseWeapon(WeaponData weapon)
    {
        if (currentNPC != null && currentNPC.TryPurchaseWeapon(weapon))
        {
            Debug.Log($"Successfully purchased {weapon.weaponName}!");
            PopulateShop();
        }
    }
    
    private void PurchaseUpgrade(UpgradeData upgrade)
    {
        if (currentNPC != null)
        {
            int currentLevel = GetCurrentUpgradeLevel(upgrade.upgradeType);
            
            if (currentNPC.TryPurchaseUpgrade(upgrade, currentLevel))
            {
                Debug.Log($"Successfully upgraded {upgrade.upgradeName}!");
                PopulateShop();
            }
        }
    }
    
    private int GetCurrentUpgradeLevel(UpgradeType type)
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
    
    private void UpdateCurrencyDisplay(int currency)
    {
        if (currencyText != null)
        {
            currencyText.text = $"Currency: ${currency}";
        }
    }
    
    private void CloseShop()
    {
        if (currentNPC != null)
        {
            currentNPC.CloseShop();
        }
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnCurrencyChanged.RemoveListener(UpdateCurrencyDisplay);
        }
    }
}
