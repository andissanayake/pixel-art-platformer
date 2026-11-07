using UnityEngine;

namespace PixelAdventure.Player
{
    /// <summary>
    /// Idle and run loops on the ground. Jump while rising, fall while coming down.
    /// Faces the direction of movement.
    /// </summary>
    public sealed class PlayerAnimator : MonoBehaviour
    {
        const float MovingSpeed = 0.1f;

        [SerializeField] SpriteRenderer target;
        [SerializeField] Sprite[] idleFrames;
        [SerializeField] Sprite[] runFrames;
        [SerializeField] Sprite jumpFrame;
        [SerializeField] Sprite fallFrame;
        [SerializeField] float idleFramesPerSecond = 12f;
        [SerializeField] float walkFramesPerSecond = 16f;
        [SerializeField] float runFramesPerSecond = 20f;

        PlayerController m_Controller;
        Rigidbody2D m_Body;

        public void Configure(
            SpriteRenderer spriteRenderer,
            Sprite[] idle,
            Sprite[] run,
            Sprite jump,
            Sprite fall)
        {
            target = spriteRenderer;
            idleFrames = idle;
            runFrames = run;
            jumpFrame = jump;
            fallFrame = fall;
        }

        void Awake()
        {
            m_Controller = GetComponent<PlayerController>();
            m_Body = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (target == null || idleFrames == null || idleFrames.Length == 0) return;

            var velocity = m_Body.linearVelocity;
            if (Mathf.Abs(velocity.x) > MovingSpeed)
                target.flipX = velocity.x < 0f;

            if (!m_Controller.IsGrounded)
            {
                target.sprite = velocity.y > 0f ? jumpFrame : fallFrame;
                return;
            }

            if (Mathf.Abs(velocity.x) > MovingSpeed && runFrames != null && runFrames.Length > 0)
            {
                var fps = m_Controller.IsRunning ? runFramesPerSecond : walkFramesPerSecond;
                target.sprite = runFrames[(int)(Time.time * fps) % runFrames.Length];
                return;
            }

            target.sprite = idleFrames[(int)(Time.time * idleFramesPerSecond) % idleFrames.Length];
        }
    }
}
