# Currency System - Flow Diagram

## 🔄 Complete Currency Flow

```
┌─────────────────────────────────────────────────────────────┐
│                        GAME START                            │
│                                                              │
│  SaveSystem loads from disk:                                │
│  ├─ Meta-Currency (Souls): 150                             │
│  ├─ Move Speed Level: 2                                    │
│  ├─ Health Level: 1                                        │
│  └─ Damage Level: 3                                        │
│                                                              │
│  PlayerStats applies saved upgrades to player              │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      RUN START                               │
│                                                              │
│  CurrencyManager.ResetGold()                                │
│  ├─ Gold: 0 🟡                                              │
│  └─ Souls: 150 🔵 (unchanged)                               │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    DURING GAMEPLAY                           │
│                                                              │
│  Player kills Enemy                                         │
│  │                                                           │
│  ├─→ EnemyHealth.Die()                                      │
│  │   └─→ CurrencyManager.AddGold(10)                       │
│  │       └─→ Gold: 10 🟡                                    │
│  │                                                           │
│  ├─→ SaveSystem.AddEnemyKill()                             │
│  │   └─→ Statistics tracking                               │
│  │                                                           │
│  └─→ Repeat for each kill...                               │
│                                                              │
│  After Wave Complete:                                       │
│  └─→ Gold accumulated: 250 🟡                               │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    RETURN TO BASE                            │
│                                                              │
│  GameProgressionManager.EnterBase()                         │
│  │                                                           │
│  └─→ UpgradeShop opens                                      │
│      │                                                       │
│      ├─→ Displays: Souls: 150 🔵                            │
│      │   (NOT Gold: 250 🟡)                                 │
│      │                                                       │
│      └─→ Player buys upgrade (costs 50 souls)              │
│          │                                                   │
│          ├─→ CurrencyManager.SpendMetaCurrency(50)         │
│          │   └─→ Souls: 100 🔵                              │
│          │                                                   │
│          ├─→ PlayerStats.UpgradeDamage()                   │
│          │   └─→ Damage Level: 3 → 4                       │
│          │                                                   │
│          └─→ SaveSystem.SaveUpgradeLevels()                │
│              └─→ Saved to disk!                            │
│                                                              │
│  Player exits base, continues run...                        │
│  Gold: 250 🟡 (unchanged)                                   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      RUN ENDS                                │
│                   (Player Dies or Wins)                      │
│                                                              │
│  GameProgressionManager.OnRunComplete(victory)              │
│  │                                                           │
│  ├─→ Calculate Meta-Currency Reward:                       │
│  │   │                                                       │
│  │   ├─ Base: Gold ÷ 2 = 250 ÷ 2 = 125                    │
│  │   ├─ Waves: 5 × 10 = 50                                │
│  │   ├─ Zone Bonus: 100 (held Zone 1)                     │
│  │   └─ Victory: 0 (died)                                  │
│  │   ────────────────────────────────                      │
│  │   Total: 275 Souls                                      │
│  │                                                           │
│  ├─→ CurrencyManager.AddMetaCurrency(275)                  │
│  │   └─→ Souls: 100 + 275 = 375 🔵                         │
│  │                                                           │
│  ├─→ SaveSystem.AddMetaCurrency(275)                       │
│  │   └─→ Saved to disk!                                    │
│  │                                                           │
│  └─→ SaveSystem.UpdateStatistics()                         │
│      ├─ Total Runs Failed: +1                              │
│      ├─ Highest Wave: Updated if needed                    │
│      └─ Saved to disk!                                     │
│                                                              │
│  Console Output:                                            │
│  ┌────────────────────────────────────────┐               │
│  │ === RUN COMPLETE ===                    │               │
│  │ Gold Earned This Run: 250               │               │
│  │ Souls (Meta-Currency) Earned: 275       │               │
│  │ Waves Completed: 5                      │               │
│  └────────────────────────────────────────┘               │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    GAME RESTART                              │
│                                                              │
│  User restarts the game (closes Unity, reopens)            │
│                                                              │
│  SaveSystem.LoadGame()                                      │
│  │                                                           │
│  └─→ Loads from disk:                                       │
│      ├─ Souls: 375 🔵 ✅ PERSISTED!                        │
│      ├─ Move Speed Level: 2 ✅                              │
│      ├─ Health Level: 1 ✅                                  │
│      └─ Damage Level: 4 ✅ (upgraded earlier)              │
│                                                              │
│  Console Output:                                            │
│  ┌────────────────────────────────────────┐               │
│  │ Save file path: .../savefile.json      │               │
│  │ Game loaded successfully!               │               │
│  │ Meta-currency: 375                      │               │
│  └────────────────────────────────────────┘               │
│                                                              │
│  Player starts with:                                        │
│  ├─ Gold: 0 🟡 (reset)                                     │
│  ├─ Souls: 375 🔵 (persisted)                              │
│  └─ All upgrades applied ✅                                │
└─────────────────────────────────────────────────────────────┘

```

