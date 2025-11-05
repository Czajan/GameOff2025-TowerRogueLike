# Shop System Setup Guide

## Overview

Your shop system consists of:
- **2 NPCs:** WeaponVendor (Blacksmith) and StatVendor (Trainer)
- **Shop UI:** Shared panel that displays different items based on NPC type
- **Data Assets:** WeaponData and UpgradeData ScriptableObjects

---

## Current Configuration ✅

### NPCs Configured

**WeaponVendor (Blacksmith):**
- Location: `/Base/NPCs/WeaponVendor`
- NPC Type: `WeaponVendor`
- Interaction Range: 3 units
- Available Weapons (4):
  - Critical Dagger (`/Assets/Data/Weapons/CriticalDagger.asset`)
  - Fire Blade (`/Assets/Data/Weapons/FireBlade.asset`)
  - Ice Blade (`/Assets/Data/Weapons/IceBlade.asset`)
  - Lightning Staff (`/Assets/Data/Weapons/LightningStaff.asset`)

**StatVendor (Trainer):**
- Location: `/Base/NPCs/StatVendor`
- NPC Type: `StatUpgradeVendor`
- Interaction Range: 3 units
- Available Upgrades (6):
  - Attack Range (`/Assets/Data/Upgrades/AttackRangeUpgrade.asset`)
  - Crit Chance (`/Assets/Data/Upgrades/CritChanceUpgrade.asset`)
  - Crit Damage (`/Assets/Data/Upgrades/CritDamageUpgrade.asset`)
  - Damage (`/Assets/Data/Upgrades/DamageUpgrade.asset`)
  - Max Health (`/Assets/Data/Upgrades/MaxHealthUpgrade.asset`)
  - Move Speed (`/Assets/Data/Upgrades/MoveSpeedUpgrade.asset`)

### Shop UI Configured

**ShopPanel:**
- Location: `/GameCanvas/ShopPanel`
- Item Container: `/GameCanvas/ShopPanel/ItemScrollView/Viewport/Content`
- Item Button Prefab: `/Assets/Prefabs/UI/ItemButton.prefab`
- Layout: VerticalLayoutGroup with 10px spacing
- Content Size Fitter: Vertical = PreferredSize

---

## Issues Fixed ✅

### Issue 1: Shop Empty on First Open (FIXED)

**Problem:**
- Shop panel was activated AFTER populating items
- UI layout system needs to be active to calculate sizes properly

**Solution:**
```csharp
public void OpenShop(ShopNPC npc)
{
    gameObject.SetActive(true);  // ✅ Activate FIRST
    PopulateShop();              // Then populate
    LayoutRebuilder.ForceRebuildLayoutImmediate(...); // Force layout
    Canvas.ForceUpdateCanvases(); // Update canvas
}
```

### Issue 2: Layout Not Updating (FIXED)

**Added:**
- `LayoutRebuilder.ForceRebuildLayoutImmediate()` after populating
- `Canvas.ForceUpdateCanvases()` to force immediate update
- Debug logging to track item creation

---

## How Items Are Added

### Weapons

Weapons are added by assigning `WeaponData` ScriptableObjects to the NPC:

1. **Create WeaponData asset:**
   - Right-click in Project > Create > Game Data > Weapon Data
   - Configure properties:
     - `weaponName`: Display name (e.g., "Blazing Edge")
     - `description`: What it does
     - `damageMultiplier`: Damage boost (1.0 = normal, 1.2 = +20%)
     - `purchaseCost`: How much it costs
     - `weaponEffect`: Effect type (e.g., "Burn", "Freeze")
     - `effectChance`: Chance to apply effect (0-1)

2. **Assign to WeaponVendor:**
   - Select `/Base/NPCs/WeaponVendor`
   - In Inspector > ShopNPC component
   - Expand `Available Weapons` array
   - Add your WeaponData asset

**Example WeaponData:**
```
Weapon Name: Blazing Edge
Description: Burns enemies with each strike
Damage Multiplier: 1.2
Purchase Cost: 200
Weapon Effect: Burn
Effect Chance: 0.5 (50%)
Effect Value: 5 (5 damage per tick)
```

### Upgrades

Upgrades are added by assigning `UpgradeData` ScriptableObjects:

