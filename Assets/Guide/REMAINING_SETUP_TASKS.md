# Remaining Setup Tasks - Complete Checklist

This guide lists **all remaining tasks** to fully complete your roguelike prototype, organized by priority.

---

## 🎯 Current Status Summary

### ✅ **COMPLETED:**
- Core player movement and combat systems
- Character controller with proper grounding
- Wave spawning and enemy AI
- Defense zone system (3 zones + fallback)
- Base trigger and gate system
- Currency and progression managers
- Weapon and upgrade ScriptableObject systems
- All ScriptableObject assets created (6 upgrades + 5 weapons)
- NPC shop system with 4 vendors
- Shop UI scripts (SimpleShopUI, DebugShopTester)
- Basic HUD elements (health bar, wave text)
- Cinemachine camera setup

### ⚠️ **NEEDS SETUP:**
- Shop UI integration and references
- Currency display UI
- Timer display UI during base phase
- NPC interaction prompts
- Shop item button prefab
- GameUI component assignment
- Some missing component references
- DebugShopTester for testing

### 🔧 **OPTIONAL ENHANCEMENTS:**
- Visual polish for NPCs
- Particle effects for upgrades/purchases
- Sound effects
- More enemy types
- Additional weapons/upgrades

---

## 📋 Priority 1: Critical Shop UI Setup (Required for Testing)

These tasks are required to test the vendor/shop system.

### Task 1.1: Add SimpleShopUI Component to ShopPanel

**Location:** `/GameCanvas/ShopPanel`

**Steps:**
1. Select `GameCanvas > ShopPanel` in Hierarchy
2. Add Component → **SimpleShopUI**
3. The component should now appear (compilation errors are fixed)

**Result:** ShopPanel now has SimpleShopUI component ready for configuration

---

### Task 1.2: Create Shop Item Button Prefab

**Why:** SimpleShopUI needs a button template to display weapons/upgrades

**Steps:**

1. **Create Button GameObject:**
   - Right-click `GameCanvas/ShopPanel` → UI → Button - TextMeshPro
   - Rename to `ItemButton`

2. **Add Child Text Elements:**
   - Right-click `ItemButton` → UI → Text - TextMeshPro
   - Rename to `Name`
   - Set Text: "Item Name"
   - Set Font Size: 18, Color: White
   - Set Anchor: Top-Stretch, Position Y: -10
   
   - Right-click `ItemButton` → UI → Text - TextMeshPro
   - Rename to `Description`
   - Set Text: "Description here"
   - Set Font Size: 12, Color: Light Gray
   - Set Anchor: Middle-Stretch, Position Y: 0
   
   - Right-click `ItemButton` → UI → Text - TextMeshPro
   - Rename to `Cost`
   - Set Text: "$100"
   - Set Font Size: 16, Color: Yellow
   - Set Anchor: Bottom-Right, Position: (-10, 10)

3. **Configure ItemButton:**
   - Set Width: 380, Height: 80
   - Button colors: Normal (dark gray), Highlighted (lighter gray), Pressed (yellow tint)

4. **Create Prefab:**
   - Drag `ItemButton` from Hierarchy to `/Assets/Prefabs/UI/`
   - Delete `ItemButton` from Hierarchy (it will be spawned by code)

**Result:** `/Assets/Prefabs/UI/ItemButton.prefab` ready for use

---

### Task 1.3: Create ScrollView Container in ShopPanel

**Why:** SimpleShopUI needs a container to spawn item buttons into

**Steps:**

1. **Create ScrollView:**
   - Right-click `GameCanvas/ShopPanel` → UI → Scroll View
   - Rename to `ItemScrollView`
   - Set Anchor: Stretch-Stretch
   - Set Margins: Left 20, Right 20, Top 80, Bottom 60

2. **Configure Viewport > Content:**
   - Select `ItemScrollView > Viewport > Content`
   - Add Component → **Vertical Layout Group**
   - Set Spacing: 10
   - Set Padding: 10 on all sides
   - Add Component → **Content Size Fitter**
   - Set Vertical Fit: Preferred Size

3. **Disable Horizontal Scroll:**
   - Select `ItemScrollView`
   - Scrollbar Horizontal: None
   - Delete `ItemScrollView > Scrollbar Horizontal`

