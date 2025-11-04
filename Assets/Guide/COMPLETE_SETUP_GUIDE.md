# Complete Game Setup Guide - From Scratch
## Isometric Roguelike Defense with NPC Shop System

**Estimated Time:** 60-90 minutes  
**Difficulty:** Intermediate  
**Prerequisites:** GameManagers object created

---

## Table of Contents
1. [Project Settings](#1-project-settings)
2. [Input System Setup](#2-input-system-setup)
3. [Create Player](#3-create-player)
4. [Create Camera](#4-create-camera)
5. [Create Enemy Prefab](#5-create-enemy-prefab)
6. [Create Defense Zones](#6-create-defense-zones)
7. [Create Base Area with NPCs](#7-create-base-area-with-npcs)
8. [Create Wave Spawner](#8-create-wave-spawner)
9. [Create ScriptableObject Data](#9-create-scriptableobject-data)
10. [Configure GameManagers](#10-configure-gamemanagers)
11. [Create UI](#11-create-ui-optional)
12. [Test Everything](#12-test-everything)

---

# 1. Project Settings

## 1.1 Configure Input System

1. **Open Project Settings:**
   - Edit → Project Settings → Player

2. **Set Active Input Handling:**
   - Scroll to "Other Settings"
   - Find "Active Input Handling"
   - Set to: **"Input System Package (New)"**
   - Unity will ask to restart - click "Yes"

3. **Verify after restart:**
   - Settings should show "Input System Package (New)"

## 1.2 Create Layers

1. **Open Tags & Layers:**
   - Edit → Project Settings → Tags and Layers

2. **Add layers:**
   - Layer 6: `Enemy`
   - Layer 7: `Ground` (optional)

3. **Keep existing layers:**
   - Layer 0: Default
   - Layer 5: UI

## 1.3 Create Tags

1. **In Tags & Layers:**
   - Click "+" under Tags
   - Add tag: `Player`
   - Add tag: `Enemy`

---

# 2. Input System Setup

## 2.1 Create Input Actions Asset

1. **Create asset:**
   - Right-click in Project → Create → Input Actions
   - Name: `InputSystem_Actions`
   - Location: `/Assets/`

2. **Open Input Actions:**
   - Double-click `InputSystem_Actions`

3. **Create Action Map: "Player"**
   - Click "+" next to "Action Maps"
   - Name: `Player`

4. **Create Actions:**

**Action 1: Move**
   - Click "+" under Actions
   - Name: `Move`
   - Action Type: `Value`
   - Control Type: `Vector2`
   - Add Binding → 2D Vector Composite → WASD
     - Up: W
     - Down: S
     - Left: A
     - Right: D
   - Add Binding → Gamepad → Left Stick

**Action 2: Jump**
   - Name: `Jump`
   - Action Type: `Button`
   - Add Binding → Keyboard → Space
   - Add Binding → Gamepad → South Button

**Action 3: Attack**
   - Name: `Attack`
   - Action Type: `Button`
   - Add Binding → Mouse → Left Button
   - Add Binding → Gamepad → West Button

**Action 4: Sprint**
   - Name: `Sprint`
   - Action Type: `Button`
   - Add Binding → Keyboard → Left Shift
   - Add Binding → Gamepad → Left Stick Button

**Action 5: Interact**
   - Name: `Interact`
   - Action Type: `Button`
   - Add Binding → Keyboard → E
   - Add Binding → Gamepad → North Button

5. **Save:**
   - Click "Save Asset" at top
   - Close window

6. **Generate C# Class:**
   - Select `InputSystem_Actions` in Project
   - Inspector → Check "Generate C# Class"
   - Click "Apply"

---

# 3. Create Player

## 3.1 Create Player GameObject

1. **Create empty GameObject:**
   - Hierarchy → Right-click → Create Empty
   - Name: `Player`
   - Position: (0, 0, 0)
   - Tag: `Player`

2. **Add visual:**
   - Right-click Player → 3D Object → Capsule
   - Name: `Model`
   - Position: (0, 0, 0)
   - Scale: (1, 1, 1)
   - Remove Capsule Collider component

## 3.2 Add CharacterController

1. **Select Player (root):**
   - Add Component → Character Controller

2. **Configure CharacterController:**
   ```
   Center: (0, 0.8, 0)
   Radius: 0.5
   Height: 1.6
   Skin Width: 0.08
   Min Move Distance: 0.001
   ```

## 3.3 Add Player Scripts

1. **Add Components to Player:**
   - Add Component → Player Controller
   - Add Component → Player Health
   - Add Component → Player Combat
   - Add Component → Visual Feedback

2. **Configure PlayerController:**
   ```
   Movement Settings:
   ├─ Move Speed: 5
   ├─ Sprint Multiplier: 1.5
   ├─ Jump Height: 2
   └─ Gravity: -20
   
   Input:
   └─ Input Actions: Drag InputSystem_Actions here
   ```

3. **Configure PlayerHealth:**
   ```
   Health Settings:
   ├─ Max Health: 100
   └─ Current Health: 100 (auto-set)
   
   Events:
   └─ (Leave for now - wire to UI later)
   ```

4. **Configure PlayerCombat:**
   ```
   Combat Settings:
   ├─ Base Damage: 10
   ├─ Attack Range: 2
   ├─ Attack Cooldown: 0.5
   
   Targeting:
   └─ Enemy Layer: Enemy (Layer 6)
   ```

5. **Configure VisualFeedback:**
   ```
   Renderers:
   └─ Element 0: Model (Renderer component)
   
   Settings:
   ├─ Flash Color: Red
   └─ Flash Duration: 0.1
   ```

---

# 4. Create Camera

## 4.1 Setup Main Camera

1. **Select Main Camera in hierarchy**

2. **Reset Transform:**
   - Position: (0, 0, 0)
   - Rotation: (0, 0, 0)

3. **Verify Tag:**
   - Tag: `MainCamera` ✓

4. **Add Cinemachine Brain:**
   - Add Component → Cinemachine Brain
   - Update Method: `Smart Update`
   - Blend Update Method: `Late Update`

## 4.2 Create Virtual Camera

1. **Create Cinemachine Camera:**
   - Hierarchy → Right-click → Cinemachine → Cinemachine Camera
   - Name: `CM_FollowPlayer`
   - **IMPORTANT:** Must be at scene root, NOT child of MainCamera!

2. **Position Virtual Camera:**
   ```
   Position: (0, 15, -10)
   Rotation: (45, 0, 0)
   ```

3. **Configure Tracking:**
   - Tracking Target: Drag `Player` here
   - Look At Target: Drag `Player` here

4. **Configure Position Composer:**
   - Find "Cinemachine Position Composer"
   - Target Offset Y: 1
   - Camera Distance: 15
   - Screen Position: (0, 0) ← CENTER in Cinemachine 3.x!
   - Dead Zone Width: 0.1
   - Dead Zone Height: 0.1
   - Damping: (2, 2, 2)

5. **Remove Rotation Composer:**
   - Find "Cinemachine Rotation Composer"
   - Click "−" to remove it
   - We use fixed rotation (45° isometric)

---

# 5. Create Enemy Prefab

## 5.1 Create Enemy GameObject

1. **Create in hierarchy:**
   - Right-click → Create Empty
   - Name: `Enemy`
   - Position: (5, 0, 0)
   - Tag: `Enemy`
   - Layer: `Enemy`

2. **Add visual model:**
   - Right-click Enemy → 3D Object → Cube
   - Name: `Model`
   - Local Position: (0, 0, 0)
   - Scale: (1, 1, 1)
   - Remove Box Collider

## 5.2 Configure Enemy Components

1. **Add CharacterController to Enemy root:**
   ```
   Center: (0, 0.8, 0)
   Radius: 0.5
   Height: 1.6
   Skin Width: 0.08
   ```

2. **Add Enemy Scripts:**
   - Add Component → Enemy AI
   - Add Component → Enemy Health
   - Add Component → Visual Feedback
   - Add Component → Visual Model Aligner

3. **Configure EnemyAI:**
   ```
   Movement Settings:
   ├─ Move Speed: 3.5
   ├─ Rotation Speed: 5
   └─ Stopping Distance: 1.5
   
   Attack Settings:
   ├─ Attack Range: 2
   ├─ Attack Damage: 10
   └─ Attack Cooldown: 1.5
   
   Detection:
   └─ Detection Range: 15
   
   Note: Player is auto-found by tag - no manual reference needed!
   ```

4. **Configure EnemyHealth:**
   ```
   Health Settings:
   └─ Max Health: 50
   
   Rewards:
   └─ Currency Reward: 10 ← IMPORTANT! This is how players earn money!
   
   Note: No Events section - EnemyHealth auto-connects to GameProgressionManager!
   ```

5. **Configure VisualFeedback:**
   ```
   Renderers:
   └─ Element 0: Model (Renderer)
   ```

6. **Configure VisualModelAligner:**
   ```
   Settings:
   └─ Visual Model: Drag Model child here ← Optional, auto-finds child named "Model"
   
   (Runs automatically on Awake)
   ```

## 5.3 Create Prefab

1. **Create Prefabs folder:**
   - Project → /Assets → Create Folder → "Prefabs"

2. **Make Prefab:**
   - Drag `Enemy` from Hierarchy to `/Assets/Prefabs/`
   - Delete `Enemy` from Hierarchy (we'll spawn via WaveSpawner)

---

# 6. Create Defense Zones

## 6.1 Create Zone Structure

1. **Create parent:**
   - Hierarchy → Create Empty
   - Name: `DefenseZones`
   - Position: (0, 0, 0)

2. **Create Zone 1 (Frontline):**
   ```
   DefenseZones
   └── DefenseZone_1
       ├─→ Position: (0, 0, 30)
       └─→ Components: DefenseZone
   ```

3. **Create Zone 2 (Middle):**
   ```
   DefenseZones
   └── DefenseZone_2
       ├─→ Position: (0, 0, 15)
       └─→ Components: DefenseZone
   ```

4. **Create Zone 3 (Base):**
   ```
   DefenseZones
   └── DefenseZone_3
       ├─→ Position: (0, 0, 0)
       └─→ Components: DefenseZone
   ```

## 6.2 Configure Defense Zones

**DefenseZone_1:**
```
Zone Settings:
├─ Zone Index: 0
├─ Zone Name: "Frontline"
├─ Is Active: ✓ (checked)
├─ Perk Multiplier: 0.0
└─ Fallback Health Threshold: 0.25
```

**DefenseZone_2:**
```
Zone Settings:
├─ Zone Index: 1
├─ Zone Name: "Middle Ground"
├─ Is Active: ☐ (unchecked)
├─ Perk Multiplier: 0.25
└─ Fallback Health Threshold: 0.25
```

**DefenseZone_3:**
```
Zone Settings:
├─ Zone Index: 2
├─ Zone Name: "Last Stand"
├─ Is Active: ☐ (unchecked)
├─ Perk Multiplier: 0.5
└─ Fallback Health Threshold: 0.0
```

## 6.3 Add Spawn Points to Each Zone

For each zone, add 5 spawn points:

1. **Create spawn points for Zone 1:**
   ```
   DefenseZone_1
   ├── SpawnPoint1 (Empty GameObject at 30, 0, 30)
   ├── SpawnPoint2 (Empty GameObject at 25, 0, 30)
   ├── SpawnPoint3 (Empty GameObject at 30, 0, 35)
   ├── SpawnPoint4 (Empty GameObject at 35, 0, 30)
   └── SpawnPoint5 (Empty GameObject at 30, 0, 25)
   ```

2. **Repeat for Zones 2 and 3** (adjust positions)

3. **Assign to DefenseZone component:**
   - Select DefenseZone_1
   - Spawn Points: Size 5
   - Drag each spawn point

---

# 7. Create Base Area with NPCs

## 7.1 Create Base Structure

1. **Create base parent:**
   ```
   Hierarchy → Create Empty
   Name: Base
   Position: (0, 0, -10)
   ```

2. **Create base ground:**
   ```
   Base
   └── Base_Ground (3D Object → Plane)
       ├─→ Position: (0, 0, 0)
       ├─→ Scale: (2, 1, 2)
       └─→ Material: Any color
   ```

3. **Create base gate:**
   ```
   Base
   └── Base_Gate (3D Object → Cube)
       ├─→ Position: (0, 2, 5)
       ├─→ Scale: (8, 4, 0.5)
       └─→ Add Component: Base Gate
   ```

4. **Configure BaseGate:**
   ```
   Settings:
   ├─ Open Position Y: 6
   ├─ Close Position Y: 2
   ├─ Animation Speed: 2
   └─ Gate Transform: (auto-filled)
   ```

5. **Create base trigger:**
   ```
   Base
   └── Base_Trigger (3D Object → Cube)
       ├─→ Position: (0, 1, 0)
       ├─→ Scale: (8, 2, 8)
       ├─→ Remove Mesh Renderer
       ├─→ Box Collider → Is Trigger: ✓
       └─→ Add Component: Base Trigger
   ```

6. **Configure BaseTrigger:**
   ```
   Settings:
   └─ Player Tag: "Player"
   ```

## 7.2 Create NPC Vendors

### Create NPC Parent
```
Base
└── NPCs (Empty GameObject)
    ├─→ Position: (0, 0, 0)
```

### NPC 1: Weapon Vendor (Blacksmith)

1. **Create GameObject:**
   ```
   NPCs
   └── NPC_Blacksmith
       ├─→ Position: (4, 0, 0)
       └─→ Model (3D Object → Capsule)
           ├─→ Scale: (1, 1, 1)
           └─→ Material: Red/Orange
   ```

2. **Add ShopNPC component:**
   ```
   NPC Configuration:
   ├─ NPC Type: Weapon Vendor
   ├─ NPC Name: "Blacksmith"
   └─ Interaction Range: 3
   
   Weapon Vendor:
   └─ Available Weapons: (assign later)
   
   UI References:
   ├─ Interaction Prompt: (create below)
   └─ Shop UI: (create in step 11 or leave empty)
   
   Visual Feedback:
   └─ Highlight Color: Yellow
   ```

3. **Create Interaction Prompt:**
   ```
   NPC_Blacksmith
   └── InteractionPrompt (UI → Canvas)
       ├─→ Render Mode: World Space
       ├─→ Position: (0, 2.5, 0)
       ├─→ Width: 200, Height: 50
       ├─→ Scale: (0.01, 0.01, 0.01)
       └─→ Child: TextMeshPro Text
           ├─→ Text: "[E] Talk to Blacksmith"
           ├─→ Font Size: 24
           ├─→ Alignment: Center
           └─→ Auto Size: ✓
   ```

4. **Add NPCInteractionPrompt to Canvas:**
   - Select `InteractionPrompt` canvas
   - Add Component → NPC Interaction Prompt
   - It auto-configures

5. **Wire prompt to ShopNPC:**
   - Select `NPC_Blacksmith`
   - Drag `InteractionPrompt` to "Interaction Prompt" field

### NPC 2: Stat Upgrade Vendor (Trainer)

1. **Create GameObject:**
   ```
   NPCs
   └── NPC_Trainer
       ├─→ Position: (-4, 0, 0)
       └─→ Model (3D Object → Capsule)
           ├─→ Scale: (1, 1, 1)
           └─→ Material: Blue/Green
   ```

2. **Add ShopNPC component:**
   ```
   NPC Configuration:
   ├─ NPC Type: Stat Upgrade Vendor
   ├─ NPC Name: "Trainer"
   └─ Interaction Range: 3
   
   Stat Upgrade Vendor:
   └─ Available Upgrades: (assign later)
   
   UI References:
   ├─ Interaction Prompt: (create below)
   └─ Shop UI: (leave empty for now)
   ```

3. **Create Interaction Prompt:**
   - Same as Blacksmith, but text: "[E] Talk to Trainer"

4. **Add NPCInteractionPrompt and wire to NPC**

---

# 8. Create Wave Spawner

## 8.1 Create WaveSpawner GameObject

1. **Create in hierarchy:**
   ```
   Create Empty
   Name: WaveSpawner
   Position: (0, 0, 0)
   ```

2. **Add WaveSpawner component**

3. **Add WaveController component**

## 8.2 Configure WaveSpawner

```
Wave Settings:
├─ Enemies Per Wave: 5
├─ Wave Delay: 5
├─ Difficulty Scaling: 1.2
└─ Max Enemies Alive: 10

Spawning:
├─ Enemy Prefab: Drag Enemy prefab here
└─ Spawn Points: (leave empty - uses DefenseZone)

References:
├─ Player: Drag Player
└─ UI Text: (leave empty or assign later)
```

## 8.3 Configure WaveController

```
References:
├─ Wave Spawner: Drag WaveSpawner (same GameObject)
└─ Defense Zones: Size 3
    ├─ Element 0: DefenseZone_1
    ├─ Element 1: DefenseZone_2
    └─ Element 2: DefenseZone_3
```

---

# 9. Create ScriptableObject Data

## 9.1 Create Folders

```
/Assets
└── Data (new folder)
    ├── Weapons (new folder)
    └── Upgrades (new folder)
```

## 9.2 Create Weapon Data

Create 4 weapons:

### Weapon 1: Basic Sword
```
Right-click /Assets/Data/Weapons → Create → Weapon Data
Name: Weapon_BasicSword

Settings:
├─ Weapon Name: "Basic Sword"
├─ Purchase Cost: 0
├─ Damage Multiplier: 1.0
├─ Range Multiplier: 1.0
├─ Weapon Effect: None
├─ Effect Chance: 0
└─ Effect Value: 0
```

### Weapon 2: Fire Blade
```
Name: Weapon_FireBlade

Settings:
├─ Weapon Name: "Fire Blade"
├─ Purchase Cost: 100
├─ Damage Multiplier: 1.5
├─ Range Multiplier: 1.0
├─ Weapon Effect: Burn
├─ Effect Chance: 0.3
└─ Effect Value: 5
```

### Weapon 3: Ice Sword
```
Name: Weapon_IceSword

Settings:
├─ Weapon Name: "Ice Sword"
├─ Purchase Cost: 150
├─ Damage Multiplier: 1.2
├─ Range Multiplier: 1.2
├─ Weapon Effect: Freeze
├─ Effect Chance: 0.4
└─ Effect Value: 0.5
```

### Weapon 4: Storm Hammer
```
Name: Weapon_StormHammer

Settings:
├─ Weapon Name: "Storm Hammer"
├─ Purchase Cost: 200
├─ Damage Multiplier: 2.0
├─ Range Multiplier: 0.8
├─ Weapon Effect: Lightning
├─ Effect Chance: 0.25
└─ Effect Value: 3
```

## 9.3 Create Upgrade Data

Create 6 stat upgrades:

### Upgrade 1: Move Speed
```
Right-click /Assets/Data/Upgrades → Create → Upgrade Data
Name: Upgrade_MoveSpeed

Settings:
├─ Upgrade Name: "Move Speed"
├─ Upgrade Type: Move Speed
├─ Base Cost: 50
├─ Cost Increase Per Level: 1.5
├─ Max Level: 10
└─ Description: "Increases movement speed"
```

### Upgrade 2: Max Health
```
Name: Upgrade_MaxHealth
Upgrade Type: Max Health
Base Cost: 50
Cost Increase: 1.5
Max Level: 10
```

### Upgrade 3: Damage
```
Name: Upgrade_Damage
Upgrade Type: Damage
Base Cost: 50
Cost Increase: 1.5
Max Level: 10
```

### Upgrade 4: Crit Chance
```
Name: Upgrade_CritChance
Upgrade Type: Crit Chance
Base Cost: 75
Cost Increase: 1.5
Max Level: 10
```

### Upgrade 5: Crit Damage
```
Name: Upgrade_CritDamage
Upgrade Type: Crit Damage
Base Cost: 75
Cost Increase: 1.5
Max Level: 10
```

### Upgrade 6: Attack Range
```
Name: Upgrade_AttackRange
Upgrade Type: Attack Range
Base Cost: 60
Cost Increase: 1.5
Max Level: 10
```

---

# 10. Configure GameManagers

## 10.1 Add Singleton Components

**Select GameManagers GameObject, add:**

1. Add Component → Game Progression Manager
2. Add Component → Player Stats
3. Add Component → Weapon System
4. Add Component → Game Manager

## 10.2 Configure GameProgressionManager

```
Base Settings:
├─ Base Timer Duration: 40
└─ Current Defense Zone Index: 0

Starting Currency:
└─ Current Currency: 0

Defense Zones:
└─ Size: 3
    ├─ Element 0: DefenseZone_1
    ├─ Element 1: DefenseZone_2
    └─ Element 2: DefenseZone_3

Events:
└─ (All UnityEvents - leave for now)
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

Per Level Increases:
├─ Move Speed Per Level: 0.5
├─ Max Health Per Level: 20
├─ Damage Per Level: 5
├─ Crit Chance Per Level: 0.05
├─ Crit Damage Per Level: 0.25
└─ Attack Range Per Level: 0.5

Current Levels:
└─ (All start at 0)

References:
└─ None! PlayerStats auto-finds player components

Note: PlayerStats automatically locates PlayerController, PlayerHealth, 
and PlayerCombat when applying upgrades. No manual references needed!
```

## 10.4 Configure WeaponSystem

```
Starting Weapon:
└─ Equipped Weapon: Drag Weapon_BasicSword

Events:
└─ (Leave for now)
```

## 10.5 Configure GameManager

```
References:
├─ Player: Drag Player
└─ Game UI: (leave empty or assign later)

Settings:
└─ (Leave defaults)
```

## 10.6 Wire NPCs to ScriptableObjects

### NPC_Blacksmith
```
Weapon Vendor:
└─ Available Weapons: Size 4
    ├─ Element 0: Weapon_BasicSword
    ├─ Element 1: Weapon_FireBlade
    ├─ Element 2: Weapon_IceSword
    └─ Element 3: Weapon_StormHammer
```

### NPC_Trainer
```
Stat Upgrade Vendor:
└─ Available Upgrades: Size 6
    ├─ Element 0: Upgrade_MoveSpeed
    ├─ Element 1: Upgrade_MaxHealth
    ├─ Element 2: Upgrade_Damage
    ├─ Element 3: Upgrade_CritChance
    ├─ Element 4: Upgrade_CritDamage
    └─ Element 5: Upgrade_AttackRange
```

---

# 11. Create UI (Optional)

## 11.1 Simple Testing UI (Recommended First)

You can skip full UI for now and use Console logs. The game works without UI!

**What works without UI:**
- ✅ Player movement, jump, combat
- ✅ Enemy AI and health
- ✅ Wave spawning
- ✅ Currency system (check Console)
- ✅ NPC interaction (check Console)
- ✅ Purchases logged to Console

## 11.2 Minimal HUD (Optional)

If you want basic UI:

1. **Create Canvas:**
   ```
   Hierarchy → UI → Canvas
   Name: GameHUD
   Render Mode: Screen Space - Overlay
   ```

2. **Add Currency Display:**
   ```
   GameHUD
   └── CurrencyText (UI → TextMeshPro)
       ├─→ Anchor: Top-Right
       ├─→ Position: (-100, -50)
       ├─→ Text: "Gold: 0"
       └─→ Font Size: 36
   ```

3. **Add Health Display:**
   ```
   GameHUD
   └── HealthText (UI → TextMeshPro)
       ├─→ Anchor: Top-Left
       ├─→ Position: (100, -50)
       ├─→ Text: "HP: 100"
       └─→ Font Size: 36
   ```

4. **Add Wave Display:**
   ```
   GameHUD
   └── WaveText (UI → TextMeshPro)
       ├─→ Anchor: Top-Center
       ├─→ Position: (0, -50)
       ├─→ Text: "Wave: 1"
       └─→ Font Size: 36
   ```

5. **Create GameUI script** (or use existing) and wire events

**For now:** Skip UI and use Console logs!

---

# 12. Test Everything

## 12.1 Pre-Flight Checklist

Before pressing Play, verify:

**✓ GameManagers:**
- [ ] GameProgressionManager added
- [ ] PlayerStats added
- [ ] WeaponSystem added
- [ ] GameManager added

**✓ Player:**
- [ ] Tag: "Player"
- [ ] Has CharacterController
- [ ] Has PlayerController, PlayerHealth, PlayerCombat
- [ ] Input Actions assigned

**✓ Camera:**
- [ ] Main Camera has CinemachineBrain
- [ ] Virtual Camera at scene root (not child!)
- [ ] Tracking Player

**✓ Enemy Prefab:**
- [ ] Tag: "Enemy", Layer: "Enemy"
- [ ] Has CharacterController, EnemyAI, EnemyHealth
- [ ] Currency Reward > 0
- [ ] Saved as prefab in /Assets/Prefabs/

**✓ Defense Zones:**
- [ ] 3 zones created
- [ ] Zone 1 IsActive = true
- [ ] Each has spawn points assigned
- [ ] All assigned to GameProgressionManager

**✓ Base Area:**
- [ ] BaseGate created with BaseGate component
- [ ] BaseTrigger created with trigger collider
- [ ] 2 NPCs created with ShopNPC components
- [ ] Interaction prompts added

**✓ Wave Spawner:**
- [ ] WaveSpawner and WaveController components
- [ ] Enemy prefab assigned
- [ ] Defense zones assigned to WaveController

**✓ ScriptableObjects:**
- [ ] 4 weapons created
- [ ] 6 upgrades created
- [ ] Assigned to NPCs

## 12.2 Test Sequence

### Test 1: Player Movement
1. Press Play
2. Use WASD - player should move
3. Press Space - player should jump
4. Hold Shift + WASD - player should sprint
5. **Expected:** Smooth movement with camera following

### Test 2: Combat (Manual Test)
1. Create temporary enemy in scene:
   - Drag Enemy prefab to hierarchy
   - Place at (5, 0, 0)
2. Press Play
3. Move near enemy
4. Click to attack
5. **Expected:** Enemy takes damage, flashes red, dies
6. **Console:** "Killed enemy! Gained 10 currency"
7. **Console:** "Currency: 10"

### Test 3: Wave Spawning
1. Remove temporary enemy
2. Press Play
3. Wait 5 seconds
4. **Expected:** 5 enemies spawn at Zone 1
5. Kill all enemies
6. **Console:** Currency increases
7. Wait 5 seconds
8. **Expected:** Wave 2 starts with more enemies

### Test 4: Base Interaction
1. Press Play
2. Walk to base area (Z = -10)
3. **Console:** "Entered base area"
4. **Console:** "Gate opening"
5. **Expected:** Gate moves up
6. Walk out of base
7. **Console:** "Exited base"
8. **Expected:** Gate closes, wave starts

### Test 5: NPC Interaction
1. Press Play
2. Walk to base
3. Approach Blacksmith (capsule with red material)
4. **Console:** "Near Blacksmith. Press E to interact."
5. **Expected:** NPC glows yellow, prompt appears
6. Press E
7. **Console:** "Opened Blacksmith's shop (WeaponVendor)"
8. **Expected:** Time pauses (timeScale = 0)
9. Press E again
10. **Console:** "Closed Blacksmith's shop"
11. Walk to Trainer, repeat
12. **Expected:** Same interaction with different shop type

### Test 6: Currency & Purchases
1. Give yourself currency via Inspector:
   - While playing
   - Find GameProgressionManager
   - Set Current Currency: 200
2. Open Blacksmith shop (Press E near NPC)
3. In Inspector, find NPC_Blacksmith → ShopNPC component
4. Expand TryPurchaseWeapon() in Inspector
5. Manually call with Weapon_FireBlade
6. **Console:** "Purchased weapon: Fire Blade for 100 currency!"
7. **Console:** "Currency: 100"
8. Attack enemy
9. **Expected:** Higher damage (1.5x), chance of burn effect

### Test 7: Stat Upgrades
1. Open Trainer shop
2. In Inspector, call TryPurchaseUpgrade(Upgrade_MoveSpeed, 0)
3. **Console:** "Purchased Move Speed for 50 currency!"
4. **Expected:** Player moves faster
5. Call again with level 1
6. **Console:** Cost is now 75 (exponential scaling)

## 12.3 Common Issues

### Issue: Input doesn't work
**Solution:**
- Verify Project Settings → Active Input Handling = "Input System Package (New)"
- Restart Unity
- Check Input Actions assigned to PlayerController

### Issue: Enemies don't spawn
**Solution:**
- Check WaveSpawner has enemy prefab assigned
- Check Defense Zones assigned to WaveController
- Check spawn points assigned to DefenseZone_1
- Check Console for errors

### Issue: Camera doesn't follow
**Solution:**
- Verify Virtual Camera is at scene root (NOT child of MainCamera!)
- Check Tracking Target assigned to Player
- Check CinemachineBrain on MainCamera

### Issue: NPCs don't respond
**Solution:**
- Check Player tag is "Player"
- Check interaction range (default 3 units)
- Walk closer to NPC
- Check Console for "Near [NPC]" message

### Issue: Can't purchase items
**Solution:**
- Check GameProgressionManager currency > 0
- Check ScriptableObjects assigned to NPCs
- Check Console for "Not enough currency" message
- Manually set currency in Inspector while playing

### Issue: Enemies float above ground
**Solution:**
- Check VisualModelAligner on enemy prefab
- Check CharacterController Center Y = 0.8, Height = 1.6
- Check Model is assigned to VisualModelAligner

---

# Next Steps

## Immediate (After Setup)

1. **Test all systems** - Follow Test Sequence above
2. **Adjust values** - Tweak enemy health, damage, currency rewards
3. **Add more enemies** - Test wave scaling
4. **Test NPC shops** - Verify purchases work

## Short Term (1-2 hours)

1. **Create proper shop UI panels** (see NPC_SHOP_SETUP.md)
2. **Add materials/colors** - Differentiate player, enemies, NPCs
3. **Create ground plane** - Build level geometry
4. **Add more weapons** - Test different effects
5. **Polish camera** - Fine-tune isometric angle

## Medium Term (1 day)

1. **Build complete UI** - Health bars, currency display, shop panels
2. **Add visual effects** - Particles, trails, hit effects
3. **Add audio** - SFX for attacks, purchases, waves
4. **Create multiple enemy types** - Fast, tank, ranged
5. **Implement animations** - Character movement, attacks

## Long Term (1 week+)

1. **Meta-progression** - Persistent upgrades between runs
2. **Boss encounters** - Special waves
3. **More zones/levels** - Different arenas
4. **Save system** - Persistence
5. **Balancing** - Full playthrough testing

---

# Quick Reference

## Essential Keyboard Controls
- **WASD** - Move
- **Space** - Jump
- **Shift** - Sprint
- **Left Mouse** - Attack
- **E** - Interact with NPC
- **ESC** - Close shop

## Important Layer Assignments
- Layer 6: Enemy
- Layer 0: Default (player, ground)

## Important Tag Assignments
- Player GameObject: "Player"
- Enemy Prefab: "Enemy"

## File Locations
```
/Assets
├── InputSystem_Actions.inputactions
├── /Scripts
│   ├── /Player (PlayerController, PlayerHealth, PlayerCombat)
│   ├── /Enemy (EnemyAI, EnemyHealth)
│   └── /Systems (All manager scripts)
├── /Prefabs
│   └── Enemy.prefab
├── /Data
│   ├── /Weapons (4 weapon ScriptableObjects)
│   └── /Upgrades (6 upgrade ScriptableObjects)
└── /Guide
    └── All documentation
```

## GameObject Hierarchy
```
Hierarchy:
├── GameManagers (4 singleton components)
├── Player (CharacterController + 3 scripts)
├── Main Camera (CinemachineBrain)
├── CM_FollowPlayer (Virtual camera at root!)
├── WaveSpawner (2 components)
├── DefenseZones
│   ├── DefenseZone_1 (5 spawn points)
│   ├── DefenseZone_2 (5 spawn points)
│   └── DefenseZone_3 (5 spawn points)
└── Base
    ├── Base_Ground
    ├── Base_Gate (BaseGate component)
    ├── Base_Trigger (BaseTrigger component)
    └── NPCs
        ├── NPC_Blacksmith (ShopNPC)
        └── NPC_Trainer (ShopNPC)
```

---

# Troubleshooting Quick Links

- **Input not working** → Section 12.3
- **Camera issues** → Section 4.2
- **Enemy spawning** → Section 12.3
- **NPC interaction** → Section 12.3
- **Visual floating** → Enemy prefab has VisualModelAligner

---

# Documentation Index

**Core Setup:**
- This file: COMPLETE_SETUP_GUIDE.md

**System Details:**
- PROJECT_CONTEXT.md - Project overview
- UPGRADE_SYSTEM_GUIDE.md - Progression systems
- NPC_SHOP_SETUP.md - NPC vendor details
- SHOP_SYSTEMS_COMPARISON.md - UI vs NPC shops

**Quick Starts:**
- QUICK_START.md - 10-minute progression setup
- IMPLEMENTATION_SUMMARY.md - System overview

**Reference:**
- SYSTEM_ARCHITECTURE.md - Visual diagrams
- FINAL_CHECKLIST.md - Verification matrix

---

**You're ready to build! Follow each section in order. Estimated time: 60-90 minutes.** 🎮

Good luck! If you encounter issues, check section 12.3 for common problems and solutions.
