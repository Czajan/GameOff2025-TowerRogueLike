using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private float fadeDuration = 1f;
    
    private CanvasGroup canvasGroup;
    private Coroutine currentNotification;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        HideImmediate();
    }
    
    private void Start()
    {
        if (GameProgressionManager.Instance != null)
        {
            WaveSpawner spawner = FindFirstObjectByType<WaveSpawner>();
            if (spawner != null)
            {
                GameProgressionManager.Instance.OnEnteredBase.AddListener(() => OnEnteredBase(spawner));
            }
        }
    }
    
    private void OnEnteredBase(WaveSpawner spawner)
    {
        if (spawner.IsSessionComplete)
        {
            ShowNotification($"Wave Session Complete! Waves cleared: {spawner.WavesCompletedThisSession}");
        }
    }
    
    public void ShowNotification(string message)
    {
        if (currentNotification != null)
        {
            StopCoroutine(currentNotification);
        }
        
        currentNotification = StartCoroutine(DisplayNotification(message));
    }
    
    private IEnumerator DisplayNotification(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
        
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);
        
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
        
        yield return new WaitForSecondsRealtime(displayDuration);
        
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        HideImmediate();
    }
    
    private void HideImmediate()
    {
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
