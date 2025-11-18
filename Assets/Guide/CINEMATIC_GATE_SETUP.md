# Cinematic Gate Push - Setup Guide

## What Changed

The gate interaction is now fully cinematic! When you press "E", the player:
- ✅ **Cannot move** (input disabled)
- ✅ **Cannot rotate camera** (mouse orbit locked)
- ✅ **Auto-rotates** to face forward
- ✅ **Auto-moves** through the gate smoothly
- ✅ **Walk animation plays** (configurable speed)
- ✅ **Restored control** after push completes

## New Features

### 1. Camera Lock
- Camera rotation is disabled during the push
- Prevents sideways sliding view
- Re-enabled automatically when push completes

### 2. Forced Player Rotation
- Player smoothly rotates to face the push direction
- Uses `Quaternion.Slerp` for smooth interpolation
- Configurable via `Force Player Rotation` checkbox

### 3. Walk Animation Override
- Walk animation plays during push
- `Walk Animation Speed` controls the Speed parameter value
- Default: `1.0` for normal walk
- Higher values: Faster walk animation
- Lower values: Slower walk animation

### 4. Animation Curve
- `Push Speed Curve` controls acceleration/deceleration
- Default: `EaseInOut` for smooth start and stop
- Customize in Inspector for different feels:
  - Linear: Constant speed
  - EaseIn: Slow start, fast end
  - EaseOut: Fast start, slow end

### 5. Unity Events
Four new events for audio/effects:
- `OnPushStarted` - When "E" is pressed
- `OnPushComplete` - When push finishes
- `OnGateOpened` - When doors open
- `OnGateClosed` - When doors close

---

## Setup Instructions

### Part 1: Configure Gate Settings

1. **Select** the gate GameObject (e.g., `/DoubleDoorGate`)
2. In **Inspector**, find `Double Door Gate` component
3. Configure **Auto-Push on Start** section:
   - ✓ **Use Auto Push** (enabled)
   - **Push Out Direction**: `(0, 0, -1)` (or adjust for your gate direction)
   - **Push Distance**: `5` (how far to push)
   - **Push Duration**: `1.5` (seconds)
   - ✓ **Lock Camera During Push** (enabled)
   - ✓ **Force Player Rotation** (enabled)
   - **Push Speed Curve**: Click to edit (default EaseInOut is good)
   - **Walk Animation Speed**: `1.0` (normal walk) or `0.5` (slow walk)

### Part 2: Add Audio (Optional)

1. **Select** the gate GameObject
2. **Add Component** → `Audio Source`
3. **Add Component** → `Gate Audio Controller`
4. In **Gate Audio Controller**:
   - Drag audio clips to:
     - **Push Start Sound** (footstep, grunt, etc.)
     - **Push Complete Sound** (arrival sound)
     - **Gate Open Sound** (door creaking)
     - **Gate Close Sound** (door slamming)
   - Adjust volumes as needed

### Part 3: Wire Up Events

1. Still on gate GameObject
2. Find `Double Door Gate` component
3. Expand **Events** section:
   - **On Push Started** → Click `+`
     - Drag gate GameObject to object field
     - Select `GateAudioController` → `PlayPushStartSound()`
   - **On Push Complete** → Click `+`
     - Drag gate GameObject to object field
     - Select `GateAudioController` → `PlayPushCompleteSound()`
   - **On Gate Opened** → Click `+`
     - Drag gate GameObject to object field
     - Select `GateAudioController` → `PlayGateOpenSound()`
   - **On Gate Closed** → Click `+`
     - Drag gate GameObject to object field
     - Select `GateAudioController` → `PlayGateCloseSound()`

---

## How It Works

### Cinematic Sequence:
1. **Player presses "E"** near gate
2. **OnPushStarted** event fires → Audio plays
3. **PlayerController** input disabled
4. **Animation override** activated (walk animation plays)
5. **CinemachineMouseOrbit** disabled (camera lock)
6. **Player rotates** smoothly to face forward
7. **Player moves** automatically through gate (using AnimationCurve)
8. **OnPushComplete** event fires → Audio plays
9. **Animation override cleared** (returns to normal)
10. **Input restored**, camera unlocked
11. Run starts!

