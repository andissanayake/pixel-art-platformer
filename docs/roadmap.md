# Roadmap

This is the build order from today's prototype to the game in the [design README](../readme.md). The README's weekly plan decides what the player gets. This page decides the order in which code delivers it.

**Skeleton first, art last.** The whole game loop is built first: title, character select, town map, level select, play, results, unlock, save. Every screen and level uses placeholder graphics. No step waits for finished art. When real art arrives, it replaces a placeholder by file name, and the code does not change.

Every step ends with something you can play. Finish a step, play it, commit it, then start the next one. Each step also names the docs it changes, following [AGENTS.md](../AGENTS.md).

## Where we start

One level, one placeholder character, walk / run / jump, 12 coins, three patrolling enemies, a goal flag, score, and game over. There are no menus and nothing is saved.

## How placeholders work

- **A missing sprite never breaks the game.** `GameArt` returns a flat coloured box with the right size and pivot when a file is missing. Today it logs an error and returns nothing. A missing file shows up as a box instead.
- **Every future asset has a name now.** The boy, the girl, each enemy kind, each boss, the emblems, the town map, and the tiles for each town get a slot in `GameArt` from the start, even with no file behind it.
- **Characters and enemies borrow the Pixel Frog pack.** It has four characters (Ninja Frog, Mask Dude, Pink Man, Virtual Guy) and several terrain colours. A town can use a different terrain colour until it has its own tiles.
- **Menus use Unity's built-in font and plain panels.** A pixel font and framed buttons come with the art pass.
- **Placeholder sound stays Kenney's.** One jingle per town can come later.

## Milestone 1 — The skeleton

Steps 1 to 10 deliver the full loop, from starting the game to unlocking the next town. Every graphic in it is a placeholder.

### Step 1 — Placeholder fallback

A sprite file that does not exist draws as a coloured box. Slots are added for the boy, the girl, emblems, and the town map.

- `GameArt` returns a generated box sprite instead of `null`, and logs a warning once per name.
- Docs: `prototype.md` (how placeholders work).

**Done when** deleting a sprite file shows a box in its place, and the game still plays.

### Step 2 — Screens and flow

The game has screens: Title, Character Select, Town Map, Level Select, Level, and Results. A screen opens, closes, and hands over to the next one. Only the Level screen builds the world.

- The session parts (input, camera, HUD, audio) stay up across screens. Level parts are built when a level starts and cleared when it ends.
- Back goes to the previous screen (Escape, gamepad East, Android back button).
- Docs: `parts.md` (screens and the session), `a-round.md` (a round now starts from Level Select).

**Done when** Play goes from Title to the current test level and back to Title, with no level objects left behind.

### Step 3 — Title screen

The game name, **Play**, **Settings**, and **Quit**. Quit is hidden on mobile.

**Done when** the game opens on the title, and Play moves to Character Select.

### Step 4 — Character select

The screen offers **Boy** and **Girl**. Each one is a borrowed Pixel Frog character with a different tint, a name label, and an idle animation. The choice is saved and used in every level.

- A character catalog lists both characters, with the same movement and the sprite names each one uses.
- `PlayerPart` builds the selected character.
- Docs: `data.md` (character catalog).

**Done when** choosing Girl, quitting, and starting again puts the Girl in the level.

### Step 5 — The world catalog

All six towns and all 18 levels exist as data: town name, level names, boss name, and which level holds the boss. Every level has a `LevelLayout`. Most are copies of the test level with a different terrain colour, so the whole progression can be walked through before any level is designed.

- One catalog lists towns in order and their three levels in order. Nothing else holds town or level names.
- Docs: `data.md` (world catalog).

**Done when** every one of the 18 levels can be started from code and finished.

### Step 6 — Town map

Six town buttons on a plain background, in journey order: Colombo, Galle, Kandy, Ella, Nuwara Eliya, Jaffna. A locked town is greyed out with a lock. An open town shows its name and how many of its three levels are done.

**Done when** only Colombo is open on a fresh save.

### Step 7 — Level select

Inside a town, three level buttons. Level 1 is open. Each level opens when the one before it is finished. A finished level shows its best score and coins.

**Done when** Colombo shows Level 1 open, and Levels 2 and 3 locked.

### Step 8 — Results, retry, and pause

