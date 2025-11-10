# Next Features Roadmap

## 🎯 Current Status

### ✅ Completed Systems
- [x] Player movement, combat, health
- [x] Enemy AI and spawning
- [x] Wave management
- [x] Currency system (earn from kills)
- [x] 6 player stats (upgradeable)
- [x] Weapon system with 8 effect types
- [x] Defense zones with fallback
- [x] Base gate with auto-close and instant barrier
- [x] Run state management (pre-run/active run)
- [x] **Centralized interaction notification UI** ✨
- [x] **Generic NPC interaction component** ✨

---

## 📋 Implementation Priority Order

Based on the GAME_DESIGN_DOCUMENT.md, here's what we'll build next:

---

## 🔥 PHASE 1: Experience & Leveling System
**Priority: HIGH - Foundation for mid-run progression**

### Experience Orbs (15 min)
**What:**
- Basic enemies drop experience orbs when killed
- Player walks over orbs to collect
- Visual orb prefab with glow/particle effect

**Implementation:**
```
New Scripts:
  • ExperienceOrb.cs - Orb behavior, collection on trigger
  • ExperienceLoot.cs - Drop orbs on enemy death
  
Updated Scripts:
  • PlayerStats.cs - Add experience tracking, level calculation
  • EnemyHealth.cs - Drop orbs on death (optional)
  
Scene Setup:
  • Create ExperienceOrb prefab (sphere + particle + collider)
  • Configure drop chance/amount per enemy type
```

**Features:**
- Automatic collection on proximity
- Experience formula: XP needed = baseXP * (level ^ 1.5)
- Visual feedback on collection

---

### Level-Up System (20 min)
**What:**
- Track player level during run
- Show level-up UI when threshold reached
- Offer stat choices when leveling up

**Implementation:**
```
New Scripts:
  • LevelUpManager.cs - Track XP, level-ups, trigger UI
  • LevelUpUI.cs - Display stat choices
  
Updated Scripts:
  • PlayerStats.cs - Apply level-up bonuses
  
UI Setup:
  • Create LevelUpPanel in GameCanvas
  • 3 stat choice buttons (random selection)
  • Pause game on level-up (optional)
```

**Features:**
- 3 random stat options per level-up
- Temporary stats (run only) or permanent
- Resume gameplay after selection

---

## 🎁 PHASE 2: Skill Drafting System
**Priority: HIGH - Core replayability feature**

### Elite/Boss Monsters (25 min)
**What:**
- Special enemy type with more health/damage
- Visual distinction (size, color, effects)
- Drop skill chests on death

**Implementation:**
```
New Scripts:
  • EliteEnemy.cs - Enhanced enemy behavior
  • SkillChest.cs - Chest drop, opening animation
  
Updated Scripts:
  • WaveManager.cs - Spawn elites at intervals
  • EnemyHealth.cs - Flag elite type, drop chest
  
Prefabs:
  • EliteEnemy prefab (larger model, particles)
  • SkillChest prefab (animated chest)
```

**Features:**
- Elite spawns every 3-5 waves
- 3x health, 1.5x damage
- Guaranteed chest drop

---

### Skill Draft UI (30 min)
**What:**
- Player interacts with chest
- Shows 2-4 random skill options
- Select one skill to add to build

**Implementation:**
```
New Scripts:
  • SkillData.cs - ScriptableObject for skills
  • SkillDraftUI.cs - Display skill choices
  • SkillManager.cs - Track active skills, apply effects
  
UI Setup:
  • Create SkillDraftPanel in GameCanvas
  • Skill card layout (icon, name, description)
  • Selection buttons
  
Data:
  • Create 10-20 SkillData assets
```

**Skill Examples:**
- Faster Attack Speed (+25%)
- Area Damage (splash on hit)
- Health Regeneration (2 HP/sec)
- Multishot (fire 2 projectiles)
- Shield (absorb 1 hit)
- Double Jump ability
- Dash ability
- Lifesteal (10% damage → heal)

---

### Skill Synergies (15 min)
**What:**
- Bonus effects when specific skills combine
- Visual notification when synergy activates

**Implementation:**
```
Updated Scripts:
  • SkillManager.cs - Check synergies, apply bonuses
  
New System:
  • SynergyData.cs - Define skill combinations
  
UI:
  • Synergy notification popup
```

**Synergy Examples:**
- Multishot + Piercing = Extra damage
- Lifesteal + Attack Speed = Better sustain
- Dash + Area Damage = AoE on dash
- Shield + Health Regen = Faster shield recharge

---

## 🏗️ PHASE 3: Obstacle Placement System
**Priority: MEDIUM - Adds strategic depth**

### Obstacle Basics (30 min)
**What:**
- Spend in-run currency to place obstacles
- Grid-based placement system
- Different obstacle types with effects

