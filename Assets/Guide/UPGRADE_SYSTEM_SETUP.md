# Roguelike Upgrade System - Complete Setup Guide

## 🎮 Overview

A complete roguelike upgrade system similar to **Vampire Survivors**, **Hades**, or **Risk of Rain 2**:

- **3 random upgrades** offered every 5 levels (5, 10, 15, 20...)
- **Rarity system**: Common (70%), Rare (25%), Legendary (5%)
- **Stat boosts**: Damage, Health, Speed, Crit, Attack Speed
- **Special abilities**: Double Jump, Dash, Lifesteal, Thorns, Explosions
- **Stacking upgrades**: Same upgrade can be picked multiple times
- **Game pauses** during selection for strategic choice
- **Build variety**: Every run feels different!

---

## 📁 Files Created

### Core Systems
- `/Assets/Scripts/Systems/Upgrades/UpgradeData.cs` - ScriptableObject definition
- `/Assets/Scripts/Systems/Upgrades/UpgradeSystem.cs` - Manager system
- `/Assets/Scripts/UI/UpgradeSelectionUI.cs` - UI controller
- `/Assets/Scripts/UI/UpgradeOptionUI.cs` - Individual upgrade card

### Modified Files
- `/Assets/Scripts/Player/PlayerController.cs` - Added double jump & dash
- `/Assets/Scripts/Systems/PlayerStats.cs` - Removed auto level-up bonuses

---

## 🛠️ Setup Instructions

### Step 1: Create Upgrade Assets Folder

1. In Project window, navigate to `/Assets`
2. Create folder structure:
   ```
   /Assets
     /UpgradeAssets
       /Common
       /Rare
       /Legendary
   ```

### Step 2: Create Example Upgrades

#### Common Upgrades (stat boosts)

**Damage Boost**
1. Right-click `/Assets/UpgradeAssets/Common` → Create → Game → Upgrade
2. Name: `Upgrade_DamageBoost`
3. Configure:
   - Upgrade Name: `"Power Up"`
   - Description: `"Increase your damage."`
   - Rarity: `Common`
   - Upgrade Type: `StatBoost`
   - Can Stack: `✓`
   - Max Stacks: `10`
   - Damage Bonus: `5`

**Health Boost**
1. Create new upgrade: `Upgrade_HealthBoost`
2. Configure:
   - Upgrade Name: `"Vitality"`
   - Description: `"Increase your max health."`
   - Rarity: `Common`
   - Max Health Bonus: `20`
   - Max Stacks: `10`

**Speed Boost**
1. Create: `Upgrade_SpeedBoost`
2. Configure:
   - Upgrade Name: `"Swift Feet"`
   - Description: `"Move faster."`
   - Rarity: `Common`
   - Move Speed Bonus: `0.10` (10% increase)
   - Max Stacks: `5`

**Critical Chance**
1. Create: `Upgrade_CritChance`
2. Configure:
   - Upgrade Name: `"Precision"`
   - Description: `"Increase critical hit chance."`
   - Rarity: `Common`
   - Crit Chance Bonus: `0.05` (5%)
   - Max Stacks: `8`

**Attack Speed**
1. Create: `Upgrade_AttackSpeed`
2. Configure:
   - Upgrade Name: `"Fury"`
   - Description: `"Attack faster."`
   - Rarity: `Common`
   - Attack Speed Bonus: `0.15` (15%)
   - Max Stacks: `5`

#### Rare Upgrades (stronger boosts)

**Mega Damage**
1. Create in `/Rare`: `Upgrade_MegaDamage`
2. Configure:
   - Upgrade Name: `"Devastation"`
   - Description: `"Massively increase damage."`
   - Rarity: `Rare`
   - Damage Bonus: `15`
   - Max Stacks: `5`

