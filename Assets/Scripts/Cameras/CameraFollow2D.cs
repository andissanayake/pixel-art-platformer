using PixelAdventure.Player;
using UnityEngine;

namespace PixelAdventure.Cameras
{
    /// <summary>
    /// Smoothly follows a target and keeps the view inside <see cref="Bounds"/>, so the camera never shows
    /// past the level edges.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public sealed class CameraFollow2D : MonoBehaviour
    {
        [SerializeField] Transform target;
        [Tooltip("Added to the target position, e.g. to keep the player in the lower part of the screen.")]
        [SerializeField] Vector2 offset;
        [SerializeField] float smoothTime = 0.15f;
        [Tooltip("If the target moves farther than this at once (e.g. a teleport), jump there instead of panning.")]
        [SerializeField] float snapDistance = 10f;
        [Tooltip("World-space area the camera view must stay inside.")]
        [SerializeField] Rect bounds;

        Camera m_Camera;
        Vector3 m_Velocity;

        public Rect Bounds
        {
            get => bounds;
            set => bounds = value;
        }

        public void Configure(Transform followTarget, Vector2 targetOffset, Rect viewBounds)
        {
            target = followTarget;
            offset = targetOffset;
            bounds = viewBounds;
        }

        public void SnapToTarget()
        {
            if (target == null) return;
            transform.position = DesiredPosition();
            m_Velocity = Vector3.zero;
        }

        void Awake()
        {
            m_Camera = GetComponent<Camera>();
        }

        void LateUpdate()
        {
            // The player can be rebuilt on its own, which destroys the object this camera was following.
            if (target == null)
            {
                var player = FindAnyObjectByType<PlayerMarker>();
                if (player == null) return;
                target = player.transform;
                SnapToTarget();
            }

            var desired = DesiredPosition();
            if (Vector2.Distance(transform.position, desired) > snapDistance)
            {
                transform.position = desired;
                m_Velocity = Vector3.zero;
                return;
            }

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref m_Velocity, smoothTime);
        }

        Vector3 DesiredPosition()
        {
            if (m_Camera == null) m_Camera = GetComponent<Camera>();

            var desired = (Vector2)target.position + offset;
            var halfHeight = m_Camera.orthographicSize;
            var halfWidth = halfHeight * m_Camera.aspect;

            return new Vector3(
                ClampAxis(desired.x, bounds.xMin + halfWidth, bounds.xMax - halfWidth),
                ClampAxis(desired.y, bounds.yMin + halfHeight, bounds.yMax - halfHeight),
                transform.position.z);
        }

        // If the level is smaller than the view on this axis, center on it instead of clamping.
        static float ClampAxis(float value, float min, float max) =>
            min > max ? (min + max) / 2f : Mathf.Clamp(value, min, max);
    }
}