**Result:** ShopPanel has a scrollable container for shop items

---

### Task 1.4: Add UI Elements to ShopPanel

**Why:** SimpleShopUI needs text fields and buttons to function

**Steps:**

1. **Add Shop Title Text:**
   - Right-click `GameCanvas/ShopPanel` → UI → Text - TextMeshPro
   - Rename to `ShopTitleText`
   - Set Text: "Shop"
   - Set Font Size: 32, Style: Bold
   - Set Anchor: Top-Center
   - Position: (0, -30, 0)

2. **Add Currency Display Text:**
   - Right-click `GameCanvas/ShopPanel` → UI → Text - TextMeshPro
   - Rename to `ShopCurrencyText`
   - Set Text: "Currency: $0"
   - Set Font Size: 20
   - Set Anchor: Top-Right
   - Position: (-20, -30, 0)

3. **Add Close Button:**
   - Right-click `GameCanvas/ShopPanel` → UI → Button - TextMeshPro
   - Rename to `CloseButton`
   - Set Text: "Close (ESC)"
   - Set Width: 150, Height: 40
   - Set Anchor: Bottom-Center
   - Position: (0, 10, 0)

4. **Configure ShopPanel Background:**
   - Select `GameCanvas/ShopPanel`
   - Set Color: Semi-transparent dark (RGBA: 0.1, 0.1, 0.1, 0.95)
   - Set Anchor: Stretch-Stretch
   - Set Margins: 100 on all sides

**Result:** ShopPanel has all required UI elements

---

### Task 1.5: Assign References in SimpleShopUI

**Location:** `/GameCanvas/ShopPanel` → SimpleShopUI component

**Steps:**

1. Select `GameCanvas/ShopPanel` in Hierarchy
2. In Inspector, find **SimpleShopUI** component
3. Assign references:
   - **Item List Container:** `ItemScrollView/Viewport/Content`
   - **Item Button Prefab:** `/Assets/Prefabs/UI/ItemButton.prefab`
   - **Shop Title Text:** `ShopTitleText` (in ShopPanel)
   - **Currency Text:** `ShopCurrencyText` (in ShopPanel)
   - **Close Button:** `CloseButton` (in ShopPanel)

4. **Disable ShopPanel by default:**
   - Uncheck the checkbox next to `ShopPanel` name in Inspector
   - ShopPanel should be inactive (grayed out in Hierarchy)

**Result:** SimpleShopUI fully configured and ready

---

### Task 1.6: Assign ShopUI Reference in ShopNPC Components

**Location:** `/Base/NPCs/WeaponVendor` and `/Base/NPCs/StatVendor`

**Steps:**

1. **WeaponVendor Setup:**
   - Select `/Base/NPCs/WeaponVendor` in Hierarchy
   - In Inspector, find **ShopNPC** component
   - Assign **Shop UI:** `GameCanvas/ShopPanel` (the GameObject itself)
   - ✅ Available Weapons already assigned (4 weapons)

2. **StatVendor Setup:**
   - Select `/Base/NPCs/StatVendor` in Hierarchy
   - In Inspector, find **ShopNPC** component
   - Assign **Shop UI:** `GameCanvas/ShopPanel` (the GameObject itself)
   - ✅ Available Upgrades already assigned (6 upgrades)

3. **Optional - ConsumableVendor and SpecialVendor:**
   - These vendors exist but have no items assigned
   - You can ignore them for now or assign items later

**Result:** NPCs can open the shop UI when interacted with

---

## 📋 Priority 2: HUD and UI Updates (Important for Gameplay)

### Task 2.1: Add GameUI Component

**Why:** Manages health bar, wave counter, and other HUD elements

**Location:** `/GameCanvas`

**Steps:**

1. Select `GameCanvas` in Hierarchy
2. Add Component → **GameUI**
3. Assign references:
   - **Health Slider:** `GameCanvas/HealthBar`
   - **Health Text:** Create new Text element or leave null
   - **Wave Text:** `GameCanvas/WaveText`
   - **Enemies Text:** Create new Text element or use WaveText
   - **Player Health:** `/Player` (drag Player GameObject)
   - **Wave Spawner:** `/WaveSpawner` (drag WaveSpawner GameObject)

