using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PixelAdventure.Level.Parts
{
    public sealed class LightingPart : ILevelPart
    {
        public const string PartId = "Lighting";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var go = LevelContext.CreateChild(parent, "Global Light 2D", Vector3.zero);

            var light = go.AddComponent<Light2D>();
            light.lightType = Light2D.LightType.Global;
            light.color = Color.white;
            light.intensity = 1f;
        }
    }
}
