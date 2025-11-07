# Currency System - Quick Reference

## 🎯 What Changed?

### Before:
- ❌ Single currency for everything
- ❌ No persistence between sessions
- ❌ Lost all progress on game restart

### After:
- ✅ **Gold** - In-run currency (resets each run)
- ✅ **Souls** - Meta-currency (persists forever)
- ✅ Upgrades and souls saved between sessions
- ✅ Automatic save system

---

## 💰 Currency Types

| Currency | Where Earned | Where Spent | Persists? | Color |
|----------|--------------|-------------|-----------|-------|
| **Gold** 🟡 | Kill enemies during run | Obstacles (future) | ❌ No | Yellow |
| **Souls** 🔵 | End of run rewards | Permanent upgrades | ✅ Yes | Cyan |

---

## 📁 New Files Created

```
/Assets/Scripts/Systems/
├── SaveSystem.cs           ← Handles save/load
├── PersistentData.cs       ← Save data structure
└── CurrencyManager.cs      ← Manages both currencies

/Assets/Guide/
├── CURRENCY_SYSTEM_SETUP.md     ← Full setup guide
└── CURRENCY_SYSTEM_SUMMARY.md   ← This file
```

---

## 🔧 Scene Setup (Quick Steps)

1. **Add SaveSystem GameObject** to GameManagers
   - Add `SaveSystem.cs` component

2. **Add CurrencyManager GameObject** to GameManagers
   - Add `CurrencyManager.cs` component

3. **Update Currency Display UI**
   - Create 2 separate TextMeshPro texts:
     - `GoldText` - Shows gold
     - `SoulsText` - Shows souls
   - Assign both to `CurrencyDisplay` component

4. **Test in Play Mode**
   - Kill enemies → Earn gold
   - Die or complete run → Earn souls
   - Restart game → Souls and upgrades persist!

---

## 🎮 How It Works

### During a Run:
1. Player kills enemies → Earn **Gold** 🟡
2. Gold displayed in HUD
3. Gold will be used for obstacles (future feature)

### End of Run:
1. Calculate soul rewards based on:
   - Gold earned
   - Waves completed
   - Zones held
   - Victory/defeat
2. Award **Souls** 🔵
3. Save souls to disk
4. Gold is reset

### In the Shop:
1. Shop displays **Souls** (not Gold)
2. Purchase permanent upgrades
3. Upgrades saved to disk
4. Upgrades persist between sessions

---

## 💎 Reward Formula

```
Souls Earned = (Gold ÷ 2) + (Waves × 10) + Zone Bonus + Victory Bonus

Zone Bonus:
- Zone 1: +100 souls
- Zone 2: +50 souls
- Zone 3: +25 souls

Victory Bonus:
- Win: +200 souls
- Lose: +0 souls

Minimum: 10 souls
```

---

## 🧪 Quick Test

1. **Start game** → Check Console for "Save file path: ..."
2. **Kill enemies** → Gold should increase
3. **Buy upgrade** → Costs souls (may have 0 on first run)
4. **Die** → Should see "Souls earned: X"
5. **Restart game** → Upgrade levels and souls persist

---

## 📊 API Reference

### SaveSystem
```csharp
SaveSystem.Instance.AddMetaCurrency(100);        // Add 100 souls
SaveSystem.Instance.SpendMetaCurrency(50);       // Spend 50 souls
SaveSystem.Instance.GetMetaCurrency();           // Get current souls
SaveSystem.Instance.SaveGame();                  // Manual save
```

### CurrencyManager
```csharp
CurrencyManager.Instance.AddGold(10);            // +10 gold
CurrencyManager.Instance.SpendGold(5);           // -5 gold
CurrencyManager.Instance.AddMetaCurrency(50);    // +50 souls
CurrencyManager.Instance.ResetGold();            // Reset gold to 0
```

---

## ⚠️ Important Notes

1. **Shop uses Souls, not Gold**
   - All permanent upgrades cost souls
   - Future obstacle shop will use gold

2. **Gold resets each run**
   - This is intentional!
   - Creates tension during gameplay

3. **Souls earned at run end only**
   - Not during gameplay
   - Based on performance

4. **Save file location**
   - Check Console for exact path
   - Can manually edit for testing
   - JSON format

---

## 🚀 What's Next?

With currency system complete, next steps are:

1. ✅ **Currency Split** - COMPLETE
2. 🔄 **Experience & Leveling** - IN PROGRESS
3. ⏭️ **Obstacle System** - Uses Gold
4. ⏭️ **Skill Drafting** - Mid-run progression

---

For detailed setup instructions, see `CURRENCY_SYSTEM_SETUP.md`
