# Script Compilation Checklist

## ⚠️ CRITICAL: Always Verify Scripts Compile

Before adding any script as a component in Unity, you **MUST** verify it compiles without errors.

---

## 🔍 Quick Verification Steps

### 1. Check Console (Mandatory)
- **Open Console:** `Ctrl+Shift+C` (Windows) or `Cmd+Shift+C` (Mac)
- **Look for red error messages**
- **Zero errors = scripts compiled successfully**

### 2. Check Script is Recognized
- If script file exists but can't be added as component → **Compilation error**
- Unity will show "Missing Script" or grayed-out component name

### 3. Verify Dependencies
- Check all `using` statements are correct
- Ensure all referenced types exist (classes, enums, interfaces)
- Verify all dependent scripts compiled successfully

---

## 🛠️ Troubleshooting Missing Script Components

### Symptom: "Script exists in Assets but can't add it as component"

**Root Cause:** The script has compilation errors and Unity can't create the class.

**Solution Steps:**

#### Step 1: Check Console for Errors
1. Open **Console** window (Window > General > Console)
2. Look for **red error messages**
3. Read the error carefully - it tells you exactly what's wrong
4. Common errors:
   - `Type or namespace 'X' could not be found`
   - `The name 'X' does not exist in the current context`
   - `'X' does not contain a definition for 'Y'`

#### Step 2: Verify Using Statements
```csharp
using UnityEngine;           // Required for MonoBehaviour
using UnityEngine.UI;        // Required for Button, Slider, etc.
using TMPro;                 // Required for TextMeshProUGUI
using UnityEngine.Events;    // Required for UnityEvent
```

#### Step 3: Check Class Name Matches File Name
- File: `SimpleShopUI.cs` → Class: `public class SimpleShopUI`
- **MUST match exactly** (case-sensitive)

#### Step 4: Verify Dependencies Exist
Check if all types referenced in the script exist:

**For SimpleShopUI.cs:**
- `ShopNPC` class exists? → Check `/Assets/Scripts/Systems/ShopNPC.cs`
- `ShopNPCType` enum exists? → Should be in ShopNPC.cs
- `WeaponData` class exists? → Check `/Assets/Scripts/Systems/WeaponData.cs`
- `UpgradeData` class exists? → Check `/Assets/Scripts/Systems/UpgradeData.cs`
- `UpgradeType` enum exists? → Should be in UpgradeData.cs
- `PlayerStats` singleton exists? → Check `/Assets/Scripts/Systems/PlayerStats.cs`
- `GameProgressionManager` singleton exists? → Check `/Assets/Scripts/Systems/GameProgressionManager.cs`

**For DebugShopTester.cs:**
- Same dependencies as above

#### Step 5: Force Recompile
If no errors but script still not recognized:

1. **Edit the script:**
   - Add a space or newline
   - Save the file (Ctrl+S / Cmd+S)
   
2. **Reimport the script:**
   - Right-click script in Project window
   - Select **Reimport**

3. **Restart Unity Editor** (last resort)

---

## 📋 Common Compilation Issues

### Issue 1: Missing Enum Definition

**Error:**
```
The type or namespace name 'ShopNPCType' could not be found
```

**Cause:** Enum not defined or in different file

**Solution:** Verify enum exists in same file or is accessible:
```csharp
// ShopNPC.cs must have this at the top
public enum ShopNPCType
{
    WeaponVendor,
    StatUpgradeVendor,
    ConsumableVendor,
    SpecialVendor
}
```

### Issue 2: Missing Method in Referenced Class

**Error:**
```
'ShopNPC' does not contain a definition for 'GetNPCName'
```

**Cause:** Method doesn't exist in the referenced class

**Solution:** Add missing method to ShopNPC.cs:
```csharp
public string GetNPCName() => npcName;
public ShopNPCType GetNPCType() => npcType;
public WeaponData[] GetAvailableWeapons() => availableWeapons;
public UpgradeData[] GetAvailableUpgrades() => availableUpgrades;
```

### Issue 3: Missing Using Statement

**Error:**
```
The type or namespace name 'TextMeshProUGUI' could not be found
```

**Cause:** Missing `using TMPro;`

**Solution:** Add to top of file:
```csharp
using TMPro;
```

### Issue 4: Circular Dependencies

**Error:**
```
[CompilerError] Circular dependency detected
```

**Cause:** Script A references Script B, and Script B references Script A

**Solution:** 
- Refactor to use interfaces
- Use singleton pattern
- Restructure dependencies

---

## ✅ Required Scripts for Shop System

Verify all these scripts exist and compile:

### Core Scripts (Must exist first):
- [x] `/Assets/Scripts/Systems/UpgradeData.cs` - Defines `UpgradeData` class and `UpgradeType` enum
- [x] `/Assets/Scripts/Systems/WeaponData.cs` - Defines `WeaponData` class and `WeaponEffect` enum
- [x] `/Assets/Scripts/Systems/PlayerStats.cs` - Singleton with stat getters
- [x] `/Assets/Scripts/Systems/GameProgressionManager.cs` - Singleton with currency system
- [x] `/Assets/Scripts/Systems/WeaponSystem.cs` - Weapon management