### Technical Details:
- Uses `CharacterController.Move()` for movement
- `Quaternion.Slerp()` for smooth rotation
- `AnimationCurve.Evaluate()` for speed control
- `SetAnimationSpeedOverride()` forces animator Speed parameter
- Coroutine-based for frame-by-frame control

---

## Customization Ideas

### Make It Feel Different

**Slow and Dramatic:**
```
Push Duration: 2.5
Walk Animation Speed: 0.5 (slow walk)
Push Speed Curve: EaseOut (0,0,1,1) → (0,0.8,1,1)
```

**Quick and Snappy:**
```
Push Duration: 0.8
Walk Animation Speed: 1.5 (fast walk)
Push Speed Curve: EaseIn
```

**Constant Speed (No Curve):**
```
Push Speed Curve: Linear (0,0) → (1,1) flat line
Walk Animation Speed: 1.0
```

**Sprint Through Gate:**
```
Push Duration: 1.0
Walk Animation Speed: 2.0 (or higher)
Push Speed Curve: EaseIn
```

### Add Visual Effects

Wire up events to:
- Particle effects (dust, wind)
- Screen shake
- Camera zoom
- UI fade
- Vignette effect

Example:
```csharp
// In your effects script
public void PlayPushEffect()
{
    // Spawn particles
    // Trigger camera shake
    // Show motion blur
}
```

Then in Inspector:
- **On Push Started** → `EffectsController.PlayPushEffect()`

---

## Testing Checklist

✓ Press "E" at gate  
✓ Player moves forward automatically  
✓ Walk animation plays during push  
✓ Cannot move with WASD during push  
✓ Cannot rotate camera during push  
✓ Player faces correct direction  
✓ Movement restored after push  
✓ Camera control restored after push  
✓ Animation returns to normal after push  
✓ Audio plays at correct times  
✓ Run starts successfully  

---

## Troubleshooting

### Player Can Still Move
- Check `Lock Camera During Push` is ✓ enabled
- Verify `CinemachineMouseOrbit` component exists on camera

### Player Doesn't Rotate
- Check `Force Player Rotation` is ✓ enabled
- Verify `Push Out Direction` is correct

### Animation Doesn't Play
- Check `Walk Animation Speed` is > 0 (try 1.0)
- Verify Animator component exists on player
- Check Animator has `Speed` float parameter
- Test: Set `Walk Animation Speed` to 2.0 to see if it's working

### Animation Too Fast/Slow
- Adjust `Walk Animation Speed`:
  - `0.5` = Slow walk
  - `1.0` = Normal walk
  - `1.5-2.0` = Fast walk/run

### Camera Stays Locked
- Check for errors in Console
- Verify `CinemachineMouseOrbit` is found in Start()

### Audio Doesn't Play
- Check `GateAudioController` component is added
- Verify audio clips are assigned
- Check events are wired up correctly
- Verify AudioSource has no conflicting settings

### Player Slides Sideways
- The new system prevents this! If it still happens:
  - Ensure `Lock Camera During Push` is enabled
  - Check PlayerController.SetMovementEnabled() is working

---

## Files Created/Modified

- **Modified**: `/Assets/Scripts/Systems/DoubleDoorGate.cs`
  - Added camera lock
  - Added rotation forcing
  - Added animation curve
  - Added walk animation override
  - Added Unity Events
- **Modified**: `/Assets/Scripts/Player/PlayerController.cs`
  - Added `SetAnimationSpeedOverride()` method
  - Added `ClearAnimationSpeedOverride()` method
  - Animation system now supports manual override
- **Created**: `/Assets/Scripts/Systems/GateAudioController.cs`
  - Simple audio player for gate events
- **Updated**: `/Assets/Guide/CINEMATIC_GATE_SETUP.md`
  - This guide

---

## Next Steps

Consider adding:
- Camera shake on push start
- Particle effects (dust, wind)
- UI fade/transition
- Voice lines
- Background music transition
- Slow-motion effect
- Post-processing effects (vignette, bloom)
