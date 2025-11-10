using UnityEngine;
using TMPro;

public class InteractionNotificationUI : MonoBehaviour
{
    public static InteractionNotificationUI Instance { get; private set; }
    
    [Header("UI References")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;
    
    [Header("Settings")]
    [SerializeField] private string defaultInteractKey = "E";
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }
    }
    
    public void ShowNotification(string message)
    {
        if (notificationText != null)
        {
            notificationText.text = message;
        }
        
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(true);
        }
    }
    
    public void ShowInteractionPrompt(string action)
    {
        ShowNotification($"Press [{defaultInteractKey}] {action}");
    }
    
    public void HideNotification()
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }
    }
    
    public void SetInteractKey(string key)
    {
        defaultInteractKey = key;
    }
}
