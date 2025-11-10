# Gate Auto-Close Setup (Using Existing BaseTrigger)

## ✅ Update: Using Existing BaseTrigger!

Instead of creating a new trigger, we've updated the **existing** `/Base/BaseTrigger` to handle gate auto-closing. Much cleaner!

---

## Purpose
Once the player exits the base zone after starting a run, the gate automatically closes behind them to prevent returning to the pre-run menu during an active run.

---

## How It Works

1. **Player presses E** → Gate opens (moves up)
2. **Player walks out of base zone** → BaseTrigger detects exit (OnTriggerExit)
3. **Gate auto-closes** (after 1 second delay) → Prevents return to base
4. **Run continues** → Player must complete or die to return to pre-run menu
5. **Run ends** → Gate opens again for next run, trigger resets

---

## ✅ Script Already Updated!

I've updated `/Assets/Scripts/Systems/BaseTrigger.cs` to include:
- Auto-close gate when player exits base zone
- Only works during active runs (not in pre-run menu)
- Configurable delay before closing
- Can be toggled on/off
- Resets when run ends

---

## ⚙️ Setup Instructions (1 minute)

### Step 1: Configure BaseTrigger

1. **Select `/Base/BaseTrigger` in Hierarchy**

2. **In Inspector, configure:**
   ```
   ┌─ Base Trigger (Script) ──────────────────┐
   │                                           │
   │ Detection Settings                        │
   │   Base Direction: (-1, 0, -1)            │
   │                                           │
   │ Gate Auto Close                           │
   │   Gate: (drag /Base/BaseGate here)       │
   │   Auto Close Delay: 1                    │
   │   Enable Auto Close: ✅                   │
   └───────────────────────────────────────────┘
   ```

3. **Drag `/Base/BaseGate`** from Hierarchy to the **Gate** field

4. **Verify BoxCollider settings:**
   - `Is Trigger: ✅` (should already be checked)
   - Adjust **Size** to cover the base exit area if needed

### Step 2: Save Scene

Press **Ctrl+S** (or **Cmd+S** on Mac)

---

## 📐 Trigger Positioning

The `BaseTrigger` should be positioned to detect when players **exit the base area**:

```
     ┌─────────────────────┐
     │   Base (Safe Zone)  │
     │   - NPCs            │
     │   - Pre-run menu    │
     └─────────┬───────────┘
               │
       ┌───────▼────────┐
       │ █████████████  │  ← Gate (opens/closes)
       └────────────────┘
               │
       ┌───────▼────────┐
       │  [TRIGGER BOX] │  ← BaseTrigger zone
       │  (detects exit)│
       └────────────────┘
               │
               ↓
        (Combat Zone)
```

**Current setup:** BaseTrigger should already be positioned to cover the base exit.

**If needed:** Adjust Transform position and BoxCollider size to ensure it covers where players exit toward the combat zone.

---

## 🧪 Testing

### Test 1: Gate Auto-Close
1. **Enter Play Mode**
2. Walk to gate, press **E**
3. **Expected:** Gate opens (moves up)
4. Walk **out of the base trigger zone** (toward combat area)
5. **Expected:** 
   - Console: `Player exited base zone - closing gate in 1s...`
   - After 1 second...
   - Console: `Gate auto-closed! Player cannot return to base during run.`
   - Gate closes (moves down)
6. Try walking back toward base
7. **Expected:** Gate collider blocks you ✅

### Test 2: Run End Reset
1. Continue from above
2. Die or let waves complete the run
3. **Expected:**
   - Run ends
   - Gate opens again
   - Trigger resets (`hasAutoClosedGate = false`)
   - Ready for next run

### Test 3: Doesn't Trigger in Pre-Run
1. **Enter Play Mode**
2. Walk around base **without pressing E**
3. Walk in and out of trigger
4. **Expected:** Nothing happens (auto-close only works during active runs)

---

## ⚙️ Configuration Options

### Auto Close Delay
```csharp
Auto Close Delay: 1  (seconds)
```
- **0.5s** - Very fast, gate closes quickly after exit
- **1.0s** - Default, gives player time to clear the zone
- **2.0s** - Slower, more forgiving

### Enable Auto Close
```csharp
Enable Auto Close: ✅
```
- **Checked (✅):** Gate auto-closes when player exits base
- **Unchecked (❌):** Old behavior, no auto-close (for testing/debugging)

### Trigger Size
Adjust **BoxCollider → Size** to:
- Cover the entire base exit area
- Ensure players trigger exit when leaving toward combat
- Make it large enough so players can't walk around it

---

## 🎨 Advanced: Trigger Visualization

In **Scene View**, select `/Base/BaseTrigger` to see the green wireframe box showing the trigger zone.

**Good positioning:**
- Trigger encompasses the gate and some area around it
- Players **enter** trigger when in base/safe zone
- Players **exit** trigger when moving toward combat

---

## 🔧 How The Updated Code Works

### New Features Added:

**1. Gate Reference & Settings:**
```csharp
[SerializeField] private BaseGate gate;
[SerializeField] private float autoCloseDelay = 1f;
[SerializeField] private bool enableAutoClose = true;
```

**2. OnTriggerExit Detection:**
```csharp
private void OnTriggerExit(Collider other)
{
    // When player exits base zone during active run
    // → Close gate after delay
}
```

**3. Auto-Close Logic:**
```csharp
private void CloseGateAfterDelay()
{
    gate.CloseGate();  // Closes gate, enables collider
}
```

**4. Run State Integration:**
```csharp
private void OnRunStarted() { hasAutoClosedGate = false; }
private void OnRunEnded() { hasAutoClosedGate = false; }
```

---

## 🗑️ Cleanup

**Removed:**
- `/Assets/Scripts/Systems/GatePassTrigger.cs` (not needed - using BaseTrigger instead)

**Note:** The earlier guide created `GatePassTrigger.cs`, but we're **not using it**. If you created it or the GameObject, you can delete them. We're using the existing `BaseTrigger` instead!

---

## 📋 Old BaseTrigger Features Still Work

The original `BaseTrigger` functionality is **preserved**:
- ✅ Direction-based base entry detection
- ✅ Enable/disable based on game state
- ✅ Session completion handling
- ✅ All original events and listeners

**Added new features:**
- ✅ Gate auto-close on zone exit
- ✅ RunStateManager integration
- ✅ Configurable delay and toggle

---

## 🎯 Summary

✅ **Updated:** `/Assets/Scripts/Systems/BaseTrigger.cs`  
⚠️ **Setup Required:**
1. Select `/Base/BaseTrigger` in Hierarchy
2. Drag `/Base/BaseGate` to **Gate** field in Inspector
3. Verify **Enable Auto Close = ✅**
4. Set **Auto Close Delay = 1**
5. Save scene

**Result:** Gate auto-closes when player exits base zone, preventing retreat during runs! 🚪✨

**Benefits over new trigger:**
- ✅ Reuses existing GameObject
- ✅ Already positioned correctly
- ✅ Preserves old functionality
- ✅ Cleaner hierarchy
- ✅ Less setup required

