# Setup Guide: New Run Loop System

This guide provides step-by-step instructions to set up the new run loop system in your scene.

---

## Prerequisites

✅ All new scripts created:
- `/Assets/Scripts/Systems/RunStateManager.cs`
- `/Assets/Scripts/UI/PreRunMenuUI.cs`
- `/Assets/Scripts/UI/BetweenSessionsUI.cs`

✅ All scripts updated:
- `/Assets/Scripts/Systems/BaseGate.cs`
- `/Assets/Scripts/Systems/WaveSpawner.cs`
- `/Assets/Scripts/Systems/WaveController.cs`
- `/Assets/Scripts/Systems/GameProgressionManager.cs`
- `/Assets/Scripts/Systems/TimerDisplay.cs`

✅ Scene loaded: `MainScene.unity` (or your main gameplay scene)

---

## Step 1: Add RunStateManager to Scene

### 1.1 Create GameObject
1. In **Hierarchy** window, right-click → **Create Empty**
2. Rename to: `RunStateManager`
3. **Reset Transform** to (0, 0, 0) — Right-click Transform component → Reset

### 1.2 Add RunStateManager Component
1. Select `RunStateManager` GameObject
2. In **Inspector**, click **Add Component**
3. Type: `RunStateManager`
4. Click to add the component

### 1.3 Configure Settings
In the **Inspector**, configure the `RunStateManager` component:

```
┌─ Run State Manager (Script) ──────────────────┐
│                                                │
│ State Configuration                           │
│   Between Sessions Duration:  60              │
│                                                │
│ Current State                                  │
│   Current State:  PreRunMenu                   │
│   Current Session Number:  0                   │
│   Between Sessions Timer:  0                   │
│                                                │
│ Events                                         │
│   ▼ On Run Started ()                          │
│   ▼ On Session Started ()                      │
│   ▼ On Session Completed ()                    │
│   ▼ On Between Sessions Timer Update (Single)  │
│   ▼ On Run Ended ()                            │
└────────────────────────────────────────────────┘
```

**Key Settings:**
- `Between Sessions Duration`: **60** (60 seconds between wave sessions)
- Leave events empty (they will be subscribed to by UI components)

### 1.4 Optional: Organize Hierarchy
1. If you have a `GameManagers` or `Systems` parent object, drag `RunStateManager` under it
2. Otherwise, leave at scene root

---

## Step 2: Update BaseGate

### 2.1 Locate BaseGate GameObject
1. In **Hierarchy**, find your `BaseGate` GameObject
   - It might be under a parent like `Environment`, `Base`, or at scene root
   - If you don't have a BaseGate yet, create one:
     - Right-click Hierarchy → **3D Object → Cube**
     - Rename to `BaseGate`
     - Position near your base/spawn area
     - Scale appropriately (e.g., Y: 5 for gate height)

### 2.2 Verify/Update BaseGate Component
1. Select `BaseGate` GameObject
2. In **Inspector**, locate the `BaseGate (Script)` component
3. If component is missing or shows "Script Missing", remove it and re-add:
   - **Add Component** → Type `BaseGate` → Add

### 2.3 Create Interaction Prompt UI

**Option A: Create New World Space Canvas (Recommended)**

1. Right-click `BaseGate` in Hierarchy → **UI → Canvas**
2. Rename child Canvas to `InteractionCanvas`
3. Select `InteractionCanvas`, configure in Inspector:
   ```
   ┌─ Canvas ─────────────────────────────────────┐
   │ Render Mode:  World Space                    │
   │ Event Camera:  (drag MainCamera here)        │
   │                                               │
   │ Rect Transform                                │
   │   Width:  200                                 │
   │   Height:  50                                 │
   └───────────────────────────────────────────────┘
   ```
4. Position canvas above gate:
   - Pos Y: **6** (adjust to float above gate visual)
   - Rotation: (0, 0, 0)
   - Scale: (0.01, 0.01, 0.01) — *Scale down for world space*

5. Right-click `InteractionCanvas` → **UI → Text - TextMeshPro**
   - If prompted to import TMP Essentials → **Import**
6. Rename to `PromptText`
7. Select `PromptText`, configure in Inspector:
   ```
   ┌─ Text - TextMeshPro ──────────────────────────┐
   │ Text Input:  Press [E] to Start Run           │
   │ Font Size:  36                                 │
   │ Alignment:  Center, Middle                     │
   │ Color:  Yellow or White                        │
   │ Auto Size:  ☑ Enabled                          │
   │   Min: 18    Max: 72                           │
   │                                                │
   │ Rect Transform (Anchor: Stretch)              │
   │   Left: 0   Right: 0   Top: 0   Bottom: 0     │
   └────────────────────────────────────────────────┘
   ```