**Alternative:** If you prefer custom HUD management, you can skip this and manually update UI elements.

**Result:** HUD displays health and wave information

---

### Task 2.2: Update Currency Display

**Why:** Players need to see how much currency they have

**Location:** `/GameCanvas/CurrencyText`

**Option A - Create Simple Currency UI Script:**

```csharp
using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    
    private void Start()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnCurrencyChanged.AddListener(UpdateDisplay);
            UpdateDisplay(GameProgressionManager.Instance.Currency);
        }
    }
    
    private void UpdateDisplay(int currency)
    {
        if (currencyText != null)
        {
            currencyText.text = $"Currency: ${currency}";
        }
    }
    
    private void OnDestroy()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnCurrencyChanged.RemoveListener(UpdateDisplay);
        }
    }
}
```

**Steps:**
1. Create `/Assets/Scripts/Systems/CurrencyDisplay.cs` with above code
2. Add **CurrencyDisplay** component to `/GameCanvas/CurrencyText`
3. Assign **Currency Text:** `CurrencyText` (itself)

**Option B - Manual Setup:**
- Leave as is and currency will only show in shop UI

**Result:** Currency displayed on HUD at all times

---

### Task 2.3: Update Timer Display

**Why:** Shows countdown during base upgrade phase

**Location:** `/GameCanvas/TimerText`

**Option A - Create Timer Display Script:**

```csharp
using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    private void Start()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnBaseTimerUpdate.AddListener(UpdateTimer);
            GameProgressionManager.Instance.OnEnteredBase.AddListener(ShowTimer);
            GameProgressionManager.Instance.OnExitedBase.AddListener(HideTimer);
        }
        
        HideTimer();
    }
    
    private void UpdateTimer(float timeRemaining)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = $"Next Wave: {minutes:00}:{seconds:00}";
        }
    }
    
    private void ShowTimer()
    {
        gameObject.SetActive(true);
    }
    
    private void HideTimer()
    {
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.OnBaseTimerUpdate.RemoveListener(UpdateTimer);
            GameProgressionManager.Instance.OnEnteredBase.RemoveListener(ShowTimer);
            GameProgressionManager.Instance.OnExitedBase.RemoveListener(HideTimer);
        }
    }
}
```

**Steps:**
1. Create `/Assets/Scripts/Systems/TimerDisplay.cs` with above code
2. Add **TimerDisplay** component to `/GameCanvas/TimerText`
3. Assign **Timer Text:** `TimerText` (itself)

**Result:** Timer shows countdown when in base, hides during waves

---

### Task 2.4: Create NPC Interaction Prompts (Optional but Recommended)

**Why:** Shows "[E] Talk to Blacksmith" when near NPCs

**Steps:**

1. **Create Prompt Prefab:**
   - Create UI → Canvas in scene (temporary)
   - Set Canvas → Render Mode: World Space
   - Set Canvas → Width: 300, Height: 100
   - Add UI → Panel as child
   - Add UI → Text - TextMeshPro to Panel
   - Set Text: "[E] Interact"
   - Style the text (white, centered, font size 24)
   - Add **NPCInteractionPrompt** component to Canvas
   - Assign references in NPCInteractionPrompt
   - Drag Canvas to `/Assets/Prefabs/UI/NPCPrompt.prefab`
   - Delete Canvas from scene

2. **Add Prompts to NPCs:**
   - For each NPC in `/Base/NPCs/`:
     - Drag `NPCPrompt.prefab` as child of the NPC
     - Position above NPC (Y: 2.5)
     - In NPC's **ShopNPC** component:
       - Assign **Interaction Prompt:** the NPCPrompt child GameObject

**Alternative:** Skip for now and test with proximity only

**Result:** Visual feedback when near NPCs

---

## 📋 Priority 3: Testing and Debug Tools

### Task 3.1: Add DebugShopTester Component

**Why:** Allows testing purchases with keyboard shortcuts

**Location:** `/GameManagers`

**Steps:**

