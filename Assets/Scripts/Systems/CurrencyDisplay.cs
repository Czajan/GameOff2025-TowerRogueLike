using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    
    private void Start()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnCurrencyChanged.AddListener(UpdateDisplay);
            UpdateDisplay(GameProgressionManager.Instance.Currency);
        }
    }
    
    private void UpdateDisplay(int currency)
    {
        if (currencyText != null)
        {
            currencyText.text = $"Currency: ${currency}";
        }
    }
    
    private void OnDestroy()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnCurrencyChanged.RemoveListener(UpdateDisplay);
        }
    }
}