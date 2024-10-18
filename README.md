# **Project: Zombie-3 - Patient X: One Survivor**
- Last Updated: 17.10.2024
- Status: Completed
- Unity Version: 2022.3.6f1
 
Description: This game was developed by Antti Sironen and Toni Luukkanen. A top-down, 3D world horror-action shooter game.

Playable on ITCH.IO: https://blueyurei.itch.io/patient-x-one-survivor


## Table of Contents
- Controls
- Applications Used
- Credits
- Sources


## Controls

- Pause/Options: P 
- Moving: A, S, D, W
- Run: Shift
- Shooting: Left Mouse
- Reload: R 
- Action: F
-
<br>
## Game Development - Scripts
- *Enemy Scripts:*
- Enemy, controls the playback of the enemy's sounds, damage functions, inherits the enemy's stats from the Scriptable object
- ZombieAnimations, control the zombies's animations using events.
- EnemyStatsSO, Scriptable object that stores the enemy's stats. Used by Enemy and Health scripts
- ZombieSpawner , spawns the zombies in the game
- Health,this script is used to manage the zombie's health.

<br>
- *Gun & Bullet Scripts:*

<br>
- *Levels:*
- Door
- DoorAnimation, controls the door's animations using events.
- PlayerHint, controls the players hint UI

<br>
- *Player Scripts:* 
- PlayerTopDownController, controls the weapon's aim and movement
- PlayerAnimations, control the player's animations using events.
- Player, controls the playback of the player's sounds, damage functions, inherits the player's stats from the Scriptable object 
- UnitsStatsSO, Scriptable object that stores the player's stats. Used Player and Health s
- Health,this script is used to manage the player's health.

<br>
- *UI*
- HealthBar, controls the player's health bar
- Menu
- Map

## Applications Used

- [A* Pathfinding Project Pro](https://arongranberg.com/astar/)`AI Path Finding`
- 3D Tilemap [MGL-3D-Rule-Tiles](https://github.com/michaelsgamelab/MGL-3D-Rule-Tiles/tree/main) `Building the levels with 3d blocks`
- Unity Package: 2d Tilemap Editor, Universal RP

## Credits:

- Developed by Antti Sironen & Toni Luukkanen
- Game Design: Antti Sironen & Toni Luukkanen
- UI Interface: Antti Sironen
- Programming:
Animations: player and enemy animations
Enemies: enemy behavior, damage, spawn system
Items: weapon and ammunition collection
Menu: main menu, level menu, sound settings
Player controls: movement/aiming
Sounds: sound playback through events
UI: weapon/weapon information, health bar, menus (menu, levels)
Units: health and damage system, stat inheritance from ScriptableObjects
Weapons & ammunition: weapon functions, shooting, reloading, weapon switching, ammunition usage. Ammunition functions, damage, hits
World: traps (damage, animation, functions), doors, tasks/puzzles

<br>

- Level Design: Antti Sironen & Toni Luukkanen
- Lighting & Post Processing: Antti Sironen
- Level Making: Antti Sironen
- 3D models & Animations: Toni Luukkanen
- Video Editing: Antti Sironen


## Sources

UI
- https://www.freepik.com/
- https://playground.com/design

TEXT
- Baron Kuffner
https://www.1001fonts.com/baron-kuffner-font.html


AUDIO
- Sounds: https://freesound.org/ 
- Soundtracks https://www.newgrounds.com/

TRAP MODELS & COFFING
- https://www.cgtrader.com/
