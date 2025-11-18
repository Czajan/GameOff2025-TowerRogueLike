using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    [Header("Building Settings")]
    [SerializeField] private GameObject barricadePrefab;
    [SerializeField] private int barricadeCost = 10;
    [SerializeField] private LayerMask groundLayer;

    [Header("Preview Settings")]
    [SerializeField] private Material validPreviewMaterial;
    [SerializeField] private Material invalidPreviewMaterial;
    [SerializeField] private float rotationStep = 45f;

    private bool isBuildingModeActive = false;
    private GameObject previewObject;
    private Vector3 currentPreviewPosition;
    private float currentRotationAngle = 0f;
    private bool isValidPlacement = false;
    private List<GameObject> placedBarricades = new List<GameObject>();
    private Camera mainCamera;

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

        mainCamera = Camera.main;
    }

    private void Start()
    {
        if (barricadePrefab == null)
        {
            Debug.LogError("BuildingSystem: Barricade prefab not assigned!");
            enabled = false;
            return;
        }

        CreatePreviewObject();

        if (CurrencyManager.Instance != null && CurrencyManager.Instance.Gold == 0)
        {
            Debug.LogWarning("<color=yellow>You have 0 gold! Adding 100 gold for testing...</color>");
            CurrencyManager.Instance.AddGold(100);
        }
    }

    private void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            ToggleBuildingMode();
        }

        if (isBuildingModeActive)
        {
            HandleRotationInput();
            UpdatePreview();

            if (Mouse.current.leftButton.wasPressedThisFrame && isValidPlacement)
            {
                PlaceBarricade();
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame && placedBarricades.Count > 0)
        {
            RemoveLastBarricade();
        }
    }

    private void CreatePreviewObject()
    {
        previewObject = Instantiate(barricadePrefab);
        previewObject.name = "BarricadePreview";

        Collider[] colliders = previewObject.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        Rigidbody[] rigidbodies = previewObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            Destroy(rb);
        }

        previewObject.SetActive(false);
    }

    private void ToggleBuildingMode()
    {
        isBuildingModeActive = !isBuildingModeActive;

        if (isBuildingModeActive)
        {
            previewObject.SetActive(true);
            Debug.Log("<color=green>Building Mode ENABLED - Use A/E to rotate, Click to place</color>");
        }
        else
        {
            previewObject.SetActive(false);
            Debug.Log("<color=orange>Building Mode DISABLED</color>");
        }
    }

    private void HandleRotationInput()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            currentRotationAngle -= rotationStep;
            previewObject.transform.rotation = Quaternion.Euler(0, currentRotationAngle, 0);
            Debug.Log($"<color=cyan>Rotated to: {currentRotationAngle}°</color>");
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentRotationAngle += rotationStep;
            previewObject.transform.rotation = Quaternion.Euler(0, currentRotationAngle, 0);
            Debug.Log($"<color=cyan>Rotated to: {currentRotationAngle}°</color>");
        }
    }

    private void UpdatePreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
        {
            Vector3 snappedPosition = SnapToGrid(hit.point);
            
            Ray downRay = new Ray(snappedPosition + Vector3.up * 10f, Vector3.down);
            if (Physics.Raycast(downRay, out RaycastHit groundHit, 20f, groundLayer))
            {
                currentPreviewPosition = groundHit.point;
            }
            else
            {
                currentPreviewPosition = snappedPosition;
            }

            previewObject.transform.position = currentPreviewPosition;

            isValidPlacement = IsValidPlacementPosition(currentPreviewPosition);

            UpdatePreviewMaterial(isValidPlacement);
        }
        else
        {
            isValidPlacement = false;
            UpdatePreviewMaterial(false);
        }
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float gridSize = 1f;
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;
        return new Vector3(x, position.y, z);
    }

    private bool IsValidPlacementPosition(Vector3 position)
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogWarning("CurrencyManager.Instance is null!");
            return false;
        }

        if (CurrencyManager.Instance.Gold < barricadeCost)
        {
            return false;
        }

        int groundLayerIndex = LayerMask.NameToLayer("Ground");
        Collider[] overlaps = Physics.OverlapBox(position + Vector3.up * 0.5f, Vector3.one * 0.4f);
        
        foreach (Collider col in overlaps)
        {
            if (col.gameObject == previewObject)
            {
                continue;
            }

            if (col.gameObject.layer == groundLayerIndex)
            {
                continue;
            }

            return false;
        }

        return true;
    }

    private void UpdatePreviewMaterial(bool valid)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        Material materialToUse = valid ? validPreviewMaterial : invalidPreviewMaterial;

        if (materialToUse != null)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.sharedMaterial = materialToUse;
            }
        }
    }

    private void PlaceBarricade()
    {
        if (CurrencyManager.Instance.SpendGold(barricadeCost))
        {
            Quaternion rotation = Quaternion.Euler(0, currentRotationAngle, 0);
            GameObject newBarricade = Instantiate(barricadePrefab, currentPreviewPosition, rotation);
            newBarricade.name = $"Barricade_{placedBarricades.Count + 1}";
            placedBarricades.Add(newBarricade);

            Debug.Log($"<color=green>Barricade placed at {currentRotationAngle}°! Total: {placedBarricades.Count}</color>");
        }
    }

    private void RemoveLastBarricade()
    {
        if (placedBarricades.Count > 0)
        {
            GameObject lastBarricade = placedBarricades[placedBarricades.Count - 1];
            placedBarricades.RemoveAt(placedBarricades.Count - 1);

            CurrencyManager.Instance.AddGold(barricadeCost);

            Destroy(lastBarricade);

            Debug.Log($"<color=orange>Barricade removed! Remaining: {placedBarricades.Count}</color>");
        }
    }

    public void EnableBuildingMode()
    {
        if (!isBuildingModeActive)
        {
            ToggleBuildingMode();
        }
    }

    public void DisableBuildingMode()
    {
        if (isBuildingModeActive)
        {
            ToggleBuildingMode();
        }
    }

    public bool IsBuildingModeActive => isBuildingModeActive;
}
