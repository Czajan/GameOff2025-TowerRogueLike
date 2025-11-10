using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance { get; private set; }
    
    [Header("Wave Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialEnemiesPerWave = 3;
    [SerializeField] private float enemiesIncreasePerWave = 2f;
    [SerializeField] private float timeBetweenWaves = 30f;
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
    [SerializeField] private bool autoStartWaves = false;
    
    private int currentWaveNumber = 0;
    private int wavesSpawned = 0;
    private bool isSpawning = false;
    private bool sessionComplete = false;
    
    private Dictionary<int, List<GameObject>> waveEnemies = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, int> waveEnemyCounts = new Dictionary<int, int>();
    private HashSet<int> completedWaves = new HashSet<int>();
    
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
        CheckWaveCompletion();
    }
    
    private void CheckWaveCompletion()
    {
        List<int> wavesToCheck = new List<int>(waveEnemies.Keys);
        
        foreach (int waveNum in wavesToCheck)
        {
            if (completedWaves.Contains(waveNum))
                continue;
            
            List<GameObject> enemies = waveEnemies[waveNum];
            enemies.RemoveAll(enemy => enemy == null);
            
            if (enemies.Count == 0 && waveEnemyCounts.ContainsKey(waveNum))
            {
                OnWaveCleared(waveNum);
            }
        }
    }
    
    private void OnWaveCleared(int waveNum)
    {
        completedWaves.Add(waveNum);
        
        if (GameProgressionManager.Instance != null && CurrencyManager.Instance != null)
        {
            GameProgressionManager.Instance.OnIndividualWaveComplete();
        }
        
        int totalKilled = waveEnemyCounts[waveNum];
        Debug.Log($"<color=green>✓ Wave {waveNum} CLEARED! All {totalKilled} enemies defeated!</color>");
        
        waveEnemies.Remove(waveNum);
        waveEnemyCounts.Remove(waveNum);
    }
    
    private IEnumerator WaveRoutine()
    {
        isSpawning = true;
        Debug.Log($"<color=cyan>=== STARTING WAVE SESSION ===</color>");
        Debug.Log($"Target: {wavesPerSession} waves spawning every {timeBetweenWaves} seconds");
        Debug.Log($"<color=yellow>Waves will OVERLAP - multiple waves can be active simultaneously!</color>");
        
        for (int i = 0; i < wavesPerSession; i++)
        {
            currentWaveNumber++;
            wavesSpawned++;
            
            StartCoroutine(SpawnWave(currentWaveNumber));
            
            if (i < wavesPerSession - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        
        Debug.Log($"<color=cyan>=== ALL {wavesPerSession} WAVES SPAWNED ===</color>");
        Debug.Log($"Waiting for all waves to be cleared...");
        
        while (waveEnemies.Count > 0)
        {
            yield return new WaitForSeconds(1f);
        }
        
        CompleteSession();
        isSpawning = false;
    }
    
    private IEnumerator SpawnWave(int waveNumber)
    {
        int enemiesToSpawn = Mathf.RoundToInt(initialEnemiesPerWave + (waveNumber - 1) * enemiesIncreasePerWave);
        
        waveEnemies[waveNumber] = new List<GameObject>();
        waveEnemyCounts[waveNumber] = enemiesToSpawn;
        
        int activeWaves = waveEnemies.Count - completedWaves.Count;
        Debug.Log($"<color=orange>▶ WAVE {waveNumber} SPAWNING: {enemiesToSpawn} enemies | Active waves: {activeWaves}</color>");
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy(waveNumber);
            yield return new WaitForSeconds(0.5f);
        }
        
        Debug.Log($"<color=yellow>Wave {waveNumber} fully spawned ({enemiesToSpawn} enemies)</color>");
    }
    
    private void SpawnEnemy(int waveNumber)
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
        
        WaveEnemy waveEnemy = enemy.GetComponent<WaveEnemy>();
        if (waveEnemy == null)
        {
            waveEnemy = enemy.AddComponent<WaveEnemy>();
        }
        waveEnemy.SetWaveNumber(waveNumber);
        
        waveEnemies[waveNumber].Add(enemy);
        
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
        if (!isSpawning)
        {
            sessionComplete = false;
            wavesSpawned = 0;
            waveEnemies.Clear();
            waveEnemyCounts.Clear();
            completedWaves.Clear();
            StartCoroutine(WaveRoutine());
            Debug.Log($"<color=cyan>StartWaves() called - beginning new overlapping wave session (continuing from wave {currentWaveNumber})</color>");
        }
        else
        {
            Debug.Log("StartWaves() called but waves already spawning! Ignoring.");
        }
    }
    
    private void CompleteSession()
    {
        sessionComplete = true;
        Debug.Log($"<color=cyan>=== WAVE SESSION COMPLETE ===</color>");
        Debug.Log($"All {wavesSpawned} waves cleared! Total waves spawned: {currentWaveNumber}");
        
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnWaveSessionComplete();
        }
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.CompleteSession();
        }
    }
    
    private void OnIndividualWaveCleared()
    {
        if (GameProgressionManager.Instance != null && CurrencyManager.Instance != null)
        {
            GameProgressionManager.Instance.OnIndividualWaveComplete();
        }
    }
    
    public void ResetSession()
    {
        sessionComplete = false;
        currentWaveNumber = 0;
        wavesSpawned = 0;
        waveEnemies.Clear();
        waveEnemyCounts.Clear();
        completedWaves.Clear();
        Debug.Log($"Session reset. Ready for next wave session.");
    }
    
    public int CurrentWave => currentWaveNumber;
    public int WavesCompletedThisSession => completedWaves.Count;
    public bool IsSessionComplete => sessionComplete;
    public int EnemiesAlive => waveEnemies.Values.Sum(list => list.Count(enemy => enemy != null));
    public int ActiveWaves => waveEnemies.Count - completedWaves.Count;
    
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
