# Shop Systems Comparison

Your game now has **two shop system options**. You can use either one, or both!

---

## 🏪 Option 1: Traditional UI Shop (`UpgradeShop.cs`)

### How It Works
```
Player Enters Base
    ↓
Shop UI Panel Opens Automatically
    ↓
Player Clicks Buttons to Purchase
    ↓
Player Exits Base or Clicks "Start Wave"
    ↓
Shop Closes
```

### Pros
- ✅ Simple to implement
- ✅ All options visible at once
- ✅ Fast to navigate
- ✅ Good for menu-driven games

### Cons
- ❌ Less immersive
- ❌ Opens automatically (no player choice)
- ❌ UI-centric experience
- ❌ All upgrades in one place

### Best For
- Fast-paced arcade games
- Mobile games
- Menu-driven experiences
- Quick prototyping

---

## 🧙 Option 2: NPC Vendor Shop (`ShopNPC.cs`)

### How It Works
```
Player Enters Base
    ↓
Player Approaches NPC
    ↓
Prompt Appears: "[E] Talk to Blacksmith"
NPC Glows Yellow
    ↓
Player Presses E
    ↓
Shop UI Opens (Specific to NPC Type)
    ↓
Player Purchases
    ↓
Player Presses E or ESC to Close
    ↓
Player Can Visit Other NPCs
```

### Pros
- ✅ Highly immersive
- ✅ RPG-like interaction
- ✅ Vendors specialized by type
- ✅ Player chooses when to shop
- ✅ Visual feedback (highlights, prompts)
- ✅ Easy to expand with new vendor types
- ✅ NPCs can be placed anywhere in world

### Cons
- ❌ Requires more scene setup
- ❌ Player must walk to each vendor
- ❌ More complex to implement

### Best For
- RPG games
- Story-driven games
- Immersive experiences
- Games with multiple upgrade categories

---

## 📊 Feature Comparison Table

| Feature | UpgradeShop | ShopNPC |
|---------|------------|---------|
| **Interaction** | Automatic | Press E near NPC |
| **Vendor Types** | Single shop | Multiple specialized NPCs |
| **Immersion** | Low | High |
| **Setup Time** | 5 min | 15 min |
| **UI Complexity** | Single panel | Panel per NPC type |
| **Player Control** | Opens auto | Player chooses |
| **Visual Feedback** | None | Highlights, prompts |
| **Expandability** | Limited | High |
| **World Integration** | None | NPCs in scene |
| **Mobile Friendly** | Yes | No (requires keyboard) |

---

## 🎮 Recommended Usage by Game Type

### Fast Arcade Roguelike
**Use: `UpgradeShop`**
- Quick, menu-driven
- No walking around
- Fast decisions

### Story-Driven Roguelike
**Use: `ShopNPC`**
- Meet characters
- Explore base
- RPG experience

### Hybrid Approach
**Use: Both!**
- NPCs for major upgrades
- Quick menu for consumables
- Best of both worlds

---

## 🔧 Implementation Details

### UpgradeShop System

**Scene Structure:**
```
GameManagers
└── UpgradeShop
    ├─→ Available Upgrades: Array[6]
    ├─→ Available Weapons: Array[4]
    └─→ Shop UI: Panel reference

Canvas
└── ShopPanel (entire shop in one UI)
    ├── UpgradeButtons
    └── WeaponButtons
```

**Code Integration:**
```csharp
// Automatically opens when entering base
GameProgressionManager.OnEnteredBase → UpgradeShop.OpenShop()

// Purchase
UpgradeShop.TryPurchaseUpgrade(upgradeData, currentLevel);
UpgradeShop.TryPurchaseWeapon(weaponData);
```

---

### ShopNPC System

**Scene Structure:**
```
Base/NPCs
├── NPC_WeaponVendor
│   ├─→ ShopNPC component
│   │   ├─→ NPC Type: Weapon Vendor
│   │   ├─→ Available Weapons: Array[4]
│   │   └─→ Shop UI: WeaponPanel
│   ├─→ Model (Cube)
│   └─→ InteractionPrompt (Canvas)
│
└── NPC_StatVendor
    ├─→ ShopNPC component
    │   ├─→ NPC Type: Stat Upgrade Vendor
    │   ├─→ Available Upgrades: Array[6]
    │   └─→ Shop UI: StatPanel
    ├─→ Model (Cube)
    └─→ InteractionPrompt (Canvas)

Canvas
├── WeaponShopPanel (for weapon vendor)
└── StatShopPanel (for stat vendor)
```

**Code Integration:**
```csharp
// Player proximity auto-detected
ShopNPC checks distance in Update()

// Player presses E
ShopNPC.OpenShop() → Show specific UI

// Purchase
ShopNPC.TryPurchaseWeapon(weaponData);
ShopNPC.TryPurchaseUpgrade(upgradeData, currentLevel);
```

---

## 🛠️ How to Choose

