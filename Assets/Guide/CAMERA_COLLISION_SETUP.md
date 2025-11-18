# Camera Collision Setup Guide

## Problem
The custom `CameraOrbitController` was fighting with Cinemachine's positioning system, causing the collision detection to fail.

## Solution
Use Cinemachine's built-in systems properly:
1. **CinemachineMouseOrbit** - Custom extension for mouse rotation (works WITH Cinemachine)
2. **CinemachineDeoccluder** - Built-in collision avoidance (prevents wall clipping)

---

## Setup Instructions

### Step 1: Remove Old Component
1. Select `/CinemachineCamera` in Hierarchy
2. In Inspector, **Remove Component** → `Camera Orbit Controller` (the old script)

### Step 2: Add Mouse Orbit Extension
1. Still on `/CinemachineCamera`
2. Click **Add Extension** (in CinemachineCamera component)
3. Select **Cinemachine Mouse Orbit** (the new script)
4. Configure settings:
   - **Mouse Sensitivity**: `0.3-0.8` (adjust to taste)
   - **Min Vertical Angle**: `10`
   - **Max Vertical Angle**: `70`

### Step 3: Add Collision Avoidance
1. Still on `/CinemachineCamera`
2. Click **Add Extension** again
3. Select **Cinemachine Deoccluder**
4. Configure collision settings:
   - **Avoid Obstacles** ✓ Enable
   - **Strategy**: `Pull Camera Forward` (simplest) or `Preserve Camera Height` (more natural)
   - **Collide Against**: Select ONLY `Ground` layer ✓
   - **Ignore Tag**: `Player`
   - **Camera Radius**: `0.2` (small value prevents clipping)
   - **Damping**: `1.0` (smooth return when clear)
   - **Damping When Occluded**: `0.3` (fast response when hitting wall)

### Step 4: Adjust CinemachinePositionComposer
1. Still on `/CinemachineCamera`
2. Find **Cinemachine Position Composer** component
3. Set **Camera Distance**: `10-15` (how far camera sits from player)
4. Set **Target Offset Y**: `1-2` (aim slightly above player feet)

---

## How It Works

### CinemachineMouseOrbit
- Hooks into Cinemachine's **Aim stage**
- Only controls **rotation** (yaw/pitch from mouse)
- Doesn't touch position (Cinemachine handles that)
- Works perfectly with other extensions

### CinemachineDeoccluder
- Runs **after** positioning and rotation
- Casts rays from LookAt target to camera
- When wall detected: **pulls camera forward**
- When clear: **smoothly returns** to desired distance
- Uses physics layers (only checks Ground layer)

### Why This Works
- No conflicting position control
- Cinemachine has full authority over camera placement
- Extensions modify the final output cooperatively
- Built-in collision is robust and tested

---

## Troubleshooting

### Camera Still Clips
1. Increase **Camera Radius** to `0.3` or `0.5`
2. Try different **Strategy**:
   - `Pull Camera Forward` = Simple zoom in
   - `Preserve Camera Height` = Moves around obstacles
   - `Preserve Camera Distance` = Maintains distance, changes angle
3. Check **Collide Against** includes `Ground` layer ✓
4. Ensure walls have **Colliders** (MeshCollider, BoxCollider, etc)

### Camera Moves Too Slowly
1. Decrease **Damping When Occluded** to `0.1-0.2`
2. Decrease **Damping** to `0.5`

### Camera Jitters
1. Increase **Damping When Occluded** to `0.5-1.0`
2. Increase **Smoothing Time** to `0.5`

### Mouse Too Sensitive/Slow
1. Adjust **Mouse Sensitivity** in CinemachineMouseOrbit
2. Range: `0.1` (very slow) to `2.0` (very fast)

---

## Optional: Distance Control

If you want to control camera distance from the mouse orbit script, you can add this to `CinemachineMouseOrbit.cs`:

```csharp
[SerializeField] private float scrollSensitivity = 2f;
[SerializeField] private float minDistance = 5f;
[SerializeField] private float maxDistance = 20f;
private float currentDistance = 10f;

// In PostPipelineStageCallback, after rotation:
if (stage == CinemachineCore.Stage.Body)
{
    float scroll = Mouse.current?.scroll.ReadValue().y ?? 0f;
    if (scroll != 0f)
    {
        currentDistance -= scroll * scrollSensitivity * 0.01f;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
    }
    
    // Apply distance to CinemachinePositionComposer if it exists
    var composer = VirtualCamera.GetCinemachineComponent<CinemachinePositionComposer>();
    if (composer != null)
    {
        composer.CameraDistance = currentDistance;
    }
}
```

---

## Testing Checklist

✓ Camera rotates with mouse movement (no right-click needed)  
✓ Camera pulls in when near walls  
✓ Camera returns to distance when walls cleared  
✓ No clipping through wall geometry  
✓ Smooth transitions (no jitter)  
✓ Mouse sensitivity feels good  

---

## Files
- **New Script**: `/Assets/Scripts/Systems/CinemachineMouseOrbit.cs`
- **Old Script (can delete)**: `/Assets/Scripts/Systems/CameraOrbitController.cs`
- **Old Script (can keep)**: `/Assets/Scripts/Systems/ThirdPersonCameraController.cs` (unused alternative)
