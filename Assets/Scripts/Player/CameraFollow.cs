using UnityEngine;
using Unity.Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [Header("Cinemachine Setup")]
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followDistance = 12f;
    [SerializeField] private Vector2 screenPosition = new Vector2(0f, 0f);
    
    private CinemachineCamera virtualCamera;
    
    private void Start()
    {
        SetupCinemachineCamera();
    }
    
    private void SetupCinemachineCamera()
    {
        virtualCamera = GetComponent<CinemachineCamera>();
        
        if (virtualCamera == null)
        {
            Debug.LogWarning("CameraFollow requires a CinemachineCamera component. Please add one manually.");
            return;
        }
        
        if (followTarget != null)
        {
            virtualCamera.Target.TrackingTarget = followTarget;
        }
        
        ConfigurePositionComposer();
        ConfigureRotationComposer();
    }
    
    private void ConfigurePositionComposer()
    {
        CinemachinePositionComposer positionComposer = GetComponent<CinemachinePositionComposer>();
        
        if (positionComposer != null)
        {
            positionComposer.CameraDistance = followDistance;
            positionComposer.Composition.ScreenPosition = screenPosition;
        }
    }
    
    private void ConfigureRotationComposer()
    {
        CinemachineRotationComposer rotationComposer = GetComponent<CinemachineRotationComposer>();
        
        if (rotationComposer != null)
        {
            rotationComposer.Composition.ScreenPosition = screenPosition;
        }
    }
    
    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
        if (virtualCamera != null)
        {
            virtualCamera.Target.TrackingTarget = target;
        }
    }
}
