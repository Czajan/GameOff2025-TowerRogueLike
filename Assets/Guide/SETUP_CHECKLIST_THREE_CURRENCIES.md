# Three-Currency System - Setup Checklist

Use this checklist to set up your three-currency system step by step.

---

## ✅ Step 1: Create XP Orb Prefab

### Visual Setup
- [ ] Create new GameObject: `XP_Orb`
- [ ] Add visual component (choose one):
  - [ ] 3D: Sphere mesh (scale 0.3-0.5) with emissive material
  - [ ] 2D: Sprite with glow effect
- [ ] Add `ExperienceOrb` script component
- [ ] Configure script values:
  - [ ] XP Value: `10`
  - [ ] Fly Speed: `10`
  - [ ] Collection Distance: `0.5`
  - [ ] Rotation Speed: `180`
  - [ ] Bob Speed: `2`
  - [ ] Bob Height: `0.3`

### Optional Polish
- [ ] Add Particle System (trail effect)
- [ ] Add Point Light (cyan, intensity 2, range 3)
- [ ] Add audio source for collection sound

### Save Prefab
- [ ] Save as `/Assets/Prefabs/XP_Orb.prefab`
- [ ] Test: Drag into scene, verify it exists and has script

---

## ✅ Step 2: Configure Enemy Prefabs

For **each** enemy prefab in your project:

- [ ] Open enemy prefab
- [ ] Find `EnemyHealth` component
- [ ] Set `Gold Reward`: `5` (adjust per enemy type)
- [ ] Set `XP Reward`: `10` (currently unused, for future)
- [ ] Assign `XP Orb Prefab`: Drag `XP_Orb` prefab here
- [ ] Save prefab

**Repeat for all enemy types:**
- [ ] Basic Enemy
- [ ] Elite Enemy  
- [ ] Boss Enemy
- [ ] (Any other enemy variants)

---

## ✅ Step 3: Add ExperienceSystem to Scene

- [ ] Create empty GameObject: `ExperienceSystem`
- [ ] Add `ExperienceSystem` script component
- [ ] Configure Inspector values:
  - [ ] Current Level: `1`
  - [ ] Current XP: `0`
  - [ ] Base XP Required: `100`
  - [ ] XP Scaling Per Level: `1.15`
- [ ] Save scene

---

## ✅ Step 4: Create Level-Up UI

### Create UI Panel
- [ ] In Hierarchy, create UI > Canvas (if not exists)
- [ ] Create child Panel: `LevelUpPanel`
- [ ] Set Panel to fill screen (or desired size)
- [ ] Set Panel background alpha to semi-transparent (e.g., 0.8)

### Add UI Components to Panel
- [ ] Add TextMeshProUGUI child: `LevelTitle`
  - [ ] Set font size: `36-48`
  - [ ] Set alignment: Center
  - [ ] Position at top of panel
  
- [ ] Add ScrollView or VerticalLayoutGroup child: `OptionsContainer`
  - [ ] This will hold the option buttons
  - [ ] Add Content Size Fitter (if using ScrollView)
  - [ ] Add Vertical Layout Group component
    - [ ] Spacing: `10-20`
    - [ ] Child Alignment: Upper Center
    - [ ] Child Force Expand: Width ✅, Height ❌

### Create Option Button Prefab
- [ ] Create Button: `LevelUpOptionButton`
- [ ] Add child TextMeshProUGUI: `Name`
  - [ ] Font size: `24`
  - [ ] Alignment: Left
- [ ] Add child TextMeshProUGUI: `Description`
  - [ ] Font size: `16`
  - [ ] Alignment: Left
  - [ ] Color: Lighter gray
- [ ] Add child TextMeshProUGUI: `Value`
  - [ ] Font size: `28`
  - [ ] Alignment: Right
  - [ ] Color: Green/Cyan
- [ ] Layout: Use Horizontal/Vertical Layout Groups as needed
- [ ] Save as `/Assets/Prefabs/UI/LevelUpOptionButton.prefab`
- [ ] Delete from scene

### Add LevelUpUI Script
- [ ] Select `LevelUpPanel`
- [ ] Add `LevelUpUI` script component
- [ ] Assign references:
  - [ ] Level Up Panel: `LevelUpPanel` (self)
  - [ ] Level Title Text: `LevelTitle`
  - [ ] Options Container: `OptionsContainer`
  - [ ] Option Button Prefab: `LevelUpOptionButton` prefab

### Disable Panel by Default
- [ ] Uncheck `LevelUpPanel` GameObject (disable it)

