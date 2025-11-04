using UnityEngine;
using UnityEngine.Events;

public class WaveController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private DefenseZone[] defenseZones;
    
    [Header("Wave Flow")]
    [SerializeField] private bool waitForBaseExit = true;
    
    public UnityEvent OnWaveStarted = new UnityEvent();
    public UnityEvent OnWaveCompleted = new UnityEvent();
    
    private bool waveInProgress = false;
    private DefenseZone currentZone;
    
    private void Start()
    {
        if (waveSpawner == null)
        {
            waveSpawner = FindFirstObjectByType<WaveSpawner>();
        }
        
        if (defenseZones == null || defenseZones.Length == 0)
        {
            defenseZones = FindObjectsByType<DefenseZone>(FindObjectsSortMode.None);
        }
        
        currentZone = GetActiveZone();
        
        if (GameProgressionManager.Instance != null && waitForBaseExit)
        {
            GameProgressionManager.Instance.OnExitedBase.AddListener(StartNextWave);
        }
        else
        {
            StartNextWave();
        }
    }
    
    private void Update()
    {
        if (waveInProgress && waveSpawner.EnemiesAlive == 0)
        {
            CompleteWave();
        }
    }
    
    public void StartNextWave()
    {
        if (waveInProgress) return;
        
        waveInProgress = true;
        currentZone = GetActiveZone();
        
        OnWaveStarted?.Invoke();
        Debug.Log($"Wave started at {currentZone?.name ?? "default location"}");
    }
    
    private void CompleteWave()
    {
        waveInProgress = false;
        
        OnWaveCompleted?.Invoke();
        
        if (GameProgressionManager.Instance != null)
        {
            int bonusCurrency = CalculateWaveBonus();
            if (bonusCurrency > 0)
            {
                GameProgressionManager.Instance.AddCurrency(bonusCurrency);
                Debug.Log($"Wave complete! Bonus currency: {bonusCurrency}");
            }
        }
    }
    
    private int CalculateWaveBonus()
    {
        int baseBonus = 50;
        
        if (currentZone != null && currentZone.IsActive)
        {
            int zoneMultiplier = currentZone.gameObject.name.Contains("1") ? 3 : 
                                 currentZone.gameObject.name.Contains("2") ? 2 : 1;
            return baseBonus * zoneMultiplier;
        }
        
        return baseBonus;
    }
    
    private DefenseZone GetActiveZone()
    {
        foreach (DefenseZone zone in defenseZones)
        {
            if (zone != null && zone.IsActive)
            {
                return zone;
            }
        }
        
        return defenseZones.Length > 0 ? defenseZones[0] : null;
    }
    
    public Vector3 GetSpawnPosition()
    {
        if (currentZone != null)
        {
            return currentZone.GetSpawnPosition();
        }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }
    
    public bool IsWaveInProgress => waveInProgress;
}