### NPC Scripts (Depend on Core):
- [x] `/Assets/Scripts/Systems/ShopNPC.cs` - Defines `ShopNPCType` enum and `ShopNPC` class
- [x] `/Assets/Scripts/Systems/NPCInteractionPrompt.cs` - Optional interaction UI

### UI Scripts (Depend on NPC + Core):
- [x] `/Assets/Scripts/Systems/SimpleShopUI.cs` - UI system for shop
- [x] `/Assets/Scripts/Systems/DebugShopTester.cs` - Debug testing tool

### Compilation Order:
```
1. UpgradeData.cs, WeaponData.cs (no dependencies)
2. PlayerStats.cs, GameProgressionManager.cs (no dependencies)
3. WeaponSystem.cs (depends on WeaponData)
4. ShopNPC.cs (depends on UpgradeData, WeaponData)
5. SimpleShopUI.cs (depends on ShopNPC, UpgradeData, WeaponData, PlayerStats, GameProgressionManager)
6. DebugShopTester.cs (depends on UpgradeData, WeaponData, PlayerStats, GameProgressionManager)
```

---

## 🔧 Manual Fix Workflow

If scripts won't compile, follow this order:

### 1. Fix Core Data Scripts First
```
✓ UpgradeData.cs - Check enum UpgradeType exists
✓ WeaponData.cs - Check enum WeaponEffect exists
```

### 2. Fix Manager Scripts
```
✓ PlayerStats.cs - Check all getter methods exist
✓ GameProgressionManager.cs - Check currency methods exist
```

### 3. Fix NPC Scripts
```
✓ ShopNPC.cs - Check enum ShopNPCType exists
✓ ShopNPC.cs - Check all public getter methods exist
```

### 4. Fix UI Scripts Last
```
✓ SimpleShopUI.cs - Should now compile if above are fixed
✓ DebugShopTester.cs - Should now compile if above are fixed
```

---

## 🎯 Verification Checklist

Before reporting "script won't compile", verify:

- [ ] Console window is open and visible
- [ ] No red error messages in Console
- [ ] All using statements are present
- [ ] Class name matches file name exactly
- [ ] All referenced types exist (classes, enums)
- [ ] All referenced methods exist in dependent classes
- [ ] Saved the file after editing (Ctrl+S / Cmd+S)
- [ ] Unity has finished compiling (spinner stopped)
- [ ] Tried reimporting the script
- [ ] Tried restarting Unity (last resort)

---

## 💡 Pro Tips

1. **Always check Console first** - Errors tell you exactly what's wrong
2. **Fix errors from top to bottom** - First error often causes others
3. **One script at a time** - Don't create multiple scripts at once
4. **Test incrementally** - Compile after each script creation
5. **Read error messages carefully** - They're very specific about the problem
6. **Check spelling and capitalization** - C# is case-sensitive
7. **Verify dependencies exist** - Check files actually exist in Project
8. **Use IDE features** - Visual Studio / Rider show errors as you type

---

## 📚 Quick Reference: Required Methods in Each Script

### PlayerStats.cs Must Have:
```csharp
public int GetMoveSpeedLevel() => moveSpeedLevel;
public int GetMaxHealthLevel() => maxHealthLevel;
public int GetDamageLevel() => damageLevel;
public int GetCritChanceLevel() => critChanceLevel;
public int GetCritDamageLevel() => critDamageLevel;
public int GetAttackRangeLevel() => attackRangeLevel;

public void UpgradeMoveSpeed()
public void UpgradeMaxHealth()
public void UpgradeDamage()
public void UpgradeCritChance()
public void UpgradeCritDamage()
public void UpgradeAttackRange()
```

### GameProgressionManager.cs Must Have:
```csharp
public void AddCurrency(int amount)
public bool SpendCurrency(int amount)
public int Currency { get; }
public UnityEvent<int> OnCurrencyChanged
```

### ShopNPC.cs Must Have:
```csharp
public string GetNPCName() => npcName;
public ShopNPCType GetNPCType() => npcType;
public WeaponData[] GetAvailableWeapons() => availableWeapons;
public UpgradeData[] GetAvailableUpgrades() => availableUpgrades;
public bool TryPurchaseWeapon(WeaponData weapon)
public bool TryPurchaseUpgrade(UpgradeData upgrade, int currentLevel)
public void OpenShop()
public void CloseShop()
```

### UpgradeData.cs Must Have:
```csharp
public enum UpgradeType
{
    MoveSpeed,
    MaxHealth,
    Damage,
    CritChance,
    CritDamage,
    AttackRange
}

public int GetCostForLevel(int currentLevel)
```

---

## 🆘 Still Having Issues?

1. **Copy exact error message** from Console
2. **Identify which script** is failing to compile
3. **Check dependencies** of that script
4. **Verify all referenced types exist**
5. **Compare against reference implementation** in this guide

**Remember:** 99% of "missing script" issues are compilation errors that Console tells you about!

---

**Updated:** After shop system implementation  
**Version:** 1.0
