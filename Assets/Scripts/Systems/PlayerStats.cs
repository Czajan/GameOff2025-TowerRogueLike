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
        }
    }
    
    public void UpgradeMoveSpeed()
    {
        moveSpeedLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void UpgradeMaxHealth()
    {
        maxHealthLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void UpgradeDamage()
    {
        damageLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void UpgradeCritChance()
    {
        critChanceLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void UpgradeCritDamage()
    {
        critDamageLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
    }
    
    public void UpgradeAttackRange()
    {
        attackRangeLevel++;
        OnStatsChanged?.Invoke();
        ApplyStatsToPlayer();
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
    
    public float GetMoveSpeed() => (baseMoveSpeed + (moveSpeedLevel * moveSpeedPerLevel)) * (1f + zoneMoveSpeedBonus);
    public float GetMaxHealth() => baseMaxHealth + (maxHealthLevel * healthPerLevel);
    public float GetDamage() => (baseDamage + (damageLevel * damagePerLevel)) * (1f + zoneDamageBonus);
    public float GetCritChance() => baseCritChance + (critChanceLevel * critChancePerLevel);
    public float GetCritDamage() => baseCritDamage + (critDamageLevel * critDamagePerLevel);
    public float GetAttackRange() => baseAttackRange + (attackRangeLevel * attackRangePerLevel);
    public float GetAttackSpeedMultiplier() => 1f + zoneAttackSpeedBonus;
    
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
}
