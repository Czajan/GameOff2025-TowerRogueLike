# Quick Reference Card

Fast lookup for common tasks and information.

---

## 🎯 Current Project State

**Status:** 85% Complete - Shop UI needs setup (20 min)  
**Next Step:** Follow `START_HERE_SHOP_SETUP.md`  
**Blocker:** None - all scripts compiled  

---

## 📁 Important File Locations

### Scripts
```
/Assets/Scripts/
  /Player/        - PlayerController, PlayerHealth, PlayerCombat
  /Enemy/         - EnemyAI, EnemyHealth
  /Systems/       - All managers and UI scripts
```

### Data Assets
```
/Assets/Data/
  /Weapons/       - 5 weapon ScriptableObjects
  /Upgrades/      - 6 upgrade ScriptableObjects
```

### Prefabs
```
/Assets/Prefabs/
  /Enemies/       - Enemy.prefab
  /UI/            - GameCanvas.prefab, ItemButton.prefab (create this)
```

### Scenes
```
/Assets/Scenes/
  MainScene.unity - Main game scene
```

---

## 🎮 Key GameObjects in Scene

### Singletons (on /GameManagers)
- **GameProgressionManager** - Currency, timers, zones
- **PlayerStats** - Stat upgrades
- **WeaponSystem** - Weapon management
- **GameManager** - General game state
- **DebugShopTester** - Testing tool (add this)

### Player
- `/Player` - Has PlayerController, PlayerHealth, PlayerCombat

### NPCs
- `/Base/NPCs/WeaponVendor` - Sells weapons
- `/Base/NPCs/StatVendor` - Sells upgrades
- `/Base/NPCs/ConsumableVendor` - (empty)
- `/Base/NPCs/SpecialVendor` - (empty)

### UI
- `/GameCanvas/ShopPanel` - Add SimpleShopUI here
- `/GameCanvas/CurrencyText` - Add CurrencyDisplay here
- `/GameCanvas/TimerText` - Add TimerDisplay here

---

## 🔧 Component Quick Add

### To add shop UI:
1. Select `/GameCanvas/ShopPanel`
2. Add Component → **SimpleShopUI**

### To add currency display:
1. Select `/GameCanvas/CurrencyText`
2. Add Component → **CurrencyDisplay**

### To add timer display:
1. Select `/GameCanvas/TimerText`
2. Add Component → **TimerDisplay**

### To add debug tester:
1. Select `/GameManagers`
2. Add Component → **DebugShopTester**

---

## 💰 Currency System API

### Get current currency:
```csharp
int amount = GameProgressionManager.Instance.Currency;
```

### Add currency:
```csharp
GameProgressionManager.Instance.AddCurrency(100);
```

### Spend currency:
```csharp
if (GameProgressionManager.Instance.SpendCurrency(50))
{
    // Purchase successful
}
```

### Listen to changes:
```csharp
GameProgressionManager.Instance.OnCurrencyChanged.AddListener(OnCurrencyUpdated);
```

---

## 📊 Player Stats API

### Get stat values:
```csharp
float speed = PlayerStats.Instance.GetMoveSpeed();
float health = PlayerStats.Instance.GetMaxHealth();
float damage = PlayerStats.Instance.GetDamage();
```

### Get upgrade levels:
```csharp
int level = PlayerStats.Instance.GetMoveSpeedLevel();
```

### Upgrade stats:
```csharp
PlayerStats.Instance.UpgradeMoveSpeed();
PlayerStats.Instance.UpgradeMaxHealth();
PlayerStats.Instance.UpgradeDamage();
```

---

## ⌨️ Debug Controls (in Play Mode)

**Add DebugShopTester to /GameManagers first!**

- **C** - Add 100 currency
- **1** - Buy Move Speed upgrade
- **2** - Buy Max Health upgrade
- **3** - Buy Damage upgrade
- **4** - Buy Crit Chance upgrade
- **5** - Buy Crit Damage upgrade
- **6** - Buy Attack Range upgrade
- **L** - Log current stats to console
- **H** - Show help message

---

## 🎮 Player Controls

