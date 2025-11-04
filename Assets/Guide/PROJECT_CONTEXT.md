# Roguelike Prototype - Project Context

## Project Information
- **Project Name:** GameOff 2025 - Roguelike Prototype
- **Genre:** Isometric Roguelike Defense with Wave-based Combat and Progression
- **Unity Version:** 6000.2 (Unity 6)
- **Render Pipeline:** Universal Render Pipeline (URP) 17.2.0
- **Target Platform:** PC (expandable)

## Technical Stack

### Key Packages
- **Input System:** 1.14.2 (New Input System - NOT legacy)
- **Cinemachine:** 3.1.5 (Version 3.x with breaking changes from 2.x)
- **AI Navigation:** 2.0.9
- **URP:** 17.2.0
- **Timeline:** 1.8.9
- **TextMeshPro:** Included in UGUI 2.0.0

### Architecture Decisions
- **Input Handling:** Unity Input System with InputActions asset (`InputSystem_Actions.inputactions`)
- **Input Settings:** Project Settings > Player > Active Input Handling = **"Input System Package (New)"** (REQUIRED - set and confirmed)
- **Camera System:** Cinemachine 3.x for isometric follow camera (camera position controlled by Cinemachine, not manual)
  - CinemachineBrain must be on the MainCamera GameObject at scene root
  - Virtual cameras (CinemachineCamera) must be **separate from MainCamera** (not child) to avoid feedback loops
  - MainCamera must have tag "MainCamera" for Camera.main to work
  - For isometric view: Use CinemachinePositionComposer, disable or remove CinemachineRotationComposer
  - **IMPORTANT - Cinemachine 3.x Screen Position:** Uses (0, 0) for CENTER, range is -0.5 to 0.5 (NOT 0 to 1 like older versions!)
  - **Recommended Settings for Isometric:**
    - Transform Rotation: (45, 45, 0) for classic isometric angle
    - Camera Distance: 15-18
    - Screen Position: (0, 0) to center player
    - Dead Zone Enabled with Size: (0.1, 0.1) for smooth following
    - Damping: (2, 2, 2) for gentle lag
    - Target Offset Y: 1 to track slightly above player feet
- **Physics:** CharacterController for player and enemies (not Rigidbody)
  - **CRITICAL: CharacterController Center Setup** - Two valid approaches:
    - **Standard Approach (Recommended):** Center Y = Height / 2, Transform Y = 0
      - Example: Height 2.0 → Center Y = 1.0, spawn at Y = 0
      - Example: Height 1.6 → Center Y = 0.8, spawn at Y = 0
    - **Alternative Approach:** Center Y = 0, Transform Y = Height / 2
      - Example: Height 2.0 → Center Y = 0, spawn at Y = 1.0
      - Example: Height 1.6 → Center Y = 0, spawn at Y = 0.8
    - **Both place the capsule bottom at ground level (Y = 0)**
  - **CRITICAL: Gravity Implementation** - Must apply constant downward force:
    - When grounded: Apply small downward force (e.g., -2f) to keep character pressed against ground
    - When airborne: Apply full gravity acceleration
    - NEVER check `!isGrounded` and do nothing when grounded - this causes floating due to skinWidth gap
    - See PlayerController and EnemyAI for reference implementation
- **Combat:** Layer-based detection with overlap spheres
- **Progression System:** Currency-based upgrades and weapon system
  - Currency earned from enemy kills
  - Between-wave shop for upgrades (stats) and weapons
  - Defense zone system with 3 locations and fallback mechanic
  - Base safe zone with gates that open/close between waves
  - 30-45 second timer for upgrades between waves
- **Enemy AI:** Simple chase-and-attack behavior
- **Wave System:** Coroutine-based spawning with progressive difficulty

## Game Design Pillars

### Core Mechanics
1. **Movement:** XZ plane movement with camera-relative controls
2. **Jump:** Y-axis jumping with gravity
3. **Combat:** Melee attacks with cursor-based aiming for isometric view
4. **Progression:** Currency-based stat upgrades and weapon purchases
5. **Defense Zones:** 3 horizontal defensive positions with fallback system
6. **Safe Base:** Between-wave upgrade shop with timer and gates
7. **Waves:** Automatic enemy spawning with progressive difficulty and rewards

