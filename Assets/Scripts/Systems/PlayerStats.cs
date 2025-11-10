using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    
    [Header("Base Stats")]
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float baseCritChance = 0.05f;
    [SerializeField] private float baseCritDamage = 1.5f;
    [SerializeField] private float baseAttackRange = 2f;
    
    [Header("Upgrade Levels")]
    [SerializeField] private int moveSpeedLevel = 0;
    [SerializeField] private int maxHealthLevel = 0;
    [SerializeField] private int damageLevel = 0;
    [SerializeField] private int critChanceLevel = 0;
    [SerializeField] private int critDamageLevel = 0;
    [SerializeField] private int attackRangeLevel = 0;
    
    [Header("Upgrade Values per Level")]
    [SerializeField] private float moveSpeedPerLevel = 0.5f;
    [SerializeField] private float healthPerLevel = 20f;
    [SerializeField] private float damagePerLevel = 5f;
    [SerializeField] private float critChancePerLevel = 0.05f;
    [SerializeField] private float critDamagePerLevel = 0.25f;
    [SerializeField] private float attackRangePerLevel = 0.5f;
    
    [Header("Zone Bonuses")]
    private float zoneDamageBonus = 0f;
    private float zoneAttackSpeedBonus = 0f;
    private float zoneMoveSpeedBonus = 0f;
    
    [Header("Temporary In-Run Bonuses")]
    private float tempMaxHealth = 0f;
    private float tempDamage = 0f;
    private float tempMoveSpeed = 0f;
    private float tempCritChance = 0f;
    private float tempCritDamage = 0f;
    private float tempAttackSpeed = 0f;
    
    public UnityEvent OnStatsChanged = new UnityEvent();
    
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
        LoadUpgradeLevelsFromSave();
        ApplyStatsToPlayer();
        
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.OnLevelUp.AddListener(OnPlayerLevelUp);
        }
    }
    
    private void OnDestroy()
    {
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.OnLevelUp.RemoveListener(OnPlayerLevelUp);
        }
    }
    
    private void OnPlayerLevelUp(int newLevel)
    {
        if (newLevel % 5 == 0)
        {
            Debug.Log($"<color=yellow>★ MILESTONE LEVEL {newLevel}! Awaiting player upgrade choice...</color>");
        }
        else
        {
            ApplyAutomaticLevelUpBonuses(newLevel);
        }
    }
    
    private void ApplyAutomaticLevelUpBonuses(int level)
    {
        AddTemporaryDamage(2f);
        AddTemporaryMaxHealth(10f);
        AddTemporaryMoveSpeed(0.02f);
        
        Debug.Log($"<color=cyan>★ LEVEL {level} - Auto bonuses applied: +2 Damage, +10 HP, +2% Speed</color>");
        OnStatsChanged?.Invoke();
    }
    
    private void LoadUpgradeLevelsFromSave()
    {
        if (SaveSystem.Instance != null)
        {
            moveSpeedLevel = SaveSystem.Instance.GetMoveSpeedLevel();
            maxHealthLevel = SaveSystem.Instance.GetMaxHealthLevel();
            damageLevel = SaveSystem.Instance.GetDamageLevel();
            critChanceLevel = SaveSystem.Instance.GetCritChanceLevel();
            critDamageLevel = SaveSystem.Instance.GetCritDamageLevel();
            attackRangeLevel = SaveSystem.Instance.GetAttackRangeLevel();
            
            Debug.Log($"Loaded upgrade levels from save: Move={moveSpeedLevel}, Health={maxHealthLevel}, Damage={damageLevel}");
        }
    }
    
    private void SaveUpgradeLevels()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.SaveUpgradeLevels(
                moveSpeedLevel,
                maxHealthLevel,
                damageLevel,
                critChanceLevel,
                critDamageLevel,
                attackRangeLevel
            );
        }
    }
    
    public void UpgradeMoveSpeed()
    {
        moveSpeedLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
        SaveUpgradeLevels();
    }
    
    public void UpgradeMaxHealth()
    {
        maxHealthLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
        SaveUpgradeLevels();
    }
    
    public void UpgradeDamage()
    {
        damageLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
        SaveUpgradeLevels();
    }
    
    public void UpgradeCritChance()
    {
        critChanceLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
        SaveUpgradeLevels();
    }
    
    public void UpgradeCritDamage()
    {
        critDamageLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
        SaveUpgradeLevels();
    }
    
    public void UpgradeAttackRange()
    {
        attackRangeLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
        SaveUpgradeLevels();
    }
    
    public void AddZoneBonus(float damageBonus, float attackSpeedBonus, float moveSpeedBonus)
    {
        zoneDamageBonus = damageBonus;
        zoneAttackSpeedBonus = attackSpeedBonus;
        zoneMoveSpeedBonus = moveSpeedBonus;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void RemoveZoneBonus(float damageBonus, float attackSpeedBonus, float moveSpeedBonus)
    {
        zoneDamageBonus -= damageBonus;
        zoneAttackSpeedBonus -= attackSpeedBonus;
        zoneMoveSpeedBonus -= moveSpeedBonus;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    private void ApplyStatsToPlayer()
    {
        PlayerController controller = FindFirstObjectByType<PlayerController>();
        if (controller != null)
        {
            controller.SetMoveSpeed(GetMoveSpeed());
        }
        
        PlayerHealth health = FindFirstObjectByType<PlayerHealth>();
        if (health != null)
        {
            health.SetMaxHealth(GetMaxHealth());
        }
        
        PlayerCombat combat = FindFirstObjectByType<PlayerCombat>();
        if (combat != null)
        {
            combat.SetDamage(GetDamage());
            combat.SetAttackRange(GetAttackRange());
        }
    }
    
    public float GetMoveSpeed() => (baseMoveSpeed + (moveSpeedLevel * moveSpeedPerLevel)) * (1f + zoneMoveSpeedBonus + tempMoveSpeed);
    public float GetMaxHealth() => baseMaxHealth + (maxHealthLevel * healthPerLevel) + tempMaxHealth;
    public float GetDamage() => (baseDamage + (damageLevel * damagePerLevel) + tempDamage) * (1f + zoneDamageBonus);
    public float GetCritChance() => baseCritChance + (critChanceLevel * critChancePerLevel) + tempCritChance;
    public float GetCritDamage() => baseCritDamage + (critDamageLevel * critDamagePerLevel) + tempCritDamage;
    public float GetAttackRange() => baseAttackRange + (attackRangeLevel * attackRangePerLevel);
    public float GetAttackSpeedMultiplier() => 1f + zoneAttackSpeedBonus + tempAttackSpeed;
    
    public int GetMoveSpeedLevel() => moveSpeedLevel;
    public int GetMaxHealthLevel() => maxHealthLevel;
    public int GetDamageLevel() => damageLevel;
    public int GetCritChanceLevel() => critChanceLevel;
    public int GetCritDamageLevel() => critDamageLevel;
    public int GetAttackRangeLevel() => attackRangeLevel;
    
    public bool RollCritical()
    {
        return Random.value < GetCritChance();
    }
    
    public float CalculateFinalDamage()
    {
        float damage = GetDamage();
        if (RollCritical())
        {
            damage *= GetCritDamage();
        }
        return damage;
    }
    
    public void AddTemporaryMaxHealth(float amount)
    {
        tempMaxHealth += amount;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void AddTemporaryDamage(float amount)
    {
        tempDamage += amount;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void AddTemporaryMoveSpeed(float multiplier)
    {
        tempMoveSpeed += multiplier;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void AddTemporaryCritChance(float amount)
    {
        tempCritChance += amount;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void AddTemporaryCritDamage(float multiplier)
    {
        tempCritDamage += multiplier;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void AddTemporaryAttackSpeed(float multiplier)
    {
        tempAttackSpeed += multiplier;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void ResetTemporaryBonuses()
    {
        tempMaxHealth = 0f;
        tempDamage = 0f;
        tempMoveSpeed = 0f;
        tempCritChance = 0f;
        tempCritDamage = 0f;
        tempAttackSpeed = 0f;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
}