**Option B: Use Existing Screen Space UI**

1. In **Hierarchy**, locate your main UI Canvas (usually `GameCanvas` or `Canvas`)
2. Right-click → **UI → Text - TextMeshPro**
3. Rename to `GateInteractionPrompt`
4. Position at bottom-center of screen:
   - Anchor: Bottom-Center
   - Pos Y: **150** (above bottom edge)
   - Width: **400**, Height: **60**
5. Configure text (same as Option A)
6. *This prompt will show on screen instead of floating above gate*

### 2.4 Configure BaseGate Component

Select `BaseGate` GameObject, configure in Inspector:

```
┌─ Base Gate (Script) ──────────────────────────┐
│                                                │
│ Gate Settings                                  │
│   Gate Visual:  (drag gate mesh/visual here)  │
│   Gate Collider:  (drag BoxCollider here)     │
│   Starts Open:  ☐                              │
│                                                │
│ Animation                                      │
│   Open Height:  5                              │
│   Animation Speed:  2                          │
│                                                │
│ Interaction                                    │
│   Interaction Range:  3                        │
│   Interaction Key:  E                          │
│   Prompt Text:  (drag PromptText TMP here)    │
└────────────────────────────────────────────────┘
```

**Field Details:**

- **Gate Visual**: Drag the actual visual mesh/GameObject that should move up/down
  - If `BaseGate` is a cube, drag itself here
  - If you have a separate mesh child, drag that child
  
- **Gate Collider**: Drag the collider that blocks player passage
  - Usually the BoxCollider on the `BaseGate` itself
  - This collider enables when gate closes, disables when open
  
- **Starts Open**: Leave **unchecked** (gate closed at start)

- **Open Height**: **5** (how high gate moves when opening)

- **Animation Speed**: **2** (units per second)

- **Interaction Range**: **3** (distance player must be within to interact)

- **Interaction Key**: **E** (key player presses to start run)

- **Prompt Text**: Drag your `PromptText` TextMeshProUGUI component here

### 2.5 Test Proximity Detection

1. Enter **Play Mode**
2. Walk your player near the gate
3. **Expected:** When within 3 units, prompt appears: "Press [E] to Start Run"
4. Walk away
5. **Expected:** Prompt disappears

**Troubleshooting:**
- Prompt doesn't appear → Check `Interaction Range` value (increase to 5 for testing)
- Prompt always visible → Check `RunStateManager` exists and `CurrentState = PreRunMenu`
- Player tag missing → Select Player, set Tag to "Player" in Inspector

---

## Step 3: Create Pre-Run Menu UI

This UI panel shows before the run starts (while in base).

### 3.1 Locate Main Canvas

1. In **Hierarchy**, find your main UI Canvas
   - Usually named: `GameCanvas`, `Canvas`, or `UICanvas`
   - If none exists:
     - Right-click Hierarchy → **UI → Canvas**
     - Rename to `GameCanvas`
     - Set Canvas Scaler: UI Scale Mode = **Scale With Screen Size**
     - Reference Resolution: **1920 x 1080**

### 3.2 Create Pre-Run Menu Panel

1. Right-click `GameCanvas` → **UI → Panel**
2. Rename to: `PreRunMenuPanel`
3. Select `PreRunMenuPanel`, configure in Inspector:

```
┌─ Rect Transform ──────────────────────────────┐
│ Anchor:  Top-Left                              │
│ Pos X:  200     Pos Y:  -100                   │
│ Width:  350     Height:  200                   │
└────────────────────────────────────────────────┘

┌─ Image (Script) ──────────────────────────────┐
│ Color:  RGBA(0, 0, 0, 180)  ← Semi-transparent│
└────────────────────────────────────────────────┘
```

### 3.3 Create Essence Text

1. Right-click `PreRunMenuPanel` → **UI → Text - TextMeshPro**
2. Rename to: `EssenceText`
3. Configure in Inspector:

```
┌─ Rect Transform ──────────────────────────────┐
│ Anchor:  Top-Stretch (top, full width)        │
│ Left: 20   Right: 20   Top: 20   Height: 40   │
└────────────────────────────────────────────────┘

┌─ Text - TextMeshPro ──────────────────────────┐
│ Text:  Essence: 400                            │
│ Font Size:  28                                 │
│ Alignment:  Left, Middle                       │
│ Color:  Yellow (#FFD700)                       │
│ Font Style:  Bold                              │
└────────────────────────────────────────────────┘
```

