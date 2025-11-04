using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterGrounder : MonoBehaviour
{
    [Header("Auto-Ground on Start")]
    [SerializeField] private bool autoGroundOnStart = true;
    [Tooltip("If true, ensures character is positioned so capsule bottom is at Y=0")]
    [SerializeField] private bool ensureGroundLevel = true;
    [SerializeField] private float groundOffset = -0.01f;
    
    private CharacterController characterController;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    private void Start()
    {
        if (autoGroundOnStart)
        {
            GroundCharacter();
        }
    }
    
    [ContextMenu("Ground Character")]
    public void GroundCharacter()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
            
        if (characterController == null)
            return;
        
        if (ensureGroundLevel)
        {
            float capsuleBottom = characterController.center.y - (characterController.height / 2f);
            Vector3 pos = transform.position;
            pos.y = -capsuleBottom + groundOffset;
            transform.position = pos;
            
            characterController.Move(Vector3.down * 0.01f);
            
            Debug.Log($"{gameObject.name} grounded at Y={pos.y} (center={characterController.center.y}, height={characterController.height}, bottom offset={capsuleBottom}, isGrounded={characterController.isGrounded})");
        }
    }
}
