using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeOptionUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button selectButton;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI stackText;
    
    [Header("Rarity Colors")]
    [SerializeField] private Color commonColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private Color rareColor = new Color(0.29f, 0.56f, 0.89f);
    [SerializeField] private Color legendaryColor = new Color(1f, 0.84f, 0f);
    
    private UpgradeData upgradeData;
    private int optionIndex;
    private UpgradeSelectionUI parentUI;
    
    private void Awake()
    {
        if (selectButton != null)
        {
            selectButton.onClick.AddListener(OnButtonClicked);
        }
    }
    
    public void SetUpgrade(UpgradeData upgrade, int index, UpgradeSelectionUI parent)
    {
        upgradeData = upgrade;
        optionIndex = index;
        parentUI = parent;
        
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (upgradeData == null) return;
        
        if (iconImage != null && upgradeData.icon != null)
        {
            iconImage.sprite = upgradeData.icon;
            iconImage.gameObject.SetActive(true);
        }
        else if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }
        
        Color rarityColor = GetRarityColor(upgradeData.rarity);
        
        if (backgroundImage != null)
        {
            backgroundImage.color = rarityColor * 0.3f;
        }
        
        if (nameText != null)
        {
            nameText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(rarityColor)}><b>{upgradeData.upgradeName}</b></color>";
        }
        
        if (descriptionText != null)
        {
            descriptionText.text = GetFormattedDescription();
        }
        
        if (rarityText != null)
        {
            rarityText.text = upgradeData.rarity.ToString().ToUpper();
            rarityText.color = rarityColor;
        }
        
        if (stackText != null && UpgradeSystem.Instance != null)
        {
            int currentStacks = UpgradeSystem.Instance.GetUpgradeStacks(upgradeData);
            if (currentStacks > 0)
            {
                stackText.text = $"Stack {currentStacks + 1}/{upgradeData.maxStacks}";
                stackText.gameObject.SetActive(true);
            }
            else
            {
                stackText.gameObject.SetActive(false);
            }
        }
    }
    
    private string GetFormattedDescription()
    {
        string desc = upgradeData.description;
        
        if (upgradeData.damageBonus > 0)
            desc += $"\n<color=yellow>+{upgradeData.damageBonus} Damage</color>";
        
        if (upgradeData.maxHealthBonus > 0)
            desc += $"\n<color=yellow>+{upgradeData.maxHealthBonus} Max HP</color>";
        
        if (upgradeData.moveSpeedBonus > 0)
            desc += $"\n<color=yellow>+{upgradeData.moveSpeedBonus * 100:F0}% Move Speed</color>";
        
        if (upgradeData.critChanceBonus > 0)
            desc += $"\n<color=yellow>+{upgradeData.critChanceBonus * 100:F0}% Crit Chance</color>";
        
        if (upgradeData.critDamageBonus > 0)
            desc += $"\n<color=yellow>+{upgradeData.critDamageBonus * 100:F0}% Crit Damage</color>";
        
        if (upgradeData.attackSpeedBonus > 0)
            desc += $"\n<color=yellow>+{upgradeData.attackSpeedBonus * 100:F0}% Attack Speed</color>";
        
        if (upgradeData.attackRangeBonus > 0)
            desc += $"\n<color=yellow>+{upgradeData.attackRangeBonus}m Attack Range</color>";
        
        return desc;
    }
    
    private Color GetRarityColor(UpgradeRarity rarity)
    {
        switch (rarity)
        {
            case UpgradeRarity.Common: return commonColor;
            case UpgradeRarity.Rare: return rareColor;
            case UpgradeRarity.Legendary: return legendaryColor;
            default: return Color.white;
        }
    }
    
    private void OnButtonClicked()
    {
        if (parentUI != null)
        {
            parentUI.OnUpgradeButtonClicked(optionIndex);
        }
    }
}
