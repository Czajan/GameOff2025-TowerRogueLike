using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName = "Basic Sword";
    public string description = "A simple weapon";
    public Sprite icon;
    
    [Header("Stats")]
    public float damageMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;
    public float rangeMultiplier = 1f;
    public float critChanceBonus = 0f;
    public float critDamageBonus = 0f;
    
    [Header("Special Effects")]
    public WeaponEffect weaponEffect = WeaponEffect.None;
    public float effectChance = 0f;
    public float effectValue = 0f;
    
    [Header("Cost")]
    public int purchaseCost = 100;
}

public enum WeaponEffect
{
    None,
    Bleed,
    Burn,
    Slow,
    Stun,
    AreaDamage,
    LifeSteal,
    ChainLightning
}
