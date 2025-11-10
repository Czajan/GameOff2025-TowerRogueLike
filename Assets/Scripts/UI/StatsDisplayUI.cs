using UnityEngine;
using TMPro;

public class StatsDisplayUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI statsText;
    
    [Header("Settings")]
    [SerializeField] private bool showDetailedStats = true;
    [SerializeField] private float updateInterval = 0.5f;
    
    private float updateTimer = 0f;
    
    private void Start()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnStatsChanged.AddListener(UpdateStatsDisplay);
        }
        
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.OnLevelUp.AddListener(OnLevelChanged);
            ExperienceSystem.Instance.OnXPChanged.AddListener(OnXPChanged);
        }
        
        UpdateStatsDisplay();
    }
    
    private void OnDestroy()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnStatsChanged.RemoveListener(UpdateStatsDisplay);
        }
        
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.OnLevelUp.RemoveListener(OnLevelChanged);
            ExperienceSystem.Instance.OnXPChanged.RemoveListener(OnXPChanged);
        }
    }
    
    private void Update()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;
            UpdateStatsDisplay();
        }
    }
    
    private void OnLevelChanged(int level)
    {
        UpdateStatsDisplay();
    }
    
    private void OnXPChanged(int currentXP, int requiredXP)
    {
        UpdateStatsDisplay();
    }
    
    private void UpdateStatsDisplay()
    {
        if (statsText == null || PlayerStats.Instance == null)
            return;
        
        int currentLevel = ExperienceSystem.Instance != null ? ExperienceSystem.Instance.CurrentLevel : 1;
        
        PlayerHealth health = FindFirstObjectByType<PlayerHealth>();
        float currentHealth = health != null ? health.CurrentHealth : 0;
        float maxHealth = PlayerStats.Instance.GetMaxHealth();
        
        if (showDetailedStats)
        {
            string statsDisplay = $"<b>STATS</b> (Lvl {currentLevel})\n";
            statsDisplay += $"HP: <color=yellow>{currentHealth:F0}/{maxHealth:F0}</color>\n";
            statsDisplay += $"DMG: <color=yellow>{PlayerStats.Instance.GetDamage():F1}</color> ";
            statsDisplay += $"Crit: <color=yellow>{PlayerStats.Instance.GetCritChance() * 100:F0}%</color>\n";
            statsDisplay += $"Spd: <color=yellow>{PlayerStats.Instance.GetMoveSpeed():F1}</color> ";
            statsDisplay += $"Rng: <color=yellow>{PlayerStats.Instance.GetAttackRange():F1}m</color>";
            
            statsText.text = statsDisplay;
        }
        else
        {
            statsText.text = $"<b>Lvl {currentLevel}</b> | HP: {currentHealth:F0}/{maxHealth:F0} | DMG: {PlayerStats.Instance.GetDamage():F1}";
        }
    }
}
