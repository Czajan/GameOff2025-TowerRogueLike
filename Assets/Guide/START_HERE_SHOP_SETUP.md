# 🚀 START HERE - Shop System Setup

**Welcome!** Your roguelike prototype is almost ready to test. All scripts are compiled and working. You just need to set up the UI.

---

## 🎯 What's Already Done

✅ All core gameplay systems (movement, combat, waves, enemies)  
✅ Currency and progression managers  
✅ Weapon and upgrade ScriptableObjects (11 assets created)  
✅ NPC shop system with 4 vendors  
✅ Shop UI scripts compiled (SimpleShopUI, DebugShopTester)  
✅ All compilation errors fixed  

---

## 🛠️ What You Need to Do

You need to **build the shop UI** and **connect the references**. This takes about 15-20 minutes.

---

## 📋 Quick Setup (Minimum Viable)

Follow these 8 steps to get the shop working:

### 1. Create ItemButton Prefab (5 min)

**What:** A button template for displaying shop items

**How:**
1. In Hierarchy, right-click `GameCanvas/ShopPanel` → UI → Button - TextMeshPro
2. Rename to `ItemButton`
3. Set Width: 380, Height: 80
4. Right-click `ItemButton` → UI → Text - TextMeshPro
   - Rename to `Name`, set text "Item Name", size 18, anchor Top-Stretch
5. Right-click `ItemButton` → UI → Text - TextMeshPro
   - Rename to `Description`, set text "Description", size 12, anchor Middle-Stretch
6. Right-click `ItemButton` → UI → Text - TextMeshPro
   - Rename to `Cost`, set text "$100", size 16, yellow color, anchor Bottom-Right
7. Drag `ItemButton` from Hierarchy → `/Assets/Prefabs/UI/ItemButton.prefab`
8. Delete `ItemButton` from Hierarchy

**Result:** ✅ ItemButton.prefab created

---

### 2. Create ScrollView Container (3 min)

**What:** A scrollable area to display shop items

**How:**
1. Right-click `GameCanvas/ShopPanel` → UI → Scroll View
2. Rename to `ItemScrollView`
3. Set Anchor: Stretch-Stretch, Margins: 20 (all sides), Top: 80, Bottom: 60
4. Select `ItemScrollView/Viewport/Content`
   - Add Component → **Vertical Layout Group**
   - Set Spacing: 10, Padding: 10 (all sides)
   - Add Component → **Content Size Fitter**
   - Set Vertical Fit: **Preferred Size**
5. Delete `ItemScrollView/Scrollbar Horizontal`
6. In `ItemScrollView` component, set Horizontal: false

**Result:** ✅ Scrollable container ready

---

### 3. Add Shop UI Text Elements (3 min)

**What:** Title and currency display for the shop

**How:**
1. Right-click `GameCanvas/ShopPanel` → UI → Text - TextMeshPro
   - Rename to `ShopTitleText`
   - Text: "Shop", Size: 32, Bold, White
   - Anchor: Top-Center, Position: (0, -30, 0)

2. Right-click `GameCanvas/ShopPanel` → UI → Text - TextMeshPro
   - Rename to `ShopCurrencyText`
   - Text: "Currency: $0", Size: 20, Yellow
   - Anchor: Top-Right, Position: (-20, -30, 0)

3. Right-click `GameCanvas/ShopPanel` → UI → Button - TextMeshPro
   - Rename to `CloseButton`
   - Text: "Close (ESC)", Size: 18
   - Width: 150, Height: 40
   - Anchor: Bottom-Center, Position: (0, 10, 0)

**Result:** ✅ Shop UI elements created

---

### 4. Configure ShopPanel Background (1 min)

**What:** Make the shop stand out visually

**How:**
1. Select `GameCanvas/ShopPanel`
2. In Image component:
   - Color: RGBA(0.1, 0.1, 0.1, 0.95) - Dark semi-transparent
3. In RectTransform:
   - Anchor: Stretch-Stretch
   - Margins: 100 (all sides)
4. **UNCHECK the checkbox** next to ShopPanel name (disable it)

**Result:** ✅ ShopPanel configured and disabled by default

---

### 5. Add SimpleShopUI Component (1 min)

**What:** The script that makes the shop work

**How:**
1. Select `GameCanvas/ShopPanel`
2. Add Component → **SimpleShopUI**
3. The component appears (if not, check Console for errors)

**Result:** ✅ SimpleShopUI component added

---

### 6. Assign SimpleShopUI References (2 min)

**What:** Connect the UI elements to the script

