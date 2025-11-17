# Jump Animation Stuck - Quick Fix Guide

## 🔴 Problem: Jump animation starts but never ends

---

## ✅ Fix Checklist - Do These in Order:

### **Step 1: Verify Jump → Idle Transition EXISTS**

1. Open **Animator** window (Window → Animation → Animator)
2. Look at the **Jump** state
3. **Do you see a WHITE ARROW coming OUT of Jump going to Idle?**

**If NO:**
- Right-click **Jump state** → Make Transition
- Click on **Idle state**
- A white arrow should appear

**If YES:**
- Continue to Step 2

---

### **Step 2: Configure Jump → Idle Transition**

1. **Click on the white arrow** going from Jump → Idle
2. In **Inspector**, verify these EXACT settings:

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
INSPECTOR SETTINGS FOR "Jump → Idle"
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Has Exit Time: ✓ CHECKED (CRITICAL!)

Settings:
  Exit Time: 0.75 (or 0.7-0.8)
  Fixed Duration: ✓ CHECKED
  Transition Duration (s): 0.15

Interruption Source: None

Conditions (1):
  IsGrounded   true
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

**CRITICAL CHECKLIST:**
- [ ] "Has Exit Time" is **CHECKED** ✓
- [ ] Exit Time is between 0.7 - 0.8
- [ ] Condition "IsGrounded" equals **true** (not false!)
- [ ] Only ONE condition (IsGrounded)

**Common Mistakes:**
- ❌ Has Exit Time is UNCHECKED → Animation loops forever
- ❌ Exit Time is 1.0 → Animation plays fully then loops
- ❌ Exit Time is 0.0 → Transitions immediately (wrong)
- ❌ IsGrounded is set to "false" → Never triggers when you land
- ❌ No conditions at all → Might not transition

---

### **Step 3: Check Jump Animation Settings**

1. Click on **Jump state** (the box, not the arrow)
2. In Inspector, check:

```
Motion: A_Jump_Idle_Masc (or similar)
Speed: 1.0
Mirror: ✗ UNCHECKED
Foot IK: ✗ UNCHECKED
Write Defaults: ✓ CHECKED
Transitions:
  - (should show "Jump → Idle" listed)
```

3. **Click the Motion field** (the animation clip)
4. In Inspector for the animation clip:
   - **Loop Time:** ✗ UNCHECKED (CRITICAL!)
   - **Loop Pose:** ✗ UNCHECKED

**If Loop Time is CHECKED:**
- The animation will loop forever!
- UNCHECK it and Apply

---

### **Step 4: Verify Animator Parameters**

In the **Animator** window, click the **Parameters** tab (left side):

**Should have:**
- `Speed` (Float) - Default: 0
- `Jump` (Trigger)
- `IsGrounded` (Bool) - Default: **true** ✓
- `Attack` (Trigger)

**Check IsGrounded default value:**
- Click on `IsGrounded` parameter
- Default value should be **checked/true**
- If it's false → Change to true

---

### **Step 5: Use Debug Tool**

1. Select `/Player` in Hierarchy
2. **Add Component** → `Jump Animation Debugger`
3. Assign:
   - **Animator** → Drag `/Player/Model/Characters`
   - **Character Controller** → Drag CharacterController from Player
4. **Enter Play Mode**
5. **Jump** and watch the debug info

**The debug will tell you:**
- Is CharacterController grounded?
- Is Animator IsGrounded parameter correct?
- What's the normalized time of the animation?
- Why the transition isn't happening

**Look for these messages:**
- ✅ "Normal operation" → Everything is working
- 🟡 "Reason: Still in air" → You're not landed yet (normal)
- 🟡 "Reason: Exit time not reached" → Animation still playing (normal)
- 🔴 "ERROR: Should transition but isn't!" → **Transition doesn't exist!**
- 🔴 "ERROR: Grounded but animator param is FALSE!" → **Code issue**
- 🔴 "NO TRANSITIONS FOUND FROM JUMP!" → **Missing Jump → Idle transition**
- 🔴 "Jump animation is set to LOOP!" → **Animation clip Loop setting wrong**

---

### **Step 6: Test the Fix**

**In Play Mode:**

1. **Jump** (press Space)
2. Watch the character:
   - ✅ Should play jump animation while in air
   - ✅ Should smoothly transition back to Idle when landing
   - ✅ Should NOT get stuck mid-jump
   - ✅ Should NOT loop the jump forever

3. **Jump while moving:**
   - Walk/Run and press Space
   - Should jump, then return to Walk/Run based on input
   - Should NOT get stuck

---

## 🔍 Advanced Diagnosis

### **If Jump STILL Stuck After Above Steps:**

**Check Transition Interruption Settings:**

1. Select **Jump → Idle** transition
2. In Inspector, look for **Interruption Source**
3. Should be: `None` or `Current State`
4. **Can Be Interrupted By:** Should allow interruption

**Check for Conflicting Transitions:**

1. In Animator, look at **Jump state**
2. Are there **multiple arrows** coming OUT of Jump?
3. If yes, they might be fighting each other
4. Solution: Keep only Jump → Idle

**Check Layer Weights:**

1. In Animator, click **Layers** tab
2. Make sure you're on "Base Layer"
3. Weight should be 1.0
4. No other layers should be affecting Jump

---

## 📋 Quick Reference: Correct Setup

### **Transitions TO Jump:**
```
Any State → Jump
  Conditions: Jump (trigger)
  Has Exit Time: ✗ UNCHECKED
  Transition Duration: 0.05
```

### **Transitions FROM Jump:**
```
Jump → Idle
  Conditions: IsGrounded = true
  Has Exit Time: ✓ CHECKED
  Exit Time: 0.75
  Transition Duration: 0.15
```

---

## 🎯 Expected Behavior

**✅ CORRECT:**
- Press Space → Jump animation plays immediately
- While in air → Animation continues
- Land on ground → Smoothly transitions to Idle/Walk/Run within 0.2s
- Can jump again immediately

**❌ INCORRECT (Your Current Issue):**
- Press Space → Jump animation plays
- Land on ground → Animation keeps looping/stuck
- Character is grounded but animation won't stop
- Have to wait or jump gets stuck forever

---

## 🛠️ Emergency Reset

**If nothing works, recreate the Jump → Idle transition:**

1. **Delete** the existing Jump → Idle transition:
   - Click the arrow
   - Press Delete key

2. **Create new transition:**
   - Right-click Jump state
   - Make Transition
   - Click Idle state

3. **Configure exactly as shown in Step 2**

4. **Test again**

---

## 💡 Most Common Solution

**90% of the time, the issue is:**

Either:
- **Has Exit Time is UNCHECKED** on Jump → Idle (needs to be CHECKED)
- **The Jump → Idle transition doesn't exist at all**
- **Loop Time is CHECKED** on the animation clip (needs to be UNCHECKED)

**Check these three things first!**

---

Use the `JumpAnimationDebugger` script to see exactly what's wrong! 🚀
