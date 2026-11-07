using PixelAdventure.Art;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class PlatformsPart : ILevelPart
    {
        public const string PartId = "Platforms";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var platforms = context.Layout.Platforms;
            for (var i = 0; i < platforms.Length; i++)
            {
                var rect = platforms[i];
                var collider = Blocks.CreateCollider(parent, $"Platform {i + 1}", rect);

                // The plank art sits in the top of its tile, so tiles hang down from the platform top.
                var columns = Mathf.RoundToInt(rect.width);
                for (var column = 0; column < columns; column++)
                {
                    var plank = Blocks.Edge(column, columns, GameArt.PlankLeft, GameArt.PlankMiddle, GameArt.PlankRight);
                    Blocks.AddTile(collider.transform, plank, new Vector2(rect.xMin + column + 0.5f, rect.yMax - 0.5f), RenderOrder.Platform);
                }

                // One-way: solid from above, the player can jump up through it from below.
                collider.usedByEffector = true;
                var effector = collider.gameObject.AddComponent<PlatformEffector2D>();
                effector.useOneWay = true;
                effector.surfaceArc = 160f;
            }
        }
    }
}
