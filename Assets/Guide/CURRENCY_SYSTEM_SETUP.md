# Split Currency System - Setup Guide

## Overview

The game now uses a **dual currency system** with persistent save functionality:

### 🟡 **Gold (In-Run Currency)**
- Earned during gameplay by killing enemies
- Used for placing obstacles (future feature)
- **LOST** at the end of each run
- Resets when starting a new run

### 🔵 **Souls (Meta-Currency)**
- Earned at the **end of runs** based on performance
- Used for **permanent upgrades** in the Base Shop
- **PERSISTS** between game sessions (saved to disk)
- Never lost

---

## 🆕 New Systems Created

### 1. **SaveSystem.cs** - Persistent Data Management
Handles saving and loading player progression.

**Features:**
- Saves meta-currency (Souls)
- Saves all upgrade levels
- Saves run statistics
- Auto-saves on game quit
- Uses JSON serialization
- Save location: `Application.persistentDataPath/savefile.json`

**Key Methods:**
```csharp
SaveSystem.Instance.AddMetaCurrency(amount)     // Add souls
SaveSystem.Instance.SpendMetaCurrency(amount)   // Spend souls
SaveSystem.Instance.GetMetaCurrency()           // Get current souls
SaveSystem.Instance.SaveGame()                  // Manual save
SaveSystem.Instance.LoadGame()                  // Manual load
SaveSystem.Instance.ResetSave()                 // Delete save
```

---

### 2. **PersistentData.cs** - Save Data Structure
Defines what gets saved between sessions.

**Saved Data:**
- Meta-currency (Souls)
- All 6 upgrade levels (Move Speed, Health, Damage, Crit Chance, Crit Damage, Attack Range)
- Total runs completed
- Total runs failed
- Total enemies killed
- Highest wave reached

---

### 3. **CurrencyManager.cs** - Dual Currency Handler
Manages both Gold and Souls separately.

**Key Methods:**
```csharp
// Gold (In-Run Currency)
CurrencyManager.Instance.AddGold(amount)
CurrencyManager.Instance.SpendGold(amount)
CurrencyManager.Instance.ResetGold()           // Called at run start
CurrencyManager.Instance.Gold                  // Current gold

// Souls (Meta-Currency)
CurrencyManager.Instance.AddMetaCurrency(amount)
CurrencyManager.Instance.SpendMetaCurrency(amount)
CurrencyManager.Instance.MetaCurrency          // Current souls
```

**Events:**
- `OnGoldChanged` - Triggered when gold changes
- `OnMetaCurrencyChanged` - Triggered when souls change

---

## 🔄 Modified Systems

### **PlayerStats.cs**
- Now loads upgrade levels from save file on start
- Saves upgrade levels after every purchase
- Upgrades persist between game sessions

### **UpgradeShop.cs**
- Now uses **Souls (meta-currency)** instead of gold
- Upgrades are permanent and persist

### **EnemyHealth.cs**
- Enemies drop **Gold** on death
- Tracks enemy kills for statistics

### **GameProgressionManager.cs**
- Added run tracking (waves completed, enemies killed)
- Added `OnRunComplete()` method
- Calculates and awards Souls at end of run based on:
  - Gold earned (50% conversion)
  - Waves completed (10 souls per wave)
  - Zones held (bonus for holding zone 1)
  - Victory bonus (200 souls)

### **GameManager.cs**
- Calls `OnRunComplete(false)` when player dies
- Increased restart delay to show rewards

### **CurrencyDisplay.cs**
- Now displays **both** Gold and Souls
- Requires 2 TextMeshPro components
- Can toggle which currency to show

### **SimpleShopUI.cs**
- Now shows **Souls** instead of generic currency
- Connected to CurrencyManager events

---

## 🎮 Scene Setup Instructions

### Step 1: Add SaveSystem to Scene

1. Create empty GameObject named `SaveSystem`
2. Add `SaveSystem.cs` component
3. Place in `GameManagers` parent object
4. **Mark as DontDestroyOnLoad** (already handled in script)

### Step 2: Add CurrencyManager to Scene

1. Create empty GameObject named `CurrencyManager`
2. Add `CurrencyManager.cs` component
3. Place in `GameManagers` parent object

### Step 3: Update GameCanvas Currency Display

#### Option A: Show Both Currencies (Recommended)
1. Find `GameCanvas/CurrencyText` GameObject
2. Rename to `CurrencyDisplay`
3. Add **second** TextMeshPro component as child:
   - Child 1: `GoldText` - Shows in-run gold
   - Child 2: `SoulsText` - Shows meta-currency
4. Update `CurrencyDisplay` component:
   - Assign `GoldText` to Gold Text field
   - Assign `SoulsText` to Souls Text field
   - Check both `Show Gold` and `Show Souls`

#### Option B: Show Only One Currency
- For gameplay HUD: Show only Gold
- For shop UI: Show only Souls

