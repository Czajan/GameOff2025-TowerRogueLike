using UnityEngine;

public class WaveEnemy : MonoBehaviour
{
    public int WaveNumber { get; private set; }
    
    public void SetWaveNumber(int waveNumber)
    {
        WaveNumber = waveNumber;
    }
}