### Choose UpgradeShop if:
- You want simple, fast implementation
- Your game is menu/UI focused
- You have limited upgrade types
- Players should see all options at once
- You're prototyping quickly

### Choose ShopNPC if:
- You want immersive world interaction
- Your game has RPG elements
- You have many upgrade categories
- You want vendor personalities/lore
- You want players to explore the base

### Use Both if:
- Different upgrade types need different UX
- You want quick access to some items
- But want special items from NPCs
- Example: Consumables in menu, weapons from NPCs

---

## 📝 Migration Guide

### From UpgradeShop → ShopNPC

1. **Create NPCs in base:**
   - Add cubes/models
   - Add `ShopNPC` components
   - Configure types

2. **Move upgrade/weapon arrays:**
   - From `UpgradeShop` component
   - To individual `ShopNPC` components

3. **Create UI panels per type:**
   - Weapon panel
   - Stat panel

4. **Optional: Remove `UpgradeShop`**
   - Or keep for other purposes

### From ShopNPC → UpgradeShop

1. **Add `UpgradeShop` to GameManagers**

2. **Gather all upgrades/weapons:**
   - Collect from all NPCs
   - Add to single arrays

3. **Create single shop panel**

4. **Wire to base events:**
   - OnEnteredBase → OpenShop

---

## 🎨 Visual Examples

### UpgradeShop Flow
```
┌─────────────────────────────────────────┐
│         🚪 ENTER BASE                   │
└─────────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────────┐
│     📋 SHOP MENU (Auto-Opens)          │
├─────────────────────────────────────────┤
│  Stat Upgrades:                         │
│  [Move Speed] 50g                       │
│  [Max Health] 50g                       │
│  [Damage] 50g                           │
│                                         │
│  Weapons:                               │
│  [Fire Blade] 100g                      │
│  [Ice Sword] 150g                       │
│                                         │
│  [Start Wave] [Close]                   │
└─────────────────────────────────────────┘
```

### ShopNPC Flow
```
┌─────────────────────────────────────────┐
│         🚪 ENTER BASE                   │
│                                         │
│    🧙 Blacksmith    👨‍🏫 Trainer         │
│   (Weapons)       (Stats)               │
└─────────────────────────────────────────┘
                  ↓
          Player walks to Blacksmith
                  ↓
┌─────────────────────────────────────────┐
│      💬 [E] Talk to Blacksmith          │
│      (NPC glows yellow)                 │
└─────────────────────────────────────────┘
                  ↓
              Press E
                  ↓
┌─────────────────────────────────────────┐
│     ⚔️ BLACKSMITH'S WEAPONS            │
├─────────────────────────────────────────┤
│  [Fire Blade] 100g                      │
│  [Ice Sword] 150g                       │
│  [Storm Hammer] 200g                    │
│                                         │
│  [Close]                                │
└─────────────────────────────────────────┘
                  ↓
          Walk to Trainer
                  ↓
┌─────────────────────────────────────────┐
│     📚 TRAINER'S UPGRADES               │
├─────────────────────────────────────────┤
│  [Move Speed] 50g                       │
│  [Max Health] 50g                       │
│  [Damage] 50g                           │
│                                         │
│  [Close]                                │
└─────────────────────────────────────────┘
```

---

## 🎯 Recommendation for Your Game

Based on your game being an **"Isometric Roguelike Defense"**:

### 🥇 **Primary Recommendation: ShopNPC**

**Reasons:**
1. ✅ Fits roguelike genre (exploration, NPCs, progression)
2. ✅ Base is a "safe zone" - perfect for vendor NPCs
3. ✅ Multiple upgrade types (stats vs weapons) benefit from specialization
4. ✅ Adds personality and lore potential
5. ✅ More engaging between-wave downtime
6. ✅ Future expansion: add more vendor types easily

**Setup Priority:**
- **NPC 1:** Weapon Vendor (Blacksmith)
- **NPC 2:** Stat Upgrade Vendor (Trainer)
- **Later:** Consumable Vendor, Special Items Vendor

### 🥈 **Fallback: UpgradeShop**

Use if:
- You need quick prototype testing
- Your art/3D pipeline isn't ready
- You want to validate upgrade balance first

**Then migrate to ShopNPC later!**

---

## 📚 Documentation

### UpgradeShop
- See: `/Assets/Guide/UPGRADE_SYSTEM_GUIDE.md`
- Section: "Creating Shop UI"

### ShopNPC
- See: `/Assets/Guide/NPC_SHOP_SETUP.md`
- Complete setup guide with examples

---

## 🔑 Key Takeaway

**Both systems use the same backend:**
- Same `PlayerStats` singleton
- Same `WeaponSystem` singleton
- Same `GameProgressionManager` currency
- Same `UpgradeData` and `WeaponData` ScriptableObjects

**You can switch between them anytime!** The data layer is shared, only the interaction layer differs.

Choose based on your game's UX goals, not technical limitations. Both are fully functional! 🎮