Finishing a level shows Results: score, coins out of total, and **Next**, **Retry**, and **Map**. Dying shows **Retry** and **Map**. A pause button stops play and offers **Resume**, **Retry**, and **Map**.

- The current end screen becomes Results. Restart keeps its current behaviour inside a level.
- Docs: `a-round.md`.

**Done when** finishing Colombo Level 1 opens Level 2, and Next starts it.

### Step 9 — Saving progress and emblems

Progress is saved: finished levels, best scores, unlocked towns, the chosen character, and town emblems. Finishing a town's last level earns its emblem and opens the next town. Settings has **Reset progress**.

- Saved with `PlayerPrefs` behind one small save class, so it can move to a file later.
- Docs: `data.md` (what is saved), `messages.md` (level finished and town finished events).

**Done when** finishing all three Colombo levels earns the Colombo emblem, opens Galle, and still shows both after the game restarts.

### Step 10 — Settings, touch, and Android

Settings has music volume, effects volume, and mute. On-screen buttons cover move, jump, and pause. The skeleton is built and played on an Android phone.

- Touch buttons feed the same `GameInput` actions as the keyboard.
- Docs: `prototype.md` (engineering phase 8, touch controls).

**Done when** a fresh install on a phone can go from Title to the Galle unlock using touch only.

## Milestone 2 — Gameplay systems, with placeholder art

Each system is built once on the test level and then used by every town. Attacks, hitboxes, and projectiles are coloured shapes until they have art.

| Step | System | Done when |
| --- | --- | --- |
| 11 | **Health.** Three hearts, knockback, a moment of safety after a hit. Falling still ends the run. | A third hit shows the death screen. |
| 12 | **Cricket bat.** An Attack button and a short hitbox. Enemies have health. | A swing defeats a basic enemy and adds score. |
| 13 | **Slingshot.** A Shoot button and stones on a cooldown. | A stone defeats an enemy from across a gap. |
| 14 | **Crates, switches, and gates.** The bat breaks crates. A stone hits a switch, which opens a gate. | A gate blocks the goal until its switch is hit. |
| 15 | **Checkpoints.** Dying sends you back to the last checkpoint, with coins kept. | Dying late in a level does not restart it. |
| 16 | **Enemy kinds.** Trouble-maker (chases), slingshot bandit (shoots), charger (rushes), flyer, rolling obstacle. | Each kind can be placed from the layout. |
| 17 | **Boss framework.** A closed arena, a boss health bar, attack phases, and a defeat event. | A box-shaped test boss with two phases can be beaten. |
| 18 | **Pickups.** Health items and hidden collectibles, counted on Results. | A hidden collectible shows as found in Level Select. |

New parts and events from these steps go in `parts.md` and `messages.md` as each one lands.

## Milestone 3 — Greybox levels

Levels are designed with placeholder tiles, so layout and difficulty can be tested before art.

- **Step 19 — Colombo, all three levels.** Pettah Trouble follows the five areas in the design. Tuk-tuk traffic is a moving coloured box.
- **Steps 20–24 — One town per step**, each built on the one new piece of play the design gives it.

| Step | Town | New piece of play | Boss |
| --- | --- | --- | --- |
| 20 | Galle | Waves, fishing nets, moving boat platforms | Harbour Boss |
| 21 | Kandy | Parade obstacles that move on a pattern | Mechanical Parade Boss |
| 22 | Ella | Passing trains and a chase sequence | Train Chaser |
| 23 | Nuwara Eliya | Slippery ground and falling tea baskets | Tea Factory Boss |
| 24 | Jaffna | Hidden passages and environmental puzzles | Main Gang Leader |

## Art pass — whenever art is ready

Art does not have to wait for the steps above, and the steps above do not wait for art. A finished asset is dropped into `Assets/Resources/Art/` under its slot name, and the placeholder for that slot goes away. Art can arrive in any order: the boy and girl first, then Colombo tiles, menus, and so on.

## Milestone 4 — Finish

- **Step 25 — Rewards.** Outfits, effects, and anything else coins can buy.
- **Step 26 — Balance pass** over difficulty, coin counts, and boss health across all 18 levels.
- **Step 27 — iOS build** and store preparation for both platforms.

## Keeping this page true

When a step is done, mark it here and move what it built into the matching article. This page lists only what is still ahead and the order it comes in.
