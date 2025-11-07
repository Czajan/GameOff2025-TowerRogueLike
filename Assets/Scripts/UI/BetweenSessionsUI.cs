using UnityEngine;
using TMPro;

public class BetweenSessionsUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI instructionText;
    
    private void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnSessionCompleted.AddListener(ShowPanel);
            RunStateManager.Instance.OnSessionStarted.AddListener(HidePanel);
            RunStateManager.Instance.OnBetweenSessionsTimerUpdate.AddListener(UpdateTimer);
        }
    }
    
    private void ShowPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
        
        if (instructionText != null)
        {
            instructionText.text = "Spend Gold on Obstacles!\nNext wave session starting soon...";
        }
    }
    
    private void HidePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
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
}
