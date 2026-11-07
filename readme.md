# Pixel Adventure

A simple 2D pixel-art platformer game built with **Unity 6 and C#**.

## 🎮 Game Overview

The player explores 2D levels, collects coins, avoids enemies and reaches the end of each level.

## 🛠️ Technology

- Game Engine: Unity 6 (Universal 2D)
- Programming Language: C#
- IDE: Visual Studio 2026
- Version Control: Git
- Target Platform: Android (iOS planned)

## 🧭 Development Approach: Code First

Gameplay is built **in C# first**, with the Unity Editor used as little as possible:

- **Scripts create the game objects.** Player, ground, platforms, coins, enemies, camera and HUD are spawned and configured from code (bootstrap and factory scripts) instead of being hand-assembled in scenes.
- **Settings live in code.** Physics values, layers, sorting and sprite import settings are set by scripts, so they are visible in diffs and reproducible on a fresh clone.
- **Scenes stay minimal.** A scene only needs an entry point; everything else is created at runtime. This keeps scene and prefab merge conflicts rare.
- **Prefabs come later.** Once a piece of gameplay is stable in code, it can be turned into a prefab for tuning in the Inspector.
- **Small commits.** Each feature is committed separately, together with the `.meta` files of any new assets.

### How the code fits together

The code is explained as short articles in [docs/README.md](docs/README.md):

| Article | What it answers |
| --- | --- |
| [Parts](docs/parts.md) | Session objects stay up. Level objects are rebuilt. |
| [Data](docs/data.md) | Layout, tuning, and draw order. |
| [Messages](docs/messages.md) | Events, the player marker, and shared input. |
| [A round](docs/a-round.md) | Play, game over, and restart. |

The phase notes below are the build log. The articles describe the code as it is now.

## 🚀 Planned Features

1. Player movement (walk, run and jump)
2. Pixel-art characters and environments
3. Ground detection and collision
4. Camera follow
5. Coins and collectibles
6. Basic enemies
7. Score system
8. Game UI and menus
9. Sound effects and background music
10. Android build

## 📅 Development Plan

| Phase | Description                 | Status    |
| ----- | --------------------------- | --------- |
| 1     | Unity project setup         | Completed |
| 2     | Visual Studio integration   | Completed |
| 3     | Player movement and jumping | Completed |
| 4     | Environment and physics     | Completed |
| 5     | Coins and enemies           | Completed |
| 6     | UI and scoring              | Completed |
| 7     | Level design and polishing  | In progress |
| 8     | Android build and testing   | Pending   |

## 📁 Project Structure

```text
Assets/
├── Animations/
├── Audio/
├── Materials/
├── Prefabs/
├── Scenes/
├── Scripts/
│   ├── Player/
│   ├── Enemies/
│   └── Managers/
├── Sprites/
└── UI/
```

## 🎯 Initial Goal

Build a small, playable 2D platformer with:

- One playable character
- One level
- Basic movement and jumping
- Collectible coins
- One enemy type
- Score display
- Game-over condition

## Graphics (pixel art)

