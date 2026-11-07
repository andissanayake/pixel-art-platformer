using PixelAdventure.Art;
using PixelAdventure.Enemies;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class EnemiesPart : ILevelPart
    {
        public const string PartId = "Enemies";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var size = LevelSettings.EnemySize;
            var frames = GameArt.Enemy;
            var patrols = context.Layout.Enemies;
            for (var i = 0; i < patrols.Length; i++)
            {
                var patrol = patrols[i];
                var start = new Vector3(patrol.MinX, patrol.SurfaceY + size.y / 2f, 0f);
                // Kinematic: moved by its script, not pushed around by the player or gravity.
                var actor = Actors.Create(
                    parent, $"Enemy {i + 1}", start, size, frames[0], RenderOrder.Enemy, RigidbodyType2D.Kinematic);
                actor.Renderer.gameObject.AddComponent<SpriteFlipbook>().Configure(frames, 16f);
                actor.Collider.isTrigger = true;
                actor.Root.AddComponent<PatrolEnemy>().Configure(patrol.MinX, patrol.MaxX, LevelSettings.EnemySpeed);
            }
        }
    }
}
