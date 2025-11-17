# Fix Level-Up UI Issues

## 🐛 Issues Identified

From your screenshot, I identified these problems:
1. ✗ White box blocking the upgrades (OptionsContainer background)
2. ✗ Wrong button prefab (ItemButton instead of UpgradeButton)
3. ✗ Layout spacing too tight
4. ✗ Panel too small

## ⚡ One-Click Fix

```
Tools > Fix Level-Up UI Layout
```

This will automatically:
- ✓ Make container background transparent
- ✓ Switch to UpgradeButton prefab
- ✓ Fix layout spacing
- ✓ Increase panel size
- ✓ Enhance title styling

## 🧪 Test After Fix

1. **Enter Play Mode**
2. **Kill enemies** to reach level 5
3. **Upgrade panel appears** - should now show 3 readable upgrade buttons!
4. **Click an upgrade** to select it

## 🎨 What You Should See

**Before (Your Screenshot):**
- White box blocking view
- Text hard to read
- Buttons overlapping

**After Fix:**
- Dark semi-transparent background
- 3 clear upgrade buttons vertically stacked
- Colored names by rarity (Gray/Blue/Gold)
- Readable descriptions with stat bonuses
- Stack counters like `[1/5]`

## 📋 Manual Fix (if tool doesn't work)

### 1. Fix OptionsContainer Background
- Select `/GameCanvas/LevelUpPanel/OptionsContainer`
- In Image component:
  - Color: Set Alpha to `0` (fully transparent)
  - Raycast Target: Uncheck

### 2. Change Button Prefab
- Select `/GameCanvas/LevelUpPanel`
- In LevelUpUI component:
  - Option Button Prefab: Drag `UpgradeButton.prefab` (not ItemButton)

### 3. Fix Layout Group
- Select `/GameCanvas/LevelUpPanel/OptionsContainer`
- In Vertical Layout Group:
  - Spacing: `20`
  - Child Alignment: `Middle Center`
  - Padding: `30` (all sides)
  - Child Force Expand: Both unchecked
  - Child Control: Both unchecked

### 4. Increase Panel Size
- Select `/GameCanvas/LevelUpPanel`
- In RectTransform:
  - Width: `900`
  - Height: `550`

## 🔍 Why This Happened

The LevelUpPanel was created earlier and configured with:
- ItemButton prefab (for shop items, shows "Essence" costs)
- White container background
- Tight layout spacing

The UpgradeButton prefab is specifically designed for level-up upgrades with:
- Larger size (700x120 vs 320x120)
- Better layout for stat descriptions
- No cost/currency display

## ✅ After Fix Checklist

- [ ] White box gone
- [ ] 3 upgrade buttons clearly visible
- [ ] Names color-coded by rarity
- [ ] Stat bonuses listed in description
- [ ] Stack counter shown (if stackable)
- [ ] Can click and select upgrades
- [ ] Panel closes after selection

---

**Run the fix tool and test! 🎮**
