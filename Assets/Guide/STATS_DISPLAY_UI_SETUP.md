# Stats Display UI - Setup Guide

## ✅ Overview

Press **TAB** during gameplay to view all your current stats in real-time!

---

## 🎯 What It Shows

### Combat Stats
- **Damage** - Your current attack damage
- **Crit Chance** - Probability to deal critical hits
- **Crit Damage** - Critical hit multiplier
- **Attack Range** - Melee attack reach
- **Attack Speed** - Attack speed multiplier

### Defense Stats
- **Current HP** - Your current health
- **Max HP** - Maximum health capacity
- **Move Speed** - Movement speed value

### Progression
- **Level** - Current XP level this run
- **XP** - Current XP / XP needed for next level
- **Gold** - Gold currency (for obstacles)
- **Essence** - Essence currency (for permanent upgrades)

---

## 🛠️ Setup Instructions

### Step 1: Create the Stats Panel UI

1. **Open your scene:** `Assets/Scenes/MainScene.unity`

2. **Navigate to GameCanvas:**
   - In Hierarchy, find `/GameCanvas`

3. **Create Stats Panel:**
   - Right-click `GameCanvas` → UI → Panel
   - Rename it to **"StatsPanel"**

4. **Configure Stats Panel:**
   - **Rect Transform:**
     - Anchor Preset: **Middle Center**
     - Pos X: `0`, Pos Y: `0`
     - Width: `400`, Height: `600`
   - **Image Component:**
     - Color: `Black` with Alpha `200` (semi-transparent)

5. **Create Stats Text:**
   - Right-click `StatsPanel` → UI → Text - TextMeshPro
   - Rename to **"StatsText"**
   
6. **Configure Stats Text:**
   - **Rect Transform:**
     - Anchor Preset: **Stretch/Stretch** (hold Alt when clicking)
     - Left: `20`, Right: `20`, Top: `20`, Bottom: `20`
   - **TextMeshPro Component:**
     - Font Size: `18`
     - Alignment: **Top Left**
     - Color: `White`
     - Enable **Rich Text**
     - Wrapping: **Enabled**
     - Overflow: **Overflow**

7. **Add Script to StatsPanel:**
   - Select `StatsPanel` in Hierarchy
   - Click **Add Component**
   - Search for `StatsDisplayUI`
   - Add the script

8. **Assign References:**
   - With `StatsPanel` selected:
     - **Stats Panel:** Drag `StatsPanel` itself into this field
     - **Stats Text:** Drag `StatsPanel/StatsText` into this field
   - **Settings:**
     - Toggle Key: `Tab` (default)
     - Show On Startup: `false` (unchecked)

9. **Disable Panel Initially:**
   - Select `StatsPanel` in Hierarchy
   - **Uncheck** the checkbox at the top of the Inspector (to disable it)
   - This makes it hidden on startup

---

## 🎮 Usage

### In Play Mode:

1. **Start the game**
2. **Press TAB** → Stats panel appears!
3. **Press TAB again** → Panel disappears

### What You'll See:

```
═══════════════════════════════════
           PLAYER STATS

LEVEL: 3

COMBAT STATS
  Damage: 14.0
  Crit Chance: 7.0%
  Crit Damage: 150%
  Attack Range: 2.0m
  Attack Speed: 100%

DEFENSE STATS
  Current HP: 95
  Max HP: 120
  Move Speed: 5.5

PROGRESSION
  XP: 45/132
  Gold: 350
  Essence: 120

        Press TAB to close
═══════════════════════════════════
```

---

## 🎨 Customization Options

### Change Toggle Key

In Inspector, select `StatsPanel`:
- **Toggle Key:** Change from `Tab` to any other key (e.g., `I` for Inventory, `C` for Character)

### Show on Startup

If you want stats always visible:
- **Show On Startup:** Check this box
- Press TAB to hide/show

### Adjust Panel Size

Select `StatsPanel` in Hierarchy:
- **Rect Transform:**
  - Width: Adjust for wider/narrower panel
  - Height: Adjust for taller/shorter panel

### Change Panel Position

Select `StatsPanel` in Hierarchy:
- **Rect Transform:**
  - Anchor to corners (e.g., Top Left, Bottom Right)
  - Adjust Pos X/Y to move panel

### Styling

**Panel Background:**
- Select `StatsPanel`
- **Image Component:**
  - Change Color/Alpha for transparency
  - Add border sprite for fancier look

**Text Styling:**
- Select `StatsText`
- **TextMeshPro Component:**
  - Font Size: Increase/decrease
  - Font: Change to different font asset
  - Color: Change base color

---

## 🔍 How It Works

### Event-Driven Updates

The UI automatically updates when:

1. **Stats change:**
   - `PlayerStats.OnStatsChanged` event fires
   - Level-ups, upgrades, buffs trigger refresh

2. **Level-up:**
   - `ExperienceSystem.OnLevelUp` event fires
   - Display updates with new level and stats

3. **XP gained:**
   - `ExperienceSystem.OnXPChanged` event fires
   - XP bar text updates

### Real-Time Calculation

All values are fetched live from systems:
```csharp
PlayerStats.Instance.GetDamage()        // Live damage calc
PlayerStats.Instance.GetMaxHealth()     // Live max HP
PlayerStats.Instance.GetCritChance()    // Live crit chance
ExperienceSystem.Instance.CurrentLevel  // Current level
CurrencyManager.Instance.GetCurrency()  // Current currencies
```