### Control Scheme
- **WASD / Left Stick:** Movement
- **Space / A Button:** Jump
- **Left Mouse Button:** Attack (aims toward cursor when stationary)
- **Hold Shift + Move:** Sprint

### Design Goals
- Fast-paced action combat with meaningful decisions
- Cursor-based aiming for precise attacks in isometric view
- Progressive difficulty through waves and enemy scaling
- Risk/reward fallback system (lose zone perks but survive)
- Meta-progression through persistent upgrades within run
- Strategic resource management (when to upgrade vs save)
- Quick prototype iteration with data-driven balance
- "Game feel" and systems over visual polish (for now)

## Code Architecture

### Folder Structure
```
/Assets
  /Scripts
    /Player       - Player-specific components
    /Enemy        - Enemy AI and behavior
    /Systems      - Game-wide systems (waves, UI, managers)
  /Prefabs        - Reusable GameObjects
  /Materials      - Material assets
  /Models         - 3D models
  /Scenes         - Scene files
```

### Core Systems

#### Player Systems
- **PlayerController.cs** - Movement, jump, sprint mechanics with stat-based speed
- **PlayerHealth.cs** - Health management with stat-based max health
- **PlayerCombat.cs** - Attack system with cursor-based aiming, layer detection, crit system, weapon effects
- **PlayerStats.cs** - Singleton managing all player stat upgrades

#### Enemy Systems
- **EnemyAI.cs** - Chase and attack AI with proper gravity
- **EnemyHealth.cs** - Health, death handling, and currency drops

#### Progression Systems
- **GameProgressionManager.cs** - Singleton managing currency, defense zones, base timer, game state
- **PlayerStats.cs** - Persistent stat upgrades (speed, health, damage, crit)
- **WeaponSystem.cs** - Singleton managing equipped weapon and special effects
- **WeaponData.cs** - ScriptableObject defining weapon stats and effects
- **UpgradeData.cs** - ScriptableObject defining upgrade costs and types
- **UpgradeShop.cs** - Traditional UI-based shop controller (optional - can use NPC system instead)
- **ShopNPC.cs** - NPC-based shop vendor with proximity interaction and type specialization
- **NPCInteractionPrompt.cs** - World-space UI prompt for NPC interactions

#### Defense & Base Systems
- **DefenseZone.cs** - Individual defense location with perks and fallback
- **BaseGate.cs** - Animated gate controller (opens/closes with wave state)
- **BaseTrigger.cs** - Trigger zone for entering safe base area

#### Game Systems
- **WaveSpawner.cs** - Wave-based enemy spawning with progressive difficulty
- **WaveController.cs** - Wave flow integration with defense zones and events
- **GameManager.cs** - Game state, pause, restart (Singleton)
- **GameUI.cs** - UI updates for health and waves
- **VisualFeedback.cs** - Damage flash effects
- **CameraFollow.cs** - Optional Cinemachine configuration helper

#### Helper Systems
- **CharacterGrounder.cs** - Auto-ground characters to place capsule bottom at Y=0
- **VisualModelAligner.cs** - Align visual model child to CharacterController capsule bottom
- **GroundingDebugger.cs** - Visualize CharacterController capsule in Scene view

### Design Patterns Used
- **Singleton:** GameManager, GameProgressionManager, PlayerStats, WeaponSystem for global access
- **ScriptableObject:** Data-driven weapon and upgrade definitions (designer-friendly)
- **Component-based:** Modular systems on GameObjects
- **Event-driven:** UnityEvents for health changes, death, currency, state transitions
- **Layer-based:** Physics layers for combat targeting
- **State Machine:** Defense zone progression with fallback system

## System Integration Reference

### Complete File List (24 files)