**Critical Damage**
1. Create: `Upgrade_CritDamage`
2. Configure:
   - Upgrade Name: `"Execution"`
   - Description: `"Critical hits deal more damage."`
   - Rarity: `Rare`
   - Crit Damage Bonus: `0.5` (50% increase to crit multiplier)
   - Max Stacks: `3`

**Double Jump**
1. Create: `Upgrade_DoubleJump`
2. Configure:
   - Upgrade Name: `"Air Walker"`
   - Description: `"Jump again while airborne."`
   - Rarity: `Rare`
   - Upgrade Type: `Functionality`
   - Can Stack: `✗` (uncheck)
   - Grants Double Jump: `✓`

**Dash**
1. Create: `Upgrade_Dash`
2. Configure:
   - Upgrade Name: `"Phantom Step"`
   - Description: `"Quickly dash in any direction."`
   - Rarity: `Rare`
   - Upgrade Type: `Functionality`
   - Can Stack: `✗`
   - Grants Dash: `✓`

#### Legendary Upgrades (game-changing)

**Lifesteal**
1. Create in `/Legendary`: `Upgrade_Lifesteal`
2. Configure:
   - Upgrade Name: `"Vampiric Touch"`
   - Description: `"Heal for a percentage of damage dealt."`
   - Rarity: `Legendary`
   - Can Stack: `✓`
   - Max Stacks: `3`
   - Grants Lifesteal: `✓`
   - Lifesteal Percent: `0.15` (15%)

**Thorns**
1. Create: `Upgrade_Thorns`
2. Configure:
   - Upgrade Name: `"Retribution"`
   - Description: `"Reflect damage back to attackers."`
   - Rarity: `Legendary`
   - Max Stacks: `3`
   - Grants Thorns: `✓`
   - Thorns Percent: `0.3` (30%)

**Explosive Hits**
1. Create: `Upgrade_ExplosiveHits`
2. Configure:
   - Upgrade Name: `"Chain Reaction"`
   - Description: `"Hits trigger explosions that damage nearby enemies."`
   - Rarity: `Legendary`
   - Max Stacks: `3`
   - Grants Explosive Hits: `✓`
   - Explosion Radius: `3`
   - Explosion Damage Percent: `0.5` (50% of hit damage)

**God Mode**
1. Create: `Upgrade_GodMode`
2. Configure:
   - Upgrade Name: `"Divine Blessing"`
   - Description: `"Massive boost to all stats."`
   - Rarity: `Legendary`
   - Can Stack: `✗`
   - Damage Bonus: `20`
   - Max Health Bonus: `100`
   - Move Speed Bonus: `0.2`
   - Crit Chance Bonus: `0.1`
   - Attack Speed Bonus: `0.3`

---

### Step 3: Create UpgradeSystem GameObject

1. **In Hierarchy, find `/GameManagers`**
2. **Right-click → Create Empty**
3. **Rename to `UpgradeSystem`**
4. **Add Component → `UpgradeSystem`**
5. **Configure in Inspector:**

```
┌─ Upgrade System (Script) ────────────┐
│ All Upgrades: (Size: 13)             │
│   Element 0: Upgrade_DamageBoost     │
│   Element 1: Upgrade_HealthBoost     │
│   Element 2: Upgrade_SpeedBoost      │
│   Element 3: Upgrade_CritChance      │
│   Element 4: Upgrade_AttackSpeed     │
│   Element 5: Upgrade_MegaDamage      │
│   Element 6: Upgrade_CritDamage      │
│   Element 7: Upgrade_DoubleJump      │
│   Element 8: Upgrade_Dash            │
│   Element 9: Upgrade_Lifesteal       │
│   Element 10: Upgrade_Thorns         │
│   Element 11: Upgrade_ExplosiveHits  │
│   Element 12: Upgrade_GodMode        │
│                                       │
│ Upgrades Per Offer: 3                │
│ Allow Duplicates: ☐                  │
│                                       │
│ Common Weight: 70                    │
│ Rare Weight: 25                      │
│ Legendary Weight: 5                  │
└───────────────────────────────────────┘
```

