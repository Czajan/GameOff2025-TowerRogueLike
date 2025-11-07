# What's New - Dual Currency System + Persistent Save

## 🎉 Major Update: Split Currency System Implemented!

Your game now has a complete **dual currency system** with **persistent saves** that work between game sessions!

---

## 📦 What's Been Added

### ✨ New Systems

1. **SaveSystem** - Automatic save/load for progression
2. **CurrencyManager** - Handles two separate currencies
3. **PersistentData** - Save file structure

### 🔄 Updated Systems

- `PlayerStats` - Loads/saves upgrade levels
- `UpgradeShop` - Uses meta-currency (Souls)
- `EnemyHealth` - Drops in-run currency (Gold)
- `GameProgressionManager` - Tracks runs and awards Souls
- `GameManager` - Triggers end-of-run rewards
- `CurrencyDisplay` - Shows both currencies
- `SimpleShopUI` - Displays Souls

---

## 💰 Two Currency Types

### 🟡 Gold (In-Run Currency)
- **Earned:** Kill enemies during gameplay
- **Used for:** Obstacles (coming soon)
- **Resets:** Every run
- **Persists:** NO
- **Display:** Yellow/Gold text

### 🔵 Souls (Meta-Currency)
- **Earned:** End of each run (based on performance)
- **Used for:** Permanent upgrades in shop
- **Resets:** Never
- **Persists:** YES (saved to disk)
- **Display:** Cyan/Blue text

---

## 🎮 How It Works

### During a Run:
1. Kill enemies → Earn **Gold** 🟡
2. Gold shows in HUD
3. Gold will be used for placing obstacles (future)

### End of Run:
1. Performance calculated:
   - Gold earned
   - Waves completed
   - Zones held
   - Victory/Defeat
2. Earn **Souls** 🔵 based on performance
3. Souls saved to disk automatically

### In the Shop:
1. Shop displays **Souls** (not Gold)
2. Buy permanent upgrades
3. Upgrades saved immediately
4. Upgrades persist between sessions

### Next Game Start:
1. Souls and upgrades load automatically
2. Start new run with improved stats
3. Gold resets to 0

---

## 💎 Meta-Currency Rewards

Souls earned at end of run:

```
Base Reward    = Gold Earned ÷ 2
Wave Bonus     = Waves Completed × 10
Zone Bonus     = 100/50/25 (based on zone held)
Victory Bonus  = 200 (if won) / 0 (if lost)
────────────────────────────────────────
Total Souls    = Sum of above (minimum 10)
```

**Example:**
- Earned 400 gold
- Completed 6 waves
- Held Zone 1
- Lost the run

**Result:** 200 + 60 + 100 + 0 = **360 Souls earned!**

---

## 📁 New Files

### Scripts Created:
```
/Assets/Scripts/Systems/
├── SaveSystem.cs           - Save/load manager
├── PersistentData.cs       - Save data structure
└── CurrencyManager.cs      - Currency handler
```

### Documentation Created:
```
/Assets/Guide/
├── CURRENCY_SYSTEM_SETUP.md     - Full setup guide
├── CURRENCY_SYSTEM_SUMMARY.md   - Quick reference
├── SETUP_CHECKLIST.md           - Step-by-step setup
├── CURRENCY_FLOW_DIAGRAM.md     - Visual flow
└── WHATS_NEW.md                 - This file
```

---

## 🔧 What You Need to Do

### Mandatory Setup (3 minutes):

1. **Add SaveSystem to scene:**
   - Create GameObject named `SaveSystem`
   - Add `SaveSystem.cs` component
   - Place under `GameManagers`

2. **Add CurrencyManager to scene:**
   - Create GameObject named `CurrencyManager`
   - Add `CurrencyManager.cs` component
   - Place under `GameManagers`

3. **Update Currency Display UI:**
   - Create 2 TextMeshPro texts: `GoldText` and `SoulsText`
   - Update `CurrencyDisplay` component with both texts

**That's it!** Everything else is automatic.

---

