using UnityEngine;

public class VisualFeedback : MonoBehaviour
{
    [Header("Damage Flash")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    
    private Color originalColor;
    private Material material;
    private float flashTimer;
    
    private void Start()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }
        
        if (targetRenderer != null)
        {
            material = targetRenderer.material;
            originalColor = material.color;
        }
    }
    
    private void Update()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0 && material != null)
            {
                material.color = originalColor;
            }
        }
    }
    
    public void FlashDamage()
    {
        if (material != null)
        {
            material.color = damageColor;
            flashTimer = flashDuration;
        }
    }
    
    private void OnDestroy()
    {
        if (material != null)
        {
            Destroy(material);
        }
    }
}
