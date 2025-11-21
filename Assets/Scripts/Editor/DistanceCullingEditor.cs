using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DistanceCullingSystem))]
public class DistanceCullingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        DistanceCullingSystem cullingSystem = (DistanceCullingSystem)target;
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Runtime Controls", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Refresh Registration", GUILayout.Height(30)))
        {
            cullingSystem.RefreshRegistration();
        }
        
        if (GUILayout.Button("Enable All Objects", GUILayout.Height(30)))
        {
            cullingSystem.EnableAll();
        }
        
        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox(
            "This system automatically culls distant objects at runtime.\n\n" +
            "• Objects beyond Culling Distance are disabled\n" +
            "• Objects within Enable Distance are re-enabled\n" +
            "• Update Interval controls how often distances are checked\n" +
            "• Enable 'Show Debug Info' to see stats in Game view", 
            MessageType.Info);
    }
}