#### New System Scripts (14)
1. `/Assets/Scripts/Systems/GameProgressionManager.cs` - Currency, zones, timer
2. `/Assets/Scripts/Systems/PlayerStats.cs` - Stat upgrades (6 types)
3. `/Assets/Scripts/Systems/WeaponSystem.cs` - Weapon management
4. `/Assets/Scripts/Systems/WeaponData.cs` - Weapon ScriptableObject
5. `/Assets/Scripts/Systems/UpgradeData.cs` - Upgrade ScriptableObject
6. `/Assets/Scripts/Systems/UpgradeShop.cs` - Traditional UI shop (optional)
7. `/Assets/Scripts/Systems/ShopNPC.cs` - NPC vendor system (NEW)
8. `/Assets/Scripts/Systems/NPCInteractionPrompt.cs` - NPC prompt UI (NEW)
9. `/Assets/Scripts/Systems/DefenseZone.cs` - Zone system
10. `/Assets/Scripts/Systems/BaseGate.cs` - Gate animation
11. `/Assets/Scripts/Systems/BaseTrigger.cs` - Base entrance
12. `/Assets/Scripts/Systems/WaveController.cs` - Wave integration
13. `/Assets/Scripts/Systems/CharacterGrounder.cs` - Auto-grounding
14. `/Assets/Scripts/Systems/VisualModelAligner.cs` - Visual alignment

#### Modified Scripts (4)
1. `/Assets/Scripts/Player/PlayerController.cs` - Stat-based speed
2. `/Assets/Scripts/Player/PlayerHealth.cs` - Stat-based health
3. `/Assets/Scripts/Player/PlayerCombat.cs` - Stats, crits, weapons
4. `/Assets/Scripts/Enemy/EnemyHealth.cs` - Currency drops

#### Documentation (7)
1. `/Assets/Guide/README.md` - Documentation index
2. `/Assets/Guide/QUICK_START.md` - 10-minute setup
3. `/Assets/Guide/IMPLEMENTATION_SUMMARY.md` - Overview
4. `/Assets/Guide/UPGRADE_SYSTEM_GUIDE.md` - Full details
5. `/Assets/Guide/SYSTEM_ARCHITECTURE.md` - Visual diagrams
6. `/Assets/Guide/FINAL_CHECKLIST.md` - Verification
7. `/Assets/Guide/NPC_SHOP_SETUP.md` - NPC vendor guide (NEW)

### Singleton Managers (Must all be on one GameObject)
```
GameManagers (Empty GameObject)
├── GameProgressionManager
│   ├─→ Currency (int)
│   ├─→ Defense Zone Index (0-2)
│   ├─→ Base Timer (40s default)
│   └─→ Events: OnCurrencyChanged, OnEnteredBase, OnExitedBase, etc.
├── PlayerStats
│   ├─→ 6 stat types with base + levels
│   ├─→ Upgrade methods for each stat
│   └─→ Events: OnStatsChanged
├── WeaponSystem
│   ├─→ Equipped weapon (WeaponData)
│   ├─→ Multiplier getters
│   └─→ Effect application methods
└── UpgradeShop
    ├─→ Available upgrades (UpgradeData[])
    ├─→ Available weapons (WeaponData[])
    └─→ Purchase methods
```

### Player Stat Types
1. **Move Speed** - Base 5.0 + 0.5 per level
2. **Max Health** - Base 100 + 20 per level
3. **Damage** - Base 10 + 5 per level
4. **Crit Chance** - Base 5% + 5% per level
5. **Crit Damage** - Base 1.5x + 0.25x per level
6. **Attack Range** - Base 2.0 + 0.5 per level

### Weapon Effect Types
1. **None** - No special effect
2. **Burn** - Fire DOT (5 dmg/sec for 3 sec)
3. **Freeze** - Slow enemy movement
4. **Lightning** - Chain damage to nearby enemies
5. **Poison** - Poison DOT (3 dmg/sec for 5 sec)
6. **Lifesteal** - Heal player for % of damage
7. **AreaDamage** - Damage in radius around hit
8. **Knockback** - Push enemies away

