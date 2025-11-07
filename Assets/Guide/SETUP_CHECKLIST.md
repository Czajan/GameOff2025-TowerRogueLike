# Currency System - Setup Checklist

Use this checklist to set up the new dual currency system in your scene.

---

## ✅ Step-by-Step Setup

### 1. Add SaveSystem (REQUIRED)

**Location:** `GameManagers` GameObject

- [ ] Create new empty GameObject
- [ ] Name it: `SaveSystem`
- [ ] Add component: `SaveSystem.cs`
- [ ] Verify it's a child of `GameManagers`

**Expected Console Output on Play:**
```
Save file path: C:/Users/.../AppData/LocalLow/.../savefile.json
Game loaded successfully! Meta-currency: 0
```

---

### 2. Add CurrencyManager (REQUIRED)

**Location:** `GameManagers` GameObject

- [ ] Create new empty GameObject
- [ ] Name it: `CurrencyManager`
- [ ] Add component: `CurrencyManager.cs`
- [ ] Verify it's a child of `GameManagers`

**Hierarchy should look like:**
```
GameManagers
├── SaveSystem           (SaveSystem.cs)
├── CurrencyManager      (CurrencyManager.cs)
├── GameManager          (existing)
├── WaveController       (existing)
└── GameProgressionManager (existing)
```

---

### 3. Update Currency Display UI

**Location:** `GameCanvas`

#### Option A: Side-by-Side Display (Recommended)

- [ ] Find `GameCanvas/CurrencyText` GameObject
- [ ] Duplicate it (Ctrl+D)
- [ ] Rename originals:
  - First one: `GoldText`
  - Second one: `SoulsText`
- [ ] Position them next to each other
- [ ] Create parent GameObject: `CurrencyDisplay`
- [ ] Move both texts under `CurrencyDisplay`
- [ ] Select `CurrencyDisplay`
- [ ] Add component: `CurrencyDisplay.cs`
- [ ] Assign fields:
  - `Gold Text` → `GoldText` TextMeshProUGUI component
  - `Souls Text` → `SoulsText` TextMeshProUGUI component
  - Check `Show Gold` ✓
  - Check `Show Souls` ✓

**Hierarchy should look like:**
```
GameCanvas
├── HealthBar
├── WaveText
├── TimerText
├── CurrencyDisplay           (CurrencyDisplay.cs)
│   ├── GoldText (TMP)       ← Set Yellow/Gold color
│   └── SoulsText (TMP)      ← Set Cyan/Blue color
└── ShopPanel
```

#### Option B: Single Display (Alternative)

If you want to show only one currency at a time:

**For Gameplay HUD (show Gold only):**
- [ ] Set `Show Gold` = ✓
- [ ] Set `Show Souls` = ✗

**For Shop UI (show Souls only):**
- [ ] Set `Show Gold` = ✗
- [ ] Set `Show Souls` = ✓

---

### 4. Verify Shop UI Updates (AUTO-UPDATED)

**No manual changes needed!** The shop automatically updated to use Souls.

**Location:** `GameCanvas/ShopPanel`

- [ ] Find `ShopCurrencyText` TextMeshProUGUI
- [ ] Verify it exists (it should already be there)

The `SimpleShopUI.cs` component now automatically shows "Souls: X" instead of "Currency: $X"

---

### 5. Verify Scene References (IMPORTANT)

Check that existing systems still have their references:

**GameManager:**
- [ ] Has reference to `PlayerHealth`
- [ ] Has reference to `WaveSpawner`

**GameProgressionManager:**
- [ ] Has reference to `DefenseZones` array (or auto-finds them)
- [ ] Timer settings configured

**WaveSpawner:**
- [ ] Has `Enemy` prefab assigned
- [ ] Has spawn settings configured
- [ ] Has reference to `WaveController` (or auto-finds it)

---

### 6. Test Basic Functionality

#### Test 1: Save System Initialization
- [ ] Enter Play Mode
- [ ] Check Console for: "Save file path: ..."
- [ ] Check Console for: "Game loaded successfully! Meta-currency: 0"
- [ ] No errors in Console

