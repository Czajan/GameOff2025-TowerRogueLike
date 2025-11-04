# NPC Shop System - Setup Guide

## Overview

The NPC shop system replaces the traditional UI-based shop with interactive NPCs in your base. Each NPC specializes in selling specific types of items:

- **Weapon Vendor** - Sells weapons
- **Stat Upgrade Vendor** - Sells player stat upgrades
- **Consumable Vendor** - For future consumables (potions, buffs, etc.)
- **Special Vendor** - For unique items (future expansion)

## How It Works

### Player Interaction Flow
```
1. Player enters base area
2. Player approaches NPC (within 3 units)
   └─→ Interaction prompt appears: "[E] Talk to [NPC Name]"
   └─→ NPC highlights (yellow glow)
3. Player presses E
   └─→ Shop UI opens (specific to that NPC type)
   └─→ Time pauses (timeScale = 0)
4. Player purchases items via UI
5. Player presses E again or ESC to close
   └─→ Shop UI closes
   └─→ Time resumes
6. Player walks away from NPC
   └─→ Prompt disappears
```

### Key Features
✅ **Proximity Detection** - Auto-detects when player is near  
✅ **Visual Feedback** - NPCs glow when interactable  
✅ **Interaction Prompts** - World-space UI above NPCs  
✅ **Type Safety** - NPCs only sell their designated items  
✅ **Pause on Interact** - Game pauses when shopping  
✅ **Easy to Extend** - Add new NPC types easily  

---

## Scene Setup

### Step 1: Create NPC GameObjects

In your base area, create NPCs:

```
Base
├── Base_Ground
├── Base_Gate
├── Base_Trigger
└── NPCs (Empty GameObject - organizer)
    ├── NPC_WeaponVendor
    │   ├─→ Model (Cube or 3D model)
    │   ├─→ ShopNPC component
    │   └─→ InteractionPrompt (World Space Canvas)
    │
    └── NPC_StatVendor
        ├─→ Model (Cube or 3D model)
        ├─→ ShopNPC component
        └─→ InteractionPrompt (World Space Canvas)
```

### Step 2: Configure Weapon Vendor NPC

1. **Create GameObject:**
   - Position: Inside your base (e.g., `(5, 0, 0)`)
   - Add a Cube or 3D model for the NPC visual
   - Scale: `(1, 2, 1)` for humanoid size

2. **Add ShopNPC Component:**
   ```
   Inspector:
   ┌─────────────────────────────────────┐
   │ ShopNPC                             │
   ├─────────────────────────────────────┤
   │ NPC Configuration:                  │
   │   NPC Type: Weapon Vendor           │
   │   NPC Name: "Blacksmith"            │
   │   Interaction Range: 3              │
   │                                     │
   │ Weapon Vendor:                      │
   │   Available Weapons: Array[4]       │
   │     Element 0: BasicSword           │
   │     Element 1: FireBlade            │
   │     Element 2: IceSword             │
   │     Element 3: StormHammer          │
   │                                     │
   │ UI References:                      │
   │   Interaction Prompt: (Canvas)      │
   │   Shop UI: (Panel - see Step 4)     │
   │                                     │
   │ Visual Feedback:                    │
   │   Highlight Color: Yellow           │
   └─────────────────────────────────────┘
   ```

### Step 3: Configure Stat Upgrade Vendor NPC

1. **Create GameObject:**
   - Position: Inside your base (e.g., `(-5, 0, 0)`)
   - Add a Cube or 3D model
   - Scale: `(1, 2, 1)`

2. **Add ShopNPC Component:**
   ```
   Inspector:
   ┌─────────────────────────────────────┐
   │ ShopNPC                             │
   ├─────────────────────────────────────┤
   │ NPC Configuration:                  │
   │   NPC Type: Stat Upgrade Vendor     │
   │   NPC Name: "Trainer"               │
   │   Interaction Range: 3              │
   │                                     │
   │ Stat Upgrade Vendor:                │
   │   Available Upgrades: Array[6]      │
   │     Element 0: Upgrade_MoveSpeed    │
   │     Element 1: Upgrade_MaxHealth    │
   │     Element 2: Upgrade_Damage       │
   │     Element 3: Upgrade_CritChance   │
   │     Element 4: Upgrade_CritDamage   │
   │     Element 5: Upgrade_AttackRange  │
   │                                     │
   │ UI References:                      │
   │   Interaction Prompt: (Canvas)      │
   │   Shop UI: (Panel - see Step 4)     │
   └─────────────────────────────────────┘
   ```

