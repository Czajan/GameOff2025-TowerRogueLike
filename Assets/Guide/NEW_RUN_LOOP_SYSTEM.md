# New Run Loop System - Overview

## 🎮 **New Game Flow**

The game loop has been restructured into three distinct states:

### **1. Pre-Run Menu (Base)**
- Player spawns in base at the start
- **No timer forcing them out**
- Can spend **Essence** (meta-currency from previous runs) on permanent upgrades
- UI shows: "Spend Essence on Upgrades"
- UI shows: "Approach the gate and press [E] to start your run!"
- Player walks to the **BaseGate** and presses **E** to start the run

### **2. Wave Session**
- 10 waves spawn (overlapping every 30 seconds)
- Player fights enemies, earns Gold and Essence
- After all 10 waves are cleared → Session Complete

### **3. Between Sessions**
- **60-second timer** to spend **Gold** on obstacles/defenses
- UI shows countdown: "Next Session: 01:00"
- UI shows: "Spend Gold on Obstacles!"
- After 60 seconds → Next session starts automatically (waves 11-20, 21-30, etc.)

---

## 📁 **New Files Created**

### **1. `/Assets/Scripts/Systems/RunStateManager.cs`**
**Purpose:** Manages the three run states and transitions between them

**Key States:**
```csharp
public enum RunState
{
    PreRunMenu,      // In base, before run starts
    WaveSession,     // Fighting 10 waves
    BetweenSessions  // 60-second timer between sessions
}
```

**Key Methods:**
- `StartRun()` - Called when player clicks gate
- `StartNextSession()` - Starts a 10-wave session
- `CompleteSession()` - Called when 10 waves cleared
- `EndRun(bool victory)` - Ends the run (victory or defeat)

**Key Properties:**
- `CurrentState` - Current run state
- `CurrentSessionNumber` - Which session (1, 2, 3...)
- `BetweenSessionsTimer` - Countdown for next session
- `IsInPreRunMenu`, `IsInWaveSession`, `IsInBetweenSessions` - State checks

**Events:**
- `OnRunStarted` - Run begins (gate clicked)
- `OnSessionStarted` - New 10-wave session starts
- `OnSessionCompleted` - 10 waves cleared
- `OnBetweenSessionsTimerUpdate(float)` - Timer countdown
- `OnRunEnded` - Run finished

---

### **2. `/Assets/Scripts/Systems/BaseGate.cs` (UPDATED)**
**Purpose:** Interactive gate that starts the run

**New Features:**
- **Player Interaction:** Detects player proximity
- **Prompt Display:** Shows "Press [E] to Start Run" when near
- **Starts Run:** Calls `RunStateManager.StartRun()` on key press
- **Opens/Closes:** Opens when run starts, closes when run ends

**Inspector Settings:**
- `Interaction Range: 3` - How close player needs to be
- `Interaction Key: E` - Key to press
- `Prompt Text` - TextMeshProUGUI reference for prompt

**Integration:**
- Subscribes to `RunStateManager.OnRunStarted` → Opens gate
- Subscribes to `RunStateManager.OnRunEnded` → Closes gate

---

### **3. `/Assets/Scripts/UI/PreRunMenuUI.cs`**
**Purpose:** UI panel shown in pre-run menu state

**Features:**
- Shows current **Essence** amount
- Shows instructions: "Spend Essence on Upgrades"
- Shows instructions: "Approach the gate and press [E] to start your run!"
- Hides when run starts
- Shows again when run ends

**Inspector Setup:**
```
Panel: GameObject (entire UI panel)
Essence Text: TextMeshProUGUI
Instruction Text: TextMeshProUGUI
```

---

### **4. `/Assets/Scripts/UI/BetweenSessionsUI.cs`**
**Purpose:** UI panel shown during between-sessions countdown

**Features:**
- Shows **60-second countdown** timer
- Shows: "Spend Gold on Obstacles!"
- Shows: "Next wave session starting soon..."
- Only visible during between-sessions state

**Inspector Setup:**
```
Panel: GameObject
Timer Text: TextMeshProUGUI
Instruction Text: TextMeshProUGUI
```

---

## 🔄 **Updated Files**

