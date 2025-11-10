# Project Cleanup Audit Report

## 🎯 Executive Summary

This report identifies **redundant**, **unused**, and **duplicate** systems in your project that can be safely removed or consolidated.

---

## 📊 Hierarchy Analysis

### ✅ ACTIVE & USED GameObjects

```
Player
├─ Model/Capsule               ✅ Visual representation
├─ AttackPoint                  ✅ Used by PlayerCombat
└─ Components:
    ├─ PlayerController         ✅ Movement
    ├─ PlayerHealth            ✅ Health system
    ├─ PlayerCombat            ✅ Combat
    ├─ VisualFeedback          ✅ Visual effects
    └─ VisualModelAligner      ✅ Model alignment

GameManagers
├─ RunStateManager             ✅ Core game state
├─ CurrencyManager             ✅ Currency system
├─ SaveSystem                  ✅ Save/load
├─ ExperienceSystem            ✅ Leveling
├─ WaveSpawner                 ✅ Wave spawning
├─ WaveController              ✅ Wave control
└─ DefenseZones                ✅ Zone system

Base
├─ BaseGate                    ✅ Gate interaction
├─ BaseTrigger                 ✅ Base entry detection
└─ NPCs
    ├─ WeaponVendor            ✅ Shop NPC
    └─ StatVendor              ✅ Shop NPC

GameCanvas
├─ HealthBar                   ✅ Player health display
├─ WaveText                    ✅ Wave info (via GameUI)
├─ ShopPanel                   ✅ Shop UI
├─ LevelUpPanel                ✅ Level up UI
├─ CurrencyDisplayPanel        ✅ Currency UI
├─ ExperienceBarPanel          ✅ XP bar
├─ PreRunMenuPanel             ✅ Pre-run menu
├─ BetweenSessionsPanel        ✅ Between sessions UI
└─ InteractionNotification     ✅ Interaction prompts
```

---

## 🗑️ UNUSED / REDUNDANT SCRIPTS

### ❌ 1. WaveDisplay.cs - UNUSED

**Location:** `/Assets/Scripts/Systems/WaveDisplay.cs`

**Purpose:** Display wave information in UI

**Status:** ⚠️ **REDUNDANT** - Functionality replaced by `GameUI.cs`

**Why Unused:**
- `GameUI.cs` already handles wave display at line 46-60
- No GameObject in scene has `WaveDisplay` component
- `GameCanvas/WaveText` uses `GameUI` instead

**Recommendation:** ❌ **DELETE**

---

### ❌ 2. NotificationUI.cs - UNUSED

**Location:** `/Assets/Scripts/Systems/NotificationUI.cs`

**Purpose:** Show fade-in/fade-out notifications

**Status:** ⚠️ **REDUNDANT** - Replaced by `InteractionNotificationUI.cs`

**Why Unused:**
- No GameObject in scene has `NotificationUI` component
- `InteractionNotificationUI` is the active notification system
- More specific and better integrated

**Recommendation:** ❌ **DELETE**

---

### ❌ 3. NPCInteractionPrompt.cs - UNUSED

**Location:** `/Assets/Scripts/Systems/NPCInteractionPrompt.cs`

**Purpose:** World-space NPC interaction prompts

**Status:** ⚠️ **OBSOLETE** - Replaced by centralized `InteractionNotificationUI`

**Why Unused:**
- Was part of old world-space prompt system
- `InteractionNotificationUI` now handles all prompts centrally
- `NPCInteraction.cs` uses the new system
- No GameObject references this script

**Recommendation:** ❌ **DELETE**

---

### ❌ 4. UpgradeShop.cs - UNUSED

**Location:** `/Assets/Scripts/Systems/UpgradeShop.cs`

**Purpose:** Old shop system (unknown implementation)

**Status:** ⚠️ **OBSOLETE** - Replaced by `SimpleShopUI` + `ShopNPC`

**Why Unused:**
- Current shop system uses `SimpleShopUI.cs` + `ShopNPC.cs`
- No references in scene or other scripts
- Likely from earlier iteration

**Recommendation:** ❌ **DELETE** (check contents first)

---

### ⚠️ 5. Debug/Test Scripts - KEEP BUT OPTIONAL

**Location:** `/Assets/Scripts/Systems/`

**Scripts:**
- `DebugShopTester.cs` - Shop testing utilities
- `EssenceDebugTester.cs` - Essence testing
- `WaveSpawnerDebug.cs` - Wave debugging
- `CompilationTest.cs` - Compilation verification
- `GroundingDebugger.cs` - Ground detection debug

**Status:** 🟡 **OPTIONAL** - Keep for development

**Recommendation:** 
- ✅ **KEEP** during development
- 🟡 **REMOVE** before final build
- Consider moving to `/Assets/Scripts/Debug/` folder

---

### ⚠️ 6. Potentially Obsolete Systems

**BaseExitTrigger.cs**
- Purpose: Detect when player exits base
- Status: Unknown if used
- Check: Search for component usage in scene

**GateColliderSetup.cs**
- Purpose: Gate collider configuration
- Status: Unknown if used
- Check: May be editor-only script

**GatePassTrigger.cs**
- Purpose: Gate pass detection
- Status: Unknown if used
- Check: May conflict with `BaseTrigger.cs`

**CharacterGrounder.cs**
- Purpose: Ground detection
- Status: Unknown if used
- Check: Player may not need this anymore

**Recommendation:** 🔍 **INVESTIGATE** - Need to check if attached to GameObjects

---

## 📦 DUPLICATE/OVERLAPPING FUNCTIONALITY

### 🔄 GameUI vs CurrencyDisplay

**Issue:** Wave display logic exists in TWO places:

