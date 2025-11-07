# Three-Currency System Implementation Guide

## 🎯 Overview

Your roguelike now features a **three-currency economy**:

1. **Gold** - In-run currency for obstacle placement (dropped by enemies as pickups)
2. **Experience (XP)** - In-run progression currency (auto-collected orbs, grants levels)
3. **Essence** - Persistent meta-currency (earned from waves/bosses, spent in Base for permanent upgrades)

---

## 💰 Currency Breakdown

### Gold (In-Run)
- **Purpose:** Place obstacles during runs
- **Source:** Dropped by defeated enemies as ground pickups
- **Persistence:** Resets at start of each run
- **Usage:** Tactical obstacle deployment (not yet implemented)

### Experience (In-Run)
- **Purpose:** Level up during runs to gain temporary stat boosts
- **Source:** Auto-collected XP orbs dropped by defeated enemies
- **Behavior:** 
  - One orb per enemy
  - Flies toward player at 10 units/sec (configurable)
  - Always travels to player regardless of distance
- **Persistence:** Resets at start of each run
- **Usage:** Fill XP bar → Level up → Choose from 3 stat upgrades

### Essence (Meta-Currency)
- **Purpose:** Permanent upgrades purchased in Base/Menu between runs
- **Source:** 
  - Wave completion rewards (configurable per wave)
  - Boss defeats (configurable)
  - Scaled by difficulty and survival time
- **Persistence:** Saved to disk between sessions
- **Usage:** Spend in Base shop for permanent stat upgrades and hero unlocks

---

## 📂 New and Updated Scripts

### Core Currency System

#### `/Assets/Scripts/Systems/CurrencyManager.cs` ✅ UPDATED
Manages all three currencies with events for UI updates.

**Key APIs:**
```csharp
// Gold
CurrencyManager.Instance.AddGold(amount);
CurrencyManager.Instance.SpendGold(amount);

// Experience
CurrencyManager.Instance.AddExperience(amount);

// Essence
CurrencyManager.Instance.AddEssence(amount);
CurrencyManager.Instance.SpendEssence(amount);

// Reset in-run currencies
CurrencyManager.Instance.ResetInRunCurrencies();
```

**Events:**
- `OnGoldChanged` - Fires when Gold changes
- `OnExperienceChanged` - Fires when XP changes
- `OnEssenceChanged` - Fires when Essence changes

---

#### `/Assets/Scripts/Systems/SaveSystem.cs` ✅ UPDATED
Renamed `metaCurrency` → `essence` for clarity.

**Key APIs:**
```csharp
SaveSystem.Instance.AddEssence(amount);
SaveSystem.Instance.SpendEssence(amount);
SaveSystem.Instance.GetEssence();
```

---

#### `/Assets/Scripts/Systems/PersistentData.cs` ✅ UPDATED
Data structure now uses `essence` instead of `metaCurrency`.

---

### Experience & Leveling System

#### `/Assets/Scripts/Systems/ExperienceSystem.cs` ✅ NEW
Handles XP accumulation and leveling.

**Inspector Settings:**
- `baseXPRequired` - XP needed for level 2 (default: 100)
- `xpScalingPerLevel` - Multiplier per level (default: 1.15)

**Key Features:**
- Automatically calculates XP required per level
- Fires `OnLevelUp` event when player levels up
- Fires `OnXPChanged` event for UI updates
- Detects milestone levels (every 5 levels)

**Example XP Progression:**
- Level 1→2: 100 XP
- Level 2→3: 115 XP
- Level 3→4: 132 XP
- Level 5: Milestone (special rewards)

---

#### `/Assets/Scripts/Pickups/ExperienceOrb.cs` ✅ NEW
Flying XP orb that auto-travels to player.

**Inspector Settings:**
- `xpValue` - XP awarded on collection (default: 10)
- `flySpeed` - Movement speed toward player (default: 10 units/sec)
- `collectionDistance` - Pickup radius (default: 0.5 units)
- `rotationSpeed` - Visual rotation (default: 180 deg/sec)
- `bobSpeed` - Vertical bobbing speed (default: 2)
- `bobHeight` - Vertical bobbing amplitude (default: 0.3 units)

**Behavior:**
- Always flies toward player
- No radius limit—works from any distance
- Auto-collected on contact
- Visual: Rotates and bobs for polish

---

#### `/Assets/Scripts/Systems/LevelUpUI.cs` ✅ NEW
Manages level-up choices with pause-on-level-up.