### **1. `/Assets/Scripts/Systems/WaveSpawner.cs`**
**Changes:**
- `autoStartWaves = false` (default is now false)
- Calls `RunStateManager.CompleteSession()` after 10 waves
- No longer auto-starts on scene load

### **2. `/Assets/Scripts/Systems/WaveController.cs`**
**Changes:**
- Subscribes to `RunStateManager.OnSessionStarted` instead of `GameProgressionManager.OnExitedBase`
- Starts waves when session begins (not when exiting base)

### **3. `/Assets/Scripts/Systems/GameProgressionManager.cs`**
**Changes:**
- Removed: `baseTimerDuration`, `currentBaseTimer`
- Removed: `OnBaseTimerUpdate` event
- Removed: `ForceStartWave()` method
- Simplified: `Update()` no longer counts down timer
- Simplified: `EnterBase()` and `ExitBase()` no longer manage timers

### **4. `/Assets/Scripts/Systems/TimerDisplay.cs`**
**Changes:**
- Now subscribes to `RunStateManager` instead of `GameProgressionManager`
- Shows between-sessions timer (60 seconds)
- Shows "Next Session: MM:SS" instead of "Next Wave: MM:SS"

---

## 🎯 **Gameplay Flow Example**

### **Step-by-Step Run:**

**1. Game Starts**
```
State: PreRunMenu
Player: In base, gate closed
UI: "Spend Essence on Upgrades"
UI: "Approach the gate and press [E] to start your run!"
Essence Display: Shows lifetime Essence
```

**2. Player Approaches Gate**
```
Prompt appears: "Press [E] to Start Run"
```

**3. Player Presses E**
```
RunStateManager.StartRun() called
Gate opens
State changes: PreRunMenu → WaveSession
Session 1 starts (Waves 1-10)
Pre-run menu UI hides
```

**4. Waves 1-10 Spawn**
```
Wave 1 spawns at 0:00
Wave 2 spawns at 0:30
Wave 3 spawns at 1:00
...
Wave 10 spawns at 4:30
(Waves overlap - multiple active simultaneously)
```

**5. Player Clears All 10 Waves**
```
WaveSpawner.CompleteSession() called
RunStateManager.CompleteSession() called
State changes: WaveSession → BetweenSessions
Timer starts: 60 seconds
```

**6. Between Sessions (60 seconds)**
```
State: BetweenSessions
UI: "Spend Gold on Obstacles!"
Timer: "Next Session: 01:00" → "00:59" → ... → "00:00"
Player can place obstacles with Gold earned
```

**7. Timer Reaches 0**
```
RunStateManager.StartNextSession() called
State changes: BetweenSessions → WaveSession
Session 2 starts (Waves 11-20)
Timer UI hides
```

**8. Repeat**
```
Sessions 3, 4, 5... continue
Each session: 10 waves → 60-second break → 10 waves → ...
```

**9. Run Ends (Player Defeated or Quits)**
```
RunStateManager.EndRun(false) called
State changes: → PreRunMenu
Gate closes
Essence awarded to lifetime total
Pre-run menu UI shows
Player back at step 1
```

---

## ⚙️ **Configuration**

### **Run State Manager**
```
Between Sessions Duration: 60 seconds (configurable)
```

### **Wave Spawner**
```
Waves Per Session: 10
Time Between Waves: 30 seconds
Auto Start Waves: false
```

### **Base Gate**
```
Interaction Range: 3 meters
Interaction Key: E
Open Height: 5 units
Animation Speed: 2 units/second
```

---

## 🧪 **Testing the New System**

### **Test 1: Pre-Run Menu**
1. Enter Play Mode
2. **Expected:** Player in base, gate closed
3. **Expected:** UI shows "Approach the gate and press [E]"
4. **Expected:** Essence display shows lifetime Essence
5. Walk to gate
6. **Expected:** Prompt appears "Press [E] to Start Run"
7. Press E
8. **Expected:** Gate opens, run starts, UI hides

### **Test 2: First Session**
1. Start run (press E at gate)
2. **Expected:** Wave 1 spawns immediately
3. **Expected:** Console: "=== SESSION 1 STARTED (Waves 1-10) ==="
4. Wait 30 seconds
5. **Expected:** Wave 2 spawns (Wave 1 may still be active)
6. Kill all enemies from Waves 1-10
7. **Expected:** Console: "=== WAVE SESSION COMPLETE ==="
8. **Expected:** Between-sessions UI appears
9. **Expected:** Timer shows "01:00"

