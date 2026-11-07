using PixelAdventure.Art;
using PixelAdventure.Player;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class PlayerPart : ILevelPart
    {
        public const string PartId = "Player";
        public string Id => PartId;

        static PhysicsMaterial2D s_NoFriction;

        public void Create(LevelContext context, Transform parent)
        {
            var actor = Actors.Create(
                parent, "Player", context.Layout.PlayerSpawn, LevelSettings.PlayerSize,
                GameArt.PlayerIdle[0], RenderOrder.Player, RigidbodyType2D.Dynamic);

            var body = actor.Rigidbody;
            body.gravityScale = LevelSettings.PlayerGravityScale;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // Zero friction so the player slides down walls instead of sticking to them while pushing sideways.
            actor.Collider.sharedMaterial = NoFriction;

            var go = actor.Root;
            go.AddComponent<PlayerMarker>();
            go.AddComponent<PlayerController>();
            go.AddComponent<PlayerAnimator>().Configure(
                actor.Renderer, GameArt.PlayerIdle, GameArt.PlayerRun, GameArt.PlayerJump, GameArt.PlayerFall);
            context.SetPlayer(go.transform);
        }

        static PhysicsMaterial2D NoFriction
        {
            get
            {
                if (s_NoFriction == null)
                {
                    s_NoFriction = new PhysicsMaterial2D("PlayerNoFriction")
                    {
                        friction = 0f,
                        bounciness = 0f,
                        hideFlags = HideFlags.HideAndDontSave,
                    };
                }
                return s_NoFriction;
            }
        }
    }
}
