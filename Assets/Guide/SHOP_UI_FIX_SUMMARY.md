# Shop UI First-Open Fix

## Problem

The shop was opening successfully (console logs confirmed items were being created), but the UI was **not visible on screen** until the second time you opened it.

## Root Cause

Unity's UI layout system needs **one frame** to initialize and calculate positions/sizes after a GameObject is activated. We were trying to force layout updates in the same frame as activation, which doesn't work reliably.

**Timeline of what was happening:**

```
Frame 1 (First Open):
1. SetActive(true) ✓
2. PopulateShop() ✓         → Items created
3. ForceRebuildLayout() ✗   → Called too early, UI system not ready
4. Canvas not visible ✗

Frame 2 (Second Open):
1. SetActive(true) ✓         → Panel already active
2. PopulateShop() ✓         → Items created
3. ForceRebuildLayout() ✓   → NOW it works (UI system ready)
4. Canvas visible ✓
```

## Solution

Use a **coroutine** to wait one frame between activation and layout rebuild:

```csharp
public void OpenShop(ShopNPC npc)
{
    currentNPC = npc;
    StopAllCoroutines();
    StartCoroutine(OpenShopCoroutine(npc));
}

private IEnumerator OpenShopCoroutine(ShopNPC npc)
{
    gameObject.SetActive(true);
    PopulateShop();
    
    yield return null;  // ✅ Wait one frame!
    
    LayoutRebuilder.ForceRebuildLayoutImmediate(...);
    Canvas.ForceUpdateCanvases();
}
```

## Why This Works

**Frame 1:**
- Panel activates
- Items are instantiated
- Layout system begins calculating

**Frame 2 (after `yield return null`):**
- Layout system has finished initialization
- `ForceRebuildLayoutImmediate()` now works correctly
- Panel becomes visible with properly positioned items

## Technical Details

### The Issue

Unity's `LayoutRebuilder.ForceRebuildLayoutImmediate()` only works if:
1. The GameObject is active ✓
2. The Canvas has been updated at least once ✗ (not on first activation)
3. All parent RectTransforms are initialized ✗ (not on first activation)

When you activate a GameObject for the first time:
- `OnEnable()` gets called
- Layout groups initialize
- But **layout calculations happen at end of frame**

Calling `ForceRebuildLayoutImmediate()` in the same frame as `SetActive(true)` tries to force a rebuild before the layout system is ready.

### The Fix

`yield return null` waits until the next frame, which means:
- All `OnEnable()` calls have completed
- Layout system has initialized
- Canvas has been updated once
- NOW we can force an immediate rebuild

## Code Changes

**File:** `/Assets/Scripts/Systems/SimpleShopUI.cs`

**Added:**
```csharp
using System.Collections;  // For IEnumerator
```

**Changed:**
```csharp
// Before:
public void OpenShop(ShopNPC npc)
{
    gameObject.SetActive(true);
    PopulateShop();
    LayoutRebuilder.ForceRebuildLayoutImmediate(...);
}

// After:
public void OpenShop(ShopNPC npc)
{
    StopAllCoroutines();  // Cancel any in-progress opens
    StartCoroutine(OpenShopCoroutine(npc));
}

private IEnumerator OpenShopCoroutine(ShopNPC npc)
{
    gameObject.SetActive(true);
    PopulateShop();
    
    yield return null;  // Wait one frame
    
    LayoutRebuilder.ForceRebuildLayoutImmediate(...);
}
```

## Why `StopAllCoroutines()`?

If the player rapidly presses E to open/close the shop, we might have multiple coroutines running. `StopAllCoroutines()` ensures only one shop-open operation runs at a time.

## Testing

**Before Fix:**
1. Enter Play Mode
2. Walk to NPC
3. Press E
4. ❌ Console shows items created, but panel not visible
5. Press E to close, then E again to reopen
6. ✓ Now it's visible

**After Fix:**
1. Enter Play Mode
2. Walk to NPC
3. Press E
4. ✓ Panel visible immediately with all items!

## Performance Impact

**Minimal** - The coroutine adds a 1-frame delay (~16ms at 60fps), which is imperceptible to players and is the standard approach for UI initialization in Unity.

## Alternative Solutions Considered

### 1. Wait for End of Frame
```csharp
yield return new WaitForEndOfFrame();
```
**Rejected:** Same effect as `yield return null`, but less clear intent.

### 2. Multiple Force Updates
```csharp
Canvas.ForceUpdateCanvases();
Canvas.ForceUpdateCanvases();
Canvas.ForceUpdateCanvases();
```
**Rejected:** Doesn't solve the underlying timing issue, just spam.

### 3. Keep Panel Always Active
```csharp
// Never call SetActive(false), just move off-screen
```
**Rejected:** Wastes performance, prevents click-through blocking.

### 4. Use OnEnable() + Invoke
```csharp
void OnEnable()
{
    Invoke(nameof(RebuildLayout), 0.1f);
}
```
**Rejected:** Arbitrary delay, not frame-perfect, harder to debug.

## Related Unity Documentation

- [LayoutRebuilder.ForceRebuildLayoutImmediate](https://docs.unity3d.com/ScriptReference/UI.LayoutRebuilder.ForceRebuildLayoutImmediate.html)
- [Canvas.ForceUpdateCanvases](https://docs.unity3d.com/ScriptReference/Canvas.ForceUpdateCanvases.html)
- [Coroutines](https://docs.unity3d.com/Manual/Coroutines.html)

## Summary

✅ **Shop now opens correctly on first attempt**  
✅ **Items are visible immediately**  
✅ **Layout is properly calculated**  
✅ **1-frame delay is imperceptible**  

The fix uses Unity's recommended approach for deferred UI updates: coroutines with frame-based waiting.