- **WASD** - Move
- **Space** - Jump
- **Left Mouse** - Attack
- **Shift** - Sprint (if implemented)
- **E** - Interact with NPC (if input action exists)

---

## 📋 Upgrade Types

```csharp
public enum UpgradeType
{
    MoveSpeed,    // Increases movement speed
    MaxHealth,    // Increases max health
    Damage,       // Increases attack damage
    CritChance,   // Increases crit chance
    CritDamage,   // Increases crit multiplier
    AttackRange   // Increases attack range
}
```

---

## 🗡️ Weapon Effects

```csharp
public enum WeaponEffect
{
    None,         // No special effect
    Fire,         // Burn damage over time
    Ice,          // Slow enemies
    Lightning,    // Chain to nearby enemies
    Poison,       // Damage over time
    Lifesteal     // Heal on hit
}
```

---

## 🏪 Shop NPC Types

```csharp
public enum ShopNPCType
{
    WeaponVendor,        // Sells weapons
    StatUpgradeVendor,   // Sells stat upgrades
    ConsumableVendor,    // Sells consumables (not implemented)
    SpecialVendor        // Sells special items (not implemented)
}
```

---

## 🎯 Defense Zones

- **DefenseZone1** - First line (best bonuses)
- **DefenseZone2** - Second line (medium bonuses)
- **DefenseZone3** - Last line (minimal bonuses)

**Fallback Threshold:** 25% health  
Player can retreat to next zone when low health.

---

## ⏱️ Timers and Cooldowns

- **Base Timer:** 40 seconds (configurable in GameProgressionManager)
- **Wave Interval:** 5 seconds (configurable in WaveSpawner)
- **Shop Auto-Close:** Manual only (ESC or button)

---

## 💡 Common Tasks

### Create new weapon:
1. Right-click `/Assets/Data/Weapons/` → Create → WeaponData
2. Name it (e.g., "PoisonDagger")
3. Set properties (name, damage, effect, cost)
4. Add to NPC's Available Weapons array

### Create new upgrade:
1. Right-click `/Assets/Data/Upgrades/` → Create → UpgradeData
2. Name it (e.g., "SpeedBoost")
3. Set type, costs, max level, bonuses
4. Add to NPC's Available Upgrades array

### Add new NPC:
1. Create empty GameObject in `/Base/NPCs/`
2. Add **ShopNPC** component
3. Set NPC type and name
4. Assign weapons or upgrades
5. Assign ShopUI reference (`/GameCanvas/ShopPanel`)

---

## 🐛 Quick Troubleshooting

### Shop won't open:
- Check ShopUI assigned in NPC
- Check SimpleShopUI component on ShopPanel
- Check ShopPanel is disabled by default

### Items not showing:
- Check NPC has weapons/upgrades assigned
- Check Item List Container assigned
- Check Item Button Prefab assigned

### Currency not updating:
- Check CurrencyDisplay component added
- Check Currency Text field assigned
- Check GameProgressionManager exists

### Stats not increasing:
- Check PlayerStats singleton exists
- Check upgrade methods exist (GetMoveSpeedLevel, etc.)
- Check L key in debug tester to verify

---

## 📚 Document Quick Links

**First time?** → `START_HERE_SHOP_SETUP.md`  
**Full task list?** → `REMAINING_SETUP_TASKS.md`  
**UI structure?** → `SHOP_UI_HIERARCHY.md`  
**Project overview?** → `PROJECT_STATUS_OVERVIEW.md`  
**Architecture?** → `PROJECT_CONTEXT.md`  
**Testing?** → `QUICK_TEST_CHECKLIST.md`  
**Errors?** → `SCRIPT_COMPILATION_CHECKLIST.md`  

---

## 🎯 Current Priority

**1. Set up shop UI** (20 min)  
↓  
**2. Test shop interaction** (5 min)  
↓  
**3. Add HUD displays** (10 min)  
↓  
**4. Polish and content** (optional)

---

## ✅ Ready to Start?

Open **`START_HERE_SHOP_SETUP.md`** and follow the 8 steps!

Time: 20 minutes  
Difficulty: Easy  
Result: Fully working shop system
