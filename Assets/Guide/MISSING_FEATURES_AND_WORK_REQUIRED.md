# Missing Features & Work Required

## 📋 Overview

This document provides a complete overview of **what's been built** and **what still needs to be done** to complete your roguelike defense game.

**Current Project Status: ~35% Complete**

---

## ✅ COMPLETED SYSTEMS (What's Working)

### 1. Basic Game Loop ✅
- Player movement and controls
- Camera system
- Base area with entry/exit
- Wave timer system (30-45 seconds between waves)
- Run start/end flow

### 2. Defense Zone System ✅
- 3 defense zones implemented
- Zone health tracking
- Zone fallback mechanics (Zone 1 → Zone 2 → Zone 3)
- Zone perks (damage, attack speed, move speed bonuses)
- Loss condition when all zones fail

### 3. Combat System ✅
- Player basic attack
- Cursor-based aiming (isometric view)
- Enemy AI (chase and attack)
- Health systems (player & enemies)
- Damage calculation with stats
- Critical hits
- Weapon system with effects (bleed, burn, slow, stun, lifesteal, chain lightning)

### 4. Currency & Shop ✅
- In-run currency system
- Currency drops from enemies
- Shop NPCs (weapon vendor, stat vendor)
- Weapon purchase system
- Stat upgrade shop (between waves)

### 5. Wave Spawning ✅
- Wave-based enemy spawning
- Difficulty scaling (more enemies per wave)
- Random spawn positions
- Enemy targeting system

### 6. Player Stats ✅
- Base stats (health, damage, speed, crit)
- Stat upgrade system
- Weapon stat modifiers
- Temporary stat bonuses (in-run)

### 7. **NEW: Level-Up Upgrade System** ✅ **(Just Implemented!)**
- Milestone level upgrades (every 5 levels: 5, 10, 15, 20...)
- 3 random upgrade choices with rarity system
- Common (70%), Rare (25%), Legendary (5%)
- Stat boosts: Damage, Health, Speed, Crit, Attack Speed
- Special abilities: Double Jump, Dash, Lifesteal, Thorns, Explosions
- Stacking upgrades (same upgrade multiple times)
- Game pause during selection
- ScriptableObject-based upgrade definitions

---

## ❌ CRITICAL MISSING SYSTEMS (Must Have)

These are **essential** to have a playable roguelike game.

---

### 🔴 **PRIORITY 1: Experience & Leveling System**

**Why Critical:** Players need progression during runs. This is core to roguelike gameplay.

#### What's Missing:
- [ ] **Experience Points (XP) System**
  - Track XP from enemy kills
  - XP requirements per level
  - Exponential XP scaling
  
- [ ] **Experience Orbs**
  - Visual orbs that drop from dead enemies
  - Auto-collection when player is near
  - Magnetic pull effect
  
- [ ] **XP Bar UI**
  - Show current XP / required XP
  - Visual fill bar on HUD
  - Level-up notification

- [ ] **Level Tracking**
  - Current player level
  - Reset each run
  - Max level cap (e.g., 30)

#### Files to Create:
- `ExperienceOrb.cs` - Orb pickup behavior
- `ExperienceBar.cs` - XP bar UI component

#### Files to Modify:
- ✅ `ExperienceSystem.cs` - **Already exists!** Just needs orb integration
- `EnemyHealth.cs` - Spawn XP orbs on death
- `GameUI.cs` - Add XP bar to HUD

#### Estimated Work: **4-6 hours**

**Note:** You already have `ExperienceSystem.cs` and the upgrade system! You just need to:
1. Create XP orb prefabs
2. Make enemies drop orbs on death
3. Add XP bar to HUD
4. Hook up orb collection

---

### 🔴 **PRIORITY 2: Elite Enemies & Active Skills from Chests**

**Why Critical:** Core replayability feature. Without active skills, runs feel same-y.

#### What's Missing:
- [ ] **Elite Enemy Type**
  - Tougher than normal enemies
  - Different visual (larger, glowing, etc.)
  - Higher health and damage
  - Spawn 1-2 per wave
  
- [ ] **Boss Enemy Type**
  - End-of-wave boss (every 5 waves?)
  - Much higher health
  - Unique attacks or abilities
  - Guaranteed chest drop
  
- [ ] **Chest Drop System**
  - Animated chest prefab
  - Drops from elite/boss on death
  - Player interacts to open
  - Chest opening UI

