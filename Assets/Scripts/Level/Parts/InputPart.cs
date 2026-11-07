using PixelAdventure.Game;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class InputPart : ILevelPart
    {
        public const string PartId = "Input";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var go = LevelContext.CreateChild(parent, "Game Input", Vector3.zero);
            go.AddComponent<GameInput>();
        }
    }
}
