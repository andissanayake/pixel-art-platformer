using PixelAdventure.Art;
using PixelAdventure.Collectibles;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class CoinsPart : ILevelPart
    {
        public const string PartId = "Coins";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var frames = GameArt.Coin;
            var coins = context.Layout.Coins;
            for (var i = 0; i < coins.Length; i++)
            {
                var go = LevelContext.CreateChild(parent, $"Coin {i + 1}", coins[i]);

                GameArt.AddRenderer(go, frames[0], RenderOrder.Coin);
                go.AddComponent<SpriteFlipbook>().Configure(frames, 12f);

                var collider = go.AddComponent<CircleCollider2D>();
                collider.radius = LevelSettings.CoinRadius;
                collider.isTrigger = true;

                go.AddComponent<Coin>();
            }
        }
    }
}
