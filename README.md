#  Unity - System for tracking progress and position in a racing game

The purpose of this project is to assist in tracking the car's progress and position in a racing game.This repository only contains a prefab made of cubes and a script. No models or UI elements are included.

![car](https://github.com/andre-mana/RaceProgressPositionUI-unity/assets/172200018/e2f0cf43-7081-4086-b541-7f7491b43aab)

## Usage 

- Tag the player's car as "Player".
- Tag all AI drivers as "AIDriver".
- Drag the Path prefab into the scene.
- Drag the script PathUIController.cs onto the Path Prefab.
- Drag the position and progress UI Text elements into their respective slots in the Path inspector.

The Path prefab consists of a sequence of waypoints (cubes). The red line in the next image represents the connection between the cubes in sequence, forming a continuous path.
You can add as many waypoints as you wish along a road or create any path you desire! To add a waypoint, simply add a new cube as a child of the path prefab. 

![path](https://github.com/andre-mana/RaceProgressPositionUI-unity/assets/172200018/9cf4b363-805c-4ce7-b01a-482b574a0bb2)
