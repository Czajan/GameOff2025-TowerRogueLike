using UnityEngine;

[RequireComponent(typeof(BaseGate))]
public class GateColliderSetup : MonoBehaviour
{
    [Header("Layer Setup")]
    [SerializeField] private bool setGateToGroundLayer = true;
    
    private void Awake()
    {
        if (setGateToGroundLayer)
        {
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (groundLayer != -1)
            {
                gameObject.layer = groundLayer;
                Debug.Log("<color=cyan>GateColliderSetup: BaseGate set to Ground layer for CharacterController collision</color>");
            }
            else
            {
                Debug.LogWarning("GateColliderSetup: Ground layer not found! Create a 'Ground' layer in Tags & Layers.");
            }
        }
    }
}