**How:**
1. Select `GameCanvas/ShopPanel`
2. In SimpleShopUI component, drag and drop:
   - **Item List Container:** Drag `ItemScrollView/Viewport/Content` from Hierarchy
   - **Item Button Prefab:** Drag `/Assets/Prefabs/UI/ItemButton.prefab` from Project
   - **Shop Title Text:** Drag `ShopTitleText` from Hierarchy (child of ShopPanel)
   - **Currency Text:** Drag `ShopCurrencyText` from Hierarchy (child of ShopPanel)
   - **Close Button:** Drag `CloseButton` from Hierarchy (child of ShopPanel)

**Result:** ✅ All references assigned

---

### 7. Connect NPCs to Shop UI (1 min)

**What:** Let NPCs open the shop when interacted with

**How:**
1. Select `/Base/NPCs/WeaponVendor` in Hierarchy
2. In **ShopNPC** component:
   - **Shop UI:** Drag `GameCanvas/ShopPanel` from Hierarchy

3. Select `/Base/NPCs/StatVendor` in Hierarchy
4. In **ShopNPC** component:
   - **Shop UI:** Drag `GameCanvas/ShopPanel` from Hierarchy

**Result:** ✅ NPCs can open shop

---

### 8. Add Debug Tester (2 min)

**What:** Keyboard shortcuts to test purchases easily

**How:**
1. Select `/GameManagers` in Hierarchy
2. Add Component → **DebugShopTester**
3. In component:
   - **Upgrades For Testing:** Set Size to 6
     - Drag all 6 upgrade assets from `/Assets/Data/Upgrades/`
   - **Weapons For Testing:** Set Size to 5
     - Drag all 5 weapon assets from `/Assets/Data/Weapons/`

**Test Controls:**
- **C** = Add 100 currency
- **1-6** = Buy upgrades directly
- **L** = Log current stats
- **H** = Show help

**Result:** ✅ Debug tools ready

---

## 🎮 Test It Now!

1. **Enter Play Mode**
2. Press **C** to add currency
3. Walk to the **Base** area (white plane)
4. Walk near **StatVendor** (the Trainer NPC cube)
5. Get close (within 3 units)
6. **Shop should open automatically!**
7. Click an upgrade to purchase
8. Press **L** to see stats increased

---

## ✅ Success Criteria

You should see:
- ✅ Shop panel opens when near NPC
- ✅ List of upgrades appears
- ✅ Currency displays correctly
- ✅ Can click to purchase
- ✅ Stats increase after purchase
- ✅ Currency deducts correctly

---

## 🐛 Troubleshooting

### Shop doesn't open

**Check:**
- ShopPanel has SimpleShopUI component
- Both NPCs have Shop UI assigned to ShopPanel
- ShopPanel is disabled by default (unchecked)

---

### No items in shop

**Check:**
- Item List Container assigned to `Content` GameObject
- Item Button Prefab assigned to `ItemButton.prefab`
- NPCs have weapons/upgrades assigned (check Inspector)

---

### Buttons don't work

**Check:**
- ItemButton prefab has Button component
- Close Button assigned in SimpleShopUI
- EventSystem exists in scene (should be automatic)

---

### "Missing Script" error

**Check:**
- Console for compilation errors (should be none now)
- All using statements present in scripts
- Scripts reimported (Assets → Refresh)

---

## 📚 Detailed Guides

If you need more detail:

- **`REMAINING_SETUP_TASKS.md`** - Full task list with all optional features
- **`SHOP_UI_HIERARCHY.md`** - Exact UI structure with visuals
- **`QUICK_TEST_CHECKLIST.md`** - Testing procedures
- **`SCRIPT_COMPILATION_CHECKLIST.md`** - Troubleshooting script issues

---

## 🚀 Next Steps After Testing

Once the shop works:

1. **Add HUD Currency Display:**
   - Add `CurrencyDisplay` component to `/GameCanvas/CurrencyText`
   - Currency shows on screen at all times

2. **Add Timer Display:**
   - Add `TimerDisplay` component to `/GameCanvas/TimerText`
   - Shows countdown when in base

3. **Create NPC Interaction Prompts:**
   - Shows "[E] Talk to Merchant" text above NPCs
   - See `REMAINING_SETUP_TASKS.md` Task 2.4

4. **Polish and Content:**
   - Add more weapons/upgrades
   - Improve visuals
   - Add audio
   - See Priority 5 in `REMAINING_SETUP_TASKS.md`

---

## 💡 Pro Tips

- **Test with DebugShopTester first** before building full UI
- **Press H in Play Mode** to see all debug controls
- **Use L key** to verify stats are changing
- **Check Console** if something doesn't work
- **Read error messages carefully** - they're specific

---

## 🎯 Summary

**Time Required:** 15-20 minutes  
**Difficulty:** Easy (mostly dragging references)  
**Payoff:** Fully working shop system  

**You're literally 8 steps away from testing your game!** 🎮

---

**Good luck!** If you get stuck, check the detailed guides or verify each step carefully.

The scripts are ready. The assets are ready. You just need to connect the UI! 🚀
