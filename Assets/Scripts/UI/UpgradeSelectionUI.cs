using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private List<UpgradeOptionUI> upgradeOptions = new List<UpgradeOptionUI>();
    
    [Header("Settings")]
    [SerializeField] private bool pauseGameOnShow = true;
    
    private List<LevelUpgradeData> currentOfferedUpgrades;
    
    private void Start()
    {
        if (UpgradeSystem.Instance != null)
        {
            UpgradeSystem.Instance.OnUpgradesOffered.AddListener(ShowUpgradeChoices);
            UpgradeSystem.Instance.OnUpgradeSelected.AddListener(OnUpgradeSelected);
        }
        
        if (selectionPanel != null)
        {
            selectionPanel.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        if (UpgradeSystem.Instance != null)
        {
            UpgradeSystem.Instance.OnUpgradesOffered.RemoveListener(ShowUpgradeChoices);
            UpgradeSystem.Instance.OnUpgradeSelected.RemoveListener(OnUpgradeSelected);
        }
    }
    
    private void ShowUpgradeChoices(List<LevelUpgradeData> upgrades)
    {
        currentOfferedUpgrades = upgrades;
        
        if (selectionPanel != null)
        {
            selectionPanel.SetActive(true);
        }
        
        if (pauseGameOnShow)
        {
            Time.timeScale = 0f;
        }
        
        if (titleText != null)
        {
            int level = ExperienceSystem.Instance != null ? ExperienceSystem.Instance.CurrentLevel : 0;
            titleText.text = $"<b>LEVEL {level} - CHOOSE YOUR UPGRADE</b>";
        }
        
        for (int i = 0; i < upgradeOptions.Count; i++)
        {
            if (i < upgrades.Count)
            {
                upgradeOptions[i].SetUpgrade(upgrades[i], i, this);
                upgradeOptions[i].gameObject.SetActive(true);
            }
            else
            {
                upgradeOptions[i].gameObject.SetActive(false);
            }
        }
        
        Debug.Log("<color=cyan>Upgrade selection UI shown</color>");
    }
    
    public void OnUpgradeButtonClicked(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= currentOfferedUpgrades.Count)
        {
            Debug.LogError($"Invalid upgrade option index: {optionIndex}");
            return;
        }
        
        LevelUpgradeData selectedUpgrade = currentOfferedUpgrades[optionIndex];
        
        if (UpgradeSystem.Instance != null)
        {
            UpgradeSystem.Instance.SelectUpgrade(selectedUpgrade);
        }
    }
    
    private void OnUpgradeSelected(LevelUpgradeData upgrade)
    {
        HideUI();
    }
    
    private void HideUI()
    {
        if (selectionPanel != null)
        {
            selectionPanel.SetActive(false);
        }
        
        if (pauseGameOnShow)
        {
            Time.timeScale = 1f;
        }
        
        currentOfferedUpgrades = null;
        
        Debug.Log("<color=cyan>Upgrade selection UI hidden</color>");
    }
}
