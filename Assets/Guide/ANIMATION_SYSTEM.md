# Player Animation System

## Overview
The player animation system uses Unity's Animator with parameters controlled by `PlayerController`.

## Animator Parameters

### Speed (Float)
- **Purpose**: Controls walk/run animation blend
- **Values**: 
  - `0` = Idle
  - `0.1 - 1.0` = Walk
  - `> 1.0` = Sprint/Run
- **Controlled by**: `moveInput.magnitude` or override

### IsGrounded (Bool)
- **Purpose**: Determines if player is on ground
- **Values**: `true` / `false`
- **Controlled by**: `CharacterController.isGrounded`

### Jump (Trigger)
- **Purpose**: Triggers jump animation
- **Activated by**: Jump input action

## Animation Override System

### When To Use
Use animation overrides when you need to control animations during:
- Cinematic sequences
- Scripted events
- Cutscenes
- Forced movement (like gate push)

### How It Works

**Normal Mode:**
```csharp
// Animation based on player input
Speed = moveInput.magnitude
```

**Override Mode:**
```csharp
// Animation forced to specific value
playerController.SetAnimationSpeedOverride(1.0f);
// Now Speed = 1.0 regardless of input
```

**Clear Override:**
```csharp
playerController.ClearAnimationSpeedOverride();
// Returns to normal input-based animation
```

## Usage Examples

### Example 1: Force Walk Animation
```csharp
// Make player look like they're walking
playerController.SetAnimationSpeedOverride(1.0f);

// Do your thing...
yield return new WaitForSeconds(2.0f);

// Restore normal
playerController.ClearAnimationSpeedOverride();
```

### Example 2: Force Run Animation
```csharp
// Make player look like they're running
playerController.SetAnimationSpeedOverride(2.0f);
```

### Example 3: Slow Walk
```csharp
// Make player walk slowly
playerController.SetAnimationSpeedOverride(0.5f);
```

### Example 4: Custom Animation Speed
```csharp
// Calculate dynamic speed
float customSpeed = Mathf.Lerp(0f, 2f, someValue);
playerController.SetAnimationSpeedOverride(customSpeed);
```

## Gate Push Integration

The gate push system uses animation override:

1. **Push starts** → `SetAnimationSpeedOverride(walkAnimationSpeed)`
2. **Player moves** → Walk animation plays
3. **Push ends** → `ClearAnimationSpeedOverride()`
4. **Normal control** → Animation responds to input again

## Important Notes

- **Override takes priority** over input
- **Always clear override** when done
- Override persists until cleared or `SetMovementEnabled(true)`
- Override doesn't affect `IsGrounded` parameter
- Works even when `movementEnabled = false`

## Troubleshooting

### Animation Stuck
- Check if override is active but not cleared
- Call `ClearAnimationSpeedOverride()` manually

### No Animation During Override
- Verify `walkAnimationSpeed` is > 0
- Check Animator has `Speed` parameter
- Ensure Animator component is assigned

### Animation Too Fast/Slow
- Adjust the override value:
  - Lower = Slower
  - Higher = Faster
  - `1.0` = Normal walk

## Technical Implementation

```csharp
private bool overrideAnimationSpeed = false;
private float animationSpeedOverride = 0f;

public void SetAnimationSpeedOverride(float speed)
{
    overrideAnimationSpeed = true;
    animationSpeedOverride = speed;
}

public void ClearAnimationSpeedOverride()
{
    overrideAnimationSpeed = false;
    animationSpeedOverride = 0f;
}

private void UpdateAnimator()
{
    if (animator == null) return;
    
    float speedValue;
    
    if (overrideAnimationSpeed)
    {
        speedValue = animationSpeedOverride;
    }
    else
    {
        speedValue = moveInput.magnitude;
        if (isSprinting && speedValue > 0.1f)
        {
            speedValue = speedValue * sprintMultiplier;
        }
    }
    
    animator.SetFloat("Speed", speedValue);
    animator.SetBool("IsGrounded", characterController.isGrounded);
}
```

## Future Enhancements

Consider adding:
- Override for other parameters (IsGrounded, etc.)
- Time-based auto-clear (override expires)
- Multiple override priorities
- Animation event callbacks
- Blend between override and input
