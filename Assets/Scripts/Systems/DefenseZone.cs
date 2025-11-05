using UnityEngine;
using UnityEngine.Events;

public class DefenseZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private int zoneIndex = 0;
    
    [Header("Defense Objective")]
    [SerializeField] private DefenseObjective defenseObjective;
    
    [Header("Zone Perks")]
    [SerializeField] private float damageBonus = 0f;
    [SerializeField] private float attackSpeedBonus = 0f;
    [SerializeField] private float moveSpeedBonus = 0f;
    
    [Header("Fallback")]
    [SerializeField] private DefenseZone nextZone;
    
    public UnityEvent OnZoneActivated = new UnityEvent();
    public UnityEvent OnZoneLost = new UnityEvent();
    
    private bool isActive = false;
    private bool hasBeenDestroyed = false;
    
    private void Start()
    {
        if (defenseObjective == null)
        {
            defenseObjective = GetComponentInChildren<DefenseObjective>();
        }
        
        if (zoneIndex == 0)
        {
            ActivateZone();
        }
        else if (defenseObjective != null)
        {
            defenseObjective.gameObject.SetActive(false);
        }
    }
    
    public void ActivateZone()
    {
        isActive = true;
        
        if (defenseObjective != null)
        {
            defenseObjective.gameObject.SetActive(true);
        }
        
        OnZoneActivated?.Invoke();
        ApplyZonePerksToPlayer();
        Debug.Log($"Defense Zone {zoneIndex + 1} activated! Perks: +{damageBonus * 100}% damage, +{attackSpeedBonus * 100}% attack speed, +{moveSpeedBonus * 100}% move speed");
    }
    
    public void OnObjectiveDestroyed()
    {
        if (hasBeenDestroyed) return;
        
        hasBeenDestroyed = true;
        RemoveZonePerksFromPlayer();
        FallbackToNextZone();
    }
    
    private void FallbackToNextZone()
    {
        if (!isActive) return;
        
        isActive = false;
        OnZoneLost?.Invoke();
        
        Debug.Log($"<color=red>⚠️ ZONE {zoneIndex + 1} LOST! Falling back...</color>");
        
        NotificationUI notification = FindFirstObjectByType<NotificationUI>();
        if (notification != null)
        {
            if (nextZone != null)
            {
                notification.ShowNotification($"ZONE {zoneIndex + 1} OBJECTIVE DESTROYED! Retreat to Zone {nextZone.ZoneIndex + 1}!");
            }
            else
            {
                notification.ShowNotification("FINAL OBJECTIVE DESTROYED! GAME OVER!");
            }
        }
        
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.FallbackToNextZone();
        }
        
        if (nextZone != null)
        {
            nextZone.ActivateZone();
            RetargetEnemies(nextZone);
            
            if (WaveSpawner.Instance != null)
            {
                WaveSpawner.Instance.SetActiveZone(nextZone);
            }
        }
        else
        {
            Debug.Log("<color=red>⚠️ NO MORE ZONES! GAME OVER!</color>");
        }
    }
    
    private void RetargetEnemies(DefenseZone targetZone)
    {
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        int retargetedCount = 0;
        
        foreach (EnemyAI enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetDefenseZone(targetZone);
                retargetedCount++;
            }
        }
        
        if (retargetedCount > 0)
        {
            Debug.Log($"<color=yellow>Retargeted {retargetedCount} enemies to Zone {targetZone.zoneIndex + 1}. They will pursue the new objective.</color>");
        }
    }
    
    private void ApplyZonePerksToPlayer()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddZoneBonus(damageBonus, attackSpeedBonus, moveSpeedBonus);
            Debug.Log($"<color=cyan>Zone perks applied: +{damageBonus * 100}% damage, +{attackSpeedBonus * 100}% attack speed, +{moveSpeedBonus * 100}% move speed</color>");
        }
    }
    
    private void RemoveZonePerksFromPlayer()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.RemoveZoneBonus(damageBonus, attackSpeedBonus, moveSpeedBonus);
            Debug.Log($"<color=orange>Zone perks removed</color>");
        }
    }
    
    public Vector3 GetCenterPosition()
    {
        return transform.position;
    }
    
    public bool IsActive => isActive;
    public float DamageBonus => damageBonus;
    public float AttackSpeedBonus => attackSpeedBonus;
    public float MoveSpeedBonus => moveSpeedBonus;
    public int ZoneIndex => zoneIndex;
    public DefenseObjective GetDefenseObjective() => defenseObjective;
}
