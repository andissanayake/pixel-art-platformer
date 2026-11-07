namespace PixelAdventure.Level
{
    /// <summary>Sprite and UI draw order. Higher numbers draw in front.</summary>
    public static class RenderOrder
    {
        public const int Ground = 0;
        public const int Platform = 1;
        public const int Goal = 3;
        public const int Coin = 5;
        public const int Enemy = 8;
        public const int Player = 10;
        public const int Hud = 100;
    }
}
