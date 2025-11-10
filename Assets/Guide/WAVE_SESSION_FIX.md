# Wave Session Continuation Fix

## Problem Identified

After completing wave 10 and the 60-second between-sessions timer expired:
- Console showed: `"Session 2 started (waves 11-20)"`
- But no enemies spawned
- Game got stuck at wave 10

### Root Cause

In `WaveSpawner.cs`, the `StartWaves()` method had flawed logic:

```csharp
// OLD BUGGY CODE
public void StartWaves()
{
    if (!isSpawning && !sessionComplete)  // ← Required sessionComplete to be false
    {
        sessionComplete = false;
        currentWaveNumber = 0;  // ← Also reset wave number to 0!
        // ... start waves
    }
    else if (sessionComplete)  // ← This blocked session 2!
    {
        Debug.Log("Already complete! Ignoring.");
    }
}
```

**The Flow:**
1. Wave 10 completes → `CompleteSession()` sets `sessionComplete = true`
2. Between-sessions timer expires
3. `RunStateManager.StartNextSession()` calls `WaveSpawner.Instance.StartWaves()`
4. **BUG:** Check fails because `sessionComplete == true`
5. StartWaves() exits early with "session already complete! Ignoring."
6. No waves spawn for session 2!

**Secondary Issue:**
- The code also reset `currentWaveNumber = 0`, which would restart from wave 1 instead of continuing from wave 11

## Solution Applied

### ✅ Fixed StartWaves() Logic

**Changes:**
1. Removed `&& !sessionComplete` check - not needed
2. Removed `currentWaveNumber = 0` - waves should continue counting
3. Simplified to single condition: `if (!isSpawning)`
4. Reset `sessionComplete = false` at start of new session
5. Improved debug log to show wave continuation

**NEW CODE:**
```csharp
public void StartWaves()
{
    if (!isSpawning)
    {
        sessionComplete = false;        // ✅ Reset for new session
        // currentWaveNumber NOT reset  // ✅ Continues from 11, 21, etc.
        wavesSpawned = 0;               // ✅ Reset per-session counter
        waveEnemies.Clear();
        waveEnemyCounts.Clear();
        completedWaves.Clear();
        StartCoroutine(WaveRoutine());
        Debug.Log($"StartWaves() called - continuing from wave {currentWaveNumber}");
    }
    else
    {
        Debug.Log("Waves already spawning! Ignoring.");
    }
}
```

## How It Works Now

### Session 1 (Waves 1-10)
```
StartRun()
  └─ StartNextSession() [Session 1]
      └─ StartWaves()
          ├─ currentWaveNumber starts at 0
          ├─ WaveRoutine() spawns waves 1-10
          └─ CompleteSession() → sessionComplete = true

CompleteSession()
  └─ RunStateManager.CompleteSession()
      └─ Sets state to BetweenSessions
      └─ Starts 60-second timer
```

### Between Sessions (60 seconds)
```
BetweenSessions State
  ├─ Timer counts down
  └─ Player can shop/build
```

### Session 2 (Waves 11-20)
```
Timer expires
  └─ StartNextSession() [Session 2]
      └─ StartWaves()
          ├─ sessionComplete = false     ✅ Reset!
          ├─ currentWaveNumber = 10      ✅ Continues from 10!
          ├─ WaveRoutine() increments to wave 11
          └─ Spawns waves 11-20          ✅ Works!
```

## Testing

### Test Full Session Flow

1. **Start run** (open gate)
2. **Complete waves 1-10**
   - ✅ Console: "SESSION 1 COMPLETE"
   - ✅ BetweenSessionsPanel appears
   - ✅ Timer shows 01:00

3. **Wait 60 seconds** (or let timer expire)
   - ✅ Console: "SESSION 2 STARTED (Waves 11-20)"
   - ✅ Console: "StartWaves() called - continuing from wave 10"
   - ✅ Wave 11 spawns immediately
   - ✅ Enemies appear

4. **Continue through waves 11-20**
   - ✅ Waves spawn every 30 seconds
   - ✅ Wave counter shows 11, 12, 13...
   - ✅ After wave 20 completes → "SESSION 2 COMPLETE"

5. **Session 3 and beyond**
   - ✅ Session 3 starts waves 21-30
   - ✅ Session 4 starts waves 31-40
   - ✅ Pattern continues indefinitely

## Files Modified

- `/Assets/Scripts/Systems/WaveSpawner.cs`
  - Fixed `StartWaves()` method
  - Removed `sessionComplete` blocking check
  - Removed `currentWaveNumber = 0` reset
  - Improved logging

## Architecture Notes

**Wave Number Tracking:**
- `currentWaveNumber` - Total waves spawned across ALL sessions (1, 2, 3... 11, 12... 21, 22...)
- `wavesSpawned` - Waves spawned THIS session (resets to 0 each session)
- `wavesPerSession` - Fixed at 10 waves per session

**Session Flow:**
```
Session 1: currentWaveNumber goes from 0 → 10
           wavesSpawned goes from 0 → 10

Session 2: currentWaveNumber continues from 10 → 20
           wavesSpawned resets 0 → 10

Session 3: currentWaveNumber continues from 20 → 30
           wavesSpawned resets 0 → 10
```

This ensures:
- Difficulty keeps scaling (wave 11 is harder than wave 1)
- Each session spawns exactly 10 waves
- Progression is maintained across sessions

---

**Result:** Sessions now properly continue! Waves 11-20 spawn after session 1, waves 21-30 after session 2, etc.
