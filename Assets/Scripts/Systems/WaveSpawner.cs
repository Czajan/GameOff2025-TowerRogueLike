using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialEnemiesPerWave = 3;
    [SerializeField] private float enemiesIncreasePerWave = 2f;
    [SerializeField] private float timeBetweenWaves = 5f;
    
    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float minSpawnDistance = 8f;
    
    [Header("Debug")]
    [SerializeField] private bool autoStartWaves = true;
    
    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;
    private List<GameObject> activeEnemies = new List<GameObject>();
    
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
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            yield return StartCoroutine(SpawnWave());
            yield return new WaitUntil(() => enemiesAlive == 0);
        }
    }
    
    private IEnumerator SpawnWave()
    {
        isSpawning = true;
        currentWave++;
        
        int enemiesToSpawn = Mathf.RoundToInt(initialEnemiesPerWave + (currentWave - 1) * enemiesIncreasePerWave);
        
        Debug.Log($"Starting Wave {currentWave} with {enemiesToSpawn} enemies!");
        
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
                Debug.Log($"Spawning enemy at Y={spawnPosition.y} (adjusted for center=0, height={enemyController.height})");
            }
            else
            {
                Debug.Log($"Spawning enemy at Y={spawnPosition.y} (center.y={enemyController.center.y}, height={enemyController.height})");
            }
        }
        
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(enemy);
        enemiesAlive++;
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        int attempts = 0;
        const int maxAttempts = 30;
        
        do
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            spawnPosition = playerTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            spawnPosition.y = 0;
            attempts++;
        }
        while (Vector3.Distance(spawnPosition, playerTransform.position) < minSpawnDistance && attempts < maxAttempts);
        
        return spawnPosition;
    }
    
    public void StartWaves()
    {
        if (!isSpawning)
        {
            StartCoroutine(WaveRoutine());
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (playerTransform == null)
            return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerTransform.position, minSpawnDistance);
    }
    
    public int CurrentWave => currentWave;
    public int EnemiesAlive => enemiesAlive;
}
