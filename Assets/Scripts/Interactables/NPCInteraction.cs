using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class NPCInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private string interactionPrompt = "to Talk";
    [SerializeField] private bool requiresPreRunMenu = false;
    
    [Header("Events")]
    public UnityEvent OnInteract;
    
    private Transform playerTransform;
    private InputAction interactAction;
    private bool canInteract = false;
    
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
                }
            }
        }
    }
    
    private void Update()
    {
        if (ShouldCheckInteraction())
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
    
    private bool ShouldCheckInteraction()
    {
        if (requiresPreRunMenu)
        {
            return RunStateManager.Instance != null && RunStateManager.Instance.IsInPreRunMenu;
        }
        return true;
    }
    
    private void CheckPlayerProximity()
    {
        if (playerTransform == null) return;
        
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool wasInteractable = canInteract;
        canInteract = distance <= interactionRange;
        
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
            OnInteract?.Invoke();
            Debug.Log($"<color=cyan>Interacted with {gameObject.name}</color>");
        }
    }
    
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
