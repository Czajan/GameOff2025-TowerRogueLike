using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private bool isEntrance = true;
    
    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (GameProgressionManager.Instance == null) return;
        
        if (isEntrance && !GameProgressionManager.Instance.IsInBase)
        {
            GameProgressionManager.Instance.EnterBase();
        }
    }
}
