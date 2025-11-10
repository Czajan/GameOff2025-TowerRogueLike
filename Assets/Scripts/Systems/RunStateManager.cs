using UnityEngine;
using UnityEngine.Events;

public enum RunState
{
    PreRunMenu,
    WaveSession,
    BetweenSessions
}

public class RunStateManager : MonoBehaviour
{
    public static RunStateManager Instance { get; private set; }
    
    [Header("State Configuration")]
    [SerializeField] private float betweenSessionsDuration = 60f;
    
    [Header("Player Teleport")]
    [SerializeField] private Transform baseSpawnPoint;
    
    [Header("Current State")]
    [SerializeField] private RunState currentState = RunState.PreRunMenu;
    [SerializeField] private int currentSessionNumber = 0;
    [SerializeField] private float betweenSessionsTimer = 0f;
    
    [Header("Events")]
    public UnityEvent OnRunStarted = new UnityEvent();
    public UnityEvent OnSessionStarted = new UnityEvent();
    public UnityEvent OnSessionCompleted = new UnityEvent();
    public UnityEvent<float> OnBetweenSessionsTimerUpdate = new UnityEvent<float>();
    public UnityEvent OnRunEnded = new UnityEvent();
    
    private bool runActive = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        if (baseSpawnPoint == null)
        {
            GameObject baseObject = GameObject.Find("Base");
            if (baseObject != null)
            {
                baseSpawnPoint = baseObject.transform;
                Debug.Log("<color=cyan>RunStateManager: Auto-assigned Base as spawn point</color>");
            }
        }
        
        SetState(RunState.PreRunMenu);
    }
    
    private void Update()
    {
        if (currentState == RunState.BetweenSessions && betweenSessionsTimer > 0f)
        {
            betweenSessionsTimer -= Time.deltaTime;
            OnBetweenSessionsTimerUpdate?.Invoke(betweenSessionsTimer);
            
            if (betweenSessionsTimer <= 0f)
            {
                betweenSessionsTimer = 0f;
                OnBetweenSessionsTimerUpdate?.Invoke(0f);
                Debug.Log("<color=yellow>Between-sessions timer expired! Starting next session...</color>");
                StartNextSession();
            }
        }
    }
    
    public void StartRun()
    {
        if (runActive)
        {
            Debug.LogWarning("Run already active!");
            return;
        }
        
        runActive = true;
        currentSessionNumber = 0;
        
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetInRunCurrencies();
        }
        
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetTemporaryBonuses();
        }
        
        if (ExperienceSystem.Instance != null)
        {
            ExperienceSystem.Instance.ResetLevel();
        }
        
        OnRunStarted?.Invoke();
        Debug.Log("<color=cyan>=== RUN STARTED ===</color>");
        
        StartNextSession();
    }
    
    public void StartNextSession()
    {
        currentSessionNumber++;
        SetState(RunState.WaveSession);
        
        OnSessionStarted?.Invoke();
        
        if (WaveSpawner.Instance != null)
        {
            WaveSpawner.Instance.StartWaves();
        }
        
        Debug.Log($"<color=green>=== SESSION {currentSessionNumber} STARTED (Waves {(currentSessionNumber - 1) * 10 + 1}-{currentSessionNumber * 10}) ===</color>");
    }
    
    public void CompleteSession()
    {
        if (currentState != RunState.WaveSession)
        {
            Debug.LogWarning("Not in wave session state!");
            return;
        }
        
        SetState(RunState.BetweenSessions);
        betweenSessionsTimer = betweenSessionsDuration;
        
        OnSessionCompleted?.Invoke();
        OnBetweenSessionsTimerUpdate?.Invoke(betweenSessionsTimer);
        
        Debug.Log($"<color=yellow>=== SESSION {currentSessionNumber} COMPLETE ===</color>");
        Debug.Log($"<color=yellow>You have {betweenSessionsDuration} seconds to spend Gold on obstacles!</color>");
    }
    
    public void EndRun(bool victory)
    {
        runActive = false;
        SetState(RunState.PreRunMenu);
        
        TeleportPlayerToBase();
        
        OnRunEnded?.Invoke();
        
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnRunComplete(victory);
        }
        
        Debug.Log($"<color=cyan>=== RUN ENDED ({(victory ? "VICTORY" : "DEFEAT")}) ===</color>");
    }
    
    private void TeleportPlayerToBase()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && baseSpawnPoint != null)
        {
            CharacterController characterController = player.GetComponent<CharacterController>();
            
            if (characterController != null)
            {
                characterController.enabled = false;
                player.transform.position = baseSpawnPoint.position;
                characterController.enabled = true;
            }
            else
            {
                player.transform.position = baseSpawnPoint.position;
            }
            
            Debug.Log("<color=green>✓ Player teleported to base spawn point</color>");
        }
        else
        {
            if (player == null)
            {
                Debug.LogWarning("TeleportPlayerToBase: Player not found!");
            }
            if (baseSpawnPoint == null)
            {
                Debug.LogWarning("TeleportPlayerToBase: Base spawn point not assigned!");
            }
        }
    }
    
    private void SetState(RunState newState)
    {
        currentState = newState;
        Debug.Log($"<color=orange>State changed to: {newState}</color>");
    }
    
    public RunState CurrentState => currentState;
    public int CurrentSessionNumber => currentSessionNumber;
    public float BetweenSessionsTimer => betweenSessionsTimer;
    public bool IsRunActive => runActive;
    public bool IsInPreRunMenu => currentState == RunState.PreRunMenu;
    public bool IsInWaveSession => currentState == RunState.WaveSession;
    public bool IsInBetweenSessions => currentState == RunState.BetweenSessions;
}
