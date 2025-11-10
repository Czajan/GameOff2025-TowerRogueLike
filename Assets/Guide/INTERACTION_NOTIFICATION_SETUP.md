# Interaction Notification UI Setup Guide

## Overview

A centralized screen-space notification system for all interactive objects (Gate, NPCs, etc.) that displays "Press [E] to..." prompts in a consistent location on the screen.

---

## Setup Steps

### 1. Add InteractionNotificationUI to GameCanvas

**Hierarchy Setup:**
```
/GameCanvas
├── (existing UI elements)
└── /InteractionNotification (new)
    └── /NotificationText
```

**Step-by-Step:**

1. **Select `/GameCanvas`** in Hierarchy
2. **Right-click** → UI → Panel
3. **Rename** to `InteractionNotification`
4. **Select InteractionNotification:**
   - Anchor: Bottom-Center
   - Pos X: 0, Pos Y: 100 (adjust to preference)
   - Width: 400, Height: 80
   - (Optional) Add background image/color

5. **Right-click InteractionNotification** → UI → Text - TextMeshPro
6. **Rename** to `NotificationText`
7. **Configure NotificationText:**
   - Anchor: Stretch (all sides)
   - Text: "Press [E] to Interact"
   - Font Size: 24-32
   - Alignment: Center + Middle
   - Color: White (or your preference)

8. **Add Component to InteractionNotification:**
   - Add Component → `InteractionNotificationUI`

9. **Configure InteractionNotificationUI:**
   - Notification Panel: `InteractionNotification` (self)
   - Notification Text: `NotificationText`
   - Default Interact Key: `E`

---

### 2. Update BaseGate

**In Inspector:**
1. Select `/Base/BaseGate`
2. **Remove/Hide** the old `InteractionCanvas` child (or delete it)
3. **BaseGate Inspector:**
   - Interaction Prompt: `to Start Run` (no "Press [E]" needed)

**The script automatically:**
- Shows notification when player is in range
- Hides notification when player leaves range
- Works with centralized UI

---

### 3. Add NPCs (Optional)

**For any NPC:**

1. **Create NPC GameObject** (or use existing)
2. **Add Component** → `NPCInteraction`
3. **Configure:**
   - Interaction Range: 3
   - Interaction Prompt: `to Talk` (or `to Buy`, `to Trade`, etc.)
   - Requires Pre Run Menu: ✅ (if only in base) or ❌ (if always)

4. **Setup Events:**
   - In `On Interact` event:
     - Add your dialogue system
     - Or call a method
     - Or open a shop UI

**Example UnityEvent:**
```
On Interact (UnityEvent)
  ├─ DialogueManager.ShowDialogue("Hello!")
  └─ ShopUI.OpenShop()
```

---

## Visual Layout

### Screen Position Example

```
┌─────────────────────────────┐
│         GAME VIEW           │
│                             │
│                             │
│        (gameplay)           │
│                             │
│                             │
│    ┌─────────────────┐     │
│    │ Press [E] to... │     │ ← Notification appears here
│    └─────────────────┘     │
└─────────────────────────────┘
```

**Recommended Position:**
- Bottom-center (above ground)
- Or top-right corner
- Or wherever fits your UI design

---

## Customization

### Change Text Style

**Select `/GameCanvas/InteractionNotification/NotificationText`:**
- Font: Your game font
- Size: 24-36
- Color: White, Yellow, or custom
- Outline: Add for better readability
- Shadow: Optional

### Change Background

**Select `/GameCanvas/InteractionNotification`:**
- Image Component:
  - Source Image: Your custom panel sprite
  - Color: Semi-transparent background
  - Material: Custom material if needed

### Change Position

**Select `/GameCanvas/InteractionNotification`:**
- Anchor Presets:
  - **Bottom-Center**: Classic prompt location
  - **Top-Right**: Corner notification
  - **Center**: Centered on screen
- Adjust Pos X/Y to fine-tune

### Change Interact Key

**In `/GameCanvas/InteractionNotification` → InteractionNotificationUI:**
- Default Interact Key: Change to `F`, `Q`, etc.

