using UnityEngine;
using UnityEngine.Rendering;

namespace PixelAdventure.Art
{
    /// <summary>
    /// Sprites from Pixel Frog's "Pixel Adventure" pack (CC0), loaded from Assets/Resources/Art.
    /// Tiles are 16×16 and characters are 32×32, all imported at 16 pixels per unit, so one tile is one
    /// world unit. Character sprites are pivoted at their feet.
    /// </summary>
    public static class GameArt
    {
        public const string ResourcesFolder = "Art";
        public const int PixelsPerUnit = 16;

        public static Sprite GrassLeft => Tile("grass_left");
        public static Sprite GrassMiddle => Tile("grass_middle");
        public static Sprite GrassRight => Tile("grass_right");
        public static Sprite DirtLeft => Tile("dirt_left");
        public static Sprite DirtMiddle => Tile("dirt_middle");
        public static Sprite DirtMiddleAlt => Tile("dirt_middle_alt");
        public static Sprite DirtRight => Tile("dirt_right");
        public static Sprite DirtBottomLeft => Tile("dirt_bottom_left");
        public static Sprite DirtBottom => Tile("dirt_bottom");
        public static Sprite DirtBottomRight => Tile("dirt_bottom_right");
        public static Sprite PlankLeft => Tile("plank_left");
        public static Sprite PlankMiddle => Tile("plank_middle");
        public static Sprite PlankRight => Tile("plank_right");

        public static Sprite[] Coin => Frames("Tiles/coin_", 17, ref s_Coin);
        public static Sprite[] Flag => Frames("Tiles/flag_", 10, ref s_Flag);

        public static Sprite[] PlayerIdle => Frames("Characters/player_idle_", 11, ref s_PlayerIdle);
        public static Sprite[] PlayerRun => Frames("Characters/player_run_", 12, ref s_PlayerRun);
        public static Sprite PlayerJump => Character("player_jump");
        public static Sprite PlayerFall => Character("player_fall");
        public static Sprite[] Enemy => Frames("Characters/enemy_run_", 12, ref s_Enemy);

        static Sprite[] s_Coin;
        static Sprite[] s_Flag;
        static Sprite[] s_PlayerIdle;
        static Sprite[] s_PlayerRun;
        static Sprite[] s_Enemy;

        public static SpriteRenderer AddRenderer(GameObject go, Sprite sprite, int sortingOrder = 0)
        {
            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;

            // The URP 2D sprite material, so sprites are lit by the Global Light 2D.
            var material = GraphicsSettings.currentRenderPipeline != null
                ? GraphicsSettings.currentRenderPipeline.default2DMaterial
                : null;
            if (material != null)
                renderer.sharedMaterial = material;

            return renderer;
        }

        static Sprite Tile(string name) => Load($"{ResourcesFolder}/Tiles/{name}");

        static Sprite Character(string name) => Load($"{ResourcesFolder}/Characters/{name}");

        static Sprite[] Frames(string prefix, int count, ref Sprite[] field)
        {
            if (field != null) return field;

            var frames = new Sprite[count];
            for (var i = 0; i < count; i++)
                frames[i] = Load($"{ResourcesFolder}/{prefix}{i:00}");
            field = frames;
            return frames;
        }

        static Sprite Load(string path)
        {
            var sprite = Resources.Load<Sprite>(path);
            if (sprite == null)
                Debug.LogError($"Missing sprite 'Assets/Resources/{path}.png'. If the file exists, run Tools > Pixel Adventure > Reimport Art.");
            return sprite;
        }
    }
}
