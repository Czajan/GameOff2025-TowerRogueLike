# Upgrade System - 15 Minute Quick Start

## ⚡ Get It Working FAST

Follow these steps to have the upgrade system running in **15 minutes**:

---

## ✅ Step 1: Create 3 Test Upgrades (3 min)

### Common: Power Up
1. Right-click in Project → Create → Game → Upgrade
2. Name: `TestUpgrade_Damage`
3. Set values:
   - Upgrade Name: `Power Up`
   - Description: `Increase damage`
   - Rarity: `Common`
   - Damage Bonus: `5`
   - Can Stack: ✓
   - Max Stacks: `10`

### Rare: Double Jump
1. Create another: `TestUpgrade_DoubleJump`
2. Set values:
   - Upgrade Name: `Air Walker`
   - Description: `Jump again in mid-air`
   - Rarity: `Rare`
   - Grants Double Jump: ✓
   - Can Stack: ✗

### Legendary: God Mode
1. Create: `TestUpgrade_GodMode`
2. Set values:
   - Upgrade Name: `Divine Power`
   - Description: `Massive stat boost`
   - Rarity: `Legendary`
   - Damage Bonus: `20`
   - Max Health Bonus: `100`
   - Move Speed Bonus: `0.2`
   - Can Stack: ✗

---

## ✅ Step 2: Create UpgradeSystem Manager (2 min)

1. In Hierarchy, find `/GameManagers`
2. Right-click → Create Empty
3. Rename to `UpgradeSystem`
4. Add Component → `UpgradeSystem`
5. In Inspector:
   - Set `All Upgrades` size to `3`
   - Drag your 3 test upgrades into slots
   - `Upgrades Per Offer`: 3
   - `Common Weight`: 70
   - `Rare Weight`: 25
   - `Legendary Weight`: 5

---

## ✅ Step 3: Create Simple UI (5 min)

### Create Panel
1. In Hierarchy, `/GameCanvas`
2. Right-click → UI → Panel
3. Rename: `UpgradeSelectionPanel`
4. Configure:
   - Rect Transform: Stretch/Stretch (full screen)
   - Image: Black, Alpha 220
   - **Disable it** (uncheck top of Inspector)

### Create Title
1. Right-click `UpgradeSelectionPanel` → UI → Text - TextMeshPro
2. Rename: `TitleText`
3. Configure:
   - Anchor: Top Center
   - Pos Y: `-100`
   - Width: `800`, Height: `80`
   - Text: `"CHOOSE YOUR UPGRADE"`
   - Font Size: `36`
   - Alignment: Center

### Create Options Container
1. Right-click `UpgradeSelectionPanel` → Create Empty
2. Rename: `OptionsContainer`
3. Add Component → `Horizontal Layout Group`
4. Configure:
   - Spacing: `30`
   - Child Alignment: Middle Center
5. Rect Transform:
   - Anchor: Middle Center
   - Width: `1200`, Height: `400`

### Create 3 Upgrade Cards
For **each** card (do this 3 times):

1. Right-click `OptionsContainer` → UI → Button
2. Rename: `UpgradeOption1` (then 2, then 3)
3. Configure Button:
   - Width: `350`, Height: `400`
4. Delete the "Text (TMP)" child inside

**For EACH button, create these children:**

**NameText:**
- Right-click button → UI → Text - TextMeshPro
- Name: `NameText`
- Anchor: Top Center
- Pos Y: `-50`
- Width: `320`, Height: `60`
- Font Size: `24`, Alignment: Center

**DescriptionText:**
- Right-click button → UI → Text - TextMeshPro
- Name: `DescriptionText`
- Anchor: Middle Center
- Width: `320`, Height: `200`
- Font Size: `16`, Alignment: Top Left
- Wrapping: Enabled

**RarityText:**
- Right-click button → UI → Text - TextMeshPro
- Name: `RarityText`
- Anchor: Top Right
- Pos X: `-10`, Pos Y: `-10`
- Width: `100`, Height: `30`
- Font Size: `14`, Alignment: Right

**IconImage** (optional):
- Right-click button → UI → Image
- Name: `IconImage`
- Anchor: Top Center
- Pos Y: `-120`
- Width: `80`, Height: `80`
- Disable it for now

**StackText:**
- Right-click button → UI → Text - TextMeshPro
- Name: `StackText`
- Anchor: Bottom Center
- Pos Y: `10`
- Width: `200`, Height: `30`
- Font Size: `14`, Alignment: Center
- Disable it for now

---

## ✅ Step 4: Add Scripts (3 min)

### Add UpgradeOptionUI to Each Card

For **each** of the 3 UpgradeOption buttons:

1. Select `UpgradeOption1`
2. Add Component → `UpgradeOptionUI`
3. Assign references:
   - **Select Button**: [the Button component]
   - **Background Image**: [the Image component on the button itself]
   - **Name Text**: [drag NameText child]
   - **Description Text**: [drag DescriptionText child]
   - **Rarity Text**: [drag RarityText child]
   - **Icon Image**: [drag IconImage child or leave empty]
   - **Stack Text**: [drag StackText child]

4. Repeat for UpgradeOption2 and UpgradeOption3

### Add UpgradeSelectionUI to Panel

1. Select `UpgradeSelectionPanel`
2. Add Component → `UpgradeSelectionUI`
3. Assign references:
   - **Selection Panel**: [drag UpgradeSelectionPanel itself]
   - **Title Text**: [drag TitleText]
   - **Upgrade Options** size: `3`
     - Element 0: [drag UpgradeOption1]
     - Element 1: [drag UpgradeOption2]
     - Element 2: [drag UpgradeOption3]
   - **Pause Game On Show**: ✓

---

## ✅ Step 5: Test! (2 min)

1. **Enter Play Mode**
2. **Kill enemies** to gain XP
3. **Reach level 5**
4. **Expected:**
   - Game pauses
   - Upgrade panel appears
   - 3 cards shown with your test upgrades
5. **Click one upgrade**
6. **Expected:**
   - Panel disappears
   - Game resumes
   - Stat increased!
7. **Press TAB** to see your updated stats

---

## 🎉 Done!

If everything works:
- ✅ Upgrades appear at level 5
- ✅ Game pauses during selection
- ✅ Clicking card applies upgrade
- ✅ Stats increase correctly

**Next:** Create more upgrades following the full setup guide!

---

## 🐛 Quick Troubleshooting

| Problem | Solution |
|---------|----------|
| No upgrades at level 5 | Check UpgradeSystem has upgrades assigned |
| Panel doesn't show | Make sure it's disabled initially, check UpgradeSelectionUI references |
| Can't click cards | Make sure Button components are enabled and references assigned |
| Stats don't change | Check Console for errors, verify PlayerStats.Instance exists |
| Game doesn't pause | Check "Pause Game On Show" is checked |
| Game doesn't resume | Check for script errors in Console |

---

## 🚀 You're Ready!

You now have a working upgrade system! Create more upgrades and experiment with different builds.

See the full guides for:
- More upgrade examples
- Better UI styling
- Special abilities
- Active skills from chests