---

### Step 4: Create Upgrade Selection UI

#### Create Panel Structure

1. **In Hierarchy, find `/GameCanvas`**
2. **Right-click → UI → Panel**
3. **Rename to `UpgradeSelectionPanel`**
4. **Configure:**
   - Rect Transform: Stretch/Stretch (full screen)
   - Image: Black, Alpha `220`

#### Create Title

1. **Right-click `UpgradeSelectionPanel` → UI → Text - TextMeshPro**
2. **Rename to `TitleText`**
3. **Configure:**
   - Rect Transform:
     - Anchor: Top Center
     - Pos Y: `-100`
     - Width: `800`, Height: `80`
   - Text: `"LEVEL UP - CHOOSE YOUR UPGRADE"`
   - Font Size: `36`
   - Alignment: Center
   - Color: White

#### Create Upgrade Options Container

1. **Right-click `UpgradeSelectionPanel` → Create Empty**
2. **Rename to `OptionsContainer`**
3. **Add Component → `Horizontal Layout Group`**
4. **Configure:**
   - Spacing: `30`
   - Child Alignment: Middle Center
   - Child Force Expand: Width ✗, Height ✗
5. **Rect Transform:**
   - Anchor: Middle Center
   - Pos Y: `0`
   - Width: `1200`, Height: `400`

#### Create Upgrade Option Cards (×3)

For **each** of the 3 upgrade options:

1. **Right-click `OptionsContainer` → UI → Button**
2. **Rename to `UpgradeOption1`** (then 2, then 3)
3. **Configure Button:**
   - Rect Transform: Width `350`, Height `400`
   - Image: Dark Gray, Alpha `200`
4. **Delete the default "Text (TMP)" child**
5. **Create card structure:**

```
UpgradeOption1
  ├─ Background (Image) - already exists
  ├─ IconImage (Create: UI → Image)
  ├─ NameText (Create: UI → Text - TMP)
  ├─ DescriptionText (Create: UI → Text - TMP)
  ├─ RarityText (Create: UI → Text - TMP)
  └─ StackText (Create: UI → Text - TMP)
```

**Configure each child:**

**IconImage:**
- Rect Transform: Top Center, Pos Y `-80`, Width `100`, Height `100`
- Image: Leave empty (set via script)

**NameText:**
- Rect Transform: Top Center, Pos Y `-200`, Width `320`, Height `40`
- Font Size: `24`
- Alignment: Center
- Font Style: Bold

**DescriptionText:**
- Rect Transform: Middle Center, Pos Y `0`, Width `320`, Height `150`
- Font Size: `16`
- Alignment: Top Left
- Wrapping: Enabled

**RarityText:**
- Rect Transform: Top Right, Pos X `-10`, Pos Y `-10`, Width `100`, Height `30`
- Font Size: `14`
- Alignment: Right
- Font Style: Bold

**StackText:**
- Rect Transform: Bottom Center, Pos Y `10`, Width `200`, Height `30`
- Font Size: `14`
- Alignment: Center

6. **Add `UpgradeOptionUI` script to each UpgradeOption**
7. **Assign references in Inspector:**

```
┌─ Upgrade Option UI (Script) ─────────┐
│ Select Button: [Button component]    │
│ Icon Image: [IconImage]              │
│ Background Image: [Background]       │
│ Name Text: [NameText]                │
│ Description Text: [DescriptionText]  │
│ Rarity Text: [RarityText]            │
│ Stack Text: [StackText]              │
│                                       │
│ Common Color: (0.8, 0.8, 0.8, 1)     │
│ Rare Color: (0.29, 0.56, 0.89, 1)    │
│ Legendary Color: (1, 0.84, 0, 1)     │
└───────────────────────────────────────┘
```

#### Add UpgradeSelectionUI Script

1. **Select `UpgradeSelectionPanel`**
2. **Add Component → `UpgradeSelectionUI`**
3. **Configure:**

