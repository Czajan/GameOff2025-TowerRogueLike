using UnityEngine;
using TMPro;

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
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private TextMeshProUGUI promptText;
    
    private bool isOpen;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 targetPosition;
    private Transform playerTransform;
    private bool canInteract = false;
    
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
        
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }
    
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
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
            if (promptText != null && promptText.gameObject.activeSelf)
            {
                promptText.gameObject.SetActive(false);
            }
            canInteract = false;
        }
    }
    
    private void CheckPlayerProximity()
    {
        if (playerTransform == null) return;
        
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        canInteract = distance <= interactionRange && !isOpen;
        
        if (promptText != null)
        {
            promptText.gameObject.SetActive(canInteract);
            if (canInteract)
            {
                promptText.text = $"Press [{interactionKey}] to Start Run";
            }
        }
    }
    
    private void HandleInteraction()
    {
        if (canInteract && Input.GetKeyDown(interactionKey))
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
        
        Debug.Log("Gate opened - run starting!");
    }
    
    public void CloseGate()
    {
        isOpen = false;
        targetPosition = closedPosition;
        
        if (gateCollider != null)
        {
            gateCollider.enabled = true;
        }
        
        Debug.Log("Gate closed - back to pre-run menu!");
    }
    
    public bool IsOpen => isOpen;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
