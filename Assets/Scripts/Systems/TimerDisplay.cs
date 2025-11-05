using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    private void Start()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnBaseTimerUpdate.AddListener(UpdateTimer);
            GameProgressionManager.Instance.OnEnteredBase.AddListener(ShowTimer);
            GameProgressionManager.Instance.OnExitedBase.AddListener(HideTimer);
            
            if (GameProgressionManager.Instance.IsInBase)
            {
                ShowTimer();
            }
            else
            {
                HideTimer();
            }
        }
        else
        {
            HideTimer();
        }
    }
    
    private void UpdateTimer(float timeRemaining)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = $"Next Wave: {minutes:00}:{seconds:00}";
        }
    }
    
    private void ShowTimer()
    {
        gameObject.SetActive(true);
    }
    
    private void HideTimer()
    {
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnBaseTimerUpdate.RemoveListener(UpdateTimer);
            GameProgressionManager.Instance.OnEnteredBase.RemoveListener(ShowTimer);
            GameProgressionManager.Instance.OnExitedBase.RemoveListener(HideTimer);
        }
    }
}