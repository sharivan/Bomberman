# Bomberman
Fan game based on the original Bomberman game from NES and SNES consoles.

**Requirements:**

- .NET Framework version 4.8.1 or higher.

**Building from the sources:**

This project was developed in C# and compiled under Visual Studio 2017 Enterprise Edition.
There is no external dependencies for this project.

**How to play:**

This game was based on Bomberman games from NES and SNES consoles. The original game was developed by Hudson Soft in 1983 for the NES (Nintendo Entertainment System).
The game consists in a stratrategic, maze-based game where you control the player moving in a bidimensional area.
Your movement is limited only to free cells at vertical and horizontal directions.
You must kill all enemies and find the exit portal for each level to advance to next level. You can kill enemies and destroy soft blocks planting bombs.

There is four type of bombs:
  - Default Bomb: Is self detonated by about 2 seconds.
  - Remote Control Bomb: Can be detonated by player command.
  - Super Bomb: A red bomb with a explosion that penetrate soft blocks.
  - Super Remote Control Bomb: A combination of Super Bomb with Remote Control Bomb.

Both bomb types also can be detonated when they are damaged by the explosion of other bomb, this can be used to make a chain detonation.
The number of levels is infinite and there is no bosses.

There is two types of enemies:
  - Creep: An enemy that die with only one hit. Always move in direction and only change his direction (randomly) when reach the end of his way.
  - Cactus: An enemy that die with two hits, with a invincibility time after the first hit. He always try to follow the player as possible, otherwise he move randomly over the level.

There is 12 types of power ups in the game, items that grants special skills or upgrades to the player:
  - Extra Bomb: Increment the number of simultaneous bombs that player can plant.
  - Explosion Expander: Increment the reach of explosion.
  - Kick: Allow the player to kick bombs.
  - Block Passer: Allow the player to pass through soft blocks.
  - Bomb Passer: Allow the player to pass through bombs.
  - Accelerator: Make player speed twice than normal speed.
  - Time: Add 3 extra minutes to the remaining time.
  - Bomberman: Add an extra life.
  - Heart: Add an extra heart.
  - Remote Control: Turn the player bombs into Remote Control Bombs.
  - Super Bomb: Turn the player bombs into Super Bombs.
  - Indestructible Armor: Make the player invincible by 15 seconds.

**To Do:**

- Add volume control to music and fx sounds.
- Add support to two players.
- Add support to netplay.
- Add support to render the graphics using Direct X and Open GL. For now the graphics are renderized using GDI only.

**Developers:**

- [Sharivan](https://github.com/sharivan): Programmer. Responsible to make the whole game logic, building the engine from the scratch.
- [Fanzack](https://github.com/danielhidalgojunior): Spriter. Responsible to make the Cactus sprite based on the original Creep, some power up sprites and bomberman sprite animation.

**References:**

- Most of graphics of this game was based on [this tutorial](https://gamedevelopment.tutsplus.com/tutorials/create-a-bomberman-inspired-game-in-construct-2-the-player-and-the-level--gamedev-8744) (or [this video](https://www.youtube.com/watch?v=aopm1jqvXMM)) developed at Construct 2, but only graphics, the game logic was totaly developed from scratch in C#.
- You can also find the sprites at http://enva.to/BombingChapSprites
- FX musics are from original ones. Visit https://www.spriters-resource.com for find then searching by "bomberman".
- Soundtrack musics are no copyright songs.
