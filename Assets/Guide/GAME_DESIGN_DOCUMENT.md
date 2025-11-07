# Roguelike Tower Defense - Game Design Document

## Overview
A roguelike tower defense game combining action combat, wave-based defense, and meta-progression. Players select heroes, defend multiple zones from enemy waves, and progress through skill drafting and currency-based upgrades.

---

## Core Loop Structure

### Phase 1: Base/Menu Phase
**Purpose:** Preparation and meta-progression

**Player Actions:**
- Select unlocked hero (melee, ranged, each with unique starter skills/abilities)
- Spend meta-currency earned from previous runs to:
  - Upgrade hero stats (health, damage, speed, etc.)
  - Unlock new hero classes
  - Purchase permanent meta upgrades
- Prepare/choose pre-run perks or loadout bonuses (optional feature)
- Decide on small boosts or initial loadout configurations

**Implementation Requirements:**
- Hero selection UI with hero preview and stat display
- Meta-currency display and persistent storage system
- Upgrade shop UI with stat upgrade options
- Hero unlock system with class progression
- Pre-run loadout/perk selection system (optional)

---

### Phase 2: Run Start
**Purpose:** Commit to the run

**Player Actions:**
- Teleport hero into Zone 1
- Entry is a commitment—no return to base until the run ends

**Implementation Requirements:**
- Scene transition or teleportation system
- Zone loading/initialization
- Run state management (no early exit)

---

### Phase 3: Combat/Defense Phase (Per Wave)
**Purpose:** Active gameplay—defend zones and fight enemies

#### Zone Defense System
**Key Features:**
- Defend 3 zones/towers from incoming waves of monsters
- Each zone/tower represents a defensive position
- If any zone fails (breached by enemies), consequences occur
- Survive waves of monsters attacking several zones/towers
- Lose condition: If any zone fails or hero is defeated

**Implementation Requirements:**
- 3 distinct defense zones/towers in the play area
- Zone health/integrity tracking system
- Enemy wave spawning system targeting zones
- Zone breach detection and consequences
- Visual feedback for zone status (health, under attack, etc.)

#### Obstacle Deployment System
**Key Features:**
- Spend in-game currency (not meta-currency) to place obstacles
- Use in-run resources earned during the current run
- Award currency from:
  - Defeated monsters
  - Completing waves
  - Special map objectives
  - Player must choose between offense, defense, and obstacle investment

**Obstacle Management:**
- Cooldowns or placement limits to prevent flooding key zones
- Consider placement limits per round or enforce minimum spacing between obstacles
- Give obstacles maintenance or decay cost so players must commit ongoing resources to keep them functional

**Tactical Features:**
- Tactical obstacle placement and tower prioritization encourage adaptation each wave/run
- Emergent tactics: Obstacle placement influences enemy pathing and tower load
- Players must choose between different powers, rewarding strategic choices

**Implementation Requirements:**
- In-run currency system (separate from meta-currency)
- Currency drops from enemies and wave completion
- Obstacle placement UI and grid/placement system
- Obstacle types with different effects (slow, damage, block, etc.)
- Cooldown/limit system for obstacle placement
- Maintenance/decay mechanics (optional feature)
- Pathfinding integration for obstacles affecting enemy routes

#### Combat System
**Key Features:**
- Fight enemies with hero abilities
- Basic monsters drop experience orbs for mid-run leveling
- Gold/currency collected and banked for post-run upgrades
- Place/tactically deploy obstacles or support buildings using in-run resources acquired during the run

**Implementation Requirements:**
- Hero combat abilities (basic attack, special skills)
- Enemy AI targeting heroes and zones
- Experience orb drops from basic enemies
- Gold/currency drops and collection system
- Combat feedback (damage numbers, effects, animations)

---

### Phase 4: Mid-Run Progression
**Purpose:** Build diversity and character growth during the run

#### Leveling System
**Key Features:**
- Level up by collecting experience orbs dropped by monsters
- Pick stat upgrades or class-related perks upon leveling
- Stat choices lead to run-to-run variety

**Implementation Requirements:**
- Experience collection and level-up system
- Stat upgrade selection UI (pause or real-time)
- Multiple stat/perk options per level-up
- Class-specific perk trees or options