**Implementation:**
```
New Scripts:
  • ObstacleData.cs - ScriptableObject for obstacles
  • ObstaclePlacement.cs - Grid placement, preview
  • Obstacle.cs - Base class for all obstacles
  • ObstacleUI.cs - Show available obstacles, costs
  
Systems:
  • Grid system for valid placement
  • Currency cost check
  • Placement preview (green/red indicator)
```

**Obstacle Types:**
1. **Wall** - Block enemy path (50 currency)
2. **Spike Trap** - Damage enemies (75 currency)
3. **Slow Field** - Reduce enemy speed (60 currency)
4. **Turret** - Auto-attack enemies (100 currency)

---

### Placement UI (20 min)
**What:**
- Bottom toolbar with obstacle icons
- Show cost and cooldown
- Click to enter placement mode

**Implementation:**
```
UI Setup:
  • ObstacleToolbar in GameCanvas
  • Obstacle icons with costs
  • Cooldown timers
  
Input:
  • Click obstacle → enter placement mode
  • Click ground → place obstacle
  • ESC → cancel placement
```

---

### Advanced Obstacle Features (Optional - 20 min)
**What:**
- Cooldowns between placements
- Obstacle decay/maintenance cost
- Upgrade existing obstacles

**Implementation:**
```
New Features:
  • Per-obstacle cooldown timers
  • Decay over time (optional)
  • Upgrade UI (click obstacle to upgrade)
```

---

## 👥 PHASE 4: Multiple Heroes
**Priority: MEDIUM - Increases variety**

### Hero Selection (25 min)
**What:**
- 3 distinct hero classes
- Each with unique starting stats and ability
- Select before run starts

**Implementation:**
```
New Scripts:
  • HeroData.cs - ScriptableObject for heroes
  • HeroSelectionUI.cs - Pre-run hero picker
  • HeroManager.cs - Apply hero stats/abilities
  
Data:
  • Create 3 HeroData assets
  
UI:
  • HeroSelectionPanel (pre-run)
  • Hero preview cards
```

**Hero Examples:**
1. **Warrior (Melee)**
   - High health, short range
   - Ability: Shield Bash (knockback)
   - Stat focus: Health, damage

2. **Ranger (Ranged)**
   - Medium health, long range
   - Ability: Piercing Shot
   - Stat focus: Attack speed, crit

3. **Mage (Area Damage)**
   - Low health, medium range
   - Ability: Fireball (AoE)
   - Stat focus: Damage, crit damage

---

### Hero Abilities (20 min)
**What:**
- Each hero has 1 signature ability
- Cooldown-based activation
- Unique visual effects

**Implementation:**
```
New Scripts:
  • HeroAbility.cs - Base class for abilities
  • ShieldBash.cs - Warrior ability
  • PiercingShot.cs - Ranger ability
  • Fireball.cs - Mage ability
  
Input:
  • Q key - Use hero ability
  
UI:
  • Ability icon with cooldown
```

---

## 💎 PHASE 5: Meta-Progression
**Priority: MEDIUM - Long-term retention**

### Meta-Currency System (20 min)
**What:**
- Separate currency from in-run gold
- Earned based on run performance
- Persistent between runs

**Implementation:**
```
New Scripts:
  • MetaCurrency.cs - Track meta-currency
  • EndRunRewards.cs - Calculate rewards
  
Updated Scripts:
  • GameProgressionManager.cs - Add meta tracking
  
Save System:
  • PlayerPrefs or JSON save file
```

**Earning Formula:**
```
Meta Currency = 
  (Waves Survived * 10) +
  (Enemies Killed * 2) +
  (Zones Defended * 50) +
  (Bonus for Win)
```

---

### Meta-Upgrade Shop (25 min)
**What:**
- Permanent upgrades using meta-currency
- Unlocks and stat boosts
- Accessed from main menu

**Implementation:**
```
New Scripts:
  • MetaUpgradeData.cs - ScriptableObject
  • MetaUpgradeShop.cs - Purchase system
  • MetaUpgradeUI.cs - Shop interface
  
UI:
  • MetaShopPanel in MainMenu
  • Upgrade cards with costs
```

**Meta-Upgrade Examples:**
- Starting Health +20 (Cost: 500)
- Starting Damage +5 (Cost: 400)
- Extra Starting Currency +50 (Cost: 300)
- Unlock Hero: Mage (Cost: 1000)
- Start with Random Skill (Cost: 800)

---

### Hero Unlock System (15 min)
**What:**
- Heroes locked by default
- Unlock with meta-currency
- Progression gates

**Implementation:**
```
Updated Scripts:
  • HeroSelectionUI.cs - Show locked heroes
  • MetaUpgradeShop.cs - Unlock purchases
  
Save System:
  • Track unlocked heroes
```

---

## 🏰 PHASE 6: Zone Health & Breach
**Priority: MEDIUM - Enhanced defense mechanics**

### Zone Health Tracking (20 min)
**What:**
- Each zone has health/integrity
- Enemies damage zones when attacking
- Zone breach = lose condition

