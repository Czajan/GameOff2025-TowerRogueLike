# Shop UI Hierarchy - Visual Guide

This document shows the exact hierarchy structure needed for the shop UI to work.

---

## 🎯 Target Hierarchy

```
GameCanvas
├── HealthBar (Slider)
├── CurrencyText (TextMeshProUGUI) ← Add CurrencyDisplay component here
├── WaveText (TextMeshProUGUI)
├── TimerText (TextMeshProUGUI) ← Add TimerDisplay component here
└── ShopPanel (Image) ← Add SimpleShopUI component here
    ├── ShopTitleText (TextMeshProUGUI)
    ├── ShopCurrencyText (TextMeshProUGUI)
    ├── ItemScrollView (Scroll Rect)
    │   ├── Viewport (Image)
    │   │   └── Content (RectTransform) ← Vertical Layout Group + Content Size Fitter
    │   └── Scrollbar Vertical (Scrollbar)
    └── CloseButton (Button)
        └── Text (TMP) (TextMeshProUGUI)
```

---

## 📦 ItemButton Prefab Structure

Create this as a prefab at `/Assets/Prefabs/UI/ItemButton.prefab`

```
ItemButton (Button)
├── Name (TextMeshProUGUI)
├── Description (TextMeshProUGUI)
└── Cost (TextMeshProUGUI)
```

---

## 🔧 Component Assignments

### ShopPanel (GameObject)

**Components:**
- RectTransform
- Canvas Renderer
- Image (background)
- **SimpleShopUI** ← Add this

**SimpleShopUI Inspector:**
```
Item List Container: → ItemScrollView/Viewport/Content
Item Button Prefab:  → /Assets/Prefabs/UI/ItemButton.prefab
Shop Title Text:     → ShopTitleText (child)
Currency Text:       → ShopCurrencyText (child)
Close Button:        → CloseButton (child)
```

**RectTransform:**
- Anchor: Stretch-Stretch
- Left: 100, Right: 100, Top: 100, Bottom: 100
- Active: **UNCHECKED** (disabled by default)

---

### ShopTitleText (TextMeshProUGUI)

**Text:** "Shop"  
**Font Size:** 32  
**Style:** Bold  
**Color:** White  
**Alignment:** Center  

**RectTransform:**
- Anchor: Top-Center
- Position X: 0, Position Y: -30, Position Z: 0
- Width: 300, Height: 50

---

### ShopCurrencyText (TextMeshProUGUI)

**Text:** "Currency: $0"  
**Font Size:** 20  
**Color:** Yellow (or White)  
**Alignment:** Right  

**RectTransform:**
- Anchor: Top-Right
- Position X: -20, Position Y: -30, Position Z: 0
- Width: 200, Height: 40

---

### ItemScrollView (Scroll Rect)

**RectTransform:**
- Anchor: Stretch-Stretch
- Left: 20, Right: 20, Top: 80, Bottom: 60

**Scroll Rect Component:**
- Content: → Viewport/Content
- Horizontal: false
- Vertical: true
- Movement Type: Elastic
- Scrollbar Vertical: → Scrollbar Vertical

---

### ItemScrollView/Viewport/Content (RectTransform)

**Components:**
- RectTransform
- **Vertical Layout Group**
- **Content Size Fitter**

**Vertical Layout Group:**
- Spacing: 10
- Padding: Left 10, Right 10, Top 10, Bottom 10
- Child Alignment: Upper Center
- Child Controls Size: Width & Height
- Child Force Expand: Width only

**Content Size Fitter:**
- Horizontal Fit: Unconstrained
- Vertical Fit: **Preferred Size** ← This makes it auto-grow

**RectTransform:**
- Anchor: Top-Stretch
- Pivot: X: 0.5, Y: 1
- Position Y: 0

---

### CloseButton (Button)

**Text:** "Close (ESC)"  
**Font Size:** 18  
**Color:** White  

**RectTransform:**
- Anchor: Bottom-Center
- Position X: 0, Position Y: 10, Position Z: 0
- Width: 150, Height: 40

**Button Component:**
- Interactable: true
- Transition: Color Tint
- Normal Color: Light Gray
- Highlighted: White
- Pressed: Yellow
- Disabled: Dark Gray

**OnClick Event:**
- Automatically handled by SimpleShopUI code (no manual setup needed)

---

## 🎨 ItemButton Prefab Details

### ItemButton (Button - Root)

**RectTransform:**
- Width: 380
- Height: 80

**Button Component:**
- Normal Color: RGBA(0.2, 0.2, 0.2, 1) - Dark Gray
- Highlighted: RGBA(0.3, 0.3, 0.3, 1) - Lighter Gray
- Pressed: RGBA(0.4, 0.35, 0, 1) - Yellow Tint
- Disabled: RGBA(0.15, 0.15, 0.15, 1) - Very Dark

---

### ItemButton/Name (TextMeshProUGUI)

**Text:** "Item Name"  
**Font Size:** 18  
**Style:** Bold  
**Color:** White  
**Alignment:** Left  

**RectTransform:**
- Anchor: Top-Stretch
- Pivot: X: 0.5, Y: 1
- Left: 10, Right: 10, Top: -10
- Height: 25

---

### ItemButton/Description (TextMeshProUGUI)

**Text:** "Description here"  
**Font Size:** 12  
**Color:** RGBA(0.8, 0.8, 0.8, 1) - Light Gray  
**Alignment:** Left  
**Wrapping:** Enabled  

