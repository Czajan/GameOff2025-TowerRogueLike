using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseExitTrigger : MonoBehaviour
{
    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (GameProgressionManager.Instance == null) return;
        
        bool currentlyInBase = GameProgressionManager.Instance.IsInBase;
        Debug.Log($"<color=cyan>BaseExitTrigger.OnTriggerExit - IsInBase: {currentlyInBase}</color>");
        
        if (currentlyInBase)
        {
            GameProgressionManager.Instance.ExitBase();
            Debug.Log("<color=green>✓ Player manually exited base zone!</color>");
        }
        else
        {
            Debug.Log("<color=orange>✗ Ignored BaseExitTrigger - Player already outside base</color>");
        }
    }
}
