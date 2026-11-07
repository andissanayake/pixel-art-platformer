# Agent guide

Island Adventure is a 2D pixel-art action platformer set in Sri Lankan towns. This repository is the Unity 6, C# prototype that game is built on.

## Read these first

| Question | Document |
| --- | --- |
| Who is the player, and what are the six worlds? | [readme.md](readme.md) |
| How do the scripts fit together? | [docs/README.md](docs/README.md) |
| What is already playable, and where are the art and sound credits? | [docs/prototype.md](docs/prototype.md) |
| What do we build next, and in what order? | [docs/roadmap.md](docs/roadmap.md) |

The code articles, in order: [parts](docs/parts.md), [data](docs/data.md), [messages](docs/messages.md), [a round](docs/a-round.md).

## Two names

| Name | Use it for |
| --- | --- |
| Island Adventure | The game, the design README, anything the player would read |
| Pixel Adventure | C# namespace `PixelAdventure`, menu `Tools/Pixel Adventure`, Pixel Frog's CC0 art pack |

The playable build is one character (Ninja Frog placeholder), one level, walk / run / jump, 12 coins, one patrolling enemy, a goal flag, score, and game over. Colombo: Pettah Trouble is the first level to build next. Weapons, the second character, emblems, and the other towns are still design.

## Design changes

Record player-facing decisions in `readme.md`: characters, weapons, towns, enemies, bosses, collectibles, and the weekly plan.

A weekly plan describes what the player sees, does, and unlocks. It does not include programming tasks, C# scripts, Unity setup, architecture, or backend design.

## Code changes

Gameplay is created in C#. Do not hand-assemble the level in the `Main` scene.

`LevelBuilder` is the only place that decides which parts exist and in what order. Each part implements `ILevelPart` and is parented under `[Generated]/<Id>`.

Session parts stay up for the whole play session: Input, Lighting, Camera, HUD, Audio. Level parts are destroyed and created again on restart: Ground, Platforms, FallZone, Coins, Enemies, Goal, Player. Something that should survive a restart goes on the session list. Something that belongs to one level goes on the level list.

`LevelLayout` holds floors, platforms, coins, enemies, the goal, the spawn, and the camera bounds for one level. `LevelSettings` holds tuning that is the same on every level. A second level is another `LevelLayout` passed into `RebuildLevel`.

`GameEvents` is how a cause reaches a reaction. Raise an event from the object that causes it. Subscribe in `OnEnable` and unsubscribe in `OnDisable`. The first of `PlayerDied` and `LevelCompleted` ends the round.

Input is read from `GameInput`. The player controller does not own the bindings.

Art files live in `Assets/Resources/Art/` and sounds in `Assets/Resources/Audio/`. Add a property in `GameArt` or `GameAudio` for a new file. Keep the Pixel Frog and Kenney license files next to the assets they cover.

After a gameplay change, open **Tools → Pixel Adventure → Level Builder Window**, click **Rebuild All**, and press Play. If a preview is already in the scene, it is rebuilt after scripts recompile and removed before the scene is saved.

## When the docs should move with the code

Update the matching article when the change alters a fact those docs state:

- A new part, or a change to what survives a restart: `docs/parts.md`
- A new layout field or a tuning value: `docs/data.md`
- A new event, or a new listener: `docs/messages.md`
- A change to start, death, completion, or restart: `docs/a-round.md`
- A finished engineering step, a new control, or a new asset: `docs/prototype.md`