### Upgrade Cost Scaling
```csharp
cost = baseCost * (costIncreasePerLevel ^ currentLevel)
Example: baseCost=50, increase=1.5
  Level 0: 50
  Level 1: 75
  Level 2: 112
  Level 3: 168
  ...exponential
```

### Defense Zone Flow
```
Zone 1 (Frontline)     Zone 2 (Middle)       Zone 3 (Base)
├─ Index: 0            ├─ Index: 1           ├─ Index: 2
├─ Perks: 0%           ├─ Perks: +25%        ├─ Perks: +50%
├─ Wave bonus: 3x      ├─ Wave bonus: 2x     ├─ Wave bonus: 1x
└─→ Fallback when      └─→ Fallback when     └─→ Last stand
    health < 25%           health < 25%           (game over if lost)
```

### Event System Map
```
GameProgressionManager:
├─ OnCurrencyChanged(int) → Update UI currency display
├─ OnEnteredBase() → Open gate, open shop, pause time
├─ OnExitedBase() → Close gate, close shop, resume time, start wave
├─ OnBaseTimerUpdate(float) → Update UI timer
└─ OnZoneChanged(int) → Update UI zone indicator

PlayerStats:
└─ OnStatsChanged() → Refresh stat displays

WeaponSystem:
└─ OnWeaponChanged(WeaponData) → Update weapon UI

UpgradeShop:
├─ OnUpgradePurchased(UpgradeData) → Play VFX/SFX
└─ OnWeaponPurchased(WeaponData) → Play VFX/SFX
```

### Key Integration Points
```csharp
// Player reads stats
float speed = PlayerStats.Instance.GetMoveSpeed();
float damage = PlayerStats.Instance.CalculateFinalDamage();

// Apply weapon multipliers
damage *= WeaponSystem.Instance.GetDamageMultiplier();

// Enemy drops currency
GameProgressionManager.Instance.AddCurrency(currencyReward);

// Shop purchases
if (GameProgressionManager.Instance.SpendCurrency(cost))
    PlayerStats.Instance.UpgradeMoveSpeed();

// Base flow
OnTriggerEnter → EnterBase() → OpenGate() + OpenShop()
OnTriggerExit → ExitBase() → CloseGate() + StartWave()
```

## Technical Constraints & Decisions

### Unity 6 Compatibility
- All code verified for Unity 6 (6000.2)
- No obsolete APIs used
- Cinemachine 3.x API (not 2.x)
- New Input System only (no legacy Input Manager)

### Performance Considerations
- GameObject pooling: NOT YET IMPLEMENTED (future optimization)
- CharacterController for physics (less overhead than Rigidbody)
- Layer masks for efficient collision detection
- Cached component references in Awake()

### Tags and Layers
- **Tag:** "Player" - Required for player GameObject
- **Layer:** "Enemy" - Required for enemy GameObjects and combat detection

## Current Development Phase

### ✅ Phase 1: Core Mechanics (COMPLETE)
- ✅ Player movement with camera-relative controls
- ✅ Jump and gravity system
- ✅ Sprint mechanic
- ✅ Cursor-based aiming for isometric combat
- ✅ Basic melee combat with layer detection
- ✅ Enemy AI with chase and attack
- ✅ Wave spawning system
- ✅ Basic health system
- ✅ Enemy grounding fixed (no floating)

### ✅ Phase 2: Progression Systems (COMPLETE)
- ✅ Currency system (earn from kills)
- ✅ Player stat upgrades (6 types: speed, health, damage, crit chance/damage, range)
- ✅ Weapon system with 8 effect types
- ✅ Defense zone system (3 zones with fallback)
- ✅ Base safe zone with timer (40s between waves)
- ✅ Gate animation system (opens/closes with game state)
- ✅ Critical hit system
- ✅ ScriptableObject data-driven balance
- ✅ Event-driven architecture for UI integration
- 🔄 Scene setup and testing (ready for implementation)

