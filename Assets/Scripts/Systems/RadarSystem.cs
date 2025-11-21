using UnityEngine;

public class RadarSystem : MonoBehaviour
{
    public static RadarSystem Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private RadarUI radarUI;
    
    private Transform currentActiveDefenseZone;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    private void Start()
    {
        if (radarUI == null)
        {
            radarUI = FindFirstObjectByType<RadarUI>();
        }
        
        RegisterAllDefenseZones();
        UpdateActiveDefenseZone();
    }
    
    private void RegisterAllDefenseZones()
    {
        DefenseZone[] zones = FindObjectsByType<DefenseZone>(FindObjectsSortMode.None);
        
        foreach (DefenseZone zone in zones)
        {
            if (zone != null && radarUI != null)
            {
                radarUI.RegisterDefenseZone(zone.transform);
                Debug.Log($"Registered defense zone on radar: {zone.name}");
            }
        }
    }
    
    private void UpdateActiveDefenseZone()
    {
        DefenseZone[] zones = FindObjectsByType<DefenseZone>(FindObjectsSortMode.None);
        
        foreach (DefenseZone zone in zones)
        {
            if (zone != null && zone.IsActive)
            {
                SetActiveDefenseZone(zone.transform);
                return;
            }
        }
    }
    
    public void SetActiveDefenseZone(Transform zoneTransform)
    {
        currentActiveDefenseZone = zoneTransform;
        
        if (radarUI != null)
        {
            radarUI.SetActiveDefenseZone(zoneTransform);
        }
    }
    
    public void RegisterEnemy(Transform enemyTransform)
    {
        if (radarUI != null && enemyTransform != null)
        {
            radarUI.RegisterEnemy(enemyTransform);
        }
    }
    
    public void UnregisterEnemy(Transform enemyTransform)
    {
        if (radarUI != null && enemyTransform != null)
        {
            radarUI.UnregisterBlip(enemyTransform);
        }
    }
    
    public void ClearAllEnemies()
    {
        if (radarUI != null)
        {
            radarUI.ClearAllEnemyBlips();
        }
    }
}