1. **Create UpgradeData asset:**
   - Right-click in Project > Create > Game Data > Upgrade Data
   - Configure properties:
     - `upgradeName`: Display name (e.g., "Swift Steps")
     - `description`: What it does
     - `upgradeType`: Which stat (MoveSpeed, MaxHealth, Damage, etc.)
     - `baseCost`: Starting cost
     - `costIncreasePerLevel`: Multiplier per level (e.g., 1.5 = +50% each level)
     - `maxLevel`: Maximum upgrade level

2. **Assign to StatVendor:**
   - Select `/Base/NPCs/StatVendor`
   - In Inspector > ShopNPC component
   - Expand `Available Upgrades` array
   - Add your UpgradeData asset

**Example UpgradeData:**
```
Upgrade Name: Swift Steps
Description: Increases your movement speed
Upgrade Type: MoveSpeed
Base Cost: 50
Cost Increase Per Level: 1.5 (50 → 75 → 112 → ...)
Max Level: 10
```

---

## Adding New Items

### To Add a New Weapon:

1. **Create the asset:**
   ```
   Right-click in /Assets/Data/Weapons
   Create > Game Data > Weapon Data
   Name it descriptively (e.g., "PoisonDagger.asset")
   ```

2. **Configure the weapon:**
   ```
   Weapon Name: Venomous Fang
   Description: Poisons enemies over time
   Damage Multiplier: 1.1
   Attack Speed Multiplier: 1.3
   Purchase Cost: 150
   Weapon Effect: Poison
   Effect Chance: 0.8
   Effect Value: 3
   ```

3. **Add to WeaponVendor:**
   ```
   Select /Base/NPCs/WeaponVendor
   Inspector > ShopNPC > Available Weapons
   Increase Size by 1
   Drag PoisonDagger.asset into the new slot
   ```

4. **Test:**
   - Enter Play Mode
   - Walk to Blacksmith
   - Press E to open shop
   - You should see "Venomous Fang - $150"

### To Add a New Upgrade:

1. **Create the asset:**
   ```
   Right-click in /Assets/Data/Upgrades
   Create > Game Data > Upgrade Data
   Name it descriptively (e.g., "AttackSpeedUpgrade.asset")
   ```

2. **Configure the upgrade:**
   ```
   Upgrade Name: Swift Strike
   Description: Increases attack speed
   Upgrade Type: (Choose from dropdown)
   Base Cost: 75
   Cost Increase Per Level: 1.4
   Max Level: 5
   ```

3. **Add to StatVendor:**
   ```
   Select /Base/NPCs/StatVendor
   Inspector > ShopNPC > Available Upgrades
   Increase Size by 1
   Drag AttackSpeedUpgrade.asset into the new slot
   ```

4. **Test:**
   - Enter Play Mode
   - Walk to Trainer
   - Press E to open shop
   - You should see "Swift Strike (Lv 0) - $75"

---

## Testing the Shop

### How to Test:

1. **Start Play Mode**
2. **Move player near an NPC** (within 3 units)
3. **Look for interaction prompt** (should appear)
4. **Press E** to open shop
5. **Check Console** for debug logs:
   ```
   "Populating Stat Upgrades Shop"
   "Populating 6 upgrades"
   "Created upgrade button: Swift Steps Lv0 - $50"
   ...
   ```

### What You Should See:

**WeaponVendor Shop:**
```
=================================
       Blacksmith
       Currency: $0
=================================

[Button] Blazing Edge          $200
         Burns enemies with each strike

[Button] Frozen Edge           $180
         Slows enemies with ice

[Button] Critical Dagger       $150
         Increased critical chance

[Button] Lightning Staff       $250
         Chain lightning attacks

=================================
              [Close]
=================================
```

**StatVendor Shop:**
```
=================================
         Trainer
       Currency: $0
=================================

[Button] Swift Steps (Lv 0)    $50
         Increases movement speed

[Button] Iron Body (Lv 0)      $60
         Increases max health

[Button] Power Strike (Lv 0)   $80
         Increases damage

... (more upgrades)

=================================
              [Close]
=================================
```

---

## Troubleshooting

### Shop is still empty?

**Check Console logs:**
- Should see `"Populating X weapons"` or `"Populating X upgrades"`
- Should see `"Created weapon/upgrade button: ..."`

**If you see "Populating 0 items":**
- Select the NPC in Hierarchy
- Check Inspector > ShopNPC component
- Verify `Available Weapons` or `Available Upgrades` array has items
- Verify the assets are not null (should show asset names)

