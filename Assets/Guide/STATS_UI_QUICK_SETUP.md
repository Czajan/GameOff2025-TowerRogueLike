# Stats Display UI - 5 Minute Setup

## 🚀 Super Quick Setup

### 1. Create UI Hierarchy (2 minutes)

In Unity Hierarchy:

```
GameCanvas
  ├─ HealthBar (existing)
  ├─ WaveText (existing)
  ├─ ... (other existing UI)
  └─ StatsPanel ⭐ NEW!
      └─ StatsText ⭐ NEW!
```

**Steps:**
1. Right-click `GameCanvas` → UI → Panel
2. Rename to `StatsPanel`
3. Right-click `StatsPanel` → UI → Text - TextMeshPro
4. Rename to `StatsText`

---

### 2. Configure StatsPanel (1 minute)

Select `StatsPanel` in Hierarchy:

**Rect Transform:**
```
Anchor: Middle Center
Pos X: 0
Pos Y: 0
Width: 400
Height: 600
```

**Image:**
```
Color: Black
Alpha: 200 (slide the A slider)
```

**At top of Inspector:**
```
☐ Uncheck the checkbox (to disable it initially)
```

---

### 3. Configure StatsText (1 minute)

Select `StatsText` in Hierarchy:

**Rect Transform:**
```
Click Anchor Preset → Hold ALT → Click "Stretch/Stretch" (bottom-right)
Left: 20
Right: 20
Top: 20
Bottom: 20
```

**TextMeshPro - Text:**
```
Font Size: 18
Alignment: Top + Left (click top-left alignment button)
Color: White
Vertex Color: White (if different)
Wrapping: ☑ Enabled
Overflow: Overflow
```

---

### 4. Add Script (1 minute)

Select `StatsPanel` in Hierarchy:

1. **Add Component** → Search: `StatsDisplayUI`
2. **Assign Fields:**
   - **Stats Panel:** Drag `StatsPanel` from Hierarchy
   - **Stats Text:** Drag `StatsPanel/StatsText` from Hierarchy
3. **Settings:**
   - Toggle Key: `Tab`
   - Show On Startup: `☐` (unchecked)

---

### 5. Test! (30 seconds)

1. **Enter Play Mode**
2. **Press TAB**
3. **See your stats!**

---

## 🎮 What You'll See

```
╔════════════════════════════════════╗
║        PLAYER STATS                ║
║                                    ║
║ LEVEL: 3                           ║
║                                    ║
║ COMBAT STATS                       ║
║   Damage: 14.0                     ║
║   Crit Chance: 7.0%                ║
║   Crit Damage: 150%                ║
║   Attack Range: 2.0m               ║
║   Attack Speed: 100%               ║
║                                    ║
║ DEFENSE STATS                      ║
║   Current HP: 95                   ║
║   Max HP: 120                      ║
║   Move Speed: 5.5                  ║
║                                    ║
║ PROGRESSION                        ║
║   XP: 45/132                       ║
║   Gold: 350                        ║
║   Essence: 120                     ║
║                                    ║
║      Press TAB to close            ║
╚════════════════════════════════════╝
```

---

## ⚙️ Inspector Settings Reference

### StatsPanel GameObject

```
┌─ Transform ─────────────────────┐
│ Position: 0, 0, 0               │
│ Rotation: 0, 0, 0               │
│ Scale: 1, 1, 1                  │
└─────────────────────────────────┘

┌─ Rect Transform ────────────────┐
│ Anchor: Middle Center           │
│ Pivot: 0.5, 0.5                 │
│ Pos X: 0    │ Pos Y: 0          │
│ Width: 400  │ Height: 600       │
└─────────────────────────────────┘

┌─ Canvas Renderer ───────────────┐
│ (Default settings)              │
└─────────────────────────────────┘

┌─ Image ─────────────────────────┐
│ Source Image: UISprite (default)│
│ Color: ███ (0, 0, 0, 200)       │
│ Material: None                  │
│ Raycast Target: ☑               │
└─────────────────────────────────┘

┌─ Stats Display UI (Script) ─────┐
│ Script: StatsDisplayUI          │
│ Stats Panel: [StatsPanel]       │
│ Stats Text: [StatsText]         │
│ Toggle Key: Tab                 │
│ Show On Startup: ☐              │
└─────────────────────────────────┘
```

---

### StatsText GameObject

```
┌─ Transform ─────────────────────┐
│ Position: 0, 0, 0               │
│ Rotation: 0, 0, 0               │
│ Scale: 1, 1, 1                  │
└─────────────────────────────────┘

┌─ Rect Transform ────────────────┐
│ Anchors: Stretch/Stretch        │
│ Pivot: 0.5, 0.5                 │
│ Left: 20   │ Right: 20          │
│ Top: 20    │ Bottom: 20         │
└─────────────────────────────────┘

┌─ Canvas Renderer ───────────────┐
│ (Default settings)              │
└─────────────────────────────────┘

┌─ TextMeshPro - Text (UI) ───────┐
│ Text Input: (empty)             │
│ Font Asset: (default TMP font)  │
│ Font Size: 18                   │
│ Auto Size: ☐                    │
│ Font Style: Regular             │
│ Color: ███ White (255,255,255)  │
│ Spacing: 0                      │
│ Alignment: ◰ (Top Left)         │
│ Wrapping: ☑ Enabled             │
│ Overflow: Overflow              │
│ Rich Text: ☑                    │
└─────────────────────────────────┘
```

---

## 🎨 Visual Layout

```
GameCanvas (full screen)
  │
  ├─ (Top Left)
  │  └─ CurrencyDisplayPanel
  │
  ├─ (Top Center)
  │  └─ WaveText
  │
  ├─ (Bottom Left)
  │  └─ HealthBar
  │
  ├─ (Bottom Center)
  │  └─ ExperienceBarPanel
  │
  └─ (Center) ⭐ NEW!
     └─ StatsPanel (hidden by default, TAB to show)
        └─ StatsText
```

---

## ✅ Checklist

- [ ] Created `StatsPanel` under `GameCanvas`
- [ ] Created `StatsText` under `StatsPanel`
- [ ] Set `StatsPanel` Rect Transform (Middle Center, 400×600)
- [ ] Set `StatsPanel` Image (Black, Alpha 200)
- [ ] Disabled `StatsPanel` initially
- [ ] Set `StatsText` Rect Transform (Stretch/Stretch, margins 20)
- [ ] Set `StatsText` Font Size 18, Top Left alignment
- [ ] Added `StatsDisplayUI` script to `StatsPanel`
- [ ] Assigned `Stats Panel` reference
- [ ] Assigned `Stats Text` reference
- [ ] Tested in Play Mode with TAB key

---

## 🐛 Quick Troubleshooting

| Problem | Solution |
|---------|----------|
| Can't see panel when pressing TAB | Make sure panel is enabled in script, check Inspector fields are assigned |
| Text is cut off | Increase panel height or decrease font size |
| Panel too small | Increase Width/Height in Rect Transform |
| Text not visible | Check text color is White, panel alpha is not 0 |
| TAB doesn't work | Check Console for errors, verify script is attached |
| Stats show 0 | Make sure game is running and PlayerStats exists |

---

## 🎯 Done!

**That's it!** You now have a fully functional stats display that:
- ✅ Shows all your current stats
- ✅ Updates in real-time
- ✅ Toggles with TAB key
- ✅ Clean, readable format
- ✅ Automatically tracks level-ups, damage, health, XP, currencies

Press TAB anytime during gameplay to check your stats! 🎉
