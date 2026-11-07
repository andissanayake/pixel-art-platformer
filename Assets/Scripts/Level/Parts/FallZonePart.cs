using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class FallZonePart : ILevelPart
    {
        public const string PartId = "FallZone";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            // Spans well past both level edges so nothing can fall around it.
            var layout = context.Layout;
            const float margin = 20f;
            var rect = Rect.MinMaxRect(
                layout.MinX - margin, layout.GroundBottom - LevelLayout.FallZoneDepth - 2f,
                layout.MaxX + margin, layout.GroundBottom - LevelLayout.FallZoneDepth);

            var go = LevelContext.CreateChild(parent, "Fall Zone", rect.center);
            var collider = go.AddComponent<BoxCollider2D>();
            collider.size = rect.size;
            collider.isTrigger = true;
            go.AddComponent<FallZone>();
        }
    }
}
