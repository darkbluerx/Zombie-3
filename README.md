# **Project: Zombie-3 - Patient X: One Survivor**
- Last Updated: 17.10.2024
- Status: Completed
- Unity Version: 2022.3.6f1
 
Description: This game was developed by Antti Sironen and Toni Luukkanen. A top-down, 3D world horror-action shooter game.

Playable on ITCH.IO: https://blueyurei.itch.io/patient-x-one-survivor


## Table of Contents
- Game Development
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

- 
## Applications Used

- [A* Pathfinding Project Pro](https://arongranberg.com/astar/)`AI Path Finding`
- 3D Tilemap [MGL-3D-Rule-Tiles](https://github.com/michaelsgamelab/MGL-3D-Rule-Tiles/tree/main) `Building the levels with 3d blocks`
- Unity Package: 2d Tilemap Editor, Universal RP

## Credits:

- Developed by Antti Sironen & Toni Luukkanen
- Game Design: Antti Sironen & Toni Luukkanen
- UI Interface: Antti Sironen
- Programming: Antti Sirone
- Level Design: Antti Sironen & Toni Luukkanen
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