**If you see correct count but no buttons:**
- Check `/GameCanvas/ShopPanel/ItemScrollView/Viewport/Content`
- Verify it has VerticalLayoutGroup component
- Verify ItemButton prefab exists at `/Assets/Prefabs/UI/ItemButton.prefab`

### Items overlap or don't layout properly?

**Check Content settings:**
- VerticalLayoutGroup spacing: 10
- ContentSizeFitter: Vertical = PreferredSize
- Child Force Expand Height: Should be FALSE (allows items to use their own size)

**ItemButton prefab should have:**
- RectTransform with Height = 80
- LayoutElement component (optional, but recommended):
  - Preferred Height: 80
  - Min Height: 60

### Items show but text is missing?

**Check ItemButton prefab structure:**
```
ItemButton (root)
├── Name (TextMeshProUGUI)
├── Description (TextMeshProUGUI)
├── Cost (TextMeshProUGUI)
└── Icon (Image, optional)
```

Names must match exactly: `"Name"`, `"Description"`, `"Cost"`

---

## Debug Logs Reference

When opening shop, you should see these logs in Console:

```
"Populating Stat Upgrades Shop"  // Or "Populating Weapons Shop"
"Populating 6 upgrades"          // Number of items
"Created upgrade button: Swift Steps Lv0 - $50"
"Created upgrade button: Iron Body Lv0 - $60"
... (one per item)
```

If you DON'T see these logs:
1. Check that SimpleShopUI.cs compiled successfully (no errors in Console)
2. Check that ShopPanel has SimpleShopUI component
3. Check that NPC's `shopUI` field references `/GameCanvas/ShopPanel`

---

## Currency System

### How to Add Currency (For Testing):

**Option 1: Use DebugShopTester (in Play Mode):**
- Press **C** key to add 100 currency
- Press **H** key to see all debug controls

**Option 2: Via Code:**
```csharp
GameProgressionManager.Instance.AddCurrency(100);
```

**Option 3: Kill Enemies:**
- Enemies drop currency on death (configured in EnemyHealth)

### Purchasing Items:

**Weapons:**
- Cost is fixed (defined in WeaponData)
- Purchase unlocks the weapon permanently
- Only one weapon can be active at a time

**Upgrades:**
- Cost increases each level
- Formula: `baseCost * (costIncreasePerLevel ^ currentLevel)`
- Example: $50 → $75 → $112 → $168 → ...
- Can upgrade up to `maxLevel`

---

## Next Steps

### Recommended Additions:

1. **Add Icons:**
   - Add `icon` sprite to WeaponData and UpgradeData
   - Display in ItemButton prefab

2. **Add Sound Effects:**
   - Purchase success sound
   - Cannot afford sound
   - Shop open/close sounds

3. **Add Visual Feedback:**
   - Highlight owned weapons
   - Show stat changes on hover
   - Disable already-purchased weapons

4. **Add More Weapons/Upgrades:**
   - Create more variety
   - Add unique effects
   - Balance costs and power

5. **Improve Layout:**
   - Add weapon/upgrade categories
   - Add tooltips
   - Add confirmation dialogs for expensive purchases

---

## File Structure

```
/Assets
├── /Data
│   ├── /Weapons
│   │   ├── FireBlade.asset
│   │   ├── IceBlade.asset
│   │   ├── CriticalDagger.asset
│   │   └── LightningStaff.asset
│   └── /Upgrades
│       ├── MoveSpeedUpgrade.asset
│       ├── MaxHealthUpgrade.asset
│       ├── DamageUpgrade.asset
│       ├── CritChanceUpgrade.asset
│       ├── CritDamageUpgrade.asset
│       └── AttackRangeUpgrade.asset
├── /Prefabs
│   └── /UI
│       ├── ItemButton.prefab
│       └── NPCPrompt.prefab
└── /Scripts
    └── /Systems
        ├── ShopNPC.cs
        ├── SimpleShopUI.cs
        ├── WeaponData.cs
        └── UpgradeData.cs
```

---

## Summary

✅ **Shop is fully configured and working**  
✅ **4 weapons available from Blacksmith**  
✅ **6 upgrades available from Trainer**  
✅ **UI layout issue fixed**  
✅ **Debug logging added**  

**To test:** Enter Play Mode → Walk to NPC → Press E → Shop opens with items!

**To add items:** Create WeaponData/UpgradeData asset → Assign to NPC → Done!
