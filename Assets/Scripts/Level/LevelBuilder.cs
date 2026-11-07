using System;
using System.Collections.Generic;
using PixelAdventure.Game;
using PixelAdventure.Level.Parts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace PixelAdventure.Level
{
    /// <summary>
    /// Creates and clears the level from code. Everything lives under the <see cref="RootName"/> object,
    /// one child per <see cref="ILevelPart"/>; nothing outside it is touched.
    /// Session parts stay for the whole play session. Level parts are replaced by <see cref="RebuildLevel"/>.
    /// </summary>
    public static class LevelBuilder
    {
        public const string RootName = "[Generated]";

        // Session first, then level content, so a level rebuild can leave the session objects in place.
        // Within each list, later parts may look up earlier ones through LevelContext.
        static readonly ILevelPart[] s_SessionParts =
        {
            new InputPart(),
            new LightingPart(),
            new CameraPart(),
            new HudPart(),
            new AudioPart(),
        };

        static readonly ILevelPart[] s_LevelParts =
        {
            new GroundPart(),
            new PlatformsPart(),
            new FallZonePart(),
            new CoinsPart(),
            new EnemiesPart(),
            new GoalPart(),
            new PlayerPart(),
        };

        public static IReadOnlyList<ILevelPart> SessionParts => s_SessionParts;
        public static IReadOnlyList<ILevelPart> LevelParts => s_LevelParts;

        static LevelLayout s_Layout = LevelLayout.Level1;

        public static LevelLayout CurrentLayout => s_Layout;

        public static bool IsBuilt => FindRoot(SceneManager.GetActiveScene()) != null;

        public static bool HasPart(string id)
        {
            var root = FindRoot(SceneManager.GetActiveScene());
            return root != null && root.Find(id) != null;
        }

        public static void Rebuild() => Rebuild(s_Layout);

        public static void Rebuild(LevelLayout layout)
        {
            Use(layout);
            Clear();
            GameEvents.BeginRound();
            var context = ContextFor(GetOrCreateRoot());
            CreateParts(s_SessionParts, context);
            CreateParts(s_LevelParts, context);
            BindSession(context);
        }

        /// <summary>
        /// Recreates the current layout's ground, coins, enemies, player and other level parts.
        /// Session parts are created if they are missing and otherwise left alone.
        /// Play-mode restart goes through <see cref="PixelAdventure.Game.GameSession.RestartLevel"/>, which also resets the HUD and audio.
        /// </summary>
        public static void RebuildLevel() => RebuildLevel(s_Layout);

        public static void RebuildLevel(LevelLayout layout)
        {
            Use(layout);
            GameEvents.BeginRound();
            var context = ContextFor(GetOrCreateRoot());
            EnsureParts(s_SessionParts, context);
            ClearParts(s_LevelParts, context);
            CreateParts(s_LevelParts, context);
            BindSession(context);
        }

        public static void Rebuild(string id)
        {
            var part = GetPartById(id);
            var context = ContextFor(GetOrCreateRoot());
            ClearPart(part, context);
            CreatePart(part, context);
            if (part.Id == PlayerPart.PartId || part.Id == CameraPart.PartId)
                CameraPart.BindToPlayer(context);
            if (part.Id == HudPart.PartId)
                HudPart.ApplyLayout(context);
        }

        public static void Clear() => Clear(SceneManager.GetActiveScene());

        public static void Clear(Scene scene)
        {
            var root = FindRoot(scene);
            if (root == null) return;

            var context = ContextFor(root);
            ClearParts(s_LevelParts, context);
            ClearParts(s_SessionParts, context);

            // DestroyImmediate (also in Play mode) so a rebuild in the same frame never sees old objects,
            // e.g. through Camera.main.
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.name == RootName)
                    Object.DestroyImmediate(go);
            }
        }

        public static void Clear(string id)
        {
            var root = FindRoot(SceneManager.GetActiveScene());
            if (root == null) return;
            ClearPart(GetPartById(id), ContextFor(root));
        }

        static void Use(LevelLayout layout) => s_Layout = layout ?? throw new ArgumentNullException(nameof(layout));

        static LevelContext ContextFor(Transform root) => new LevelContext(root, s_Layout);

        static void BindSession(LevelContext context)
        {
            CameraPart.BindToPlayer(context);
            HudPart.ApplyLayout(context);
        }

        static void CreateParts(ILevelPart[] parts, LevelContext context)
        {
            foreach (var part in parts)
                CreatePart(part, context);
        }

        static void EnsureParts(ILevelPart[] parts, LevelContext context)
        {
            foreach (var part in parts)
            {
                if (context.GetPart(part.Id) == null)
                    CreatePart(part, context);
            }
        }

        static void ClearParts(ILevelPart[] parts, LevelContext context)
        {
            for (var i = parts.Length - 1; i >= 0; i--)
                ClearPart(parts[i], context);
        }

        static void CreatePart(ILevelPart part, LevelContext context)
        {
            var parent = new GameObject(part.Id).transform;
            parent.SetParent(context.Root, false);
            parent.SetSiblingIndex(SiblingIndexFor(part, context.Root));
            part.Create(context, parent);
        }

        static void ClearPart(ILevelPart part, LevelContext context)
        {
            part.Clear(context);
            var parent = context.GetPart(part.Id);
            if (parent != null)
                Object.DestroyImmediate(parent.gameObject);
        }

        static int SiblingIndexFor(ILevelPart part, Transform root)
        {
            var index = 0;
            foreach (var other in s_SessionParts)
            {
                if (other == part) return index;
                if (root.Find(other.Id) != null) index++;
            }

            foreach (var other in s_LevelParts)
            {
                if (other == part) return index;
                if (root.Find(other.Id) != null) index++;
            }

            return index;
        }

        static ILevelPart GetPartById(string id)
        {
            var part = FindPart(s_SessionParts, id) ?? FindPart(s_LevelParts, id);
            if (part == null)
                throw new ArgumentException($"No level part with id '{id}'", nameof(id));
            return part;
        }

        static ILevelPart FindPart(ILevelPart[] parts, string id)
        {
            foreach (var part in parts)
            {
                if (part.Id == id) return part;
            }

            return null;
        }

        static Transform FindRoot(Scene scene)
        {
            if (!scene.isLoaded) return null;
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.name == RootName) return go.transform;
            }
            return null;
        }

        static Transform GetOrCreateRoot()
        {
            var scene = SceneManager.GetActiveScene();
            var root = FindRoot(scene);
            if (root != null) return root;

            var go = new GameObject(RootName);
            SceneManager.MoveGameObjectToScene(go, scene);
            return go.transform;
        }
    }
}
