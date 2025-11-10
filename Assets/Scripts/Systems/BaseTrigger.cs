using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseTrigger : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private Vector3 baseDirection = new Vector3(-1, 0, -1);
    
    [Header("Gate Auto-Close")]
    [SerializeField] private BaseGate gate;
    [SerializeField] private float autoCloseDelay = 1f;
    [SerializeField] private bool enableAutoClose = true;
    
    private Transform playerTransform;
    private Vector3 lastPlayerPosition;
    private bool playerInTrigger = false;
    private Collider triggerCollider;
    private bool isActive = true;
    private bool hasAutoClosedGate = false;
    
    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
        baseDirection.Normalize();
        
        if (gate == null)
        {
            gate = FindFirstObjectByType<BaseGate>();
        }
    }
    
    private void Start()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnExitedBase.AddListener(DisableTrigger);
        }
        
        if (WaveController.Instance != null)
        {
            WaveController.Instance.OnSessionComplete.AddListener(EnableTrigger);
        }
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.AddListener(OnRunStarted);
            RunStateManager.Instance.OnRunEnded.AddListener(OnRunEnded);
        }
    }
    
    private void OnDestroy()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnExitedBase.RemoveListener(DisableTrigger);
        }
        
        if (WaveController.Instance != null)
        {
            WaveController.Instance.OnSessionComplete.RemoveListener(EnableTrigger);
        }
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.RemoveListener(OnRunStarted);
            RunStateManager.Instance.OnRunEnded.RemoveListener(OnRunEnded);
        }
        
        CancelInvoke(nameof(CloseGateVisualAfterDelay));
    }
    
    private void DisableTrigger()
    {
        isActive = false;
        playerInTrigger = false;
        Debug.Log("<color=red>BaseTrigger DISABLED - Player exited base, waves starting</color>");
    }
    
    private void EnableTrigger()
    {
        isActive = true;
        Debug.Log("<color=green>BaseTrigger ENABLED - Session complete, player can return to base</color>");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (!other.CompareTag("Player")) return;
        
        playerTransform = other.transform;
        lastPlayerPosition = playerTransform.position;
        playerInTrigger = true;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;
        if (!other.CompareTag("Player")) return;
        if (GameProgressionManager.Instance == null) return;
        if (!playerInTrigger) return;
        
        Vector3 currentPosition = playerTransform.position;
        Vector3 movementDirection = (currentPosition - lastPlayerPosition).normalized;
        
        if (movementDirection.magnitude < 0.1f)
        {
            return;
        }
        
        float dotProduct = Vector3.Dot(movementDirection, baseDirection);
        
        bool currentlyInBase = GameProgressionManager.Instance.IsInBase;
        bool movingTowardBase = dotProduct > 0.3f;
        
        Debug.Log($"<color=yellow>BaseTrigger - IsInBase: {currentlyInBase}, Moving toward base: {movingTowardBase}, Dot: {dotProduct:F2}</color>");
        
        if (!currentlyInBase && movingTowardBase)
        {
            GameProgressionManager.Instance.EnterBase();
            Debug.Log("<color=green>✓ Player entered base zone! (Returning from waves)</color>");
            playerInTrigger = false;
        }
        else if (currentlyInBase && !movingTowardBase)
        {
            Debug.Log("<color=orange>✗ Player leaving base through trigger - ignored</color>");
            playerInTrigger = false;
        }
        
        lastPlayerPosition = currentPosition;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInTrigger = false;
        
        if (enableAutoClose && !hasAutoClosedGate)
        {
            if (RunStateManager.Instance != null && !RunStateManager.Instance.IsInPreRunMenu)
            {
                if (gate != null && gate.IsOpen)
                {
                    hasAutoClosedGate = true;
                    
                    gate.EnableBarrierInstantly();
                    Debug.Log("<color=yellow>Player exited base zone - BARRIER ENABLED INSTANTLY!</color>");
                    
                    Invoke(nameof(CloseGateVisualAfterDelay), autoCloseDelay);
                    Debug.Log("<color=yellow>Gate visual will close in " + autoCloseDelay + "s...</color>");
                }
            }
        }
    }
    
    private void CloseGateVisualAfterDelay()
    {
        if (gate != null)
        {
            gate.CloseGate();
            Debug.Log("<color=orange>Gate visual closed (barrier already active)</color>");
        }
    }
    
    private void OnRunStarted()
    {
        hasAutoClosedGate = false;
    }
    
    private void OnRunEnded()
    {
        hasAutoClosedGate = false;
        CancelInvoke(nameof(CloseGateVisualAfterDelay));
    }
}
