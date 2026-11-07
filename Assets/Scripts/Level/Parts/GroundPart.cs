using PixelAdventure.Art;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class GroundPart : ILevelPart
    {
        public const string PartId = "Ground";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var segments = context.Layout.GroundSegments;
            for (var i = 0; i < segments.Length; i++)
            {
                var rect = segments[i];
                var block = Blocks.CreateCollider(parent, $"Floor {i + 1}", rect).transform;

                var columns = Mathf.RoundToInt(rect.width);
                var rows = Mathf.RoundToInt(rect.height);
                for (var column = 0; column < columns; column++)
                {
                    var x = rect.xMin + column + 0.5f;

                    // Grass on top. The last row is the bottom edge of the ground; rows in between are fill.
                    var grass = Blocks.Edge(column, columns, GameArt.GrassLeft, GameArt.GrassMiddle, GameArt.GrassRight);
                    Blocks.AddTile(block, grass, new Vector2(x, rect.yMax - 0.5f), RenderOrder.Ground);

                    for (var row = 1; row < rows; row++)
                    {
                        var bottom = row == rows - 1;
                        var middle = bottom
                            ? GameArt.DirtBottom
                            : (column % 3 == 1) ? GameArt.DirtMiddleAlt : GameArt.DirtMiddle;
                        var left = bottom ? GameArt.DirtBottomLeft : GameArt.DirtLeft;
                        var right = bottom ? GameArt.DirtBottomRight : GameArt.DirtRight;
                        var dirt = Blocks.Edge(column, columns, left, middle, right);
                        Blocks.AddTile(block, dirt, new Vector2(x, rect.yMax - row - 0.5f), RenderOrder.Ground);
                    }
                }
            }
        }
    }
}
