using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance { get; private set; }
    
    [Header("Upgrade Pool")]
    [SerializeField] private List<UpgradeData> allUpgrades = new List<UpgradeData>();
    
    [Header("Offering Settings")]
    [SerializeField] private int upgradesPerOffer = 3;
    [SerializeField] private bool allowDuplicates = false;
    
    [Header("Rarity Weights")]
    [SerializeField] private float commonWeight = 70f;
    [SerializeField] private float rareWeight = 25f;
    [SerializeField] private float legendaryWeight = 5f;
    
    [Header("Events")]
    public UnityEvent<List<UpgradeData>> OnUpgradesOffered = new UnityEvent<List<UpgradeData>>();
    public UnityEvent<UpgradeData> OnUpgradeSelected = new UnityEvent<UpgradeData>();
    
    private Dictionary<UpgradeData, int> acquiredUpgrades = new Dictionary<UpgradeData, int>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.OnLevelUp.AddListener(OnPlayerLevelUp);
        }
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted += ResetUpgrades;
        }
    }
    
    private void OnDestroy()
    {
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.OnLevelUp.RemoveListener(OnPlayerLevelUp);
        }
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted -= ResetUpgrades;
        }
    }
    
    private void OnPlayerLevelUp(int newLevel)
    {
        if (ShouldOfferUpgrade(newLevel))
        {
            OfferUpgradeChoices();
        }
    }
    
    private bool ShouldOfferUpgrade(int level)
    {
        return level % 5 == 0;
    }
    
    public void OfferUpgradeChoices()
    {
        List<UpgradeData> offeredUpgrades = SelectRandomUpgrades(upgradesPerOffer);
        
        if (offeredUpgrades.Count > 0)
        {
            Debug.Log($"<color=cyan>Offering {offeredUpgrades.Count} upgrade choices!</color>");
            OnUpgradesOffered?.Invoke(offeredUpgrades);
        }
    }
    
    private List<UpgradeData> SelectRandomUpgrades(int count)
    {
        List<UpgradeData> availableUpgrades = GetAvailableUpgrades();
        List<UpgradeData> selected = new List<UpgradeData>();
        
        if (availableUpgrades.Count == 0)
        {
            Debug.LogWarning("No upgrades available!");
            return selected;
        }
        
        for (int i = 0; i < count && availableUpgrades.Count > 0; i++)
        {
            UpgradeData upgrade = SelectWeightedRandomUpgrade(availableUpgrades);
            selected.Add(upgrade);
            
            if (!allowDuplicates)
            {
                availableUpgrades.Remove(upgrade);
            }
        }
        
        return selected;
    }
    
    private List<UpgradeData> GetAvailableUpgrades()
    {
        if (allowDuplicates)
        {
            return new List<UpgradeData>(allUpgrades);
        }
        
        List<UpgradeData> available = new List<UpgradeData>();
        
        foreach (UpgradeData upgrade in allUpgrades)
        {
            int currentStacks = GetUpgradeStacks(upgrade);
            
            if (!acquiredUpgrades.ContainsKey(upgrade))
            {
                available.Add(upgrade);
            }
            else if (upgrade.canStack && currentStacks < upgrade.maxStacks)
            {
                available.Add(upgrade);
            }
        }
        
        return available;
    }
    
    private UpgradeData SelectWeightedRandomUpgrade(List<UpgradeData> upgrades)
    {
        Dictionary<UpgradeRarity, float> rarityWeights = new Dictionary<UpgradeRarity, float>
        {
            { UpgradeRarity.Common, commonWeight },
            { UpgradeRarity.Rare, rareWeight },
            { UpgradeRarity.Legendary, legendaryWeight }
        };
        
        float totalWeight = 0f;
        foreach (UpgradeData upgrade in upgrades)
        {
            totalWeight += rarityWeights[upgrade.rarity];
        }
        
        float randomValue = Random.value * totalWeight;
        float cumulativeWeight = 0f;
        
        foreach (UpgradeData upgrade in upgrades)
        {
            cumulativeWeight += rarityWeights[upgrade.rarity];
            if (randomValue <= cumulativeWeight)
            {
                return upgrade;
            }
        }
        
        return upgrades[upgrades.Count - 1];
    }
    
    public void SelectUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogError("Cannot select null upgrade!");
            return;
        }
        
        if (acquiredUpgrades.ContainsKey(upgrade))
        {
            acquiredUpgrades[upgrade]++;
        }
        else
        {
            acquiredUpgrades[upgrade] = 1;
        }
        
        ApplyUpgrade(upgrade);
        OnUpgradeSelected?.Invoke(upgrade);
        
        int stacks = acquiredUpgrades[upgrade];
        Debug.Log($"<color=yellow>★ Selected: {upgrade.upgradeName} (Stack {stacks})</color>");
    }
    
    private void ApplyUpgrade(UpgradeData upgrade)
    {
        if (PlayerStats.Instance != null)
        {
            if (upgrade.damageBonus != 0)
                PlayerStats.Instance.AddTemporaryDamage(upgrade.damageBonus);
            
            if (upgrade.maxHealthBonus != 0)
                PlayerStats.Instance.AddTemporaryMaxHealth(upgrade.maxHealthBonus);
            
            if (upgrade.moveSpeedBonus != 0)
                PlayerStats.Instance.AddTemporaryMoveSpeed(upgrade.moveSpeedBonus);
            
            if (upgrade.critChanceBonus != 0)
                PlayerStats.Instance.AddTemporaryCritChance(upgrade.critChanceBonus);
            
            if (upgrade.critDamageBonus != 0)
                PlayerStats.Instance.AddTemporaryCritDamage(upgrade.critDamageBonus);
            
            if (upgrade.attackSpeedBonus != 0)
                PlayerStats.Instance.AddTemporaryAttackSpeed(upgrade.attackSpeedBonus);
        }
        
        if (upgrade.grantsDoubleJump)
        {
            PlayerController controller = FindFirstObjectByType<PlayerController>();
            if (controller != null)
            {
                controller.EnableDoubleJump();
            }
        }
        
        if (upgrade.grantsDash)
        {
            PlayerController controller = FindFirstObjectByType<PlayerController>();
            if (controller != null)
            {
                controller.EnableDash();
            }
        }
    }
    
    public int GetUpgradeStacks(UpgradeData upgrade)
    {
        return acquiredUpgrades.ContainsKey(upgrade) ? acquiredUpgrades[upgrade] : 0;
    }
    
    public bool HasUpgrade(UpgradeData upgrade)
    {
        return acquiredUpgrades.ContainsKey(upgrade);
    }
    
    public void ResetUpgrades()
    {
        acquiredUpgrades.Clear();
        Debug.Log("Upgrades reset for new run.");
    }
    
    public Dictionary<UpgradeData, int> GetAcquiredUpgrades()
    {
        return new Dictionary<UpgradeData, int>(acquiredUpgrades);
    }
}