---

## 💰 Currency Comparison

### Gold (In-Run Currency) 🟡

```
Earned: During gameplay
Source: Enemy kills, wave completion
Used For: Obstacles (future)
Resets: Every run
Persists: NO
Display: Yellow/Gold color
Range: 0 - ~1000 per run
```

**Lifecycle:**
```
Run Start → 0
Kill Enemies → Increases
Return to Base → Unchanged
Run Ends → Calculated for Soul conversion
Next Run Start → Reset to 0
```

---

### Souls (Meta-Currency) 🔵

```
Earned: End of run only
Source: Performance rewards
Used For: Permanent upgrades
Resets: Never
Persists: YES (saved to disk)
Display: Cyan/Blue color
Range: 0 - Unlimited (accumulates)
```

**Lifecycle:**
```
Game Start → Load from save
During Run → Unchanged
Return to Base → Can spend in shop
Run Ends → Earn more souls
Next Run Start → Same amount (unless spent)
Game Restart → Persists
```

---

## 🏪 Shop Flow

```
Player Interacts with NPC
         │
         ▼
┌────────────────────┐
│  ShopNPC.OnInteract│
└────────────────────┘
         │
         ▼
┌────────────────────┐
│ SimpleShopUI.Open  │
│                    │
│ Display Items:     │
│ ┌────────────────┐ │
│ │ Damage Upgrade │ │
│ │ Level: 4       │ │
│ │ Cost: 50 Souls │ │ ← Uses SOULS, not Gold!
│ └────────────────┘ │
│                    │
│ Currency Display:  │
│ Souls: 375 🔵      │ ← Shows SOULS only
└────────────────────┘
         │
         ▼
Player Clicks "Buy"
         │
         ▼
┌─────────────────────────┐
│ UpgradeShop.TryPurchase │
└─────────────────────────┘
         │
         ▼
┌──────────────────────────────┐
│ CurrencyManager.SpendMeta... │
│ ├─ Check: 375 >= 50? ✅      │
│ ├─ Deduct: 375 - 50 = 325    │
│ └─ Trigger OnMetaChanged     │
└──────────────────────────────┘
         │
         ▼
┌──────────────────────────────┐
│ PlayerStats.UpgradeDamage()  │
│ └─ Level: 4 → 5              │
└──────────────────────────────┘
         │
         ▼
┌──────────────────────────────┐
│ SaveSystem.SaveUpgradeLevels │
│ └─ Write to disk ✅          │
└──────────────────────────────┘
         │
         ▼
┌──────────────────────────────┐
│ Shop UI Refreshes            │
│ ├─ Show: Level 5             │
│ ├─ New Cost: 60 Souls        │
│ └─ Currency: 325 Souls       │
└──────────────────────────────┘
```

---

## 💾 Save File Structure

**Location:** `Application.persistentDataPath/savefile.json`

**Example Content:**
```json
{
  "metaCurrency": 375,
  "moveSpeedLevel": 2,
  "maxHealthLevel": 1,
  "damageLevel": 5,
  "critChanceLevel": 0,
  "critDamageLevel": 0,
  "attackRangeLevel": 1,
  "totalRunsCompleted": 3,
  "totalRunsFailed": 7,
  "totalEnemiesKilled": 152,
  "highestWaveReached": 8
}
```

**When Saved:**
- Game start (creates if missing)
- After meta-currency change
- After upgrade purchase
- After run complete
- On game quit

**When Loaded:**
- Game start (SaveSystem.Awake)
- Manual load (SaveSystem.LoadGame)

---

## 🎯 Key Takeaways

1. **Two separate currency systems:**
   - Gold = Temporary (resets)
   - Souls = Permanent (persists)

2. **Gold earned during gameplay:**
   - Immediately from enemy kills
   - Displayed in real-time

3. **Souls earned at run end:**
   - Based on performance
   - Saved immediately to disk

4. **Upgrades use Souls only:**
   - Shop displays Souls
   - Permanent progression
   - Saved after purchase

5. **Everything persists:**
   - Meta-currency (Souls)
   - Upgrade levels
   - Statistics
   - Automatically saved

---

## 🔍 Debugging Tips

**Check Currency Values:**
```csharp
// In Console or Debug script
Debug.Log($"Gold: {CurrencyManager.Instance.Gold}");
Debug.Log($"Souls: {CurrencyManager.Instance.MetaCurrency}");
```

**Force Save:**
```csharp
SaveSystem.Instance.SaveGame();
```

**Check Save File Location:**
```csharp
Debug.Log(Application.persistentDataPath);
// Open this folder and look for savefile.json
```

**Reset Save Data:**
```csharp
SaveSystem.Instance.ResetSave();
// Or manually delete savefile.json
```

---

For implementation details, see:
- `CURRENCY_SYSTEM_SETUP.md` - Setup guide
- `SETUP_CHECKLIST.md` - Step-by-step checklist
- `CURRENCY_SYSTEM_SUMMARY.md` - Quick reference
