# ⚡ Quick Testing Checklist

You have all the ScriptableObjects created! Here's what you need to do to start testing:

## 🎯 Minimal Setup (10 minutes)

### ✅ Already Done
- [x] Created all 6 upgrade assets
- [x] Created all 5 weapon assets
- [x] Enemy currency reward set to 10
- [x] All scripts in place

---

### 📝 Step 1: Assign ScriptableObjects to NPCs (2 min)

#### **WeaponVendor:**
1. Select `/Base/NPCs/WeaponVendor` in Hierarchy
2. In Inspector → **ShopNPC** component:
   - Set **Available Weapons** array size to **5**
   - Drag all 5 weapons from `/Assets/Data/Weapons`:
     - BasicSword
     - FireBlade
     - IceBlade
     - LightningStaff
     - CriticalDagger

#### **StatVendor:**
1. Select `/Base/NPCs/StatVendor` in Hierarchy
2. In Inspector → **ShopNPC** component:
   - Set **Available Upgrades** array size to **6**
   - Drag all 6 upgrades from `/Assets/Data/Upgrades`:
     - MoveSpeedUpgrade
     - MaxHealthUpgrade
     - DamageUpgrade
     - CritChanceUpgrade
     - CritDamageUpgrade
     - AttackRangeUpgrade

---

### 📝 Step 2: Create Simple Shop UI (5 min)

The easiest way is to create a minimal UI:

1. **Select** `/GameCanvas` in Hierarchy
2. **Right-click** → **UI** → **Panel** → Name: `ShopPanel`
3. **Inside ShopPanel**, add:
   - **TextMeshPro - Text** → Name: `ShopTitle` (for vendor name)
   - **TextMeshPro - Text** → Name: `CurrencyText` (for currency display)
   - **Scroll View** → Inside Content, add **Vertical Layout Group**
   - **Button - TextMeshPro** → Name: `CloseButton`

4. **Create Item Button Prefab:**
   - Right-click Hierarchy → **UI** → **Button - TextMeshPro** → Name: `ShopItemButton`
   - Add 3 TextMeshPro children: `Name`, `Description`, `Cost`
   - Drag to `/Assets/Prefabs/UI/` to make prefab
   - Delete from Hierarchy

5. **Add SimpleShopUI component** to `ShopPanel`
   - Assign all references (drag components)
   - **Disable ShopPanel** initially

---

### 📝 Step 3: Create Interaction Prompts (3 min)

For **each NPC** (do this twice):

1. **Select** NPC in Hierarchy
2. **Right-click** → **UI** → **Canvas** → Name: `InteractionCanvas`
   - **Render Mode:** World Space
   - **Width:** 200, **Height:** 100
   - **Scale:** 0.01 on all axes
3. Add **Panel** child → Add **TextMeshPro - Text** child
4. Add **NPCInteractionPrompt** component to Canvas
5. **Disable** the Canvas

---

### 📝 Step 4: Link Everything (2 min)

**For each NPC:**
1. Select NPC in Hierarchy
2. In **ShopNPC** component:
   - **Interaction Prompt:** Drag its `InteractionCanvas`
   - **Shop UI:** Drag `GameCanvas/ShopPanel`

**For WaveController:**
1. Select `/WaveController`
2. Set **Defense Zones** size to 3
3. Drag DefenseZone1, DefenseZone2, DefenseZone3

---

## 🎮 EVEN FASTER - Debug Mode (1 minute)

If you want to test **immediately without UI**:

1. **Open Play Mode**
2. **Walk near NPCs** - you'll see console messages
3. **Press E near NPC** - shop logic runs (check console)
4. **Purchases work via code** - stats update (no UI needed for now)

The ShopNPC script has full debug logging, so you can test purchases even without UI!

---

## 🧪 Test Flow (Without UI)

1. **Enter Play Mode**
2. **Open Console window** (Ctrl+Shift+C)
3. **Kill 5 enemies** → Console: "Added 10 currency" (x5)
4. **Walk to StatVendor** → Console: "Near Trainer. Press E to interact"
5. **Press E** → Console: "Opened Trainer's shop"
   - *(No UI appears, but shop is "open")*
6. **Manually test in code** or **add temporary debug buttons**

---

## 🛠️ Temporary Debug Testing Script

Create this simple script to test without UI:

