using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game State")]
    [SerializeField] private bool isPaused = false;
    
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private WaveSpawner waveSpawner;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath.AddListener(OnPlayerDeath);
        }
    }
    
    private void OnPlayerDeath()
    {
        Debug.Log("Game Over! Player has died.");
        
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnRunComplete(false);
        }
        
        Invoke(nameof(RestartGame), 3f);
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public bool IsPaused => isPaused;
}
