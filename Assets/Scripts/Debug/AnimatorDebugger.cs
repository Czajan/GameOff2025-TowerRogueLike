using UnityEngine;

public class AnimatorDebugger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private bool showDebugInfo = true;
    
    private void OnGUI()
    {
        if (!showDebugInfo || animator == null) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 400, 300));
        GUILayout.Box("Animator Debug Info");
        
        GUILayout.Label($"Current State: {GetCurrentStateName()}");
        GUILayout.Label($"Speed Parameter: {animator.GetFloat("Speed"):F2}");
        GUILayout.Label($"IsGrounded Parameter: {animator.GetBool("IsGrounded")}");
        
        if (characterController != null)
        {
            GUILayout.Label($"CC IsGrounded: {characterController.isGrounded}");
            GUILayout.Label($"CC Velocity: {characterController.velocity}");
        }
        
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
        {
            GUILayout.Label($"Current Clip: {clipInfos[0].clip.name}");
            GUILayout.Label($"Clip Length: {clipInfos[0].clip.length:F2}s");
            GUILayout.Label($"Normalized Time: {GetNormalizedTime():F2}");
        }
        
        GUILayout.EndArea();
    }
    
    private string GetCurrentStateName()
    {
        if (animator == null) return "N/A";
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Idle") ? "Idle" :
               stateInfo.IsName("Walk") ? "Walk" :
               stateInfo.IsName("Run") ? "Run" :
               stateInfo.IsName("Jump") ? "Jump" :
               "Unknown";
    }
    
    private float GetNormalizedTime()
    {
        if (animator == null) return 0f;
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
