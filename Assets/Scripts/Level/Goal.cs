using PixelAdventure.Game;
using PixelAdventure.Player;
using UnityEngine;

namespace PixelAdventure.Level
{
    /// <summary>
    /// The finish flag: the player touching it completes the level.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public sealed class Goal : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerMarker>(out _))
                GameEvents.RaiseLevelCompleted();
        }
    }
}
