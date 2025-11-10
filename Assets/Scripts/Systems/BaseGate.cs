using UnityEngine;
using UnityEngine.InputSystem;

public class BaseGate : MonoBehaviour
{
    [Header("Gate Settings")]
    [SerializeField] private GameObject gateVisual;
    [SerializeField] private Collider gateCollider;
    [SerializeField] private bool startsOpen = false;
    
    [Header("Animation")]
    [SerializeField] private float openHeight = 5f;
    [SerializeField] private float animationSpeed = 2f;
    
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private string interactionPrompt = "to Start Run";
    
    [Header("Instant Barrier (Prevents Sneaking)")]
    [SerializeField] private bool useInstantBarrier = true;
    [SerializeField] private Vector3 barrierSize = new Vector3(6f, 4f, 1f);
    [SerializeField] private Vector3 barrierOffset = new Vector3(0f, 2f, 0f);
    
    private bool isOpen;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 targetPosition;
    private Transform playerTransform;
    private bool canInteract = false;
    private InputAction interactAction;
    private BoxCollider instantBarrier;
    
    private void Awake()
    {
        if (gateVisual == null)
            gateVisual = gameObject;
        
        closedPosition = gateVisual.transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
        
        isOpen = startsOpen;
        targetPosition = isOpen ? openPosition : closedPosition;
        gateVisual.transform.position = targetPosition;
        
        if (gateCollider != null)
        {
            gateCollider.enabled = !isOpen;
        }
        
        if (useInstantBarrier)
        {
            CreateInstantBarrier();
        }
    }
    
    private void CreateInstantBarrier()
    {
        GameObject barrierObj = new GameObject("InstantBarrier");
        barrierObj.transform.SetParent(transform);
        barrierObj.transform.localPosition = barrierOffset;
        barrierObj.transform.localRotation = Quaternion.identity;
        barrierObj.transform.localScale = Vector3.one;
        
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer != -1)
        {
            barrierObj.layer = groundLayer;
            Debug.Log($"<color=cyan>BaseGate: Instant barrier set to Ground layer ({groundLayer})</color>");
        }
        else
        {
            barrierObj.layer = 0;
            Debug.LogWarning("BaseGate: Ground layer not found - using Default layer. May not block CharacterController!");
        }
        
        instantBarrier = barrierObj.AddComponent<BoxCollider>();
        instantBarrier.size = barrierSize;
        instantBarrier.center = Vector3.zero;
        instantBarrier.isTrigger = false;
        instantBarrier.enabled = !isOpen;
        
        Debug.Log($"<color=cyan>BaseGate: Created instant barrier (size={barrierSize}, offset={barrierOffset}, enabled={instantBarrier.enabled})</color>");
    }
    
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            
            PlayerInput playerInput = player.GetComponent<PlayerInput>();
            if (playerInput != null && playerInput.actions != null)
            {
                interactAction = playerInput.actions.FindActionMap("Player").FindAction("Interact");
                if (interactAction != null)
                {
                    interactAction.Enable();
                    Debug.Log("<color=cyan>BaseGate: Interact action successfully bound!</color>");
                }
                else
                {
                    Debug.LogWarning("BaseGate: Could not find 'Interact' action in Player action map.");
                }
            }
            else
            {
                Debug.LogWarning("BaseGate: PlayerInput component or actions not found on Player.");
            }
        }
        else
        {
            Debug.LogError("BaseGate: Player GameObject with 'Player' tag not found!");
        }
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.AddListener(OpenGate);
            RunStateManager.Instance.OnRunEnded.AddListener(CloseGate);
        }
    }
    
    private void Update()
    {
        if (Vector3.Distance(gateVisual.transform.position, targetPosition) > 0.01f)
        {
            gateVisual.transform.position = Vector3.MoveTowards(
                gateVisual.transform.position,
                targetPosition,
                animationSpeed * Time.deltaTime
            );
        }
        
        if (RunStateManager.Instance != null && RunStateManager.Instance.IsInPreRunMenu)
        {
            CheckPlayerProximity();
            HandleInteraction();
        }
        else
        {
            if (canInteract)
            {
                HideInteractionPrompt();
                canInteract = false;
            }
        }
    }
    
    private void CheckPlayerProximity()
    {
        if (playerTransform == null) return;
        
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool wasInteractable = canInteract;
        canInteract = distance <= interactionRange && !isOpen;
        
        if (canInteract && !wasInteractable)
        {
            ShowInteractionPrompt();
        }
        else if (!canInteract && wasInteractable)
        {
            HideInteractionPrompt();
        }
    }
    
    private void ShowInteractionPrompt()
    {
        if (InteractionNotificationUI.Instance != null)
        {
            InteractionNotificationUI.Instance.ShowInteractionPrompt(interactionPrompt);
        }
    }
    
    private void HideInteractionPrompt()
    {
        if (InteractionNotificationUI.Instance != null)
        {
            InteractionNotificationUI.Instance.HideNotification();
        }
    }
    
    private void HandleInteraction()
    {
        if (canInteract && interactAction != null && interactAction.WasPressedThisFrame())
        {
            if (RunStateManager.Instance != null)
            {
                RunStateManager.Instance.StartRun();
                Debug.Log("<color=green>Player clicked gate - Run starting!</color>");
            }
        }
    }
    
    public void OpenGate()
    {
        isOpen = true;
        targetPosition = openPosition;
        
        if (gateCollider != null)
        {
            gateCollider.enabled = false;
        }
        
        if (instantBarrier != null)
        {
            instantBarrier.enabled = false;
        }
        
        Debug.Log("Gate opened - run starting!");
    }
    
    public void EnableBarrierInstantly()
    {
        if (instantBarrier != null && !instantBarrier.enabled)
        {
            instantBarrier.enabled = true;
            Debug.Log("<color=red>⚠ INSTANT BARRIER ENABLED - Gate blocked immediately!</color>");
        }
        
        if (gateCollider != null && !gateCollider.enabled)
        {
            gateCollider.enabled = true;
        }
    }
    
    public void CloseGate()
    {
        isOpen = false;
        targetPosition = closedPosition;
        
        if (gateCollider != null)
        {
            gateCollider.enabled = true;
        }
        
        if (instantBarrier != null)
        {
            instantBarrier.enabled = true;
        }
        
        Debug.Log("Gate closed - back to pre-run menu!");
    }
    
    public bool IsOpen => isOpen;
    
    private void OnDestroy()
    {
        if (interactAction != null)
        {
            interactAction.Disable();
        }
        
        HideInteractionPrompt();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
        
        if (useInstantBarrier)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(barrierOffset, barrierSize);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}