Or **dynamically** in code:
```csharp
InteractionNotificationUI.Instance.SetInteractKey("F");
```

---

## Usage in Scripts

### Show Custom Notification

```csharp
InteractionNotificationUI.Instance.ShowNotification("Wave Complete!");
```

### Show Interaction Prompt

```csharp
InteractionNotificationUI.Instance.ShowInteractionPrompt("to Open Door");
// Displays: "Press [E] to Open Door"
```

### Hide Notification

```csharp
InteractionNotificationUI.Instance.HideNotification();
```

---

## Features

✅ **Centralized** - One UI for all interactions  
✅ **Screen-space** - Always visible, not affected by camera  
✅ **Consistent** - Same style everywhere  
✅ **Reusable** - Works for Gate, NPCs, items, etc.  
✅ **Easy to customize** - One place to change appearance  
✅ **Automatic** - Shows/hides based on proximity  

---

## Interaction System Components

### InteractionNotificationUI
- **Location:** `/GameCanvas/InteractionNotification`
- **Purpose:** Centralized UI manager (Singleton)
- **Methods:**
  - `ShowNotification(string message)`
  - `ShowInteractionPrompt(string action)`
  - `HideNotification()`
  - `SetInteractKey(string key)`

### BaseGate
- **Location:** `/Base/BaseGate`
- **Purpose:** Gate interaction
- **Uses:** Automatically shows/hides notification on proximity

### NPCInteraction
- **Location:** Any NPC GameObject
- **Purpose:** Generic NPC interaction
- **Uses:** Automatically shows/hides notification on proximity
- **Events:** `OnInteract` UnityEvent for custom behavior

---

## Migration from Old System

### Old System (World-Space Canvas):
```
/BaseGate
  └─ /InteractionCanvas (World Space)
      └─ /PromptText
```

**Problems:**
- Canvas above each object
- Hard to maintain consistency
- Affected by camera angle
- Duplicated UI elements

### New System (Screen-Space):
```
/GameCanvas (Screen Space)
  └─ /InteractionNotification
      └─ /NotificationText

/BaseGate (no UI child)
/NPC (no UI child)
```

**Benefits:**
- One notification for everything
- Consistent position
- Easy to style
- Less scene clutter

---

## Testing

### Test Gate Interaction:
1. **Enter Play Mode**
2. Walk to gate
3. **Expected:** Notification appears: `Press [E] to Start Run`
4. Walk away
5. **Expected:** Notification disappears

### Test NPC Interaction:
1. **Add NPCInteraction** to any GameObject
2. Set prompt: `to Talk`
3. Walk to NPC
4. **Expected:** `Press [E] to Talk`
5. Press E
6. **Expected:** OnInteract event fires

---

## Troubleshooting

### Notification doesn't appear:
- Check InteractionNotificationUI exists on GameCanvas
- Check references are assigned in Inspector
- Check notification panel is active in scene (not disabled)
- Look for errors in Console

### Notification appears but no text:
- Check NotificationText reference is assigned
- Check text color isn't transparent
- Check font is assigned

### Multiple notifications overlap:
- Only one notification should exist
- InteractionNotificationUI is a Singleton
- Check you don't have multiple instances

### Old world-space text still shows:
- Delete or disable `/Base/BaseGate/InteractionCanvas`
- Remove `promptText` reference from BaseGate Inspector

---

## Next Steps

1. **Style the notification** to match your game's UI
2. **Add NPCs** with custom interaction prompts
3. **Add more interactables** (chests, doors, etc.) using NPCInteraction script
4. **Optional:** Add animations (fade in/out) to notification panel
5. **Optional:** Add sound effects on show/hide

---

## File Locations

**Scripts:**
- `/Assets/Scripts/UI/InteractionNotificationUI.cs`
- `/Assets/Scripts/Systems/BaseGate.cs` (updated)
- `/Assets/Scripts/Interactables/NPCInteraction.cs` (new)

**Scene Setup:**
- `/GameCanvas/InteractionNotification` (needs manual setup)
