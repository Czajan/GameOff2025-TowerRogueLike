# Quick Manual Setup - Level-Up UI

## ✅ Step-by-Step (3 Minutes)

### **Step 1: Setup Level-Up UI Panel**
1. In Unity menu: **Tools > Setup Level-Up UI Panel**
2. Click **OK** when dialog appears
3. This will:
   - Configure the LevelUpPanel layout and styling
   - Create UpgradeButton prefab
   - Assign all references automatically

### **Step 2: Generate Upgrade Assets**
1. In Unity menu: **Tools > Generate Level Upgrade Assets**
2. Click **OK** when dialog appears
3. Check `/Assets/Data/LevelUpgrades/` - you should see 15 `.asset` files

**If tools don't appear:**
- Wait for Unity to finish compiling (check bottom-right corner)
- Check Console for errors

---

### **Step 3: Assign Upgrades to UpgradeSystem**

1. **Select** `/GameManagers` in Hierarchy
2. **Find** `UpgradeSystem` component in Inspector
   - **If it doesn't exist**: Click **Add Component** → type `UpgradeSystem` → add it
3. **Expand** "Upgrade Pool" array
4. **Set Size** to `15`
5. **Drag all 15 upgrade assets** from `/Assets/Data/LevelUpgrades/` into the slots

**Quick Tip:** Select all 15 assets at once in Project window, then drag onto the first empty slot - Unity will auto-fill them!

---

### **Step 4: Test!**

1. **Enter Play Mode**
2. **Start a run** (if you have a start button/mechanism)
3. **Kill enemies** to gain XP
4. **At Level 5**, the upgrade panel should appear with 3 choices
5. **Click an upgrade** to select it
6. Panel closes, game continues with your new power!

---

## 🐛 Troubleshooting

### **Console Error: "GetCurrentLevel"**
✅ **FIXED** - This has been corrected. If you still see it:
- Unity needs to recompile
- Check bottom-right corner for spinning icon
- Wait for compilation to finish
- Error should disappear

### **No "Generate Level Upgrade Assets" menu item**
**Cause:** Script compilation error or Unity hasn't refreshed
**Fix:**
1. Check Console for red errors
2. If no errors, try: **Assets > Refresh** (Ctrl+R)
3. Wait for recompilation
4. Check **Tools** menu again

**Manual Alternative if tool doesn't work:**
You can create upgrade assets manually:
1. Right-click in `/Assets/Data/LevelUpgrades/`
2. Create > Game Data > Level Upgrade (or search for LevelUpgradeData)
3. Name it and configure the values

### **Panel doesn't appear at level 5**
**Check:**
- Console for errors
- Is UpgradeSystem on GameManagers?
- Are 15 upgrades assigned to Upgrade Pool?
- Is LevelUpPanel inactive in Hierarchy? (should be - script activates it)

### **Upgrades show but look wrong**
**Check:**
- ItemButton prefab has "Name" and "Description" children
- Both children are TextMeshProUGUI components
- Background Image component exists on ItemButton root

### **Can't click upgrades**
**Check:**
- Canvas has GraphicRaycaster component
- EventSystem exists in scene
- Button component exists on ItemButton prefab

---

## 📋 Checklist

- [ ] Generated 15 upgrade assets in `/Assets/Data/LevelUpgrades/`
- [ ] Assigned 4 references in LevelUpPanel → LevelUpUI component
- [ ] Added UpgradeSystem to GameManagers
- [ ] Assigned 15 upgrades to UpgradeSystem pool
- [ ] OptionsContainer has Vertical Layout Group
- [ ] Tested in Play Mode - panel appears at level 5
- [ ] Can select upgrades and panel closes

---

## ⚡ Quick Visual Reference

```
Scene Hierarchy:
├── GameCanvas
│   └── LevelUpPanel [LevelUpUI component]
│       ├── LevelUpTitle [TextMeshProUGUI]
│       └── OptionsContainer [Vertical Layout Group]
│
└── GameManagers [Multiple components]
    ├── GameProgressionManager
    ├── PlayerStats
    ├── ExperienceSystem
    ├── WeaponSystem
    ├── UpgradeSystem ← ADD THIS!
    └── ...

Project Files:
/Assets
  /Data
    /LevelUpgrades ← 15 .asset files here
  /Prefabs
    /UI
      ItemButton.prefab ← Used for upgrade buttons
  /Scripts
    /Systems
      LevelUpUI.cs
      UpgradeSystem.cs
      ...
```

---

**Once setup is complete, the system is fully automatic!**
- Levels 2, 3, 4, 6, 7... → Auto bonuses (+2 Damage, +10 HP, +2% Speed)
- Levels 5, 10, 15, 20... → Choice popup (3 random upgrades)

**Have fun testing! 🎮**
