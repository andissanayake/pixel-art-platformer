using PixelAdventure.Cameras;
using PixelAdventure.Player;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class CameraPart : ILevelPart
    {
        public const string PartId = "Camera";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var go = LevelContext.CreateChild(parent, "Main Camera", new Vector3(0f, 0f, -10f));
            go.tag = "MainCamera";

            var camera = go.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = LevelSettings.CameraSize;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = LevelSettings.SkyColor;

            go.AddComponent<AudioListener>();
            go.AddComponent<CameraFollow2D>();
            BindToPlayer(context);
        }

        /// <summary>
        /// Points the existing camera at the current player and frames it immediately.
        /// The player is level content, so it is a different object after each level rebuild.
        /// </summary>
        public static void BindToPlayer(LevelContext context)
        {
            var follow = context.Find<CameraFollow2D>(PartId);
            if (follow == null) return;

            // A camera-only rebuild did not run the player part, so the context has no registration yet.
            var player = context.Player;
            if (player == null)
            {
                var marker = UnityEngine.Object.FindAnyObjectByType<PlayerMarker>();
                player = marker != null ? marker.transform : null;
            }

            follow.Configure(player, LevelSettings.CameraOffset, context.Layout.CameraBounds);
            follow.SnapToTarget();
        }
    }
}
