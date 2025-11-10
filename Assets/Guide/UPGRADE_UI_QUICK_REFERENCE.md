# Upgrade Selection UI - Quick Visual Reference

## 🎨 Final Result Preview

```
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║                     LEVEL 5 - CHOOSE YOUR UPGRADE                            ║
║                                                                              ║
║     ┌─────────────┐         ┌─────────────┐         ┌─────────────┐        ║
║     │   COMMON    │         │    RARE     │         │   COMMON    │        ║
║     ├─────────────┤         ├─────────────┤         ├─────────────┤        ║
║     │    [Icon]   │         │    [Icon]   │         │    [Icon]   │        ║
║     │             │         │             │         │             │        ║
║     │  POWER UP   │         │  AIR WALKER │         │  VITALITY   │        ║
║     │             │         │             │         │             │        ║
║     │ Increase    │         │ Jump again  │         │ Increase    │        ║
║     │ your damage │         │ while       │         │ your max    │        ║
║     │             │         │ airborne    │         │ health      │        ║
║     │ +5 Damage   │         │             │         │ +20 Max HP  │        ║
║     │             │         │  Unlocks    │         │             │        ║
║     │             │         │ Double Jump │         │             │        ║
║     │             │         │             │         │             │        ║
║     │  Stack 1/10 │         │             │         │  Stack 1/10 │        ║
║     └─────────────┘         └─────────────┘         └─────────────┘        ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝
```

---

## 📐 Hierarchy Structure

```
GameCanvas
  └─ UpgradeSelectionPanel (Panel, disabled by default)
      ├─ TitleText (TextMeshPro)
      └─ OptionsContainer (Empty + Horizontal Layout Group)
          ├─ UpgradeOption1 (Button + UpgradeOptionUI script)
          │   ├─ Background (Image - part of Button)
          │   ├─ IconImage (Image)
          │   ├─ NameText (TextMeshPro)
          │   ├─ DescriptionText (TextMeshPro)
          │   ├─ RarityText (TextMeshPro)
          │   └─ StackText (TextMeshPro)
          ├─ UpgradeOption2 (same as Option1)
          └─ UpgradeOption3 (same as Option1)
```

---

## 🎯 Component Settings Quick Reference

### UpgradeSelectionPanel

```
Rect Transform: Stretch/Stretch (full screen)
Left: 0, Right: 0, Top: 0, Bottom: 0

Image Component:
  Color: Black (0, 0, 0, 220)
  
Canvas Group (optional, for fade effects):
  Alpha: 1
  Interactable: ✓
  Block Raycasts: ✓

Initial State: DISABLED (unchecked in Inspector)
```

### TitleText

```
Rect Transform:
  Anchor: Top Center
  Pos X: 0, Pos Y: -100
  Width: 800, Height: 80

TextMeshPro:
  Text: "LEVEL UP - CHOOSE YOUR UPGRADE"
  Font Size: 36
  Alignment: Center (horizontally & vertically)
  Color: White (255, 255, 255)
  Font Style: Bold
```

### OptionsContainer

```
Rect Transform:
  Anchor: Middle Center
  Pos X: 0, Pos Y: 0
  Width: 1200, Height: 400

Horizontal Layout Group:
  Spacing: 30
  Child Alignment: Middle Center
  Child Control Size:
    Width: ✗
    Height: ✗
  Child Force Expand:
    Width: ✗
    Height: ✗
```

### UpgradeOption (×3)

```
Rect Transform:
  Width: 350
  Height: 400

Button Component:
  Target Graphic: Background (Image)
  Transition: Color Tint
  Normal Color: (1, 1, 1, 1)
  Highlighted Color: (0.9, 0.9, 0.9, 1)
  Pressed Color: (0.7, 0.7, 0.7, 1)
  
Background Image:
  Color: Dark Gray (0.2, 0.2, 0.2, 1)
  Material: None
  Raycast Target: ✓
```

### IconImage (child of UpgradeOption)

```
Rect Transform:
  Anchor: Top Center
  Pos X: 0, Pos Y: -80
  Width: 100, Height: 100

Image Component:
  Source Image: (set by script)
  Color: White
  Preserve Aspect: ✓
  Raycast Target: ✗
```

### NameText (child of UpgradeOption)

```
Rect Transform:
  Anchor: Top Center
  Pos X: 0, Pos Y: -200
  Width: 320, Height: 40

TextMeshPro:
  Text: (set by script)
  Font Size: 24
  Alignment: Center
  Color: White
  Font Style: Bold
  Auto Size: ✗
  Wrapping: Disabled
  Overflow: Ellipsis
```

### DescriptionText (child of UpgradeOption)

```
Rect Transform:
  Anchor: Middle Center
  Pos X: 0, Pos Y: 0
  Width: 320, Height: 150

TextMeshPro:
  Text: (set by script)
  Font Size: 16
  Alignment: Top Left
  Color: White (200, 200, 200)
  Auto Size: ✗
  Wrapping: Enabled
  Overflow: Overflow
  Rich Text: ✓ (for colored stat text)
```

