using UnityEngine;
using TMPro;

public class BetweenSessionsUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI instructionText;
    
    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
    
    private void Start()
    {
        HidePanel();
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnSessionCompleted.AddListener(ShowPanel);
            RunStateManager.Instance.OnSessionStarted.AddListener(HidePanel);
            RunStateManager.Instance.OnBetweenSessionsTimerUpdate.AddListener(UpdateTimer);
        }
    }
    
    private void ShowPanel()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        if (instructionText != null)
        {
            instructionText.text = "Spend Gold on Obstacles!\nNext wave session starting soon...";
        }
        
        Debug.Log("<color=cyan>BetweenSessionsPanel shown</color>");
    }
    
    private void HidePanel()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        
        Debug.Log("<color=cyan>BetweenSessionsPanel hidden</color>");
    }
    
    private void UpdateTimer(float timeRemaining)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    
    private void OnDestroy()
    {
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnSessionCompleted.RemoveListener(ShowPanel);
            RunStateManager.Instance.OnSessionStarted.RemoveListener(HidePanel);
            RunStateManager.Instance.OnBetweenSessionsTimerUpdate.RemoveListener(UpdateTimer);
        }
    }
}
