# Upgrade & Defense System Implementation Guide

## Overview

This guide explains the new roguelike defense systems added to your project:
- Currency economy and rewards
- Player stats and upgrades
- Weapon system with special effects
- Base safe zone with shop
- 3-location defense system with fallback
- Between-wave timer for upgrades

---

## Core Systems

### 1. GameProgressionManager (Singleton)
**Purpose:** Manages game state, currency, defense zones, and base timer.

**Key Features:**
- Currency management (earn from kills, spend on upgrades)
- Defense zone tracking (3 zones with fallback)
- Base timer (30-45 seconds to upgrade between waves)
- Events for entering/exiting base

### 2. PlayerStats (Singleton)
**Purpose:** Manages all player stat upgrades.

**Upgradeable Stats:**
- Move Speed
- Max Health
- Base Damage
- Critical Chance
- Critical Damage
- Attack Range

**How it works:**
- Each stat has a base value + level-based bonuses
- Automatically applies stat changes to Player components
- Supports critical hit rolls with damage multipliers

### 3. WeaponSystem (Singleton)
**Purpose:** Manages equipped weapon and applies weapon effects.

**Weapon Stats:**
- Damage Multiplier
- Attack Speed Multiplier
- Range Multiplier
- Crit Chance/Damage Bonus

**Weapon Effects:**
- Bleed (damage over time)
- Burn (damage over time)
- Slow (reduce enemy speed)
- Stun (freeze enemy)
- Area Damage (splash damage)
- Life Steal (heal on hit)
- Chain Lightning (bounce to nearby enemies)

### 4. UpgradeShop
**Purpose:** Shop UI where players spend currency on upgrades.

**Features:**
- Opens automatically when entering base
- Pauses game (Time.timeScale = 0)
- Purchase upgrades (cost increases per level)
- Purchase weapons
- Close shop to start next wave

### 5. Defense Zone System
**Purpose:** 3 horizontal defense locations with fallback mechanic.

**How it works:**
- Start at Zone 1 (furthest from base)
- If player health drops below threshold, fallback to next zone
- Each zone closer to base has stronger perks
- Bonus rewards based on how many zones defended

### 6. Base Gate System
**Purpose:** Controls access to safe zone between waves.

**How it works:**
- Gate opens when returning to base after wave
- Gate closes when exiting base to start wave
- Animated gate movement (slides up/down)
- Collider blocks passage when closed

---

## Setup Instructions

### Step 1: Create Manager GameObject
1. Create empty GameObject named "GameManagers"
2. Add components:
   - `GameProgressionManager`
   - `PlayerStats`
   - `WeaponSystem`
   - `UpgradeShop`

### Step 2: Configure Defense Zones
1. Create 3 empty GameObjects:
   - "DefenseZone_1" (furthest from base)
   - "DefenseZone_2" (middle)
   - "DefenseZone_3" (closest to base - the base itself)

2. For each zone, add `DefenseZone` component:
   - Set Zone Index (0, 1, 2)
   - Assign Spawn Center (create child transform)
   - Set Spawn Radius (e.g., 15)
   - Configure Zone Perks (increasing closer to base):
     - Zone 1: 0% bonus
     - Zone 2: 25% damage/speed bonus
     - Zone 3: 50% damage/speed bonus
   - Link Next Zone (Zone1 → Zone2 → Zone3)

### Step 3: Create Base Area
1. Create cube for ground: "Base_Ground"
2. Create cube for gate: "Base_Gate"
   - Add `BaseGate` component
   - Set Open Height = 5
   - Set Animation Speed = 2
   - Add Box Collider

3. Create trigger zone: "Base_Trigger"
   - Add Box Collider (Is Trigger = true)
   - Add `BaseTrigger` component
   - Position at base entrance

### Step 4: Create Upgrade Data Assets
1. In Project window, right-click → Create → Game → Upgrade Data
2. Create upgrades for each stat type:
   - "Upgrade_MoveSpeed"
   - "Upgrade_MaxHealth"
   - "Upgrade_Damage"
   - "Upgrade_CritChance"
   - "Upgrade_CritDamage"
   - "Upgrade_AttackRange"

3. Configure each:
   - Set Upgrade Name and Description
   - Set Base Cost (e.g., 50, 75, 100)
   - Set Cost Increase Per Level (e.g., 1.5x)
   - Set Max Level (e.g., 10)
   - Set Upgrade Type

### Step 5: Create Weapon Data Assets
1. Right-click → Create → Game → Weapon Data
2. Create starter weapons:
   - "Weapon_BasicSword" (1x damage, no effects)
   - "Weapon_FireBlade" (1.2x damage, 20% burn chance)
   - "Weapon_IceSword" (1x damage, 30% slow chance)
   - "Weapon_StormHammer" (1.5x damage, 15% chain lightning)