#### Skill Drafting System (Mid-Run Build Diversity)
**Key Features:**
- Each chest offers random skills/upgrades that can change your play style between previous choices
- Choices lead to run-to-run variety
- Upon defeating elite/boss monsters, receive a chest with randomized skill draft options
- Choose one skill to modify playstyle for current run
- Randomized pool similar to Vampire Survivors' addictive loop

**Elite/Boss Monster Drops:**
- Elite/Boss monsters drop animated chests
- Chests grant a choice of new skills or upgrades (randomized pool)
- Skills picked from chests should occasionally reward synergies that stack powers

**Implementation Requirements:**
- Elite/boss monster identification and spawn system
- Chest drop system with animations
- Skill draft UI with 2-4 random options per chest
- Skill pool database with various effects
- Skill synergy system (skills that combo well together)
- Visual indication of skill combinations/synergies

---

### Phase 5: End Condition
**Purpose:** Run termination and reward distribution

**Loss Conditions:**
- All zones/towers are breached by enemies
- Hero is defeated (health reaches zero)

**Rewards:**
- Earn rewards based on run progress:
  - Gold (for immediate spending, if any remains)
  - Meta-currency (for permanent upgrades)
  - Unlock points (for new heroes/classes)
- Reward scaling based on:
  - Number of waves survived
  - Enemies defeated
  - Zones defended successfully
  - Special objectives completed

**Implementation Requirements:**
- End-of-run results screen
- Reward calculation system based on performance
- Meta-currency award and persistence
- Unlock point system for progression gates
- Statistics tracking (waves survived, kills, damage dealt, etc.)

---

### Phase 6: Meta-Progression & Return to Base
**Purpose:** Permanent progression and preparation for next run

**Player Actions:**
- Return to menu/base to spend upgrades
- Use earned currency/points/experience to:
  - Upgrade current heroes (permanent stat increases)
  - Unlock new classes
  - Purchase meta upgrades (game-wide bonuses)
- Prepare for the next run with enhanced capabilities

**Implementation Requirements:**
- Persistent save system for meta-progression
- Currency/unlock point persistence between runs
- Upgrade purchase and application system
- Hero unlock requirements and gating

---

### Phase 7: Repeat with Variety
**Purpose:** Replayability through randomization

**Randomized Elements:**
- Skill drafts (different options each run)
- Enemy patterns and compositions
- Map layouts (optional)
- Elite/boss spawn timing and types
- Obstacle availability or costs

**Implementation Requirements:**
- Procedural/randomized skill draft pool
- Variable enemy wave compositions
- Random elite/boss spawns
- Seed-based randomization system (optional for consistent runs)

---

## Replayability & Depth Features

### Unique Heroes System
**Design Goals:**
- Each hero starts with a signature ability and distinct playstyle
- Scale differently (melee could get lifesteal, offense, knockbacks; ranged gets piercing, crowd control, kiting)
- Encourage trying different heroes for variety

**Implementation Requirements:**
- Multiple hero classes with distinct starting abilities
- Hero-specific skill trees or upgrade paths
- Signature abilities that define hero identity
- Stat scaling differences per hero archetype

### Synergy Layers
**Design Goals:**
- Skills picked from chests should occasionally reward synergies that stack powers
- Knockbacks + area effects = crowd control combos
- Slow + piercing shot = defensive synergies
- Players must choose between different powers, rewarding strategic choices

**Implementation Requirements:**
- Skill combination detection system
- Bonus effects when specific skill combinations are active
- Visual/audio feedback for synergy activation
- Skill descriptions mentioning potential synergies

### Emergent Tactics
**Design Goals:**
- Tactical obstacle placement influences enemy pathing
- Tower prioritization encourages adaptation each wave/run
- Encourage adaptation each wave/run through dynamic challenges

**Implementation Requirements:**
- Dynamic enemy pathfinding that responds to obstacles
- Multiple valid strategies for defense
- Visible impact of tactical decisions
- Escalating difficulty that requires adaptation

