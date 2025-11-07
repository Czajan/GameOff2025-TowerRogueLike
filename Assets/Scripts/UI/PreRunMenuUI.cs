using UnityEngine;
using TMPro;

public class PreRunMenuUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI essenceText;
    [SerializeField] private TextMeshProUGUI instructionText;
    
    private void Start()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
        
        if (instructionText != null)
        {
            instructionText.text = "Spend Essence on Upgrades\nApproach the gate and press [E] to start your run!";
        }
        
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnEssenceChanged.AddListener(UpdateEssenceDisplay);
            UpdateEssenceDisplay(SaveSystem.Instance != null ? SaveSystem.Instance.GetEssence() : 0);
        }
        
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.AddListener(HidePanel);
            RunStateManager.Instance.OnRunEnded.AddListener(ShowPanel);
        }
    }
    
    private void ShowPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
        
        if (SaveSystem.Instance != null && CurrencyManager.Instance != null)
        {
            UpdateEssenceDisplay(SaveSystem.Instance.GetEssence());
        }
    }
    
    private void HidePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
    
    private void UpdateEssenceDisplay(int essence)
    {
        if (essenceText != null)
        {
            essenceText.text = $"Essence: {essence}";
        }
    }
}
