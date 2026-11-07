using UnityEngine;

namespace PixelAdventure.Player
{
    /// <summary>
    /// Marks the object the level treats as the player. Coins, hazards, the goal and the camera
    /// look for this, not for <see cref="PlayerController"/>.
    /// </summary>
    public sealed class PlayerMarker : MonoBehaviour
    {
    }
}
