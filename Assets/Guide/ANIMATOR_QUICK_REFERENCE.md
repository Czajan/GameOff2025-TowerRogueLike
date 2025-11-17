# Animator Quick Reference - Correct Setup

## 📋 Animation State Machine Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                         ANY STATE                            │
│                             │                                │
│                             │ Jump (trigger)                 │
│                             ▼                                │
│         ┌──────────────────────────────────┐                │
│         │           JUMP                   │                │
│         │  A_Jump_Idle_Masc.fbx           │                │
│         │  Has Exit Time: YES (0.7-0.8)   │                │
│         └──────────┬───────────────────────┘                │
│                    │ IsGrounded = true                       │
└────────────────────┼─────────────────────────────────────────┘
                     ▼
       ┌─────────────────────────┐
       │         IDLE             │◄────── DEFAULT STATE
       │  A_Idle_Standing_Masc   │
       │  Speed = 0              │
       └────┬────────────────┬───┘
            │                │
   Speed>0.1│       Speed>1.5│
            ▼                ▼
    ┌───────────┐    ┌──────────┐
    │   WALK    │◄───│   RUN    │
    │ A_Walk_F  │    │ A_Run_F  │
    │ Speed:    │    │ Speed:   │
    │ 0.1-1.5   │    │ 1.5+     │
    └─────┬─────┘    └────┬─────┘
          │               │
   Speed<0.1│      Speed<1.5│
          ▼               ▼
       (back to IDLE or each other)
