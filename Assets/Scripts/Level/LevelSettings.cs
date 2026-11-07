using UnityEngine;

namespace PixelAdventure.Level
{
    public static class LevelSettings
    {
        // Flat blue of the Pixel Adventure background.
        public static readonly Color SkyColor = new Color(188f / 255f, 203f / 255f, 213f / 255f);

        // Colliders, in world units (1 unit = one 16px tile). Slightly smaller than the sprites so edges feel fair.
        public static readonly Vector2 PlayerSize = new Vector2(0.75f, 1.15f);
        public static readonly Vector2 EnemySize = new Vector2(0.75f, 0.65f);
        public const float CoinRadius = 0.35f;

        public const float EnemySpeed = 2f;

        // A full jump is about 4 units. This framing keeps the ground they left in view at the top of it.
        public const float CameraSize = 6.5f;
        // Keeps the player a little below the screen center. The same in every level.
        public static readonly Vector2 CameraOffset = new Vector2(0f, 1.5f);

        public const float PlayerGravityScale = 2.5f;

        // 0 to 1. Music sits under the effects so jumps and coins stay audible.
        public const float MusicVolume = 0.4f;
        public const float EffectsVolume = 0.8f;
    }
}