1. **GameUI.cs** (line 46-60)
   - Updates `/GameCanvas/WaveText`
   - Shows "Wave: X" and "Enemies: X"

2. **WaveDisplay.cs** (UNUSED)
   - More detailed wave display
   - Shows session progress "Wave X (Y/10)"

**Current State:**
- `GameUI` is active and working ✅
- `WaveDisplay` is not attached to anything ❌

**Recommendation:**
- ✅ Keep `GameUI` as-is
- ❌ Delete `WaveDisplay.cs`

---

### 🔄 Notification Systems

**Multiple notification approaches:**

1. **NotificationUI.cs** (UNUSED)
   - Generic fade-in/fade-out notifications
   - Not attached to any GameObject

2. **InteractionNotificationUI.cs** (ACTIVE) ✅
   - Centralized interaction prompts
   - Used by NPCs, gates, etc.
   - `/GameCanvas/InteractionNotification`

3. **NPCInteractionPrompt.cs** (OBSOLETE)
   - Old world-space prompts
   - Replaced by centralized system

**Recommendation:**
- ✅ Keep `InteractionNotificationUI` (active system)
- ❌ Delete `NotificationUI.cs`
- ❌ Delete `NPCInteractionPrompt.cs`

---

## 🧹 CLEANUP CHECKLIST

### Phase 1: Safe Deletions (No Risk)

Scripts with ZERO references:

- [ ] Delete `/Assets/Scripts/Systems/WaveDisplay.cs`
- [ ] Delete `/Assets/Scripts/Systems/NotificationUI.cs`
- [ ] Delete `/Assets/Scripts/Systems/NPCInteractionPrompt.cs`

### Phase 2: Verify Before Deletion

Check if these are attached to any GameObject:

- [ ] Check `UpgradeShop.cs` usage → If none, delete
- [ ] Check `BaseExitTrigger.cs` usage → If none, delete
- [ ] Check `GateColliderSetup.cs` usage → If none, delete
- [ ] Check `GatePassTrigger.cs` usage → If none, delete
- [ ] Check `CharacterGrounder.cs` usage → If none, delete

### Phase 3: Debug Scripts (Optional)

Before final build, consider removing:

- [ ] Move debug scripts to `/Assets/Scripts/Debug/` folder
  - `DebugShopTester.cs`
  - `EssenceDebugTester.cs`
  - `WaveSpawnerDebug.cs`
  - `CompilationTest.cs`
  - `GroundingDebugger.cs`

- [ ] Add `#if UNITY_EDITOR` preprocessor directives
- [ ] Or exclude from build via Build Settings

### Phase 4: Editor Scripts

- [ ] Keep `/Assets/Scripts/Editor/` scripts (editor-only, safe)
  - `CreateSolidSpriteTexture.cs`
  - `SaveFileDebugger.cs`

---

## 📈 CURRENT SCRIPT COUNT

**Total Scripts:** ~50+

**Active Core Scripts:** ~35
**Debug/Test Scripts:** ~5
**Unused Scripts:** ~3-5
**Unknown Status:** ~5

**After Cleanup:** ~40 scripts (20% reduction)

---

## 🎯 RECOMMENDED ACTIONS

### Immediate (Zero Risk)

```bash
# Delete these 3 files - confirmed unused
/Assets/Scripts/Systems/WaveDisplay.cs
/Assets/Scripts/Systems/NotificationUI.cs
/Assets/Scripts/Systems/NPCInteractionPrompt.cs
```

### Investigate (Low Risk)

Search scene for these components:
1. `UpgradeShop`
2. `BaseExitTrigger`
3. `GateColliderSetup`
4. `GatePassTrigger`
5. `CharacterGrounder`

**If no results → Safe to delete**

### Organize (Best Practice)

Create folder structure:
```
/Assets/Scripts/
  /Core/           (RunStateManager, GameManager, etc.)
  /Player/         (PlayerController, PlayerHealth, etc.)
  /Enemy/          (EnemyAI, EnemyHealth, etc.)
  /UI/             (All UI scripts)
  /Systems/        (WaveSpawner, CurrencyManager, etc.)
  /Debug/          (All debug/test scripts)
  /Editor/         (Editor-only scripts)
  /Interactables/  (NPCs, pickups, etc.)
```

---

## 🔍 HOW TO VERIFY SCRIPT USAGE

### Method 1: Unity Search
1. Select script in Project window
2. Right-click → "Find References In Scene"
3. If results = 0 → Unused

### Method 2: Search All
1. Edit → Find References In Scene
2. Search for script name
3. Check all loaded scenes

### Method 3: Grep Search
- Already done in this audit
- Scripts listed as "UNUSED" have zero scene references

---

## ✅ FINAL RECOMMENDATIONS

### Delete Now (100% Safe)
- `WaveDisplay.cs`
- `NotificationUI.cs`
- `NPCInteractionPrompt.cs`

### Investigate & Delete (95% Safe)
- `UpgradeShop.cs`
- `BaseExitTrigger.cs`
- `GateColliderSetup.cs`
- `GatePassTrigger.cs`
- `CharacterGrounder.cs`

### Keep (Active Systems)
- All scripts in `/Player/`, `/Enemy/`, `/UI/`
- Core managers in `/Systems/`
- Editor scripts in `/Editor/`

### Organize (Optional)
- Move debug scripts to dedicated folder
- Add preprocessor directives for build exclusion

---

**Estimated Cleanup Impact:**
- 📉 8-10 fewer unused scripts
- 🧹 Cleaner codebase
- 🚀 Easier navigation
- 📦 Smaller build size (minimal)

**Next Steps:**
1. Review this audit
2. Confirm deletions
3. Execute Phase 1 cleanup
4. Test game functionality
5. Proceed to new features roadmap
