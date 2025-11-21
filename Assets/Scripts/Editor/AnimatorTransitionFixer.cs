using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class AnimatorTransitionFixer : EditorWindow
{
    private AnimatorController animatorController;
    
    [MenuItem("Tools/Fix Jump Animation Transitions")]
    public static void ShowWindow()
    {
        GetWindow<AnimatorTransitionFixer>("Animator Transition Fixer");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Jump Animation Transition Fixer", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        animatorController = (AnimatorController)EditorGUILayout.ObjectField(
            "Animator Controller", 
            animatorController, 
            typeof(AnimatorController), 
            false);
        
        GUILayout.Space(10);
        
        if (animatorController == null)
        {
            EditorGUILayout.HelpBox("Assign your PlayerAnimatorController to fix jump transitions.", MessageType.Info);
        }
        
        GUI.enabled = animatorController != null;
        
        if (GUILayout.Button("Fix Jump Transitions", GUILayout.Height(40)))
        {
            FixJumpTransitions();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Show All Transitions Info", GUILayout.Height(30)))
        {
            ShowTransitionsInfo();
        }
        
        GUI.enabled = true;
    }
    
    private void FixJumpTransitions()
    {
        if (animatorController == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign an Animator Controller first!", "OK");
            return;
        }
        
        int fixedCount = 0;
        
        foreach (var layer in animatorController.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                string stateName = state.state.name.ToLower();
                
                if (stateName.Contains("jump") || stateName.Contains("inair") || stateName.Contains("fall"))
                {
                    Debug.Log($"<color=yellow>Found jump state: {state.state.name}</color>");
                    
                    foreach (var transition in state.state.transitions)
                    {
                        string targetName = transition.destinationState?.name ?? "Any State";
                        Debug.Log($"  Transition to: {targetName}");
                        
                        bool wasChanged = false;
                        
                        if (transition.hasExitTime)
                        {
                            transition.hasExitTime = false;
                            Debug.Log($"    ✓ Disabled Exit Time");
                            wasChanged = true;
                        }
                        
                        if (transition.duration > 0.15f)
                        {
                            transition.duration = 0.1f;
                            Debug.Log($"    ✓ Reduced transition duration to 0.1");
                            wasChanged = true;
                        }
                        
                        if (transition.interruptionSource != TransitionInterruptionSource.Source)
                        {
                            transition.interruptionSource = TransitionInterruptionSource.Source;
                            Debug.Log($"    ✓ Set interruption source to Current State");
                            wasChanged = true;
                        }
                        
                        transition.canTransitionToSelf = false;
                        
                        if (wasChanged)
                        {
                            fixedCount++;
                        }
                    }
                }
            }
        }
        
        if (fixedCount > 0)
        {
            EditorUtility.SetDirty(animatorController);
            AssetDatabase.SaveAssets();
            Debug.Log($"<color=green>✓ Fixed {fixedCount} jump transitions!</color>");
            EditorUtility.DisplayDialog("Success", $"Fixed {fixedCount} jump transitions!\n\nChanges saved.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Info", "No jump transitions found or no changes needed.", "OK");
        }
    }
    
    private void ShowTransitionsInfo()
    {
        if (animatorController == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign an Animator Controller first!", "OK");
            return;
        }
        
        Debug.Log("<color=cyan>======= ANIMATOR TRANSITIONS INFO =======</color>");
        
        foreach (var layer in animatorController.layers)
        {
            Debug.Log($"\n<color=yellow>Layer: {layer.name}</color>");
            
            foreach (var state in layer.stateMachine.states)
            {
                if (state.state.transitions.Length > 0)
                {
                    Debug.Log($"\n  State: <b>{state.state.name}</b>");
                    
                    foreach (var transition in state.state.transitions)
                    {
                        string targetName = transition.destinationState?.name ?? "Exit";
                        Debug.Log($"    → {targetName}");
                        Debug.Log($"      Has Exit Time: {transition.hasExitTime}");
                        Debug.Log($"      Duration: {transition.duration}");
                        Debug.Log($"      Conditions: {transition.conditions.Length}");
                        
                        foreach (var condition in transition.conditions)
                        {
                            Debug.Log($"        - {condition.parameter} {condition.mode} {condition.threshold}");
                        }
                    }
                }
            }
        }
    }
}
