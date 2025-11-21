using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class CharacterSetupFixer : EditorWindow
{
    private GameObject targetCharacter;
    
    [MenuItem("Tools/Fix Character Ground Position")]
    public static void ShowWindow()
    {
        GetWindow<CharacterSetupFixer>("Character Setup Fixer");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Character Ground Position Fixer", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "This tool fixes floating characters by adjusting the Model child position.\n\n" +
            "Select your Player GameObject and click Fix.", 
            MessageType.Info);
        
        GUILayout.Space(10);
        
        targetCharacter = (GameObject)EditorGUILayout.ObjectField(
            "Character GameObject", 
            targetCharacter, 
            typeof(GameObject), 
            true);
        
        GUILayout.Space(10);
        
        GUI.enabled = targetCharacter != null;
        
        if (GUILayout.Button("Analyze Character", GUILayout.Height(30)))
        {
            AnalyzeCharacter();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("Fix Ground Position", GUILayout.Height(40)))
        {
            FixGroundPosition();
        }
        
        GUI.enabled = true;
        
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "Quick Settings:\n" +
            "• Move Speed: 3.5 (walk feel)\n" +
            "• Sprint Speed: 5.25 (3.5 × 1.5)\n" +
            "• Gravity: -20 (snappy jumps)", 
            MessageType.None);
    }
    
    private void AnalyzeCharacter()
    {
        if (targetCharacter == null) return;
        
        CharacterController cc = targetCharacter.GetComponent<CharacterController>();
        
        if (cc == null)
        {
            EditorUtility.DisplayDialog("Error", "No CharacterController found on the GameObject!", "OK");
            return;
        }
        
        Debug.Log("<color=cyan>===== CHARACTER ANALYSIS =====</color>");
        Debug.Log($"GameObject: {targetCharacter.name}");
        Debug.Log($"Position Y: {targetCharacter.transform.position.y}");
        Debug.Log($"CharacterController Height: {cc.height}");
        Debug.Log($"CharacterController Center Y: {cc.center.y}");
        Debug.Log($"CharacterController Radius: {cc.radius}");
        
        float capsuleBottom = targetCharacter.transform.position.y + cc.center.y - (cc.height / 2f);
        Debug.Log($"<color=yellow>Capsule Bottom Position: {capsuleBottom:F3}</color>");
        
        if (Mathf.Abs(capsuleBottom) > 0.01f)
        {
            Debug.Log($"<color=red>⚠ Character is {capsuleBottom:F3} units off the ground!</color>");
        }
        else
        {
            Debug.Log($"<color=green>✓ Character is properly positioned on ground</color>");
        }
        
        Transform modelChild = targetCharacter.transform.Find("Model");
        if (modelChild != null)
        {
            Debug.Log($"Model Child Position Y: {modelChild.localPosition.y}");
            
            if (Mathf.Abs(modelChild.localPosition.y) > 0.01f)
            {
                Debug.Log($"<color=yellow>⚠ Model child has Y offset: {modelChild.localPosition.y:F3}</color>");
            }
        }
        
        PlayerController pc = targetCharacter.GetComponent<PlayerController>();
        if (pc != null)
        {
            Debug.Log($"PlayerController found with MoveSpeed property available");
        }
    }
    
    private void FixGroundPosition()
    {
        if (targetCharacter == null) return;
        
        CharacterController cc = targetCharacter.GetComponent<CharacterController>();
        
        if (cc == null)
        {
            EditorUtility.DisplayDialog("Error", "No CharacterController found on the GameObject!", "OK");
            return;
        }
        
        Undo.RecordObject(targetCharacter.transform, "Fix Character Ground Position");
        
        float expectedY = cc.height / 2f;
        Vector3 currentPos = targetCharacter.transform.position;
        
        targetCharacter.transform.position = new Vector3(currentPos.x, expectedY, currentPos.z);
        
        Transform modelChild = targetCharacter.transform.Find("Model");
        if (modelChild != null)
        {
            Undo.RecordObject(modelChild, "Reset Model Position");
            modelChild.localPosition = Vector3.zero;
        }
        
        float capsuleBottom = targetCharacter.transform.position.y + cc.center.y - (cc.height / 2f);
        
        Debug.Log($"<color=green>✓ Character fixed! Capsule bottom at Y = {capsuleBottom:F3}</color>");
        
        EditorUtility.SetDirty(targetCharacter);
        
        if (PrefabUtility.IsPartOfPrefabInstance(targetCharacter))
        {
            bool applyToPrefab = EditorUtility.DisplayDialog(
                "Apply to Prefab?",
                "Do you want to apply these changes to the prefab?",
                "Yes, Apply",
                "No, Scene Only");
            
            if (applyToPrefab)
            {
                PrefabUtility.ApplyPrefabInstance(targetCharacter, InteractionMode.UserAction);
                Debug.Log("<color=green>✓ Changes applied to prefab!</color>");
            }
        }
        
        EditorUtility.DisplayDialog("Success", 
            $"Character position fixed!\n\n" +
            $"Character Y: {targetCharacter.transform.position.y:F2}\n" +
            $"Capsule Bottom: {capsuleBottom:F3}\n\n" +
            "The character should now be on the ground.", "OK");
    }
}
