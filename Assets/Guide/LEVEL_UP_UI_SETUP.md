# Level-Up Upgrade Selection UI - Setup Guide

## ✅ What's Been Updated

### **1. LevelUpUI Script Updated**
- **Location**: `/Assets/Scripts/Systems/LevelUpUI.cs`
- **Changes**:
  - Now listens to `UpgradeSystem.OnUpgradesOffered` event instead of old system
  - Displays `LevelUpgradeData` ScriptableObjects with rarity colors
  - Shows stat bonuses and stack counts
  - Pauses game (Time.timeScale = 0) during selection
  - Resumes game after choice is made

### **2. Integration Flow**
```
Player gains XP → ExperienceSystem detects level-up
    ↓
PlayerStats.OnPlayerLevelUp(level)
    ↓
If level % 5 == 0:
    UpgradeSystem.OfferUpgrades() → selects 3 random upgrades by rarity
    ↓
    Fires OnUpgradesOffered event
    ↓
    LevelUpUI.ShowUpgradeChoices(upgrades)
    ↓
    UI appears, Time.timeScale = 0
    ↓
Player clicks an upgrade
    ↓
    UpgradeSystem.SelectUpgrade(upgrade) → applies bonuses to PlayerStats
    ↓
    UI closes, Time.timeScale = 1
Else:
    Auto-apply: +2 Damage, +10 HP, +2% Speed
```

---

## 🔧 **Scene Setup Required**

### **Step 1: Assign References in LevelUpPanel**

1. **Select GameObject**: `/GameCanvas/LevelUpPanel` in the Hierarchy
2. **In Inspector**, find the `LevelUpUI` component
3. **Assign the following references**:
   - **Level Up Panel**: Drag `/GameCanvas/LevelUpPanel` itself
   - **Level Title Text**: Drag `/GameCanvas/LevelUpPanel/LevelUpTitle`
   - **Options Container**: Drag `/GameCanvas/LevelUpPanel/OptionsContainer`
   - **Option Button Prefab**: Drag `/Assets/Prefabs/UI/ItemButton.prefab`

### **Step 2: Verify LevelUpPanel Hierarchy**

Your `/GameCanvas/LevelUpPanel` should have this structure:

```
/GameCanvas
  /LevelUpPanel (LevelUpUI component, Image, inactive by default)
    /LevelUpTitle (TextMeshProUGUI)
    /OptionsContainer (RectTransform, VerticalLayoutGroup recommended)
```

**If OptionsContainer doesn't exist:**
1. Right-click `/LevelUpPanel` → Create Empty
2. Rename to `OptionsContainer`
3. Add Component → Layout Group → **Vertical Layout Group**
4. Set:
   - Child Alignment: Upper Center
   - Spacing: 15
   - Child Force Expand: Width = ✓, Height = ✗
   - Padding: Left=10, Right=10, Top=10, Bottom=10

### **Step 3: Verify ItemButton Prefab Structure**

Open `/Assets/Prefabs/UI/ItemButton.prefab` and ensure it has:
```
ItemButton (Button, Image)
  /Name (TextMeshProUGUI) ← upgrade name displays here
  /Description (TextMeshProUGUI) ← description + bonuses display here
```

**The UI script looks for these exact child names!**

### **Step 4: Assign Upgrade Pool to UpgradeSystem**

1. **Select**: `/GameManagers` GameObject
2. **Find**: `UpgradeSystem` component in Inspector
3. **Upgrade Pool**: Set size to 15 (or however many you created)
4. **Drag all assets** from `/Assets/Data/LevelUpgrades/` into the slots:
   - AirWalker
   - BerserkRage
   - CriticalMastery
   - Executioner
   - Fortress
   - GlassCannonI
   - IronSkin
   - PowerSurge
   - ShadowStep
   - Sharpshooter
   - SwiftStrike
   - VampiricTouch
   - Velocity
   - Vitality
   - Whirlwind

5. **Rarity Weights** (optional tuning):
   - Common Weight: 70
   - Rare Weight: 25
   - Legendary Weight: 5

---

## 🎨 **Visual Customization**

### **Rarity Background Colors**
The UI automatically colors upgrade buttons by rarity:
- **Common**: Dark gray `rgba(51, 51, 51, 0.9)`
- **Rare**: Blue `rgba(26, 77, 153, 0.9)`
- **Legendary**: Gold `rgba(153, 102, 0, 0.9)`

These are set in `LevelUpUI.GetRarityBackgroundColor()` - edit if desired.

### **Rarity Name Colors**
Defined in `LevelUpgradeData.GetRarityColor()`:
- **Common**: `#CCCCCC` (light gray)
- **Rare**: `#4A90E2` (blue)
- **Legendary**: `#FFD700` (gold)

---

## 🧪 **Testing the System**

### **Quick Test**
1. Enter Play Mode
2. Start a run
3. Kill enemies to gain XP
4. At level 5, the LevelUpPanel should appear
5. You should see 3 random upgrades with colored names and backgrounds
6. Click one to select it
7. Panel closes, game resumes
8. Your stats should have increased