### Randomized Skill Drafts
**Design Goals:**
- Elite drops introduce randomness and new builds every run
- Mirroring Vampire Survivors' addictive loop
- Each run feels unique due to different skill combinations

**Implementation Requirements:**
- Large pool of skills with varied effects
- Weighted randomization for balance
- Guaranteed minimum viable build paths
- No dead-end builds (all combinations should be playable)

### Persistent Progression
**Design Goals:**
- Use meta-currency for upgrades/unlocks
- Even failed runs contribute to overall progression
- Sense of advancement even in defeat

**Implementation Requirements:**
- Meta-currency earned based on performance (not just wins)
- Unlock gates that reward repeated play
- Visible progression metrics
- Multiple progression paths (heroes, stats, abilities, game modifiers)

---

## System Implementation Checklist

### Phase 1: Core Foundation
- [ ] Hero selection and basic stats
- [ ] Single hero with basic movement and attack
- [ ] Single test zone with health tracking
- [ ] Basic enemy spawning system
- [ ] Simple wave system

### Phase 2: Combat & Defense
- [ ] 3 zone defense system
- [ ] Enemy AI targeting zones and hero
- [ ] Hero combat with basic abilities
- [ ] Zone breach detection and loss condition
- [ ] Experience orbs and leveling system
- [ ] Basic stat upgrade UI

### Phase 3: Currency & Obstacles
- [ ] In-run currency system
- [ ] Currency drops from enemies
- [ ] Obstacle placement system
- [ ] Basic obstacle types (block, damage, slow)
- [ ] Cooldown/limit system for obstacles
- [ ] Pathfinding integration

### Phase 4: Skill Drafting
- [ ] Elite/boss enemy types
- [ ] Chest drop system
- [ ] Skill draft UI with random options
- [ ] Skill pool database
- [ ] Skill application and effects
- [ ] Basic synergy detection

### Phase 5: Meta-Progression
- [ ] End-of-run reward calculation
- [ ] Meta-currency system
- [ ] Persistent save system
- [ ] Base/menu shop for permanent upgrades
- [ ] Hero unlock system
- [ ] Results screen with statistics

### Phase 6: Replayability
- [ ] Multiple heroes with unique abilities
- [ ] Randomized skill draft pool
- [ ] Variable wave compositions
- [ ] Synergy system with combos
- [ ] Run-to-run variety through randomization

### Phase 7: Polish & Balance
- [ ] Difficulty scaling and balance
- [ ] Visual feedback and animations
- [ ] Audio feedback
- [ ] UI/UX polish
- [ ] Tutorial/onboarding
- [ ] Balance testing and iteration

---

## Design Notes & Considerations

### Balance Considerations
- In-run currency vs meta-currency split keeps runs self-contained
- Obstacle costs and cooldowns prevent dominant strategies
- Skill draft randomization ensures build variety
- Zone failure consequences should feel fair but punishing
- Meta-progression should provide meaningful power without trivializing content

### Technical Considerations
- Save system must handle meta-progression persistence
- Skill system needs flexible architecture for new additions
- Pathfinding must respond dynamically to obstacle placement
- UI must support real-time or paused decision-making
- Performance considerations for many enemies and obstacles

### Scope Management
- Start with 1-2 heroes, expand later
- Begin with 3-5 core obstacle types
- Initial skill pool of 10-15 options
- 5-10 waves for first prototype
- Expand content after core loop validation

### Future Expansion Ideas
- Multiple map layouts
- Special event waves
- Daily/weekly challenges
- Alternative game modes
- Co-op multiplayer
- Endless mode
- Achievement system

---

## Success Metrics

### Core Loop Validation
- Players complete at least 3 runs in a session
- Run duration: 10-20 minutes ideal
- Clear understanding of meta-progression within first 2 runs
- Desire to try "one more run" after loss

### Engagement Metrics
- Different builds attempted per 10 runs
- Hero variety usage
- Obstacle utilization rate
- Skill synergy discovery rate

### Balance Metrics
- Average waves survived
- Meta-progression rate (time to unlock new content)
- Win rate after X runs
- Difficulty curve feedback

---

**Document Version:** 1.0  
**Last Updated:** Initial Creation  
**Status:** Living Document - Update as design evolves
