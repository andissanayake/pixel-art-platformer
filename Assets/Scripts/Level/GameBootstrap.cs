using UnityEngine;

namespace PixelAdventure.Level
{
    public static class GameBootstrap
    {
        public const string BuildOnPlayKey = "PixelAdventure.LevelBuilder.BuildOnPlay";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnFirstSceneLoaded()
        {
#if UNITY_EDITOR
            // In the Editor, Play mode builds the level when enabled in the Level Builder window, or when a
            // preview exists: entering Play mode drops the preview's generated sprites (they are never saved),
            // so the preview has to be recreated to be visible. Player builds always build, the scene is empty.
            if (!UnityEditor.EditorPrefs.GetBool(BuildOnPlayKey, false) && !LevelBuilder.IsBuilt) return;
#endif
            LevelBuilder.Rebuild();
        }
    }
}
