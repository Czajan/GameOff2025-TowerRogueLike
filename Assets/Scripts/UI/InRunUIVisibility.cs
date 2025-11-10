using UnityEngine;

public class InRunUIVisibility : MonoBehaviour
{
    [Header("UI Panels to Show/Hide")]
    [SerializeField] private GameObject[] inRunPanels;
    
    private void Start()
    {
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.AddListener(ShowInRunUI);
            RunStateManager.Instance.OnRunEnded.AddListener(HideInRunUI);
        }
        
        if (RunStateManager.Instance != null && RunStateManager.Instance.IsInPreRunMenu)
        {
            HideInRunUI();
        }
        else
        {
            ShowInRunUI();
        }
    }
    
    private void ShowInRunUI()
    {
        SetPanelsActive(true);
    }
    
    private void HideInRunUI()
    {
        SetPanelsActive(false);
    }
    
    private void SetPanelsActive(bool active)
    {
        if (inRunPanels == null) return;
        
        foreach (GameObject panel in inRunPanels)
        {
            if (panel != null)
            {
                panel.SetActive(active);
            }
        }
    }
}
