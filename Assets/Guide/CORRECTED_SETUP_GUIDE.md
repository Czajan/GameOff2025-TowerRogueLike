# ✅ CORRECTED Complete Setup Guide - From Scratch
## Isometric Roguelike Defense with NPC Shop System

**Estimated Time:** 60-90 minutes  
**Difficulty:** Intermediate  
**Unity Version:** 6000.2  

> ⚠️ **IMPORTANT:** This is the corrected version of COMPLETE_SETUP_GUIDE.md with ALL script field mismatches fixed!

---

## Quick Reference: Known Issues Fixed

This guide corrects the following errors from the original:

1. ✅ **EnemyAI** - Removed non-existent "Player" reference (auto-finds by tag)
2. ✅ **EnemyHealth** - Removed non-existent "Events" section  
3. ✅ **DefenseZone** - Fixed 3 non-existent fields (zoneName, isActive checkbox, perkMultiplier)
4. ✅ **DefenseZone** - Removed spawn points section (no spawn points array exists!)
5. ✅ **BaseGate** - Fixed position fields (openHeight instead of open/close position Y)
6. ✅ **PlayerStats** - Removed non-existent "Player GameObject" reference
7. ✅ **VisualModelAligner** - Corrected field name (Visual Model not Model Transform)

---

## Table of Contents