### 3.4 Create Instruction Text

1. Right-click `PreRunMenuPanel` → **UI → Text - TextMeshPro**
2. Rename to: `InstructionText`
3. Configure in Inspector:

```
┌─ Rect Transform ──────────────────────────────┐
│ Anchor:  Stretch (fill panel)                 │
│ Left: 20   Right: 20   Top: 80   Bottom: 20   │
└────────────────────────────────────────────────┘

┌─ Text - TextMeshPro ──────────────────────────┐
│ Text:  Spend Essence on Upgrades               │
│       Approach the gate and press [E] to      │
│       start your run!                          │
│                                                │
│ Font Size:  20                                 │
│ Alignment:  Center, Top                        │
│ Color:  White                                  │
│ Wrapping:  Enabled                             │
└────────────────────────────────────────────────┘
```

**Tip:** Use **Shift+Enter** in text field for line breaks

### 3.5 Add PreRunMenuUI Component

1. Select `PreRunMenuPanel`
2. In **Inspector**, click **Add Component**
3. Type: `PreRunMenuUI`
4. Click to add

5. Configure component:

```
┌─ Pre Run Menu UI (Script) ────────────────────┐
│                                                │
│ UI Elements                                    │
│   Panel:  (drag PreRunMenuPanel here)         │
│   Essence Text:  (drag EssenceText here)      │
│   Instruction Text:  (drag InstructionText)   │
└────────────────────────────────────────────────┘
```

**Drag References:**
- **Panel**: Drag `PreRunMenuPanel` GameObject itself
- **Essence Text**: Expand `PreRunMenuPanel`, drag `EssenceText` child
- **Instruction Text**: Expand `PreRunMenuPanel`, drag `InstructionText` child

### 3.6 Test Pre-Run Menu

1. Enter **Play Mode**
2. **Expected:** Panel visible at top-left showing Essence and instructions
3. Walk to gate and press **E**
4. **Expected:** Panel hides when run starts
5. (If you implement run end) End run
6. **Expected:** Panel reappears

---

## Step 4: Create Between-Sessions UI

This UI shows during the 60-second countdown between wave sessions.

### 4.1 Create Between-Sessions Panel

1. Right-click `GameCanvas` → **UI → Panel**
2. Rename to: `BetweenSessionsPanel`
3. Configure in Inspector:

```
┌─ Rect Transform ──────────────────────────────┐
│ Anchor:  Center                                │
│ Pos X:  0       Pos Y:  0                      │
│ Width:  600     Height:  300                   │
└────────────────────────────────────────────────┘

┌─ Image (Script) ──────────────────────────────┐
│ Color:  RGBA(0, 0, 0, 200)  ← Dark background │
└────────────────────────────────────────────────┘
```

### 4.2 Create Timer Text (Large Countdown)

1. Right-click `BetweenSessionsPanel` → **UI → Text - TextMeshPro**
2. Rename to: `TimerText`
3. Configure in Inspector:

```
┌─ Rect Transform ──────────────────────────────┐
│ Anchor:  Top-Stretch                           │
│ Left: 20   Right: 20   Top: 40   Height: 80   │
└────────────────────────────────────────────────┘

┌─ Text - TextMeshPro ──────────────────────────┐
│ Text:  01:00                                   │
│ Font Size:  72                                 │
│ Alignment:  Center, Middle                     │
│ Color:  Cyan or Orange (#FFA500)               │
│ Font Style:  Bold                              │
└────────────────────────────────────────────────┘
```

### 4.3 Create Instruction Text

1. Right-click `BetweenSessionsPanel` → **UI → Text - TextMeshPro**
2. Rename to: `InstructionText`
3. Configure in Inspector:

```
┌─ Rect Transform ──────────────────────────────┐
│ Anchor:  Stretch                               │
│ Left: 30   Right: 30   Top: 140   Bottom: 30  │
└────────────────────────────────────────────────┘

┌─ Text - TextMeshPro ──────────────────────────┐
│ Text:  Spend Gold on Obstacles!                │
│       Next wave session starting soon...       │
│                                                │
│ Font Size:  28                                 │
│ Alignment:  Center, Top                        │
│ Color:  White                                  │
│ Wrapping:  Enabled                             │
└────────────────────────────────────────────────┘
```

### 4.4 Add BetweenSessionsUI Component

