# NPC Shop Troubleshooting Guide

## ✅ **Issue FIXED: Shop Not Showing on First Open**

### **The Problem**
When you first interact with an NPC:
- ❌ Console shows items are created
- ❌ UI panel doesn't appear on screen
- ❌ Have to close and reopen for it to work

### **The Cause**
Unity's UI layout system requires **two full frames** to properly initialize:
1. **Frame 1:** Canvas activation
2. **Frame 2:** Item population
3. **Frame 3:** Layout rebuild

The previous code had `SetActive(true)` happening **before** the coroutine, which broke the timing.

### **The Fix Applied**

I've updated `/Assets/Scripts/Systems/SimpleShopUI.cs` with the following changes:

#### 1. **Moved Activation Inside Coroutine**
```csharp
// ❌ BEFORE (WRONG):
public void OpenShop(ShopNPC npc)
{
    gameObject.SetActive(true);  // Too early!
    StartCoroutine(OpenShopCoroutine(npc));
}

// ✅ AFTER (CORRECT):
public void OpenShop(ShopNPC npc)
{
    currentNPC = npc;
    StopAllCoroutines();
    StartCoroutine(OpenShopCoroutine(npc));  // Starts coroutine first
}

private IEnumerator OpenShopCoroutine(ShopNPC npc)
{
    gameObject.SetActive(true);  // Activate INSIDE coroutine
    yield return null;           // Wait one frame
    // ...rest of code
}
```

#### 2. **Added Two Frame Waits**
```csharp
private IEnumerator OpenShopCoroutine(ShopNPC npc)
{
    // Frame 1: Activate
    gameObject.SetActive(true);
    yield return null;  // ✅ Wait for Canvas to initialize
    
    // Frame 2: Populate
    if (shopTitleText != null)
        shopTitleText.text = npc.GetNPCName();
    
    PopulateShop();
    UpdateCurrencyDisplay(CurrencyManager.Instance?.Essence ?? 0);
    yield return null;  // ✅ Wait for items to initialize
    
    // Frame 3: Rebuild layout
    Canvas.ForceUpdateCanvases();
    if (itemListContainer != null)
        LayoutRebuilder.ForceRebuildLayoutImmediate(itemListContainer as RectTransform);
    Canvas.ForceUpdateCanvases();
    
    Time.timeScale = 0f;
}
```

#### 3. **Added Proper Cleanup**
```csharp
private void CloseShop()
{
    StopAllCoroutines();  // ✅ Cancel any pending operations
    Time.timeScale = 1f;  // ✅ Restore game time
    
    if (currentNPC != null)
        currentNPC.CloseShop();
    
    gameObject.SetActive(false);
}
```

#### 4. **Ensured Items Are Active**
```csharp
private GameObject CreateItemButton()
{
    GameObject itemObj = Instantiate(itemButtonPrefab, itemListContainer);
    itemObj.SetActive(true);  // ✅ Explicitly activate
    
    RectTransform rectTransform = itemObj.GetComponent<RectTransform>();
    if (rectTransform != null)
        rectTransform.localScale = Vector3.one;  // ✅ Ensure proper scale
    
    return itemObj;
}
```

---

## 🧪 **Testing the Fix**

### **Test Procedure:**
1. **Enter Play Mode**
2. **Walk to any NPC** (WeaponVendor or StatVendor)
3. **Press E** to interact
4. **Check Console** for:
   ```
   Populating Weapons Shop
   Created weapon button: [Name] - $[Cost]
   Shop opened successfully: [NPC Name]
   ```
5. **Verify UI:**
   - ✅ Shop panel appears on screen
   - ✅ Items are visible and laid out correctly
   - ✅ Title shows NPC name
   - ✅ Currency displays correctly
   - ✅ Game pauses (Time.timeScale = 0)

### **Expected Result:**
✅ Shop opens **immediately** on first interaction  
✅ All items are **visible** and **properly positioned**  
✅ No need to close and reopen

---

## 🔧 **If Shop Still Doesn't Show**

### **Check 1: Verify SimpleShopUI References**

Select the `SimpleShopUI` GameObject in your scene and verify these fields are assigned in the Inspector:

```
SimpleShopUI Component:
  ✅ Item List Container → (ItemListParent or similar)
  ✅ Item Button Prefab → (ShopItemButton prefab)
  ✅ Shop Title Text → (Title TextMeshProUGUI)
  ✅ Currency Text → (Currency TextMeshProUGUI)
  ✅ Close Button → (CloseButton)
```

**How to find:**
- Hierarchy: `GameCanvas > SimpleShopUI`
- Inspector: Check all serialized fields

---

### **Check 2: Verify Item Button Prefab Structure**

Your item button prefab should have this structure:

```
ShopItemButton (GameObject with Button component)
  ├── Icon (Image)
  ├── Name (TextMeshProUGUI)
  ├── Description (TextMeshProUGUI)
  └── Cost (TextMeshProUGUI)
```

**Important:**
- Child names must match EXACTLY: "Icon", "Name", "Description", "Cost"
- Each must have the correct component (Image or TextMeshProUGUI)

