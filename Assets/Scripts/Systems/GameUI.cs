using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Header("Wave UI")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private WaveSpawner waveSpawner;
    
    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged.AddListener(UpdateHealthUI);
            UpdateHealthUI(playerHealth.HealthPercentage);
        }
    }
    
    private void Update()
    {
        UpdateWaveUI();
    }
    
    private void UpdateHealthUI(float healthPercentage)
    {
        if (healthSlider != null)
        {
            healthSlider.value = healthPercentage;
        }
        
        if (healthText != null && playerHealth != null)
        {
            healthText.text = $"{Mathf.CeilToInt(playerHealth.CurrentHealth)} / {playerHealth.MaxHealth}";
        }
    }
    
    private void UpdateWaveUI()
    {
        if (waveSpawner == null)
            return;
        
        if (waveText != null)
        {
            waveText.text = $"Wave: {waveSpawner.CurrentWave}";
        }
        
        if (enemiesText != null)
        {
            enemiesText.text = $"Enemies: {waveSpawner.EnemiesAlive}";
        }
    }
}
