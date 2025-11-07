using UnityEngine;

public class EssenceDebugTester : MonoBehaviour
{
    [Header("Debug Controls")]
    [SerializeField] private KeyCode testVictoryKey = KeyCode.F1;
    [SerializeField] private KeyCode testDefeatKey = KeyCode.F2;
    [SerializeField] private KeyCode addEssenceKey = KeyCode.F3;
    [SerializeField] private KeyCode simulateWaveCompleteKey = KeyCode.F4;
    [SerializeField] private int essenceToAdd = 50;
    
    private void Start()
    {
        Debug.Log("<color=cyan>=== ESSENCE DEBUG TESTER ACTIVE ===</color>");
        Debug.Log($"[{testVictoryKey}] - Simulate Victory (Zone + Victory bonus)");
        Debug.Log($"[{testDefeatKey}] - Simulate Defeat (Zone bonus only)");
        Debug.Log($"[{addEssenceKey}] - Add {essenceToAdd} Essence directly");
        Debug.Log($"[{simulateWaveCompleteKey}] - Simulate Wave Complete (per-wave Essence)");
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(testVictoryKey))
        {
            TestVictory();
        }
        
        if (Input.GetKeyDown(testDefeatKey))
        {
            TestDefeat();
        }
        
        if (Input.GetKeyDown(addEssenceKey))
        {
            AddEssenceDirectly();
        }
        
        if (Input.GetKeyDown(simulateWaveCompleteKey))
        {
            SimulateWaveComplete();
        }
    }
    
    private void TestVictory()
    {
        if (GameProgressionManager.Instance != null)
        {
            Debug.Log("<color=yellow>=== TESTING VICTORY ESSENCE REWARD ===</color>");
            GameProgressionManager.Instance.OnRunComplete(true);
        }
        else
        {
            Debug.LogError("GameProgressionManager.Instance is null!");
        }
    }
    
    private void TestDefeat()
    {
        if (GameProgressionManager.Instance != null)
        {
            Debug.Log("<color=yellow>=== TESTING DEFEAT ESSENCE REWARD ===</color>");
            GameProgressionManager.Instance.OnRunComplete(false);
        }
        else
        {
            Debug.LogError("GameProgressionManager.Instance is null!");
        }
    }
    
    private void AddEssenceDirectly()
    {
        if (CurrencyManager.Instance != null)
        {
            Debug.Log($"<color=magenta>=== ADDING {essenceToAdd} ESSENCE DIRECTLY ===</color>");
            CurrencyManager.Instance.AddEssence(essenceToAdd);
        }
        else
        {
            Debug.LogError("CurrencyManager.Instance is null!");
        }
    }
    
    private void SimulateWaveComplete()
    {
        if (GameProgressionManager.Instance != null)
        {
            Debug.Log("<color=yellow>=== SIMULATING WAVE COMPLETE ===</color>");
            GameProgressionManager.Instance.OnWaveSessionComplete();
        }
        else
        {
            Debug.LogError("GameProgressionManager.Instance is null!");
        }
    }
}