**Inspector Settings:**
- `levelUpPanel` - UI panel GameObject to show/hide
- `levelTitleText` - Title showing current level
- `optionsContainer` - Parent transform for option buttons
- `optionButtonPrefab` - Prefab for each upgrade choice
- `smallBoosts[]` - Array of normal level-up options
- `milestoneBoosts[]` - Array of special options for levels 5, 10, 15, etc.

**Features:**
- Automatically pauses game (`Time.timeScale = 0`)
- Shows 3 random choices from appropriate pool
- Small boosts for normal levels (e.g., +5 damage, +10% move speed)
- Significant boosts every 5 levels (e.g., +25 damage, +30% crit chance)
- Resumes game after selection

**StatUpgradeOption Structure:**
```csharp
[Serializable]
public class StatUpgradeOption
{
    public string upgradeName;        // "Damage Boost"
    public string description;        // "Increase damage by 5"
    public UpgradeStatType statType;  // MaxHealth, Damage, MoveSpeed, etc.
    public float value;               // 5.0, 10.0, etc.
}
```

---

### Player Stats (Temporary Bonuses)

#### `/Assets/Scripts/Systems/PlayerStats.cs` ✅ UPDATED
Added temporary in-run stat bonuses from level-ups.

**New APIs:**
```csharp
PlayerStats.Instance.AddTemporaryMaxHealth(amount);
PlayerStats.Instance.AddTemporaryDamage(amount);
PlayerStats.Instance.AddTemporaryMoveSpeed(multiplier);  // 0.1 = +10%
PlayerStats.Instance.AddTemporaryCritChance(amount);
PlayerStats.Instance.AddTemporaryCritDamage(multiplier);
PlayerStats.Instance.AddTemporaryAttackSpeed(multiplier);
PlayerStats.Instance.ResetTemporaryBonuses();  // Called at run start
```

**How it Works:**
- Temporary bonuses stack with permanent upgrades
- Applied by `LevelUpUI` when player selects a level-up option
- Reset at the start of each run

---

### Enemy Drops

#### `/Assets/Scripts/Enemy/EnemyHealth.cs` ✅ UPDATED
Now drops both Gold (pickup) and XP (flying orb).

**Inspector Settings:**
- `goldReward` - Gold value dropped (default: 5)
- `xpReward` - XP value in orb (default: 10) [Currently unused—orb has its own value]
- `xpOrbPrefab` - Prefab for XP orb (assign in Inspector!)

**Behavior on Death:**
- Awards Gold immediately via `CurrencyManager.Instance.AddGold(goldReward)`
- Spawns XP orb prefab at enemy position + 0.5 units up
- XP orb flies to player automatically

---

### Progression & Rewards

#### `/Assets/Scripts/Systems/GameProgressionManager.cs` ✅ UPDATED
Calculates and awards Essence at run end.

**Inspector Settings (Essence Rewards):**
- `essencePerWave` - Base Essence per wave completed (default: 10)
- `essenceForVictory` - Bonus for completing all waves (default: 200)
- `essenceZone1Bonus` - Bonus for Zone 1 (default: 100)
- `essenceZone2Bonus` - Bonus for Zone 2 (default: 50)
- `essenceZone3Bonus` - Bonus for Zone 3 (default: 25)
- `minimumEssenceReward` - Minimum Essence even on failure (default: 10)

**Essence Calculation:**
```
Total Essence = (Waves Completed × essencePerWave) 
              + Zone Bonus 
              + Victory Bonus
```

**Example:**
- Player survives 8 waves in Zone 1 and completes the run
- Essence = (8 × 10) + 100 + 200 = **380 Essence**

---

### UI Systems

#### `/Assets/Scripts/Systems/CurrencyDisplay.cs` ✅ UPDATED
Now displays all three currencies.

**Inspector Settings:**
- `goldText` - TextMeshProUGUI for Gold
- `experienceText` - TextMeshProUGUI for Experience/Level
- `essenceText` - TextMeshProUGUI for Essence
- `showGold` - Toggle Gold display (default: true in-run)
- `showExperience` - Toggle XP display (default: true in-run)
- `showEssence` - Toggle Essence display (default: false in-run, true in Base)

**Usage:**
- Create separate `CurrencyDisplay` for HUD (Gold + XP)
- Create separate `CurrencyDisplay` for Base UI (Essence only)

---

#### `/Assets/Scripts/Systems/SimpleShopUI.cs` ✅ UPDATED
Now uses Essence instead of "Souls."

**Changes:**
- Displays "Essence: X" instead of "Souls: X"
- Uses `CurrencyManager.Instance.SpendEssence(cost)`

---

#### `/Assets/Scripts/Systems/UpgradeShop.cs` ✅ UPDATED
Purchases now consume Essence.

