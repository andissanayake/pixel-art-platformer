using UnityEngine;

namespace PixelAdventure.Level
{
    /// <summary>
    /// Where things are in one level, in world units. Jump reach for reference: about 4 units high,
    /// about 5.5 units across a gap when walking and 9 when running.
    /// <see cref="LevelBuilder"/> chooses a layout and passes it to parts through <see cref="LevelContext"/>.
    /// </summary>
    public sealed class LevelLayout
    {
        // The wooden plank fills the top 5 pixels of its 16px tile.
        public const float PlatformThickness = 5f / 16f;

        // The player falling this far below the floor ends the game.
        public const float FallZoneDepth = 4f;

        // The view covers the level's width, from the floor's bottom edge up to this height.
        const float CameraHeight = 20f;

        public static readonly LevelLayout Level1 = CreateLevel1();

        public float GroundTop { get; }
        public float GroundThickness { get; }
        public float GroundBottom => GroundTop - GroundThickness;

        // Floor pieces from left to right. Edges are whole numbers: the floor is drawn as 1×1 tiles.
        public Rect[] GroundSegments { get; }
        public Rect[] Platforms { get; }
        public Vector2[] Coins { get; }
        public float GoalX { get; }
        public Patrol[] Enemies { get; }
        public Vector3 PlayerSpawn { get; }
        public Rect CameraBounds { get; }

        public float MinX => GroundSegments[0].xMin;
        public float MaxX => GroundSegments[GroundSegments.Length - 1].xMax;

        public readonly struct Patrol
        {
            public readonly float MinX;
            public readonly float MaxX;
            // Top of the surface the enemy walks on.
            public readonly float SurfaceY;

            public Patrol(float minX, float maxX, float surfaceY)
            {
                MinX = minX;
                MaxX = maxX;
                SurfaceY = surfaceY;
            }
        }

        public LevelLayout(
            float groundTop,
            float groundThickness,
            Rect[] groundSegments,
            Rect[] platforms,
            Vector2[] coins,
            float goalX,
            Patrol[] enemies,
            Vector3 playerSpawn,
            Rect cameraBounds)
        {
            GroundTop = groundTop;
            GroundThickness = groundThickness;
            GroundSegments = groundSegments;
            Platforms = platforms;
            Coins = coins;
            GoalX = goalX;
            Enemies = enemies;
            PlayerSpawn = playerSpawn;
            CameraBounds = cameraBounds;
        }

        /// <summary>
        /// Builds camera bounds from this level's floor: its width, from the bottom edge up <see cref="CameraHeight"/> units.
        /// </summary>
        public static Rect BoundsAboveFloor(float minX, float maxX, float groundBottom) =>
            Rect.MinMaxRect(minX, groundBottom, maxX, groundBottom + CameraHeight);

        static LevelLayout CreateLevel1()
        {
            const float groundTop = -3f;
            const float groundThickness = 2f;
            var groundBottom = groundTop - groundThickness;

            // Spaces between the pieces are gaps to jump over.
            var ground = new[]
            {
                Ground(-15f, 5f, groundBottom, groundTop),
                Ground(8f, 22f, groundBottom, groundTop),
                Ground(26f, 45f, groundBottom, groundTop),
            };

            var platforms = new[]
            {
                Platform(centerX: -2f, top: -1f, width: 3f),
                Platform(centerX: 2f, top: 1.5f, width: 3f),
                Platform(centerX: 13f, top: -0.5f, width: 4f),
                Platform(centerX: 18f, top: 2f, width: 3f),
                Platform(centerX: 31f, top: -1f, width: 3f),
                Platform(centerX: 35f, top: 1.5f, width: 3f),
                Platform(centerX: 40f, top: -0.5f, width: 3f),
            };

            // About 0.75 above the floor or a platform top, plus one over each gap.
            var coins = new[]
            {
                new Vector2(-8f, -2.25f),
                new Vector2(-2f, -0.25f),
                new Vector2(2f, 2.25f),
                new Vector2(6.5f, -0.5f),
                new Vector2(13f, 0.25f),
                new Vector2(18f, 2.75f),
                new Vector2(20f, -2.25f),
                new Vector2(24f, -0.5f),
                new Vector2(31f, -0.25f),
                new Vector2(35f, 2.25f),
                new Vector2(40f, 0.25f),
                new Vector2(41.5f, -2.25f),
            };

            // One enemy per floor segment, each patrolling well inside its segment.
            var enemies = new[]
            {
                new Patrol(-6f, 0f, groundTop),
                new Patrol(14f, 21f, groundTop),
                new Patrol(29f, 38f, groundTop),
            };

            var minX = ground[0].xMin;
            var maxX = ground[ground.Length - 1].xMax;
            return new LevelLayout(
                groundTop,
                groundThickness,
                ground,
                platforms,
                coins,
                goalX: 43.5f,
                enemies,
                playerSpawn: new Vector3(-12f, groundTop + LevelSettings.PlayerSize.y / 2f, 0f),
                cameraBounds: BoundsAboveFloor(minX, maxX, groundBottom));
        }

        static Rect Ground(float xMin, float xMax, float groundBottom, float groundTop) =>
            Rect.MinMaxRect(xMin, groundBottom, xMax, groundTop);

        static Rect Platform(float centerX, float top, float width) =>
            Rect.MinMaxRect(centerX - width / 2f, top - PlatformThickness, centerX + width / 2f, top);
    }
}
