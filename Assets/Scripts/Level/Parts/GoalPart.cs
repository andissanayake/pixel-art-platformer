using PixelAdventure.Art;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class GoalPart : ILevelPart
    {
        public const string PartId = "Goal";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var layout = context.Layout;
            // Pivot is the base of the pole, so the flag stands on the ground.
            var go = LevelContext.CreateChild(parent, "Goal Flag", new Vector3(layout.GoalX, layout.GroundTop, 0f));

            var frames = GameArt.Flag;
            GameArt.AddRenderer(go, frames[0], RenderOrder.Goal);
            go.AddComponent<SpriteFlipbook>().Configure(frames, 12f);

            // The checkered flag hangs to the right of the pole.
            var trigger = go.AddComponent<BoxCollider2D>();
            trigger.isTrigger = true;
            trigger.offset = new Vector2(0.7f, 1.5f);
            trigger.size = new Vector2(2f, 3f);
            go.AddComponent<Goal>();
        }
    }
}
