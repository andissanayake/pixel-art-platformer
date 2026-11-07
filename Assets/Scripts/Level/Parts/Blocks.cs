using PixelAdventure.Art;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    /// <summary>
    /// Static rectangular level geometry: one collider for the whole rectangle, drawn as a row or grid of
    /// 1×1 tiles.
    /// </summary>
    static class Blocks
    {
        public static BoxCollider2D CreateCollider(Transform parent, string name, Rect rect)
        {
            var go = LevelContext.CreateChild(parent, name, rect.center);
            var collider = go.AddComponent<BoxCollider2D>();
            collider.size = rect.size;
            return collider;
        }

        /// <summary>Adds a 1×1 tile centered at a world position, as a child of <paramref name="block"/>.</summary>
        public static GameObject AddTile(Transform block, Sprite sprite, Vector2 center, int sortingOrder)
        {
            var go = new GameObject(sprite != null ? sprite.name : "Tile");
            go.transform.SetParent(block, false);
            go.transform.position = center;
            GameArt.AddRenderer(go, sprite, sortingOrder);
            return go;
        }

        /// <summary>Picks the left, middle or right variant for column <paramref name="index"/> of <paramref name="count"/>.</summary>
        public static Sprite Edge(int index, int count, Sprite left, Sprite middle, Sprite right)
        {
            if (count > 1 && index == 0) return left;
            if (count > 1 && index == count - 1) return right;
            return middle;
        }
    }
}
