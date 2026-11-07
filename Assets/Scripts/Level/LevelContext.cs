using System;
using UnityEngine;

namespace PixelAdventure.Level
{
    /// <summary>
    /// Gives parts access to the <c>[Generated]</c> root, the current <see cref="LevelLayout"/>, and each other.
    /// Lookups go through the scene rather than cached references, so they stay valid when a single part is rebuilt.
    /// </summary>
    public sealed class LevelContext
    {
        public Transform Root { get; }
        public LevelLayout Layout { get; }

        /// <summary>Set by the player part while it is created. Null when this context did not build the player.</summary>
        public Transform Player { get; private set; }

        public LevelContext(Transform root, LevelLayout layout)
        {
            Root = root;
            Layout = layout ?? throw new ArgumentNullException(nameof(layout));
        }

        public void SetPlayer(Transform player) => Player = player;

        public Transform GetPart(string id) => Root.Find(id);

        public T Find<T>(string partId) where T : Component
        {
            var part = GetPart(partId);
            return part != null ? part.GetComponentInChildren<T>(true) : null;
        }

        public static GameObject CreateChild(Transform parent, string name, Vector3 localPosition)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            return go;
        }
    }
}
