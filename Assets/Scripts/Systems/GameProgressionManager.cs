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
    
    private bool waveSessionActive = false;
    
    [Header("Events")]
    public UnityEvent<int> OnCurrencyChanged = new UnityEvent<int>();
    public UnityEvent<int> OnDefenseZoneChanged = new UnityEvent<int>();
    public UnityEvent OnEnteredBase = new UnityEvent();
    public UnityEvent OnExitedBase = new UnityEvent();
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
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.AddListener(OnRunStarted);
            RunStateManager.Instance.OnRunEnded.AddListener(OnRunEndedReset);
        }
    }
    
    private void OnDestroy()
    {
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.RemoveListener(OnRunStarted);
            RunStateManager.Instance.OnRunEnded.RemoveListener(OnRunEndedReset);
        }
    }
    
    private void OnRunStarted()
    {
        enemiesKilledThisRun = 0;
        wavesCompletedThisRun = 0;
        currentDefenseZone = 0;
        isInBase = true;
        waveSessionActive = false;
        
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
        
        DefenseZone[] allZones = FindObjectsByType<DefenseZone>(FindObjectsSortMode.None);
        foreach (DefenseZone zone in allZones)
        {
            zone.ResetZone();
        }
        
        Debug.Log("<color=green>GameProgressionManager: Run started, all systems reset</color>");
    }
    
    private void OnRunEndedReset()
    {
        enemiesKilledThisRun = 0;
        wavesCompletedThisRun = 0;
        currentDefenseZone = 0;
        isInBase = true;
        waveSessionActive = false;
        
        Debug.Log("<color=cyan>GameProgressionManager: Run ended, state reset</color>");
    }
    
    private void Update()
    {
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
        OnEnteredBase?.Invoke();
        
        if (waveSessionActive)
        {
            WaveSpawner spawner = FindFirstObjectByType<WaveSpawner>();
            if (spawner != null)
            {
                spawner.ResetSession();
            }
            waveSessionActive = false;
        }
        
        Debug.Log("Entered base!");
    }
    
    public void ExitBase()
    {
        if (!isInBase) return;
        
        isInBase = false;
        waveSessionActive = true;
        
        OnExitedBase?.Invoke();
        
        Debug.Log("Exited base! Wave session starting...");
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
        
        Debug.Log($"<color=cyan>=== WAVE SESSION COMPLETE! {wavesCompletedThisRun} waves completed this run. Return to base for upgrades! ===</color>");
    }
    
    public void OnIndividualWaveComplete()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddEssence(essencePerWave);
            Debug.Log($"<color=magenta>Wave cleared! +{essencePerWave} Essence</color>");
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
        int bonusEssence = CalculateBonusEssenceReward(victory);
        
        if (bonusEssence > 0 && CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddEssence(bonusEssence);
        }
        
        int totalEssenceEarned = CurrencyManager.Instance != null ? CurrencyManager.Instance.EssenceEarnedThisRun : 0;
        
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
        Debug.Log($"<color=magenta>Total Essence Earned This Run: {totalEssenceEarned}</color>");
        Debug.Log($"<color=purple>  - Waves: {wavesCompletedThisRun} × {essencePerWave} = {wavesCompletedThisRun * essencePerWave}</color>");
        Debug.Log($"<color=purple>  - Bonus: {bonusEssence}</color>");
        Debug.Log($"<color=green>Waves Completed: {wavesCompletedThisRun}</color>");
    }
    
    private int CalculateBonusEssenceReward(bool victory)
    {
        int zoneBonus = 0;
        if (currentDefenseZone == 0) zoneBonus = essenceZone1Bonus;
        else if (currentDefenseZone == 1) zoneBonus = essenceZone2Bonus;
        else if (currentDefenseZone == 2) zoneBonus = essenceZone3Bonus;
        
        int victoryBonus = victory ? essenceForVictory : 0;
        
        return zoneBonus + victoryBonus;
    }
    
    public int Currency => currentCurrency;
    public int CurrentDefenseZone => currentDefenseZone;
    public bool IsInBase => isInBase;
}