- [ ] **Active Skill System**
  - ScriptableObject skill database
  - 2-4 skills offered per chest
  - Rarity-based selection (like level upgrades)
  - Hotkey activation (Q, E, R, etc.)
  - Cooldown system
  - Visual effects for skills

- [ ] **Skill Pool (10-15 skills minimum)**
  Examples:
  - Fireball (shoot projectile)
  - Dash (quick movement)
  - Shield (temporary invulnerability)
  - Area blast (damage around player)
  - Healing wave
  - Time slow
  - Summon turret
  - Chain lightning
  - Ground slam

#### Files to Create:
- `EliteEnemy.cs` - Elite enemy behavior
- `BossEnemy.cs` - Boss enemy behavior (can extend EliteEnemy)
- `ChestDrop.cs` - Chest spawning and interaction
- `ActiveSkillData.cs` - ScriptableObject for skills
- `ActiveSkillSystem.cs` - Manage active skills
- `SkillCooldownUI.cs` - Show skill icons and cooldowns
- `ChestOpeningUI.cs` - Skill selection UI (similar to LevelUpgradeSelectionUI)

#### Files to Modify:
- `EnemyHealth.cs` - Spawn chest on elite/boss death
- `WaveSpawner.cs` - Spawn elites and bosses
- `PlayerCombat.cs` or new `SkillCaster.cs` - Activate skills

#### Estimated Work: **12-16 hours**

**Note:** You can reuse the upgrade UI pattern for chest skill selection!

---

### 🔴 **PRIORITY 3: Obstacle/Tower Deployment**

**Why Critical:** This is what makes it a "defense" game. Without it, it's just a survivor game.

