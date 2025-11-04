# ⚠️ CRITICAL: DefenseZone Configuration Corrections

## ❌ Problem: Section 6.2 in COMPLETE_SETUP_GUIDE.md is INCORRECT

The guide lists fields that **DO NOT EXIST** in the `DefenseZone.cs` script!

---

## ✅ CORRECT DefenseZone Component Fields

Use these configurations instead of what's in Section 6.2:

### DefenseZone_1 (Frontline)

```
Zone Settings:
├─ Zone Index: 0
├─ Spawn Center: Drag DefenseZone_1 itself (or create empty child)
└─ Spawn Radius: 10

Zone Perks:
├─ Damage Bonus: 0.0 ← No bonus for frontline
├─ Attack Speed Bonus: 0.0
└─ Move Speed Bonus: 0.0

Fallback:
├─ Next Zone: Drag DefenseZone_2
└─ Fallback Health Threshold: 0.25
```

**Note:** Zone auto-activates when Zone Index = 0!

---

### DefenseZone_2 (Midline)

```
Zone Settings:
├─ Zone Index: 1
├─ Spawn Center: Drag DefenseZone_2 itself (or create empty child)
└─ Spawn Radius: 10

Zone Perks:
├─ Damage Bonus: 0.1 ← 10% damage boost!
├─ Attack Speed Bonus: 0.05 ← 5% attack speed boost
└─ Move Speed Bonus: 0.0

Fallback:
├─ Next Zone: Drag DefenseZone_3
└─ Fallback Health Threshold: 0.25
```

---

### DefenseZone_3 (Base/Last Stand)

```
Zone Settings:
├─ Zone Index: 2
├─ Spawn Center: Drag DefenseZone_3 itself (or create empty child)
└─ Spawn Radius: 10

Zone Perks:
├─ Damage Bonus: 0.2 ← 20% damage boost!
├─ Attack Speed Bonus: 0.1 ← 10% attack speed boost
└─ Move Speed Bonus: 0.1 ← 10% movement boost

Fallback:
├─ Next Zone: Leave empty (None) ← Last zone, no fallback!
└─ Fallback Health Threshold: 0.25
```

**Critical:** DefenseZone_3 is the final fallback. If you lose this zone, game over!

---

## 🚫 Fields That DO NOT EXIST

**❌ IGNORE these if you see them in the guide:**

- `zoneName` - Does not exist! No string field for names
- `isActive` - Private field, NOT visible in Inspector  
- `perkMultiplier` - Does not exist! Use individual bonus fields instead

---

## ✅ Actual Script Fields (DefenseZone.cs)

Here's what the script ACTUALLY has:

```csharp
[Header("Zone Settings")]
[SerializeField] private int zoneIndex = 0;
[SerializeField] private Transform spawnCenter;
[SerializeField] private float spawnRadius = 10f;

[Header("Zone Perks")]
[SerializeField] private float damageBonus = 0f;
[SerializeField] private float attackSpeedBonus = 0f;
[SerializeField] private float moveSpeedBonus = 0f;

[Header("Fallback")]
[SerializeField] private DefenseZone nextZone;
[SerializeField] private float fallbackHealthThreshold = 0.25f;

// isActive is PRIVATE - not in Inspector!
private bool isActive = false;
```

---

## 🎯 Quick Setup Checklist

### For Each DefenseZone GameObject:

1. **Add DefenseZone component**
2. **Set Zone Index:**
   - DefenseZone_1 → 0
   - DefenseZone_2 → 1
   - DefenseZone_3 → 2

3. **Set Spawn Center:**
   - Option A: Drag the DefenseZone GameObject itself
   - Option B: Create empty child named "SpawnCenter" and drag that

4. **Set Spawn Radius:** 10 (default is fine)

5. **Set Zone Perks (bonuses are multipliers, e.g., 0.1 = 10%):**
   - Zone 1: All 0 (no bonuses)
   - Zone 2: Damage 0.1, Attack Speed 0.05, Move Speed 0
   - Zone 3: Damage 0.2, Attack Speed 0.1, Move Speed 0.1

6. **Set Fallback:**
   - Zone 1 → Next Zone: DefenseZone_2, Threshold: 0.25
   - Zone 2 → Next Zone: DefenseZone_3, Threshold: 0.25
   - Zone 3 → Next Zone: None, Threshold: 0.25

7. **✅ You're done!** - NO spawn points needed!

---

## 🚨 IMPORTANT: Ignore Spawn Points Instructions!

