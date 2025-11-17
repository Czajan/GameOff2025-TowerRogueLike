using UnityEngine;

public class JumpAnimationDebugger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    
    private void OnGUI()
    {
        if (animator == null || characterController == null) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 450, 400));
        GUILayout.Box("Jump Animation Debugger", GUILayout.Width(440));
        
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        bool isInJumpState = currentState.IsName("Jump");
        
        GUILayout.Label($"<b>Current State:</b> {(isInJumpState ? "<color=red>JUMP</color>" : "Other")}");
        GUILayout.Label($"<b>Normalized Time:</b> {currentState.normalizedTime:F3}");
        GUILayout.Label($"");
        
        GUILayout.Label($"<b>CharacterController.isGrounded:</b> {characterController.isGrounded}");
        GUILayout.Label($"<b>Animator IsGrounded Parameter:</b> {animator.GetBool("IsGrounded")}");
        GUILayout.Label($"<b>Speed Parameter:</b> {animator.GetFloat("Speed"):F2}");
        GUILayout.Label($"");
        
        AnimatorTransitionInfo transitionInfo = animator.GetAnimatorTransitionInfo(0);
        if (transitionInfo.fullPathHash != 0)
        {
            GUILayout.Label($"<color=yellow>IN TRANSITION</color>");
            GUILayout.Label($"Transition Progress: {transitionInfo.normalizedTime:F2}");
        }
        else if (isInJumpState)
        {
            GUILayout.Label($"<color=red>STUCK IN JUMP STATE!</color>");
            
            if (!characterController.isGrounded)
            {
                GUILayout.Label($"<color=orange>Reason: Still in air</color>");
            }
            else if (!animator.GetBool("IsGrounded"))
            {
                GUILayout.Label($"<color=red>ERROR: Grounded but animator param is FALSE!</color>");
            }
            else if (currentState.normalizedTime < 0.7f)
            {
                GUILayout.Label($"<color=orange>Reason: Exit time not reached (< 0.7)</color>");
            }
            else
            {
                GUILayout.Label($"<color=red>ERROR: Should transition but isn't!</color>");
                GUILayout.Label($"<color=red>Check Jump → Idle transition exists!</color>");
            }
        }
        else
        {
            GUILayout.Label($"<color=green>Normal operation</color>");
        }
        
        GUILayout.Label($"");
        GUILayout.Label($"<b>Next Transitions Available:</b>");
        AnimatorTransitionInfo[] nextTransitions = animator.GetNextAnimatorStateInfo(0).fullPathHash != 0 ? 
            new AnimatorTransitionInfo[] { animator.GetAnimatorTransitionInfo(0) } : 
            new AnimatorTransitionInfo[0];
        
        if (nextTransitions.Length == 0 && isInJumpState)
        {
            GUILayout.Label($"<color=red>NO TRANSITIONS FOUND FROM JUMP!</color>");
            GUILayout.Label($"<color=yellow>Action: Create Jump → Idle transition</color>");
        }
        
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
        {
            GUILayout.Label($"");
            GUILayout.Label($"<b>Current Animation Clip:</b>");
            GUILayout.Label($"  Name: {clipInfos[0].clip.name}");
            GUILayout.Label($"  Length: {clipInfos[0].clip.length:F2}s");
            GUILayout.Label($"  Loop: {clipInfos[0].clip.isLooping}");
            
            if (isInJumpState && clipInfos[0].clip.isLooping)
            {
                GUILayout.Label($"<color=red>WARNING: Jump animation is set to LOOP!</color>");
            }
        }
        
        GUILayout.EndArea();
    }
}