---

## Creating Interaction Prompts

### World Space Canvas Setup

For each NPC, create a floating prompt:

1. **Create Canvas:**
   - Right-click NPC → UI → Canvas
   - Name: `InteractionPrompt`

2. **Configure Canvas:**
   ```
   Canvas Component:
   ├─ Render Mode: World Space
   ├─ Position: (0, 2.5, 0)
   ├─ Width: 200
   ├─ Height: 50
   ├─ Scale: (0.01, 0.01, 0.01)
   └─ Sorting Layer: Default (Order: 10)
   ```

3. **Add Text:**
   - Add child: TextMeshPro Text
   - Text: "[E] Talk to Blacksmith"
   - Font Size: 24
   - Alignment: Center
   - Auto Size: Enabled

4. **Add Script:**
   - Add `NPCInteractionPrompt` component to Canvas
   - It will auto-configure and face camera

---

## Creating Shop UI Panels

You can create shop UIs in two ways:

### Option 1: Simple Testing (No UI - Console Only)

Leave `Shop UI` field **empty** in ShopNPC component:
- NPC interaction still works
- Purchases logged to Console
- Perfect for testing without UI

### Option 2: Full UI Implementation

Create a UI panel for each NPC type:

#### Weapon Shop UI Example

```
Canvas (Screen Space)
└── WeaponShopPanel
    ├── Background (Image)
    ├── Title (Text: "Blacksmith's Weapons")
    ├── CurrencyDisplay (Text: "Gold: 150")
    ├── WeaponsList (Vertical Layout Group)
    │   ├── WeaponButton1 (Button)
    │   │   ├─→ Name: "Fire Blade"
    │   │   ├─→ Cost: "100 Gold"
    │   │   └─→ OnClick: Call ShopNPC.TryPurchaseWeapon(FireBlade)
    │   ├── WeaponButton2 (Button)
    │   └── WeaponButton3 (Button)
    └── CloseButton (Button)
        └─→ OnClick: Call ShopNPC.CloseShop()
```

#### Stat Upgrade Shop UI Example

```
Canvas (Screen Space)
└── StatShopPanel
    ├── Background (Image)
    ├── Title (Text: "Trainer's Upgrades")
    ├── CurrencyDisplay (Text: "Gold: 150")
    ├── UpgradesList (Vertical Layout Group)
    │   ├── UpgradeButton1 (Button)
    │   │   ├─→ Name: "Move Speed +1"
    │   │   ├─→ Cost: "50 Gold"
    │   │   └─→ OnClick: Call ShopNPC.TryPurchaseUpgrade(Upgrade_MoveSpeed, currentLevel)
    │   ├── UpgradeButton2 (Button)
    │   └── ...
    └── CloseButton (Button)
```

**Tip:** You can create a dynamic UI that reads `GetAvailableWeapons()` or `GetAvailableUpgrades()` to auto-populate buttons!

---

## Testing Without UI

### Quick Test Setup (5 Minutes)

1. **Create 2 Cubes in your base:**
   ```
   NPC_WeaponVendor (Cube at 5, 0, 0)
   NPC_StatVendor (Cube at -5, 0, 0)
   ```

2. **Add ShopNPC to each:**
   - Set NPC Type
   - Set NPC Name
   - Assign weapon/upgrade arrays

3. **Play the game:**
   - Enter base
   - Approach cube
   - Watch Console: "Near [NPC Name]. Press E to interact."
   - Press E
   - Watch Console: "Opened [NPC]'s shop"
   - Press E again to close

4. **Test purchases via code/Inspector:**
   - While shop is open, manually call `TryPurchaseWeapon()` in Inspector
   - Or create temp buttons that call the methods

---

## Code Integration

### Accessing NPC Data from UI Scripts

