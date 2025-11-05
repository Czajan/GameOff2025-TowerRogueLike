# Immediate Fix: SimpleShopUI Script Not Showing

## The Problem
You can see `SimpleShopUI.cs` in the Assets but can't add it as a component to GameObjects.

## Why This Happens
The script file exists but Unity can't compile the class, so it appears as "Missing Script" when you try to add it.

---

## ✅ **SOLUTION: Follow These Steps**

### Step 1: Open Console Window
1. **Window > General > Console** (or press `Ctrl+Shift+C`)
2. **Look for red error messages**
3. If you see errors, read them carefully

### Step 2: Force Scripts to Recompile

The scripts were just created and Unity might not have compiled them yet.

**Do this:**
1. In Unity, select **Assets > Refresh** (or press `Ctrl+R`)
2. Wait for the spinning icon in bottom-right to stop
3. Check Console again - errors should be gone

### Step 3: Verify Dependencies Exist

Open each of these files and confirm they exist:

#### Required Files Checklist:
- [ ] `/Assets/Scripts/Systems/UpgradeData.cs`
- [ ] `/Assets/Scripts/Systems/WeaponData.cs`
- [ ] `/Assets/Scripts/Systems/PlayerStats.cs`
- [ ] `/Assets/Scripts/Systems/GameProgressionManager.cs`
- [ ] `/Assets/Scripts/Systems/ShopNPC.cs`
- [ ] `/Assets/Scripts/Systems/SimpleShopUI.cs`
- [ ] `/Assets/Scripts/Systems/DebugShopTester.cs`

### Step 4: Verify PlayerStats Has Level Getters

Open `/Assets/Scripts/Systems/PlayerStats.cs` and verify these methods exist (around line 117):

```csharp
public int GetMoveSpeedLevel() => moveSpeedLevel;
public int GetMaxHealthLevel() => maxHealthLevel;
public int GetDamageLevel() => damageLevel;
public int GetCritChanceLevel() => critChanceLevel;
public int GetCritDamageLevel() => critDamageLevel;
public int GetAttackRangeLevel() => attackRangeLevel;
```

**If they're missing:** I already added them earlier, so check if the file was saved properly.

### Step 5: Verify ShopNPC Has Getter Methods

Open `/Assets/Scripts/Systems/ShopNPC.cs` and verify these methods exist (around line 262):

```csharp
public ShopNPCType GetNPCType() => npcType;
public string GetNPCName() => npcName;
public WeaponData[] GetAvailableWeapons() => availableWeapons;
public UpgradeData[] GetAvailableUpgrades() => availableUpgrades;
```

**If they're missing:** They should already be there in the original ShopNPC.cs file.

### Step 6: Check for Enum Definitions

#### In `/Assets/Scripts/Systems/ShopNPC.cs` (top of file):
```csharp
public enum ShopNPCType
{
    WeaponVendor,
    StatUpgradeVendor,
    ConsumableVendor,
    SpecialVendor
}
```

#### In `/Assets/Scripts/Systems/UpgradeData.cs`:
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
```

#### In `/Assets/Scripts/Systems/WeaponData.cs`:
```csharp
public enum WeaponEffect
{
    None,
    Fire,
    Ice,
    Lightning,
    Poison,
    Lifesteal
}
```

---

## 🔧 **If Still Broken: Manual Recompile**

### Method 1: Reimport Script
1. **Find** `SimpleShopUI.cs` in Project window
2. **Right-click** → **Reimport**
3. Wait for compilation to finish
4. Check Console

### Method 2: Force Edit
1. **Open** `SimpleShopUI.cs` in your code editor
2. **Add a space** anywhere in the file
3. **Save** the file (Ctrl+S / Cmd+S)
4. **Switch back to Unity** - it will auto-recompile
5. Check Console

### Method 3: Restart Unity
1. **Save** your scene (Ctrl+S)
2. **Close** Unity Editor
3. **Reopen** the project
4. Check Console after it loads

---

## 🎯 **Expected Result**

After following the steps above:

✅ **Console shows 0 errors**  
✅ **SimpleShopUI appears in Add Component menu**  
✅ **DebugShopTester appears in Add Component menu**  
✅ **No "Missing Script" warnings**

---

## 🐛 **Debugging: What To Look For**

### If Console Shows Errors:

#### Error Type 1: Missing Type
```
The type or namespace name 'ShopNPCType' could not be found
```
**Fix:** Check `ShopNPC.cs` has the enum at the top of the file

#### Error Type 2: Missing Method
```
'ShopNPC' does not contain a definition for 'GetNPCName'
```
**Fix:** Check `ShopNPC.cs` has all getter methods (line ~262)

#### Error Type 3: Missing Using
```
The type or namespace name 'TextMeshProUGUI' could not be found
```
**Fix:** Check `SimpleShopUI.cs` has `using TMPro;` at the top

#### Error Type 4: Missing PlayerStats Methods
```
'PlayerStats' does not contain a definition for 'GetMoveSpeedLevel'
```
**Fix:** Check `PlayerStats.cs` has level getter methods (line ~117)

---

## ✅ **Quick Test After Fix**

Once scripts compile:

1. **Select** `/GameCanvas/ShopPanel` in Hierarchy
2. **Look** in Inspector - SimpleShopUI component should be there
3. **If you just added it:** The component will show with all fields
4. **Assign references:**
   - Item List Container
   - Item Button Prefab
   - Shop Title Text
   - Currency Text
   - Close Button

---

## 📋 **Verification Steps**

After fixing, verify:

- [ ] Console has 0 errors (red messages)
- [ ] SimpleShopUI shows in Add Component menu
- [ ] DebugShopTester shows in Add Component menu
- [ ] ShopPanel has SimpleShopUI component
- [ ] All fields in SimpleShopUI are visible in Inspector
- [ ] Can drag references to fields

---

## 💡 **Why This Happened**

When I created the scripts:
1. Unity might not have immediately compiled them
2. Dependencies might have compiled in wrong order
3. Unity sometimes needs a manual refresh

**This is normal** when creating multiple interdependent scripts at once.

---

## 🚀 **Next Steps After Fix**

Once SimpleShopUI is working:

1. ✅ Assign all references in SimpleShopUI component
2. ✅ Create item button prefab
3. ✅ Add DebugShopTester to GameManagers
4. ✅ Test the shop system

**Follow:** `/Assets/Guide/QUICK_TEST_CHECKLIST.md` for setup

---

## ⚠️ **Important Notes**

- **Don't rename** the script files - must match class names
- **Don't move** scripts to different folders without checking references
- **Always check Console** before asking why a script won't work
- **Compilation is automatic** but sometimes needs a nudge

---

**This should fix your SimpleShopUI issue! If Console shows specific errors, read them carefully - they tell you exactly what's wrong.**