```
┌─ Upgrade Selection UI (Script) ──────┐
│ Selection Panel: [UpgradeSelectionPanel] │
│ Title Text: [TitleText]              │
│ Upgrade Options: (Size: 3)           │
│   Element 0: [UpgradeOption1]        │
│   Element 1: [UpgradeOption2]        │
│   Element 2: [UpgradeOption3]        │
│                                       │
│ Pause Game On Show: ✓                │
└───────────────────────────────────────┘
```

#### Disable Panel Initially

1. **Select `UpgradeSelectionPanel`**
2. **Uncheck the box at top of Inspector** (to disable it)

---

### Step 5: Add Dash Input (Optional)

If you want dash functionality:

1. **Open Window → Asset Settings → Input Actions**
2. **Find your Player action map**
3. **Add new action:**
   - Name: `Dash`
   - Action Type: Button
   - Binding: `Left Shift` (or your preference)
4. **In Player Input component**, make sure it's connected
5. **In PlayerController script connection**, it will call `OnDash`

---

## 🎮 How It Works

### Flow Diagram

```
Player kills enemies
  ↓
Gains XP
  ↓
Reaches level 5, 10, 15, 20...
  ↓
UpgradeSystem.OnPlayerLevelUp()
  ↓
Selects 3 random upgrades (weighted by rarity)
  ↓
OnUpgradesOffered event fires
  ↓
UpgradeSelectionUI shows panel
  ↓
Time.timeScale = 0 (game pauses)
  ↓
Player clicks an upgrade card
  ↓
UpgradeSystem.SelectUpgrade()
  ↓
Applies stat bonuses / unlocks abilities
  ↓
OnUpgradeSelected event fires
  ↓
UI hides, Time.timeScale = 1 (game resumes)
  ↓
Player is stronger! Continue fighting...
```

### Rarity Weighting Example

When 3 upgrades are offered:
- 70% chance each slot is Common
- 25% chance each slot is Rare
- 5% chance each slot is Legendary

**Possible results:**
- Common + Common + Common (most likely)
- Common + Common + Rare
- Rare + Rare + Common
- Common + Legendary + Rare (lucky!)
- Legendary + Legendary + Legendary (extremely rare!)

### Stacking System

**Non-stackable** (e.g., Double Jump):
- Can only be picked once
- Won't appear again after acquired

**Stackable** (e.g., Damage Boost):
- Can be picked multiple times
- Each pick stacks the effect
- UI shows "Stack 2/10", "Stack 3/10", etc.
- Won't appear once max stacks reached

---

## 🧪 Testing Checklist

### Test 1: Basic Upgrade Offering

- [ ] Start a run
- [ ] Kill enemies to reach level 5
- [ ] **Expected:**
  - ✅ Game pauses
  - ✅ Upgrade panel appears
  - ✅ 3 upgrade cards shown
  - ✅ Each card shows name, description, rarity
  - ✅ Console: `"Offering 3 upgrade choices!"`

### Test 2: Upgrade Selection

- [ ] Click one of the upgrade cards
- [ ] **Expected:**
  - ✅ Panel disappears
  - ✅ Game resumes
  - ✅ Stats increase (check StatsPanel with TAB)
  - ✅ Console: `"★ Selected: [Upgrade Name]"`

### Test 3: Stat Boosts

- [ ] Select "Power Up" (+5 damage)
- [ ] Check stats (TAB)
- [ ] **Expected:**
  - ✅ Damage increased by 5
  - ✅ Enemies take more damage

### Test 4: Double Jump

- [ ] Reach level 5, select "Air Walker"
- [ ] Jump, then press jump again in mid-air
- [ ] **Expected:**
  - ✅ Second jump works!
  - ✅ Console: `"Double Jump!"`

### Test 5: Dash (if input configured)

- [ ] Select "Phantom Step"
- [ ] Press Shift (or dash key)
- [ ] **Expected:**
  - ✅ Player dashes forward
  - ✅ Console: `"Dash!"`

