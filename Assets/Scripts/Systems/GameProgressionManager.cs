using UnityEngine;
using UnityEngine.Events;

public class GameProgressionManager : MonoBehaviour
{
    public static GameProgressionManager Instance { get; private set; }
    
    [Header("Currency")]
    [SerializeField] private int currentCurrency = 0;
    
    [Header("Run Tracking")]
    [SerializeField] private int enemiesKilledThisRun = 0;
    [SerializeField] private int wavesCompletedThisRun = 0;
    
    [Header("Essence Rewards (Meta-Currency)")]
    [SerializeField] private int essencePerWave = 10;
    [SerializeField] private int essenceForVictory = 200;
    [SerializeField] private int essenceZone1Bonus = 100;
    [SerializeField] private int essenceZone2Bonus = 50;
    [SerializeField] private int essenceZone3Bonus = 25;
    [SerializeField] private int minimumEssenceReward = 10;
    
    [Header("Defense Zones")]
    [SerializeField] private int currentDefenseZone = 0;
    [SerializeField] private int maxDefenseZones = 3;
    
    [Header("Game State")]
    [SerializeField] private bool isInBase = true;
    [SerializeField] private float baseTimerDuration = 45f;
    [SerializeField] private float currentBaseTimer = 0f;
    
    private bool waveSessionActive = false;
    
    [Header("Events")]
    public UnityEvent<int> OnCurrencyChanged = new UnityEvent<int>();
    public UnityEvent<int> OnDefenseZoneChanged = new UnityEvent<int>();
    public UnityEvent OnEnteredBase = new UnityEvent();
    public UnityEvent OnExitedBase = new UnityEvent();
    public UnityEvent<float> OnBaseTimerUpdate = new UnityEvent<float>();
    public UnityEvent OnDefenseFailed = new UnityEvent();
    public UnityEvent OnDefenseComplete = new UnityEvent();
    
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
        if (isInBase)
        {
            currentBaseTimer = baseTimerDuration;
            OnBaseTimerUpdate?.Invoke(currentBaseTimer);
            Debug.Log($"Game started! Base timer initialized: {currentBaseTimer} seconds");
        }
        
        enemiesKilledThisRun = 0;
        wavesCompletedThisRun = 0;
        
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetInRunCurrencies();
        }
        
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetTemporaryBonuses();
        }
        
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.ResetLevel();
        }
    }
    
    private void Update()
    {
        if (isInBase && currentBaseTimer > 0f)
        {
            currentBaseTimer -= Time.deltaTime;
            
            if (currentBaseTimer <= 0f)
            {
                currentBaseTimer = 0f;
                OnBaseTimerUpdate?.Invoke(currentBaseTimer);
                Debug.Log("Base timer reached 0! Force starting wave...");
                ForceStartWave();
            }
            else
            {
                OnBaseTimerUpdate?.Invoke(currentBaseTimer);
            }
        }
    }
    
    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        OnCurrencyChanged?.Invoke(currentCurrency);
    }
    
    public bool SpendCurrency(int amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            OnCurrencyChanged?.Invoke(currentCurrency);
            return true;
        }
        return false;
    }
    
    public void EnterBase()
    {
        if (isInBase) return;
        
        isInBase = true;
        currentBaseTimer = baseTimerDuration;
        OnEnteredBase?.Invoke();
        OnBaseTimerUpdate?.Invoke(currentBaseTimer);
        
        if (waveSessionActive)
        {
            WaveSpawner spawner = FindFirstObjectByType<WaveSpawner>();
            if (spawner != null)
            {
                spawner.ResetSession();
            }
            waveSessionActive = false;
        }
        
        Debug.Log("Entered base! Timer started for next wave session.");
    }
    
    public void ExitBase()
    {
        if (!isInBase) return;
        
        isInBase = false;
        currentBaseTimer = 0f;
        waveSessionActive = true;
        
        OnExitedBase?.Invoke();
        
        Debug.Log("Exited base! Wave session starting...");
    }
    
    private void ForceStartWave()
    {
        ExitBase();
        WaveSpawner spawner = FindFirstObjectByType<WaveSpawner>();
        if (spawner != null)
        {
            Debug.Log("Base timer expired! Starting next wave...");
        }
    }
    
    public void FallbackToNextZone()
    {
        currentDefenseZone++;
        
        if (currentDefenseZone >= maxDefenseZones)
        {
            OnDefenseFailed?.Invoke();
            Debug.Log("All defense zones lost! Game Over!");
        }
        else
        {
            OnDefenseZoneChanged?.Invoke(currentDefenseZone);
            Debug.Log($"Falling back to defense zone {currentDefenseZone + 1}!");
        }
    }
    
    public void CompleteDefense()
    {
        OnDefenseComplete?.Invoke();
        int bonusMultiplier = (maxDefenseZones - currentDefenseZone);
        int bonusCurrency = 100 * bonusMultiplier;
        AddCurrency(bonusCurrency);
        Debug.Log($"Defense complete! Bonus: {bonusCurrency} (held {bonusMultiplier} zones)");
    }
    
    public void OnWaveSessionComplete()
    {
        wavesCompletedThisRun++;
        
        Debug.Log("=== WAVE SESSION COMPLETE! Return to base for upgrades! ===");
        
        NotificationUI notification = FindFirstObjectByType<NotificationUI>();
        if (notification != null)
        {
            notification.ShowNotification("Wave Session Complete! Return to base for upgrades!");
        }
    }
    
    public void OnRunComplete(bool victory)
    {
        if (WaveSpawner.Instance != null)
        {
            int wavesCompleted = WaveSpawner.Instance.CurrentWave;
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.UpdateHighestWave(wavesCompleted);
            }
        }
        
        int goldEarned = CurrencyManager.Instance != null ? CurrencyManager.Instance.Gold : 0;
        int essenceEarned = CalculateEssenceReward(wavesCompletedThisRun, victory);
        
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddEssence(essenceEarned);
        }
        
        if (SaveSystem.Instance != null)
        {
            if (victory)
            {
                SaveSystem.Instance.IncrementRunsCompleted();
            }
            else
            {
                SaveSystem.Instance.IncrementRunsFailed();
            }
        }
        
        Debug.Log($"<color=cyan>=== RUN COMPLETE ===</color>");
        Debug.Log($"<color=yellow>Gold Earned This Run: {goldEarned}</color>");
        Debug.Log($"<color=magenta>Essence (Meta-Currency) Earned: {essenceEarned}</color>");
        Debug.Log($"<color=green>Waves Completed: {wavesCompletedThisRun}</color>");
    }
    
    private int CalculateEssenceReward(int wavesCompleted, bool victory)
    {
        int waveReward = wavesCompleted * essencePerWave;
        
        int zoneBonus = 0;
        if (currentDefenseZone == 0) zoneBonus = essenceZone1Bonus;
        else if (currentDefenseZone == 1) zoneBonus = essenceZone2Bonus;
        else if (currentDefenseZone == 2) zoneBonus = essenceZone3Bonus;
        
        int victoryBonus = victory ? essenceForVictory : 0;
        
        int totalReward = waveReward + zoneBonus + victoryBonus;
        
        return Mathf.Max(totalReward, minimumEssenceReward);
    }
    
    public int Currency => currentCurrency;
    public int CurrentDefenseZone => currentDefenseZone;
    public bool IsInBase => isInBase;
    public float BaseTimer => currentBaseTimer;
}
