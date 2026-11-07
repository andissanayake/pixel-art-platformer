using PixelAdventure.Game;
using PixelAdventure.Player;
using UnityEngine;

namespace PixelAdventure.Enemies
{
    /// <summary>
    /// Walks back and forth between two x positions. Touching the player ends the game.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PatrolEnemy : MonoBehaviour
    {
        [SerializeField] float minX;
        [SerializeField] float maxX;
        [SerializeField] float speed = 2f;

        Rigidbody2D m_Body;
        SpriteRenderer m_Renderer;
        int m_Direction = 1;

        public void Configure(float patrolMinX, float patrolMaxX, float patrolSpeed)
        {
            minX = patrolMinX;
            maxX = patrolMaxX;
            speed = patrolSpeed;
        }

        void Awake()
        {
            m_Body = GetComponent<Rigidbody2D>();
            m_Renderer = GetComponentInChildren<SpriteRenderer>();
        }

        void Update()
        {
            // The run cycle faces right. Flip it while walking left.
            if (m_Renderer != null)
                m_Renderer.flipX = m_Direction < 0;
        }

        void FixedUpdate()
        {
            var position = m_Body.position;
            position.x += m_Direction * speed * Time.fixedDeltaTime;

            if (position.x >= maxX)
            {
                position.x = maxX;
                m_Direction = -1;
            }
            else if (position.x <= minX)
            {
                position.x = minX;
                m_Direction = 1;
            }

            m_Body.MovePosition(position);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerMarker>(out _))
                GameEvents.RaisePlayerDied();
        }
    }
}
