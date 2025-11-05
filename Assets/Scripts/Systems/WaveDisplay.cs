using UnityEngine;
using TMPro;

public class WaveDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private int wavesPerSession = 10;
    
    private WaveSpawner waveSpawner;
    
    private void Start()
    {
        waveSpawner = FindFirstObjectByType<WaveSpawner>();
        
        if (waveText == null)
        {
            waveText = GetComponent<TextMeshProUGUI>();
        }
        
        UpdateDisplay();
    }
    
    private void Update()
    {
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (waveSpawner == null || waveText == null) return;
        
        int currentWave = waveSpawner.CurrentWave;
        int sessionProgress = waveSpawner.WavesCompletedThisSession;
        int nextSessionTarget = ((currentWave / wavesPerSession) + 1) * wavesPerSession;
        
        if (waveSpawner.IsSessionComplete)
        {
            waveText.text = $"Wave {currentWave} - Return to Base!";
        }
        else if (sessionProgress > 0)
        {
            waveText.text = $"Wave {currentWave} ({sessionProgress}/{wavesPerSession})";
        }
        else
        {
            waveText.text = $"Wave {currentWave}";
        }
    }
}
