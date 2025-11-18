using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

[RequireComponent(typeof(CinemachineCamera))]
public class CinemachineDynamicDistance : MonoBehaviour
{
    [Header("Distance Settings")]
    [SerializeField] private float preRunDistance = 7f;
    [SerializeField] private float runDistance = 12f;
    [SerializeField] private float transitionDuration = 2f;
    [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Debug")]
    [SerializeField] private bool showDebug = false;
    
    private CinemachineCamera cinemachineCamera;
    private CinemachinePositionComposer positionComposer;
    private Coroutine transitionCoroutine;
    
    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        positionComposer = GetComponent<CinemachinePositionComposer>();
        
        if (positionComposer == null)
        {
            Debug.LogError("CinemachineDynamicDistance: CinemachinePositionComposer not found!");
            enabled = false;
            return;
        }
    }
    
    private void Start()
    {
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.AddListener(ZoomOutForRun);
            RunStateManager.Instance.OnRunEnded.AddListener(ZoomInForMenu);
            
            if (RunStateManager.Instance.IsInPreRunMenu)
            {
                SetDistanceImmediate(preRunDistance);
            }
            else
            {
                SetDistanceImmediate(runDistance);
            }
            
            if (showDebug)
            {
                Debug.Log($"<color=cyan>CinemachineDynamicDistance: Initialized with distance {positionComposer.CameraDistance}</color>");
            }
        }
        else
        {
            Debug.LogWarning("CinemachineDynamicDistance: RunStateManager not found!");
        }
    }
    
    private void ZoomOutForRun()
    {
        if (showDebug)
        {
            Debug.Log("<color=green>CinemachineDynamicDistance: Zooming OUT for run</color>");
        }
        TransitionToDistance(runDistance);
    }
    
    private void ZoomInForMenu()
    {
        if (showDebug)
        {
            Debug.Log("<color=cyan>CinemachineDynamicDistance: Zooming IN for menu</color>");
        }
        TransitionToDistance(preRunDistance);
    }
    
    private void SetDistanceImmediate(float distance)
    {
        if (positionComposer != null)
        {
            positionComposer.CameraDistance = distance;
            if (showDebug)
            {
                Debug.Log($"<color=yellow>CinemachineDynamicDistance: Set distance immediately to {distance}</color>");
            }
        }
    }
    
    private void TransitionToDistance(float targetDistance)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(TransitionCoroutine(targetDistance));
    }
    
    private IEnumerator TransitionCoroutine(float targetDistance)
    {
        float startDistance = positionComposer.CameraDistance;
        float elapsed = 0f;
        
        if (showDebug)
        {
            Debug.Log($"<color=cyan>CinemachineDynamicDistance: Transitioning from {startDistance} to {targetDistance} over {transitionDuration}s</color>");
        }
        
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            float curveValue = transitionCurve.Evaluate(t);
            
            float newDistance = Mathf.Lerp(startDistance, targetDistance, curveValue);
            positionComposer.CameraDistance = newDistance;
            
            yield return null;
        }
        
        positionComposer.CameraDistance = targetDistance;
        
        if (showDebug)
        {
            Debug.Log($"<color=green>CinemachineDynamicDistance: Transition complete! Distance = {targetDistance}</color>");
        }
        
        transitionCoroutine = null;
    }
    
    private void OnDestroy()
    {
        if (RunStateManager.Instance != null)
        {
            RunStateManager.Instance.OnRunStarted.RemoveListener(ZoomOutForRun);
            RunStateManager.Instance.OnRunEnded.RemoveListener(ZoomInForMenu);
        }
    }
    
    private void OnValidate()
    {
        if (Application.isPlaying && positionComposer != null)
        {
            if (RunStateManager.Instance != null && RunStateManager.Instance.IsInPreRunMenu)
            {
                positionComposer.CameraDistance = preRunDistance;
            }
            else
            {
                positionComposer.CameraDistance = runDistance;
            }
        }
    }
}
