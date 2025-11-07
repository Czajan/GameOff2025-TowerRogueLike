using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    
    [Header("In-Run Currencies")]
    [SerializeField] private int currentGold = 0;
    [SerializeField] private int currentExperience = 0;
    
    [Header("Events")]
    public UnityEvent<int> OnGoldChanged = new UnityEvent<int>();
    public UnityEvent<int> OnExperienceChanged = new UnityEvent<int>();
    public UnityEvent<int> OnEssenceChanged = new UnityEvent<int>();
    
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
        if (SaveSystem.Instance != null)
        {
            OnEssenceChanged?.Invoke(SaveSystem.Instance.GetEssence());
        }
        
        OnGoldChanged?.Invoke(currentGold);
        OnExperienceChanged?.Invoke(currentExperience);
    }
    
    public void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldChanged?.Invoke(currentGold);
        Debug.Log($"<color=yellow>+{amount} Gold! Total: {currentGold}</color>");
    }
    
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            OnGoldChanged?.Invoke(currentGold);
            Debug.Log($"<color=orange>-{amount} Gold. Remaining: {currentGold}</color>");
            return true;
        }
        Debug.Log($"<color=red>Not enough gold! Need {amount}, have {currentGold}</color>");
        return false;
    }
    
    public void AddExperience(int amount)
    {
        currentExperience += amount;
        OnExperienceChanged?.Invoke(currentExperience);
        Debug.Log($"<color=cyan>+{amount} XP! Total: {currentExperience}</color>");
    }
    
    public void ResetExperience()
    {
        currentExperience = 0;
        OnExperienceChanged?.Invoke(currentExperience);
    }
    
    public void AddEssence(int amount)
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.AddEssence(amount);
            OnEssenceChanged?.Invoke(SaveSystem.Instance.GetEssence());
            Debug.Log($"<color=magenta>+{amount} Essence! Total: {SaveSystem.Instance.GetEssence()}</color>");
        }
    }
    
    public bool SpendEssence(int amount)
    {
        if (SaveSystem.Instance != null && SaveSystem.Instance.SpendEssence(amount))
        {
            OnEssenceChanged?.Invoke(SaveSystem.Instance.GetEssence());
            Debug.Log($"<color=purple>-{amount} Essence. Remaining: {SaveSystem.Instance.GetEssence()}</color>");
            return true;
        }
        int available = SaveSystem.Instance != null ? SaveSystem.Instance.GetEssence() : 0;
        Debug.Log($"<color=red>Not enough Essence! Need {amount}, have {available}</color>");
        return false;
    }
    
    public void ResetInRunCurrencies()
    {
        currentGold = 0;
        currentExperience = 0;
        OnGoldChanged?.Invoke(currentGold);
        OnExperienceChanged?.Invoke(currentExperience);
        Debug.Log("In-run currencies reset for new run.");
    }
    
    public int Gold => currentGold;
    public int Experience => currentExperience;
    public int Essence => SaveSystem.Instance != null ? SaveSystem.Instance.GetEssence() : 0;
}