### **Debug Console Messages**
When working correctly, you'll see:
```
<color=yellow>★ MILESTONE LEVEL 5! Awaiting player upgrade choice...</color>
UpgradeSystem: Offering 3 upgrades
★ Selected: Power Surge (Stack 1)
```

### **Common Issues**

**Issue**: Panel doesn't appear at level 5
- **Check**: Is `UpgradeSystem` on `/GameManagers`?
- **Check**: Are upgrades assigned to `Upgrade Pool`?
- **Check**: Is `LevelUpPanel` GameObject active in scene? (It should start inactive, script activates it)

**Issue**: No upgrades showing in panel
- **Check**: Is `Option Button Prefab` assigned?
- **Check**: Is `Options Container` assigned?
- **Check**: Does ItemButton prefab have "Name" and "Description" children?

**Issue**: Can't click upgrades
- **Check**: Does ItemButton have a `Button` component?
- **Check**: Is there a GraphicRaycaster on the Canvas?
- **Check**: Is there an EventSystem in the scene?

**Issue**: Game doesn't unpause after selection
- **Check**: Console for errors during selection
- **Solution**: Script should set `Time.timeScale = 1f` - verify no exceptions

---

## 📝 **How Upgrade Selection Works**

### **Rarity-Weighted Selection Algorithm**
When offering 3 upgrades:
1. Filter available upgrades (non-maxed stacks)
2. For each of 3 slots:
   - Calculate total weight of all remaining upgrades
   - Pick random value between 0 and total weight
   - Select upgrade based on cumulative rarity weights
   - Remove from pool (no duplicates in same offer)

**Example Calculation**:
- 7 Common upgrades × 70 weight = 490
- 5 Rare upgrades × 25 weight = 125
- 2 Legendary upgrades × 5 weight = 10
- **Total weight**: 625

**Chances**:
- Common: 490/625 = 78.4%
- Rare: 125/625 = 20%
- Legendary: 10/625 = 1.6%

### **Stack Tracking**
- Each selected upgrade increments its stack count
- When stack reaches `maxStacks`, upgrade is removed from pool
- Stack count displays on button: `[Stacks: 2/5]`

---

## 🔄 **Integration Points**

### **Events**
The system uses these UnityEvents:

**UpgradeSystem**:
- `OnUpgradesOffered(List<LevelUpgradeData>)` - Fired when 3 upgrades are selected
- `OnUpgradeSelected(LevelUpgradeData)` - Fired when player picks one

**ExperienceSystem**:
- `OnLevelUp(int)` - Fired on every level-up

**PlayerStats**:
- `OnStatsChanged()` - Fired when any stat changes

### **Methods You Can Call**
```csharp
// Manually offer upgrades (for testing)
UpgradeSystem.Instance.OfferUpgrades();

// Check upgrade stacks
int stacks = UpgradeSystem.Instance.GetUpgradeStacks(upgradeData);

// Reset all upgrades (on run start)
UpgradeSystem.Instance.ResetUpgrades();
```

---

## 🎯 **Next Steps**

After setup is complete:
1. ✓ Test level-up upgrade selection
2. ⏳ Add icon sprites to upgrade assets
3. ⏳ Create "Start Run" button in PreRunMenuPanel
4. ⏳ Wire shop UI for between-sessions stat purchases
5. ⏳ Balance tuning (XP curve, auto-bonuses, upgrade values)

---

## 💡 **Design Notes**

### **Why Time.timeScale = 0?**
- Pauses combat during upgrade selection
- Player can read and think without pressure
- Enemies stop moving/attacking
- Player movement disabled

### **Why Only Milestone Levels Get Choices?**
- Creates anticipation and reward cadence
- Non-milestone levels give small auto-bonuses
- Every 5 levels feels significant
- Prevents choice fatigue

### **Why Remove Old StatUpgradeOption System?**
- New system uses ScriptableObjects (data-driven, easier to balance)
- Supports special abilities (double jump, dash, lifesteal)
- Rarity system for varied progression
- Stack tracking for repeated upgrades
- More flexible and expandable

---

## 📚 **Related Files**
- `/Assets/Scripts/Systems/LevelUpUI.cs` - UI controller
- `/Assets/Scripts/Systems/Upgrades/UpgradeSystem.cs` - Logic
- `/Assets/Scripts/Systems/Upgrades/LevelUpgradeData.cs` - Data structure
- `/Assets/Scripts/Systems/PlayerStats.cs` - Stat management
- `/Assets/Scripts/Systems/ExperienceSystem.cs` - XP and leveling
- `/Assets/Prefabs/UI/ItemButton.prefab` - Button template
- `/Assets/Data/LevelUpgrades/` - All upgrade assets

---

**Setup Complete! Test in Play Mode and enjoy your upgrade system! 🎮**
