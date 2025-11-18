using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float flySpeed = 10f;
    
    [Header("Visual")]
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.3f;
    
    private Transform playerTransform;
    private Vector3 startPosition;
    private float bobOffset;
    private bool isFlying = false;
    private int xpValue;
    
    public void Initialize(int xp)
    {
        xpValue = xp;
    }
    
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            isFlying = true;
        }
        else
        {
            Debug.LogWarning("ExperienceOrb: Player not found! Make sure player has 'Player' tag.");
            Destroy(gameObject);
            return;
        }
        
        startPosition = transform.position;
        bobOffset = Random.Range(0f, Mathf.PI * 2f);
    }
    
    private void Update()
    {
        if (!isFlying || playerTransform == null) return;
        
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        transform.position += directionToPlayer * flySpeed * Time.deltaTime;
        
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        float bobAmount = Mathf.Sin(Time.time * bobSpeed + bobOffset) * bobHeight;
        transform.position += Vector3.up * bobAmount * Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }
    
    private void Collect()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddExperience(xpValue);
        }
        
        Destroy(gameObject);
    }
}
