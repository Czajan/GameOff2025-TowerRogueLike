using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    
    private PersistentData persistentData;
    private string saveFilePath;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSaveSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeSaveSystem()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        Debug.Log($"Save file path: {saveFilePath}");
        LoadGame();
    }
    
    public void SaveGame()
    {
        try
        {
            string json = JsonUtility.ToJson(persistentData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Game saved successfully! Essence: {persistentData.essence}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }
    
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                persistentData = JsonUtility.FromJson<PersistentData>(json);
                Debug.Log($"Game loaded successfully! Essence: {persistentData.essence}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load game: {e.Message}");
                persistentData = new PersistentData();
            }
        }
        else
        {
            Debug.Log("No save file found. Creating new save data.");
            persistentData = new PersistentData();
            SaveGame();
        }
    }
    
    public void ResetSave()
    {
        persistentData = new PersistentData();
        SaveGame();
        Debug.Log("Save file reset!");
    }
    
    public PersistentData GetData()
    {
        return persistentData;
    }
    
    public void AddEssence(int amount)
    {
        persistentData.essence += amount;
        SaveGame();
        Debug.Log($"Added {amount} Essence. Total: {persistentData.essence}");
    }
    
    public bool SpendEssence(int amount)
    {
        if (persistentData.essence >= amount)
        {
            persistentData.essence -= amount;
            SaveGame();
            return true;
        }
        return false;
    }
    
    public int GetEssence() => persistentData.essence;
    
    public void SaveUpgradeLevels(int moveSpeed, int health, int damage, int critChance, int critDamage, int attackRange)
    {
        persistentData.moveSpeedLevel = moveSpeed;
        persistentData.maxHealthLevel = health;
        persistentData.damageLevel = damage;
        persistentData.critChanceLevel = critChance;
        persistentData.critDamageLevel = critDamage;
        persistentData.attackRangeLevel = attackRange;
        SaveGame();
    }
    
    public void IncrementRunsCompleted()
    {
        persistentData.totalRunsCompleted++;
        SaveGame();
    }
    
    public void IncrementRunsFailed()
    {
        persistentData.totalRunsFailed++;
        SaveGame();
    }
    
    public void AddEnemyKill()
    {
        persistentData.totalEnemiesKilled++;
    }
    
    public void UpdateHighestWave(int wave)
    {
        if (wave > persistentData.highestWaveReached)
        {
            persistentData.highestWaveReached = wave;
            SaveGame();
        }
    }
    
    public int GetMoveSpeedLevel() => persistentData.moveSpeedLevel;
    public int GetMaxHealthLevel() => persistentData.maxHealthLevel;
    public int GetDamageLevel() => persistentData.damageLevel;
    public int GetCritChanceLevel() => persistentData.critChanceLevel;
    public int GetCritDamageLevel() => persistentData.critDamageLevel;
    public int GetAttackRangeLevel() => persistentData.attackRangeLevel;
    
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