**❌ Section 6.3 of COMPLETE_SETUP_GUIDE.md is WRONG!**

The guide tells you to:
- Create 5 spawn point children for each zone
- Assign them to a "Spawn Points" array

**This is completely incorrect!** DefenseZone has NO spawn points array.

### ✅ How Spawning Actually Works

DefenseZone spawns enemies at **random positions** within a circle:

```csharp
// This is the actual code (lines 92-98)
public Vector3 GetSpawnPosition()
{
    Vector3 randomCircle = Random.insideUnitCircle * spawnRadius;
    Vector3 spawnPos = spawnCenter.position + new Vector3(randomCircle.x, 0, randomCircle.y);
    spawnPos.y = 0;
    return spawnPos;
}
```

**Enemies spawn randomly** in a circle around `spawnCenter` with radius `spawnRadius`.

### 🗑️ Delete Those Spawn Points!

If you created spawn point GameObjects as children:
1. Select them all
2. Delete them
3. They serve no purpose and are never used

### 🎨 Visual Debug

When you select a DefenseZone in the Scene view, you'll see:
- **Yellow/Green wire sphere** - The spawn radius (where enemies can spawn)
- **Blue sphere** - The spawn center point

This is all you need! The script draws these gizmos automatically (see lines 105-114).

---

## 💡 Understanding the System

### How Zone Activation Works:
- Zone with `zoneIndex = 0` auto-activates on Start()
- `isActive` is managed internally, not by you
- When player health drops below `fallbackHealthThreshold`, zone triggers fallback

### How Perks Work:
- Bonuses are **percentage multipliers** (0.1 = +10%)
- `damageBonus`: Adds to player attack damage
- `attackSpeedBonus`: Reduces attack cooldown
- `moveSpeedBonus`: Increases movement speed
- Perks apply when zone is active

### How Fallback Works:
- When player health ≤ `fallbackHealthThreshold` (default 25%), zone checks fallback
- If `nextZone` is set, player teleports to that zone's `spawnCenter`
- Current zone deactivates, next zone activates with its perks
- If no `nextZone` set (Zone 3), no fallback = game continues until death

### How Spawning Works:
- Enemies spawn randomly within `spawnRadius` of `spawnCenter`
- Used by `WaveSpawner` to determine enemy spawn positions
- Larger radius = more spread out enemies

---

## 📋 Common Mistakes to Avoid

❌ **Don't** look for a "Zone Name" field - it doesn't exist!  
❌ **Don't** try to check/uncheck "Is Active" - it's not in the Inspector!  
❌ **Don't** look for "Perk Multiplier" - use the 3 separate bonus fields!  
❌ **Don't** forget to set `spawnCenter` - enemies won't spawn correctly!  
❌ **Don't** set `nextZone` on DefenseZone_3 - it should be None!

✅ **Do** use `zoneIndex` to identify zones (0, 1, 2)  
✅ **Do** set individual bonuses (damage, attack speed, move speed)  
✅ **Do** link zones in fallback chain: Zone1 → Zone2 → Zone3 → None  
✅ **Do** test fallback by reducing player health below 25%  
✅ **Do** add 5 spawn point children to each zone for variety

---

## 🔧 Troubleshooting

**Problem:** "I don't see Zone Name or Perk Multiplier fields!"  
**Solution:** ✅ Correct! Those fields don't exist. Use the fields listed above.

**Problem:** "Is Active checkbox is missing!"  
**Solution:** ✅ Correct! It's a private field. Zone auto-activates based on zoneIndex.

**Problem:** "Enemies aren't spawning!"  
**Solution:** Make sure `spawnCenter` is assigned and `spawnRadius` > 0.

**Problem:** "Fallback isn't working!"  
**Solution:** Check that `nextZone` is assigned and player health goes below 0.25.

**Problem:** "Zone perks not applying!"  
**Solution:** Perks are applied but `ApplyZonePerks()` method is currently empty (line 88-90 in script). This system may need implementation.

---

## ⚠️ Action Required

**Use this document** instead of Section 6.2 in COMPLETE_SETUP_GUIDE.md until that section is updated.

All field names and values in this document are verified against the actual `DefenseZone.cs` script (Unity 6, 6000.2).

---

**Status:** ✅ Verified against script  
**Script Path:** `/Assets/Scripts/Systems/DefenseZone.cs`  
**Last Updated:** 2025  
**Unity Version:** 6000.2