**Example Setup:**
```
GameCanvas
├── HealthBar
├── WaveText
├── TimerText
├── CurrencyDisplay           (CurrencyDisplay.cs component)
│   ├── GoldText (TMP)       ← Assign to Gold Text field
│   └── SoulsText (TMP)      ← Assign to Souls Text field
```

### Step 4: Update PlayerStats

**PlayerStats now automatically loads from save!**

No changes needed - it will:
1. Load upgrade levels from save on Start
2. Save after every upgrade purchase
3. Apply stats to player automatically

### Step 5: Test Save System

**To verify save system is working:**

1. **Start Play Mode**
2. **Kill enemies** → Earn gold
3. **Return to base** → Buy upgrades with souls (you start with 0, need to complete a run first)
4. **Complete or fail a run** → Souls awarded based on performance
5. **Stop Play Mode**
6. **Start Play Mode again** → Upgrades and souls should persist!

**To check save file location:**
- Look at Console when game starts
- Shows path: `Save file path: <path>/savefile.json`
- You can manually edit this file for testing

---

## 💰 Meta-Currency Reward Formula

When a run ends (player death or victory):

```
Base Reward = Gold Earned ÷ 2
Wave Bonus = Waves Completed × 10
Zone Bonus = 100 (Zone 1) / 50 (Zone 2) / 25 (Zone 3)
Victory Bonus = 200 (if won) / 0 (if lost)

Total Souls = Base Reward + Wave Bonus + Zone Bonus + Victory Bonus
Minimum: 10 souls
```

**Example:**
- Earned 500 gold
- Completed 5 waves
- Held Zone 1
- Lost the run

```
Base = 500 ÷ 2 = 250
Wave Bonus = 5 × 10 = 50
Zone Bonus = 100
Victory Bonus = 0
Total = 400 Souls
```

---

## 🧪 Testing Checklist

### Basic Functionality
- [ ] SaveSystem loads on game start
- [ ] CurrencyManager displays both currencies
- [ ] Enemies drop gold when killed
- [ ] Gold increases in UI
- [ ] Shop shows souls (not gold)
- [ ] Can purchase upgrades with souls
- [ ] Upgrades persist after restart
- [ ] Souls persist after restart

### Save/Load Testing
- [ ] Save file created on first run
- [ ] Upgrades saved after purchase
- [ ] Souls saved when earned
- [ ] Data loads correctly on game restart
- [ ] Stats transfer correctly from save

### Economy Testing
- [ ] Gold resets each run
- [ ] Souls never reset
- [ ] Rewards scale with performance
- [ ] Can't buy with insufficient souls
- [ ] Shop deducts souls correctly

### Edge Cases
- [ ] Starting fresh (no save file)
- [ ] Corrupted save file handling
- [ ] Very high currency values
- [ ] Multiple rapid purchases

---

## 🐛 Troubleshooting

### "Souls don't persist after restart"
- Check Console for save file path
- Verify save file exists at that location
- Check for errors during save/load
- Ensure SaveSystem is DontDestroyOnLoad

### "Can't buy upgrades"
- Check if you have enough Souls (not Gold!)
- Verify CurrencyManager exists in scene
- Check Console for "Not enough souls" message
- Try earning souls by completing a run first

### "Gold shows 0 at run start"
- This is correct! Gold resets each run
- Souls are what persist, not gold

### "Save file won't load"
- Delete `savefile.json` manually and restart
- Check for JSON parsing errors in Console
- Verify SaveSystem.LoadGame() is called on Start

### "Upgrades don't save"
- Ensure SaveSystem exists before PlayerStats.Start()
- Check that SaveUpgradeLevels() is called after each upgrade
- Verify SaveSystem.Instance is not null

---

## 🎯 Next Steps

Now that currency is split and saving:

1. **✅ DONE** - Dual currency system
2. **✅ DONE** - Persistent save system
3. **✅ DONE** - Meta-currency rewards
4. **NEXT** - Experience & Leveling system
5. **NEXT** - Obstacle placement system (uses Gold)

---

## 📝 Design Notes

### Why Split Currency?

**Gold (In-Run):**
- Creates tension during runs
- Encourages aggressive play for rewards
- Used for tactical obstacles
- Risk/reward decisions (save gold or spend?)

**Souls (Meta):**
- Long-term progression
- Feels rewarding even after losing
- Permanent power increases
- Encourages "one more run"

### Balance Considerations

**If players earn too many Souls too fast:**
- Reduce conversion rate (0.5 → 0.25)
- Reduce wave bonus (10 → 5)
- Increase upgrade costs

**If progression feels too slow:**
- Increase conversion rate (0.5 → 0.75)
- Add first-time bonuses
- Add achievement rewards

---

**Document Version:** 1.0  
**Last Updated:** Currency System Implementation  
**Next Update:** After experience system added
