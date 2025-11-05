using UnityEngine;
using UnityEngine.InputSystem;

public class WaveSpawnerDebug : MonoBehaviour
{
    [SerializeField] private WaveSpawner waveSpawner;
    
    private void Start()
    {
        Debug.Log("=== WAVE DEBUG CONTROLS ===");
        Debug.Log("[T] - Force start waves");
        Debug.Log("[K] - Kill all enemies in current wave");
    }
    
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            StartWaves();
        }
        
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            KillAllEnemies();
        }
    }
    
    [ContextMenu("Start Waves")]
    public void StartWaves()
    {
        if (waveSpawner != null)
        {
            waveSpawner.StartWaves();
            Debug.Log("Manually started waves!");
        }
        else
        {
            Debug.LogError("WaveSpawner reference not set!");
        }
    }
    
    [ContextMenu("Kill All Enemies")]
    public void KillAllEnemies()
    {
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        int count = enemies.Length;
        
        foreach (EnemyAI enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        
        Debug.Log($"<color=red>DEBUG: Killed {count} enemies!</color>");
    }
}