**Changes:**
- `TryPurchaseUpgrade()` and `TryPurchaseWeapon()` now call `SpendEssence()`

---

## 🎮 Setup Checklist

### 1. Create XP Orb Prefab
1. Create empty GameObject: `XP_Orb`
2. Add `ExperienceOrb` script
3. Add visual (Sphere mesh or Sprite)
4. Set Inspector values:
   - XP Value: `10`
   - Fly Speed: `10`
   - Collection Distance: `0.5`
5. Save as prefab in `/Assets/Prefabs/XP_Orb.prefab`

---

### 2. Assign XP Orb to Enemies
1. Select enemy prefab (e.g., `Enemy.prefab`)
2. Find `EnemyHealth` component
3. Assign `XP_Orb` prefab to `Xp Orb Prefab` field
4. Set `Gold Reward` (e.g., 5)
5. Repeat for all enemy types

---

### 3. Setup Managers in Scene
1. **CurrencyManager:**
   - Already exists as singleton
   - No additional setup needed

2. **ExperienceSystem:**
   - Create empty GameObject: `ExperienceSystem`
   - Add `ExperienceSystem` script
   - Configure Inspector:
     - Base XP Required: `100`
     - XP Scaling Per Level: `1.15`

3. **LevelUpUI:**
   - Create UI Canvas if not exists
   - Create Panel: `LevelUpPanel`
   - Add child TextMeshProUGUI: `LevelTitle`
   - Add child ScrollView or VerticalLayoutGroup: `OptionsContainer`
   - Create button prefab: `LevelUpOptionButton` with:
     - TextMeshProUGUI child: `Name`
     - TextMeshProUGUI child: `Description`
     - TextMeshProUGUI child: `Value`
   - Add `LevelUpUI` script to `LevelUpPanel`
   - Assign references in Inspector
   - Configure `smallBoosts[]` and `milestoneBoosts[]` arrays with stat options

---

### 4. Configure Level-Up Options

**Example Small Boosts (Normal Levels):**
```
Name: "Damage +"
Description: "Increase base damage"
Stat Type: Damage
Value: 5

Name: "Speed +"
Description: "Move faster"
Stat Type: MoveSpeed
Value: 10 (= +10%)

Name: "Health +"
Description: "Increase max health"
Stat Type: MaxHealth
Value: 20
```

**Example Milestone Boosts (Levels 5, 10, 15...):**
```
Name: "Massive Damage"
Description: "Greatly increase damage"
Stat Type: Damage
Value: 25

Name: "Critical Mastery"
Description: "Huge crit chance boost"
Stat Type: CritChance
Value: 15 (= +15%)

Name: "Tank"
Description: "Massive health increase"
Stat Type: MaxHealth
Value: 100
```

---

### 5. Update HUD (In-Run UI)
1. Add two TextMeshProUGUI elements to HUD Canvas:
   - `GoldText`
   - `ExperienceText`
2. Add `CurrencyDisplay` script to HUD
3. Assign:
   - Gold Text → `GoldText`
   - Experience Text → `ExperienceText`
   - Show Gold: ✅
   - Show Experience: ✅
   - Show Essence: ❌

---

### 6. Update Base UI (Menu/Shop)
1. Add TextMeshProUGUI to Base Canvas: `EssenceText`
2. Add `CurrencyDisplay` script to Base UI
3. Assign:
   - Essence Text → `EssenceText`
   - Show Gold: ❌
   - Show Experience: ❌
   - Show Essence: ✅

---

### 7. Configure Essence Rewards
1. Select `GameProgressionManager` in scene
2. Set Essence Rewards:
   - Essence Per Wave: `10` (adjust for balance)
   - Essence For Victory: `200`
   - Essence Zone 1 Bonus: `100`
   - Essence Zone 2 Bonus: `50`
   - Essence Zone 3 Bonus: `25`
   - Minimum Essence Reward: `10`

---

## 🧪 Testing

### Test Gold System
1. Enter Play mode
2. Kill an enemy
3. Verify Gold counter increases in HUD
4. Check Console for: `+5 Gold` (or configured value)

### Test Experience & Leveling
1. Enter Play mode
2. Kill enemies to spawn XP orbs
3. Verify orbs fly toward player
4. Watch XP bar fill
5. On level-up:
   - Game should pause
   - Level-up panel should appear
   - 3 random options should display
6. Select an option
7. Verify game resumes
8. Check stat increase applied

### Test Essence Rewards
1. Complete a wave or die
2. Check Console for Essence calculation log
3. Return to Base/Menu
4. Verify Essence counter shows correct amount
5. Purchase an upgrade
6. Verify Essence decreases
7. Exit and reload project
8. Verify Essence persisted to save file

