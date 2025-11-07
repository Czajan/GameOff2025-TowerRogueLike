using UnityEngine;
using UnityEngine.Events;

public class ExperienceSystem : MonoBehaviour
{
    public static ExperienceSystem Instance { get; private set; }
    
    [Header("Level Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int baseXPRequired = 100;
    [SerializeField] private float xpScalingPerLevel = 1.15f;
    
    [Header("Events")]
    public UnityEvent<int> OnLevelUp;
    public UnityEvent<int, int> OnXPChanged;
    
    private int totalXP = 0;
    private int xpRequiredForNextLevel;
    private int xpUsedForCurrentLevel = 0;
    
    public int CurrentLevel => currentLevel;
    public int CurrentXP => totalXP - xpUsedForCurrentLevel;
    public int XPRequired => xpRequiredForNextLevel;
    
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
        
        CalculateXPRequired();
    }
    
    private void Start()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnExperienceChanged.AddListener(OnExperienceGained);
        }
    }
    
    private void OnExperienceGained(int totalXPFromCurrency)
    {
        totalXP = totalXPFromCurrency;
        int currentLevelXP = totalXP - xpUsedForCurrentLevel;
        
        Debug.Log($"<color=magenta>[XP System] OnExperienceGained: Total={totalXP}, Used={xpUsedForCurrentLevel}, Current Level XP={currentLevelXP}, Required={xpRequiredForNextLevel}</color>");
        
        CheckForLevelUp();
        
        int displayXP = totalXP - xpUsedForCurrentLevel;
        OnXPChanged?.Invoke(displayXP, xpRequiredForNextLevel);
    }
    
    private void CheckForLevelUp()
    {
        int currentLevelXP = totalXP - xpUsedForCurrentLevel;
        
        while (currentLevelXP >= xpRequiredForNextLevel)
        {
            LevelUp();
            currentLevelXP = totalXP - xpUsedForCurrentLevel;
        }
    }
    
    private void LevelUp()
    {
        xpUsedForCurrentLevel += xpRequiredForNextLevel;
        currentLevel++;
        
        CalculateXPRequired();
        
        Debug.Log($"<color=yellow>★ LEVEL UP! Now Level {currentLevel} ★ (Total XP: {totalXP}, Used: {xpUsedForCurrentLevel})</color>");
        OnLevelUp?.Invoke(currentLevel);
        
        int displayXP = totalXP - xpUsedForCurrentLevel;
        OnXPChanged?.Invoke(displayXP, xpRequiredForNextLevel);
    }
    
    private void CalculateXPRequired()
    {
        xpRequiredForNextLevel = Mathf.RoundToInt(baseXPRequired * Mathf.Pow(xpScalingPerLevel, currentLevel - 1));
    }
    
    public void ResetLevel()
    {
        currentLevel = 1;
        totalXP = 0;
        xpUsedForCurrentLevel = 0;
        CalculateXPRequired();
        OnXPChanged?.Invoke(0, xpRequiredForNextLevel);
    }
    
    public bool IsMilestoneLevel()
    {
        return currentLevel % 5 == 0;
    }
}
