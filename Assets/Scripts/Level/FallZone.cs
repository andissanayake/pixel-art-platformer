using PixelAdventure.Game;
using PixelAdventure.Player;
using UnityEngine;

namespace PixelAdventure.Level
{
    /// <summary>
    /// Trigger below the level: the player falling into it ends the game.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public sealed class FallZone : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerMarker>(out _))
                GameEvents.RaisePlayerDied();
        }
    }
}
