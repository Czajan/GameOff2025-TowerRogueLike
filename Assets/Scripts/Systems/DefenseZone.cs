using UnityEngine;
using UnityEngine.Events;

public class DefenseZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private int zoneIndex = 0;
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private float spawnRadius = 10f;
    
    [Header("Zone Perks")]
    [SerializeField] private float damageBonus = 0f;
    [SerializeField] private float attackSpeedBonus = 0f;
    [SerializeField] private float moveSpeedBonus = 0f;
    
    [Header("Fallback")]
    [SerializeField] private DefenseZone nextZone;
    [SerializeField] private float fallbackHealthThreshold = 0.25f;
    
    public UnityEvent OnZoneActivated = new UnityEvent();
    public UnityEvent OnZoneLost = new UnityEvent();
    
    private bool isActive = false;
    
    private void Start()
    {
        if (zoneIndex == 0)
        {
            ActivateZone();
        }
    }
    
    public void ActivateZone()
    {
        isActive = true;
        OnZoneActivated?.Invoke();
        ApplyZonePerks();
        Debug.Log($"Defense Zone {zoneIndex + 1} activated! Perks: +{damageBonus * 100}% damage, +{attackSpeedBonus * 100}% attack speed");
    }
    
    public void CheckFallback()
    {
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null && playerHealth.HealthPercentage <= fallbackHealthThreshold)
        {
            FallbackToNextZone();
        }
    }
    
    private void FallbackToNextZone()
    {
        if (!isActive) return;
        
        isActive = false;
        OnZoneLost?.Invoke();
        
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.FallbackToNextZone();
        }
        
        if (nextZone != null)
        {
            nextZone.ActivateZone();
            TeleportPlayerToZone(nextZone);
        }
    }
    
    private void TeleportPlayerToZone(DefenseZone zone)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                player.transform.position = zone.spawnCenter.position;
                controller.enabled = true;
            }
            else
            {
                player.transform.position = zone.spawnCenter.position;
            }
        }
    }
    
    private void ApplyZonePerks()
    {
    }
    
    public Vector3 GetSpawnPosition()
    {
        Vector3 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = spawnCenter.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        spawnPos.y = 0;
        return spawnPos;
    }
    
    public bool IsActive => isActive;
    public float DamageBonus => damageBonus;
    public float AttackSpeedBonus => attackSpeedBonus;
    public float MoveSpeedBonus => moveSpeedBonus;
    
    private void OnDrawGizmosSelected()
    {
        if (spawnCenter == null) return;
        
        Gizmos.color = isActive ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(spawnCenter.position, spawnRadius);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(spawnCenter.position, 0.5f);
    }
}