1. [Project Setup](#1-project-setup)
2. [Input System](#2-input-system-setup)
3. [Player Setup](#3-player-setup)
4. [Camera Setup](#4-camera-setup)
5. [Enemy Setup](#5-enemy-setup)
6. [Defense Zones](#6-defense-zones)
7. [Base & NPCs](#7-base--shop-system)
8. [Wave System](#8-wave-spawning-system)
9. [UI System](#9-ui-system)
10. [Game Managers](#10-game-managers)
11. [Testing](#11-testing)

---

# 1. Project Setup

## 1.1 Configure Input System

1. **Open Project Settings:**
   - Edit → Project Settings → Player
   
2. **Set Active Input Handling:**
   ```
   Active Input Handling: Input System Package (New)
   ```

3. **Restart Unity** when prompted

## 1.2 Create Layers

Go to **Edit → Project Settings → Tags and Layers**

Add these layers:
```
Layer 6: Enemy
Layer 7: Ground
```

## 1.3 Create Tags

In **Tags and Layers**, add:
```
- Player
- Enemy
```

---

# 2. Input System Setup

## 2.1 Create Input Actions Asset

1. **Create the asset:**
   - Right-click in `/Assets` → Create → Input Actions
   - Name it `InputSystem_Actions`

2. **Configure Action Maps:**

**Player Action Map:**
```
Actions:
├─ Move (Value, Vector2)
│  └─ Binding: WASD / Left Stick
├─ Jump (Button)
│  └─ Binding: Space / A Button  
├─ Attack (Button)
│  └─ Binding: Left Mouse / X Button
└─ Sprint (Button, Hold)
   └─ Binding: Left Shift / Left Stick Press
```

3. **Save and Generate C# Class:**
   - Click "Generate C# Class"
   - Click "Apply"

---

# 3. Player Setup

## 3.1 Create Player GameObject

1. **Create empty GameObject:**
   - Hierarchy → Right-click → Create Empty
   - Name: `Player`
   - Tag: `Player`
   - Position: `(0, 0, 0)`

2. **Add visual placeholder:**
   - Right-click Player → 3D Object → Capsule
   - Name it `Model`
   - Position: `(0, 1, 0)` ← Adjust based on CharacterController center
   - Scale: `(1, 1, 1)`

## 3.2 Add CharacterController

1. **Add Component:**
   - Select Player
   - Add Component → Character Controller

2. **Configure:**
   ```
   Character Controller:
   ├─ Center: (0, 1, 0) ← Half of height
   ├─ Radius: 0.5
   ├─ Height: 2
   ├─ Skin Width: 0.08
   └─ Min Move Distance: 0.001
   ```

## 3.3 Add Player Scripts

1. **Add Components to Player:**
   - Add Component → Player Controller
   - Add Component → Player Health
   - Add Component → Player Combat
   - Add Component → Visual Feedback
   - Add Component → Visual Model Aligner
   - Add Component → Player Input

2. **Configure PlayerController:**
   ```
   Movement Settings:
   ├─ Move Speed: 5
   ├─ Sprint Multiplier: 1.5
   └─ Rotation Speed: 10
   
   Jump Settings:
   ├─ Jump Height: 2
   └─ Gravity: -15
   
   Camera:
   └─ Camera Transform: Leave empty ← Auto-finds Camera.main
   ```

3. **Configure PlayerHealth:**
   ```
   Health Settings:
   └─ Max Health: 100
   
   Events:
   ├─ On Health Changed (float): (empty - can add UI later)
   └─ On Death (): (empty - GameManager will listen)
   ```

4. **Configure PlayerCombat:**
   ```
   Attack Settings:
   ├─ Attack Damage: 25
   ├─ Attack Range: 2
   ├─ Attack Cooldown: 0.5
   └─ Enemy Layer: Enemy
   
   References:
   └─ Attack Point: Drag child GameObject "AttackPoint" ← CREATE THIS!
   
   Aiming:
   ├─ Main Camera: Leave empty ← Auto-finds Camera.main
   ├─ Rotation Speed: 15
   └─ Aim With Cursor: ☑ (checked)
   ```

   **Create AttackPoint:**
   - Right-click Player → Create Empty
   - Name: `AttackPoint`
   - Position: `(0, 1, 0.5)` ← In front of player
   - Drag to PlayerCombat's Attack Point field

5. **Configure VisualFeedback:**
   ```
   Damage Flash:
   ├─ Target Renderer: Drag Model's MeshRenderer
   ├─ Damage Color: Red
   └─ Flash Duration: 0.1
   ```

6. **Configure VisualModelAligner:**
   ```
   Model Alignment:
   ├─ Visual Model: Drag Model child here ← Optional, auto-finds child named "Model"
   ├─ Align On Awake: ☑ (checked)
   └─ Visual Model Height: 1
   ```
   
   **Note:** VisualModelAligner auto-finds a child named "Model" if not assigned.

7. **Configure PlayerInput:**
   ```
   Actions:
   └─ Actions: Drag InputSystem_Actions
   
   Default Map: Player
   
   Behavior:
   └─ Behavior: Invoke Unity Events
   
   Events:
   ├─ Player → Move → OnMove(PlayerController)
   ├─ Player → Jump → OnJump(PlayerController)
   ├─ Player → Attack → OnAttack(PlayerCombat)
   └─ Player → Sprint → OnSprint(PlayerController)
   ```

---

# 4. Camera Setup

## 4.1 Setup Main Camera

1. **Select Main Camera:**
   - Ensure it's at **scene root** (not child of anything!)
   - Tag: `MainCamera`
   - Position: `(0, 10, -10)` (temporary)

2. **Add Cinemachine Brain:**
   - Add Component → Cinemachine Brain

3. **Configure Brain:**
   ```
   Update Method: Late Update
   Blend Update Method: Late Update
   Default Blend: EaseInOut, 1 second
   ```

## 4.2 Create Virtual Camera

1. **Create Virtual Camera:**
   - Hierarchy → Right-click → Cinemachine → Cinemachine Camera
   - Name: `VCam_Follow`
   - Position: Doesn't matter (Cinemachine controls this)
   - **IMPORTANT:** Must be at scene root, NOT child of Player or MainCamera!

2. **Configure VCam_Follow:**
   ```
   Tracking Target:
   └─ Tracking Target: Drag Player GameObject
   
   Position Control - Follow:
   ├─ Damping: (2, 2, 2)
   ├─ Camera Distance: 15
   └─ Target Offset: (0, 1, 0) ← Track slightly above player feet
   
   Rotation Control - Position Composer:
   ├─ Screen Position: (0, 0) ← CENTER in Cinemachine 3.x!
   ├─ Dead Zone Size: (0.1, 0.1)
   └─ Damping: (2, 2)
   
   CinemachineCamera Transform:
   └─ Rotation: (45, 45, 0) ← Isometric angle
   ```

   **⚠️ IMPORTANT:** Cinemachine 3.x uses (0, 0) for CENTER, not (0.5, 0.5)!

---

# 5. Enemy Setup

## 5.1 Create Enemy GameObject

1. **Create GameObject:**
   - Hierarchy → Create Empty
   - Name: `Enemy`
   - Tag: `Enemy`
   - Layer: `Enemy`
   - Position: `(5, 0, 5)`

2. **Add visual placeholder:**
   - Right-click Enemy → 3D Object → Capsule
   - Name: `Model`
   - Position: `(0, 1, 0)`
   - Material: Red (create new material)

3. **Add CharacterController:**
   - Add Component → Character Controller
   ```
   Center: (0, 1, 0)
   Radius: 0.5
   Height: 2
   ```

## 5.2 Configure Enemy Components

1. **Add Components:**
   - Add Component → Enemy AI
   - Add Component → Enemy Health
   - Add Component → Visual Feedback
   - Add Component → Visual Model Aligner

2. **Configure EnemyAI:**
   ```
   Movement:
   ├─ Move Speed: 3
   └─ Rotation Speed: 5
   
   Combat:
   ├─ Attack Range: 1.5
   ├─ Attack Damage: 10
   └─ Attack Cooldown: 1
   
   Detection:
   └─ Chase Range: 20
   ```
   
   **Note: Player is auto-found by tag - no manual reference needed!**

3. **Configure EnemyHealth:**
   ```
   Health Settings:
   ├─ Max Health: 50
   └─ Currency Reward: 10
   ```
   
   **Note: No Events section - EnemyHealth auto-connects to GameProgressionManager!**

4. **Configure VisualFeedback:**
   ```
   Damage Flash:
   ├─ Target Renderer: Drag Model's MeshRenderer
   ├─ Damage Color: Red
   └─ Flash Duration: 0.1
   ```

5. **Configure VisualModelAligner:**
   ```
   Model Alignment:
   └─ Visual Model: Drag Model child ← Optional, auto-finds "Model"
   ```

## 5.3 Create Prefab

1. **Create Prefabs folder:**
   - In Project window: `/Assets/Prefabs`

2. **Make prefab:**
   - Drag Enemy from Hierarchy to `/Assets/Prefabs`
   - Delete Enemy from scene

---

# 6. Defense Zones

## 6.1 Create Zone Structure

1. **Create parent:**
   - Hierarchy → Create Empty
   - Name: `DefenseZones`
   - Position: `(0, 0, 0)`

2. **Create zones:**
   ```
   DefenseZones
   ├── DefenseZone_1 (Position: 30, 0, 0)
   ├── DefenseZone_2 (Position: 15, 0, 0)
   └── DefenseZone_3 (Position: 0, 0, 0)
   ```

3. **Add visuals to each zone (optional):**
   - Right-click zone → 3D Object → Plane
   - Scale: `(3, 1, 3)`
   - Different colors for each zone

## 6.2 Configure Defense Zones

Add DefenseZone component to each zone:

**DefenseZone_1:**
```
Zone Settings:
├─ Zone Index: 0
├─ Spawn Center: Drag DefenseZone_1 itself
└─ Spawn Radius: 10

Zone Perks:
├─ Damage Bonus: 0.0 ← No bonus for frontline
├─ Attack Speed Bonus: 0.0
└─ Move Speed Bonus: 0.0

Fallback:
├─ Next Zone: Drag DefenseZone_2
└─ Fallback Health Threshold: 0.25
```

**Note:** Zone auto-activates when Zone Index = 0!

**DefenseZone_2:**
```
Zone Settings:
├─ Zone Index: 1
├─ Spawn Center: Drag DefenseZone_2 itself
└─ Spawn Radius: 10

Zone Perks:
├─ Damage Bonus: 0.1 ← 10% damage boost!
├─ Attack Speed Bonus: 0.05 ← 5% attack speed boost
└─ Move Speed Bonus: 0.0

Fallback:
├─ Next Zone: Drag DefenseZone_3
└─ Fallback Health Threshold: 0.25
```

**DefenseZone_3:**
```
Zone Settings:
├─ Zone Index: 2
├─ Spawn Center: Drag DefenseZone_3 itself
└─ Spawn Radius: 10

Zone Perks:
├─ Damage Bonus: 0.2 ← 20% damage boost!
├─ Attack Speed Bonus: 0.1 ← 10% attack speed boost
└─ Move Speed Bonus: 0.1 ← 10% movement boost

Fallback:
├─ Next Zone: Leave empty (None) ← Last zone!
└─ Fallback Health Threshold: 0.25
```

**🚨 CRITICAL: DefenseZone fields explained**

The script has these fields (lines 6-18):
- `zoneIndex` - Which zone number (0, 1, 2)
- `spawnCenter` - Single Transform for spawn center
- `spawnRadius` - Radius around center for random spawns
- `damageBonus`, `attackSpeedBonus`, `moveSpeedBonus` - Individual perk bonuses
- `nextZone` - Reference to next fallback zone
- `fallbackHealthThreshold` - HP % when fallback triggers

The script does NOT have:
- ❌ `zoneName` string
- ❌ `isActive` as a serialized field (it's private!)
- ❌ `perkMultiplier` (use individual bonuses instead)
- ❌ Spawn points array (enemies spawn randomly!)

---

# 7. Base & Shop System

## 7.1 Create Base Structure

1. **Create Base parent:**
   - Hierarchy → Create Empty
   - Name: `Base`
   - Position: `(0, 0, -10)`

2. **Create Base visual (optional):**
   - Right-click Base → 3D Object → Cube
   - Name: `BaseVisual`
   - Scale: `(10, 1, 10)`
   - Material: Green

3. **Create BaseGate:**
   ```
   Base
   └── BaseGate
       └── GateVisual (Cube, scale 10x4x1, red material)
   ```

   **Position:**
   - BaseGate: `(0, 0, 5)` ← Front of base
   - GateVisual: `(0, 2, 0)` ← 2 units up (closed position)

4. **Configure BaseGate:**
   - Select BaseGate
   - Add Component → BaseGate
   - Add Component → Box Collider (for blocking)
   
   ```
   Gate Settings:
   ├─ Gate Visual: Drag GateVisual
   ├─ Gate Collider: Drag BaseGate's BoxCollider
   └─ Starts Open: ☑ (checked)
   
   Animation:
   ├─ Open Height: 5 ← How high gate rises (Y offset)
   └─ Animation Speed: 2
   ```

   **Configure BoxCollider:**
   ```
   Box Collider:
   ├─ Is Trigger: ☐ (unchecked) ← Blocks player when closed!
   ├─ Center: (0, 2, 0)
   └─ Size: (10, 4, 1)
   ```

   **🚨 FIELD CORRECTION:**
   
   BaseGate does NOT have "Open Position Y" or "Close Position Y"!
   
   Instead, it has:
   - `openHeight` (float) - How many units the gate moves up
   - The script calculates positions automatically:
     - Closed = current position
     - Open = current position + (0, openHeight, 0)

5. **Create BaseTrigger:**
   - Right-click Base → Create Empty
   - Name: `BaseTrigger`
   - Position: `(0, 0, 3)` ← Just inside gate
   - Add Component → BaseTrigger
   - Add Component → Box Collider
   
   ```
   Box Collider:
   ├─ Is Trigger: ☑ (checked)
   ├─ Center: (0, 1, 0)
   └─ Size: (10, 2, 2)
   
   Trigger Settings:
   └─ Is Entrance: ☑ (checked)
   ```

## 7.2 Create NPC Vendors

### Create NPC Parent
```
Base
└── NPCs (Empty GameObject at 0, 0, -10)
```

### NPC 1: Weapon Vendor (Blacksmith)

1. **Create NPC:**
   ```
   NPCs
   └── WeaponVendor
       └── Model (Cube, blue material)
   ```
   
   Position: `(-3, 0, -12)`

2. **Configure ShopNPC:**
   - Add Component → ShopNPC
   
   ```
   NPC Configuration:
   ├─ NPC Type: Weapon Vendor
   ├─ NPC Name: "Blacksmith"
   └─ Interaction Range: 3
   
   Weapon Vendor:
   └─ Available Weapons: Size 0 ← Add WeaponData assets later
   
   UI References:
   ├─ Interaction Prompt: (leave empty - create UI later)
   └─ Shop UI: (leave empty - create UI later)
   
   Visual Feedback:
   ├─ Interaction Indicator: (optional)
   └─ Highlight Color: Yellow
   ```

### NPC 2: Stat Upgrade Vendor (Trainer)

1. **Create NPC:**
   ```
   NPCs
   └── StatVendor
       └── Model (Cube, green material)
   ```
   
   Position: `(3, 0, -12)`

2. **Configure ShopNPC:**
   ```
   NPC Configuration:
   ├─ NPC Type: Stat Upgrade Vendor
   ├─ NPC Name: "Trainer"
   └─ Interaction Range: 3
   
   Stat Upgrade Vendor:
   └─ Available Upgrades: Size 0 ← Add UpgradeData assets later
   
   UI References:
   ├─ Interaction Prompt: (leave empty)
   └─ Shop UI: (leave empty)
   ```

---

# 8. Wave Spawning System

## 8.1 Create WaveSpawner GameObject

1. **Create GameObject:**
   - Hierarchy → Create Empty
   - Name: `WaveSpawner`
   - Position: `(0, 0, 0)`

2. **Add Component:**
   - Add Component → Wave Spawner

## 8.2 Configure WaveSpawner

```
Wave Settings:
├─ Enemy Prefab: Drag Enemy prefab from /Assets/Prefabs
├─ Initial Enemies Per Wave: 3
├─ Enemies Increase Per Wave: 2
└─ Time Between Waves: 5

Spawn Settings:
├─ Spawn Radius: 15
├─ Player Transform: Leave empty ← Auto-finds by tag!
└─ Min Spawn Distance: 8

Debug:
└─ Auto Start Waves: ☑ (checked)
```

**Note:** WaveSpawner auto-finds player by "Player" tag - no manual assignment needed!

## 8.3 Configure WaveController

1. **Create WaveController:**
   - Hierarchy → Create Empty
   - Name: `WaveController`
   - Add Component → Wave Controller

2. **Configure:**
   ```
   References:
   ├─ Wave Spawner: Leave empty ← Auto-finds WaveSpawner!
   └─ Defense Zones: Leave empty ← Auto-finds DefenseZone[]!
   
   Wave Flow:
   └─ Wait For Base Exit: ☑ (checked)
   ```

**Note:** WaveController auto-finds references if not manually assigned!

---

# 9. UI System

## 9.1 Create Canvas

1. **Create UI:**
   - Hierarchy → Right-click → UI → Canvas
   - Name: `GameCanvas`

2. **Configure Canvas:**
   ```
   Render Mode: Screen Space - Overlay
   Canvas Scaler:
   ├─ UI Scale Mode: Scale With Screen Size
   ├─ Reference Resolution: 1920 x 1080
   └─ Match: 0.5 (balanced)
   ```

## 9.2 Create HUD Elements

Create these as children of GameCanvas:

1. **Health Bar:**
   - UI → Slider
   - Name: `HealthBar`
   - Anchor: Top-left
   - Position: `(200, -50, 0)`
   - Width: 300

2. **Currency Display:**
   - UI → Text - TextMeshPro
   - Name: `CurrencyText`
   - Anchor: Top-right
   - Position: `(-200, -50, 0)`
   - Text: "Currency: 0"

3. **Wave Display:**
   - UI → Text - TextMeshPro
   - Name: `WaveText`
   - Anchor: Top-center
   - Text: "Wave: 1"

4. **Timer Display:**
   - UI → Text - TextMeshPro
   - Name: `TimerText`
   - Anchor: Top-center
   - Position: `(0, -100, 0)`
   - Text: "Time: 40s"

---

# 10. Game Managers

## 10.1 Create GameManagers GameObject

1. **Create parent:**
   - Hierarchy → Create Empty
   - Name: `GameManagers`
   - Position: `(0, 0, 0)`

2. **Add manager components:**
   - Add Component → Game Progression Manager
   - Add Component → Player Stats
   - Add Component → Weapon System
   - Add Component → Game Manager

## 10.2 Configure GameProgressionManager

```
Currency:
└─ Current Currency: 0 ← Starting currency

Defense Zones:
├─ Current Defense Zone: 0
└─ Max Defense Zones: 3

Game State:
├─ Is In Base: ☑ (checked)
├─ Base Timer Duration: 40
└─ Current Base Timer: 0

Events:
└─ (All UnityEvents - configure in UI scripts later)
```

## 10.3 Configure PlayerStats

```
Base Stats:
├─ Base Move Speed: 5
├─ Base Max Health: 100
├─ Base Damage: 10
├─ Base Crit Chance: 0.05
├─ Base Crit Damage: 1.5
└─ Base Attack Range: 2

Upgrade Levels:
└─ All: 0 ← Starting levels

Upgrade Values per Level:
├─ Move Speed Per Level: 0.5
├─ Health Per Level: 20
├─ Damage Per Level: 5
├─ Crit Chance Per Level: 0.05
├─ Crit Damage Per Level: 0.25
└─ Attack Range Per Level: 0.5
```

**Note: PlayerStats has NO Player reference field - it auto-finds player components!**

## 10.4 Configure WeaponSystem

```
Current Weapon:
└─ Equipped Weapon: (leave empty - equipped via shop)
```

## 10.5 Configure GameManager

```
Game State:
└─ Is Paused: ☐ (unchecked)

References:
├─ Player Health: Drag Player's PlayerHealth component ← REQUIRED!
└─ Wave Spawner: Drag WaveSpawner ← REQUIRED!
```

**Note: GameManager requires manual assignment of both references!**

---

# 11. Testing

## 11.1 Basic Movement Test

1. **Enter Play Mode**
2. **Test controls:**
   - WASD to move
   - Space to jump
   - Left Shift to sprint
   - Camera should follow at isometric angle

## 11.2 Combat Test

1. **Place enemy in scene** (drag prefab temporarily)
2. **Test attack:**
   - Left mouse button to attack
   - Should damage enemy
   - Enemy should flash red
   - Enemy should chase player

## 11.3 Wave System Test

1. **Start game in Base area**
2. **Observe:**
   - Base timer counting down
   - Gate should be open
3. **Exit base trigger:**
   - Gate should close
   - Wave should start
   - Enemies should spawn
4. **Kill all enemies:**
   - Get currency reward
   - Re-enter base
   - Timer starts again

## 11.4 Progression Test

1. **Collect currency from enemies**
2. **Enter base**
3. **Approach NPC:**
   - Press E to interact
   - Shop UI should open (if configured)
4. **Test fallback:**
   - Reduce player health below 25%
   - Should teleport to next zone

---

# 12. Common Issues & Troubleshooting

## Player Issues

**Problem:** Player not moving  
**Solution:** Check InputSystem_Actions is assigned in PlayerInput

**Problem:** Player falling through ground  
**Solution:** Add a plane with Ground layer, ensure CharacterController is grounded

**Problem:** Camera not following  
**Solution:** Ensure VCam_Follow has Player as Tracking Target, and MainCamera has CinemachineBrain

## Enemy Issues

**Problem:** Enemies not spawning  
**Solution:** Check Enemy Prefab is assigned in WaveSpawner

**Problem:** Enemies falling through ground  
**Solution:** Same as player - ensure ground exists

**Problem:** Enemies not chasing  
**Solution:** Player must have "Player" tag, EnemyAI auto-finds by tag

## System Issues

**Problem:** Gate not opening/closing  
**Solution:** Ensure GameProgressionManager exists and BaseTrigger is configured

**Problem:** Currency not updating  
**Solution:** Ensure GameProgressionManager exists in scene

**Problem:** Zones not activating  
**Solution:** Check zoneIndex values (0, 1, 2) and ensure Zone 0 auto-activates

---

# Summary of Key Auto-Find Systems

These components auto-find references - **leave these fields empty**:

1. **EnemyAI** → Player (finds by tag)
2. **WaveSpawner** → Player Transform (finds by tag)
3. **WaveController** → WaveSpawner, DefenseZones (finds in scene)
4. **PlayerController** → Camera Transform (finds Camera.main)
5. **PlayerCombat** → Main Camera (finds Camera.main)
6. **PlayerStats** → Player components (finds by type)
7. **ShopNPC** → Player (finds by tag)
8. **VisualModelAligner** → Visual Model (finds child named "Model")
9. **BaseGate** → Gate Visual (uses self if not assigned)

**Manual Assignment Required:**
- GameManager → PlayerHealth, WaveSpawner
- PlayerCombat → AttackPoint, Enemy Layer
- BaseGate → Gate Collider
- ShopNPC → UI references

---

**Status:** ✅ All fields verified against actual Unity 6 scripts  
**Last Updated:** 2025  
**Version:** 1.0 - Corrected
