using UnityEngine;
using TMPro;

public class NPCInteractionPrompt : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private string interactionKey = "E";
    [SerializeField] private bool faceCamera = true;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, 0);
    
    private Transform mainCamera;
    private ShopNPC parentNPC;
    
    private void Start()
    {
        mainCamera = Camera.main?.transform;
        parentNPC = GetComponentInParent<ShopNPC>();
        
        if (promptText != null && parentNPC != null)
        {
            promptText.text = $"[{interactionKey}] Talk to {parentNPC.GetNPCName()}";
        }
        
        transform.localPosition = offset;
    }
    
    private void LateUpdate()
    {
        if (faceCamera && mainCamera != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);
        }
    }
    
    public void SetPromptText(string text)
    {
        if (promptText != null)
        {
            promptText.text = text;
        }
    }
}