1. Select `BetweenSessionsPanel`
2. In **Inspector**, click **Add Component**
3. Type: `BetweenSessionsUI`
4. Click to add

5. Configure component:

```
┌─ Between Sessions UI (Script) ────────────────┐
│                                                │
│ UI Elements                                    │
│   Panel:  (drag BetweenSessionsPanel here)    │
│   Timer Text:  (drag TimerText here)          │
│   Instruction Text:  (drag InstructionText)   │
└────────────────────────────────────────────────┘
```

**Drag References:**
- **Panel**: Drag `BetweenSessionsPanel` GameObject itself
- **Timer Text**: Expand `BetweenSessionsPanel`, drag `TimerText` child
- **Instruction Text**: Expand `BetweenSessionsPanel`, drag `InstructionText` child

### 4.5 Disable Panel by Default

1. Select `BetweenSessionsPanel`
2. At top-left of Inspector, **uncheck** the checkbox next to the GameObject name
   - This disables the panel (it will be enabled by script when needed)

### 4.6 Test Between-Sessions UI

1. Enter **Play Mode**
2. Start run (press E at gate)
3. Complete 10 waves (or use debug keys if you have them)
4. **Expected:** When Session 1 completes:
   - `BetweenSessionsPanel` appears at center of screen
   - Timer shows **01:00** and counts down
   - Instructions show: "Spend Gold on Obstacles!"
5. Wait for timer to reach **00:00**
6. **Expected:** Panel hides, Session 2 starts

---

## Step 5: Update Existing Systems (If Needed)

### 5.1 Check WaveSpawner Settings

1. Locate your `WaveSpawner` GameObject in Hierarchy
2. Select it, find `WaveSpawner (Script)` component
3. **Verify:** `Auto Start Waves` = **☐ (unchecked)**
   - If checked, uncheck it — waves should only start when RunStateManager says so

### 5.2 Check TimerDisplay (Old System)

If you have a `TimerDisplay` component in your scene showing the old base timer:

1. Search Hierarchy for `TimerDisplay` or objects with `TimerDisplay` component
2. Select the GameObject
3. **Option A:** Update reference to new system (already done in script)
   - The updated `TimerDisplay.cs` now uses `RunStateManager`
4. **Option B:** Disable or delete if not needed
   - Right-click component → **Remove Component**

### 5.3 Add Player Tag (If Missing)

The `BaseGate` interaction requires the player to have the "Player" tag:

1. Select your Player GameObject in Hierarchy
2. In **Inspector**, at the top, find **Tag** dropdown
3. Set to: **Player**
   - If "Player" tag doesn't exist:
     - Click **Add Tag...**
     - Click **+** under Tags
     - Enter "Player"
     - Close Tag Manager
     - Return to Player GameObject and set Tag to "Player"

---

## Step 6: Final Scene Hierarchy

After completing all steps, your Hierarchy should look similar to this:

```
MainScene
├── GameManagers (or root)
│   ├── GameManager
│   ├── GameProgressionManager
│   ├── WaveController
│   ├── CurrencyManager
│   ├── SaveSystem
│   ├── RunStateManager          ← NEW
│   └── ...
├── Environment
│   ├── Ground
│   ├── BaseGate                 ← UPDATED
│   │   └── InteractionCanvas    ← NEW (if world space)
│   │       └── PromptText       ← NEW
│   └── ...
├── Player
├── MainCamera
├── CinemachineCamera
├── WaveSpawner
├── GameCanvas
│   ├── PreRunMenuPanel          ← NEW
│   │   ├── EssenceText          ← NEW
│   │   └── InstructionText      ← NEW
│   ├── BetweenSessionsPanel     ← NEW (disabled by default)
│   │   ├── TimerText            ← NEW
│   │   └── InstructionText      ← NEW
│   ├── WaveDisplay
│   ├── CurrencyDisplayPanel
│   └── ...
└── ...
```

---

## Testing Checklist

### ✅ Pre-Run Menu State
- [ ] Pre-Run Menu panel visible at scene start
- [ ] Essence amount displays correctly
- [ ] Instructions visible
- [ ] Gate is closed
- [ ] Player can move freely

### ✅ Gate Interaction
- [ ] Walking near gate (within 3 units) shows prompt
- [ ] Prompt says "Press [E] to Start Run"
- [ ] Walking away hides prompt
- [ ] Pressing E starts run
- [ ] Gate opens when run starts
- [ ] Pre-Run Menu panel hides

