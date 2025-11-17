# Animator Fix Guide - Sliding & Jump Issues

## Issues Identified

### ✅ **FIXED: Speed Parameter Calculation**
I've fixed the Speed parameter calculation in `PlayerController.cs`:
- **Old:** Was dividing by statMoveSpeed which made the value always around 1.0
- **New:** Uses raw input magnitude (0-1) and multiplies by sprint multiplier when sprinting
- **Result:** Speed parameter now correctly ranges from 0 (idle) to ~1.5 (sprinting)

---

## Critical Animator Settings to Check

### 🔴 Problem 1: Character Sliding (Movement doesn't match animation)

This happens when:
1. **Wrong animations are used** (RootMotion versions)
2. **Transition settings have exit times enabled**
3. **Animation speeds don't match movement**

### ✅ Solution:

**Step 1: Verify You're Using NON-RootMotion Animations**

Open your Animator Controller and check EACH state:

**Idle State:**
- ✓ Correct: `A_Idle_Standing_Masc.fbx`
- ✗ Wrong: `A_Idle_Standing_RootMotion_Masc.fbx`

**Walk State:**
- ✓ Correct: `A_Walk_F_Masc.fbx`
- ✗ Wrong: `A_Walk_F_RootMotion_Masc.fbx`

**Run State:**
- ✓ Correct: `A_Run_F_Masc.fbx`
- ✗ Wrong: `A_Run_F_RootMotion_Masc.fbx`

**Jump State:**
- ✓ Correct: `A_Jump_Idle_Masc.fbx`
- ✗ Wrong: `A_Jump_Idle_RootMotion_Masc.fbx`

**How to Fix:**
1. Open Animator window (Window → Animation → Animator)
2. Click on each state (Idle, Walk, Run, Jump)
3. In Inspector, look at **Motion** field
4. If it says "RootMotion" in the name → **CHANGE IT!**
5. Click the circle next to Motion → Search for the non-RootMotion version

---

**Step 2: Fix Transition Settings (CRITICAL!)**

Open your Animator Controller and check ALL transitions:

### **From Idle → Walk:**
- **Has Exit Time:** ✗ UNCHECKED
- **Exit Time:** N/A
- **Transition Duration:** 0.1 - 0.15
- **Conditions:** Speed > 0.1

### **From Idle → Run:**
- **Has Exit Time:** ✗ UNCHECKED
- **Exit Time:** N/A
- **Transition Duration:** 0.15 - 0.2
- **Conditions:** Speed > 1.5

### **From Walk → Idle:**
- **Has Exit Time:** ✗ UNCHECKED
- **Exit Time:** N/A
- **Transition Duration:** 0.1 - 0.15
- **Conditions:** Speed < 0.1

### **From Walk → Run:**
- **Has Exit Time:** ✗ UNCHECKED
- **Exit Time:** N/A
- **Transition Duration:** 0.1 - 0.15
- **Conditions:** Speed > 1.5

### **From Run → Walk:**
- **Has Exit Time:** ✗ UNCHECKED
- **Exit Time:** N/A
- **Transition Duration:** 0.1 - 0.15
- **Conditions:** Speed < 1.5

### **From Run → Idle:**
- **Has Exit Time:** ✗ UNCHECKED
- **Exit Time:** N/A
- **Transition Duration:** 0.15 - 0.2
- **Conditions:** Speed < 0.1

**How to Fix:**
1. Click on the transition arrow in Animator
2. In Inspector, look at settings
3. UNCHECK "Has Exit Time" (this is critical!)
4. Set Transition Duration to 0.1-0.2
5. Verify conditions match above

---

**Step 3: Adjust Speed Thresholds**

The Speed parameter now works like this:
- **Speed = 0**: Character not moving (Idle)
- **Speed = 0.1 - 1.0**: Character walking
- **Speed = 1.5+**: Character sprinting

Update your transition conditions:

**Recommended Settings:**
```
Idle → Walk: Speed > 0.1
Walk → Run: Speed > 1.2
Run → Walk: Speed < 1.2 (with small delay)
Walk → Idle: Speed < 0.1
```

---

## 🔴 Problem 2: Jump Animation Issues

This is caused by:
1. **Has Exit Time enabled on Jump state**
2. **Exit Time set too high/low**
3. **IsGrounded bool not updating correctly**

### ✅ Solution:

**Step 1: Fix Jump Transition FROM any state TO Jump**

**From Idle/Walk/Run → Jump:**
- **Has Exit Time:** ✗ UNCHECKED
- **Transition Duration:** 0.05 - 0.1 (very fast!)
- **Conditions:** Jump (Trigger)

**How to Fix:**
1. Select transition arrow going INTO Jump state
2. UNCHECK "Has Exit Time"
3. Set Transition Duration to 0.05
4. Verify Jump trigger is the only condition

---

**Step 2: Fix Jump Transition BACK to Idle/Walk/Run**

**From Jump → Idle:**
- **Has Exit Time:** ✓ CHECKED (yes, this one NEEDS it!)
- **Exit Time:** 0.7 - 0.8
- **Transition Duration:** 0.15 - 0.2
- **Conditions:** IsGrounded = true

**Why Exit Time is needed here:**
- The jump animation needs to play most of the way through
- Exit Time 0.7 means "start transitioning when 70% done"
- This prevents the animation from cutting off mid-air

