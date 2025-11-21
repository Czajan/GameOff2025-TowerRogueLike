using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadarUI : MonoBehaviour
{
    [Header("Radar Settings")]
    [SerializeField] private RectTransform radarContainer;
    [SerializeField] private float radarRadius = 100f;
    [SerializeField] private float worldRadius = 50f;
    [SerializeField] private bool rotateWithPlayer = false;
    
    [Header("Radar Colors")]
    [SerializeField] private Color playerColor = Color.yellow;
    [SerializeField] private Color defenseZoneColor = Color.cyan;
    [SerializeField] private Color enemyColor = Color.red;
    [SerializeField] private Color radarBackgroundColor = new Color(0f, 0f, 0f, 0.7f);
    
    [Header("Blip Settings")]
    [SerializeField] private float playerBlipSize = 8f;
    [SerializeField] private float defenseZoneBlipSize = 10f;
    [SerializeField] private float enemyBlipSize = 6f;
    
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Image radarBackground;
    
    private List<RadarBlip> radarBlips = new List<RadarBlip>();
    private Image playerBlip;
    private Sprite circleSprite;
    private Transform activeDefenseZoneTransform;
    
    private void Awake()
    {
        circleSprite = CreateCircleSprite(64);
    }
    
    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform = mainCam.transform;
            }
        }
        
        SetupRadarBackground();
        CreatePlayerBlip();
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.AddListener(ShowRadar);
            RunStateManager.Instance.OnRunEnded.AddListener(HideRadar);
            
            if (RunStateManager.Instance.IsInPreRunMenu)
            {
                HideRadar();
            }
            else
            {
                ShowRadar();
            }
        }
        
        Debug.Log($"<color=cyan>Radar UI initialized. Player: {(player != null ? player.name : "NULL")}, Camera: {(cameraTransform != null ? cameraTransform.name : "NULL")}</color>");
    }
    
    private void SetupRadarBackground()
    {
        if (radarBackground != null)
        {
            radarBackground.sprite = circleSprite;
            radarBackground.color = radarBackgroundColor;
            radarBackground.type = Image.Type.Filled;
        }
        
        Image containerImage = radarContainer.GetComponent<Image>();
        if (containerImage == null)
        {
            containerImage = radarContainer.gameObject.AddComponent<Image>();
        }
        containerImage.sprite = circleSprite;
        containerImage.color = new Color(0.1f, 0.1f, 0.1f, 0.3f);
    }
    
    private Sprite CreateCircleSprite(int resolution)
    {
        Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        
        Color[] pixels = new Color[resolution * resolution];
        Vector2 center = new Vector2(resolution / 2f, resolution / 2f);
        float radius = resolution / 2f;
        
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                Vector2 pos = new Vector2(x, y);
                float distance = Vector2.Distance(pos, center);
                
                if (distance <= radius)
                {
                    float alpha = 1f - Mathf.Clamp01((distance - (radius - 2f)) / 2f);
                    pixels[y * resolution + x] = new Color(1f, 1f, 1f, alpha);
                }
                else
                {
                    pixels[y * resolution + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f), 100f);
    }
    
    private void CreatePlayerBlip()
    {
        GameObject blipObj = new GameObject("PlayerBlip");
        blipObj.transform.SetParent(radarContainer, false);
        
        playerBlip = blipObj.AddComponent<Image>();
        playerBlip.sprite = circleSprite;
        playerBlip.color = playerColor;
        
        RectTransform rectTransform = blipObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(playerBlipSize, playerBlipSize);
        rectTransform.anchoredPosition = Vector2.zero;
        
        Debug.Log("<color=yellow>Player blip created on radar</color>");
    }
    
    private void Update()
    {
        if (player == null) return;
        
        UpdateRadarRotation();
        UpdateRadarBlips();
    }
    
    private void UpdateRadarRotation()
    {
        if (rotateWithPlayer && radarContainer != null && cameraTransform != null)
        {
            radarContainer.rotation = Quaternion.Euler(0f, 0f, -cameraTransform.eulerAngles.y);
        }
    }
    
    private void UpdateRadarBlips()
    {
        if (player == null || cameraTransform == null) return;
        
        foreach (RadarBlip blip in radarBlips)
        {
            if (blip.target == null)
            {
                blip.blipImage.gameObject.SetActive(false);
                continue;
            }
            
            if (blip.isDefenseZone && blip.target != activeDefenseZoneTransform)
            {
                blip.blipImage.gameObject.SetActive(false);
                continue;
            }
            
            blip.blipImage.gameObject.SetActive(true);
            
            Vector3 playerPos = new Vector3(player.position.x, 0f, player.position.z);
            Vector3 targetPos = new Vector3(blip.target.position.x, 0f, blip.target.position.z);
            Vector3 offset = targetPos - playerPos;
            
            float distance = offset.magnitude;
            
            bool clampToEdge = blip.isDefenseZone && distance > worldRadius;
            float radarDistance = Mathf.Min(distance / worldRadius, 1f) * radarRadius;
            
            Vector3 forward = rotateWithPlayer ? Vector3.forward : new Vector3(Mathf.Sin(cameraTransform.eulerAngles.y * Mathf.Deg2Rad), 0f, Mathf.Cos(cameraTransform.eulerAngles.y * Mathf.Deg2Rad));
            Vector3 right = rotateWithPlayer ? Vector3.right : new Vector3(Mathf.Sin((cameraTransform.eulerAngles.y + 90f) * Mathf.Deg2Rad), 0f, Mathf.Cos((cameraTransform.eulerAngles.y + 90f) * Mathf.Deg2Rad));
            
            float dotForward = Vector3.Dot(offset.normalized, forward);
            float dotRight = Vector3.Dot(offset.normalized, right);
            
            Vector2 radarPos = new Vector2(dotRight * radarDistance, dotForward * radarDistance);
            blip.rectTransform.anchoredPosition = radarPos;
        }
    }
    
    public void RegisterDefenseZone(Transform zoneTransform)
    {
        CreateBlip(zoneTransform, defenseZoneColor, defenseZoneBlipSize, "DefenseZoneBlip", true);
        Debug.Log($"<color=cyan>Defense zone registered on radar: {zoneTransform.name}</color>");
    }
    
    public void SetActiveDefenseZone(Transform activeZoneTransform)
    {
        activeDefenseZoneTransform = activeZoneTransform;
        
        int hiddenCount = 0;
        int shownCount = 0;
        
        foreach (RadarBlip blip in radarBlips)
        {
            if (blip.isDefenseZone && blip.blipImage != null)
            {
                bool shouldShow = blip.target == activeZoneTransform;
                blip.blipImage.gameObject.SetActive(shouldShow);
                
                if (shouldShow)
                {
                    shownCount++;
                    Debug.Log($"<color=cyan>Showing defense zone blip: {blip.target.name}</color>");
                }
                else
                {
                    hiddenCount++;
                }
            }
        }
        
        Debug.Log($"<color=cyan>Active defense zone on radar: {(activeZoneTransform != null ? activeZoneTransform.name : "NONE")} - Shown: {shownCount}, Hidden: {hiddenCount}</color>");
    }
    
    public void RegisterEnemy(Transform enemyTransform)
    {
        CreateBlip(enemyTransform, enemyColor, enemyBlipSize, "EnemyBlip", false);
        Debug.Log($"<color=red>Enemy registered on radar: {enemyTransform.name}</color>");
    }
    
    private void CreateBlip(Transform target, Color color, float size, string name, bool isDefenseZone)
    {
        if (target == null)
        {
            Debug.LogWarning("Attempted to create radar blip with null target!");
            return;
        }
        
        GameObject blipObj = new GameObject(name);
        blipObj.transform.SetParent(radarContainer, false);
        
        Image blipImage = blipObj.AddComponent<Image>();
        blipImage.sprite = circleSprite;
        blipImage.color = color;
        
        RectTransform rectTransform = blipObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(size, size);
        
        RadarBlip radarBlip = new RadarBlip
        {
            target = target,
            blipImage = blipImage,
            rectTransform = rectTransform,
            isDefenseZone = isDefenseZone
        };
        
        radarBlips.Add(radarBlip);
        
        if (isDefenseZone)
        {
            blipImage.gameObject.SetActive(false);
        }
    }
    
    public void UnregisterBlip(Transform target)
    {
        RadarBlip blipToRemove = radarBlips.Find(b => b.target == target);
        if (blipToRemove != null)
        {
            if (blipToRemove.blipImage != null)
            {
                Destroy(blipToRemove.blipImage.gameObject);
            }
            radarBlips.Remove(blipToRemove);
        }
    }
    
    public void ClearAllEnemyBlips()
    {
        List<RadarBlip> blipsToRemove = radarBlips.FindAll(b => b.blipImage != null && b.blipImage.color == enemyColor);
        
        foreach (RadarBlip blip in blipsToRemove)
        {
            if (blip.blipImage != null)
            {
                Destroy(blip.blipImage.gameObject);
            }
            radarBlips.Remove(blip);
        }
    }
    
    private void ShowRadar()
    {
        gameObject.SetActive(true);
        Debug.Log("<color=green>Radar shown</color>");
    }
    
    private void HideRadar()
    {
        gameObject.SetActive(false);
        Debug.Log("<color=yellow>Radar hidden</color>");
    }
    
    private class RadarBlip
    {
        public Transform target;
        public Image blipImage;
        public RectTransform rectTransform;
        public bool isDefenseZone;
    }
}