**Implementation:**
```
Updated Scripts:
  • DefenseZone.cs - Add health, damage tracking
  
New Scripts:
  • ZoneHealthUI.cs - Display zone health bars
  
Visuals:
  • Health bar above each zone
  • Warning effects when low
```

---

### Enhanced Enemy AI (25 min)
**What:**
- Enemies target both player and zones
- Prioritize zones when player is far
- Attack zones to damage them

**Implementation:**
```
Updated Scripts:
  • EnemyAI.cs - Add zone targeting logic
  • DefenseZone.cs - TakeDamage method
  
New Features:
  • Zone selection based on distance
  • Attack animations for zones
```

---

## 🎨 PHASE 7: Visual Polish
**Priority: LOW - After core features**

### Damage Numbers (15 min)
**What:**
- Floating text showing damage
- Different colors for crits
- Animated popup

**Implementation:**
```
New Scripts:
  • DamageNumber.cs - Floating text behavior
  • DamageNumberPool.cs - Object pooling
  
UI:
  • WorldSpace canvas for damage text
  • Animation curves
```

---

### Weapon Effect Visuals (30 min)
**What:**
- Particle effects for each weapon type
- Visual feedback on hit
- Status effect indicators

**Implementation:**
```
New Prefabs:
  • BurnEffect (fire particles)
  • FreezeEffect (ice particles)
  • ChainLightning (lightning beam)
  • etc.
  
Updated Scripts:
  • WeaponSystem.cs - Spawn effects
  • PlayerCombat.cs - Trigger on hit
```

---

### UI Animations (20 min)
**What:**
- Fade in/out transitions
- Button hover effects
- Screen shake on damage

**Implementation:**
```
New Scripts:
  • UIAnimator.cs - Tween animations
  • ScreenShake.cs - Camera shake
  
Animations:
  • Panel fade in/out
  • Button scale on hover
  • Damage flash
```

---

## 🎵 PHASE 8: Audio
**Priority: LOW - Polish**

### Sound Effects (20 min)
**What:**
- Combat sounds (attack, hit, death)
- UI sounds (click, hover, purchase)
- Environmental sounds

**Implementation:**
```
New Scripts:
  • AudioManager.cs - Centralized sound system
  • SFXPlayer.cs - Play one-shot sounds
  
Audio:
  • Import/create sound effects
  • Configure AudioSource components
```

---

### Music System (15 min)
**What:**
- Background music
- Combat music
- Menu music
- Transition between tracks

**Implementation:**
```
Updated Scripts:
  • AudioManager.cs - Music crossfade
  
Audio:
  • Import music tracks
  • Configure looping
```

---

## 📊 PHASE 9: Statistics & End Screen
**Priority: MEDIUM - Player feedback**

### Statistics Tracking (20 min)
**What:**
- Track player performance
- Display end-of-run stats
- Show improvement over time

**Implementation:**
```
New Scripts:
  • StatisticsTracker.cs - Track stats during run
  • EndRunScreen.cs - Display results
  
Data Tracked:
  • Enemies killed
  • Damage dealt
  • Damage taken
  • Currency earned
  • Waves survived
  • Time played
```

---

### End Run UI (25 min)
**What:**
- Results screen after run ends
- Show stats and rewards
- Option to retry or return to menu

**Implementation:**
```
New UI:
  • EndRunPanel in GameCanvas
  • Stat display layout
  • Reward breakdown
  • Buttons (Retry, Menu)
```

---

## 📝 Summary: Recommended Implementation Order

### Week 1: Core Progression
1. ✅ Interaction UI (DONE)
2. Experience Orbs (15 min)
3. Level-Up System (20 min)
4. Elite Monsters (25 min)
5. Skill Drafting (30 min)

**Total: ~90 minutes**

### Week 2: Strategic Depth
6. Obstacle Placement (30 min)
7. Obstacle UI (20 min)
8. Skill Synergies (15 min)
9. Zone Health (20 min)

**Total: ~85 minutes**

### Week 3: Variety & Polish
10. Hero Selection (25 min)
11. Hero Abilities (20 min)
12. Meta-Currency (20 min)
13. Meta-Upgrades (25 min)
14. Enhanced Enemy AI (25 min)

**Total: ~115 minutes**

### Week 4: Polish & Feedback
15. Damage Numbers (15 min)
16. Weapon Visuals (30 min)
17. Statistics (20 min)
18. End Run Screen (25 min)
19. Sound Effects (20 min)
20. UI Animations (20 min)

**Total: ~130 minutes**

---

## 🎯 Quick Start: After UI Implementation

Once you've implemented the Interaction UI, I'm ready to help you with:

1. **Next Immediate Step:** Experience Orbs + Level-Up System
   - Quick to implement (~35 min)
   - Immediate gameplay impact
   - Foundation for skill system

2. **High Value:** Skill Drafting System
   - Core replayability feature
   - Most exciting for players
   - ~55 min total

3. **Strategic:** Obstacle Placement
   - Adds tactical depth
   - New gameplay dimension
   - ~50 min total

**Just let me know when you're ready to proceed!**