All art is from **[Pixel Adventure by Pixel Frog](https://pixelfrog-assets.itch.io/pixel-adventure-1)**, licensed **CC0** (free for any use, no attribution required; see `Assets/Resources/Art/PixelFrog_License.txt`). Only the sprites the game uses are in the project, renamed to say what they are, in **`Assets/Resources/Art/`** so code can load them by name (`Art/GameArt.cs`).

| File | Used for |
|------|----------|
| `Tiles/grass_left`, `grass_middle`, `grass_right` | Top row of each floor segment |
| `Tiles/dirt_left`, `dirt_middle`, `dirt_middle_alt`, `dirt_right` | Dirt fill, when a floor is more than two tiles thick |
| `Tiles/dirt_bottom_left`, `dirt_bottom`, `dirt_bottom_right` | Bottom row of each floor segment |
| `Tiles/plank_left`, `plank_middle`, `plank_right` | One-way wooden platforms |
| `Tiles/coin_00` … `coin_16` | Apple (spinning) |
| `Tiles/flag_00` … `flag_09` | Checkpoint flag at the end of the level |
| `Characters/player_idle_00` … `player_idle_10` | Player standing |
| `Characters/player_run_00` … `player_run_11` | Player walking and running |
| `Characters/player_jump`, `player_fall` | Player in the air |
| `Characters/enemy_run_00` … `enemy_run_11` | Patrolling enemy |

The player is the Ninja Frog and the enemy is the Pink Man, both from that pack. The same pack also includes orange, purple and stone terrain if the level theme changes later.

**Pixel scale:** tiles are 16×16 px and characters 32×32 px. Everything is imported at **16 pixels per unit**, so one tile is exactly one world unit. Character sprites are pivoted at their feet; the flag is pivoted on its pole. Colliders stay the same world size as before, a little smaller than the character art.

**Import settings in code:** `Editor/PixelArtImporter.cs` applies Sprite mode, 16 PPU, Point filtering, no compression and no mipmaps to every texture under `Assets/Resources/Art/`, so nothing needs to be set in the Inspector. If a sprite ever looks blurry or the wrong size, run **Tools → Pixel Adventure → Reimport Art**.

**Animation:** `SpriteFlipbook` loops frames for coins, the flag and enemies; `PlayerAnimator` plays idle, run (faster while running), jump and fall, and flips the player to face the way they move.

**Adding or replacing art:** copy the PNG into `Assets/Resources/Art/Tiles` or `Characters` (keep the same name to replace one), and add a property for new sprites in `GameArt.cs`.

## Sound

All sounds are by **Kenney**, licensed **CC0** (see `Assets/Resources/Audio/Kenney_License.txt`), from [Digital Audio](https://kenney.nl/assets/digital-audio), [Music Jingles](https://kenney.nl/assets/music-jingles) and the older *Music Loops* pack. They live in **`Assets/Resources/Audio/`** and are loaded by name (`Audio/GameAudio.cs`).

| File | Original | Plays when |
|------|----------|------------|
| `Sfx/jump.ogg` | Digital Audio `phaseJump1` | The player jumps (`PlayerController.Jumped`) |
| `Sfx/coin.ogg` | Digital Audio `highUp` | A coin is collected (`Coin.Collected`) |
| `Sfx/game_over.ogg` | Digital Audio `lowDown` | The player touches an enemy or falls (`GameEvents.PlayerDied`) |
| `Sfx/level_complete.ogg` | Music Jingles `jingles_NES00` | The player reaches the flag (`GameEvents.LevelCompleted`) |
| `Music/music.ogg` | Music Loops `Retro Beat` | Loops during play; stops when the game ends |

**How it works:** `AudioPart` creates a **Sound Player** object with two Audio Sources (looping music, and one-shot effects) and the `SoundPlayer` component (`Audio/SoundPlayer.cs`). It only listens to static events, so it doesn't reference the player, coins or UI, and each part can be rebuilt on its own. Restarting rebuilds the level, which starts the music again. The Audio Listener is on the camera.

**Volume and mute:** music 0.4 and effects 0.8 (`LevelSettings.MusicVolume`, `EffectsVolume`). Press **M** (gamepad **Select**) to mute or unmute; the setting is saved with `PlayerPrefs`.

**Import settings in code:** `Editor/SoundImporter.cs` sets every clip under `Assets/Resources/Audio/` to Vorbis. Effects are mono and decompressed on load so they start instantly, and music is streamed. **Tools → Pixel Adventure → Reimport Audio** reapplies the settings.

**Replacing a sound:** drop another `.ogg` (or `.wav`) over the file with the same name. The downloaded packs have many more jumps, pickups and jingles to choose from.

## Phase 7 — Level design and polish (in progress)

- **Goal flag (done):** `GoalPart` puts a pole with a waving flag at `LevelLayout.GoalX` (x = 43.5, near the right end of the last floor). Its trigger covers the whole pole; touching it raises `GameEvents.LevelCompleted` (`Level/Goal.cs`).
- **End screen (done):** `EndScreen` (`UI/EndScreen.cs`) handles both outcomes: **Level Complete!** on a green tint or **Game Over** on a red tint, each with the score and coins collected (for example *Coins 9/12*). Whichever happens first counts.
- **Already in place:** coins bob, coyote time and jump buffering, camera stops at the level edges.
- **Polish (pending):** spacing of platforms, enemies and coins; more feel tweaks.

## Phase 5b — Enemies and game over (done)

- **Enemies:** `EnemiesPart` creates one walking enemy per floor segment from `LevelLayout.Enemies` (patrol start x, end x and the surface it walks on). Each has a **Kinematic** Rigidbody 2D, so it is moved only by its script, and a trigger collider slightly smaller than its sprite so near misses don't count.
- **Patrol:** `PatrolEnemy` (`Enemies/PatrolEnemy.cs`) walks back and forth between its two x positions at 2 units/s (`LevelSettings.EnemySpeed`).
- **Hit:** touching an enemy raises `GameEvents.PlayerDied` (`Game/GameEvents.cs`). The enemy doesn't know about the UI; anything can listen to the event.
- Jump over enemies to get past them. Touching an enemy or falling into a gap ends the game.

## Phase 6 — Score and game over UI (done)

- **Score display:** `HudPart` creates a uGUI canvas (Screen Space Overlay, scaled for 1920×1080) with **Score N    Coins n/12** in the top-left corner (`UI/ScoreDisplay.cs`). It uses Unity's built-in font, so nothing has to be imported; it can be swapped for a pixel font later.
- **End screen:** `EndScreen` (`UI/EndScreen.cs`, on the HUD canvas) listens to `GameEvents.PlayerDied` and `GameEvents.LevelCompleted`. It freezes the game (`Time.timeScale = 0`) and shows an overlay with **Game Over** or **Level Complete!**, the final score, coins collected and the restart hint.
- **Restart:** **R**, gamepad **Start** or **South**, or a click/tap (for Android). Input is ignored for the first 0.5 s, so a jump press at the moment of death doesn't restart immediately. Restarting calls `LevelBuilder.Rebuild()`, which recreates the whole level, coins and score included.

## Phase 5a — Coins and score (done)

- **Coins:** `CoinsPart` places 12 coins from `LevelLayout.Coins`: on the floor, above every platform, and one over each gap. Each is a spinning gold coin with a trigger collider, and bobs gently up and down.
- **Collecting:** touching a coin raises the static `Coin.Collected` event and removes the coin. Each coin is worth 10 points.
- **Score:** `ScoreCounter` (`Game/ScoreCounter.cs`, on the HUD canvas) listens to that event and keeps the score and coin count. Coins never reference the score, so the **Coins** and **HUD** parts can each be rebuilt on their own.
- Restarting after game over rebuilds the level, which resets the coins and the score.

## Phase 4 — Environment and camera

**Camera follow (done):** `CameraPart` adds **Camera Follow 2D** (`Scripts/Cameras/CameraFollow2D.cs`) and points it at the player, so `CameraPart` is created after `PlayerPart`. The camera:

- smoothly follows the player (`smoothTime` 0.15 s), keeping them about 1.5 units below the screen center (`LevelSettings.CameraOffset`)
- never shows past the level edges: the view stays inside `LevelSettings.CameraBounds` (the floor's width, from the bottom of the floor up to 20 units), and is centered if the level is narrower than the screen
- starts already on the player, so the Editor preview shows the same framing as Play mode
- finds the player again by itself if only the **Player** part is rebuilt

**Level layout (done):** `LevelLayout.cs` defines the level in world units, so changing the level means editing one file:

- **Floor:** three segments from x = -15 to 45 with gaps of 3 and 4 units (walking jumps about 5.5 units, running about 9).
- **Platforms:** seven one-way platforms (`PlatformEffector2D`): solid from above, and you can jump up through them from below. Each is 0.5 units thick and placed within jump reach (about 4 units) of the floor or the platform below.
- Floors and platforms are static **Box Collider 2D**s with tiled sprites, created by the shared `Blocks` helper. Ground detection works on any upward-facing surface, so no layer setup is needed.

**Falling (done):** `FallZonePart` puts a trigger 4 units below the floor, spanning past both level edges. Falling into a gap raises `GameEvents.PlayerDied`, the same as touching an enemy, which shows the game over screen.

## Phase 3 — Player (code-first)

Nothing is set up by hand. `PlayerPart` (`Level/Parts/PlayerPart.cs`) creates `[Generated]/Player/Player` with:

- **Sprite** child with the Ninja Frog, feet on the bottom of the collider, animated by `PlayerAnimator`
- **Collider size** 0.75 × 1.15 units (`LevelSettings.PlayerSize`)
- **Rigidbody 2D**: Dynamic, rotation frozen, Continuous collision, Interpolate, Gravity Scale **2.5** (`LevelSettings.PlayerGravityScale`)
- **Box Collider 2D** with a zero-friction material, so the player doesn't stick to walls
- **Player Controller** (`Scripts/Player/PlayerController.cs`)

**Controls** (Input System actions created in code):

| Action | Keyboard | Gamepad |
|--------|----------|---------|
| Move | **A** / **D** or arrow keys | Left stick |
| Jump | **W**, **Up** or **Space** | South button (A / Cross) |
| Run (hold) | **Left Shift** | West button (X / Square) |
| Mute / unmute | **M** | Select |

**Feel:** walk 5, run 8, jump velocity 14 (about 4 units high). **Coyote time** (0.1 s) still allows a jump just after walking off an edge, **jump buffering** (0.1 s) remembers a press made just before landing, and releasing jump early gives a shorter hop. All values are on the Player Controller component.

**Ground detection** uses the Rigidbody's contacts: only surfaces whose normal points up (45°–135°) count as ground, so walls and ceilings never do. No Ground layer or ground-check object is needed.

**Try it:** open **Tools → Pixel Adventure → Level Builder Window**, click **Rebuild All**, then press **Play** (or tick *Build level when entering Play mode* and press Play from an empty scene).
### Blank or black Game view?

- **"No cameras rendering"**: the level isn't built. Click **Rebuild All** in the Level Builder window, or tick *Build level when entering Play mode*.
- **Sprites look black**: the Global Light 2D is missing. Rebuild the **Lighting** part.

## 🧱 Using This Project as a Base for a New 2D Game

The git tag **`base-template-2d-game`** marks a clean Unity 6 2D baseline with no game content. It contains:

- Unity **6000.3.27f2** with **URP 17.3** and the **URP 2D Renderer** (`Assets/Settings/Renderer2D.asset`) on both quality levels (PC and Mobile)
- Editor **Default Behavior Mode: 2D** (textures import as sprites, scenes open in 2D view)
- One empty scene, `Assets/Scenes/Main.unity`, ready for a code-first bootstrap
- Packages: URP, Input System, 2D Sprite, uGUI, Timeline, Test Framework, Visual Studio and Rider integration
- A Unity `.gitignore` and a `.gitattributes` that stores binary art (PNG, audio, models) in Git LFS

### 1. Copy the baseline

```powershell
git clone --branch base-template-2d-game https://github.com/andissanayake/My-project.git "D:\GameDevelopment\NewGame"
cd "D:\GameDevelopment\NewGame"
Remove-Item .git -Recurse -Force
git init
git lfs install
```

Removing `.git` gives the new game its own history instead of carrying over this one.

### 2. Open it in Unity

1. In **Unity Hub**, choose **Add → Add project from disk** and select the new folder.
2. Open it with **6000.3.27f2**. The first open takes a few minutes while Unity rebuilds the `Library` folder.

### 3. Set the game's identity

In **Edit → Project Settings → Player**:

| Setting | Example | Why |
|---------|---------|-----|
| Company Name | `YourName` | Shown in builds; used for save-data folders |
| Product Name | `New Game` | The game's display name |
| Version | `0.1.0` | First build version |
| Android → Other Settings → **Package Name** | `com.yourname.newgame` | Must be unique per game; Google Play rejects duplicates and two games with the same name overwrite each other on a device |

### 4. Platform and scene

1. **File → Build Profiles → Android → Switch Platform** (if targeting mobile).
2. Optional: rename the `Main` scene from the **Project** window (select it, press **F2**). Renaming there keeps the `.meta` file and the build list in sync.

### 5. Replace this README and make the first commit

1. Replace `readme.md` with one describing the new game.
2. Create an empty repository on GitHub for the new game, then:

```powershell
git add -A
git commit -m "Initial project from base-template-2d-game"
git branch -M main
git remote add origin https://github.com/<you>/<new-game>.git
git push -u origin main
```

### Notes

- Add new rendering layers, tags and sorting layers per game; the baseline only has Unity's defaults.
- Never move or rename assets in File Explorer; always do it in Unity's Project window so `.meta` files follow.
- Commit each new asset together with its `.meta` file.

## 🤖 AI-Assisted Development

Cursor will be introduced after the initial player movement implementation.

AI assistance will be used for:

- Code generation and refactoring
- Debugging
- Feature development
- Code explanations and documentation

## 🏁 Final Goal

Create a small, polished 2D mobile game that can be installed and played on an Android device.
