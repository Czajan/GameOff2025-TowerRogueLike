# Dynamic Camera Zoom Setup

## Overview
The camera now automatically zooms in/out based on game state:
- **Pre-Run Menu** (at base): Camera closer (distance 7)
- **During Run** (in world): Camera farther (distance 12-15)

## Setup Instructions

### Step 1: Add Component
1. Select `/CinemachineCamera` in Hierarchy
2. Click **Add Component**
3. Search for and add **Cinemachine Dynamic Distance**

### Step 2: Configure Settings
In the **Cinemachine Dynamic Distance** component:

- **Pre Run Distance**: `7` (close view for base/menu)
- **Run Distance**: `12` (or `15` for wider combat view)
- **Transition Duration**: `2` seconds (smooth zoom)
- **Transition Curve**: `EaseInOut` (default is good)
- **Show Debug**: ✓ Check to see logs (optional)

### Step 3: Test
1. **Start Play Mode** - Camera should be at distance 7
2. **Press E at gate** - Camera smoothly zooms out to 12-15
3. **Die or end run** - Camera zooms back in to 7

---

## How It Works

### Automatic State Tracking
- Listens to `RunStateManager` events:
  - `OnRunStarted` → Zoom out to `runDistance`
  - `OnRunEnded` → Zoom in to `preRunDistance`

### Smooth Transition
- Uses coroutine-based interpolation
- Animates over `transitionDuration` seconds
- Applies `transitionCurve` for easing

### Technical Details
```csharp
// Accesses Cinemachine 3.x API
positionComposer.CameraDistance = targetDistance;

// Smooth lerp with curve
float newDistance = Mathf.Lerp(startDistance, targetDistance, curveValue);
```

---

## Customization

### Different Distances

**Tight Combat View:**
```
Pre Run Distance: 5
Run Distance: 10
```

**Wide Strategic View:**
```
Pre Run Distance: 7
Run Distance: 18
```

**Dramatic Cinematic:**
```
Pre Run Distance: 6
Run Distance: 20
Transition Duration: 3
```

### Different Timing

**Instant Zoom (No Transition):**
```
Transition Duration: 0
```

**Slow Dramatic Zoom:**
```
Transition Duration: 4
Transition Curve: Custom (slow start, slow end)
```

**Quick Snap Out:**
```
Transition Duration: 0.5
Transition Curve: EaseIn (quick acceleration)
```

### Custom Curves

Click **Transition Curve** field and edit the curve:
- **Linear**: Constant speed zoom
- **EaseIn**: Slow start, fast end
- **EaseOut**: Fast start, slow end
- **EaseInOut**: Smooth start and end (default)
- **Custom**: Create your own shape!

---

## Advanced: Trigger Manually

You can also trigger zoom from code:

```csharp
// Get the component
var dynamicZoom = FindFirstObjectByType<CinemachineDynamicDistance>();

// Doesn't have public methods by default, but you can add:
public void SetDistance(float distance, bool immediate = false)
{
    if (immediate)
    {
        SetDistanceImmediate(distance);
    }
    else
    {
        TransitionToDistance(distance);
    }
}
```

---

## Integration with Gate Push

The zoom happens automatically when the run starts:

```
Press "E" at gate
  ↓
DoubleDoorGate calls RunStateManager.StartRun()
  ↓
CinemachineDynamicDistance receives OnRunStarted event
  ↓
Camera zooms out from 7 → 12 over 2 seconds
  ↓
Player pushed through gate (cinematic)
  ↓
Run begins with wide camera view
```

**Timing Perfect for Cinematic Feel:**
- Push duration: ~1.5s
- Zoom duration: ~2s
- They overlap for smooth transition!

---

## Troubleshooting

### Camera Doesn't Zoom
1. Check `CinemachinePositionComposer` exists on camera
2. Verify `RunStateManager.Instance` is not null
3. Enable `Show Debug` and check Console for logs
4. Ensure events are firing (check RunStateManager)

### Zoom Too Fast/Slow
- Adjust `Transition Duration`
- Try different values: 0.5 (fast), 2 (medium), 4 (slow)

### Zoom Feels Jerky
- Edit `Transition Curve` to be smoother
- Try pure `EaseInOut` curve
- Increase `Transition Duration`

### Wrong Starting Distance
- Check `RunStateManager.IsInPreRunMenu` state
- In Start(), script sets initial distance based on state
- Manually set in Inspector during Play Mode to test

### Zoom During Push Looks Weird
- Adjust timing:
  - Make zoom faster: Decrease `Transition Duration` to 1-1.5s
  - Make zoom slower: Increase to 3-4s
  - Match push duration for sync

---

## Best Settings for Different Feels

### Subtle Shift
```
Pre Run Distance: 10
Run Distance: 13
Transition Duration: 3
```
*Barely noticeable, professional*

### Dramatic Cinematic
```
Pre Run Distance: 5
Run Distance: 18
Transition Duration: 2.5
```
*Big change, exciting*

### Snappy Action
```
Pre Run Distance: 7
Run Distance: 12
Transition Duration: 1
Transition Curve: EaseOut
```
*Quick, responsive*

### Slow Reveal
```
Pre Run Distance: 6
Run Distance: 15
Transition Duration: 4
Transition Curve: EaseIn
```
*Builds anticipation*

---

## Files
- **Created**: `/Assets/Scripts/Systems/CinemachineDynamicDistance.cs`
- **Created**: `/Assets/Guide/DYNAMIC_CAMERA_ZOOM.md`

---

## Testing Checklist

✓ Camera at distance 7 in pre-run menu  
✓ Smooth zoom out when run starts  
✓ Camera at distance 12-15 during run  
✓ Smooth zoom in when run ends  
✓ No jerky movement  
✓ Deoccluder still works during zoom  
✓ Mouse orbit still works during zoom  
✓ Timing feels good with gate push  

---

## Next Steps

Consider adding:
- Different distances for different game modes
- Distance based on player health (zoom in when low HP)
- Boss fight zoom out
- Dynamic FOV changes alongside distance
- Shake/trauma during zoom transitions