```

---

## 🎯 Critical Settings Summary

### IDLE State
```yaml
Animation: A_Idle_Standing_Masc.fbx
Speed: 1.0
Write Defaults: Yes
```

**Transitions OUT:**
- To Walk: `Speed > 0.1` | No Exit Time | Duration: 0.15
- To Run: `Speed > 1.5` | No Exit Time | Duration: 0.2

---

### WALK State
```yaml
Animation: A_Walk_F_Masc.fbx
Speed: 1.0 (adjust 0.9-1.2 for feel)
Write Defaults: Yes
```

**Transitions OUT:**
- To Idle: `Speed < 0.1` | No Exit Time | Duration: 0.15
- To Run: `Speed > 1.5` | No Exit Time | Duration: 0.15

---

### RUN State
```yaml
Animation: A_Run_F_Masc.fbx
Speed: 1.0 (adjust 0.9-1.2 for feel)
Write Defaults: Yes
```

**Transitions OUT:**
- To Walk: `Speed < 1.5` | No Exit Time | Duration: 0.15
- To Idle: `Speed < 0.1` | No Exit Time | Duration: 0.2

---

### JUMP State
```yaml
Animation: A_Jump_Idle_Masc.fbx
Speed: 1.0
Write Defaults: Yes
```

**Transitions IN:**
- From Any State: `Jump (trigger)` | No Exit Time | Duration: 0.05

**Transitions OUT:**
- To Idle: `IsGrounded = true` | HAS Exit Time (0.7-0.8) | Duration: 0.15

⚠️ **CRITICAL:** Jump → Idle MUST have Exit Time enabled!
This ensures the jump animation plays most of the way through before landing.

---

## 🔍 Common Mistakes & Fixes

### ❌ Mistake 1: Using RootMotion Animations
**Symptom:** Character slides across ground, movement speed is weird
**Check:** Animation names contain "_RootMotion"
**Fix:** Replace with non-RootMotion versions (remove _RootMotion from name)

### ❌ Mistake 2: Exit Time Enabled on Movement Transitions
**Symptom:** Character continues old animation for too long, feels sluggish
**Check:** "Has Exit Time" is checked on Idle/Walk/Run transitions
**Fix:** UNCHECK "Has Exit Time" on ALL transitions except Jump → Idle

### ❌ Mistake 3: Transition Duration Too Long
**Symptom:** Animations blend too slowly, character looks like it's "between" animations
**Check:** Transition Duration > 0.5
**Fix:** Set to 0.1 - 0.2 seconds

### ❌ Mistake 4: Wrong Speed Thresholds
**Symptom:** Character runs when you want to walk, or vice versa
**Check:** Speed conditions don't match 0.1 (walk) and 1.5 (run)
**Fix:** Adjust conditions to match values below

### ❌ Mistake 5: Jump Transition Has No Exit Time
**Symptom:** Jump animation cuts off mid-air or plays after landing
**Check:** Jump → Idle transition has "Has Exit Time" unchecked
**Fix:** CHECK "Has Exit Time" and set to 0.7-0.8

---

## 📊 Parameter Values Reference

### Speed Parameter (Float)
| Value Range | Animation State | Player Action |
|-------------|----------------|---------------|
| 0.0 | Idle | Standing still |
| 0.01 - 0.09 | Idle | Slight input (deadzone) |
| 0.1 - 1.0 | Walk | Normal movement |
| 1.0 - 1.49 | Walk | Fast movement (no sprint) |
| 1.5+ | Run | Sprint + movement |

**Transition Thresholds:**
- **Idle ↔ Walk:** 0.1
- **Walk ↔ Run:** 1.5

### IsGrounded Parameter (Bool)
- `true`: On ground (can jump, normal animations)
- `false`: In air (falling or jumping)

### Jump Parameter (Trigger)
- Triggered when Space/Jump button pressed
- Automatically resets after one frame

---

## 🎮 Testing Checklist

Enter Play Mode and test:

**Movement Tests:**
- [ ] Standing still → Plays Idle animation
- [ ] Press W slowly → Transitions to Walk within 0.2 seconds
- [ ] Release W → Returns to Idle within 0.2 seconds
- [ ] Hold Shift+W → Transitions to Run within 0.2 seconds
- [ ] Release Shift (keep W) → Transitions back to Walk
- [ ] Move in all directions → Animations look smooth, no sliding

**Jump Tests:**
- [ ] Press Space from Idle → Jump animation starts immediately
- [ ] Jump animation plays fully while in air
- [ ] Land on ground → Smoothly transitions to Idle
- [ ] Press Space while moving → Jump works, no animation weirdness
- [ ] Land while holding movement → Smoothly transitions to Walk/Run

**Transition Tests:**
- [ ] All transitions feel responsive (< 0.3 seconds)
- [ ] No "stuck" animations
- [ ] No sudden pops or jerks between animations
- [ ] Character rotation matches movement direction

---

## 🛠️ Tuning Guide

After basic setup works, fine-tune for game feel:

### Movement Feel Too Sluggish?
- Reduce transition durations → 0.05 - 0.1
- Lower Speed threshold for walk → 0.05 instead of 0.1
- Increase animation Speed multipliers → 1.1 - 1.2

### Movement Feel Too Twitchy?
- Increase transition durations → 0.2 - 0.25
- Add interruption settings → Can Be Interrupted By: Next State
- Decrease animation Speed multipliers → 0.9 - 1.0

### Jump Feels Wrong?
**Cuts off mid-air:**
- Increase Jump → Idle Exit Time → 0.8 or 0.9

**Plays after landing:**
- Decrease Jump → Idle Exit Time → 0.6 or 0.7

**Animation too slow:**
- Increase Jump state Speed multiplier → 1.2 - 1.5

**Animation too fast:**
- Decrease Jump state Speed multiplier → 0.8 - 0.9

### Walk/Run Not Syncing?
- Adjust animation Speed multipliers on states
- Check actual PlayerController moveSpeed values
- Use AnimatorDebugger to see real-time Speed parameter

---

## 🎯 Expected Behavior

**✅ CORRECT:**
- Character responds to input within ~0.1-0.2 seconds
- Animations smoothly blend between states
- No sliding or foot skating
- Jump animation plays fully in air, lands smoothly
- Sprinting feels faster and looks faster

**❌ INCORRECT:**
- Character slides while animating in place (wrong animations)
- Animations take > 0.5 seconds to change (exit time issue)
- Jump cuts off or plays on ground (exit time issue)
- Character "sticks" in an animation (exit time + condition issue)
- Feet slide on ground (animation speed mismatch)

---

## 📝 Quick Copy-Paste Settings

For each transition, select it and apply these in Inspector:

**Idle → Walk:**
```
Has Exit Time: ✗
Settings:
  - Transition Duration: 0.15
Conditions:
  - Speed > 0.1
```

**Idle → Run:**
```
Has Exit Time: ✗
Settings:
  - Transition Duration: 0.2
Conditions:
  - Speed > 1.5
```

**Walk → Idle:**
```
Has Exit Time: ✗
Settings:
  - Transition Duration: 0.15
Conditions:
  - Speed < 0.1
```

**Walk → Run:**
```
Has Exit Time: ✗
Settings:
  - Transition Duration: 0.15
Conditions:
  - Speed > 1.5
```

**Run → Walk:**
```
Has Exit Time: ✗
Settings:
  - Transition Duration: 0.15
Conditions:
  - Speed < 1.5
```

**Run → Idle:**
```
Has Exit Time: ✗
Settings:
  - Transition Duration: 0.2
Conditions:
  - Speed < 0.1
```

**Any State → Jump:**
```
Has Exit Time: ✗
Settings:
  - Transition Duration: 0.05
Conditions:
  - Jump (trigger)
```

**Jump → Idle:**
```
Has Exit Time: ✓ CHECKED!
Settings:
  - Exit Time: 0.75
  - Transition Duration: 0.15
Conditions:
  - IsGrounded = true
```

---

**Use this as your reference while setting up the Animator Controller! 🎮**
