using UnityEngine;

public class GatePushZone : MonoBehaviour
{
    private Vector3 pushDirection;
    private float pushForce;
    
    public void Initialize(Vector3 direction, float force)
    {
        pushDirection = direction.normalized;
        pushForce = force;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            Vector3 pushVelocity = pushDirection * pushForce * Time.deltaTime;
            controller.Move(pushVelocity);
            
            Debug.Log($"<color=orange>Pushing player forward! (force={pushForce})</color>");
        }
    }
}
