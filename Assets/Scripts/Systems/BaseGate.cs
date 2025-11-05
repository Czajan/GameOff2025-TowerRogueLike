using UnityEngine;

public class BaseGate : MonoBehaviour
{
    [Header("Gate Settings")]
    [SerializeField] private GameObject gateVisual;
    [SerializeField] private Collider gateCollider;
    [SerializeField] private bool startsOpen = false;
    
    [Header("Animation")]
    [SerializeField] private float openHeight = 5f;
    [SerializeField] private float animationSpeed = 2f;
    
    private bool isOpen;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 targetPosition;
    
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
    }
    
    private void Start()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnEnteredBase.AddListener(CloseGate);
            GameProgressionManager.Instance.OnExitedBase.AddListener(OpenGate);
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
    }
    
    public void OpenGate()
    {
        isOpen = true;
        targetPosition = openPosition;
        
        if (gateCollider != null)
        {
            gateCollider.enabled = false;
        }
        
        Debug.Log("Gate opened - wave starting!");
    }
    
    public void CloseGate()
    {
        isOpen = false;
        targetPosition = closedPosition;
        
        if (gateCollider != null)
        {
            gateCollider.enabled = true;
        }
        
        Debug.Log("Gate closed - returned to base!");
    }
    
    public bool IsOpen => isOpen;
}