1. Select `/GameManagers` in Hierarchy
2. Add Component → **DebugShopTester**
3. Assign test assets:
   - **Upgrades For Testing (Size: 6):**
     - Element 0: `AttackRangeUpgrade`
     - Element 1: `CritChanceUpgrade`
     - Element 2: `CritDamageUpgrade`
     - Element 3: `DamageUpgrade`
     - Element 4: `MaxHealthUpgrade`
     - Element 5: `MoveSpeedUpgrade`
   - **Weapons For Testing (Size: 5):**
     - Element 0: `BasicSword`
     - Element 1: `CriticalDagger`
     - Element 2: `FireBlade`
     - Element 3: `IceBlade`
     - Element 4: `LightningStaff`

4. **Test Controls (During Play Mode):**
   - Press **H** to show help
   - Press **C** to add 100 currency
   - Press **1-6** to buy upgrades directly
   - Press **L** to log current stats

**Result:** Easy testing without needing full shop UI setup

---

### Task 3.2: Verify All Manager Components

**Location:** `/GameManagers`

**Expected Components:**
- ✅ **GameProgressionManager** (handles currency, base timer, zones)
- ✅ **PlayerStats** (handles stat upgrades)
- ✅ **WeaponSystem** (handles weapon switching)
- ✅ **GameManager** (general game state)
- 🆕 **DebugShopTester** (testing tool)

**Steps:**
1. Select `/GameManagers` in Hierarchy
2. Verify all 5 components are present
3. No additional setup needed (all are configured)

**Result:** All manager singletons ready and accessible

---

## 📋 Priority 4: Final Polish and Verification

### Task 4.1: Test Wave System Integration

**Steps:**

1. **Enter Play Mode**
2. **Verify WaveController:**
   - Should automatically start first wave (if `autoStartWaves = true` in WaveSpawner)
   - Or wait for player to exit base
3. **Check Defense Zones:**
   - Waves should spawn at current defense zone
   - Player should get fallback option if health low
4. **Check Base System:**
   - After wave ends, base gate should open
   - Walking into base should start timer (40 seconds)
   - Timer expiring should force next wave

**Expected Behavior:**
- Wave starts → Enemies spawn → Defeat enemies → Return to base → Shop/upgrade → Timer expires → Next wave

**Issues to Watch For:**
- Player transform not assigned in WaveSpawner (auto-finds by tag "Player")
- WaveController not linked to WaveSpawner
- Defense zones not properly assigned

---

### Task 4.2: Input System Verification

**Why:** Ensure player can interact with NPCs

**Check:**

1. **Verify Input Settings:**
   - Project Settings → Player → Active Input Handling = **Input System Package (New)**
   - ✅ Already confirmed in PROJECT_CONTEXT

2. **Check Input Actions:**
   - `/Assets/InputSystem_Actions.inputactions` should have:
     - Movement (WASD / Left Stick)
     - Jump (Space / A Button)
     - Attack (Mouse Left / Right Trigger)
     - Interact (E / B Button) ← **Verify this exists for NPC interaction**

3. **If Interact missing:**
   - Open `InputSystem_Actions.inputactions`
   - Add new action: `Interact`
   - Binding: Keyboard **E**, Gamepad **B Button**
   - Add to Player Input Component

**Result:** Player can interact with NPCs using E key

---

### Task 4.3: Verify Enemy Prefab Configuration

**Location:** `/Assets/Prefabs/Enemies/Enemy.prefab`

**Check:**
- ✅ Has `EnemyHealth` component
- ✅ `currencyReward = 10`
- Enemy drops 10 currency on death

**No action needed** - already configured

---

### Task 4.4: Test Full Gameplay Loop

**Steps:**

1. **Start Play Mode**
2. **Press C** to add test currency (100)
3. **Walk to Base** (should trigger BaseTrigger)
4. **Walk near StatVendor** (Trainer NPC)
5. **Press E** to interact (if prompts set up) or just get close
6. **Shop should open** with upgrade list
7. **Click upgrade button** to purchase
8. **Press L** to verify stat increased
9. **Close shop** and test combat
10. **Defeat enemies** to earn currency naturally
11. **Repeat** upgrade/weapon purchase cycle

**Success Criteria:**
- ✅ Shop opens when near NPC
- ✅ Currency displays correctly
- ✅ Can purchase upgrades/weapons
- ✅ Stats increase after purchase
- ✅ Currency deducted correctly
- ✅ Shop shows "MAX" for maxed upgrades