---

## 🔧 Tuning Parameters

### XP Requirements (ExperienceSystem)
- **Increase** `baseXPRequired` for slower early leveling
- **Increase** `xpScalingPerLevel` for steeper level curve
- **Decrease** for faster leveling

### XP Orb Behavior (ExperienceOrb)
- **Increase** `flySpeed` for faster collection
- **Increase** `collectionDistance` for easier pickup
- Adjust `bobSpeed` and `bobHeight` for visual polish

### Essence Rewards (GameProgressionManager)
- **Increase** `essencePerWave` for faster meta-progression
- **Increase** `essenceForVictory` to reward full completions
- Adjust zone bonuses to incentivize harder zones

### Level-Up Rewards (LevelUpUI)
- Add more options to `smallBoosts[]` for variety
- Make milestone boosts 3-5× stronger than small boosts
- Test balance: players should feel rewarded but not overpowered

---

## 📊 Currency Flow Summary

```
┌─────────────────────────────────────────────────────────────┐
│                        PLAYER LOOP                          │
└─────────────────────────────────────────────────────────────┘

[BASE/MENU]
  │
  ├─→ Spend ESSENCE on permanent upgrades
  │   (Saved to disk, persists between sessions)
  │
  └─→ Start Run
      │
      ┌──────────────────────────────────────────────┐
      │              IN-RUN PHASE                    │
      ├──────────────────────────────────────────────┤
      │                                              │
      │  [Kill Enemy]                                │
      │    ├─→ Drop GOLD (ground pickup)            │
      │    └─→ Spawn XP ORB (flies to player)       │
      │                                              │
      │  [Collect XP]                                │
      │    └─→ Fill XP bar → Level Up               │
      │        └─→ Choose temporary stat boost      │
      │                                              │
      │  [Spend GOLD]                                │
      │    └─→ Place obstacles (future feature)     │
      │                                              │
      └──────────────────────────────────────────────┘
      │
      [Run Ends - Victory or Defeat]
      │
      └─→ Award ESSENCE based on:
          ├─→ Waves completed × essencePerWave
          ├─→ Zone bonus
          └─→ Victory bonus
      │
      └─→ Return to BASE/MENU (loop)
```

---

## 🐛 Troubleshooting

### XP Orbs not spawning
- Check `xpOrbPrefab` is assigned in `EnemyHealth`
- Verify `ExperienceOrb` script is on the prefab
- Check Console for instantiation errors

### XP Orbs not flying to player
- Verify player has tag `"Player"`
- Check `flySpeed` is not 0
- Ensure `ExperienceSystem` exists in scene

### Level-up UI not appearing
- Check `ExperienceSystem.OnLevelUp` event is hooked
- Verify `levelUpPanel` is assigned in `LevelUpUI`
- Check `optionButtonPrefab` is assigned
- Ensure `smallBoosts[]` and `milestoneBoosts[]` are populated

### Essence not persisting
- Check `SaveSystem` is in scene
- Verify save file path in Console logs
- Check for file permissions issues
- Look for `savefile.json` at `Application.persistentDataPath`

### UI not updating
- Verify UI text references are assigned
- Check `CurrencyManager` events are hooked in `Start()`
- Use `Debug.Log` to confirm events fire

---

## 🎯 Next Steps

1. ✅ Implement XP orb prefab with visuals
2. ✅ Configure level-up UI with stat options
3. ✅ Tune XP requirements and Essence rewards
4. ⏳ Implement Gold-based obstacle placement system
5. ⏳ Add visual polish to XP orbs (particles, sound)
6. ⏳ Create milestone-level special effects
7. ⏳ Balance all three economies through playtesting

---

**System Status:** ✅ Fully Implemented and Ready for Setup!

**Files Updated:**
- `/Assets/Scripts/Systems/CurrencyManager.cs`
- `/Assets/Scripts/Systems/SaveSystem.cs`
- `/Assets/Scripts/Systems/PersistentData.cs`
- `/Assets/Scripts/Systems/ExperienceSystem.cs` (NEW)
- `/Assets/Scripts/Pickups/ExperienceOrb.cs` (NEW)
- `/Assets/Scripts/Systems/LevelUpUI.cs` (NEW)
- `/Assets/Scripts/Systems/PlayerStats.cs`
- `/Assets/Scripts/Enemy/EnemyHealth.cs`
- `/Assets/Scripts/Systems/GameProgressionManager.cs`
- `/Assets/Scripts/Systems/CurrencyDisplay.cs`
- `/Assets/Scripts/Systems/SimpleShopUI.cs`
- `/Assets/Scripts/Systems/UpgradeShop.cs`
