# XP Orb Collection Fix

## Problem Fixed

**Issue:** XP orbs were getting stuck underneath the player's legs and levitating when the player stopped moving.

**Root Cause:**
- Orbs targeted `playerTransform.position` (player's feet at Y=0)
- Player's CharacterController extends from Y=0 to Y=1.6
- Orbs couldn't reach the feet because they collided with the player's body
- Without movement, orbs would hover underneath, stuck trying to reach an unreachable point

## Solution Applied

### 1. Target Height Offset
Changed orb targeting from feet to chest/center height:
```csharp
// OLD: Targets player feet (Y=0)
Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

// NEW: Targets chest height (Y=1.0)
Vector3 targetPosition = playerTransform.position + Vector3.up * targetHeight;
Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
```

### 2. Distance-Based Collection
Added automatic collection when orb gets close enough:
```csharp
float distanceToPlayer = Vector3.Distance(transform.position, targetPosition);

if (distanceToPlayer < collectionDistance)
{
    Collect();
    return;
}
```

This ensures orbs are collected even if they can't reach the exact target point.

### 3. Configurable Settings
New inspector fields for fine-tuning:
- **Target Height**: `1.0` (chest level)
- **Collection Distance**: `1.5` (generous pickup range)

## How It Works Now

### Orb Movement Flow:
```
Orb spawns → Flies toward player chest (Y+1.0)
           → Gets within 1.5 units
           → Auto-collects (distance check)
           → Destroyed
```

### Fallback Collection:
- **Primary**: Distance check in Update() (1.5 units)
- **Secondary**: OnTriggerEnter collision
- Both ensure reliable collection

## Settings

### Default Values:
- **Fly Speed**: `10` units/sec
- **Target Height**: `1.0` (chest level)
- **Collection Distance**: `1.5` (generous range)
- **Bob Speed**: `2` (visual effect)
- **Bob Height**: `0.3` (vertical wobble)
- **Rotation Speed**: `180` deg/sec

### Customization:

**For Faster Collection:**
```
Fly Speed: 15
Collection Distance: 2.0
```

**For Slower "Magnet" Effect:**
```
Fly Speed: 6
Collection Distance: 1.2
```

**For Different Player Heights:**
If your player is taller/shorter, adjust:
```
Target Height = Player CharacterController Center Y
```

## Technical Details

### Why Chest Height?
```
Player CharacterController:
- Height: 1.6
- Center Y: 0.8
- Transform Y: 0 (feet on ground)

Target Point:
- Y = 0 (feet) + 1.0 = Y = 1.0 (mid-chest)
- Above player's center (0.8)
- Below player's head (1.6)
- Easy to reach from all angles
```

### Collection Distance Math:
```
Player radius: 0.5
Safety margin: 1.0
Collection Distance: 1.5

This allows collection from:
- Above player
- Below player
- Any horizontal angle
- Even if orb path is slightly off
```

### Visual Bobbing:
The bobbing effect is additive on top of movement:
```csharp
// Move toward player
transform.position += directionToPlayer * flySpeed * Time.deltaTime;

// Add bobbing (doesn't interfere with collection)
transform.position += Vector3.up * bobAmount * Time.deltaTime;
```

## Advantages

### Before Fix:
- ✗ Orbs stuck under player
- ✗ Required player movement to collect
- ✗ Orbs could levitate indefinitely
- ✗ Frustrating UX

### After Fix:
- ✓ Orbs collected reliably
- ✓ Works when player is stationary
- ✓ No stuck orbs
- ✓ Smooth collection feel
- ✓ Configurable behavior

## Integration with Player

### Player Setup Required:
```
CharacterController:
- Center Y: Any value (typically 0.8 for height 1.6)
- Height: Any value (typically 1.6)
- Radius: Any value (typically 0.5)

ExperienceOrb:
- Target Height: Set to player center or slightly higher
- Collection Distance: > player radius (recommend 1.5-2.0)
```

### Auto-Adjustment:
You can make orbs auto-detect player height:
```csharp
// In Start():
CharacterController playerController = player.GetComponent<CharacterController>();
if (playerController != null)
{
    targetHeight = playerController.center.y;
}
```

## Troubleshooting

### Orbs Still Get Stuck
1. **Increase Collection Distance**
   - Try `2.0` or `2.5`
   - Larger is more forgiving

2. **Adjust Target Height**
   - Try player's center Y
   - Or player's head: `characterController.height`

3. **Check Colliders**
   - Orb should have trigger collider
   - Player should have CharacterController or collider
   - Layers should allow collision

### Orbs Collected Too Early
1. **Decrease Collection Distance**
   - Try `1.0` or `0.8`
   - Makes collection require closer proximity

2. **Disable Distance Check**
   - Comment out distance check
   - Rely only on OnTriggerEnter

### Orbs Don't Bob/Rotate
- Bob and rotation are visual only
- Don't affect collection
- Adjust `bobSpeed`, `bobHeight`, `rotationSpeed` in Inspector

### Player Height Changed
- Update `targetHeight` to match new player center
- Or use auto-detection code above

## Future Enhancements

Consider adding:
- **Magnet range**: Start flying only when player is close
- **Speed curve**: Accelerate as orb gets closer
- **Collection VFX**: Particle effect on collect
- **Collection SFX**: Sound when collected
- **Batch collection**: Collect multiple nearby orbs at once
- **XP popup text**: Show "+10 XP" on collection
- **Streak bonuses**: Extra XP for rapid collections

## Files Modified
- **Modified**: `/Assets/Scripts/Pickups/ExperienceOrb.cs`
  - Added target height offset
  - Added distance-based collection
  - Added configurable collection range
- **Created**: `/Assets/Guide/XP_ORB_IMPROVEMENTS.md`

## Testing Checklist

✓ Orbs fly toward player chest (not feet)  
✓ Orbs collected when close enough  
✓ Works when player is stationary  
✓ Works when player is moving  
✓ No orbs stuck underneath  
✓ No orbs levitating indefinitely  
✓ OnTriggerEnter still works as backup  
✓ Visual bobbing still works  
✓ Rotation still works  