---

## ✅ Step 5: Configure Level-Up Options

### Small Boosts (Normal Levels)
In `LevelUpUI` Inspector, expand `Small Boosts` array:

- [ ] Set Size: `6` (or more for variety)
- [ ] Configure options:

**Option 0:**
- [ ] Upgrade Name: `"Damage +"`
- [ ] Description: `"Increase base damage"`
- [ ] Stat Type: `Damage`
- [ ] Value: `5`

**Option 1:**
- [ ] Upgrade Name: `"Health +"`
- [ ] Description: `"Increase max health"`
- [ ] Stat Type: `MaxHealth`
- [ ] Value: `20`

**Option 2:**
- [ ] Upgrade Name: `"Speed +"`
- [ ] Description: `"Move faster"`
- [ ] Stat Type: `MoveSpeed`
- [ ] Value: `10` (= +10%)

**Option 3:**
- [ ] Upgrade Name: `"Critical +"`
- [ ] Description: `"Increase crit chance"`
- [ ] Stat Type: `CritChance`
- [ ] Value: `5` (= +5%)

**Option 4:**
- [ ] Upgrade Name: `"Crit Damage +"`
- [ ] Description: `"Increase crit damage multiplier"`
- [ ] Stat Type: `CritDamage`
- [ ] Value: `15` (= +15%)

**Option 5:**
- [ ] Upgrade Name: `"Attack Speed +"`
- [ ] Description: `"Attack faster"`
- [ ] Stat Type: `AttackSpeed`
- [ ] Value: `10` (= +10%)

### Milestone Boosts (Levels 5, 10, 15...)
Expand `Milestone Boosts` array:

- [ ] Set Size: `6`
- [ ] Configure stronger options:

**Option 0:**
- [ ] Upgrade Name: `"★ MASSIVE DAMAGE ★"`
- [ ] Description: `"Greatly increase damage"`
- [ ] Stat Type: `Damage`
- [ ] Value: `25`

**Option 1:**
- [ ] Upgrade Name: `"★ TANK ★"`
- [ ] Description: `"Massive health increase"`
- [ ] Stat Type: `MaxHealth`
- [ ] Value: `100`

**Option 2:**
- [ ] Upgrade Name: `"★ SPEED DEMON ★"`
- [ ] Description: `"Much faster movement"`
- [ ] Stat Type: `MoveSpeed`
- [ ] Value: `30`

**Option 3:**
- [ ] Upgrade Name: `"★ CRITICAL MASTER ★"`
- [ ] Description: `"Huge crit chance boost"`
- [ ] Stat Type: `CritChance`
- [ ] Value: `15`

**Option 4:**
- [ ] Upgrade Name: `"★ BERSERKER ★"`
- [ ] Description: `"Devastating critical hits"`
- [ ] Stat Type: `CritDamage`
- [ ] Value: `50`

**Option 5:**
- [ ] Upgrade Name: `"★ RAPID FIRE ★"`
- [ ] Description: `"Massive attack speed"`
- [ ] Stat Type: `AttackSpeed`
- [ ] Value: `25`

---

## ✅ Step 6: Setup HUD (In-Run UI)

### Add Currency Display
- [ ] Find or create HUD Canvas
- [ ] Add empty GameObject: `CurrencyDisplay_HUD`
- [ ] Add `CurrencyDisplay` script
- [ ] Add TextMeshProUGUI child: `GoldText`
  - [ ] Position: Top-left
  - [ ] Anchor: Top-Left
  - [ ] Font size: `20-24`
- [ ] Add TextMeshProUGUI child: `XPText`
  - [ ] Position: Below GoldText
  - [ ] Font size: `18-20`
- [ ] Configure `CurrencyDisplay` script:
  - [ ] Gold Text: Assign `GoldText`
  - [ ] Experience Text: Assign `XPText`
  - [ ] Essence Text: (leave empty)
  - [ ] Show Gold: ✅
  - [ ] Show Experience: ✅
  - [ ] Show Essence: ❌

### Add Experience Bar
- [ ] Create UI > Image: `ExperienceBar_Background`
  - [ ] Position: Bottom of screen
  - [ ] Anchor: Bottom Stretch
  - [ ] Height: `30-40`
  - [ ] Color: Dark gray/black
- [ ] Add child Image: `ExperienceBar_Fill`
  - [ ] Image Type: `Filled`
  - [ ] Fill Method: `Horizontal`
  - [ ] Fill Amount: `0`
  - [ ] Color: Cyan/Blue