---

## 📋 Priority 5: Optional Enhancements

These are **not required** but improve the experience.

### Enhancement 5.1: Visual Indicators for NPCs

**Ideas:**
- Floating icon above NPC head
- Glowing effect when in range
- Animated merchant stand/shop visual
- Different colored cubes for each vendor type

---

### Enhancement 5.2: Audio Feedback

**Ideas:**
- Purchase sound effect
- Shop open/close sound
- Currency pickup sound
- Upgrade confirmation sound

---

### Enhancement 5.3: Particle Effects

**Ideas:**
- Sparkles when purchasing upgrade
- Weapon glow when equipped
- Currency pickup particle
- Shop opening particle burst

---

### Enhancement 5.4: More Weapons and Upgrades

**Ideas:**
- Create more WeaponData assets with different effects
- Create consumable items (health potions, buffs)
- Add weapon rarities (common, rare, legendary)
- Add set bonuses for multiple upgrades

---

## 🎯 Quick Start Checklist

If you want to test the shop **RIGHT NOW**, do these minimum steps:

- [ ] **1.** Create ItemButton prefab (Task 1.2)
- [ ] **2.** Create ScrollView in ShopPanel (Task 1.3)
- [ ] **3.** Add UI elements to ShopPanel (Task 1.4)
- [ ] **4.** Add SimpleShopUI to ShopPanel (Task 1.1)
- [ ] **5.** Assign all SimpleShopUI references (Task 1.5)
- [ ] **6.** Assign ShopUI in both NPCs (Task 1.6)
- [ ] **7.** Add DebugShopTester to GameManagers (Task 3.1)
- [ ] **8.** Enter Play Mode and press C, then walk to NPC

**Estimated Time:** 15-20 minutes

---

## 🚀 Full Completion Checklist

For a complete, polished experience:

### Shop System:
- [ ] Task 1.1 - Add SimpleShopUI component
- [ ] Task 1.2 - Create ItemButton prefab
- [ ] Task 1.3 - Create ScrollView container
- [ ] Task 1.4 - Add UI elements to ShopPanel
- [ ] Task 1.5 - Assign SimpleShopUI references
- [ ] Task 1.6 - Assign ShopUI in NPCs

### HUD System:
- [ ] Task 2.1 - Add GameUI component (optional)
- [ ] Task 2.2 - Add CurrencyDisplay script and component
- [ ] Task 2.3 - Add TimerDisplay script and component
- [ ] Task 2.4 - Create NPC interaction prompts (optional)

### Testing:
- [ ] Task 3.1 - Add DebugShopTester
- [ ] Task 3.2 - Verify all managers
- [ ] Task 4.1 - Test wave system
- [ ] Task 4.2 - Verify input system
- [ ] Task 4.3 - Verify enemy configuration
- [ ] Task 4.4 - Test full gameplay loop

### Polish (Optional):
- [ ] Enhancement 5.1 - Visual NPC indicators
- [ ] Enhancement 5.2 - Audio feedback
- [ ] Enhancement 5.3 - Particle effects
- [ ] Enhancement 5.4 - More content

**Estimated Time:** 1-2 hours for full completion

---

## 📚 Reference Documents

- **`PROJECT_CONTEXT.md`** - Overall project architecture
- **`NPC_SHOP_SETUP.md`** - Original shop system design
- **`QUICK_TEST_CHECKLIST.md`** - Quick testing guide
- **`SCRIPT_COMPILATION_CHECKLIST.md`** - Troubleshooting script errors
- **`FIX_APPLIED_GetCurrentCurrency.md`** - Currency API reference

---

## ✅ Summary

**CURRENT STATE:**
- All core systems implemented and working
- All ScriptableObject assets created
- Shop scripts written and compiled
- NPCs placed and configured with items

**BLOCKING ISSUES:**
- ❌ None! All compilation errors fixed

**NEXT STEP:**
- ✅ Follow Priority 1 tasks to set up shop UI
- ✅ Add DebugShopTester for easy testing
- ✅ Enter Play Mode and test!

**You're very close to having a fully playable prototype!** 🎮

---

**Last Updated:** After compilation error fixes  
**Status:** Ready to proceed with UI setup
