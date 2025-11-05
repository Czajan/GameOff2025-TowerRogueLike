# Defense Zone Fallback System - Setup Guide

## Overview
A multi-zone defense system where enemies attack destructible objectives. When an objective is destroyed, enemies automatically relocate to attack the next zone while you manually retreat.

---

## Architecture

### Zone Flow
```
Zone 1 → Zone 2 → Zone 3 → Game Over
  ↓        ↓        ↓
Obj1     Obj2     Obj3
500 HP   500 HP   500 HP
```

### Enemy AI Priority System
1. **Player within 20 units** → Attack Player (detection range applies)
2. **Player far away (>20 units)** → Attack Active Objective (NO range limit - they will pursue across entire map)
3. **Objective destroyed** → All enemies automatically relocate to next zone and retarget new objective
4. **Player must manually retreat** - No automatic teleportation

---

## Setup Instructions

### 1. Create Defense Objectives

For each DefenseZone (1, 2, 3):

**A. Create GameObject:**
- Parent: `/DefenseZones/DefenseZone1` (or 2, 3)
- Type: 3D Object → Cube/Cylinder
- Name: `DefenseObjective1` (match zone number)
- Position: Center of zone (same X, Z as zone's SpawnCenter)
- Scale: `(2, 2, 2)` or larger for visibility

**B. Add DefenseObjective Component:**
- Add Component → `DefenseObjective`
- **Max Health:** `500` (adjust for difficulty)
- **Objective Name:** `"Zone 1 Defense Point"`
- **Objective Renderer:** Auto-assigned if MeshRenderer on same GameObject
- **Colors:**
  - Healthy Color: Green `(0, 1, 0)`
  - Damaged Color: Yellow `(1, 1, 0)`
  - Critical Color: Red `(1, 0, 0)`

**C. Add Material (Optional):**
- Create Material in `/Assets/Materials/DefenseObjectiveMat.mat`
- Assign to MeshRenderer
- Material color will be overridden by DefenseObjective script

---

### 2. Link Objectives to Zones

For each DefenseZone:

**DefenseZone1:**
- Select `/DefenseZones/DefenseZone1`
- **Defense Objective** field → Drag `DefenseObjective1` child

**DefenseZone2:**
- Select `/DefenseZones/DefenseZone2`
- **Defense Objective** field → Drag `DefenseObjective2` child

**DefenseZone3:**
- Select `/DefenseZones/DefenseZone3`
- **Defense Objective** field → Drag `DefenseObjective3` child

---

### 3. Configure Zone Fallback Chain

**DefenseZone1:**
- **Next Zone:** Drag `/DefenseZones/DefenseZone2`

**DefenseZone2:**
- **Next Zone:** Drag `/DefenseZones/DefenseZone3`

**DefenseZone3:**
- **Next Zone:** `None` (last zone - triggers game over)

---

### 4. Configure WaveSpawner

Find WaveSpawner GameObject (usually in `/GameManagers`):

**WaveSpawner Component:**
- **Active Defense Zone:** Drag `/DefenseZones/DefenseZone1`

This ensures enemies spawn near Zone 1 initially.

---

### 5. Update Enemy Prefab

Open `/Assets/Prefabs/Enemy.prefab`:

**EnemyAI Component:**
- **Detection Range:** `15`
- **Player Priority Range:** `20` ← NEW
- **Prioritize Player:** `✅ Checked` ← NEW
- **Attack Damage:** `10` (damages both player and objectives)
- **Attack Range:** `2`
- **Attack Cooldown:** `1.5`

**Save the prefab.**

---

### 6. Create Objective Health UI (Optional)

**A. Create UI Canvas:**
- Right-click Hierarchy → UI → Canvas
- Name: `ObjectiveHealthCanvas`
- Canvas Scaler → UI Scale Mode: Scale With Screen Size

**B. Create Health Bar:**
- Right-click Canvas → UI → Slider
- Name: `ObjectiveHealthSlider`
- Position: Top-center of screen
- Width: 400, Height: 40

**C. Configure Slider:**
- **Min Value:** `0`
- **Max Value:** `1`
- **Value:** `1`
- **Interactable:** `❌ Unchecked`

**D. Add Text (Optional):**
- Right-click Slider → UI → Text - TextMeshPro
- Name: `HealthText`
- Position: Center of slider
- Anchor: Stretch

**E. Add ObjectiveHealthUI Script:**
- Select `ObjectiveHealthSlider`
- Add Component → `ObjectiveHealthUI`
- **Target Objective:** Drag `DefenseObjective1`
- **Health Slider:** Drag the Slider component
- **Health Text:** Drag the TextMeshPro text
- **Fill Image:** Drag Slider → Fill Area → Fill (Image component)

---

## Testing the System

### Test Scenario 1: Normal Fallback
1. Start the game
2. Exit base (timer expires or manual exit)
3. Stay away from the objective (> 20 units)
4. Observe enemies attacking DefenseObjective1
5. Wait for objective HP to reach 0
6. **Expected:**
   - Zone 1 lost message + UI notification: "ZONE 1 OBJECTIVE DESTROYED! Retreat to Zone 2!"
   - All enemies relocated to Zone 2 spawn point
   - Enemies now targeting DefenseObjective2 (even from across the map)
   - **You must manually run to Zone 2** - no teleportation
   - DefenseObjective2 is now active and vulnerable

### Test Scenario 2: Player Priority
1. Stand near the objective (< 20 units)
2. Observe enemies ignoring objective and attacking you
3. Run far away (> 20 units)
4. **Expected:** Enemies switch to attacking objective
5. **Note:** Enemies will pursue objective regardless of how far you go (no detection range limit for objectives)

### Test Scenario 3: Final Zone
1. Let Zone 1 and Zone 2 objectives be destroyed
2. Manually retreat to Zone 3 each time
3. Defend or let DefenseObjective3 be destroyed
4. **Expected:**
   - "FINAL OBJECTIVE DESTROYED! GAME OVER!" notification
   - `GameProgressionManager.OnDefenseFailed` event fires

---

## Debug Console Messages

### Zone Activation
```
<color=cyan>⚡ Defense Zone 1 ACTIVATED! Defend the objective!</color>
```

### Objective Taking Damage
```
Zone 1 Defense Point took 10 damage! Health: 490/500 (98%)
```

### Objective Destroyed
```
<color=red>⚠️ Zone 1 Defense Point DESTROYED! Falling back...</color>
<color=red>⚠️ ZONE 1 LOST! Falling back...</color>
```

### Objective Destroyed
```
<color=red>⚠️ Zone 1 Defense Point DESTROYED! Falling back...</color>
<color=red>⚠️ ZONE 1 LOST! Falling back...</color>
UI Notification: "ZONE 1 OBJECTIVE DESTROYED! Retreat to Zone 2!"
```

### Enemy Relocation
```
<color=yellow>Relocated 5 enemies to Zone 2</color>
<color=yellow>WaveSpawner: Zone changed to 2. Enemies will now spawn there.</color>
Enemy found active objective: DefenseObjective2
```

### Enemy Behavior
```
<color=cyan>Enemy TryAttack: Distance=1.45, AttackRange=2, Target=DefenseObjective2</color>
<color=orange>Enemy attacked DefenseObjective2 for 10 damage!</color>
```

**Note:** Enemies will now pursue objectives from any distance - no detection range limit!

---

## Common Issues

### Issue: Enemies ignore objective
**Cause:** `DefenseObjective` not assigned or destroyed
**Fix:** Check DefenseZone component has DefenseObjective linked

### Issue: No fallback when objective destroyed
**Cause:** Next Zone not assigned
**Fix:** DefenseZone1 → Next Zone must point to DefenseZone2

### Issue: Enemies won't move to Zone 2 objective
**Cause:** Detection range limiting pursuit
**Fix:** System now updated - enemies always pursue active objective regardless of distance

### Issue: Player teleported when objective destroyed
**Cause:** Old behavior
**Fix:** System updated - player must manually retreat to next zone

### Issue: Visual feedback not working
**Cause:** Objective Renderer not assigned
**Fix:** DefenseObjective → Objective Renderer must reference the MeshRenderer

### Issue: Zone objectives all active at start
**Cause:** Only Zone 1 should start active
**Fix:** DefenseZone script auto-deactivates objectives for zones > 0

---

## Balancing Tips

### Objective Health
- **Easy:** 1000 HP per objective
- **Medium:** 500 HP per objective (default)
- **Hard:** 250 HP per objective

### Enemy Damage vs Objective
- Use same `attackDamage` for both player and objective
- Or create separate field: `objectiveDamage`

### Player Priority Range
- **20 units:** Balanced (player can kite away to make enemies switch to objective)
- **30 units:** Easier (enemies chase player more, less objective pressure)
- **10 units:** Harder (must be very close to divert enemies from objective)

### Detection Range (Player Only)
- **15 units (default):** Enemies detect player and engage within this range
- **Objective Targeting:** NO range limit - enemies pursue objectives from across entire map
- **This creates strategic choice:** Engage enemies to save objective, or kite them away

---

## Future Enhancements

- **Repair Mechanic:** Player can repair objectives with currency
- **Zone Perks:** Bonuses for holding Zone 1 longer
- **Timed Fallback:** Auto-fallback after X minutes
- **Multi-Objective:** Multiple objectives per zone
- **Objective Types:** Different objectives with unique abilities

---

## Script Reference

### DefenseObjective.cs
- **Purpose:** Destructible target for enemies
- **Events:** `OnHealthChanged(float)`, `OnObjectiveDestroyed()`
- **Properties:** `HealthPercentage`, `CurrentHealth`, `MaxHealth`, `IsDestroyed`, `Position`
- **Methods:** `TakeDamage(float)`

### DefenseZone.cs
- **Purpose:** Zone manager with fallback logic
- **New Method:** `OnObjectiveDestroyed()` - Called when objective dies
- **New Method:** `GetDefenseObjective()` - Returns linked objective
- **New Property:** `ZoneIndex` - Zone number (0, 1, 2)

### EnemyAI.cs
- **Purpose:** Enemy behavior with smart targeting
- **New Method:** `SetDefenseZone(DefenseZone)` - Assign target zone
- **New Method:** `FindDefenseObjective()` - Locate active objective
- **New Method:** `UpdateTarget()` - Switch between player/objective
- **New Method:** `GetHorizontalDistance()` - 2D distance calculation (ignores Y-axis)
- **Updated Behavior:** Always pursues objectives regardless of distance; detection range only applies to player detection

### WaveSpawner.cs
- **Purpose:** Enemy spawning with zone awareness
- **New Method:** `SetActiveZone(DefenseZone)` - Change spawn location
- **New Property:** `Instance` - Singleton access

### ObjectiveHealthUI.cs
- **Purpose:** Display objective health bar
- **Method:** `SetTargetObjective(DefenseObjective)` - Switch to new objective UI

---

## Event Flow Diagram

```
DefenseObjective HP → 0
         ↓
OnObjectiveDestroyed.Invoke()
         ↓
DefenseZone.OnObjectiveDestroyed()
         ↓
    FallbackToNextZone()
         ↓
    ┌────┴────┬────────────┬─────────────┐
    ↓         ↓            ↓             ↓
Deactivate  Activate    Show UI      Relocate
  Zone 1     Zone 2   Notification   Enemies
    ↓         ↓            ↓             ↓
GameProgressionManager.FallbackToNextZone()
    ↓
WaveSpawner.SetActiveZone(Zone 2)
    ↓
Future enemies spawn at Zone 2
    ↓
Player manually retreats to Zone 2
```

---

**System is now complete and ready for testing!** 🎮
