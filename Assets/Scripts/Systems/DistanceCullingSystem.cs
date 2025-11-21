using UnityEngine;
using System.Collections.Generic;

public class DistanceCullingSystem : MonoBehaviour
{
    public static DistanceCullingSystem Instance { get; private set; }
    
    [Header("Culling Settings")]
    [SerializeField] private float cullingDistance = 100f;
    [SerializeField] private float enableDistance = 80f;
    [SerializeField] private float updateInterval = 0.5f;
    
    [Header("Object Management")]
    [SerializeField] private bool autoRegisterOnStart = true;
    [SerializeField] private string[] tagsToIgnore = { "Player", "MainCamera", "GameController" };
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = false;
    
    private Transform playerTransform;
    private List<CullableObject> cullableObjects = new List<CullableObject>();
    private float nextUpdateTime;
    private int activeObjectCount;
    private int culledObjectCount;
    
    private class CullableObject
    {
        public GameObject gameObject;
        public Transform transform;
        public Vector3 initialPosition;
        public bool wasActive;
        public float sqrCullingDistance;
        public float sqrEnableDistance;
        
        public CullableObject(GameObject obj, float cullingDist, float enableDist)
        {
            gameObject = obj;
            transform = obj.transform;
            initialPosition = transform.position;
            wasActive = obj.activeSelf;
            sqrCullingDistance = cullingDist * cullingDist;
            sqrEnableDistance = enableDist * enableDist;
        }
    }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("DistanceCullingSystem: Player not found! Culling disabled.");
            enabled = false;
            return;
        }
        
        if (autoRegisterOnStart)
        {
            RegisterAllEnvironmentObjects();
        }
        
        Debug.Log($"<color=cyan>DistanceCullingSystem initialized. Culling Distance: {cullingDistance}, Enable Distance: {enableDistance}</color>");
    }
    
    private void Update()
    {
        if (playerTransform == null || Time.time < nextUpdateTime)
            return;
        
        nextUpdateTime = Time.time + updateInterval;
        UpdateCulling();
    }
    
    private void RegisterAllEnvironmentObjects()
    {
        cullableObjects.Clear();
        
        MeshRenderer[] allRenderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
        HashSet<string> ignoreTags = new HashSet<string>(tagsToIgnore);
        
        int registeredCount = 0;
        
        foreach (MeshRenderer renderer in allRenderers)
        {
            GameObject obj = renderer.gameObject;
            
            if (ignoreTags.Contains(obj.tag))
                continue;
            
            if (obj.GetComponent<CullableObject>() != null)
                continue;
            
            if (obj.transform.root != obj.transform && 
                (obj.transform.root.CompareTag("Player") || obj.transform.root.CompareTag("Enemy")))
                continue;
            
            CullableObject cullable = new CullableObject(obj, cullingDistance, enableDistance);
            cullableObjects.Add(cullable);
            registeredCount++;
        }
        
        Debug.Log($"<color=green>Registered {registeredCount} objects for distance culling</color>");
    }
    
    private void UpdateCulling()
    {
        if (playerTransform == null)
            return;
        
        Vector3 playerPos = playerTransform.position;
        activeObjectCount = 0;
        culledObjectCount = 0;
        
        foreach (CullableObject cullable in cullableObjects)
        {
            if (cullable.gameObject == null)
                continue;
            
            float sqrDistance = (cullable.initialPosition - playerPos).sqrMagnitude;
            bool isCurrentlyActive = cullable.gameObject.activeSelf;
            
            if (isCurrentlyActive && sqrDistance > cullable.sqrCullingDistance)
            {
                cullable.gameObject.SetActive(false);
                culledObjectCount++;
            }
            else if (!isCurrentlyActive && sqrDistance < cullable.sqrEnableDistance)
            {
                cullable.gameObject.SetActive(true);
                activeObjectCount++;
            }
            else if (isCurrentlyActive)
            {
                activeObjectCount++;
            }
            else
            {
                culledObjectCount++;
            }
        }
    }
    
    public void RegisterObject(GameObject obj)
    {
        if (obj == null)
            return;
        
        CullableObject cullable = new CullableObject(obj, cullingDistance, enableDistance);
        cullableObjects.Add(cullable);
    }
    
    public void UnregisterObject(GameObject obj)
    {
        if (obj == null)
            return;
        
        cullableObjects.RemoveAll(c => c.gameObject == obj);
    }
    
    public void SetCullingDistance(float distance)
    {
        cullingDistance = distance;
        enableDistance = distance * 0.8f;
        
        foreach (CullableObject cullable in cullableObjects)
        {
            cullable.sqrCullingDistance = cullingDistance * cullingDistance;
            cullable.sqrEnableDistance = enableDistance * enableDistance;
        }
    }
    
    public void EnableAll()
    {
        foreach (CullableObject cullable in cullableObjects)
        {
            if (cullable.gameObject != null)
            {
                cullable.gameObject.SetActive(true);
            }
        }
    }
    
    public void RefreshRegistration()
    {
        RegisterAllEnvironmentObjects();
    }
    
    private void OnGUI()
    {
        if (!showDebugInfo)
            return;
        
        GUI.Box(new Rect(10, 120, 250, 100), "Distance Culling System");
        GUI.Label(new Rect(20, 145, 230, 20), $"Total Objects: {cullableObjects.Count}");
        GUI.Label(new Rect(20, 165, 230, 20), $"Active: {activeObjectCount}");
        GUI.Label(new Rect(20, 185, 230, 20), $"Culled: {culledObjectCount}");
        GUI.Label(new Rect(20, 205, 230, 20), $"Culling Distance: {cullingDistance:F0}");
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
