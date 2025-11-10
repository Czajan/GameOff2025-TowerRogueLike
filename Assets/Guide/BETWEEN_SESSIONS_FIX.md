# Between Sessions Panel Fix

## Problem Identified

The **BetweenSessionsPanel** was not visible during the 60-second between-sessions period because:

1. The `BetweenSessionsUI` script was trying to activate its own GameObject via `panel.SetActive(true)`
2. This approach has timing issues when the script is on the GameObject it's trying to activate
3. The old **TimerText** system with `TimerDisplay` script was redundant and showing instead

## Solution Applied

### ✅ Fixed BetweenSessionsUI.cs

**Changes Made:**
- Removed the `panel` field (was causing self-reference issues)
- Added **CanvasGroup** control for visibility (like SimpleShopUI)
- Panel stays active in hierarchy, visibility controlled by alpha/interactable/blocksRaycasts
- Added debug logs to track show/hide calls
- Added proper cleanup in `OnDestroy()`

**New Behavior:**
```csharp
ShowPanel() → CanvasGroup: alpha=1, interactable=true, blocksRaycasts=true
HidePanel() → CanvasGroup: alpha=0, interactable=false, blocksRaycasts=false
```

### 📋 Manual Cleanup Required

#### 1. Remove Old TimerText GameObject

The standalone `/GameCanvas/TimerText` with `TimerDisplay` script is now redundant:

**To Remove:**
1. Select `/GameCanvas/TimerText` in Hierarchy
2. Press **Delete** key
3. This removes the old timer system

**Why?** The BetweenSessionsPanel has its own timer display at `/GameCanvas/BetweenSessionsPanel/TimerText`.

#### 2. Add CanvasGroup to BetweenSessionsPanel

The script will auto-create it, but you can add it manually for clarity:

1. Select `/GameCanvas/BetweenSessionsPanel`
2. Click **Add Component**
3. Search for **Canvas Group**
4. Click to add

#### 3. Ensure BetweenSessionsPanel is Active

1. Select `/GameCanvas/BetweenSessionsPanel`
2. Check the **checkbox** at the top of the Inspector to enable it
3. The CanvasGroup will control visibility, not GameObject activation

#### 4. Reassign References (if needed)

Select `/GameCanvas/BetweenSessionsPanel`, check `BetweenSessionsUI` component:
- ~~`Panel`~~ (removed, no longer needed)
- `Timer Text` → Should point to `/GameCanvas/BetweenSessionsPanel/TimerText`
- `Instruction Text` → Should point to `/GameCanvas/BetweenSessionsPanel/InstructionText`

## Testing

### Test 1: Start Run and Complete Session

1. Start game in base
2. Open gate to start wave session
3. Complete or die in the session
4. **Expected:**
   - ✅ BetweenSessionsPanel appears with timer
   - ✅ Shows "Spend Gold on Obstacles!"
   - ✅ Timer counts down from 01:00
   - ✅ Console shows: `<color=cyan>BetweenSessionsPanel shown</color>`

### Test 2: Next Session Starts

1. Wait for 60 seconds or let timer reach 00:00
2. **Expected:**
   - ✅ BetweenSessionsPanel disappears
   - ✅ New wave session starts
   - ✅ Console shows: `<color=cyan>BetweenSessionsPanel hidden</color>`

## Files Modified

- `/Assets/Scripts/UI/BetweenSessionsUI.cs`
  - Removed `panel` field
  - Added CanvasGroup control
  - Added debug logs
  - Added OnDestroy cleanup

## Files To Delete (Manual)

- `/Assets/Scripts/Systems/TimerDisplay.cs` (optional - after deleting TimerText GameObject)
- Remove `/GameCanvas/TimerText` GameObject from scene

## Architecture Notes

**Before:**
```
TimerText (standalone, old system)
  └─ TimerDisplay script → Shows "Next Session: 00:XX"

BetweenSessionsPanel (not showing)
  └─ BetweenSessionsUI → trying to activate itself (failed)
      └─ TimerText (child, not used)
```

**After:**
```
BetweenSessionsPanel (always active)
  └─ BetweenSessionsUI → CanvasGroup control ✅
      ├─ TimerText → Shows "00:XX"
      └─ InstructionText → Shows instructions
```

---

**Result:** Clean, unified between-sessions UI that properly shows/hides!