### **Test 3: Between Sessions**
1. Complete Session 1
2. **Expected:** Timer counts down: 01:00 → 00:59 → ...
3. **Expected:** UI: "Spend Gold on Obstacles!"
4. Wait for timer to reach 00:00
5. **Expected:** Console: "=== SESSION 2 STARTED (Waves 11-20) ==="
6. **Expected:** Timer UI hides
7. **Expected:** Wave 11 spawns

### **Test 4: Multiple Sessions**
1. Complete Sessions 1, 2, 3
2. **Expected:** Session numbers increase: 1, 2, 3, 4...
3. **Expected:** Wave numbers: 1-10, 11-20, 21-30, 31-40...
4. **Expected:** Each session ends with 60-second break

---

## 🔍 **Debug Properties**

### **RunStateManager**
```csharp
RunStateManager.Instance.CurrentState          // PreRunMenu, WaveSession, or BetweenSessions
RunStateManager.Instance.CurrentSessionNumber  // 1, 2, 3...
RunStateManager.Instance.BetweenSessionsTimer  // 60 → 0
RunStateManager.Instance.IsRunActive           // true during run
RunStateManager.Instance.IsInPreRunMenu        // true before run starts
```

### **Console Logging**
```
Press [E]:
  → <color=green>Player clicked gate - Run starting!</color>
  → <color=cyan>=== RUN STARTED ===</color>
  → <color=green>=== SESSION 1 STARTED (Waves 1-10) ===</color>

Session Complete:
  → <color=cyan>=== WAVE SESSION COMPLETE ===</color>
  → <color=yellow>=== SESSION 1 COMPLETE ===</color>
  → <color=yellow>You have 60 seconds to spend Gold on obstacles!</color>

Timer Expires:
  → <color=yellow>Between-sessions timer expired! Starting next session...</color>
  → <color=green>=== SESSION 2 STARTED (Waves 11-20) ===</color>
```

---

## 📋 **Summary of Changes**

### **What's Different:**

**Before:**
- Base had a timer forcing player out (45 seconds)
- Waves auto-started when timer hit 0
- Player had no control over when run started
- Return to base between each 10-wave session

**After:**
- ✅ Base is a **pre-run menu** with no timer
- ✅ Player **clicks gate (E key)** to start run
- ✅ Run consists of **multiple 10-wave sessions**
- ✅ **60-second breaks** between sessions to spend Gold
- ✅ **No return to base** until run ends (defeat or quit)
- ✅ **Clear state management** with RunStateManager

### **Benefits:**
- Player has full control over when to start
- Can spend Essence at own pace before starting
- Clear separation: Essence (pre-run) vs Gold (during run)
- Between-session breaks provide strategic moments
- Run feels like a cohesive experience (not returning to base every 10 waves)

---

## 🎨 **UI Setup Requirements**

### **Scene Setup Needed:**

1. **Add RunStateManager to scene**
   - Create empty GameObject: "RunStateManager"
   - Add `RunStateManager` component
   - Set: Between Sessions Duration = 60

2. **Update BaseGate**
   - Add `Prompt Text` reference (TextMeshProUGUI)
   - Set: Interaction Range = 3
   - Set: Interaction Key = E

3. **Create Pre-Run Menu UI**
   - Create UI Panel: "PreRunMenuPanel"
   - Add TextMeshProUGUI: "EssenceText"
   - Add TextMeshProUGUI: "InstructionText"
   - Add `PreRunMenuUI` component to panel
   - Link references

4. **Create Between-Sessions UI**
   - Create UI Panel: "BetweenSessionsPanel"
   - Add TextMeshProUGUI: "TimerText"
   - Add TextMeshProUGUI: "InstructionText"
   - Add `BetweenSessionsUI` component to panel
   - Link references

5. **Update WaveSpawner prefab/instance**
   - Set: Auto Start Waves = false

---

The new system is now ready! The base serves as a pre-run menu where players prepare with Essence, then click the gate to start their run. Sessions flow seamlessly with 60-second breaks to spend Gold between each set of 10 waves. 🎮
