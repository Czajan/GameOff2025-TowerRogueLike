using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

public class DoubleDoorGate : MonoBehaviour
{
    [Header("Door References")]
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private Collider leftDoorCollider;
    [SerializeField] private Collider rightDoorCollider;
    [SerializeField] private bool startsOpen = false;
    
    [Header("Animation")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float animationSpeed = 90f;
    
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Vector3 interactionOffset = new Vector3(0f, 10f, 0f);
    [SerializeField] private string interactionPrompt = "to Start Run";
    
    [Header("Instant Barrier (Prevents Sneaking)")]
    [SerializeField] private bool useInstantBarrier = true;
    [SerializeField] private Vector3 barrierSize = new Vector3(6f, 4f, 1f);
    [SerializeField] private Vector3 barrierOffset = new Vector3(0f, 2f, 0f);
    
    [Header("Auto-Push on Start")]
    [SerializeField] private bool useAutoPush = true;
    [SerializeField] private Vector3 pushOutDirection = new Vector3(0f, 0f, -1f);
    [SerializeField] private float pushDistance = 5f;
    [SerializeField] private float pushDuration = 1.5f;
    [SerializeField] private bool lockCameraDuringPush = true;
    [SerializeField] private bool forcePlayerRotation = true;
    [SerializeField] private AnimationCurve pushSpeedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float walkAnimationSpeed = 1f;
    
    [Header("Events")]
    public UnityEvent OnPushStarted;
    public UnityEvent OnPushComplete;
    public UnityEvent OnGateOpened;
    public UnityEvent OnGateClosed;
    
    private bool isOpen;
    private Quaternion leftDoorClosedRotation;
    private Quaternion rightDoorClosedRotation;
    private Quaternion leftDoorOpenRotation;
    private Quaternion rightDoorOpenRotation;
    private Quaternion leftDoorTargetRotation;
    private Quaternion rightDoorTargetRotation;
    private Transform playerTransform;
    private bool canInteract = false;
    private InputAction interactAction;
    private BoxCollider instantBarrier;
    private CharacterController playerController;
    private CinemachineMouseOrbit cameraOrbit;
    
    private void Awake()
    {
        if (leftDoor == null || rightDoor == null)
        {
            Debug.LogError("DoubleDoorGate: Left or Right door not assigned!");
            return;
        }
        
        leftDoorClosedRotation = leftDoor.localRotation;
        rightDoorClosedRotation = rightDoor.localRotation;
        
        leftDoorOpenRotation = leftDoorClosedRotation * Quaternion.Euler(0f, -openAngle, 0f);
        rightDoorOpenRotation = rightDoorClosedRotation * Quaternion.Euler(0f, openAngle, 0f);
        
        isOpen = startsOpen;
        leftDoorTargetRotation = isOpen ? leftDoorOpenRotation : leftDoorClosedRotation;
        rightDoorTargetRotation = isOpen ? rightDoorOpenRotation : rightDoorClosedRotation;
        
        leftDoor.localRotation = leftDoorTargetRotation;
        rightDoor.localRotation = rightDoorTargetRotation;
        
        if (leftDoorCollider != null)
        {
            leftDoorCollider.enabled = !isOpen;
        }
        
        if (rightDoorCollider != null)
        {
            rightDoorCollider.enabled = !isOpen;
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
            Debug.Log($"<color=cyan>DoubleDoorGate: Instant barrier set to Ground layer ({groundLayer})</color>");
        }
        else
        {
            barrierObj.layer = 0;
            Debug.LogWarning("DoubleDoorGate: Ground layer not found - using Default layer. May not block CharacterController!");
        }
        
        instantBarrier = barrierObj.AddComponent<BoxCollider>();
        instantBarrier.size = barrierSize;
        instantBarrier.center = Vector3.zero;
        instantBarrier.isTrigger = false;
        instantBarrier.enabled = !isOpen;
        
        Debug.Log($"<color=cyan>DoubleDoorGate: Created instant barrier (size={barrierSize}, offset={barrierOffset}, enabled={instantBarrier.enabled})</color>");
    }
    
    private void Start()
    {
        Debug.Log("<color=cyan>====== DoubleDoorGate Start() called ======</color>");
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerController = player.GetComponent<CharacterController>();
            Debug.Log($"<color=green>DoubleDoorGate: Player found at {player.transform.position}</color>");
            
            PlayerInput playerInput = player.GetComponent<PlayerInput>();
            if (playerInput != null && playerInput.actions != null)
            {
                interactAction = playerInput.actions.FindActionMap("Player").FindAction("Interact");
                if (interactAction != null)
                {
                    interactAction.Enable();
                    Debug.Log("<color=cyan>DoubleDoorGate: Interact action successfully bound!</color>");
                }
                else
                {
                    Debug.LogWarning("DoubleDoorGate: Could not find 'Interact' action in Player action map.");
                }
            }
            else
            {
                Debug.LogWarning("DoubleDoorGate: PlayerInput component or actions not found on Player.");
            }
        }
        else
        {
            Debug.LogError("DoubleDoorGate: Player GameObject with 'Player' tag not found!");
        }
        
        if (lockCameraDuringPush)
        {
            cameraOrbit = FindFirstObjectByType<CinemachineMouseOrbit>();
            if (cameraOrbit != null)
            {
                Debug.Log("<color=green>DoubleDoorGate: Found CinemachineMouseOrbit for camera lock</color>");
            }
        }
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.AddListener(OpenGate);
            RunStateManager.Instance.OnRunEnded.AddListener(CloseGate);
            Debug.Log("<color=green>DoubleDoorGate: Registered with RunStateManager</color>");
        }
        else
        {
            Debug.LogError("<color=red>DoubleDoorGate: RunStateManager.Instance is NULL!</color>");
        }
        
