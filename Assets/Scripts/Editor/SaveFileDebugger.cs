using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveFileDebugger : EditorWindow
{
    private string saveFilePath;
    private string saveFileContent = "";
    private Vector2 scrollPosition;
    
    [MenuItem("Tools/Save File Debugger")]
    public static void ShowWindow()
    {
        GetWindow<SaveFileDebugger>("Save File Debugger");
    }
    
    private void OnEnable()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        LoadSaveFile();
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Save File Debugger", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        GUILayout.Label($"Save File Path:", EditorStyles.label);
        EditorGUILayout.TextField(saveFilePath);
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Open Save File Location", GUILayout.Height(30)))
        {
            EditorUtility.RevealInFinder(saveFilePath);
        }
        
        GUILayout.Space(10);
        
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Refresh", GUILayout.Height(30)))
        {
            LoadSaveFile();
        }
        
        if (GUILayout.Button("Reset Save File", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Reset Save File", 
                "Are you sure you want to reset the save file? This will delete all progress!", 
                "Yes, Reset", "Cancel"))
            {
                ResetSaveFile();
            }
        }
        
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        if (File.Exists(saveFilePath))
        {
            GUILayout.Label("Save File Content:", EditorStyles.boldLabel);
            
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));
            EditorGUILayout.TextArea(saveFileContent, GUILayout.ExpandHeight(true));
            GUILayout.EndScrollView();
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Copy to Clipboard", GUILayout.Height(25)))
            {
                EditorGUIUtility.systemCopyBuffer = saveFileContent;
                Debug.Log("Save file content copied to clipboard!");
            }
        }
        else
        {
            GUILayout.Label("Save file does not exist yet.", EditorStyles.helpBox);
        }
        
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "This tool allows you to view and reset the save file.\n\n" +
            "• Refresh: Reload the save file content\n" +
            "• Reset: Delete all save data and create a fresh save\n" +
            "• Open Location: Open the folder containing the save file",
            MessageType.Info);
    }
    
    private void LoadSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                saveFileContent = File.ReadAllText(saveFilePath);
                Debug.Log($"Save file loaded from: {saveFilePath}");
            }
            catch (System.Exception e)
            {
                saveFileContent = $"Error loading save file: {e.Message}";
                Debug.LogError($"Failed to load save file: {e.Message}");
            }
        }
        else
        {
            saveFileContent = "Save file does not exist yet.";
        }
    }
    
    private void ResetSaveFile()
    {
        try
        {
            PersistentData newData = new PersistentData();
            string json = JsonUtility.ToJson(newData, true);
            File.WriteAllText(saveFilePath, json);
            saveFileContent = json;
            Debug.Log("<color=green>Save file reset successfully!</color>");
            EditorUtility.DisplayDialog("Success", "Save file has been reset!", "OK");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to reset save file: {e.Message}");
            EditorUtility.DisplayDialog("Error", $"Failed to reset save file: {e.Message}", "OK");
        }
    }
}
