using UnityEngine;

namespace PixelAdventure.Level
{
    /// <summary>
    /// One recreatable piece of the level (camera, ground, player, coins, ...).
    /// <see cref="LevelBuilder"/> gives each part its own parent object named <see cref="Id"/> under
    /// <c>[Generated]</c>, and clearing the part destroys that object, so <see cref="Create"/> must put
    /// everything it makes under <c>parent</c>.
    /// </summary>
    public interface ILevelPart
    {
        string Id { get; }

        void Create(LevelContext context, Transform parent);

        /// <summary>
        /// Extra cleanup for state that lives outside the part's objects (statics, events).
        /// The objects themselves are destroyed by <see cref="LevelBuilder"/>.
        /// </summary>
        void Clear(LevelContext context) { }
    }
}