```csharp
public class WeaponShopUI : MonoBehaviour
{
    private ShopNPC currentNPC;
    
    public void Initialize(ShopNPC npc)
    {
        currentNPC = npc;
        
        // Get all weapons this NPC sells
        WeaponData[] weapons = npc.GetAvailableWeapons();
        
        // Create UI buttons for each weapon
        foreach (WeaponData weapon in weapons)
        {
            CreateWeaponButton(weapon);
        }
    }
    
    private void CreateWeaponButton(WeaponData weapon)
    {
        // Create button, set text, wire onClick
    }
    
    public void OnWeaponButtonClicked(WeaponData weapon)
    {
        if (currentNPC.TryPurchaseWeapon(weapon))
        {
            // Success - play sound, show VFX
        }
        else
        {
            // Failed - show "not enough gold" message
        }
    }
}
```

### Listening to Purchase Events

```csharp
private void Start()
{
    ShopNPC npc = GetComponent<ShopNPC>();
    
    npc.OnWeaponPurchased.AddListener(OnWeaponBought);
    npc.OnUpgradePurchased.AddListener(OnUpgradeBought);
    npc.OnPlayerEnterRange.AddListener(OnPlayerNear);
}

private void OnWeaponBought(WeaponData weapon)
{
    Debug.Log($"Weapon purchased: {weapon.weaponName}");
    // Play purchase sound/VFX
}
```

---

## Comparison: Old vs New System

### Old System (UpgradeShop)
```
❌ Single shop for everything
❌ Opens automatically on base entry
❌ UI-only interaction
❌ Less immersive
✅ Simple to implement
```

### New System (ShopNPC)
```
✅ Multiple specialized vendors
✅ Player chooses when to shop
✅ World interaction (press E)
✅ More immersive and RPG-like
✅ Easy to add new vendor types
✅ NPCs can be positioned anywhere
✅ Visual feedback (highlights, prompts)
```

---

## Adding More NPC Types (Future)

To add a new NPC type (e.g., Consumable Vendor):

1. **Add to enum in ShopNPC.cs:**
   ```csharp
   public enum ShopNPCType
   {
       WeaponVendor,
       StatUpgradeVendor,
       ConsumableVendor,  // NEW
       SpecialVendor
   }
   ```

2. **Add new fields:**
   ```csharp
   [Header("Consumable Vendor")]
   [SerializeField] private ConsumableData[] availableConsumables;
   ```

3. **Add purchase method:**
   ```csharp
   public bool TryPurchaseConsumable(ConsumableData consumable)
   {
       // Similar to TryPurchaseWeapon
   }
   ```

4. **Create NPC in scene** with new type

---

## Tips & Best Practices

### Visual Design
- Make each NPC visually distinct (different colors/models)
- Use icons above NPCs (sword for weapons, scroll for upgrades)
- Add ambient animations (idle, wave)

### Interaction
- Keep interaction range at 3 units (not too close, not too far)
- Always show currency in shop UI
- Disable player movement while shopping (timeScale = 0)

### Performance
- NPCs only check distance in Update (very cheap)
- Shop UI only active when needed
- No raycasts or physics needed

### UX Polish
- Add sound when approaching NPC
- Play different sound for successful/failed purchase
- Show floating damage numbers for stat increases
- Highlight affordable items in green, expensive in red

---

## Migration from Old UpgradeShop

If you want to keep both systems:

1. **Keep UpgradeShop** for traditional UI shops
2. **Add ShopNPCs** for base vendors
3. They use the same backend (PlayerStats, WeaponSystem)

If you want to replace it completely:

1. **Remove UpgradeShop** component from GameManagers
2. **Add ShopNPCs** to your base
3. Update any references in your code

**Both systems work with the same data!** UpgradeData and WeaponData ScriptableObjects work with either approach.

---

## Quick Reference

### Key Controls
- **E** - Interact with NPC (open/close shop)
- **ESC** - Close shop
- **Walk Away** - Auto-closes shop and removes prompt

### NPC Types
- `WeaponVendor` - Sells weapons
- `StatUpgradeVendor` - Sells stat upgrades
- `ConsumableVendor` - Future use
- `SpecialVendor` - Future use

### Gizmo
- **Cyan wire sphere** in Scene view shows interaction range

---

**Your NPC shop system is ready!** Create NPCs, assign them types, and players can interact naturally. No UI required for testing! 🎮
