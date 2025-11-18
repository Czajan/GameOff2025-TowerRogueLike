using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GateAudioController : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip pushStartSound;
    [SerializeField] private AudioClip pushCompleteSound;
    [SerializeField] private AudioClip gateOpenSound;
    [SerializeField] private AudioClip gateCloseSound;
    
    [Header("Settings")]
    [SerializeField] private float pushStartVolume = 1f;
    [SerializeField] private float pushCompleteVolume = 1f;
    [SerializeField] private float gateOpenVolume = 1f;
    [SerializeField] private float gateCloseVolume = 1f;
    
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayPushStartSound()
    {
        if (pushStartSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pushStartSound, pushStartVolume);
            Debug.Log("<color=cyan>Playing push start sound</color>");
        }
    }
    
    public void PlayPushCompleteSound()
    {
        if (pushCompleteSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pushCompleteSound, pushCompleteVolume);
            Debug.Log("<color=cyan>Playing push complete sound</color>");
        }
    }
    
    public void PlayGateOpenSound()
    {
        if (gateOpenSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gateOpenSound, gateOpenVolume);
            Debug.Log("<color=cyan>Playing gate open sound</color>");
        }
    }
    
    public void PlayGateCloseSound()
    {
        if (gateCloseSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gateCloseSound, gateCloseVolume);
            Debug.Log("<color=cyan>Playing gate close sound</color>");
        }
    }
}