### 🔄 Phase 3: Content & Polish (CURRENT)
- [ ] Create ScriptableObject data assets (upgrades and weapons)
- [ ] Set up GameManagers GameObject with all 4 singletons
- [ ] Configure defense zones in scene
- [ ] Build base area with gate and trigger
- [ ] Create shop UI (canvas, buttons, currency display)
- [ ] Wire UI to UpgradeShop events
- [ ] Visual polish (particles, screen shake, hit effects)
- [ ] Animation system integration
- [ ] Audio implementation (SFX and music)

### 📋 Phase 4: Future Features (PLANNED)
- More enemy types and variations
- Weapon visual effects (fire, ice, lightning, etc.)
- Status effect visuals (burn DOT, slow, stun)
- Multiple levels/arenas
- Boss encounters
- Meta-progression system (persistent between runs)
- Save system
- Settings and options menu
- Performance optimization (object pooling)

## Known Issues & TODOs
- [x] Create Enemy layer in project settings ✓
- [x] Enemy floating issue (CharacterController gravity) ✓
- [x] Visual model alignment to capsule bottom ✓
- [x] Currency system implementation ✓
- [x] Player stat upgrade system ✓
- [x] Weapon system with effects ✓
- [x] Defense zone progression ✓
- [x] Base safe zone with timer ✓
- [ ] Create GameManagers GameObject with 4 manager components
- [ ] Set enemy prefab currency reward > 0
- [ ] Create upgrade/weapon ScriptableObject assets
- [ ] Set up 3 defense zones in scene
- [ ] Build base area (ground, gate, trigger)
- [ ] Create shop UI canvas
- [ ] Wire UI to events
- [ ] Add visual feedback for attacks (particles, trails)
- [ ] Implement animation state machine
- [ ] Add sound effects

## Development Guidelines

### Code Style
- Self-explanatory variable and method names
- Public method comments
- Constants instead of magic numbers
- Proper namespace usage

### Asset Organization
- All scripts in `/Assets/Scripts`
- All materials in `/Assets/Materials`
- All prefabs in `/Assets/Prefabs`
- All 3D models in `/Assets/Models`
- All scenes in `/Assets/Scenes`
- All guides, documentation, and context files in `/Assets/Guide`

### Testing Approach
- Prototype for feel first
- Iterate on movement and combat
- Balance difficulty curve through playtesting
- Performance profiling before major systems

## Reference Documentation
All documentation stored in `/Assets/Guide/`:
- `PROJECT_CONTEXT.md` - Project specifications and context (THIS FILE)
- `README.md` - Documentation index and quick reference
- `QUICK_START.md` - 10-minute setup guide for progression systems
- `IMPLEMENTATION_SUMMARY.md` - Complete overview of all systems
- `UPGRADE_SYSTEM_GUIDE.md` - Full implementation details
- `SYSTEM_ARCHITECTURE.md` - Visual diagrams and flows
- `FINAL_CHECKLIST.md` - Verification and testing matrix

## Version History
- **v0.3** - Progression systems implementation (Current)
  - Currency and upgrade system
  - Weapon system with 8 effects
  - Defense zone system (3 zones + fallback)
  - Base safe zone with timer
  - Gate animation system
  - Critical hit mechanics
  - ScriptableObject data architecture
  - Complete documentation suite
- **v0.2** - Enemy grounding fixes
  - CharacterController gravity improvements
  - Visual model alignment system
  - Debug visualization tools
- **v0.1** - Initial prototype foundation
  - Core movement mechanics
  - Basic combat system
  - Wave spawning
  - Enemy AI

---

## 🤖 Auto-Update Instructions (For Bezi AI)

**This file should be automatically updated when:**
- New packages are added or versions change
- Major architecture decisions are made
- New core systems are implemented
- Development phase changes (e.g., Phase 1 → Phase 2)
- New design patterns are adopted
- Technical constraints or requirements change
- New layers, tags, or project settings are added
- Important TODOs are completed or new ones identified

**Update the relevant sections without being asked.** Keep this file as the single source of truth for project context.

**File Organization:**
- All new guide/documentation files should be created in `/Assets/Guide/`
- This includes setup guides, reference docs, context files, etc.

---

**Last Updated:** Initial creation
**Maintained By:** Development team + Bezi AI
**Status:** Active Development - Prototype Phase