### Test 6: Stacking

- [ ] Reach level 5, select "Power Up"
- [ ] Reach level 10, select "Power Up" again
- [ ] Check stats
- [ ] **Expected:**
  - ✅ Damage = base + 10 (5 × 2 stacks)
  - ✅ Second card showed "Stack 2/10"

### Test 7: Rarity Distribution

- [ ] Reach multiple levels (5, 10, 15, 20)
- [ ] Observe upgrade rarities
- [ ] **Expected:**
  - ✅ Mostly see Common (gray)
  - ✅ Occasionally see Rare (blue)
  - ✅ Rarely see Legendary (gold)

### Test 8: Run Reset

- [ ] Acquire several upgrades
- [ ] Die or complete run
- [ ] Start new run
- [ ] Reach level 5
- [ ] **Expected:**
  - ✅ All upgrades reset
  - ✅ Can acquire them again
  - ✅ Stats back to base values

---

## 🎨 Customization

### Adjust Rarity Chances

In `UpgradeSystem` Inspector:
```
Common Weight: 70   → 50   (less common)
Rare Weight: 25     → 40   (more rare)
Legendary Weight: 5 → 10   (more legendary)
```

### Change Milestone Levels

Edit `UpgradeSystem.cs`, line ~64:
```csharp
private bool ShouldOfferUpgrade(int level)
{
    return level % 3 == 0;  // Every 3 levels instead of 5
}
```

### Offer More/Fewer Upgrades

In `UpgradeSystem` Inspector:
```
Upgrades Per Offer: 3 → 5  (5 choices instead of 3)
```

### Create Custom Upgrades

1. Create new ScriptableObject
2. Set unique name, description, icon
3. Configure stat bonuses or special effects
4. Add to `UpgradeSystem.allUpgrades` list

---

## 💡 Design Tips

### Balancing Upgrades

**Common (pick often):**
- Small stat boosts: +5 damage, +20 HP, +10% speed
- Stack 5-10 times
- Core build foundation

**Rare (occasional picks):**
- Medium stat boosts: +15 damage, +50 HP
- Special abilities: Double Jump, Dash
- Stack 3-5 times
- Build modifiers

**Legendary (rare, powerful):**
- Game-changing effects: Lifesteal, Explosive Hits
- Huge stat boosts: +100 HP, +20 damage
- Stack 1-3 times
- Build-defining picks

### Creating Build Variety

**Melee Fighter Build:**
- Power Up (stacked)
- Vitality (stacked)
- Lifesteal
- Thorns

**Glass Cannon Build:**
- Power Up (max stacks)
- Precision (max crit chance)
- Execution (crit damage)
- Dash (survivability)

**Tank Build:**
- Vitality (max stacks)
- Thorns
- Lifesteal
- Retribution

**Speed Runner Build:**
- Swift Feet (max stacks)
- Dash
- Double Jump
- Fury (attack speed)

---

## 🚀 Next Steps: Active Skills from Chests

The same system foundation can be used for:

1. **Elite Enemy Chests**
   - Elite dies → drops chest
   - Open chest → get active skill
   - Active skills: Fireball, Shield, Teleport, etc.

2. **Different Upgrade Pools**
   - Passive pool (level-ups)
   - Active pool (chests)

3. **Skill Cooldowns**
   - Press key to activate
   - Visual cooldown indicator
   - Limited uses per run

I can implement this next if you'd like!

---

## ✅ Setup Complete!

You now have a fully functional roguelike upgrade system with:
- ✅ Random upgrade selection at milestone levels
- ✅ Rarity-based weighting (Common/Rare/Legendary)
- ✅ Stat boosts (damage, health, speed, crit, etc.)
- ✅ Special abilities (double jump, dash)
- ✅ Stacking system
- ✅ Pause-on-select UI
- ✅ Build variety and replayability

**Every run will feel different!** 🎉
