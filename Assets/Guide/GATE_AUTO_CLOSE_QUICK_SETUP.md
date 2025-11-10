# Gate Auto-Close - Quick Setup

## ✅ Using Existing BaseTrigger!

Instead of creating a new trigger, I've updated the **existing** `/Base/BaseTrigger` to auto-close the gate when you leave the base during a run.

---

## 🎯 What It Does

When you start a run and walk out of the base area, the gate automatically closes behind you after 1 second, preventing you from returning to the pre-run menu. You're committed to the run!

---

## ⚙️ Setup (30 seconds)

### 1. Select BaseTrigger

In **Hierarchy**, select `/Base/BaseTrigger`

### 2. Configure in Inspector

```
┌─ Base Trigger (Script) ──────────────────┐
│                                           │
│ Detection Settings                        │
│   Base Direction: (-1, 0, -1)      [unchanged]
│                                           │
│ Gate Auto Close                    [NEW]  │
│   Gate: ⊙ (None - BaseGate)        [DRAG BaseGate HERE]
│   Auto Close Delay: 1              [GOOD]
│   Enable Auto Close: ✅             [CHECKED]
└───────────────────────────────────────────┘
```

**Action:**
- Drag `/Base/BaseGate` from Hierarchy → Drop on **Gate** field

### 3. Save Scene

Press **Ctrl+S** / **Cmd+S**

---

## ✅ Done!

That's it! The gate will now auto-close when you exit the base zone during a run.

---

## 🧪 Test It

1. **Play**
2. Walk to gate → Press **E**
3. Gate opens
4. Walk **out of base zone**
5. **Watch:**
   - Console: `Player exited base zone - closing gate in 1s...`
   - 1 second later
   - Console: `Gate auto-closed! Player cannot return to base during run.`
   - Gate closes (moves down)
6. Try walking back → **Blocked!** ✅

---

## 🎚️ Optional Settings

### Auto Close Delay
- **0.5** = Fast close
- **1.0** = Default (recommended)
- **2.0** = Slow close (more forgiving)

### Enable Auto Close
- **✅ Checked** = Auto-close enabled (recommended)
- **❌ Unchecked** = No auto-close (for testing)

---

## 📋 What Changed

**Updated File:**
- `/Assets/Scripts/Systems/BaseTrigger.cs`

**New Features:**
- Detects when player exits base zone (`OnTriggerExit`)
- Closes gate after configurable delay
- Only works during active runs
- Resets when run ends
- Preserves all original BaseTrigger functionality

**Not Used (Can Delete If Created):**
- `/Assets/Scripts/Systems/GatePassTrigger.cs` ❌
- `/Base/BaseGate/GatePassTrigger` GameObject ❌

---

## 🎉 Benefits

✅ Reuses existing BaseTrigger GameObject  
✅ Already positioned correctly  
✅ Simple 1-field setup  
✅ Preserves old functionality  
✅ Cleaner hierarchy  
✅ Less code to maintain  

---

**Full documentation:** `/Assets/Guide/GATE_AUTO_CLOSE_SETUP.md`