### RarityText (child of UpgradeOption)

```
Rect Transform:
  Anchor: Top Right
  Pos X: -10, Pos Y: -10
  Width: 100, Height: 30

TextMeshPro:
  Text: (set by script)
  Font Size: 14
  Alignment: Right
  Color: (set by script based on rarity)
  Font Style: Bold
  Alpha: 0.8
```

### StackText (child of UpgradeOption)

```
Rect Transform:
  Anchor: Bottom Center
  Pos X: 0, Pos Y: 10
  Width: 200, Height: 30

TextMeshPro:
  Text: (set by script)
  Font Size: 14
  Alignment: Center
  Color: Yellow (255, 255, 0)
  Font Style: Italic

Initial State: Hidden (enabled by script when needed)
```

---

## 🎨 Rarity Color Scheme

### In UpgradeOptionUI Inspector

```
Common Color:     RGB (204, 204, 204) or (0.8, 0.8, 0.8) - Light Gray
Rare Color:       RGB (74, 144, 226) or (0.29, 0.56, 0.89) - Blue
Legendary Color:  RGB (255, 215, 0) or (1, 0.84, 0) - Gold
```

### Usage

The script automatically:
- Colors the **NameText** based on rarity
- Sets **RarityText** color
- Tints the **Background** image slightly

---

## ⚡ Quick Setup Checklist

### Phase 1: Panel Structure
- [ ] Create `UpgradeSelectionPanel` (full-screen panel)
- [ ] Set background to black, alpha 220
- [ ] Disable panel initially

### Phase 2: Title
- [ ] Add `TitleText` (TextMeshPro)
- [ ] Position at top center
- [ ] Font size 36, center alignment

### Phase 3: Container
- [ ] Create `OptionsContainer` (Empty GameObject)
- [ ] Add Horizontal Layout Group
- [ ] Position at middle center
- [ ] Set spacing to 30

### Phase 4: Upgrade Cards (×3)
- [ ] Create `UpgradeOption1` (Button)
- [ ] Set size to 350×400
- [ ] Add children: IconImage, NameText, DescriptionText, RarityText, StackText
- [ ] Add `UpgradeOptionUI` script
- [ ] Assign all references in Inspector
- [ ] Duplicate for Option2 and Option3

### Phase 5: Main Script
- [ ] Add `UpgradeSelectionUI` script to panel
- [ ] Assign Selection Panel reference
- [ ] Assign Title Text reference
- [ ] Assign all 3 UpgradeOption references
- [ ] Check "Pause Game On Show"

---

## 🧪 Visual Test States

### State 1: Hidden (Initial)
```
Panel: disabled
Time.timeScale: 1
Player can move and fight
```

### State 2: Showing Upgrades
```
Panel: enabled
Time.timeScale: 0 (paused)
3 upgrade cards visible
Title shows current level
Each card displays:
  - Icon (if assigned)
  - Upgrade name (colored by rarity)
  - Description
  - Stat bonuses in yellow
  - Rarity badge in corner
  - Stack count (if applicable)
```

### State 3: Hover Effect
```
Card background lightens slightly
Button highlight transition
Shows user which card they're about to select
```

### State 4: After Selection
```
Panel: disabled
Time.timeScale: 1 (resumed)
Selected upgrade applied
Player continues run
```

---

## 💡 Pro Tips

### Make It Feel Good

1. **Add transition animations** (optional):
   - Panel fade in/out
   - Cards slide in from sides
   - Selected card grows slightly before hiding

2. **Add sound effects** (optional):
   - Panel appear: "whoosh" sound
   - Hover card: subtle "tick" sound
   - Select upgrade: "power-up" chime

3. **Visual feedback**:
   - Highlight selected card briefly
   - Show "+5 Damage" floating text
   - Flash screen with rarity color

4. **Polish details**:
   - Add glow effect to legendary cards
   - Animate icon with slight pulse
   - Add particle effects on selection

---

## 🎮 Example Layouts

### Compact Layout (more upgrades, smaller cards)

```
Upgrades Per Offer: 5
Card Width: 220
Card Height: 350
Container Width: 1400
Spacing: 20
```

### Large Layout (bigger, more detailed cards)

```
Upgrades Per Offer: 3
Card Width: 400
Card Height: 500
Container Width: 1300
Spacing: 50
Font sizes: +4 for all text
```

### Vertical Layout (stack cards vertically)

```
Replace Horizontal Layout Group with Vertical Layout Group
Container: Width 400, Height 1200
Card size: 380 × 350
Position container at center-left or center-right
```

---

## ✅ Quick Reference Complete!

Follow this guide to quickly build the upgrade selection UI. The system will:
- ✅ Automatically populate cards with upgrade data
- ✅ Handle rarity colors
- ✅ Show stack counts
- ✅ Pause/resume game
- ✅ Apply selected upgrades

Now create those upgrade assets and start experimenting with different builds! 🎉