#### Test 2: Gold Earning
- [ ] Kill an enemy
- [ ] Check Console for: "+10 Gold! Total: 10" (or similar)
- [ ] Verify Gold display in UI increases
- [ ] No errors in Console

#### Test 3: Soul Earning
- [ ] Complete a run (die or win)
- [ ] Check Console for: "+X Souls (Meta-Currency)! Total: X"
- [ ] Note the souls earned
- [ ] Exit Play Mode
- [ ] Re-enter Play Mode
- [ ] Check Console - should load the saved souls
- [ ] Verify souls persist

#### Test 4: Upgrade Purchasing
- [ ] Earn some souls (complete/fail a run)
- [ ] Return to base
- [ ] Open shop (talk to NPC)
- [ ] Verify shop shows "Souls: X" (not "Currency: $X")
- [ ] Purchase an upgrade
- [ ] Check Console for: "Purchased ... for X souls!"
- [ ] Exit Play Mode
- [ ] Re-enter Play Mode
- [ ] Verify upgrade level persisted

#### Test 5: Gold Reset
- [ ] Enter Play Mode
- [ ] Earn some gold (kill enemies)
- [ ] Note gold amount
- [ ] Die or complete run
- [ ] Start new run (return to base, exit)
- [ ] Verify gold reset to 0
- [ ] Verify souls did NOT reset

---

### 7. Visual Feedback (OPTIONAL BUT RECOMMENDED)

Make currencies visually distinct:

**Gold Text:**
- [ ] Set color to Yellow/Gold (#FFD700)
- [ ] Set font size to 24-32
- [ ] Position: Top-left or top-right

**Souls Text:**
- [ ] Set color to Cyan/Blue (#00FFFF)
- [ ] Set font size to 24-32
- [ ] Position: Next to Gold text

**Example Colors:**
```
Gold:  R=255, G=215, B=0   (Yellow/Gold)
Souls: R=0,   G=255, B=255 (Cyan)
```

---

## 🐛 Troubleshooting

### Issue: "NullReferenceException: SaveSystem.Instance"

**Solution:**
- Ensure SaveSystem GameObject exists in scene
- Verify SaveSystem.cs component is attached
- Check that SaveSystem initializes before other systems
- SaveSystem must be in scene from the start (not spawned later)

---

### Issue: "Currencies not displaying"

**Solution:**
- Check that CurrencyDisplay.cs has both text fields assigned
- Verify "Show Gold" and "Show Souls" are checked
- Check that TextMeshProUGUI components exist
- Look for errors in Console

---

### Issue: "Shop still shows 'Currency' not 'Souls'"

**Solution:**
- SimpleShopUI.cs should be updated automatically
- If not, verify you're using the updated script
- Check `UpdateCurrencyDisplay()` method uses "Souls: {currency}"

---

### Issue: "Souls don't persist after restart"

**Solution:**
- Check Console for save file path
- Verify file exists at that location
- Look for save/load errors in Console
- Try manually deleting save file and restarting

---

### Issue: "Gold doesn't reset at run start"

**Solution:**
- Verify CurrencyManager.ResetGold() is called
- Check GameProgressionManager.Start() calls ResetGold()
- Look for errors in Console

---

## 📋 Final Verification

Before considering setup complete:

- [ ] No errors in Console
- [ ] Gold increases when killing enemies
- [ ] Souls earned at end of run
- [ ] Souls persist after game restart
- [ ] Upgrades persist after game restart
- [ ] Gold resets each run
- [ ] Shop uses souls (not gold)
- [ ] Both currencies display correctly

---

## 🎉 Success!

If all checkboxes are checked, your currency system is fully set up!

**Next Steps:**
1. Balance meta-currency rewards
2. Implement Experience & Leveling system
3. Implement Obstacle placement (uses Gold)

---

**Need Help?** Check these documents:
- `CURRENCY_SYSTEM_SETUP.md` - Detailed setup guide
- `CURRENCY_SYSTEM_SUMMARY.md` - Quick reference
- `IMPLEMENTATION_STATUS.md` - Overall project status
