using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timerPanel;
    
    private void Start()
    {
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnBetweenSessionsTimerUpdate.AddListener(UpdateTimer);
            RunStateManager.Instance.OnSessionCompleted.AddListener(ShowTimer);
            RunStateManager.Instance.OnSessionStarted.AddListener(HideTimer);
            RunStateManager.Instance.OnRunEnded.AddListener(HideTimer);
        }
        
        HideTimer();
    }
    
    private void UpdateTimer(float timeRemaining)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = $"Next Session: {minutes:00}:{seconds:00}";
        }
    }
    
    private void ShowTimer()
    {
        if (timerPanel != null)
        {
            timerPanel.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    
    private void HideTimer()
    {
        if (timerPanel != null)
        {
            timerPanel.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnBetweenSessionsTimerUpdate.RemoveListener(UpdateTimer);
            RunStateManager.Instance.OnSessionCompleted.RemoveListener(ShowTimer);
            RunStateManager.Instance.OnSessionStarted.RemoveListener(HideTimer);
            RunStateManager.Instance.OnRunEnded.RemoveListener(HideTimer);
        }
    }
}