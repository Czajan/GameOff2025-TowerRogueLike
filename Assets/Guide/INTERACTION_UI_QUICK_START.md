# Interaction Notification - Quick Start

## 5-Minute Setup

### 1. Create UI (2 minutes)

**In Hierarchy:**
```
/GameCanvas
  └─ Right-click → UI → Panel → Rename to "InteractionNotification"
      └─ Right-click → UI → Text - TextMeshPro → Rename to "NotificationText"
```

**Configure InteractionNotification Panel:**
- **Rect Transform:**
  - Anchor: Bottom-Center (⬇️ bottom row, middle column)
  - Pos X: 0
  - Pos Y: 100
  - Width: 400
  - Height: 80

- **Image** (optional styling):
  - Color: Black with alpha 150 (semi-transparent)
  - Or disable Image component for no background

**Configure NotificationText:**
- **Rect Transform:**
  - Anchor: Stretch (top-left preset, hold Shift+Alt)
  - Left/Right/Top/Bottom: 10 (padding)

- **TextMeshPro:**
  - Font Size: 28
  - Alignment: Center + Middle (⊞)
  - Color: White
  - Font: Your game font

---

### 2. Add Script (1 minute)

**Select InteractionNotification:**
- Add Component → `InteractionNotificationUI`

**Assign References:**
- Notification Panel: Drag `InteractionNotification` (itself)
- Notification Text: Drag `NotificationText` (child)
- Default Interact Key: `E`

---

### 3. Update Gate (1 minute)

**Select `/Base/BaseGate`:**
- **Old System:** Delete `/Base/BaseGate/InteractionCanvas` child
- **BaseGate Inspector:**
  - Prompt Text: (remove reference)
  - Interaction Prompt: `to Start Run`

---

### 4. Test (1 minute)

1. **Enter Play Mode**
2. Walk to gate
3. **See:** "Press [E] to Start Run" at bottom-center of screen ✅
4. Walk away
5. **See:** Notification disappears ✅

---

## Visual Example

```
┌───────────────────────────────────┐
│         GAME VIEW (1920x1080)     │
│                                   │
│         🎮 Gameplay Area          │
│                                   │
│              🚶 Player            │
│                                   │
│              🚪 Gate              │
│                                   │
│      ╔═══════════════════╗       │
│      ║ Press [E] to      ║       │ ← Notification (bottom-center)
│      ║    Start Run      ║       │
│      ╚═══════════════════╝       │
└───────────────────────────────────┘
     Pos Y: 100 (pixels from bottom)
```

---

## Customization

### Change Position

**Bottom-Center (recommended):**
- Anchor: Bottom-Center
- Pos Y: 100

**Top-Right:**
- Anchor: Top-Right
- Pos X: -200, Pos Y: -50

**Center:**
- Anchor: Center
- Pos X: 0, Pos Y: 0

### Change Style

**Background:**
```
InteractionNotification → Image:
  - Color: (0, 0, 0, 150) ← Black semi-transparent
  - Or (50, 50, 50, 200) ← Dark gray
  - Or disable for no background
```

**Text:**
```
NotificationText → TextMeshPro:
  - Font Size: 24-36
  - Color: White, Yellow, or Cyan
  - Outline: Add TextMeshPro Outline for readability
  - Shadow: Add Shadow component (optional)
```

**Font:**
- Import your custom font
- Assign to NotificationText

---

## Add NPCs

### Any NPC/Interactable:

1. **Create GameObject** (or select existing NPC)
2. **Add Component** → `NPCInteraction`
3. **Configure:**
   - Interaction Range: 3
   - Interaction Prompt: `to Talk` (or `to Buy`, `to Open`, etc.)
   - Requires Pre Run Menu: Check if only in base

4. **Add Events:**
   - On Interact → Add your dialogue/shop/action

**Examples:**
- Shop NPC: Prompt = `to Buy Items`
- Dialogue NPC: Prompt = `to Talk`
- Door: Prompt = `to Open`
- Chest: Prompt = `to Loot`

---

## Code Examples

### Show Custom Notification

```csharp
// From any script
InteractionNotificationUI.Instance.ShowNotification("Wave Complete!");
InteractionNotificationUI.Instance.ShowNotification("Checkpoint Saved");
```

### Show Interaction Prompt

```csharp
// Shows "Press [E] to Open Door"
InteractionNotificationUI.Instance.ShowInteractionPrompt("to Open Door");
```

### Hide Notification

```csharp
InteractionNotificationUI.Instance.HideNotification();
```

### Change Interact Key

```csharp
// Change key display to F
InteractionNotificationUI.Instance.SetInteractKey("F");
```

---

## Troubleshooting

### ❌ Notification doesn't appear

**Check:**
1. InteractionNotificationUI component exists on `/GameCanvas/InteractionNotification`
2. References are assigned (Panel + Text)
3. NotificationText has correct color (not black on black)
4. No Console errors

### ❌ Text is invisible

**Check:**
1. NotificationText color is not transparent
2. Font is assigned
3. Font size > 0
4. Canvas is in Screen Space - Overlay mode

### ❌ Old world-space text still shows

**Fix:**
1. Delete `/Base/BaseGate/InteractionCanvas`
2. Remove promptText reference from BaseGate script

---

## Result

✅ **Single notification** for all interactions  
✅ **Screen-space** (always in same position)  
✅ **Easy to style** (one place to customize)  
✅ **Reusable** (works for Gate, NPCs, items, etc.)  
✅ **Automatic** (shows/hides based on proximity)  

---

## Next: Add More Interactables

**Use NPCInteraction for:**
- Shop NPCs
- Dialogue NPCs
- Doors
- Chests
- Levers/Buttons
- Pickup items
- Quest givers
- Anything interactive!

All will use the **same** centralized notification UI! 🎯