        Debug.Log("<color=cyan>====== DoubleDoorGate Start() complete ======</color>");
    }
    
    private void Update()
    {
        if (leftDoor != null && Quaternion.Angle(leftDoor.localRotation, leftDoorTargetRotation) > 0.1f)
        {
            leftDoor.localRotation = Quaternion.RotateTowards(
                leftDoor.localRotation,
                leftDoorTargetRotation,
                animationSpeed * Time.deltaTime
            );
        }
        
        if (rightDoor != null && Quaternion.Angle(rightDoor.localRotation, rightDoorTargetRotation) > 0.1f)
        {
            rightDoor.localRotation = Quaternion.RotateTowards(
                rightDoor.localRotation,
                rightDoorTargetRotation,
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
        
        Vector3 interactionPoint = transform.position + interactionOffset;
        float distance = Vector3.Distance(interactionPoint, playerTransform.position);
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
            if (useAutoPush)
            {
                StartCoroutine(PushPlayerOutAndStartRun());
            }
            else
            {
                if (RunStateManager.Instance != null)
                {
                    RunStateManager.Instance.StartRun();
                    Debug.Log("<color=green>Player clicked gate - Run starting!</color>");
                }
            }
        }
    }
    
    private IEnumerator PushPlayerOutAndStartRun()
    {
        Debug.Log("<color=cyan>Starting cinematic gate push sequence!</color>");
        
        OnPushStarted?.Invoke();
        
        PlayerController playerControllerScript = playerTransform?.GetComponent<PlayerController>();
        if (playerControllerScript != null)
        {
            playerControllerScript.SetMovementEnabled(false);
            playerControllerScript.SetAnimationSpeedOverride(walkAnimationSpeed);
        }
        
        if (lockCameraDuringPush && cameraOrbit != null)
        {
            cameraOrbit.enabled = false;
        }
        
        HideInteractionPrompt();
        
        Vector3 startPosition = playerTransform.position;
        Quaternion startRotation = playerTransform.rotation;
        Vector3 pushDir = pushOutDirection.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(pushDir);
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.StartRun();
        }
        
        float elapsed = 0f;
        
        while (elapsed < pushDuration && playerController != null && playerTransform != null)
        {
            float t = elapsed / pushDuration;
            float curveValue = pushSpeedCurve.Evaluate(t);
            
            float stepDistance = (pushDistance / pushDuration) * Time.deltaTime;
            Vector3 movement = pushDir * stepDistance;
            playerController.Move(movement);
            
            if (forcePlayerRotation)
            {
                playerTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            }
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        Debug.Log("<color=green>Cinematic gate push complete! Player control restored.</color>");
        
        if (playerControllerScript != null)
        {
            playerControllerScript.ClearAnimationSpeedOverride();
            playerControllerScript.SetMovementEnabled(true);
        }
        
        if (lockCameraDuringPush && cameraOrbit != null)
        {
            cameraOrbit.enabled = true;
        }
        
        OnPushComplete?.Invoke();
    }
    
    public void OpenGate()
    {
        isOpen = true;
        leftDoorTargetRotation = leftDoorOpenRotation;
        rightDoorTargetRotation = rightDoorOpenRotation;
        
        if (leftDoorCollider != null)
        {
            leftDoorCollider.enabled = false;
        }
        
        if (rightDoorCollider != null)
        {
            rightDoorCollider.enabled = false;
        }
        
        if (instantBarrier != null)
        {
            instantBarrier.enabled = false;
        }
        
        OnGateOpened?.Invoke();
        Debug.Log("Double doors opened - run starting!");
    }
    
    public void EnableBarrierInstantly()
    {
        if (instantBarrier != null && !instantBarrier.enabled)
        {
            instantBarrier.enabled = true;
            Debug.Log("<color=red>⚠ INSTANT BARRIER ENABLED - Gate blocked immediately!</color>");
        }
        
        if (leftDoorCollider != null && !leftDoorCollider.enabled)
        {
            leftDoorCollider.enabled = true;
        }
        
        if (rightDoorCollider != null && !rightDoorCollider.enabled)
        {
            rightDoorCollider.enabled = true;
        }
    }
    
    public void CloseGate()
    {
        isOpen = false;
        leftDoorTargetRotation = leftDoorClosedRotation;
        rightDoorTargetRotation = rightDoorClosedRotation;
        
        if (leftDoorCollider != null)
        {
            leftDoorCollider.enabled = true;
        }
        
        if (rightDoorCollider != null)
        {
            rightDoorCollider.enabled = true;
        }
        
        if (instantBarrier != null)
        {
            instantBarrier.enabled = true;
        }
        
        OnGateClosed?.Invoke();
        Debug.Log("Double doors closed - back to pre-run menu!");
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
        Vector3 interactionPoint = transform.position + interactionOffset;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint, 0.3f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactionPoint, interactionRange);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, interactionPoint);
        
        if (useInstantBarrier)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(barrierOffset, barrierSize);
            Gizmos.matrix = Matrix4x4.identity;
        }
        
        if (useAutoPush)
        {
            Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
            Vector3 pushStart = transform.position;
            Vector3 pushEnd = pushStart + pushOutDirection.normalized * pushDistance;
            Gizmos.DrawLine(pushStart, pushEnd);
            Gizmos.DrawWireSphere(pushEnd, 0.5f);
        }
    }
}
