# Character Model Setup Guide - Synty Polygon Dungeon Realms

## Current Setup Overview

You have already added the **Synty Polygon Dungeon Realms** character model to your Player:
- Model: `Chr_Hero_Male_01` (already in `/Player/Model/Characters`)
- Animator: Already on `/Player/Model/Characters` GameObject
- Animations: Synty Animation Base Locomotion (Masculine) available

## Step-by-Step Setup

### ✅ Step 1: Create Animator Controller

1. In the **Project** window, navigate to `/Assets` folder
2. Right-click → **Create → Folder** → Name it `Animators` (if it doesn't exist)
3. Right-click in `/Assets/Animators` → **Create → Animator Controller**
4. Name it `PlayerAnimatorController`

### ✅ Step 2: Set Up Animation States

Open the `PlayerAnimatorController` (double-click) to open the **Animator** window.

#### Create Parameters:
1. Click the **Parameters** tab (left side)
2. Click **+** and add these parameters:
   - `Speed` (Float) - Default: 0
   - `Jump` (Trigger)
   - `IsGrounded` (Bool) - Default: true
   - `Attack` (Trigger)

#### Add Animation States:
1. Right-click in the Animator grid → **Create State → Empty**
2. Create these states (repeat for each):

**State: Idle**
- Name: `Idle`
- Motion: Navigate to `/Assets/3rd Party assets/Synty/AnimationBaseLocomotion/Animations/Polygon/Masculine/Idle/`
- Select: `A_Idle_Standing_Masc`

**State: Walk**
- Name: `Walk`
- Motion: `/Assets/3rd Party assets/Synty/AnimationBaseLocomotion/Animations/Polygon/Masculine/Locomotion/Walk/`
- Select: `A_Walk_F_Masc` (forward walk WITHOUT root motion)

**State: Run**
- Name: `Run`
- Motion: `/Assets/3rd Party assets/Synty/AnimationBaseLocomotion/Animations/Polygon/Masculine/Locomotion/Run/`
- Select: `A_Run_F_Masc` (forward run WITHOUT root motion)

**State: Jump**
- Name: `Jump`
- Motion: `/Assets/3rd Party assets/Synty/AnimationBaseLocomotion/Animations/Polygon/Masculine/InAir/`
- Select: `A_Jump_Idle_Masc`

#### Create Transitions:

**From Idle:**
- Right-click Idle → **Make Transition** → Click Walk
  - Condition: Speed > 0.1 AND Speed < 2.5
  - Has Exit Time: ✗ (unchecked)
  - Transition Duration: 0.25

- Right-click Idle → **Make Transition** → Click Run
  - Condition: Speed > 2.5
  - Has Exit Time: ✗ (unchecked)
  - Transition Duration: 0.25

- Right-click Idle → **Make Transition** → Click Jump
  - Condition: Jump (trigger)
  - Has Exit Time: ✗ (unchecked)
  - Transition Duration: 0.1

**From Walk:**
- Right-click Walk → **Make Transition** → Click Idle
  - Condition: Speed < 0.1
  - Has Exit Time: ✗ (unchecked)
  - Transition Duration: 0.25

- Right-click Walk → **Make Transition** → Click Run
  - Condition: Speed > 2.5
  - Has Exit Time: ✗ (unchecked)
  - Transition Duration: 0.25

**From Run:**
- Right-click Run → **Make Transition** → Click Walk
  - Condition: Speed < 2.5
  - Has Exit Time: ✗ (unchecked)
  - Transition Duration: 0.25

**From Jump:**
- Right-click Jump → **Make Transition** → Click Idle
  - Condition: IsGrounded (true)
  - Has Exit Time: ✓ (checked)
  - Exit Time: 0.9
  - Transition Duration: 0.25

### ✅ Step 3: Assign Animator Controller

1. In **Hierarchy**, select `/Player/Model/Characters`
2. In **Inspector**, find the **Animator** component
3. Drag `PlayerAnimatorController` to the **Controller** field
4. Set **Avatar**: Should auto-assign from the character FBX
   - If not, click the circle icon → Search for `Chr_Hero_Male_01Avatar`
5. Set **Apply Root Motion**: ✗ (unchecked) - we control movement via CharacterController
6. Set **Update Mode**: Normal
7. Set **Culling Mode**: Always Animate

### ✅ Step 4: Update PlayerController Script

The PlayerController needs to send animation parameters to the Animator.

**✓ ALREADY DONE!** I've updated your scripts:
- `PlayerController.cs` - Now updates Speed, IsGrounded, and Jump parameters
- `PlayerCombat.cs` - Now triggers Attack animation

### ✅ Step 5: Assign Animator Reference

1. In **Hierarchy**, select `/Player`
2. In **Inspector**, find the **Player Controller** component
3. Expand the **Animation** section
4. Drag `/Player/Model/Characters` from Hierarchy → **Animator** field

### ✅ Step 6: Materials Setup

Your character model already has materials! Let me check if they're rendering correctly:

**Option A: Materials Already Assigned**
- If the character looks good in the Scene view, you're done! ✓

**Option B: Missing/Pink Materials**
1. Navigate to `/Assets/3rd Party assets/Synty/PolygonDungeonRealms/Materials/`
2. Find the character material (usually `PolygonDungeonRealms_Character_01_Mat`)
3. Select `/Player/Model/Characters/Chr_Hero_Male_01` in Hierarchy
4. In Inspector, find **Skinned Mesh Renderer** → **Materials**
5. Drag the material from the folder into the Material slot

**Option C: Extract Materials from FBX**
1. In Project window, navigate to `/Assets/3rd Party assets/Synty/PolygonDungeonRealms/Models/Characters/`
2. Click on `Characters.fbx`
3. In Inspector, go to the **Materials** tab
4. Click **Extract Materials**
5. Choose `/Assets/Materials/` as destination
6. Materials will now be editable

### ✅ Step 7: Test the Setup

**Enter Play Mode and test:**
- ✓ **Idle**: Stand still → Should play idle animation
- ✓ **Walk**: Press WASD (slow) → Should play walk animation
- ✓ **Run**: Hold Shift + WASD → Should play run animation  
- ✓ **Jump**: Press Space → Should play jump animation
- ✓ **Attack**: Left Mouse Click → Should trigger attack (when you add attack animations)

### 🎯 Optional: Add Attack Animations

For combat, you'll need to either:

**Option 1: Find Combat Animations**
- Search Unity Asset Store for "Polygon Combat Animations" by Synty
- Or use free animations from Mixamo

**Option 2: Create Attack State (Placeholder)**
1. In Animator window, create a new state called `Attack`
2. Use any animation temporarily (e.g., `A_Idle_Standing_Masc`)
3. Create transition from **Any State → Attack**
   - Condition: Attack (trigger)
   - Has Exit Time: ✓ (checked)
   - Exit Time: 0.8
4. This will play a basic animation when attacking

---

## 🔧 Troubleshooting

### Character Model Not Visible
- Check if the SkinnedMeshRenderer is enabled on `/Player/Model/Characters/Chr_Hero_Male_01`
- Verify materials are assigned (not pink/missing)
- Check if the Model GameObject is active in hierarchy

### Animations Not Playing
- Make sure `PlayerAnimatorController` is assigned on the Animator component
- Check if Avatar is assigned (should be `Chr_Hero_Male_01Avatar`)
- Verify Apply Root Motion is **unchecked**
- Check Console for errors

### Character Sliding/Not Matching Movement
- This is normal! The animations don't have root motion
- Movement is controlled by `CharacterController`, animations are just visual
- Adjust animation speeds in Animator states if needed

### Character Floating or Clipping Ground
- Select `/Player` in Hierarchy
- Check **Character Controller** component
- Adjust **Center** Y value (should be around 1.0 for height 2.0)
- Adjust **Height** to match your character (try 1.8-2.0)

### Wrong Orientation/Rotation
- The character model might be facing the wrong direction
- Select `/Player/Model` in Hierarchy
- Set Transform Rotation Y to 0, 90, 180, or 270 to fix orientation

---

## 📋 Quick Reference

### Animation Parameters
- `Speed` (Float): 0 = idle, 0.1-2.5 = walk, 2.5+ = run
- `Jump` (Trigger): Triggers jump animation
- `IsGrounded` (Bool): True when on ground
- `Attack` (Trigger): Triggers attack animation (when added)

### Key Files
- Animator Controller: `/Assets/Animators/PlayerAnimatorController.controller`
- Character Model: `/Player/Model/Characters`
- Animation Files: `/Assets/3rd Party assets/Synty/AnimationBaseLocomotion/Animations/Polygon/Masculine/`

### Synty Animation Naming
- `A_` = Animation
- `_Masc` = Masculine
- `_Femn` = Feminine  
- `_RootMotion` = Animation moves the character (DON'T use these)
- Without RootMotion = Character stays in place (USE these)

---

## 🎨 Next Steps

1. **Set up Animator Controller** (follow Step 1-3 above)
2. **Assign Animator reference** on Player (Step 5)
3. **Test in Play Mode** (Step 7)
4. **Add attack animations** when ready (Optional)
5. **Customize materials/colors** if desired

---

**You're all set! Your character model is ready to be animated! 🎉**

