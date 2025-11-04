using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VisualModelAligner : MonoBehaviour
{
    [Header("Model Alignment")]
    [SerializeField] private Transform visualModel;
    [SerializeField] private bool alignOnAwake = true;
    [SerializeField] private float visualModelHeight = 1f;
    
    private void Awake()
    {
        if (alignOnAwake)
        {
            AlignVisualModel();
        }
    }
    
    [ContextMenu("Align Visual Model to Capsule Bottom")]
    public void AlignVisualModel()
    {
        if (visualModel == null)
        {
            visualModel = transform.Find("Model");
        }
        
        if (visualModel == null)
        {
            Debug.LogWarning($"{gameObject.name}: No visual model found to align!");
            return;
        }
        
        CharacterController controller = GetComponent<CharacterController>();
        if (controller == null)
            return;
        
        float capsuleBottom = controller.center.y - (controller.height / 2f);
        
        float visualBottom = capsuleBottom + (visualModelHeight / 2f);
        
        Vector3 modelPos = visualModel.localPosition;
        modelPos.y = visualBottom;
        visualModel.localPosition = modelPos;
        
        Debug.Log($"{gameObject.name}: Aligned visual model to Y={visualBottom} (capsule bottom={capsuleBottom}, visual height={visualModelHeight})");
    }
}
