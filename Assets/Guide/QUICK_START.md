# Quick Start Guide - Upgrade System

## 🚀 Get Running in 10 Minutes

Follow these steps to get the upgrade system working in your game:

---

## Step 1: Create Manager GameObject (2 min)

1. **Create Empty GameObject:**
   - Hierarchy → Right-click → Create Empty
   - Name it: `GameManagers`

2. **Add Manager Components:**
   - Select GameManagers
   - Add Component → `GameProgressionManager`
   - Add Component → `PlayerStats`
   - Add Component → `WeaponSystem`
   - Add Component → `UpgradeShop`

3. **Configure GameProgressionManager:**
   - Base Timer Duration: `40` (seconds)
   - Max Defense Zones: `3`

**Result:** ✅ Core managers are ready

---

## Step 2: Create Defense Zones (3 min)

1. **Create Zone 1 (Frontline):**
   - Create Empty: `DefenseZone_1`
   - Position: `(0, 0, -30)` (far from base)
   - Add Component: `DefenseZone`
     - Zone Index: `0`
     - Spawn Radius: `15`
     - Damage Bonus: `0`
     - Attack Speed Bonus: `0`
     - Move Speed Bonus: `0`

2. **Create Spawn Center:**
   - Create child empty: `SpawnCenter`
   - Assign in DefenseZone → Spawn Center

3. **Create Zone 2 (Mid):**
   - Duplicate Zone 1, rename `DefenseZone_2`
   - Position: `(0, 0, 0)`
   - Zone Index: `1`
   - Damage Bonus: `0.25` (25% bonus)
   - Attack Speed Bonus: `0.25`
   - Move Speed Bonus: `0.25`

4. **Create Zone 3 (Base):**
   - Duplicate Zone 2, rename `DefenseZone_3`
   - Position: `(0, 0, 30)` (near base)
   - Zone Index: `2`
   - Damage Bonus: `0.5` (50% bonus)
   - Attack Speed Bonus: `0.5`
   - Move Speed Bonus: `0.5`

5. **Link Zones:**
   - Select DefenseZone_1 → Next Zone: DefenseZone_2
   - Select DefenseZone_2 → Next Zone: DefenseZone_3
   - Select DefenseZone_3 → Next Zone: None

**Result:** ✅ 3 defense locations with perks ready

---

## Step 3: Create Base with Gate (3 min)

1. **Create Base Ground:**
   - Create → 3D Object → Cube
   - Name: `Base_Ground`
   - Position: `(0, 0, 40)`
   - Scale: `(20, 0.5, 15)`

2. **Create Gate:**
   - Create → 3D Object → Cube
   - Name: `Base_Gate`
   - Position: `(0, 2.5, 32)` (entrance to base)
   - Scale: `(15, 5, 0.5)`
   - Add Component: `BaseGate`
     - Gate Visual: Assign itself
     - Gate Collider: Its Box Collider
     - Starts Open: `✓` checked
     - Open Height: `5`
     - Animation Speed: `2`

3. **Create Entrance Trigger:**
   - Create → 3D Object → Cube
   - Name: `Base_Trigger`
   - Position: `(0, 1, 33)`
   - Scale: `(15, 3, 3)`
   - Add Component: `BaseTrigger`
     - Is Entrance: `✓` checked
   - Box Collider:
     - Is Trigger: `✓` checked
   - **Delete MeshRenderer** (make invisible)

**Result:** ✅ Base area with working gate and trigger

---

## Step 4: Update Enemy Prefab (1 min)

1. **Open Enemy Prefab:** `/Assets/Prefabs/Enemies/Enemy.prefab`
2. **Select Enemy (root)**
3. **Find EnemyHealth component**
4. **Set Currency Reward:** `25` (adjust based on difficulty)
5. **Save Prefab**

**Result:** ✅ Enemies now drop currency

---

## Step 5: Test Without UI (1 min)

1. **Press Play**
2. **Open Console Window**
3. **Walk into Base_Trigger**
4. **Watch Console for:**
   - `"Shop opened! Time to upgrade."`
   - `"Gate opened!"`
5. **Kill enemies and watch:**
   - Currency increases in Console
6. **Walk out of base:**
   - `"Gate closed - wave started!"`

**Result:** ✅ Core systems working!

---

## Optional: Create Example Upgrade Assets

You'll need these for the shop to sell anything.

### Create Upgrade: Move Speed
1. **Project Window → Right-click**
2. **Create → Game → Upgrade Data**
3. **Name:** `Upgrade_MoveSpeed`
4. **Configure:**
   - Upgrade Name: `"Swift Feet"`
   - Description: `"Increases movement speed"`
   - Upgrade Type: `MoveSpeed`
   - Base Cost: `50`
   - Cost Increase Per Level: `1.5`
   - Max Level: `10`

