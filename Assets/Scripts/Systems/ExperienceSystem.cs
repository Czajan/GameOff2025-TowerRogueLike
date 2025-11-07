using UnityEngine;
using UnityEngine.Events;

public class ExperienceSystem : MonoBehaviour
{
    public static ExperienceSystem Instance { get; private set; }
    
    [Header("Level Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int baseXPRequired = 100;
    [SerializeField] private float xpScalingPerLevel = 1.15f;
    
    [Header("Events")]
    public UnityEvent<int> OnLevelUp;
    public UnityEvent<int, int> OnXPChanged;
    
    private int xpRequiredForNextLevel;
    
    public int CurrentLevel => currentLevel;
    public int CurrentXP => currentXP;
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
    
    private void OnExperienceGained(int totalXP)
    {
        currentXP = totalXP;
        CheckForLevelUp();
        OnXPChanged?.Invoke(currentXP, xpRequiredForNextLevel);
    }
    
    private void CheckForLevelUp()
    {
        while (currentXP >= xpRequiredForNextLevel)
        {
            LevelUp();
        }
    }
    
    private void LevelUp()
    {
        currentXP -= xpRequiredForNextLevel;
        currentLevel++;
        
        CalculateXPRequired();
        
        Debug.Log($"<color=yellow>★ LEVEL UP! Now Level {currentLevel} ★</color>");
        OnLevelUp?.Invoke(currentLevel);
        OnXPChanged?.Invoke(currentXP, xpRequiredForNextLevel);
    }
    
    private void CalculateXPRequired()
    {
        xpRequiredForNextLevel = Mathf.RoundToInt(baseXPRequired * Mathf.Pow(xpScalingPerLevel, currentLevel - 1));
    }
    
    public void ResetLevel()
    {
        currentLevel = 1;
        currentXP = 0;
        CalculateXPRequired();
        OnXPChanged?.Invoke(currentXP, xpRequiredForNextLevel);
    }
    
    public bool IsMilestoneLevel()
    {
        return currentLevel % 5 == 0;
    }
}