This means the display **always shows accurate, up-to-date values**!

---

## 🧪 Testing Checklist

### Test 1: Basic Toggle
- [ ] Enter Play Mode
- [ ] Press TAB
- [ ] **Expected:** Stats panel appears
- [ ] Press TAB again
- [ ] **Expected:** Panel disappears

### Test 2: Initial Stats
- [ ] Press TAB to view stats
- [ ] **Expected:**
  - ✅ Level: 1
  - ✅ Damage: 10 (or your base damage)
  - ✅ Max HP: 100 (or your base HP)
  - ✅ Crit Chance: 5%
  - ✅ XP: 0/100

### Test 3: Level Up Updates
- [ ] Kill enemies to gain XP
- [ ] Level up to Level 2
- [ ] Open stats (TAB)
- [ ] **Expected:**
  - ✅ Level: 2
  - ✅ Damage: 12 (base 10 + 2 from level)
  - ✅ Max HP: 110 (base 100 + 10 from level)
  - ✅ Crit Chance: 6% (base 5% + 1% from level)

### Test 4: Currency Updates
- [ ] Collect gold from enemies
- [ ] Open stats (TAB)
- [ ] **Expected:**
  - ✅ Gold value matches currency display
  - ✅ Essence value shown

### Test 5: Health Updates
- [ ] Take damage from enemies
- [ ] Open stats (TAB)
- [ ] **Expected:**
  - ✅ Current HP decreases
  - ✅ Max HP stays same
  - ✅ Current HP / Max HP ratio accurate

### Test 6: Combat Stats
- [ ] Level up multiple times
- [ ] Open stats (TAB)
- [ ] **Expected:**
  - ✅ All stats show correct values
  - ✅ Attack speed shows multiplier
  - ✅ Crit damage shows percentage

---

## 💡 Advanced Features (Optional)

### Add Stat Breakdown Tooltip

Show how stats are calculated:
```csharp
// Example for damage breakdown
statsDisplay += $"  Base: {baseDamage}\n";
statsDisplay += $"  Permanent: +{damageLevel * damagePerLevel}\n";
statsDisplay += $"  Temporary: +{tempDamage}\n";
statsDisplay += $"  Zone Bonus: {zoneDamageBonus * 100}%\n";
```

### Add Stat Comparison

Show stat changes since last level:
```csharp
// Track previous stats and show differences
statsDisplay += $"  Damage: {currentDamage} <color=green>(+{damageIncrease})</color>\n";
```

### Add Keybind Reminder

At bottom of panel:
```csharp
statsDisplay += $"\n<size=14><i>TAB: Toggle Stats | I: Inventory | ESC: Pause</i></size>";
```

---

## 🐛 Troubleshooting

### Stats Panel Doesn't Appear
- ✅ Check `StatsPanel` has `StatsDisplayUI` component
- ✅ Check both fields are assigned (Stats Panel, Stats Text)
- ✅ Try pressing TAB multiple times
- ✅ Check Console for errors

### Stats Show Zero/Wrong Values
- ✅ Make sure `PlayerStats.Instance` exists in scene
- ✅ Make sure `ExperienceSystem.Instance` exists
- ✅ Check player has started a run (stats initialize on run start)

### Text Doesn't Fit Panel
- ✅ Increase panel Height in Rect Transform
- ✅ Decrease Font Size in TextMeshPro
- ✅ Enable Overflow in TextMeshPro

### Can't Read Text
- ✅ Increase panel background alpha (darker)
- ✅ Increase font size
- ✅ Change text color to brighter color
- ✅ Add outline to TextMeshPro

---

## 📊 Display Format Reference

The UI uses rich text formatting:

```csharp
<b>Bold Text</b>                    // Section headers
<size=24>Large Text</size>          // Title
<color=#FF6B6B>Colored Text</color> // Category colors
<color=yellow>Value</color>         // Highlight values
<i>Italic Text</i>                  // Instructions
```

**Category Colors:**
- 🔴 Combat Stats: `#FF6B6B` (Red)
- 🟢 Defense Stats: `#6BCF7F` (Green)
- 🔵 Progression: `#7FC3FF` (Blue)
- 🟡 Values: `yellow`

---

## 🎯 Future Enhancements

### 1. Stat Icons
Add icons next to each stat for visual appeal

### 2. Tabs/Categories
Split into multiple pages (Combat, Defense, Progression)

### 3. Comparisons
Show before/after when hovering over shop items

### 4. Milestones
Highlight when at milestone levels (5, 10, 15, etc.)

### 5. Animations
Fade in/out, slide animations when toggling

### 6. DPS Meter
Show actual damage-per-second calculated from combat

---

## ✅ Complete Setup Summary

**Quick Setup:**
1. Create Panel under GameCanvas
2. Add TextMeshPro text child
3. Add `StatsDisplayUI` script to Panel
4. Assign references
5. Disable panel initially
6. Press TAB in Play Mode!

**Result:**
- ✅ Press TAB to show stats anytime during gameplay
- ✅ Real-time stat tracking
- ✅ Auto-updates on level-ups and changes
- ✅ Clean, readable format
- ✅ Shows all important player information

Now you can track your progression and power growth in real-time! 🎉