**RectTransform:**
- Anchor: Middle-Stretch
- Pivot: X: 0.5, Y: 0.5
- Left: 10, Right: 10
- Position Y: 0
- Height: 30

---

### ItemButton/Cost (TextMeshProUGUI)

**Text:** "$100"  
**Font Size:** 16  
**Style:** Bold  
**Color:** RGBA(1, 0.92, 0, 1) - Gold/Yellow  
**Alignment:** Right  

**RectTransform:**
- Anchor: Bottom-Right
- Pivot: X: 1, Y: 0
- Position X: -10, Position Y: 10, Position Z: 0
- Width: 100, Height: 30

---

## 🎯 Quick Creation Steps

### Step 1: Create ShopPanel Children

1. Select `ShopPanel` in Hierarchy
2. Right-click → UI → Text - TextMeshPro → Rename to `ShopTitleText`
3. Right-click → UI → Text - TextMeshPro → Rename to `ShopCurrencyText`
4. Right-click → UI → Scroll View → Rename to `ItemScrollView`
5. Right-click → UI → Button - TextMeshPro → Rename to `CloseButton`

### Step 2: Configure ItemScrollView

1. Select `ItemScrollView/Viewport/Content`
2. Add Component → Vertical Layout Group
3. Add Component → Content Size Fitter
4. Set properties as shown above
5. Delete `ItemScrollView/Scrollbar Horizontal` (not needed)

### Step 3: Create ItemButton Prefab

1. Right-click `ShopPanel` → UI → Button - TextMeshPro
2. Rename to `ItemButton`
3. Add three Text children: `Name`, `Description`, `Cost`
4. Configure each as shown above
5. Drag `ItemButton` to `/Assets/Prefabs/UI/` folder
6. Delete `ItemButton` from Hierarchy

### Step 4: Add Components

1. Select `ShopPanel` → Add Component → **SimpleShopUI**
2. Select `CurrencyText` → Add Component → **CurrencyDisplay**
3. Select `TimerText` → Add Component → **TimerDisplay**

### Step 5: Assign References

1. Select `ShopPanel` → SimpleShopUI component
   - Assign all 5 references as shown above
2. Select `CurrencyText` → CurrencyDisplay component
   - Assign Currency Text: CurrencyText (itself)
3. Select `TimerText` → TimerDisplay component
   - Assign Timer Text: TimerText (itself)

### Step 6: Configure NPCs

1. Select `/Base/NPCs/WeaponVendor`
   - ShopNPC → Shop UI: `GameCanvas/ShopPanel`
2. Select `/Base/NPCs/StatVendor`
   - ShopNPC → Shop UI: `GameCanvas/ShopPanel`

---

## ✅ Verification Checklist

Before testing, verify:

- [ ] ShopPanel has SimpleShopUI component
- [ ] ShopPanel is **INACTIVE** by default (unchecked)
- [ ] ItemScrollView/Viewport/Content has Vertical Layout Group
- [ ] ItemScrollView/Viewport/Content has Content Size Fitter
- [ ] ItemButton.prefab exists in `/Assets/Prefabs/UI/`
- [ ] All 5 references assigned in SimpleShopUI
- [ ] Both NPCs have Shop UI assigned
- [ ] CurrencyDisplay component on CurrencyText
- [ ] TimerDisplay component on TimerText

---

## 🎨 Visual Preview

### Shop Panel Appearance:

```
┌─────────────────────────────────────────────────┐
│  Shop                      Currency: $250  │
├─────────────────────────────────────────────────┤
│  ┌────────────────────────────────────────┐ ▲ │
│  │ Move Speed Upgrade (Lv 2)              │ █ │
│  │ Increases movement speed               │ █ │
│  │                                  $100  │ █ │
│  ├────────────────────────────────────────┤ █ │
│  │ Max Health Upgrade (Lv 1)              │ █ │
│  │ Increases maximum health               │ █ │
│  │                                  $120  │ █ │
│  ├────────────────────────────────────────┤ ▼ │
│  └────────────────────────────────────────┘   │
│                                                 │
│              [ Close (ESC) ]                    │
└─────────────────────────────────────────────────┘
```

---

## 🐛 Common Issues

### Issue: Items not appearing in shop

**Cause:** Content doesn't have Vertical Layout Group or Content Size Fitter

**Fix:** Add both components to `ItemScrollView/Viewport/Content`

---

### Issue: Shop doesn't open

**Cause:** ShopUI not assigned in ShopNPC

**Fix:** Assign `GameCanvas/ShopPanel` to both NPCs' Shop UI field

---

### Issue: Currency not updating

**Cause:** CurrencyDisplay not assigned or GameProgressionManager missing

**Fix:** 
1. Check CurrencyDisplay component exists on CurrencyText
2. Check Currency Text field is assigned (to itself)
3. Verify GameProgressionManager exists on GameManagers

---

### Issue: Buttons not clickable

**Cause:** ShopPanel blocking raycasts when inactive

**Fix:** ShopPanel should start inactive, only activate when opened

---

## 📚 Related Files

- `/Assets/Scripts/Systems/SimpleShopUI.cs` - Main shop UI logic
- `/Assets/Scripts/Systems/CurrencyDisplay.cs` - Currency HUD display
- `/Assets/Scripts/Systems/TimerDisplay.cs` - Timer HUD display
- `/Assets/Scripts/Systems/ShopNPC.cs` - NPC interaction handler

---

**This hierarchy matches what SimpleShopUI expects!**

Follow this structure exactly for guaranteed compatibility.
