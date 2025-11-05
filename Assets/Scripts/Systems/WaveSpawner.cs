using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance { get; private set; }
    
    [Header("Wave Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialEnemiesPerWave = 3;
    [SerializeField] private float enemiesIncreasePerWave = 2f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int wavesPerSession = 10;
    
    [Header("Spawn Settings")]
    [SerializeField] private Transform globalSpawnPoint;
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float minSpawnDistance = 8f;
    [SerializeField] private WaveController waveController;
    
    [Header("Defense Zones")]
    [SerializeField] private DefenseZone activeDefenseZone;
    
    [Header("Debug")]
    [SerializeField] private bool autoStartWaves = true;
    
    private int currentWave = 0;
    private int wavesCompletedThisSession = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;
    private bool sessionComplete = false;
    private List<GameObject> activeEnemies = new List<GameObject>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        if (waveController == null)
        {
            waveController = FindFirstObjectByType<WaveController>();
        }
        
        if (activeDefenseZone == null)
        {
            DefenseZone[] zones = FindObjectsByType<DefenseZone>(FindObjectsSortMode.None);
            foreach (DefenseZone zone in zones)
            {
                if (zone.IsActive)
                {
                    activeDefenseZone = zone;
                    Debug.Log($"WaveSpawner: Found active zone {zone.ZoneIndex + 1}");
                    break;
                }
            }
        }
        
        if (autoStartWaves)
        {
            StartCoroutine(WaveRoutine());
        }
    }
    
    private void Update()
    {
        CleanupDestroyedEnemies();
    }
    
    private void CleanupDestroyedEnemies()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        enemiesAlive = activeEnemies.Count;
    }
    
    private IEnumerator WaveRoutine()
    {
        isSpawning = true;
        Debug.Log($"Starting wave session! Target: {wavesPerSession} waves");
        
        while (wavesCompletedThisSession < wavesPerSession)
        {
            yield return StartCoroutine(SpawnWave());
            
            wavesCompletedThisSession++;
            Debug.Log($"Wave {currentWave} spawned! Session progress: {wavesCompletedThisSession}/{wavesPerSession}. Enemies alive: {enemiesAlive}");
            
            if (wavesCompletedThisSession >= wavesPerSession)
            {
                CompleteSession();
                break;
            }
            
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        
        isSpawning = false;
        Debug.Log("Wave routine ended. isSpawning = false");
    }
    
    private IEnumerator SpawnWave()
    {
        isSpawning = true;
        currentWave++;
        
        int enemiesToSpawn = Mathf.RoundToInt(initialEnemiesPerWave + (currentWave - 1) * enemiesIncreasePerWave);
        
        if (enemiesAlive > 0)
        {
            Debug.Log($"<color=orange>Starting Wave {currentWave} with {enemiesToSpawn} enemies! ({enemiesAlive} enemies from previous waves still alive)</color>");
        }
        else
        {
            Debug.Log($"Starting Wave {currentWave} with {enemiesToSpawn} enemies!");
        }
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        
        isSpawning = false;
    }
    
    private void SpawnEnemy()
    {
        if (enemyPrefab == null || playerTransform == null)
        {
            Debug.LogWarning("Enemy prefab or player transform not set!");
            return;
        }
        
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        CharacterController enemyController = enemyPrefab.GetComponent<CharacterController>();
        if (enemyController != null)
        {
            if (Mathf.Approximately(enemyController.center.y, 0f))
            {
                spawnPosition.y = enemyController.height / 2f;
            }
        }
        
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(enemy);
        enemiesAlive++;
        
        if (activeDefenseZone != null)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.SetDefenseZone(activeDefenseZone);
            }
        }
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 basePosition;
        
        if (globalSpawnPoint != null)
        {
            basePosition = globalSpawnPoint.position;
        }
        else if (activeDefenseZone != null)
        {
            basePosition = activeDefenseZone.GetCenterPosition();
        }
        else if (waveController != null)
        {
            basePosition = waveController.GetSpawnPosition();
        }
        else if (playerTransform != null)
        {
            basePosition = playerTransform.position;
        }
        else
        {
            basePosition = Vector3.zero;
        }
        
        Vector3 spawnPosition;
        int attempts = 0;
        const int maxAttempts = 30;
        
        do
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            spawnPosition = basePosition + new Vector3(randomCircle.x, 0, randomCircle.y);
            spawnPosition.y = 0;
            attempts++;
        }
        while (playerTransform != null && 
               Vector3.Distance(spawnPosition, playerTransform.position) < minSpawnDistance && 
               attempts < maxAttempts);
        
        return spawnPosition;
    }
    
    public void SetActiveZone(DefenseZone zone)
    {
        activeDefenseZone = zone;
        Debug.Log($"<color=yellow>WaveSpawner: Zone changed to {zone.ZoneIndex + 1}. Enemies will now spawn there.</color>");
    }
    
    public void StartWaves()
    {
        if (!isSpawning && !sessionComplete)
        {
            sessionComplete = false;
            wavesCompletedThisSession = 0;
            StartCoroutine(WaveRoutine());
            Debug.Log("StartWaves() called - beginning new wave session");
        }
        else if (sessionComplete)
        {
            Debug.Log("StartWaves() called but session already complete! Ignoring.");
        }
        else
        {
            Debug.Log("StartWaves() called but waves already spawning! Ignoring.");
        }
    }
    
    private void CompleteSession()
    {
        sessionComplete = true;
        Debug.Log($"Wave session complete! Completed {wavesCompletedThisSession} waves. Total waves: {currentWave}. Return to base!");
        
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnWaveSessionComplete();
        }
    }
    
    public void ResetSession()
    {
        wavesCompletedThisSession = 0;
        sessionComplete = false;
        Debug.Log($"Session reset. Ready for next wave session. Current wave: {currentWave}");
    }
    
    public int CurrentWave => currentWave;
    public int WavesCompletedThisSession => wavesCompletedThisSession;
    public bool IsSessionComplete => sessionComplete;
    public int EnemiesAlive => enemiesAlive;
    
    private void OnDrawGizmosSelected()
    {
        if (globalSpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(globalSpawnPoint.position, spawnRadius);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(globalSpawnPoint.position, 0.5f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(globalSpawnPoint.position, minSpawnDistance);
        }
        else if (playerTransform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerTransform.position, minSpawnDistance);
        }
    }
}