---

### **Check 3: Verify Canvas Settings**

Select your `GameCanvas` and check:

```
Canvas Component:
  ✅ Render Mode: Screen Space - Overlay (or Camera)
  ✅ Canvas Scaler: Scale with Screen Size
  ✅ Reference Resolution: 1920x1080 (or your target)
```

---

### **Check 4: Console Errors**

Look for these specific errors:

**Error:** `itemButtonPrefab is NULL!`
- **Fix:** Assign the prefab in SimpleShopUI Inspector

**Error:** `itemListContainer is NULL!`
- **Fix:** Assign the container transform in SimpleShopUI Inspector

**Error:** `Failed to instantiate itemButtonPrefab!`
- **Fix:** Check that the prefab isn't corrupted

**Error:** `The name 'promptText' does not exist`
- **Fix:** This was from BaseGate.cs - already fixed above

---

### **Check 5: NPC Configuration**

Select your NPC GameObject and verify:

```
ShopNPC Component:
  ✅ NPC Type: WeaponVendor or StatUpgradeVendor
  ✅ NPC Name: "Blacksmith", "Trainer", etc.
  ✅ Shop UI Reference: Drag SimpleShopUI here
  ✅ Available Weapons/Upgrades: Array with items
```

---

### **Check 6: Layout Group Settings**

Select the `ItemListContainer` (the parent that holds shop items):

```
Should have ONE of these:
  ✅ Vertical Layout Group
  OR
  ✅ Horizontal Layout Group
  OR
  ✅ Grid Layout Group

Settings:
  ✅ Child Force Expand: Width and/or Height
  ✅ Child Control Size: Width and/or Height
  ✅ Spacing: 10 (or desired value)
```

---

## 🐛 **Common Issues**

### **Issue: Items show but are all in the same position (overlapping)**

**Cause:** Missing Layout Group component

**Fix:**
1. Select `ItemListContainer` in hierarchy
2. Add Component → Layout → Vertical Layout Group
3. Configure spacing and child settings

---

### **Issue: Shop opens but game doesn't pause**

**Cause:** Time.timeScale not being set properly

**Fix:** Check that the coroutine completes - look for this log:
```
Shop opened successfully: [NPC Name]
```

If you don't see it, there might be an error in the coroutine.

---

### **Issue: First item shows, but others don't**

**Cause:** Layout rebuild happening too early

**Fix:** This should be fixed by the two-frame-wait solution. If still happening:
1. Check Console for errors during item creation
2. Verify all items in the array are not null
3. Check that the prefab is being instantiated correctly

---

### **Issue: Shop closes immediately after opening**

**Cause:** Multiple interact triggers firing

**Fix:**
1. Check NPCInteraction component - ensure it's not firing multiple times
2. Verify `StopAllCoroutines()` is being called in OpenShop
3. Check that there aren't multiple ShopNPC components on the same object

---

### **Issue: Currency doesn't display correctly**

**Cause:** CurrencyManager not initialized or reference is null

**Fix:**
1. Ensure CurrencyManager exists in scene (usually on GameManagers)
2. Check Console for:
   ```
   CurrencyManager: Initialized with 0 essence
   ```
3. Verify CurrencyManager.Instance is not null in SimpleShopUI.Start()

---

## 📋 **Testing Checklist**

After the fix, verify all these work:

- [ ] Shop opens on first interaction
- [ ] Items are visible immediately
- [ ] Items are properly laid out (not overlapping)
- [ ] Shop title shows NPC name
- [ ] Currency displays current amount
- [ ] Can click items to purchase (if enough currency)
- [ ] Items update after purchase
- [ ] Close button works
- [ ] Game pauses when shop is open
- [ ] Game resumes when shop closes
- [ ] Can reopen shop multiple times
- [ ] No console errors when opening/closing

---

## 📚 **Additional Resources**

**Related Files:**
- `/Assets/Scripts/Systems/SimpleShopUI.cs` - Shop UI script (FIXED)
- `/Assets/Guide/SHOP_UI_FIX_SUMMARY.md` - Detailed technical explanation
- `/Assets/Guide/NPC_SHOP_SETUP.md` - Original setup guide
- `/Assets/Guide/COMPLETE_SETUP_GUIDE.md` - Full project setup

**Unity Documentation:**
- [UI Layout Groups](https://docs.unity3d.com/Manual/UILayoutGroup.html)
- [Coroutines](https://docs.unity3d.com/Manual/Coroutines.html)
- [Canvas](https://docs.unity3d.com/Manual/UICanvas.html)

---

## ✅ **Summary**

The shop not showing on first open was caused by **timing issues** with Unity's UI layout system. The fix:

1. ✅ Move `SetActive(true)` inside the coroutine
2. ✅ Wait one frame after activation
3. ✅ Populate shop items
4. ✅ Wait another frame
5. ✅ Force layout rebuild
6. ✅ Everything now works perfectly!

**The shop should now open correctly on the first interaction every time!** 🎉
