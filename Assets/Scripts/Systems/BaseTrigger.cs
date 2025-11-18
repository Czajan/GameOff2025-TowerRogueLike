using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseTrigger : MonoBehaviour
{
    [Header("Gate Auto-Close")]
    [SerializeField] private DoubleDoorGate doubleDoorGate;
    [SerializeField] private BaseGate oldGate;
    [SerializeField] private float autoCloseDelay = 1f;
    [SerializeField] private bool enableAutoClose = true;
    
    private Collider triggerCollider;
    private bool isActive = true;
    private bool hasAutoClosedGate = false;
    
    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
        
        if (doubleDoorGate == null)
        {
            doubleDoorGate = FindFirstObjectByType<DoubleDoorGate>();
        }
        
        if (doubleDoorGate == null && oldGate == null)
        {
            oldGate = FindFirstObjectByType<BaseGate>();
        }
    }
    
    private void Start()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnExitedBase.AddListener(DisableTrigger);
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
        Debug.Log("<color=red>BaseTrigger DISABLED - Player exited base, trigger permanently disabled for this run</color>");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (!other.CompareTag("Player")) return;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;
        if (!other.CompareTag("Player")) return;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (enableAutoClose && !hasAutoClosedGate)
        {
            if (RunStateManager.Instance != null && !RunStateManager.Instance.IsInPreRunMenu)
            {
                bool gateIsOpen = false;
                
                if (doubleDoorGate != null && doubleDoorGate.IsOpen)
                {
                    gateIsOpen = true;
                }
                else if (oldGate != null && oldGate.IsOpen)
                {
                    gateIsOpen = true;
                }
                
                if (gateIsOpen)
                {
                    hasAutoClosedGate = true;
                    
                    if (doubleDoorGate != null)
                    {
                        doubleDoorGate.EnableBarrierInstantly();
                    }
                    else if (oldGate != null)
                    {
                        oldGate.EnableBarrierInstantly();
                    }
                    
                    Debug.Log("<color=yellow>Player exited base zone - BARRIER ENABLED INSTANTLY!</color>");
                    
                    Invoke(nameof(CloseGateVisualAfterDelay), autoCloseDelay);
                    Debug.Log("<color=yellow>Gate visual will close in " + autoCloseDelay + "s...</color>");
                }
            }
        }
    }
    
    private void CloseGateVisualAfterDelay()
    {
        if (doubleDoorGate != null)
        {
            doubleDoorGate.CloseGate();
            Debug.Log("<color=orange>Double door visual closed (barrier already active)</color>");
        }
        else if (oldGate != null)
        {
            oldGate.CloseGate();
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
