using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance { get; private set; }
    
    [Header("Upgrade Pool")]
    [SerializeField] private List<LevelUpgradeData> allUpgrades = new List<LevelUpgradeData>();
    
    [Header("Offering Settings")]
    [SerializeField] private int upgradesPerOffer = 3;
    [SerializeField] private bool allowDuplicates = false;
    
    [Header("Rarity Weights")]
    [SerializeField] private float commonWeight = 70f;
    [SerializeField] private float rareWeight = 25f;
    [SerializeField] private float legendaryWeight = 5f;
    
    [Header("Events")]
    public UnityEvent<List<LevelUpgradeData>> OnUpgradesOffered = new UnityEvent<List<LevelUpgradeData>>();
    public UnityEvent<LevelUpgradeData> OnUpgradeSelected = new UnityEvent<LevelUpgradeData>();
    
    private Dictionary<LevelUpgradeData, int> acquiredUpgrades = new Dictionary<LevelUpgradeData, int>();
    
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
            RunStateManager.Instance.OnRunStarted.AddListener(ResetUpgrades);
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
            RunStateManager.Instance.OnRunStarted.RemoveListener(ResetUpgrades);
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
        List<LevelUpgradeData> offeredUpgrades = SelectRandomUpgrades(upgradesPerOffer);
        
        if (offeredUpgrades.Count > 0)
        {
            Debug.Log($"<color=cyan>Offering {offeredUpgrades.Count} upgrade choices!</color>");
            OnUpgradesOffered?.Invoke(offeredUpgrades);
        }
    }
    
    private List<LevelUpgradeData> SelectRandomUpgrades(int count)
    {
        List<LevelUpgradeData> availableUpgrades = GetAvailableUpgrades();
        List<LevelUpgradeData> selected = new List<LevelUpgradeData>();
        
        if (availableUpgrades.Count == 0)
        {
            Debug.LogWarning("No upgrades available!");
            return selected;
        }
        
        for (int i = 0; i < count && availableUpgrades.Count > 0; i++)
        {
            LevelUpgradeData upgrade = SelectWeightedRandomUpgrade(availableUpgrades);
            selected.Add(upgrade);
            
            if (!allowDuplicates)
            {
                availableUpgrades.Remove(upgrade);
            }
        }
        
        return selected;
    }
    
    private List<LevelUpgradeData> GetAvailableUpgrades()
    {
        if (allowDuplicates)
        {
            return new List<LevelUpgradeData>(allUpgrades);
        }
        
        List<LevelUpgradeData> available = new List<LevelUpgradeData>();
        
        foreach (LevelUpgradeData upgrade in allUpgrades)
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
    
    private LevelUpgradeData SelectWeightedRandomUpgrade(List<LevelUpgradeData> upgrades)
    {
        Dictionary<LevelUpgradeRarity, float> rarityWeights = new Dictionary<LevelUpgradeRarity, float>
        {
            { LevelUpgradeRarity.Common, commonWeight },
            { LevelUpgradeRarity.Rare, rareWeight },
            { LevelUpgradeRarity.Legendary, legendaryWeight }
        };
        
        float totalWeight = 0f;
        foreach (LevelUpgradeData upgrade in upgrades)
        {
            totalWeight += rarityWeights[upgrade.rarity];
        }
        
        float randomValue = Random.value * totalWeight;
        float cumulativeWeight = 0f;
        
        foreach (LevelUpgradeData upgrade in upgrades)
        {
            cumulativeWeight += rarityWeights[upgrade.rarity];
            if (randomValue <= cumulativeWeight)
            {
                return upgrade;
            }
        }
        
        return upgrades[upgrades.Count - 1];
    }
    
    public void SelectUpgrade(LevelUpgradeData upgrade)
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
    
    private void ApplyUpgrade(LevelUpgradeData upgrade)
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
    
    public int GetUpgradeStacks(LevelUpgradeData upgrade)
    {
        return acquiredUpgrades.ContainsKey(upgrade) ? acquiredUpgrades[upgrade] : 0;
    }
    
    public bool HasUpgrade(LevelUpgradeData upgrade)
    {
        return acquiredUpgrades.ContainsKey(upgrade);
    }
    
    public void ResetUpgrades()
    {
        acquiredUpgrades.Clear();
        Debug.Log("Upgrades reset for new run.");
    }
    
    public Dictionary<LevelUpgradeData, int> GetAcquiredUpgrades()
    {
        return new Dictionary<LevelUpgradeData, int>(acquiredUpgrades);
    }
}
