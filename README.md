# Unity Modular Ability System

A flexible, modular ability system for Unity using ScriptableObjects, designed around phased execution and consequence chaining.

## Features

- **ScriptableObject Architecture**: Data-driven design for maximum flexibility
- **Phased Ability Execution**: Abilities run phases in sequence with precise timing control
- **Consequence System**: Chain gameplay effects together to create complex abilities
- **Parameter Passing**: Use a blackboard pattern with ScriptableObject keys for communication
- **Asynchronous Flow**: Built with UniTask for clean execution flow

## Core Components

- **Ability**: Manages cooldown, input handling, and execution
- **AbilityBehaviour**: Controls phase execution and transitions
- **AbilityPhase**: Handles timing and consequence triggering
- **Consequence**: Implements specific gameplay effects
  - SphereOverlap: Area effect using Physics.OverlapSphere
  - RectOverlap: Area effect using Physics.OverlapBox
  - DealDamage: Applies damage to enemies

## Implemented Abilities

- **CircularSlash** (Key 1): 360° area attack with sphere detection
- **QuickSlash** (Key 2): Forward attack using box detection
- **SlashAndStomp** (Key 3): Two-phase attack with forward slash followed by ground stomp

## Usage

1. **Movement**: Control the blue hero cube with WASD keys
2. **Abilities**: Press 1, 2, or 3 to trigger the corresponding ability
3. **Combat**: Damage red enemy cubes with abilities until they are destroyed

## Implementation Notes

The system uses a chain of ScriptableObjects that pass data through a parameter dictionary. This allows for flexible ability design without hardcoding relationships between components.

## Dependencies

- Unity 2022.3.0 or newer
- UniTask package (included)