3. Configure each:
   - Set Name, Description
   - Set Damage/Speed/Range Multipliers
   - Set Weapon Effect and Effect Chance
   - Set Purchase Cost

### Step 6: Link Everything
1. Select GameManagers GameObject
2. In `GameProgressionManager`:
   - Set Base Timer Duration (30-45 seconds)
   - Set Max Defense Zones = 3

3. In `UpgradeShop`:
   - Assign Available Upgrades array (drag upgrade assets)
   - Assign Available Weapons array (drag weapon assets)
   - Assign Shop UI (create UI later)

4. In Enemy prefab:
   - Set Currency Reward (e.g., 10-50 based on difficulty)

---

## Creating Shop UI (Simple Text-Based)

### Quick Console-Based Shop (For Testing)
The system works without UI - check Console for messages:
- Currency changes
- Purchase confirmations
- Timer countdown

### Future: Full UI
Create Canvas with:
- Currency display (top-right)
- Timer display (top-center)
- Upgrade buttons (grid layout)
- Weapon shop (separate panel)
- "Exit Shop" button

---

## Workflow Example

### Wave Flow:
1. **Player spawns in Base** (Gate open)
2. **Shop opens automatically** (40 second timer starts)
3. **Player buys upgrades** (spending currency from previous wave)
4. **Player exits base** OR **Timer expires**
5. **Gate closes** (wave starts)
6. **Enemies spawn at active Defense Zone**
7. **Player fights** (earns currency per kill)
8. **If health low:** Fallback to next zone (closer to base)
9. **Wave complete** (gate opens, player returns to base)
10. **Repeat** with harder waves

### Currency Flow:
- Kill enemy → Earn 10-50 currency
- Return to base → Spend currency on upgrades
- Upgrades get more expensive each level
- Weapons are one-time purchases

### Defense Zone Flow:
- Start at Zone 1 (far from base, no perks)
- Health drops below 25% → Teleport to Zone 2 (+25% perks)
- Health drops again → Teleport to Zone 3 / Base (+50% perks)
- Lose Zone 3 → Game Over
- Complete defense → Bonus based on zones held

---

## Integration with Existing Systems

### WaveSpawner Integration
Update `WaveSpawner.cs` to:
- Check active Defense Zone for spawn position
- Only spawn when gate is closed (not in base)
- Trigger base gate opening when wave completes

### GameManager Integration
- Keep existing pause/restart functionality
- Add progression tracking
- Add wave completion detection

---

## Testing Checklist

- [ ] Currency increases when killing enemies
- [ ] Base gate opens when entering base trigger
- [ ] Shop opens automatically when entering base
- [ ] Timer counts down from 40 seconds
- [ ] Upgrades cost currency and increase stats
- [ ] Gate closes when exiting base or timer expires
- [ ] Weapons apply multipliers to damage
- [ ] Defense zones activate in sequence
- [ ] Fallback teleports player to next zone
- [ ] Bonus currency awarded based on zones defended

---

## Next Steps

1. **Test core systems** with console debugging
2. **Create shop UI** using Unity UI Toolkit or uGUI
3. **Add weapon visual effects** (particles for effects)
4. **Implement status effects** (bleed, burn, slow DOT)
5. **Add spawn pooling** for performance
6. **Create more weapon varieties** with unique effects
7. **Balance costs and rewards** through playtesting
8. **Add visual feedback** (damage numbers, crit indicators)

---

## Code Reference

### Currency Example:
```csharp
// Add currency
GameProgressionManager.Instance.AddCurrency(50);

// Spend currency
bool success = GameProgressionManager.Instance.SpendCurrency(100);

// Check currency
int current = GameProgressionManager.Instance.Currency;
```

### Stats Example:
```csharp
// Get player damage with crits
float damage = PlayerStats.Instance.CalculateFinalDamage();

// Check if crit
bool isCrit = PlayerStats.Instance.RollCritical();

// Get specific stat
float moveSpeed = PlayerStats.Instance.GetMoveSpeed();
```

### Weapon Example:
```csharp
// Equip weapon
WeaponSystem.Instance.EquipWeapon(weaponData);

// Get damage multiplier
float mult = WeaponSystem.Instance.GetDamageMultiplier();

// Apply weapon effect
WeaponSystem.Instance.TryApplyWeaponEffect(enemy.gameObject);
```

---

## Architecture Benefits

✅ **Data-Driven:** Upgrades and weapons are ScriptableObjects (no code changes for balance)  
✅ **Modular:** Each system is independent and can be extended  
✅ **Scalable:** Easy to add new upgrade types, weapons, effects  
✅ **Event-Based:** Systems communicate via UnityEvents (loosely coupled)  
✅ **Singleton Pattern:** Global access where needed (managers)  
✅ **Designer-Friendly:** All values exposed in Inspector for tuning  