```csharp
// DebugShopTester.cs - attach to GameManagers
using UnityEngine;

public class DebugShopTester : MonoBehaviour
{
    private void Update()
    {
        // Press 1-6 to buy stat upgrades
        if (Input.GetKeyDown(KeyCode.Alpha1)) BuyMoveSpeed();
        if (Input.GetKeyDown(KeyCode.Alpha2)) BuyMaxHealth();
        if (Input.GetKeyDown(KeyCode.Alpha3)) BuyDamage();
        
        // Press C to add currency
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameProgressionManager.Instance?.AddCurrency(100);
            Debug.Log("Added 100 currency for testing!");
        }
        
        // Press L to log stats
        if (Input.GetKeyDown(KeyCode.L))
        {
            LogStats();
        }
    }
    
    private void BuyMoveSpeed()
    {
        var upgrade = GetUpgrade("MoveSpeedUpgrade");
        if (upgrade != null)
        {
            int level = PlayerStats.Instance.GetMoveSpeedLevel();
            TryBuy(upgrade, level, () => PlayerStats.Instance.UpgradeMoveSpeed());
        }
    }
    
    private void BuyMaxHealth()
    {
        var upgrade = GetUpgrade("MaxHealthUpgrade");
        if (upgrade != null)
        {
            int level = PlayerStats.Instance.GetMaxHealthLevel();
            TryBuy(upgrade, level, () => PlayerStats.Instance.UpgradeMaxHealth());
        }
    }
    
    private void BuyDamage()
    {
        var upgrade = GetUpgrade("DamageUpgrade");
        if (upgrade != null)
        {
            int level = PlayerStats.Instance.GetDamageLevel();
            TryBuy(upgrade, level, () => PlayerStats.Instance.UpgradeDamage());
        }
    }
    
    private void TryBuy(UpgradeData upgrade, int currentLevel, System.Action onSuccess)
    {
        int cost = upgrade.GetCostForLevel(currentLevel);
        
        if (GameProgressionManager.Instance.SpendCurrency(cost))
        {
            onSuccess?.Invoke();
            Debug.Log($"✓ Bought {upgrade.upgradeName}! Now level {currentLevel + 1}");
        }
        else
        {
            Debug.Log($"✗ Not enough currency! Need {cost}");
        }
    }
    
    private UpgradeData GetUpgrade(string name)
    {
        return Resources.Load<UpgradeData>($"Data/Upgrades/{name}");
    }
    
    private void LogStats()
    {
        Debug.Log("=== CURRENT STATS ===");
        Debug.Log($"Currency: {GameProgressionManager.Instance?.Currency}");
        Debug.Log($"Move Speed: {PlayerStats.Instance?.GetMoveSpeed()} (Lv {PlayerStats.Instance?.GetMoveSpeedLevel()})");
        Debug.Log($"Max Health: {PlayerStats.Instance?.GetMaxHealth()} (Lv {PlayerStats.Instance?.GetMaxHealthLevel()})");
        Debug.Log($"Damage: {PlayerStats.Instance?.GetDamage()} (Lv {PlayerStats.Instance?.GetDamageLevel()})");
    }
}
```

**Usage:**
- Press **C** to add 100 currency
- Press **1** to buy Move Speed
- Press **2** to buy Max Health
- Press **3** to buy Damage
- Press **L** to see current stats in console

---

## 🎯 Recommended Testing Order

### Option A: Quick Test (No UI)
1. ✅ Assign assets to NPCs
2. ✅ Add DebugShopTester script
3. ✅ Enter Play Mode
4. ✅ Test with keyboard shortcuts

### Option B: Full UI Test
1. ✅ Follow full TESTING_SETUP.md guide
2. ✅ Create all UI elements
3. ✅ Link everything
4. ✅ Test with mouse clicks

---

## 📋 Final Verification

Before testing, check these exist:
- [ ] `/GameManagers` has all 4 singleton components
- [ ] `/Base/NPCs/WeaponVendor` has 5 weapons assigned
- [ ] `/Base/NPCs/StatVendor` has 6 upgrades assigned
- [ ] `/WaveController` has 3 defense zones assigned
- [ ] Player has tag "Player"
- [ ] Enemy is on layer "Enemy"

---

**Choose your path and start testing! Both options work.**