## ✅ What Works Right Now

- ✅ Enemies drop Gold when killed
- ✅ Gold displays in UI
- ✅ Souls earned at end of run
- ✅ Shop uses Souls for purchases
- ✅ Upgrades saved to disk
- ✅ Upgrades load on game start
- ✅ Souls persist between sessions
- ✅ Statistics tracked
- ✅ Auto-save on quit

---

## 🧪 Testing the System

### Quick Test (2 minutes):

1. **Start Play Mode**
2. **Kill some enemies** → Gold increases
3. **Return to base** → Open shop
4. **Note your Souls** (may be 0 on first run)
5. **Die or complete run** → Souls awarded
6. **Stop Play Mode**
7. **Start Play Mode again** → Souls should persist!
8. **Buy upgrade** → Spend some Souls
9. **Stop and Start again** → Upgrade should persist!

---

## 🎯 What's Next

Now that currency is working, next up:

### Phase A Continues:
1. ✅ **Currency Split** - COMPLETE!
2. 🔄 **Experience & Leveling** - Next up!
   - XP orbs drop from enemies
   - Level up UI with stat choices
   - Mid-run progression

3. ⏭️ **Obstacle System** - After leveling
   - Place obstacles using Gold
   - Strategic defense options

4. ⏭️ **End-Run Results Screen** - Final polish
   - Show stats and rewards
   - Display Souls earned

---

## 💡 Design Benefits

### Player Perspective:
- **Losing feels less bad** - You still earn Souls
- **Clear progression** - Upgrades persist forever
- **Strategic depth** - Save Gold or spend Gold?
- **"One more run"** - Want to earn more Souls

### Development Perspective:
- **Clean separation** - Two economies don't interfere
- **Easy balancing** - Adjust rewards independently
- **Persistent progression** - Natural meta-game
- **Flexible expansion** - Easy to add new features

---

## 📊 Save File Details

**Location:**
```
Windows: C:/Users/<Name>/AppData/LocalLow/<Company>/<Game>/savefile.json
Mac: ~/Library/Application Support/<Company>/<Game>/savefile.json
Linux: ~/.config/unity3d/<Company>/<Game>/savefile.json
```

**Check Console on game start for exact path!**

**Format:** JSON (human-readable)

**When Saved:**
- On game start (if missing)
- After earning Souls
- After buying upgrades
- On game quit
- Manual save calls

**Safe to Edit:** Yes! For testing, you can manually edit values.

---

## 🐛 Known Issues & Solutions

### "Can't buy upgrades"
- You need Souls, not Gold!
- Complete a run first to earn Souls
- Check Console for "Not enough souls" message

### "Souls don't persist"
- Check Console for save file path
- Verify SaveSystem exists in scene
- Look for save errors in Console

### "Gold doesn't reset"
- This should auto-reset each run
- Check that CurrencyManager.ResetGold() is called
- Look for errors in GameProgressionManager

---

## 📚 Documentation Quick Links

**New to the system?**
→ Read `CURRENCY_SYSTEM_SUMMARY.md` (2 min read)

**Setting up the scene?**
→ Follow `SETUP_CHECKLIST.md` (step-by-step)

**Need detailed info?**
→ See `CURRENCY_SYSTEM_SETUP.md` (full guide)

**Want visual flow?**
→ Check `CURRENCY_FLOW_DIAGRAM.md` (diagrams)

**Overall project status?**
→ See `IMPLEMENTATION_STATUS.md` (35% complete)

---

## 🎊 Congratulations!

You now have a working **roguelike meta-progression system**!

Players can:
- ✅ Earn currency during runs
- ✅ Earn permanent currency after runs
- ✅ Buy permanent upgrades
- ✅ Keep progression between sessions
- ✅ See meaningful advancement

Next up: **Experience & Leveling** for mid-run progression!

---

**Questions or Issues?**
Check the documentation files above, or refer to the implementation notes in the scripts.

**Version:** 1.0  
**Date:** Currency System Implementation  
**Status:** ✅ Ready to Use
