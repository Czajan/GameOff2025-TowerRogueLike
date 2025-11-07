# Migration to Three-Currency System

## 🔄 What Changed

Your project has been refactored from a **two-currency system** to a **three-currency system**.

---

## 📝 Old System vs New System

### Old System (Two Currencies)
1. **Gold** - In-run currency
2. **Souls** - Persistent meta-currency

### New System (Three Currencies)
1. **Gold** - In-run currency for obstacle placement
2. **Experience** - In-run XP system with leveling
3. **Essence** - Persistent meta-currency (replaces "Souls")

---

## 🚨 Breaking Changes

### 1. Renamed: Souls → Essence

**SaveSystem API Changes:**
```csharp
// OLD
SaveSystem.Instance.AddMetaCurrency(amount);
SaveSystem.Instance.SpendMetaCurrency(amount);
SaveSystem.Instance.GetMetaCurrency();

// NEW
SaveSystem.Instance.AddEssence(amount);
SaveSystem.Instance.SpendEssence(amount);
SaveSystem.Instance.GetEssence();
```

**CurrencyManager API Changes:**
```csharp
// OLD
CurrencyManager.Instance.AddMetaCurrency(amount);
CurrencyManager.Instance.SpendMetaCurrency(amount);
CurrencyManager.Instance.MetaCurrency;
CurrencyManager.Instance.OnMetaCurrencyChanged;

// NEW
CurrencyManager.Instance.AddEssence(amount);
CurrencyManager.Instance.SpendEssence(amount);
CurrencyManager.Instance.Essence;
CurrencyManager.Instance.OnEssenceChanged;
```

**PersistentData Field Renamed:**
```csharp
// OLD
persistentData.metaCurrency

// NEW
persistentData.essence
```

---

### 2. New: Experience System

**New Currency:**
- `CurrencyManager.Instance.AddExperience(amount)`
- `CurrencyManager.Instance.Experience`
- `CurrencyManager.Instance.OnExperienceChanged`

**New Leveling:**
- `ExperienceSystem.Instance.CurrentLevel`
- `ExperienceSystem.Instance.CurrentXP`
- `ExperienceSystem.Instance.XPRequired`
- `ExperienceSystem.Instance.OnLevelUp`
- `ExperienceSystem.Instance.OnXPChanged`

---

### 3. New: Temporary In-Run Stats

**PlayerStats New APIs:**
```csharp
PlayerStats.Instance.AddTemporaryMaxHealth(amount);
PlayerStats.Instance.AddTemporaryDamage(amount);
PlayerStats.Instance.AddTemporaryMoveSpeed(multiplier);
PlayerStats.Instance.AddTemporaryCritChance(amount);
PlayerStats.Instance.AddTemporaryCritDamage(multiplier);
PlayerStats.Instance.AddTemporaryAttackSpeed(multiplier);
PlayerStats.Instance.ResetTemporaryBonuses();
```

---

### 4. Enemy Drops Changed

**Old Behavior:**
- Enemies called `CurrencyManager.Instance.AddGold(currencyReward)`

**New Behavior:**
- Enemies call `CurrencyManager.Instance.AddGold(goldReward)`
- Enemies spawn XP orb prefab that flies to player
- XP orb auto-collects and awards Experience

**New Inspector Fields (EnemyHealth):**
- `goldReward` (replaces `currencyReward`)
- `xpReward` (currently unused - orb has its own value)
- `xpOrbPrefab` (must be assigned!)

---

### 5. Run Reset Logic Updated

**GameProgressionManager:**
```csharp
// OLD
CurrencyManager.Instance.ResetGold();

// NEW
CurrencyManager.Instance.ResetInRunCurrencies();  // Resets Gold + XP
PlayerStats.Instance.ResetTemporaryBonuses();     // Resets level-up bonuses
ExperienceSystem.Instance.ResetLevel();           // Resets to level 1
```

---

## 🛠️ Action Items

### If You Have Existing Save Files
Existing save files use `metaCurrency` instead of `essence`. You have two options:

**Option A: Reset Save (Recommended for Testing)**
1. Delete `savefile.json` at `Application.persistentDataPath`
2. Fresh start with new system

**Option B: Manual Migration (Advanced)**
1. Load old `savefile.json`
2. Rename field `metaCurrency` → `essence`
3. Save file

---

### If You Have Custom Scripts Using Old APIs

**Search and Replace:**
1. Find: `AddMetaCurrency` → Replace: `AddEssence`
2. Find: `SpendMetaCurrency` → Replace: `SpendEssence`
3. Find: `GetMetaCurrency` → Replace: `GetEssence`
4. Find: `OnMetaCurrencyChanged` → Replace: `OnEssenceChanged`
5. Find: `MetaCurrency` → Replace: `Essence`
6. Find: `metaCurrency` → Replace: `essence`

---

### Update Enemy Prefabs

For each enemy prefab:
1. Open prefab
2. Find `EnemyHealth` component
3. Rename `Currency Reward` → `Gold Reward` (if shown)
4. Assign `Xp Orb Prefab` field
5. Save prefab

---

### Setup New Systems

Follow the checklist in `/Assets/Guide/THREE_CURRENCY_SYSTEM_GUIDE.md`:
1. Create XP Orb prefab
2. Add ExperienceSystem to scene
3. Setup LevelUpUI
4. Configure level-up options
5. Update HUD to show Gold + XP
6. Update Base UI to show Essence

---

## 📁 Files to Delete (Optional Cleanup)

These "New" versions were created to avoid conflicts. You can now:

1. **Delete old versions:**
   - (If they exist and are outdated)

2. **Rename new versions:**
   - `/Assets/Scripts/Systems/CurrencyManagerNew.cs` → `/Assets/Scripts/Systems/CurrencyManager.cs`
   - `/Assets/Scripts/Systems/SaveSystemNew.cs` → `/Assets/Scripts/Systems/SaveSystem.cs`
   - `/Assets/Scripts/Systems/PersistentDataNew.cs` → `/Assets/Scripts/Systems/PersistentData.cs`

3. **Update references in scene:**
   - Ensure all GameObjects reference the correct scripts

**OR:** Keep using the existing updated files if no "New" versions were created.

---

## ✅ Verification Checklist

After migration, verify:

- [ ] No compilation errors in Console
- [ ] Gold increases when killing enemies
- [ ] XP orbs spawn and fly to player
- [ ] XP bar fills and triggers level-up
- [ ] Level-up UI appears with 3 options
- [ ] Game pauses on level-up
- [ ] Stat boost applies after selection
- [ ] Essence is awarded at run end
- [ ] Essence persists after exiting Play mode
- [ ] Base shop uses Essence for purchases
- [ ] Save file contains "essence" field

---

## 🐛 Common Migration Issues

### "Missing Script" errors
- **Cause:** Old scripts deleted before updating scene references
- **Fix:** Reassign scripts to GameObjects in scene

### Two CurrencyManager instances
- **Cause:** Both old and new scripts exist
- **Fix:** Delete duplicate, keep only one

### XP orbs not spawning
- **Cause:** `xpOrbPrefab` not assigned
- **Fix:** Assign XP Orb prefab to all enemy prefabs

### Level-up UI not working
- **Cause:** Missing UI setup or event listeners
- **Fix:** Follow setup guide in `THREE_CURRENCY_SYSTEM_GUIDE.md`

### Save file errors
- **Cause:** Old save file uses `metaCurrency`
- **Fix:** Delete old save file or manually migrate

---

## 📖 Resources

- **Full Setup Guide:** `/Assets/Guide/THREE_CURRENCY_SYSTEM_GUIDE.md`
- **Original Design Doc:** `/Assets/Guide/GAME_DESIGN_DOCUMENT.md`
- **Project Context:** `/Assets/Guide/PROJECT_CONTEXT.md`

---

**Migration Status:** ✅ Complete

All systems have been updated to support the three-currency economy. Follow the setup checklist to configure your scene and prefabs.