### Create Upgrade: Max Health
1. **Create → Game → Upgrade Data**
2. **Name:** `Upgrade_MaxHealth`
3. **Configure:**
   - Upgrade Name: `"Iron Body"`
   - Description: `"Increases maximum health"`
   - Upgrade Type: `MaxHealth`
   - Base Cost: `75`
   - Cost Increase Per Level: `1.5`
   - Max Level: `10`

### Create Weapon: Basic Sword
1. **Create → Game → Weapon Data**
2. **Name:** `Weapon_BasicSword`
3. **Configure:**
   - Weapon Name: `"Iron Sword"`
   - Description: `"A reliable blade"`
   - Damage Multiplier: `1`
   - Attack Speed Multiplier: `1`
   - Range Multiplier: `1`
   - Weapon Effect: `None`
   - Purchase Cost: `0` (starter weapon)

### Create Weapon: Fire Blade
1. **Create → Game → Weapon Data**
2. **Name:** `Weapon_FireBlade`
3. **Configure:**
   - Weapon Name: `"Flame Sword"`
   - Description: `"Burns enemies"`
   - Damage Multiplier: `1.2`
   - Attack Speed Multiplier: `1`
   - Range Multiplier: `1`
   - Weapon Effect: `Burn`
   - Effect Chance: `0.3` (30%)
   - Effect Value: `5` (burn damage)
   - Purchase Cost: `200`

### Link to Shop
1. **Select GameManagers**
2. **UpgradeShop component**
3. **Available Upgrades → Size: 2**
   - Element 0: Drag `Upgrade_MoveSpeed`
   - Element 1: Drag `Upgrade_MaxHealth`
4. **Available Weapons → Size: 2**
   - Element 0: Drag `Weapon_BasicSword`
   - Element 1: Drag `Weapon_FireBlade`

**Result:** ✅ Shop has items to sell

---

## 🎮 Manual Testing Commands

While game is running, test systems manually:

### Give Currency
1. **Pause Game** (or play)
2. **Select GameManagers** in Hierarchy
3. **GameProgressionManager → Right-click component**
4. **Debug → Add Currency**
5. **Watch Console:** Currency changes

### Apply Upgrades
1. **Select GameManagers**
2. **PlayerStats → Right-click component**
3. **Try methods:**
   - `UpgradeMoveSpeed()`
   - `UpgradeDamage()`
   - `UpgradeMaxHealth()`
4. **Watch Player move faster / hit harder!**

### Equip Weapon
1. **Select GameManagers**
2. **WeaponSystem → Equipped Weapon**
3. **Drag a WeaponData asset**
4. **Attack enemies** → damage changes

---

## 🐛 Common Issues

### "Gate not moving"
- Check BaseGate → Gate Visual is assigned
- Check gate has a visual mesh (cube)
- Check Animation Speed > 0

### "No currency on kill"
- Check Enemy prefab → EnemyHealth → Currency Reward > 0
- Check GameProgressionManager exists in scene
- Check Console for currency messages

### "Can't buy upgrades"
- Check UpgradeShop → Available Upgrades array has items
- Check you have enough currency
- Check upgrade isn't at max level

### "Stats not applying"
- Check PlayerStats component exists
- Check Player has PlayerController, PlayerHealth, PlayerCombat
- Check Player GameObject tag is "Player"

### "Player falls through base"
- Check Base_Ground has a Box Collider
- Check collider is NOT trigger
- Check Player has CharacterController

---

## 📊 Test Checklist

- [ ] GameManagers exists with all 4 manager components
- [ ] 3 Defense Zones exist and are linked
- [ ] Base Gate opens when entering trigger
- [ ] Currency increases when killing enemies
- [ ] Console shows "Shop opened" message
- [ ] Gate closes after 40 seconds or leaving base
- [ ] Player stats increase when calling upgrade methods
- [ ] Weapon damage multiplies when equipped

---

## 🎯 Next: Create Shop UI

Once core systems work, create UI:

1. **Create Canvas** (UI → Canvas)
2. **Add Currency Text** (top-right)
3. **Add Timer Text** (top-center)
4. **Add Shop Panel** (hidden by default)
5. **Add Upgrade Buttons** (grid layout)
6. **Wire buttons to UpgradeShop methods**

See `UPGRADE_SYSTEM_GUIDE.md` for UI integration details.

---

## 🎉 You're Ready!

You now have:
- ✅ Currency economy
- ✅ Between-wave shop system
- ✅ Base safe zone with gates
- ✅ 3 defense zones with fallback
- ✅ Player stat upgrades
- ✅ Weapon system with effects

**Test it, tweak values in Inspector, and build your UI!**
