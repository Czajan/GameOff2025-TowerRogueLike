# Three-Currency System - Quick Reference

## 💰 Currency Types

| Currency | Type | Source | Usage | Persistence |
|----------|------|--------|-------|-------------|
| **Gold** | In-Run | Enemy drops (pickups) | Obstacle placement | ❌ Resets each run |
| **Experience** | In-Run | Enemy XP orbs (auto-collect) | Level up for temp stats | ❌ Resets each run |
| **Essence** | Meta | Wave/boss rewards | Permanent upgrades | ✅ Saved to disk |

---

## 🎮 Player Flow

```
[START RUN]
    ↓
[KILL ENEMIES]
    ├─→ Gold drops (ground pickup)
    └─→ XP orb spawns (flies to player)
    ↓
[COLLECT XP]
    ↓
[LEVEL UP] → Pause game → Choose 1 of 3 stat boosts → Resume
    ↓
[REPEAT UNTIL RUN ENDS]
    ↓
[RUN COMPLETE]
    └─→ Award Essence based on performance
    ↓
[RETURN TO BASE]
    └─→ Spend Essence on permanent upgrades
    ↓
[LOOP]
```

---

## 🔧 Essential APIs

### Add Currency
```csharp
CurrencyManager.Instance.AddGold(amount);
CurrencyManager.Instance.AddExperience(amount);
CurrencyManager.Instance.AddEssence(amount);
```

### Spend Currency
```csharp
CurrencyManager.Instance.SpendGold(amount);      // Returns bool
CurrencyManager.Instance.SpendEssence(amount);   // Returns bool
```

### Get Current Values
```csharp
int gold = CurrencyManager.Instance.Gold;
int xp = CurrencyManager.Instance.Experience;
int essence = CurrencyManager.Instance.Essence;
```

### Events
```csharp
CurrencyManager.Instance.OnGoldChanged.AddListener(UpdateUI);
CurrencyManager.Instance.OnExperienceChanged.AddListener(UpdateUI);
CurrencyManager.Instance.OnEssenceChanged.AddListener(UpdateUI);
```

---

## 📊 Experience System

### Get Level Info
```csharp
int currentLevel = ExperienceSystem.Instance.CurrentLevel;
int currentXP = ExperienceSystem.Instance.CurrentXP;
int xpNeeded = ExperienceSystem.Instance.XPRequired;
```

### Level-Up Event
```csharp
ExperienceSystem.Instance.OnLevelUp.AddListener(OnPlayerLevelUp);

void OnPlayerLevelUp(int newLevel)
{
    Debug.Log($"Player reached level {newLevel}!");
}
```

### Check Milestone
```csharp
bool isMilestone = ExperienceSystem.Instance.IsMilestoneLevel();
// Returns true on levels 5, 10, 15, 20, etc.
```

---

## 💪 Temporary Stat Boosts

### Apply Temp Boosts (from level-ups)
```csharp
PlayerStats.Instance.AddTemporaryMaxHealth(20);        // +20 HP
PlayerStats.Instance.AddTemporaryDamage(5);            // +5 damage
PlayerStats.Instance.AddTemporaryMoveSpeed(0.1f);      // +10% speed
PlayerStats.Instance.AddTemporaryCritChance(0.05f);    // +5% crit
PlayerStats.Instance.AddTemporaryCritDamage(0.25f);    // +25% crit dmg
PlayerStats.Instance.AddTemporaryAttackSpeed(0.15f);   // +15% atk speed
```

### Reset (call at run start)
```csharp
PlayerStats.Instance.ResetTemporaryBonuses();
```

---

## 🎯 Enemy Setup

### EnemyHealth Inspector
```
Gold Reward: 5
XP Reward: 10 (currently unused)
XP Orb Prefab: [Assign XP_Orb prefab]
```

### On Enemy Death
- Calls `CurrencyManager.Instance.AddGold(goldReward)`
- Spawns `xpOrbPrefab` at `transform.position + Vector3.up * 0.5f`
- XP orb flies to player automatically

---

## 🏆 Essence Rewards

### Calculation Formula
```
Essence = (Waves × essencePerWave) + zoneBonus + victoryBonus
```

### Configurable Values (GameProgressionManager)
```
essencePerWave = 10          // Per wave completed
essenceForVictory = 200      // Bonus for completing run
essenceZone1Bonus = 100      // Bonus for Zone 1
essenceZone2Bonus = 50       // Bonus for Zone 2
essenceZone3Bonus = 25       // Bonus for Zone 3
minimumEssenceReward = 10    // Minimum on failure
```

### Example Calculation
```
Scenario: 8 waves, Zone 1, Victory
Essence = (8 × 10) + 100 + 200 = 380
```

---

## 🎨 UI Display

### In-Run HUD
```csharp
// Show Gold + Experience
showGold = true
showExperience = true
showEssence = false
```

### Base/Menu UI
```csharp
// Show Essence only
showGold = false
showExperience = false
showEssence = true
```

---

## ⚙️ Tuning Quick Access

### XP Scaling (ExperienceSystem)
```
baseXPRequired = 100         // XP for level 2
xpScalingPerLevel = 1.15     // Multiplier per level
```

**Result:**
- Level 1→2: 100 XP
- Level 2→3: 115 XP
- Level 3→4: 132 XP
- Level 10: ~305 XP

### XP Orb Behavior (ExperienceOrb)
```
xpValue = 10                 // XP awarded
flySpeed = 10                // Units per second
collectionDistance = 0.5     // Pickup radius
```

### Level-Up Rewards (LevelUpUI)
```
smallBoosts[] = { ... }      // Normal levels
milestoneBoosts[] = { ... }  // Every 5 levels
```

**Recommended Values:**
- Small boosts: 5-10 flat, 10-15% multipliers
- Milestone boosts: 20-30 flat, 25-50% multipliers

---

## 🐛 Quick Troubleshooting

| Issue | Check This |
|-------|------------|
| XP orbs not spawning | `xpOrbPrefab` assigned? |
| Orbs not flying | Player has tag `"Player"`? |
| Level-up UI missing | `LevelUpUI` in scene? Options configured? |
| Essence not saving | `SaveSystem` in scene? Check Console for file path |
| UI not updating | Text references assigned? Events hooked in `Start()`? |
| Stats not applying | `PlayerStats.Instance` exists? `ApplyStatsToPlayer()` called? |

---

## 📁 Key Files

### Core Systems
- `CurrencyManager.cs` - Manages all three currencies
- `SaveSystem.cs` - Saves/loads Essence + upgrades
- `ExperienceSystem.cs` - XP tracking and leveling
- `PlayerStats.cs` - Permanent + temporary stats

### Pickups & Rewards
- `ExperienceOrb.cs` - Flying XP orb behavior
- `EnemyHealth.cs` - Drops gold and spawns XP orbs
- `GameProgressionManager.cs` - Calculates Essence rewards

### UI
- `LevelUpUI.cs` - Level-up choice screen
- `CurrencyDisplay.cs` - Shows currencies in HUD/Base
- `SimpleShopUI.cs` - Base shop UI
- `UpgradeShop.cs` - Purchase logic

---

## 📚 Full Documentation

For detailed setup instructions, see:
- `/Assets/Guide/THREE_CURRENCY_SYSTEM_GUIDE.md`
- `/Assets/Guide/MIGRATION_TO_THREE_CURRENCIES.md`

---

**Quick Start:** Create XP Orb prefab → Assign to enemies → Add ExperienceSystem & LevelUpUI to scene → Configure options → Play!
