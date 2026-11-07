# A round

A round is one try at the current level: from the moment play starts until the player dies or reaches the flag, and then the restart.

## Start

`GameBootstrap` runs after the first scene loads and calls `LevelBuilder.Rebuild`. That builds the session and `LevelLayout.Level1`.

The HUD shows score 0. `SoundPlayer` starts the music. The camera snaps to the player.

## Play

The player moves. Coins raise `CoinCollected`. `ScoreCounter` adds the value and tells `ScoreDisplay` to refresh. `SoundPlayer` plays the coin sound. Nothing in that chain holds a reference to the others.

## End

Touching an enemy or falling into the pit raises `PlayerDied`. Touching the flag raises `LevelCompleted`. Whichever happens first sets `GameEvents.HasEnded`.

`EndScreen` pauses the game with `Time.timeScale = 0` and shows **Game Over** or **Level Complete!**, the score, and the coins collected. `SoundPlayer` stops the music and plays the matching sting.

```mermaid
sequenceDiagram
  participant Hazard as Enemy, pit, or goal
  participant Events as GameEvents
  participant Screen as EndScreen
  participant Sound as SoundPlayer

  Hazard->>Events: RaisePlayerDied or RaiseLevelCompleted
  Events->>Events: HasEnded = true
  Events->>Screen: show the overlay and pause
  Events->>Sound: play the sting and stop the music
  Note over Events: A second ending in the same moment is ignored
```

## Restart

R, gamepad Start or South, or a tap raises `GameInput.RestartPressed`. For the first half second the end screen ignores it.

`GameSession.RestartLevel` then does three things, in order:

1. Set `Time.timeScale` back to 1.
2. `LevelBuilder.RebuildLevel`, which destroys and recreates the level content and points the camera at the new player.
3. `GameEvents.RaiseLevelRestarted`.

The listeners reset themselves. The score goes back to 0. The overlay hides. The music starts again. The camera, the HUD object, and the sound player were never destroyed.

```mermaid
sequenceDiagram
  participant Input as GameInput
  participant Screen as EndScreen
  participant Session as GameSession
  participant Builder as LevelBuilder
  participant Events as GameEvents

  Input->>Screen: RestartPressed
  Screen->>Session: RestartLevel
  Session->>Builder: RebuildLevel
  Note over Builder: Level parts are new. Session parts stay.
  Session->>Events: LevelRestarted
  Events->>Screen: hide the overlay
  Events-->>Events: score resets, music restarts
```

**Rebuild All** is different. It destroys the session too, then builds everything from scratch. That is what Play mode and the editor preview use. Restart during play does not do that.
