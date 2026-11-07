using PixelAdventure.Art;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    /// <summary>
    /// A character body: one collider, a rigidbody, and a sprite child whose feet sit on the bottom of the collider.
    /// Character art is pivoted at the feet.
    /// </summary>
    static class Actors
    {
        public readonly struct Body
        {
            public readonly GameObject Root;
            public readonly SpriteRenderer Renderer;
            public readonly Rigidbody2D Rigidbody;
            public readonly BoxCollider2D Collider;

            public Body(GameObject root, SpriteRenderer renderer, Rigidbody2D rigidbody, BoxCollider2D collider)
            {
                Root = root;
                Renderer = renderer;
                Rigidbody = rigidbody;
                Collider = collider;
            }
        }

        public static Body Create(
            Transform parent,
            string name,
            Vector3 position,
            Vector2 size,
            Sprite sprite,
            int sortingOrder,
            RigidbodyType2D bodyType)
        {
            var root = LevelContext.CreateChild(parent, name, position);
            var visual = LevelContext.CreateChild(root.transform, "Sprite", new Vector3(0f, -size.y / 2f, 0f));
            var renderer = GameArt.AddRenderer(visual, sprite, sortingOrder);

            var body = root.AddComponent<Rigidbody2D>();
            body.bodyType = bodyType;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            var collider = root.AddComponent<BoxCollider2D>();
            collider.size = size;

            return new Body(root, renderer, body, collider);
        }
    }
}
