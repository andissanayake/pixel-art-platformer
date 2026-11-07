# Data

Three kinds of facts live in code, in three different places. A position that changes per level does not sit next to a volume that is the same in every level.

## LevelLayout

`LevelLayout` is one level: floors, platforms, coins, enemies, the goal, the player spawn, and the camera bounds. `LevelLayout.Level1` is the level the game ships with.

Parts do not read a global layout. `LevelBuilder` chooses one and passes it in:

```csharp
LevelBuilder.RebuildLevel(layout);
```

Inside a part, the positions are `context.Layout.Coins`, `context.Layout.GroundSegments`, and so on. A second level is another `LevelLayout` next to `Level1`, passed into `RebuildLevel`. Restart keeps whichever layout is already current.

Jump reach, for placing things: about 4 units high, about 5.5 units across a gap when walking, and about 9 when running. Floor edges are whole numbers because the floor is drawn as 1×1 tiles.

`FallZoneDepth` is the same rule for every level: the player dies 4 units below the floor. The pit's left and right edges still come from that level's floor.

## LevelSettings

`LevelSettings` is tuning that does not change from level to level.

| Value | Used for |
| --- | --- |
| `PlayerSize`, `EnemySize`, `CoinRadius` | colliders |
| `PlayerGravityScale` | how heavy the player feels |
| `EnemySpeed` | patrol speed |
| `CameraSize`, `CameraOffset` | how much of the level you see, and where the player sits in the frame |
| `SkyColor` | the camera background |
| `MusicVolume`, `EffectsVolume` | loudness |

Spawn position and camera bounds are on the layout, because they follow the floor. Camera offset stays in settings, because the player is kept about 1.5 units below the center of the screen in every level.

## RenderOrder

Sprites that share a space need a draw order. Higher numbers draw in front. The numbers live in `RenderOrder`, not as raw literals in each part.

| Name | Order |
| --- | --- |
| Ground | 0 |
| Platform | 1 |
| Goal | 3 |
| Coin | 5 |
| Enemy | 8 |
| Player | 10 |
| Hud | 100 |

## Shared builders

Two helpers build objects that several parts would otherwise copy.

`Blocks` makes a rectangular collider and fills it with 1×1 tiles. Ground and platforms use it. `Blocks.Edge` picks the left, middle, or right sprite for a column.

`Actors` makes a character: a box collider, a rigidbody, and a sprite child whose feet sit on the bottom of the collider. The player and the enemies use it. Each caller still sets what is different. The player is dynamic, with gravity and no friction. An enemy is kinematic and its collider is a trigger.

Art files are loaded by name from `GameArt`. Sounds are loaded by name from `GameAudio`. Both read `Assets/Resources/`.
