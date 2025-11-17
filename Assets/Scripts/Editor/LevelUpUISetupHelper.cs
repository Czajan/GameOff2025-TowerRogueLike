using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class LevelUpUISetupHelper : EditorWindow
{
    [MenuItem("Tools/Setup Level-Up UI")]
    public static void ShowWindow()
    {
        GetWindow<LevelUpUISetupHelper>("Level-Up UI Setup");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Level-Up UI Setup Helper", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        if (GUILayout.Button("1. Auto-Assign LevelUpPanel References", GUILayout.Height(40)))
        {
            AutoAssignLevelUpPanelReferences();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("2. Assign All Upgrades to UpgradeSystem", GUILayout.Height(40)))
        {
            AssignUpgradesToSystem();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("3. Validate Complete Setup", GUILayout.Height(40)))
        {
            ValidateSetup();
        }
        
        GUILayout.Space(20);
        EditorGUILayout.HelpBox("Click buttons in order:\n1. Assigns UI references\n2. Loads upgrade assets\n3. Validates everything", MessageType.Info);
    }
    
    private void AutoAssignLevelUpPanelReferences()
    {
        GameObject levelUpPanel = GameObject.Find("LevelUpPanel");
        if (levelUpPanel == null)
        {
            EditorUtility.DisplayDialog("Error", "LevelUpPanel GameObject not found in scene!\n\nMake sure you have /GameCanvas/LevelUpPanel in your hierarchy.", "OK");
            return;
        }
        
        LevelUpUI levelUpUI = levelUpPanel.GetComponent<LevelUpUI>();
        if (levelUpUI == null)
        {
            EditorUtility.DisplayDialog("Error", "LevelUpUI component not found on LevelUpPanel!\n\nAdd the LevelUpUI component to /GameCanvas/LevelUpPanel.", "OK");
            return;
        }
        
        SerializedObject so = new SerializedObject(levelUpUI);
        
        so.FindProperty("levelUpPanel").objectReferenceValue = levelUpPanel;
        
        Transform titleTransform = levelUpPanel.transform.Find("LevelUpTitle");
        if (titleTransform != null)
        {
            so.FindProperty("levelTitleText").objectReferenceValue = titleTransform.GetComponent<TextMeshProUGUI>();
        }
        
        Transform containerTransform = levelUpPanel.transform.Find("OptionsContainer");
        if (containerTransform != null)
        {
            so.FindProperty("optionsContainer").objectReferenceValue = containerTransform;
        }
        
        GameObject itemButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/ItemButton.prefab");
        if (itemButtonPrefab != null)
        {
            so.FindProperty("optionButtonPrefab").objectReferenceValue = itemButtonPrefab;
        }
        
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(levelUpUI);
        
        Debug.Log("<color=green>✓ LevelUpUI references assigned successfully!</color>");
        EditorUtility.DisplayDialog("Success", "LevelUpUI references have been assigned!\n\n✓ Panel\n✓ Title Text\n✓ Options Container\n✓ Button Prefab", "OK");
    }
    
    private void AssignUpgradesToSystem()
    {
        GameObject gameManagers = GameObject.Find("GameManagers");
        if (gameManagers == null)
        {
            EditorUtility.DisplayDialog("Error", "GameManagers GameObject not found in scene!", "OK");
            return;
        }
        
        UpgradeSystem upgradeSystem = gameManagers.GetComponent<UpgradeSystem>();
        if (upgradeSystem == null)
        {
            EditorUtility.DisplayDialog("Error", "UpgradeSystem component not found on GameManagers!\n\nAdd the UpgradeSystem component first.", "OK");
            return;
        }
        
        string[] guids = AssetDatabase.FindAssets("t:LevelUpgradeData", new[] { "Assets/Data/LevelUpgrades" });
        
        if (guids.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "No LevelUpgradeData assets found!\n\nRun Tools > Generate Level Upgrade Assets first.", "OK");
            return;
        }
        
        LevelUpgradeData[] upgrades = new LevelUpgradeData[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            upgrades[i] = AssetDatabase.LoadAssetAtPath<LevelUpgradeData>(path);
        }
        
        SerializedObject so = new SerializedObject(upgradeSystem);
        SerializedProperty upgradePoolProp = so.FindProperty("upgradePool");
        upgradePoolProp.arraySize = upgrades.Length;
        
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgradePoolProp.GetArrayElementAtIndex(i).objectReferenceValue = upgrades[i];
        }
        
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(upgradeSystem);
        
        Debug.Log($"<color=green>✓ Assigned {upgrades.Length} upgrades to UpgradeSystem!</color>");
        EditorUtility.DisplayDialog("Success", $"Assigned {upgrades.Length} upgrade assets to UpgradeSystem!\n\nCheck /GameManagers in the Inspector.", "OK");
    }
    
    private void ValidateSetup()
    {
        bool allGood = true;
        string report = "=== VALIDATION REPORT ===\n\n";
        
        GameObject levelUpPanel = GameObject.Find("LevelUpPanel");
        if (levelUpPanel == null)
        {
            report += "❌ LevelUpPanel not found\n";
            allGood = false;
        }
        else
        {
            report += "✓ LevelUpPanel exists\n";
            
            LevelUpUI ui = levelUpPanel.GetComponent<LevelUpUI>();
            if (ui == null)
            {
                report += "❌ LevelUpUI component missing\n";
                allGood = false;
            }
            else
            {
                report += "✓ LevelUpUI component attached\n";
            }
        }
        
        GameObject gameManagers = GameObject.Find("GameManagers");
        if (gameManagers == null)
        {
            report += "❌ GameManagers not found\n";
            allGood = false;
        }
        else
        {
            report += "✓ GameManagers exists\n";
            
            UpgradeSystem upgradeSystem = gameManagers.GetComponent<UpgradeSystem>();
            if (upgradeSystem == null)
            {
                report += "❌ UpgradeSystem component missing\n";
                allGood = false;
            }
            else
            {
                report += "✓ UpgradeSystem component attached\n";
                
                SerializedObject so = new SerializedObject(upgradeSystem);
                SerializedProperty upgradePoolProp = so.FindProperty("upgradePool");
                if (upgradePoolProp.arraySize == 0)
                {
                    report += "⚠️ UpgradeSystem has no upgrades assigned\n";
                    allGood = false;
                }
                else
                {
                    report += $"✓ UpgradeSystem has {upgradePoolProp.arraySize} upgrades\n";
                }
            }
            
            ExperienceSystem xpSystem = gameManagers.GetComponent<ExperienceSystem>();
            if (xpSystem == null)
            {
                report += "⚠️ ExperienceSystem component missing\n";
            }
            else
            {
                report += "✓ ExperienceSystem component attached\n";
            }
            
            PlayerStats stats = gameManagers.GetComponent<PlayerStats>();
            if (stats == null)
            {
                report += "⚠️ PlayerStats component missing\n";
            }
            else
            {
                report += "✓ PlayerStats component attached\n";
            }
        }
        
        GameObject itemButton = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/ItemButton.prefab");
        if (itemButton == null)
        {
            report += "❌ ItemButton prefab not found\n";
            allGood = false;
        }
        else
        {
            report += "✓ ItemButton prefab exists\n";
        }
        
        report += "\n";
        
        if (allGood)
        {
            report += "🎉 ALL SYSTEMS GO! Ready to test!";
            Debug.Log("<color=green>" + report + "</color>");
            EditorUtility.DisplayDialog("Validation Complete", report, "Awesome!");
        }
        else
        {
            report += "⚠️ Some issues found. Check report above.";
            Debug.LogWarning(report);
            EditorUtility.DisplayDialog("Validation Issues", report, "OK");
        }
    }
}
