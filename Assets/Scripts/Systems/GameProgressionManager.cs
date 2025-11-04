using UnityEngine;
using UnityEngine.Events;

public class GameProgressionManager : MonoBehaviour
{
    public static GameProgressionManager Instance { get; private set; }
    
    [Header("Currency")]
    [SerializeField] private int currentCurrency = 0;
    
    [Header("Defense Zones")]
    [SerializeField] private int currentDefenseZone = 0;
    [SerializeField] private int maxDefenseZones = 3;
    
    [Header("Game State")]
    [SerializeField] private bool isInBase = true;
    [SerializeField] private float baseTimerDuration = 40f;
    [SerializeField] private float currentBaseTimer = 0f;
    
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
    
    private void Update()
    {
        if (isInBase && currentBaseTimer > 0f)
        {
            currentBaseTimer -= Time.deltaTime;
            OnBaseTimerUpdate?.Invoke(currentBaseTimer);
            
            if (currentBaseTimer <= 0f)
            {
                ForceStartWave();
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
    }
    
    public void ExitBase()
    {
        if (!isInBase) return;
        
        isInBase = false;
        currentBaseTimer = 0f;
        OnExitedBase?.Invoke();
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
    
    public int Currency => currentCurrency;
    public int CurrentDefenseZone => currentDefenseZone;
    public bool IsInBase => isInBase;
    public float BaseTimer => currentBaseTimer;
}