- [ ] Add child TextMeshProUGUI: `LevelText`
  - [ ] Anchor: Left
  - [ ] Text: `"Level 1"`
  - [ ] Font size: `18`
- [ ] Add child TextMeshProUGUI: `XPAmountText`
  - [ ] Anchor: Right
  - [ ] Text: `"0 / 100"`
  - [ ] Font size: `16`
- [ ] Add `ExperienceBar` script to `ExperienceBar_Background`
- [ ] Configure script:
  - [ ] Fill Image: Assign `ExperienceBar_Fill`
  - [ ] Level Text: Assign `LevelText`
  - [ ] XP Text: Assign `XPAmountText`
  - [ ] Normal Color: Cyan `(0.2, 0.8, 1)`
  - [ ] Milestone Color: Gold `(1, 0.84, 0)`
  - [ ] Smooth Fill: ✅
  - [ ] Fill Speed: `5`

---

## ✅ Step 7: Setup Base/Menu UI

### Add Essence Display
- [ ] Find or create Base/Menu Canvas
- [ ] Add TextMeshProUGUI: `EssenceText`
  - [ ] Position: Top-right or top-center
  - [ ] Font size: `28-32`
  - [ ] Text: `"Essence: 0"`
- [ ] Add empty GameObject: `CurrencyDisplay_Base`
- [ ] Add `CurrencyDisplay` script
- [ ] Configure script:
  - [ ] Gold Text: (leave empty)
  - [ ] Experience Text: (leave empty)
  - [ ] Essence Text: Assign `EssenceText`
  - [ ] Show Gold: ❌
  - [ ] Show Experience: ❌
  - [ ] Show Essence: ✅

---

## ✅ Step 8: Configure Essence Rewards

- [ ] Select `GameProgressionManager` in scene
- [ ] Find "Essence Rewards (Meta-Currency)" section
- [ ] Configure values:
  - [ ] Essence Per Wave: `10` (test and adjust)
  - [ ] Essence For Victory: `200`
  - [ ] Essence Zone 1 Bonus: `100`
  - [ ] Essence Zone 2 Bonus: `50`
  - [ ] Essence Zone 3 Bonus: `25`
  - [ ] Minimum Essence Reward: `10`
- [ ] Save scene

---

## ✅ Step 9: Test Gold System

- [ ] Enter Play mode
- [ ] Kill an enemy
- [ ] Verify:
  - [ ] Gold counter increases in HUD
  - [ ] Console shows: `"+X Gold"`
  - [ ] Value matches enemy's `goldReward`
- [ ] Start new run
- [ ] Verify Gold resets to 0

---

## ✅ Step 10: Test Experience System

- [ ] Enter Play mode
- [ ] Kill an enemy
- [ ] Verify:
  - [ ] XP orb spawns at enemy position
  - [ ] Orb rotates and bobs visually
  - [ ] Orb flies toward player
  - [ ] Orb is collected when near player
  - [ ] XP bar fills
  - [ ] Console shows: `"+X XP"`
- [ ] Kill enough enemies to level up
- [ ] Verify:
  - [ ] Game pauses (`Time.timeScale = 0`)
  - [ ] Level-up panel appears
  - [ ] 3 random options display
  - [ ] Option buttons are clickable
- [ ] Click an option
- [ ] Verify:
  - [ ] Panel closes
  - [ ] Game resumes (`Time.timeScale = 1`)
  - [ ] Stat boost is applied (check stats in Inspector or gameplay)
- [ ] Continue to level 5
- [ ] Verify:
  - [ ] XP bar turns gold color (milestone)
  - [ ] Milestone options appear (stronger boosts)

---

## ✅ Step 11: Test Essence System

- [ ] Enter Play mode
- [ ] Complete or fail a run
- [ ] Verify Console shows:
  ```
  === RUN COMPLETE ===
  Gold Earned This Run: X
  Essence (Meta-Currency) Earned: Y
  Waves Completed: Z
  ```
- [ ] Check Essence calculation matches expected formula
- [ ] Return to Base/Menu
- [ ] Verify:
  - [ ] Essence displayed correctly in UI
  - [ ] Essence value matches Console log
- [ ] Purchase an upgrade
- [ ] Verify:
  - [ ] Essence decreases
  - [ ] Purchase works correctly