#### What's Missing:
- [ ] **Obstacle Placement System**
  - Grid-based or free-form placement
  - Placement preview (ghost model)
  - Placement validation (can't place on enemies, player, etc.)
  - Placement cost (in-run currency)
  
- [ ] **Obstacle Types (Start with 3-5)**
  - **Wall** - Blocks enemy path
  - **Damage Tower** - Auto-shoots nearby enemies
  - **Slow Trap** - Slows enemies that walk over it
  - **Spike Trap** - Damages enemies that walk over it
  - **Healing Station** - Heals player when nearby (optional)
  
- [ ] **Obstacle Management**
  - Remove/sell obstacles (get partial refund)
  - Upgrade obstacles (increase stats)
  - Obstacle health/durability (optional)
  - Obstacle limit (max 10-20 active)
  
- [ ] **Pathfinding Integration**
  - NavMeshObstacle on placed obstacles
  - Dynamic NavMesh updates
  - Enemies repath when obstacles placed

- [ ] **Obstacle UI**
  - Radial menu or bottom toolbar
  - Show available obstacles
  - Show costs and stats
  - Placement mode toggle

#### Files to Create:
- `ObstacleData.cs` - ScriptableObject for obstacle types
- `ObstaclePlacementSystem.cs` - Main placement logic
- `ObstaclePlacer.cs` - Player-controlled placement
- `Obstacle.cs` - Base class for all obstacles
- `Wall.cs`, `DamageTower.cs`, `SlowTrap.cs`, etc. - Specific obstacle types
- `ObstaclePlacementUI.cs` - UI for obstacle selection
- `ObstaclePreview.cs` - Ghost preview when placing

#### Files to Modify:
- `CurrencyManager.cs` - Deduct cost when placing
- Enemy AI - React to NavMeshObstacle changes

#### Estimated Work: **16-20 hours** (major system)

**Note:** Start simple with just walls and one tower type. Expand later.

---

### 🟡 **PRIORITY 4: Visual Feedback & Polish**

**Why Important:** Makes the game feel good to play. Huge impact on player experience.

#### What's Missing:
- [ ] **Damage Numbers**
  - Floating text showing damage dealt
  - Different colors for crits, DoT, etc.
  - Number animation (rise and fade)
  
- [ ] **Hit Effects**
  - Particle effects on hit
  - Flash enemy red when damaged
  - Screen shake on big hits
  - Knockback effects
  
- [ ] **XP Orb Visual**
  - Glowing sphere with particle trail
  - Color-coded by XP value (small, medium, large)
  - Magnetic pull animation
  
- [ ] **Weapon Visual**
  - Show equipped weapon model on player
  - Weapon swing/attack animation
  - Weapon trail effects
  
- [ ] **Skill Effects**
  - Particle systems for each skill
  - Sound effects
  - Camera effects (shake, zoom, etc.)
  
- [ ] **UI Polish**
  - Smooth transitions
  - Button hover effects
  - Level-up animation
  - Victory/defeat animations

#### Files to Create:
- `DamageNumbers.cs` - Floating damage text
- `HitEffects.cs` - Particle effects on hits
- `WeaponVisual.cs` - Show/hide weapon models

#### Files to Modify:
- `PlayerCombat.cs` - Trigger hit effects
- `EnemyHealth.cs` - Spawn damage numbers
- `VisualFeedback.cs` - **Already exists!** Expand it

#### Estimated Work: **8-12 hours**

---

## ⚠️ OPTIONAL SYSTEMS (Nice to Have)

These enhance the game but aren't critical for initial launch.

---

### 🟢 **OPTIONAL 1: Hero Selection System**

**What:** Multiple playable characters with different stats/abilities.

#### What's Missing:
- [ ] Multiple hero classes (Warrior, Ranger, Mage, etc.)
- [ ] Hero selection UI (main menu)
- [ ] Hero-specific stats and abilities
- [ ] Hero unlock system
- [ ] Hero preview and stat display

#### Files to Create:
- `HeroData.cs` - ScriptableObject for heroes
- `HeroSelectionUI.cs` - Selection screen
- `HeroManager.cs` - Track unlocked heroes

#### Estimated Work: **10-12 hours**

---

### 🟢 **OPTIONAL 2: Meta-Progression**

**What:** Persistent upgrades between runs.

#### What's Missing:
- [ ] **Meta-Currency**
  - Separate from in-run currency
  - Earned even on loss
  - Persists between runs
  
- [ ] **Permanent Upgrades**
  - Buy stat boosts that persist
  - Unlock starting weapons
  - Unlock starting perks
  
- [ ] **Save System**
  - Save meta-currency
  - Save unlocks
  - Save hero progress
  - Load on game start

- [ ] **Main Menu**
  - Meta shop
  - Hero selection
  - Settings
  - Tutorial

#### Files to Create:
- `MetaCurrency.cs` - Persistent currency
- `MetaUpgradeShop.cs` - Meta upgrade shop
- `SaveSystem.cs` - Save/load data
- `PersistentData.cs` - Save file structure
- `MainMenuUI.cs` - Main menu

#### Files to Modify:
- `GameManager.cs` - Load save on start

#### Estimated Work: **12-16 hours**

---

### 🟢 **OPTIONAL 3: More Enemy Types**

**What:** Variety of enemy behaviors for more interesting combat.

#### What's Missing:
- [ ] **Ranged Enemies** - Shoot from distance
- [ ] **Fast Enemies** - Quick but weak
- [ ] **Tank Enemies** - Slow but high HP
- [ ] **Flying Enemies** - Fly over obstacles (if implemented)
- [ ] **Spawner Enemies** - Spawn smaller enemies
- [ ] **Healer Enemies** - Heal nearby enemies

#### Files to Create:
- `RangedEnemy.cs`
- `FastEnemy.cs`
- `TankEnemy.cs`
- etc.

#### Files to Modify:
- `WaveSpawner.cs` - Spawn different enemy types
- `EnemyData.cs` - Add enemy type enum

#### Estimated Work: **6-8 hours** (for 3-4 types)

---

### 🟢 **OPTIONAL 4: More Maps/Zones**

**What:** Different battlefields for variety.

#### What's Missing:
- [ ] Multiple map layouts
- [ ] Map selection UI
- [ ] Map-specific obstacles/hazards
- [ ] Environmental effects (weather, time of day, etc.)

#### Estimated Work: **8-12 hours** per map

---

### 🟢 **OPTIONAL 5: Audio System**

**What:** Sound effects and music.

#### What's Missing:
- [ ] Background music
- [ ] Sound effects (attack, hit, level up, etc.)
- [ ] Audio manager
- [ ] Volume controls

#### Estimated Work: **4-6 hours** (implementation, not creation)

---

### 🟢 **OPTIONAL 6: Advanced Features**

- [ ] **Daily Challenges** - Special runs with modifiers
- [ ] **Leaderboards** - High score tracking
- [ ] **Achievements** - Unlock system
- [ ] **Run Mutations** - Difficulty modifiers (harder = better rewards)
- [ ] **Skill Synergies** - Bonus when specific skills combine
- [ ] **Tutorial System** - Teach new players
- [ ] **Settings Menu** - Graphics, controls, audio
- [ ] **Pause Menu** - In-run pause
- [ ] **Performance Optimization** - Object pooling, LOD, etc.

---

## 📊 WORK BREAKDOWN

### Time Estimates Summary

| System | Priority | Estimated Time | Complexity |
|--------|----------|----------------|------------|
| **XP Orb System** | 🔴 Critical | 4-6 hours | Low |
| **Elite/Boss Enemies** | 🔴 Critical | 6-8 hours | Medium |
| **Active Skill System** | 🔴 Critical | 6-8 hours | Medium |
| **Obstacle System** | 🔴 Critical | 16-20 hours | High |
| **Visual Feedback** | 🟡 Important | 8-12 hours | Medium |
| **Hero Selection** | 🟢 Optional | 10-12 hours | Medium |
| **Meta-Progression** | 🟢 Optional | 12-16 hours | High |
| **More Enemy Types** | 🟢 Optional | 6-8 hours | Low |
| **Audio System** | 🟢 Optional | 4-6 hours | Low |

**Total Critical Work Remaining: ~40-54 hours** (MVP)  
**Total Optional Work: ~32-42 hours** (Full game)

---

## 🎯 RECOMMENDED DEVELOPMENT ORDER

### Phase 1: Core Gameplay Loop (MVP)
**Goal:** Make the game actually playable and fun.

1. **XP Orbs & Collection** (4-6 hours)
   - Already have ExperienceSystem.cs
   - Add orb prefabs
   - Make enemies drop orbs
   - Add XP bar to HUD

2. **Visual Feedback** (8-12 hours)
   - Damage numbers
   - Hit effects
   - Basic particles

3. **Elite Enemies** (6-8 hours)
   - Create elite enemy variant
   - Spawn 1-2 per wave
   - Drop chests

4. **Active Skills (Basic)** (6-8 hours)
   - Start with 5-6 simple skills
   - Chest opening UI (reuse upgrade UI)
   - Skill activation system
   - Cooldown UI

**Phase 1 Total: ~24-34 hours**

### Phase 2: Defense Mechanics
**Goal:** Add tower defense elements.

5. **Obstacle System (Basic)** (16-20 hours)
   - Start with walls and damage towers
   - Grid-based placement
   - Purchase with currency
   - NavMesh integration

**Phase 2 Total: ~16-20 hours**

### Phase 3: Polish & Variety
**Goal:** Make it replayable and polished.

6. **More Skills** (4-6 hours)
   - Expand to 15-20 skills
   - Add more variety

7. **More Enemy Types** (6-8 hours)
   - Add 3-4 enemy variants
   - Different behaviors

8. **Boss Enemies** (4-6 hours)
   - Every 5 waves
   - Unique attacks

**Phase 3 Total: ~14-20 hours**

### Phase 4: Meta-Progression (Optional)
**Goal:** Long-term player retention.

9. **Hero Selection** (10-12 hours)
10. **Meta-Currency & Upgrades** (12-16 hours)
11. **Save System** (included in #10)

**Phase 4 Total: ~22-28 hours**

### Phase 5: Final Polish
12. Audio system
13. Settings menu
14. Tutorial
15. Performance optimization

---

## 🐛 KNOWN ISSUES TO FIX

### Critical Bugs:
- [ ] **File Conflict:** `/Assets/Scripts/Systems/Upgrades/UpgradeData.cs` conflicts with `/Assets/Scripts/Systems/UpgradeData.cs`
  - **Fix:** Delete `/Assets/Scripts/Systems/Upgrades/UpgradeData.cs` (duplicate)
  - **Use:** `/Assets/Scripts/Systems/Upgrades/LevelUpgradeData.cs` instead

### Minor Issues:
- [ ] No dash input configured in Input System
- [ ] Zone health UI could be more visible
- [ ] Enemy spawn points might overlap with zones

---

## 📝 IMPLEMENTATION NOTES

### What You Already Have:
✅ `ExperienceSystem.cs` - XP tracking, level-up events  
✅ `LevelUpgradeData.cs` - ScriptableObject for level upgrades  
✅ `UpgradeSystem.cs` - Manages upgrade selection and application  
✅ `UpgradeSelectionUI.cs` - UI for choosing upgrades  
✅ `PlayerController.cs` - Has double jump & dash functionality  
✅ `PlayerStats.cs` - Stat management with temporary bonuses  
✅ `DefenseZone.cs` - Zone mechanics  
✅ `WaveSpawner.cs` - Wave-based spawning  
✅ `CurrencyManager.cs` - Currency system  
✅ `ShopNPC.cs` - Shop system  

### What Can Be Reused:
- **Upgrade UI → Chest UI:** The `UpgradeSelectionUI` can be duplicated for chest skill selection
- **LevelUpgradeData → ActiveSkillData:** Same pattern for ScriptableObjects
- **UpgradeSystem → ActiveSkillSystem:** Similar architecture
- **Enemy AI → Elite AI:** Extend `EnemyAI.cs` for elite behavior

---

## 🚀 QUICK START: What to Build Next

### Option A: Fastest Playable Version (XP System)
1. Create `ExperienceOrb.cs` prefab (glowing sphere)
2. Modify `EnemyHealth.OnDeath()` to spawn orb
3. Add XP bar to `GameUI.cs`
4. Test: Kill enemies → get XP → level up → choose upgrade
5. **Result:** Full level-up loop working!

### Option B: Most Fun Addition (Active Skills)
1. Create `EliteEnemy.cs` (just higher HP + different material)
2. Create `ChestDrop.cs` prefab (simple cube or model)
3. Copy `UpgradeSelectionUI.cs` → `ChestOpeningUI.cs`
4. Create 3-5 simple skills (dash, fireball, heal)
5. **Result:** Elite enemies drop chests with active skills!

### Option C: True Defense Game (Obstacles)
1. Create `Wall.cs` (NavMeshObstacle + health)
2. Create `ObstaclePlacer.cs` (raycast placement)
3. Add UI button to toggle placement mode
4. **Result:** Can place walls to block enemies!

---

## 💡 DESIGN RECOMMENDATIONS

### For Active Skills:
- **Start Simple:** Dash, Heal, Fireball, Shield, Area Damage
- **Add Variety Later:** Summons, buffs, debuffs, terrain effects
- **Balance:** Skills should feel powerful but not game-breaking
- **Cooldowns:** 5-15 seconds for most skills

### For Obstacles:
- **Keep it Simple:** Grid-based placement is easier than free-form
- **Cost Balance:** Walls = cheap, Towers = expensive
- **Limit Count:** Max 15-20 obstacles to prevent spam
- **Visuals:** Use simple cubes/cylinders initially, replace later

### For Elites/Bosses:
- **Visual Clarity:** Make them OBVIOUS (larger, glowing, different color)
- **Fair Difficulty:** 3-5x normal enemy health
- **Guaranteed Rewards:** Always drop chest
- **Spawn Timing:** 1 elite per wave, 1 boss every 5 waves

---

## ✅ ACTION ITEMS

### Immediate (This Week):
1. ✅ Fix file conflict: Delete duplicate `UpgradeData.cs` in `/Upgrades/` folder
2. ✅ Test level-up upgrade system
3. ⬜ Create XP orb system (4-6 hours)
4. ⬜ Add XP bar to HUD (1-2 hours)

### Short Term (Next 2 Weeks):
5. ⬜ Create elite enemies (6-8 hours)
6. ⬜ Implement active skills (6-8 hours)
7. ⬜ Add damage numbers and hit effects (4-6 hours)

### Medium Term (Next Month):
8. ⬜ Obstacle placement system (16-20 hours)
9. ⬜ More enemy types (6-8 hours)
10. ⬜ Boss enemies (4-6 hours)

### Long Term (Optional):
11. ⬜ Hero selection
12. ⬜ Meta-progression
13. ⬜ Additional maps
14. ⬜ Audio system

---

## 🎉 CONCLUSION

**You have ~35% of a complete roguelike defense game built!**

### What's Great:
✅ Core combat works  
✅ Defense zones work  
✅ Level-up upgrades work  
✅ Shop system works  
✅ Wave spawning works  

### What's Needed for MVP:
🔴 XP orbs (4-6 hours)  
🔴 Active skills from chests (12-16 hours)  
🔴 Obstacle placement (16-20 hours)  
🟡 Visual polish (8-12 hours)  

**Total MVP Time: ~40-54 hours of work**

### Next Immediate Step:
**Build the XP orb system!** It's the quickest way to complete the level-up loop and make the game feel like a proper roguelike.

---

**Good luck! You're closer than you think!** 🚀