### ✅ Wave Session
- [ ] Waves spawn (Wave 1, 2, 3... up to 10)
- [ ] Enemies can be killed
- [ ] Console logs wave clears
- [ ] After 10 waves: Session Complete message

### ✅ Between-Sessions State
- [ ] Between-Sessions panel appears after 10 waves
- [ ] Timer shows 01:00
- [ ] Timer counts down (00:59, 00:58...)
- [ ] Instructions visible: "Spend Gold on Obstacles!"
- [ ] At 00:00: Panel hides, next session starts (Waves 11-20)

### ✅ Console Logging
Check Console for colored debug messages:
- [ ] `<color=green>Player clicked gate - Run starting!</color>`
- [ ] `<color=cyan>=== RUN STARTED ===</color>`
- [ ] `<color=green>=== SESSION 1 STARTED (Waves 1-10) ===</color>`
- [ ] `<color=yellow>=== SESSION 1 COMPLETE ===</color>`
- [ ] `<color=yellow>Between-sessions timer expired! Starting next session...</color>`
- [ ] `<color=green>=== SESSION 2 STARTED (Waves 11-20) ===</color>`

---

## Troubleshooting

### Issue: "Script Missing" on Components

**Cause:** Compilation errors in the new scripts

**Solution:**
1. Open **Console** window (Ctrl+Shift+C / Cmd+Shift+C)
2. Look for red error messages
3. Double-click error to open script
4. Fix error and save
5. Wait for Unity to recompile
6. Re-add component if needed

---

### Issue: Prompt Doesn't Appear

**Possible Causes:**
1. **Player tag missing** → Add "Player" tag to Player GameObject
2. **Interaction Range too small** → Increase to 5 for testing
3. **RunStateManager not in scene** → Add RunStateManager GameObject
4. **PromptText reference missing** → Drag PromptText to BaseGate component

---

### Issue: Between-Sessions Panel Always Visible

**Cause:** Panel not disabled by default

**Solution:**
1. Select `BetweenSessionsPanel` in Hierarchy
2. Uncheck checkbox at top-left of Inspector (next to GameObject name)
3. Save scene

---

### Issue: Timer Shows "00:00" Instead of "01:00"

**Cause:** Timer not receiving initial value

**Solution:**
1. Check `RunStateManager` is in scene
2. Check `Between Sessions Duration` = **60** in RunStateManager Inspector
3. Check Console for errors in `BetweenSessionsUI` script

---

### Issue: Waves Don't Start When Pressing E

**Possible Causes:**
1. **WaveSpawner Auto Start = true** → Disable it
2. **RunStateManager missing** → Add to scene
3. **WaveController not listening** → Check Console for errors

**Debug:**
1. Press E at gate
2. Check Console for: `<color=green>Player clicked gate - Run starting!</color>`
3. If message appears but waves don't start → Check WaveSpawner/WaveController setup

---

## Optional: Visual Polish

### Add Outline to Between-Sessions Panel

1. Select `BetweenSessionsPanel`
2. Add Component → **Outline** (Unity UI component)
3. Configure:
   - Effect Color: Cyan or Yellow
   - Effect Distance: (5, -5)

### Add Background Blur/Dim

1. Select `BetweenSessionsPanel`
2. Add Component → **Canvas Group**
3. Configure:
   - Alpha: 0.95
   - (In script, you can fade in/out by animating Alpha)

### Animate Timer Text

Add pulsing/scaling animation when timer is low (< 10 seconds):
- Use Unity's Animation system or DOTween
- Scale up when timer < 10 seconds
- Change color to red when < 5 seconds

---

## Next Steps

Once setup is complete:

1. **Test full run flow:**
   - Start at base → Press E → Complete Session 1 → Between-sessions timer → Session 2
   
2. **Integrate with existing systems:**
   - Hook up obstacle placement during between-sessions
   - Add shop UI for spending Gold during breaks
   - Connect defeat/victory conditions to `RunStateManager.EndRun()`

3. **Polish:**
   - Add sound effects for gate opening/closing
   - Add visual effects for session transitions
   - Add more detailed instructions in UI

4. **Save scene:**
   - **File → Save** (Ctrl+S / Cmd+S)

---

## Summary

You should now have:

✅ **RunStateManager** managing run states  
✅ **BaseGate** with interactive prompt  
✅ **Pre-Run Menu UI** showing Essence and instructions  
✅ **Between-Sessions UI** with 60-second countdown  
✅ Complete run loop: Base → Session → Break → Session → ...  

The system is ready to use! 🎮