- [ ] Exit Play mode
- [ ] Re-enter Play mode
- [ ] Verify:
  - [ ] Essence persisted (didn't reset)
  - [ ] Upgraded stats still saved

---

## ✅ Step 12: Test Persistence

- [ ] Enter Play mode
- [ ] Earn some Essence
- [ ] Note current Essence value
- [ ] Exit Play mode
- [ ] Check Console for save file path
- [ ] Navigate to `Application.persistentDataPath`
- [ ] Verify `savefile.json` exists
- [ ] Open `savefile.json`
- [ ] Verify it contains `"essence": X`
- [ ] Re-enter Play mode
- [ ] Verify Essence value matches saved value
- [ ] Spend Essence on upgrades
- [ ] Exit and re-enter Play mode
- [ ] Verify upgrades persisted

---

## ✅ Step 13: Balance Tuning

### XP Curve Tuning
- [ ] Playtest: Are you leveling too fast or too slow?
- [ ] Adjust `ExperienceSystem` values:
  - [ ] Increase `baseXPRequired` for slower leveling
  - [ ] Increase `xpScalingPerLevel` for steeper curve
  - [ ] Decrease for faster progression
- [ ] Recommended: 5-8 levels in a typical successful run

### XP Rewards per Enemy
- [ ] Low-tier enemies: `10 XP`
- [ ] Mid-tier enemies: `25 XP`
- [ ] Elite enemies: `50 XP`
- [ ] Bosses: `100-200 XP`
- [ ] Adjust `xpValue` in `ExperienceOrb` script (or make it configurable per enemy)

### Essence Rewards Tuning
- [ ] Playtest: Earning enough Essence?
- [ ] Adjust `essencePerWave` to tune pace
- [ ] Recommended: Players should afford 1-2 upgrades per successful run
- [ ] Make upgrades cost 50-300 Essence each

### Level-Up Rewards Tuning
- [ ] Playtest: Are boosts too weak or too strong?
- [ ] Small boosts should feel helpful but not game-breaking
- [ ] Milestone boosts should feel powerful and exciting
- [ ] Recommended ratio: Milestone = 3-5× Small boost value

---

## ✅ Step 14: Polish (Optional)

### Visual Polish
- [ ] Add particles to XP orb (trail effect)
- [ ] Add glow/light to XP orb
- [ ] Animate level-up panel (slide in, scale)
- [ ] Add screen flash on level-up
- [ ] Add XP bar pulse animation when near full
- [ ] Add "+XP" floating text on orb collection

### Audio Polish
- [ ] XP orb collection sound effect
- [ ] Level-up fanfare sound
- [ ] Milestone level-up special sound
- [ ] UI button click sounds
- [ ] Essence reward jingle

### Feedback Polish
- [ ] Screen shake on level-up
- [ ] Particle burst when choosing upgrade
- [ ] Visual indicator of applied stat boost
- [ ] Hover tooltips on upgrade options

---

## 🎯 Final Verification

### Core Systems
- [ ] No compilation errors in Console
- [ ] CurrencyManager exists in scene
- [ ] SaveSystem exists in scene
- [ ] ExperienceSystem exists in scene
- [ ] PlayerStats exists in scene
- [ ] GameProgressionManager exists in scene

### Prefabs
- [ ] XP Orb prefab created and functional
- [ ] All enemy prefabs have XP Orb assigned
- [ ] Level-up option button prefab created

### UI
- [ ] HUD shows Gold + XP during runs
- [ ] Experience bar fills and resets correctly
- [ ] Level-up UI appears and works
- [ ] Base UI shows Essence
- [ ] Shop uses Essence for purchases

### Data Flow
- [ ] Gold awards correctly
- [ ] XP orbs spawn and fly
- [ ] Leveling works and pauses game
- [ ] Stat boosts apply
- [ ] Essence calculates correctly
- [ ] Essence persists between sessions
- [ ] Upgrades persist

---

## 📚 Documentation Reference

All documentation is located in `/Assets/Guide/`:

- [ ] `THREE_CURRENCY_SYSTEM_GUIDE.md` - Full implementation guide
- [ ] `MIGRATION_TO_THREE_CURRENCIES.md` - Migration from old system
- [ ] `CURRENCY_QUICK_REFERENCE.md` - API quick reference
- [ ] `IMPLEMENTATION_COMPLETE.md` - Summary of what's implemented
- [ ] `SETUP_CHECKLIST_THREE_CURRENCIES.md` - This checklist

---

## ✅ Setup Complete!

Once all checkboxes are ✅, your three-currency system is fully operational!

**Next:** Playtest, balance, and add polish! 🎮🎉
