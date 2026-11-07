using UnityEngine;

namespace PixelAdventure.Art
{
    /// <summary>
    /// Loops through sprite frames at a fixed rate. Used for coins, the flag and enemies.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteFlipbook : MonoBehaviour
    {
        [SerializeField] Sprite[] frames;
        [SerializeField] float framesPerSecond = 4f;

        SpriteRenderer m_Renderer;
        float m_TimeOffset;

        public void Configure(Sprite[] animationFrames, float fps)
        {
            frames = animationFrames;
            framesPerSecond = fps;
        }

        void Awake()
        {
            m_Renderer = GetComponent<SpriteRenderer>();
            // Start each object at a different point so neighbours don't animate in lockstep.
            m_TimeOffset = Mathf.Abs(transform.position.x) * 0.37f;
        }

        void Update()
        {
            if (frames == null || frames.Length == 0) return;
            var index = (int)((Time.time + m_TimeOffset) * framesPerSecond) % frames.Length;
            m_Renderer.sprite = frames[index];
        }
    }
}
