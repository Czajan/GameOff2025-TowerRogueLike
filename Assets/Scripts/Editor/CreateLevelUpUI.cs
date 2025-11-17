using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class CreateLevelUpUI : EditorWindow
{
    [MenuItem("Tools/Setup Level-Up UI Panel")]
    public static void SetupLevelUpUIPanel()
    {
        GameObject canvas = GameObject.Find("GameCanvas");
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("Error", "GameCanvas not found in scene!", "OK");
            return;
        }

        GameObject levelUpPanel = GameObject.Find("LevelUpPanel");
        if (levelUpPanel == null)
        {
            EditorUtility.DisplayDialog("Error", "LevelUpPanel not found! It should already exist under GameCanvas.", "OK");
            return;
        }

        ConfigureLevelUpPanel(levelUpPanel);
        CreateUpgradeButtonPrefab();
        AssignReferencesToLevelUpUI(levelUpPanel);

        EditorUtility.DisplayDialog("Success", 
            "Level-Up UI Panel configured!\n\n" +
            "✓ Panel background and layout set\n" +
            "✓ Title text configured\n" +
            "✓ Options container set to vertical layout\n" +
            "✓ Upgrade button prefab created\n" +
            "✓ References assigned to LevelUpUI component\n\n" +
            "Next: Run 'Tools > Generate Level Upgrade Assets'", 
            "OK");

        Debug.Log("<color=green>✓ Level-Up UI Panel setup complete!</color>");
    }

    private static void ConfigureLevelUpPanel(GameObject panel)
    {
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(800, 500);

        Image panelImage = panel.GetComponent<Image>();
        if (panelImage != null)
        {
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        }

        Transform titleTransform = panel.transform.Find("LevelUpTitle");
        if (titleTransform != null)
        {
            RectTransform titleRect = titleTransform.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.pivot = new Vector2(0.5f, 1);
            titleRect.anchoredPosition = new Vector2(0, -20);
            titleRect.sizeDelta = new Vector2(-40, 80);

            TextMeshProUGUI titleText = titleTransform.GetComponent<TextMeshProUGUI>();
            if (titleText != null)
            {
                titleText.text = "★ LEVEL UP ★\nChoose Your Upgrade";
                titleText.fontSize = 36;
                titleText.alignment = TextAlignmentOptions.Center;
                titleText.color = Color.white;
                titleText.fontStyle = FontStyles.Bold;
            }
        }

        Transform containerTransform = panel.transform.Find("OptionsContainer");
        if (containerTransform != null)
        {
            RectTransform containerRect = containerTransform.GetComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0, 0);
            containerRect.anchorMax = new Vector2(1, 1);
            containerRect.pivot = new Vector2(0.5f, 0.5f);
            containerRect.anchoredPosition = new Vector2(0, -60);
            containerRect.sizeDelta = new Vector2(-40, -140);

            Image containerImage = containerTransform.GetComponent<Image>();
            if (containerImage != null)
            {
                containerImage.enabled = false;
            }

            HorizontalLayoutGroup horizLayout = containerTransform.GetComponent<HorizontalLayoutGroup>();
            if (horizLayout != null)
            {
                DestroyImmediate(horizLayout);
            }

            VerticalLayoutGroup vertLayout = containerTransform.GetComponent<VerticalLayoutGroup>();
            if (vertLayout == null)
            {
                vertLayout = containerTransform.gameObject.AddComponent<VerticalLayoutGroup>();
            }

            vertLayout.childAlignment = TextAnchor.UpperCenter;
            vertLayout.spacing = 15;
            vertLayout.childForceExpandWidth = true;
            vertLayout.childForceExpandHeight = false;
            vertLayout.childControlWidth = true;
            vertLayout.childControlHeight = true;
            vertLayout.padding = new RectOffset(20, 20, 20, 20);
        }

        panel.SetActive(false);

        EditorUtility.SetDirty(panel);
    }

    private static void CreateUpgradeButtonPrefab()
    {
        string prefabPath = "Assets/Prefabs/UI/UpgradeButton.prefab";
        
        if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
        {
            Debug.Log("UpgradeButton prefab already exists, skipping creation.");
            return;
        }

        GameObject buttonObj = new GameObject("UpgradeButton");
        
        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(700, 120);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        
        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        colors.highlightedColor = new Color(0.45f, 0.45f, 0.45f, 1f);
        colors.pressedColor = new Color(0.6f, 0.6f, 0.3f, 1f);
        button.colors = colors;

        GameObject nameObj = new GameObject("Name");
        nameObj.transform.SetParent(buttonObj.transform);
        RectTransform nameRect = nameObj.AddComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0, 0.5f);
        nameRect.anchorMax = new Vector2(1, 1);
        nameRect.pivot = new Vector2(0.5f, 1);
        nameRect.anchoredPosition = new Vector2(0, -10);
        nameRect.sizeDelta = new Vector2(-20, 40);
        
        TextMeshProUGUI nameText = nameObj.AddComponent<TextMeshProUGUI>();
        nameText.text = "Upgrade Name";
        nameText.fontSize = 28;
        nameText.alignment = TextAlignmentOptions.Left;
        nameText.color = Color.white;
        nameText.fontStyle = FontStyles.Bold;

        GameObject descObj = new GameObject("Description");
        descObj.transform.SetParent(buttonObj.transform);
        RectTransform descRect = descObj.AddComponent<RectTransform>();
        descRect.anchorMin = new Vector2(0, 0);
        descRect.anchorMax = new Vector2(1, 0.5f);
        descRect.pivot = new Vector2(0.5f, 0);
        descRect.anchoredPosition = new Vector2(0, 10);
        descRect.sizeDelta = new Vector2(-20, -20);
        
        TextMeshProUGUI descText = descObj.AddComponent<TextMeshProUGUI>();
        descText.text = "Upgrade description and bonuses";
        descText.fontSize = 18;
        descText.alignment = TextAlignmentOptions.TopLeft;
        descText.color = new Color(0.9f, 0.9f, 0.9f, 1f);

        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/UI"))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "UI");
        }

        PrefabUtility.SaveAsPrefabAsset(buttonObj, prefabPath);
        DestroyImmediate(buttonObj);

        Debug.Log($"<color=green>✓ Created UpgradeButton prefab at {prefabPath}</color>");
    }

    private static void AssignReferencesToLevelUpUI(GameObject panel)
    {
        LevelUpUI levelUpUI = panel.GetComponent<LevelUpUI>();
        if (levelUpUI == null)
        {
            levelUpUI = panel.AddComponent<LevelUpUI>();
        }

        SerializedObject so = new SerializedObject(levelUpUI);

        so.FindProperty("levelUpPanel").objectReferenceValue = panel;

        Transform titleTransform = panel.transform.Find("LevelUpTitle");
        if (titleTransform != null)
        {
            so.FindProperty("levelTitleText").objectReferenceValue = titleTransform.GetComponent<TextMeshProUGUI>();
        }

        Transform containerTransform = panel.transform.Find("OptionsContainer");
        if (containerTransform != null)
        {
            so.FindProperty("optionsContainer").objectReferenceValue = containerTransform;
        }

        GameObject buttonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/UpgradeButton.prefab");
        if (buttonPrefab != null)
        {
            so.FindProperty("optionButtonPrefab").objectReferenceValue = buttonPrefab;
        }

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(levelUpUI);

        Debug.Log("<color=green>✓ Assigned all references to LevelUpUI component!</color>");
    }
}
