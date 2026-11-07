using PixelAdventure.Game;
using PixelAdventure.Player;
using UnityEngine;

namespace PixelAdventure.Collectibles
{
    /// <summary>
    /// Collected when the player touches it. Raises <see cref="GameEvents.CoinCollected"/>,
    /// so the score and the sound player don't need references to the coin.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public sealed class Coin : MonoBehaviour
    {
        [SerializeField] int value = 10;
        [SerializeField] float bobHeight = 0.1f;
        [SerializeField] float bobSpeed = 3f;

        Vector3 m_StartPosition;
        float m_BobPhase;
        bool m_Collected;

        void Awake()
        {
            m_StartPosition = transform.localPosition;
            // Offset by position so neighbouring coins don't bob in sync.
            m_BobPhase = m_StartPosition.x;
        }

        void Update()
        {
            var bob = Mathf.Sin(Time.time * bobSpeed + m_BobPhase) * bobHeight;
            transform.localPosition = m_StartPosition + Vector3.up * bob;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (m_Collected || !other.TryGetComponent<PlayerMarker>(out _)) return;

            m_Collected = true;
            GameEvents.RaiseCoinCollected(value);
            Destroy(gameObject);
        }
    }
}
