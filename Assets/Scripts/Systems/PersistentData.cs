using System;
using UnityEngine;

[Serializable]
public class PersistentData
{
    public int essence = 0;
    
    public int moveSpeedLevel = 0;
    public int maxHealthLevel = 0;
    public int damageLevel = 0;
    public int critChanceLevel = 0;
    public int critDamageLevel = 0;
    public int attackRangeLevel = 0;
    
    public int totalRunsCompleted = 0;
    public int totalRunsFailed = 0;
    public int totalEnemiesKilled = 0;
    public int highestWaveReached = 0;
    
    public PersistentData()
    {
        essence = 0;
        moveSpeedLevel = 0;
        maxHealthLevel = 0;
        damageLevel = 0;
        critChanceLevel = 0;
        critDamageLevel = 0;
        attackRangeLevel = 0;
        totalRunsCompleted = 0;
        totalRunsFailed = 0;
        totalEnemiesKilled = 0;
        highestWaveReached = 0;
    }
}
