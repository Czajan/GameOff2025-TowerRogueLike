using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GatePassTrigger : MonoBehaviour
{
    [Header("Gate Reference")]
    [SerializeField] private BaseGate gate;
    
    [Header("Trigger Settings")]
    [SerializeField] private float autoCloseDelay = 1f;
    
    private bool hasTriggered = false;
    private Collider triggerCollider;
    
    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
        
        if (gate == null)
        {
            gate = GetComponentInParent<BaseGate>();
        }
    }
    
    private void Start()
    {
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunEnded.AddListener(ResetTrigger);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        
        if (RunStateManager.Instance == null) return;
        if (RunStateManager.Instance.IsInPreRunMenu) return;
        
        if (gate != null && gate.IsOpen)
        {
            hasTriggered = true;
            Invoke(nameof(CloseGateAfterDelay), autoCloseDelay);
            Debug.Log("<color=yellow>Player passed through gate - closing in " + autoCloseDelay + "s to prevent return!</color>");
        }
    }
    
    private void CloseGateAfterDelay()
    {
        if (gate != null)
        {
            gate.CloseGate();
            Debug.Log("<color=orange>Gate auto-closed! Player cannot return to pre-run area.</color>");
        }
    }
    
    private void ResetTrigger()
    {
        hasTriggered = false;
        CancelInvoke(nameof(CloseGateAfterDelay));
    }
    
    private void OnDestroy()
    {
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunEnded.RemoveListener(ResetTrigger);
        }
    }
    
    private void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (col is BoxCollider boxCol)
            {
                Gizmos.DrawCube(boxCol.center, boxCol.size);
            }
        }
    }
}
