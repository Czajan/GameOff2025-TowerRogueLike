using UnityEngine;
using UnityEngine.Events;

public class WaveController : MonoBehaviour
{
    public static WaveController Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private DefenseZone[] defenseZones;
    
    [Header("Wave Flow")]
    [SerializeField] private bool waitForBaseExit = true;
    
    public UnityEvent OnWaveStarted = new UnityEvent();
    public UnityEvent OnWaveCompleted = new UnityEvent();
    public UnityEvent OnSessionComplete = new UnityEvent();
    
    private bool waveInProgress = false;
    private DefenseZone currentZone;
    
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
        if (waveSpawner == null)
        {
            waveSpawner = FindFirstObjectByType<WaveSpawner>();
        }
        
        if (defenseZones == null || defenseZones.Length == 0)
        {
            defenseZones = FindObjectsByType<DefenseZone>(FindObjectsSortMode.None);
        }
        
        currentZone = GetActiveZone();
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnSessionStarted.AddListener(StartNextWave);
        }
    }
    
    private void Update()
    {
        if (waveInProgress && waveSpawner != null)
        {
            if (waveSpawner.IsSessionComplete)
            {
                CompleteSession();
            }
        }
    }
    
    public void StartNextWave()
    {
        if (waveInProgress) return;
        
        waveInProgress = true;
        currentZone = GetActiveZone();
        
        if (waveSpawner != null)
        {
            waveSpawner.StartWaves();
        }
        
        OnWaveStarted?.Invoke();
        Debug.Log($"Wave session started at {currentZone?.name ?? "default location"}");
    }
    
    private void CompleteSession()
    {
        waveInProgress = false;
        
        OnWaveCompleted?.Invoke();
        OnSessionComplete?.Invoke();
        
        if (GameProgressionManager.Instance != null)
        {
            int bonusCurrency = CalculateSessionBonus();
            if (bonusCurrency > 0)
            {
                GameProgressionManager.Instance.AddCurrency(bonusCurrency);
                Debug.Log($"Wave session complete! Bonus currency: {bonusCurrency}");
            }
        }
    }
    
    private int CalculateSessionBonus()
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
            return currentZone.GetCenterPosition();
        }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }
    
    public bool IsWaveInProgress => waveInProgress;
}
