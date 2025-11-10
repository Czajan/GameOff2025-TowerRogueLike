# Gate Barrier Troubleshooting

## Problem: Player Can Still Walk Through Gate

The instant barrier isn't blocking the player because **CharacterController** (used by the player) has specific collision requirements.

---

## ✅ **Quick Fix: Set BaseGate to Ground Layer**

### Step 1: Set BaseGate Layer

1. **Select `/Base/BaseGate`** in Hierarchy
2. In Inspector, at the top, find **Layer** dropdown
3. Change from **Default** → **Ground**

```
┌─ BaseGate ────────────────┐
│ Tag: Untagged             │
│ Layer: Ground    ← SET THIS
└───────────────────────────┘
```

4. When prompted "Change layer for child objects too?", click **Yes, change children**

### Step 2: Test Again

1. **Enter Play Mode**
2. Press **E** to start run
3. Walk through gate
4. Turn around and try to walk back
5. **Expected:** You should be **BLOCKED** ✅

---

## Why This Works

**CharacterController** (player movement) uses Unity's physics system and only collides with:
- Colliders on certain layers (Ground, Default, etc.)
- **Static** colliders or colliders with proper layer setup

By putting the gate on the **Ground layer**, the CharacterController properly detects and collides with it.

---

## Verification Checklist

After setting the layer, verify in Play Mode:

### 1. Check Console Messages

Look for these messages when entering Play Mode:
```
BaseGate: Instant barrier set to Ground layer (6)
BaseGate: Created instant barrier (size=(6, 4, 1), offset=(0, 2, 0), enabled=False)
```

**Good:** Layer number should be 6 (Ground layer)  
**Bad:** If it says "Ground layer not found", create the Ground layer!

### 2. Check Hierarchy in Play Mode

```
/Base
  └── /BaseGate  (Layer: Ground)
      ├── /InteractionCanvas
      └── /InstantBarrier  ← Should appear in Play Mode
```

Select `/Base/BaseGate/InstantBarrier` in Play Mode and verify:
- **Layer:** Ground
- **Box Collider:**
  - Is Trigger: ❌ (unchecked)
  - Enabled: ✅ (when gate is closed)
  - Size: (6, 4, 1)

### 3. Test Collision in Scene View

1. **In Play Mode**, select `/Base/BaseGate/InstantBarrier`
2. In Scene view, you should see a **green wireframe box**
3. The box should cover the gate opening
4. Walk player into the box → Should be blocked

---

## Alternative Fix: Add GateColliderSetup Script

If changing layers manually doesn't work, use the helper script:

### 1. Add Component

1. Select `/Base/BaseGate`
2. **Add Component** → Search: `GateColliderSetup`
3. Add it

### 2. Verify Settings

```
┌─ Gate Collider Setup (Script) ─┐
│ Set Gate To Ground Layer: ✅   │
└─────────────────────────────────┘
```

This automatically sets the BaseGate to Ground layer when the scene loads.

---

## Common Issues

### Issue: "Ground layer not found"

**Solution:** Create the Ground layer

1. **Edit → Project Settings → Tags and Layers**
2. Find **Layers** section
3. Find an empty layer (e.g., Layer 6)
4. Name it: `Ground`
5. Click outside to save
6. Go back to BaseGate and set Layer to Ground

### Issue: Barrier not appearing in Play Mode

**Check:**
- `useInstantBarrier = ✅` in BaseGate Inspector
- No errors in Console
- BaseGate script is enabled

**Fix:**
- Make sure you're in **Play Mode** (barrier is created at runtime)
- Check Console for error messages

### Issue: Barrier exists but player phases through

**Possible causes:**
1. **Layer collision matrix disabled**
   - **Fix:** Edit → Project Settings → Physics
   - Make sure **Ground** layer collides with **Default** layer (check matrix)

2. **Player layer ignores Ground**
   - **Fix:** Select Player, check if Player layer is set
   - Make sure Player layer collides with Ground in Physics settings

3. **Barrier too small**
   - **Fix:** Increase barrier size:
   ```
   Barrier Size: (8, 5, 2)  ← Wider, taller, deeper
   ```

4. **CharacterController skin width too large**
   - **Fix:** Select Player
   - CharacterController → Skin Width: 0.05 (smaller value)

---

## Debug: Visualize the Barrier

### In Scene View (Play Mode):

1. Select `/Base/BaseGate/InstantBarrier`
2. You'll see the **green collider wireframe**
3. Position your camera to see if it covers the gate opening
4. The collider should be like a wall blocking the passage

### Add Visual Mesh (Temporary):

If you want to SEE the barrier while testing:

1. **In Play Mode**, select `/Base/BaseGate/InstantBarrier`
2. **Add Component** → **Mesh Filter**
3. Set Mesh to **Cube**
4. **Add Component** → **Mesh Renderer**
5. Assign any material

Now you'll see a visible cube blocking the gate! (Remove these components when done testing)

---

## Advanced: Layer Collision Matrix

Ensure Ground and Default layers collide:

1. **Edit → Project Settings → Physics**
2. Scroll down to **Layer Collision Matrix**
3. Find row: **Ground**
4. Find column: **Default**
5. **Check the box** at their intersection ✅

This ensures Ground layer objects collide with Default layer objects (player).

---

## Summary

**Quick Fix:**
1. Set `/Base/BaseGate` layer to **Ground**
2. Test in Play Mode

**If that doesn't work:**
1. Verify Ground layer exists
2. Check Physics layer collision matrix
3. Increase barrier size
4. Check console for error messages

**Still not working?**
- Post Console messages in Discord
- Share screenshot of BaseGate Inspector
- Share screenshot of Player CharacterController settings
