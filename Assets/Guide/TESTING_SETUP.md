# Testing Setup Guide - Simple Shop UI

This guide will help you set up everything needed to test your game with the simple shop UI system.

## ✅ What You've Already Done

- Created all 6 Upgrade ScriptableObjects
- Created all 5 Weapon ScriptableObjects
- Enemy currency reward is set to 10

## 🎯 Next Steps - Scene Setup

### Step 1: Create Shop UI in GameCanvas

1. **Select** `/GameCanvas` in Hierarchy
2. **Right-click** GameCanvas → **UI** → **Panel**
   - Rename to `ShopPanel`
   - Set Anchor to **stretch** (full screen)
   - Add **SimpleShopUI** component

3. **Inside ShopPanel**, create the UI structure:

#### **A. Shop Header**
- Right-click ShopPanel → **UI** → **Panel** → Rename to `HeaderPanel`
  - Add **Horizontal Layout Group** component
  - Padding: Left 10, Right 10, Top 10
  - Child Alignment: Middle Left
  - Add **TextMeshPro - Text** child → Name: `ShopTitle`
    - Text: "Shop"
    - Font Size: 32
    - Alignment: Center
  - Add **TextMeshPro - Text** child → Name: `CurrencyText`
    - Text: "Currency: $0"
    - Font Size: 24
    - Alignment: Right

#### **B. Item List Area**
- Right-click ShopPanel → **UI** → **Scroll View** → Rename to `ItemScrollView`
  - Position below HeaderPanel
  - Inside Scroll View, find **Content** object:
    - Add **Vertical Layout Group** component
    - Child Force Expand: Width = true, Height = false
    - Spacing: 10
    - Padding: 10 all sides

#### **C. Close Button**
- Right-click ShopPanel → **UI** → **Button - TextMeshPro** → Rename to `CloseButton`
  - Position at bottom or top-right corner
  - Button text: "Close (ESC)"

#### **D. Item Button Prefab**
- In Project, create `/Assets/Prefabs/UI` folder
- Right-click in Hierarchy → **UI** → **Button - TextMeshPro** → Rename to `ShopItemButton`
  - Add these children to ShopItemButton:
    - **TextMeshPro - Text** → Name: `Name` (item name)
    - **TextMeshPro - Text** → Name: `Description` (item description)
    - **TextMeshPro - Text** → Name: `Cost` (item cost)
  - **Drag ShopItemButton** from Hierarchy to `/Assets/Prefabs/UI` to create prefab
  - **Delete** the ShopItemButton from Hierarchy

### Step 2: Configure ShopPanel SimpleShopUI Component

1. **Select** `ShopPanel` in Hierarchy
2. In **SimpleShopUI** component Inspector:
   - **Item List Container:** Drag `ItemScrollView/Viewport/Content`
   - **Item Button Prefab:** Drag the `ShopItemButton` prefab from Project
   - **Shop Title Text:** Drag `HeaderPanel/ShopTitle`
   - **Currency Text:** Drag `HeaderPanel/CurrencyText`
   - **Close Button:** Drag `CloseButton`

3. **Disable ShopPanel** (uncheck active in Inspector)

### Step 3: Create NPC Interaction Prompts

For **each NPC** (WeaponVendor, StatVendor):

1. **Select** the NPC in Hierarchy (e.g., `/Base/NPCs/WeaponVendor`)
2. **Right-click** NPC → **UI** → **Canvas** → Rename to `InteractionCanvas`
   - Set **Render Mode** to **World Space**
   - Set **Width**: 200, **Height**: 100
   - Set **Position Y**: 2.5 (above NPC)
   - Set **Scale**: 0.01, 0.01, 0.01 (all axes)

3. **Add child** to InteractionCanvas → **UI** → **Panel** → Rename to `PromptPanel`
4. **Add child** to PromptPanel → **TextMeshPro - Text** → Rename to `PromptText`
   - Text: "[E] Talk to Merchant"
   - Font Size: 24
   - Alignment: Center
   - Auto Size: Enable

5. **Add** **NPCInteractionPrompt** component to **InteractionCanvas**
   - **Prompt Text:** Drag the `PromptText` component
   - **Interaction Key:** "E"
   - **Face Camera:** true
   - **Offset:** (0, 2.5, 0)

6. **Disable InteractionCanvas** (uncheck active)

### Step 4: Configure NPC Components

#### **WeaponVendor Setup**
1. Select `/Base/NPCs/WeaponVendor`
2. In **ShopNPC** component:
   - **NPC Type:** WeaponVendor
   - **NPC Name:** "Blacksmith"
   - **Interaction Range:** 3
   - **Available Weapons** (size: 5):
     - Drag all 5 weapon assets from `/Assets/Data/Weapons`
   - **Interaction Prompt:** Drag the `InteractionCanvas` GameObject
   - **Shop UI:** Drag the `ShopPanel` GameObject from GameCanvas
   - **Highlight Color:** Yellow

#### **StatVendor Setup**
1. Select `/Base/NPCs/StatVendor`
2. In **ShopNPC** component:
   - **NPC Type:** StatUpgradeVendor
   - **NPC Name:** "Trainer"
   - **Interaction Range:** 3
   - **Available Upgrades** (size: 6):
     - Drag all 6 upgrade assets from `/Assets/Data/Upgrades`
   - **Interaction Prompt:** Drag its own `InteractionCanvas` GameObject
   - **Shop UI:** Drag the `ShopPanel` GameObject from GameCanvas
   - **Highlight Color:** Yellow