**How to Fix:**
1. Select transition arrow FROM Jump state
2. CHECK "Has Exit Time"
3. Set Exit Time to 0.7 or 0.8
4. Add condition: IsGrounded = true
5. Set Transition Duration to 0.15

---

**Step 3: Check Jump Animation Length**

1. Open Animator window
2. Click on Jump state
3. Look at the animation clip assigned
4. The animation should be 0.5 - 1.0 seconds long

If your jump feels too slow/fast:
1. Click on Jump state
2. In Inspector, adjust **Speed** multiplier
   - Speed = 1.0 (normal)
   - Speed = 1.5 (faster)
   - Speed = 0.8 (slower)

---

## 🎯 Complete Animator Setup Checklist

### Parameters (should already be set):
- ✓ `Speed` (Float) - Default: 0
- ✓ `Jump` (Trigger)
- ✓ `IsGrounded` (Bool) - Default: true
- ✓ `Attack` (Trigger)

### States:
- ✓ **Idle** - Default state (orange)
- ✓ **Walk** - Non-RootMotion animation
- ✓ **Run** - Non-RootMotion animation
- ✓ **Jump** - Non-RootMotion animation

### Transitions - FROM Idle:
- ✓ Idle → Walk: Speed > 0.1, No Exit Time
- ✓ Idle → Run: Speed > 1.5, No Exit Time
- ✓ Idle → Jump: Jump trigger, No Exit Time

### Transitions - FROM Walk:
- ✓ Walk → Idle: Speed < 0.1, No Exit Time
- ✓ Walk → Run: Speed > 1.5, No Exit Time
- ✓ Walk → Jump: Jump trigger, No Exit Time

### Transitions - FROM Run:
- ✓ Run → Walk: Speed < 1.5, No Exit Time
- ✓ Run → Idle: Speed < 0.1, No Exit Time
- ✓ Run → Jump: Jump trigger, No Exit Time

### Transitions - FROM Jump:
- ✓ Jump → Idle: IsGrounded = true, HAS Exit Time (0.7-0.8)

---

## 🔧 Testing with Debug Tool

I've created an `AnimatorDebugger` script for you at `/Assets/Scripts/Debug/AnimatorDebugger.cs`.

**To use it:**
1. Select `/Player` in Hierarchy
2. Add Component → `Animator Debugger`
3. Drag `/Player/Model/Characters` to **Animator** field
4. Drag the CharacterController to **Character Controller** field
5. Enter Play Mode

**You'll see on-screen debug info:**
- Current animator state
- Speed parameter value
- IsGrounded parameter value
- Current animation clip playing
- Normalized time (how far through animation)

**Use this to diagnose:**
- Is Speed parameter changing correctly? (Should be 0-1.5)
- Is IsGrounded updating? (Should be true when on ground)
- Do animations transition at the right time?
- Is the Jump state playing the full animation?

---

## 🎨 Fine-Tuning After Fix

Once basic movement works:

**1. Adjust Animation Speeds:**
- If walk looks too slow → Select Walk state → Increase Speed to 1.2
- If run looks too slow → Select Run state → Increase Speed to 1.1-1.2
- If idle is too animated → Select Idle state → Decrease Speed to 0.9

**2. Adjust Transition Durations:**
- Smoother = 0.25-0.3 (slower blend)
- Snappier = 0.05-0.1 (faster blend)
- Default = 0.15-0.2 (balanced)

**3. Adjust Speed Thresholds:**
- Earlier walk → Speed > 0.05 instead of 0.1
- Earlier run → Speed > 1.0 instead of 1.5
- Test in Play Mode to find sweet spot

---

## 📊 Expected Speed Values

With the fixed code:

| Player Action | Input Magnitude | Sprint? | Final Speed Value |
|--------------|-----------------|---------|-------------------|
| Standing Still | 0.0 | No | 0.0 |
| Walking Forward | 1.0 | No | 1.0 |
| Walking Diagonal | 1.0 | No | 1.0 |
| Running Forward | 1.0 | Yes | 1.5 |
| Running Diagonal | 1.0 | Yes | 1.5 |

**Transition Triggers:**
- `Speed = 0.0` → Idle
- `Speed = 0.1 - 1.2` → Walk
- `Speed = 1.5+` → Run
- `Jump Trigger` → Jump (any time)

---

## ✅ Quick Fix Checklist

Do these in order:

1. ✓ **Code Fix Applied** - PlayerController.cs updated (already done!)
2. ⬜ **Check Animations** - Verify NO "RootMotion" in animation names
3. ⬜ **Disable Exit Times** - Uncheck "Has Exit Time" on ALL transitions EXCEPT Jump → Idle
4. ⬜ **Set Jump Exit Time** - Jump → Idle needs Exit Time = 0.7-0.8
5. ⬜ **Reduce Transition Durations** - Set all to 0.1-0.2 seconds
6. ⬜ **Verify Speed Thresholds** - Walk at 0.1, Run at 1.5
7. ⬜ **Test in Play Mode** - Use AnimatorDebugger to verify

---

**After following this guide, your character should:**
- ✓ Stop sliding - animations match movement speed
- ✓ Transition smoothly between idle/walk/run
- ✓ Jump animation plays correctly and lands smoothly
- ✓ Respond instantly to input changes

