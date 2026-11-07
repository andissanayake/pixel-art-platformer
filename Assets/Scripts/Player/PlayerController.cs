using PixelAdventure.Game;
using UnityEngine;

namespace PixelAdventure.Player
{
    /// <summary>
    /// Walk, run and jump with coyote time, jump buffering and variable jump height.
    /// Reads <see cref="GameInput"/>; it does not own the bindings.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] float walkSpeed = 5f;
        [SerializeField] float runSpeed = 8f;
        [SerializeField] float jumpVelocity = 14f;
        [Tooltip("Seconds after leaving the ground during which a jump still works.")]
        [SerializeField] float coyoteTime = 0.1f;
        [Tooltip("Seconds a jump press is remembered before landing.")]
        [SerializeField] float jumpBufferTime = 0.1f;
        [Tooltip("Upward speed is multiplied by this when jump is released early, for shorter hops.")]
        [SerializeField, Range(0f, 1f)] float jumpCutMultiplier = 0.5f;

        Rigidbody2D m_Body;
        ContactFilter2D m_GroundFilter;

        float m_MoveInput;
        bool m_JumpReleased;
        float m_LastGroundedTime = float.NegativeInfinity;
        float m_LastJumpPressedTime = float.NegativeInfinity;

        public bool IsGrounded { get; private set; }
        public bool IsRunning => GameInput.Instance != null && GameInput.Instance.Run.IsPressed();

        void Awake()
        {
            m_Body = GetComponent<Rigidbody2D>();

            // Only contacts whose surface faces up count as ground, so walls and ceilings don't.
            m_GroundFilter = new ContactFilter2D { useTriggers = false };
            m_GroundFilter.SetNormalAngle(45f, 135f);
        }

        void Update()
        {
            var input = GameInput.Instance;
            if (input == null)
            {
                m_MoveInput = 0f;
                return;
            }

            m_MoveInput = input.Move.ReadValue<float>();
            if (input.Jump.WasPressedThisFrame()) m_LastJumpPressedTime = Time.time;
            if (input.Jump.WasReleasedThisFrame()) m_JumpReleased = true;
        }

        void FixedUpdate()
        {
            IsGrounded = m_Body.IsTouching(m_GroundFilter);
            if (IsGrounded) m_LastGroundedTime = Time.time;

            var velocity = m_Body.linearVelocity;
            velocity.x = m_MoveInput * (IsRunning ? runSpeed : walkSpeed);

            var canJump = Time.time - m_LastGroundedTime <= coyoteTime;
            var wantsJump = Time.time - m_LastJumpPressedTime <= jumpBufferTime;
            if (canJump && wantsJump)
            {
                velocity.y = jumpVelocity;
                m_LastJumpPressedTime = float.NegativeInfinity;
                m_LastGroundedTime = float.NegativeInfinity;
                GameEvents.RaiseJumped();
            }

            if (m_JumpReleased)
            {
                if (velocity.y > 0f) velocity.y *= jumpCutMultiplier;
                m_JumpReleased = false;
            }

            m_Body.linearVelocity = velocity;
        }
    }
}