### Step 5: Configure WaveController Defense Zones

1. Select `/WaveController` in Hierarchy
2. In **WaveController** component:
   - **Wave Spawner:** Drag `/WaveSpawner`
   - **Defense Zones** (size: 3):
     - Element 0: Drag `/DefenseZones/DefenseZone1`
     - Element 1: Drag `/DefenseZones/DefenseZone2`
     - Element 2: Drag `/DefenseZones/DefenseZone3`
   - **Wait For Base Exit:** true (checked)

### Step 6: Optional - Set Starting Weapon

1. Select `/GameManagers` in Hierarchy
2. In **WeaponSystem** component:
   - **Equipped Weapon:** Drag `BasicSword` asset (optional)

### Step 7: Update GameCanvas with Currency Display

1. Select `/GameCanvas` in Hierarchy
2. Add **TextMeshPro - Text** child → Name: `CurrencyDisplay`
   - Text: "Currency: $0"
   - Position: Top-left or top-right corner
   - Font Size: 24

3. Create new script or update existing to show currency always:

```csharp
// Add to GameUI.cs or create new CurrencyDisplay.cs
private void Start()
{
    if (GameProgressionManager.Instance != null)
    {
        GameProgressionManager.Instance.OnCurrencyChanged.AddListener(UpdateCurrency);
        UpdateCurrency(GameProgressionManager.Instance.Currency);
    }
}

private void UpdateCurrency(int currency)
{
    currencyDisplayText.text = $"Currency: ${currency}";
}
```

## 🎮 Testing Instructions

### Initial Test
1. **Enter Play Mode**
2. **Check Console** for any errors
3. **Move Player** (WASD) to verify controls work
4. **Jump** (Space) to test jump
5. **Attack** (Left Mouse) to test combat

### Shop System Test
1. **Kill some enemies** to earn currency (10 per enemy)
2. **Walk to Base area** (should see "Entered base" in console)
3. **Approach StatVendor or WeaponVendor** NPC
   - NPC should **highlight yellow**
   - Interaction prompt should appear: `[E] Talk to Trainer`
4. **Press E** to open shop
   - Shop UI should appear
   - Game should pause (Time.timeScale = 0)
   - See list of upgrades or weapons
5. **Click on an item** to purchase
   - If enough currency: Purchase succeeds, stats/weapon updated
   - If not enough: "Not enough currency" message
6. **Press ESC or Close Button** to close shop
   - Game should resume

### Currency Flow Test
1. **Start with 0 currency**
2. **Kill 5 enemies** → Should have 50 currency
3. **Buy MoveSpeed upgrade** (costs 50) → Currency should drop to 0
4. **Kill 5 more enemies** → Should have 50 currency
5. **Buy MoveSpeed again** (costs 75 now) → "Not enough currency"
6. **Kill 3 more enemies** → Should have 80 currency
7. **Buy MoveSpeed** (costs 75) → Success, 5 currency remaining

### Upgrade Scaling Test
Each upgrade increases in cost:
- Level 0 → 1: Base cost
- Level 1 → 2: Base cost × 1.5
- Level 2 → 3: Base cost × 1.5²
- etc.

Example for MoveSpeed (base: 50):
- Level 1: 50
- Level 2: 75
- Level 3: 112
- Level 4: 168

## ⚠️ Common Issues & Solutions

### Issue: "NPC not highlighting when I approach"
**Solution:** 
- Check NPC has **ShopNPC** component
- Verify **Interaction Range** is set (default: 3)
- Make sure Player has tag "Player"

### Issue: "Shop UI doesn't appear when I press E"
**Solution:**
- Check **Shop UI** field in ShopNPC is assigned to ShopPanel
- Verify ShopPanel has **SimpleShopUI** component
- Make sure all UI references in SimpleShopUI are assigned

### Issue: "Can't purchase anything"
**Solution:**
- Check you have enough currency (kill enemies first)
- Verify GameProgressionManager exists in scene
- Check upgrade isn't at max level

### Issue: "Currency doesn't increase when killing enemies"
**Solution:**
- Verify Enemy prefab has `currencyReward = 10` in EnemyHealth
- Check GameProgressionManager is in scene
- Look for errors in Console

### Issue: "Game doesn't pause when shop is open"
**Solution:**
- This is expected behavior in ShopNPC.OpenShop()
- If not working, check ShopNPC.OpenShop() is being called

## 📊 Quick Reference

### Starting Currency: 0
### Enemy Drops: 10 currency each
### Upgrade Costs:
- MoveSpeed: 50 base
- MaxHealth: 75 base
- Damage: 100 base
- CritChance: 150 base
- CritDamage: 150 base
- AttackRange: 100 base

### Weapon Costs:
- BasicSword: 0 (free)
- FireBlade: 200
- IceBlade: 250
- LightningStaff: 300
- CriticalDagger: 350

### Controls:
- **WASD:** Move
- **Space:** Jump
- **Left Mouse:** Attack
- **Shift + Move:** Sprint
- **E:** Interact with NPC
- **ESC:** Close shop

## 🚀 After Testing Works

Once basic testing is successful:
1. Add visual feedback (particles, sounds)
2. Create proper UI design
3. Add weapon effects visuals
4. Implement status effect indicators
5. Add animations for NPCs
6. Create more weapon/upgrade varieties

---

**Good luck with testing! If you encounter issues, check the Console for error messages.**
