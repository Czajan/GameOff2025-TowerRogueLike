using UnityEngine;
using UnityEngine.Events;

public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance { get; private set; }
    
    [Header("Current Weapon")]
    [SerializeField] private WeaponData equippedWeapon;
    
    public UnityEvent<WeaponData> OnWeaponChanged = new UnityEvent<WeaponData>();
    
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
    
    public void EquipWeapon(WeaponData weapon)
    {
        equippedWeapon = weapon;
        OnWeaponChanged?.Invoke(weapon);
        Debug.Log($"Equipped weapon: {weapon.weaponName}");
    }
    
    public float GetDamageMultiplier()
    {
        return equippedWeapon != null ? equippedWeapon.damageMultiplier : 1f;
    }
    
    public float GetAttackSpeedMultiplier()
    {
        return equippedWeapon != null ? equippedWeapon.attackSpeedMultiplier : 1f;
    }
    
    public float GetRangeMultiplier()
    {
        return equippedWeapon != null ? equippedWeapon.rangeMultiplier : 1f;
    }
    
    public float GetCritChanceBonus()
    {
        return equippedWeapon != null ? equippedWeapon.critChanceBonus : 0f;
    }
    
    public float GetCritDamageBonus()
    {
        return equippedWeapon != null ? equippedWeapon.critDamageBonus : 0f;
    }
    
    public bool TryApplyWeaponEffect(GameObject target)
    {
        if (equippedWeapon == null || equippedWeapon.weaponEffect == WeaponEffect.None)
            return false;
        
        if (Random.value > equippedWeapon.effectChance)
            return false;
        
        ApplyEffect(target, equippedWeapon.weaponEffect, equippedWeapon.effectValue);
        return true;
    }
    
    private void ApplyEffect(GameObject target, WeaponEffect effect, float value)
    {
        switch (effect)
        {
            case WeaponEffect.Bleed:
                Debug.Log($"Applied bleed to {target.name}");
                break;
            case WeaponEffect.Burn:
                Debug.Log($"Applied burn to {target.name}");
                break;
            case WeaponEffect.Slow:
                Debug.Log($"Applied slow to {target.name}");
                break;
            case WeaponEffect.Stun:
                Debug.Log($"Applied stun to {target.name}");
                break;
            case WeaponEffect.AreaDamage:
                ApplyAreaDamage(target.transform.position, value);
                break;
            case WeaponEffect.LifeSteal:
                HealPlayer(value);
                break;
            case WeaponEffect.ChainLightning:
                Debug.Log($"Chain lightning from {target.name}");
                break;
        }
    }
    
    private void ApplyAreaDamage(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Enemy"));
        foreach (Collider col in colliders)
        {
            EnemyHealth enemyHealth = col.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                float areaDamage = PlayerStats.Instance.GetDamage() * 0.5f;
                enemyHealth.TakeDamage(areaDamage);
            }
        }
    }
    
    private void HealPlayer(float amount)
    {
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(amount);
        }
    }
    
    public WeaponData EquippedWeapon => equippedWeapon;
}
